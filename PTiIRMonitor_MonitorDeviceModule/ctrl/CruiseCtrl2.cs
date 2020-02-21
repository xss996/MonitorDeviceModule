using MySql.Data.MySqlClient;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.entities;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
   public  class CruiseCtrl2
    {
        public Constant.CruiseState cruiseStatus = Constant.CruiseState.STOP;
        public bool CruiseSwtich { get; set; }      
        public DateTime LastCruiseTime { get; set; }

        public bool CheckTimeIsNeedCruise()
        {
            bool result = false;
            CruiseObj cruiseObj = new CruiseObj();
            bool flag= GetCruiseInfo(out cruiseObj);

            if (!flag)
                return false;

            DateTime currentTime = DateTime.Now;
            if (cruiseObj.CruiseType == 0)
            {
                TimeSpan ts = currentTime - cruiseObj.LastCruiseTime;
                if(ts.TotalSeconds  >=cruiseObj .CruiseTime*60)
                {
                    Debug.WriteLine(string.Format("系统执行隔时巡检,开始巡检时间:{0},当前系统时间:{1}", cruiseObj.LastCruiseTime, currentTime));

                    result = true;
                }
               
                else
                {
                    result = false;
                }
            }
            else if (cruiseObj.CruiseType == 1)
            {
                for (var i = 0; i < cruiseObj.dateTimeList.Count; i++)
                {
                    if (DateUtil.GetSumMinutes(currentTime) - DateUtil.GetSumMinutes(cruiseObj.dateTimeList[i]) <= cruiseObj.CruiseTime)
                    {
                        Debug.WriteLine(string.Format("系统执行定时巡检,开始巡检时间:{0},当前系统时间:{1}", cruiseObj.dateTimeList[i], currentTime));                      
                        result = true;
                        break;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }      
            return result;
        }

        public void CruiseExcute(IRCtrl irCtrl,TVCtrl tvCtrl)
        {
            Debug.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^系统执行巡检相关操作");

            //查询任务列表
            if(GetCruiseTasks(1, out List<int> mlstintTastTable))
            {
                //for循环执行每个任务点
                for (int i = 0; i < mlstintTastTable.Count; i++)
                {
                    if (CruiseSwtich == false)
                        break;                  
                    actionOneTaskCruise(mlstintTastTable[i],irCtrl,tvCtrl);
                }
            }       
        }

        public bool GetCruiseInfo(out CruiseObj cruiseObj)
        {
             cruiseObj = new CruiseObj();
            //   cruiseObj.StartTime = startCruiseTime;
            cruiseObj.CruiseTime = 3;
           // cruiseObj.Interval = 5;
            cruiseObj.CruiseType = 0;

            //  cruiseObj.CruiseType = 1;
            DateTime date1 = DateTime.Parse("2019-12-02 15:50:58");
            DateTime date2 = DateTime.Parse("2019-12-02 15:53:58");
            DateTime date3 = DateTime.Parse("2019-12-02 15:56:58");
            List<DateTime> dateTimes = new List<DateTime>();
            dateTimes.Add(date1);
            dateTimes.Add(date2);
            dateTimes.Add(date3);
            cruiseObj.dateTimeList = dateTimes;
            return true;
        }

        public bool GetCruiseTasks(int intPosIndex,out List<int> mlstintTastTable)
        {
            //返回任务点的ID列表
            string ip = INIUtil.Read("DATABASE", "ip", Constant.IniFilePath);
            string strPort = INIUtil.Read("DATABASE", "port", Constant.IniFilePath);
            string username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
            string password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
            string databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);
            MySqlConnection conn = SqlHelper.GetConnection(ip, Convert.ToInt32(strPort), username, password, databaseName);
            string strSql = "select * from cruise_measuretaskset";
            DataTable datas = SqlHelper.QueryData(conn, strSql, null);

            mlstintTastTable = new List<int>();
            for (int i = 0; i < datas.Rows.Count; i++)
            {
                mlstintTastTable.Add(Convert.ToInt32(datas.Rows[i]["MeasureTask_Index"]));
            }
            return true;
        }

        public void actionOneTaskCruise(int taskId,IRCtrl irCtrl,TVCtrl tvCtrl)
        {
            //Check  PrePosImf           
            MySqlConnection conn = GlobalCtrl.GetSqlConnection();
            string strSql = "SELECT m.MeasureTask_Index,m.PrePosSet_Index,m.DeviceInfo_Index,m.MeaType,m.TaskName,m.GetValueType," +
            "p.Position_Index,p.PrePositionNO,p.PrePositionName,p.PrePosType,p.TVZoom,p.IRFocus,p.GoPrePosDelays,p.PanAngle,p.TiltAngle,p.PTZHorAngle,p.PTZVerAngle,p.PaletteType  " +
            "from cruise_measuretaskset m, cruise_prepositionset p where m.PrePosSet_Index = p.PrePosSet_Index and p.EnbleMeasure = 1 and m.MeasureTask_Index = @taskId";

            List<MySqlParameter> parmList = new List<MySqlParameter>();
            parmList.Add(new MySqlParameter("@taskId", taskId));

            DataTable datas = SqlHelper.QueryData(conn, strSql,parmList.ToArray());   //根据巡检任务id获取对应的预置位相关的信息
            if (datas.Rows.Count != 0)
            {
                for (var i = 0; i < datas.Rows.Count; i++)
                {
                    //check IR Camera Seting
                    irCtrl.SetFocusPos(Convert.ToInt32(datas.Rows[i]["IRFocus"]));
                    //Goto PrePos;
                    tvCtrl.InvokePrePos(Convert.ToInt32(datas.Rows[i]["PrePosSet_Index"]));
                    //Sleep Time wait for Pantilt to Position
                    Thread.Sleep(3000);
                    //Autofocus Or Manual Focus;
                    irCtrl.SetAutoFocus();
                    //sleep time wait for Auto Focus Finish
                    Thread.Sleep(3000);
                    //Snap Picture;     
                    string IRImagefileName = "";
                    string TVImagefileName = "";
                    CruiseSnapPictures(taskId, datas.Rows[i]["Position_Index"].ToString(), datas.Rows[i]["PrePositionName"].ToString(), irCtrl, tvCtrl,out IRImagefileName,out TVImagefileName);
                    //sleep wiait for FIle save finish
                    Thread.Sleep(3000);
                    //Analyse Hot Picture;                     
                    switch (Convert.ToInt32(datas.Rows[i]["MeaType"]))
                    {                       
                        case 0:   //点温
                            Cruise_SpotAnalyseHotPic(taskId,irCtrl,tvCtrl, IRImagefileName);                                                    
                            break;
                        case 1:   //线温
                            Cruise_LineAnalyseHotPic(taskId, Convert.ToInt32(datas.Rows[i]["GetValueType"]), irCtrl, tvCtrl);                                                     
                            break;
                        case 2:  //区域测温
                            Cruise_AreaAnalyseHotPic(taskId, Convert.ToInt32(datas.Rows[i]["GetValueType"]), irCtrl, tvCtrl);
                            break;
                        case 3:  //多边形测温
                            Cruise_PolygonAnalyseHotPic(taskId, Convert.ToInt32(datas.Rows[i]["GetValueType"]), irCtrl, tvCtrl);
                            break;
                        case 4:   //圆形测温
                            Cruise_CircleAnalyseHotPic(taskId, Convert.ToInt32(datas.Rows[i]["GetValueType"]), irCtrl, tvCtrl);
                            break;                                           
                    }
                    //Create Temptrue Record

                    //Analyse AlarmRecord;

                    //Upload FTP File

                    //check File 
                }
            }
        }

        private void CruiseSnapPictures(int taskId,string positionIndex,string PositionName,IRCtrl irCtrl,TVCtrl tvCtrl,out string IRImagefileName,out string TVImagefileName)
        {
             IRImagefileName = Constant.imageSavePath + "IR_" + positionIndex + "_" + taskId + "_" + PositionName + "_" + DateUtil.DateToString(DateTime.Now) + ".jpg";
             TVImagefileName = Constant.imageSavePath + "TV_" + positionIndex + "_" + taskId + "_" + PositionName + "_" + DateUtil.DateToString(DateTime.Now);
            if (!irCtrl.SaveIRHotImage(IRImagefileName))
            {
                IRImagefileName = null;
            }
            if (!tvCtrl.SaveImage(TVImagefileName))
            {
                TVImagefileName = null;
            }     
        }

        private void Cruise_SpotAnalyseHotPic(int taskId,IRCtrl irCtrl,TVCtrl tvCtrl,string IRImagefileName)
        {
            MySqlConnection conn = GlobalCtrl.GetSqlConnection();
            double temperature = 0;
            string strSql = "select * from cruise_spotset where MeasureTask_Index = @taskId";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@taskId", taskId));
            DataTable spotDatas = SqlHelper.QueryData(conn, strSql, parameters.ToArray());
            if (spotDatas.Rows.Count != 0)
            {
                for (var i = 0; i < spotDatas.Rows.Count; i++)
                {
                    
                }
            }
        }

        private void Cruise_LineAnalyseHotPic(int taskId,int tempValueType, IRCtrl irCtrl, TVCtrl tvCtrl)
        {
            MySqlConnection conn = GlobalCtrl.GetSqlConnection();
            string strSql = "select * from cruise_lineset where MeasureTask_Index = @taskId";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@taskId", taskId));
            DataTable lineDatas = SqlHelper.QueryData(conn, strSql, parameters.ToArray());
            if (lineDatas.Rows.Count != 0)
            {
                for (var i = 0; i < lineDatas.Rows.Count; i++)
                {
                    switch (tempValueType)
                    {
                        case 0: //最大值
                            break;
                        case 1:  //平均值
                            break;
                        case 2:  //最小值
                            break;
                        case 4:   //差值
                            break;
                    }
                }
            }
        }
        private void Cruise_AreaAnalyseHotPic(int taskId, int tempValueType, IRCtrl irCtrl, TVCtrl tvCtrl)
        {
            switch (tempValueType)
            {
                case 0: //最大值
                    break;
                case 1:  //平均值
                    break;
                case 2:  //最小值
                    break;
                case 4:   //差值
                    break;
            }
        }
        private void Cruise_PolygonAnalyseHotPic(int taskId, int tempValueType, IRCtrl irCtrl, TVCtrl tvCtrl)
        {
            switch (tempValueType)
            {
                case 0: //最大值
                    break;
                case 1:  //平均值
                    break;
                case 2:  //最小值
                    break;
                case 4:   //差值
                    break;
            }
        }
        private void Cruise_CircleAnalyseHotPic(int taskId, int tempValueType, IRCtrl irCtrl, TVCtrl tvCtrl)
        {
            switch (tempValueType)
            {
                case 0: //最大值
                    break;
                case 1:  //平均值
                    break;
                case 2:  //最小值
                    break;
                case 4:   //差值
                    break;
            }
        }
        private bool AddSpotMeaTempRecords(int taskId,double temperature)
        {
            MySqlConnection conn = GlobalCtrl.GetSqlConnection();
            string strSql = "insert into record_tempmeasure (MeasureTask_Index,MonDev_Index,MeasureValueMin,MeasureValueAvg,MeasureValueMax,AirTemperature,Humidity,Electricty,RecordTime," +
                                        "TVFilePath,IRFilePath,TVVImgPath,IRVImgPath,IRHImgPath,Readed) values(@MeasureTask_Index,@MonDev_Index,@MeasureValueMin,@MeasureValueAvg,@MeasureValueMax,@AirTemperature," +
                                        "@Humidity,@Electricty,@RecordTime,@TVFilePath,@IRFilePath,@TVVImgPath,@IRVImgPath,@IRHImgPath,@Readed)";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@MeasureTask_Index", taskId));
            parameters.Add(new MySqlParameter("@MonDev_Index", 0));
            parameters.Add(new MySqlParameter("@MeasureValueMin", temperature));
            parameters.Add(new MySqlParameter("@MeasureValueAvg", temperature));
            parameters.Add(new MySqlParameter("@MeasureValueMax", temperature));
            parameters.Add(new MySqlParameter("@AirTemperature", 0));
            parameters.Add(new MySqlParameter("@Humidity", 0));
            parameters.Add(new MySqlParameter("@Electricty", 0));
            parameters.Add(new MySqlParameter("@RecordTime", DateTime.Now));
            parameters.Add(new MySqlParameter("@TVFilePath", null));
            parameters.Add(new MySqlParameter("@IRFilePath", null));
            parameters.Add(new MySqlParameter("@TVVImgPath", null));
            parameters.Add(new MySqlParameter("@IRVImgPath", null));
            parameters.Add(new MySqlParameter("@IRHImgPath", null));
            parameters.Add(new MySqlParameter("@Readed", false));

           return  SqlHelper.AddData(conn, strSql, parameters.ToArray());
        }
    }
}
