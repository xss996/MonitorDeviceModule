
using MySql.Data.MySqlClient;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.dao;
using PTiIRMonitor_MonitorDeviceModule.entities;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;

namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
    public class CruiseCtrl
    {
        public Constant.CruiseState cruiseStatus = Constant.CruiseState.STOP;
        public bool CruiseSwtich { get; set; }
        public bool isFirstCruise { get; set; }
        public DateTime LastCruiseTime { get; set; }

         public List<String> cruiseLogMsgs = new List<string>();
         MeasureTaskSetDao taskSetDao = new MeasureTaskSetDao();
        PrePositionSetDao prePosSetDao = new PrePositionSetDao();
        TempMeasureDao tempMeasureDao = new TempMeasureDao();
        AlarmRecordDao alarmRecordDao = new AlarmRecordDao();
        public bool CheckTimeIsNeedCruise()
        {
            bool result = false;
            CruiseObj cruiseObj =GetCruiseInfo();

            DateTime currentTime = DateTime.Now;
            if (cruiseObj.CruiseType == 0)    //隔时巡检
            {
                TimeSpan ts = currentTime - cruiseObj.LastCruiseTime;
                if (ts.TotalSeconds >= cruiseObj.CruiseTime*60 || isFirstCruise)
                {
                    gLogWriter.WriteLog("系统执行隔时巡检",string.Format("上次巡检结束时间:{0},当前系统时间:{1}", cruiseObj.LastCruiseTime, currentTime));
                    cruiseLogMsgs.Add(string.Format("系统执行隔时巡检,上次巡检结束时间:{0},当前系统时间:{1}", cruiseObj.LastCruiseTime, currentTime));
                    result = true;
                }
                else
                {
                    gLogWriter.WriteLog("巡检空闲", string.Format("当前系统时间:{0}",currentTime));
                    cruiseLogMsgs.Add(string.Format("巡检空闲,当前系统时间:{0}", currentTime));
                    result = false;
                }
            }
            else if (cruiseObj.CruiseType == 1)   //定时巡检
            {
                for (var i = 0; i < cruiseObj.dateTimeList.Count; i++)
                {
                    TimeSpan ts = currentTime - cruiseObj.dateTimeList[i];
                    if (ts.TotalSeconds >cruiseObj.CruiseTime*60 || isFirstCruise)
                    {
                        gLogWriter.WriteLog("系统执行定时巡检", string.Format("开始巡检时间:{0},当前系统时间: {1}",cruiseObj.dateTimeList[i],currentTime ));
                        cruiseLogMsgs.Add(string.Format("系统执行定时巡检,开始巡检时间:{0},当前系统时间: {1}", cruiseObj.dateTimeList[i], currentTime));
                        result = true;
                        break;
                    }
                    else
                    {
                        gLogWriter.WriteLog("系统空闲", string.Format("当前系统时间:{0}", currentTime));
                        cruiseLogMsgs.Add(string.Format("巡检空闲,当前系统时间:{0}", currentTime));
                        result = false;
                    }
                }
            }
            return result;
        }

        //public bool GetCruiseInfo(out CruiseObj cruiseObj )
        //{
        //    cruiseObj = new CruiseObj();
        //    cruiseObj.LastCruiseTime = LastCruiseTime;
        //    cruiseObj.CruiseTime = 3;
        //    cruiseObj.Interval = 5;
        //    cruiseObj.CruiseType = 0;

        //    //  cruiseObj.CruiseType = 1;
        //    DateTime date1 = DateTime.Parse("2019-12-02 15:50:58");
        //    DateTime date2 = DateTime.Parse("2019-12-02 15:53:58");
        //    DateTime date3 = DateTime.Parse("2019-12-02 15:56:58");
        //    List<DateTime> dateTimes = new List<DateTime>();
        //    dateTimes.Add(date1);
        //    dateTimes.Add(date2);
        //    dateTimes.Add(date3);
        //    cruiseObj.dateTimeList = dateTimes;
        //    return true;
        //}
        public CruiseObj GetCruiseInfo()
        {
            CruiseObj cruiseObj = new CruiseObj();
            cruiseObj.LastCruiseTime = LastCruiseTime;
            cruiseObj.CruiseTime = 5;
            cruiseObj.CruiseType = 0;
          
            //  cruiseObj.CruiseType = 1;
            //DateTime date1 = DateTime.Parse("2019-12-02 15:50:58");
            //DateTime date2 = DateTime.Parse("2019-12-02 15:53:58");
            //DateTime date3 = DateTime.Parse("2019-12-02 15:56:58");
            //List<DateTime> dateTimes = new List<DateTime>();
            //dateTimes.Add(date1);
            //dateTimes.Add(date2);
            //dateTimes.Add(date3);
            //cruiseObj.dateTimeList = dateTimes;
            return cruiseObj;
        }
        public void CruiseExcute(IRCtrl irCtrl, TVCtrl tvCtrl)
        {
            gLogWriter.WriteLog("巡检进行中", "查询任务点");

            //查询任务列表
            if (GetCruiseTasks(1, out List<int> mlstintTastTable))
            {
                //for循环执行每个任务点
                for (int i = 0; i < mlstintTastTable.Count; i++)
                {
                    //if (CruiseSwtich == false)
                    //    break;
                    actionOneTaskCruise(mlstintTastTable[i], irCtrl, tvCtrl);
                }
            }
        }
        public void actionOneTaskCruise(int taskIndex, IRCtrl irCtrl, TVCtrl tvCtrl)
        {
            //Check  PrePosImf                      
            gLogWriter.WriteLog("巡检进行中--根据任务点查询对应的预置位,当前任务点Index:"+ taskIndex.ToString(), "");
            cruiseLogMsgs.Add("巡检进行中--根据任务点查询对应的预置位,当前任务点Index:" + taskIndex.ToString());
         
            List<PrePositionSet> prePosList = GetPrePosInfos(taskIndex);

            //check IR Camera Seting
            gLogWriter.WriteLog("巡检进行中--检查红外相机的配置", "");
            cruiseLogMsgs.Add("巡检进行中--检查红外相机的配置");

            //Goto PrePos;
            for (var i = 0; i < prePosList.Count; i++)
            {
                gLogWriter.WriteLog("巡检进行中--走预置位,预置位编号:" + prePosList[i].PrePositionNO, "当前任务点Index=" + taskIndex);
                cruiseLogMsgs.Add("巡检进行中--走预置位,预置位编号:" + prePosList[i].PrePositionNO + ",当前任务点Index:" + taskIndex);
               // tvCtrl.InvokePrePos(prePosList[i].PrePositionNO); 

                //Sleep Time wait for Pantilt to Position
                Thread.Sleep(50);

                //Autofocus Or Manual Focus;   
                gLogWriter.WriteLog("巡检进行中--红外聚焦", "");
                cruiseLogMsgs.Add("巡检进行中--红外聚焦");
                //if (prePosList[i].IRFocus == 0)
                //{
                //    gLogWriter.WriteLog("巡检进行中--红外自动聚焦", "");
                //    cruiseLogMsgs.Add("巡检进行中--红外自动聚焦");
                //    irCtrl.SetAutoFocus();
                //}
                //else
                //{
                //    gLogWriter.WriteLog("巡检进行中--红外手动聚焦", "焦距值"+ prePosList[i].IRFocus);
                //    cruiseLogMsgs.Add("巡检进行中--红外手动聚焦");
                //    irCtrl.SetManualFocus(prePosList[i].IRFocus);
                //}
               
                
                //sleep time wait for Auto Focus Finish
                Thread.Sleep(500);

                //Snap Picture;    
                gLogWriter.WriteLog("巡检进行中--可见光、红外抓图", "");
                cruiseLogMsgs.Add("巡检进行中--可见光、红外抓图");
                
                string IRImagefilePath = null;
                string TVImagefilePath = null;
                CruiseSnapPictures(taskIndex, 1, "测试预置位" + taskIndex + "-" + prePosList[i].PrePositionNO, irCtrl, tvCtrl, out IRImagefilePath, out TVImagefilePath); //路径为本地目录,暂未上传到ftp服务器

                //sleep wiait for FIle save finish
                Thread.Sleep(2000);

                //Analyse Hot Picture; 
                gLogWriter.WriteLog("巡检进行中--分析图片并获取温度值", "");
                cruiseLogMsgs.Add("巡检进行中--分析图片并获取温度值");
                double temp = 0;
                switch (prePosList[i].OneTask.MeaType)
                {
                    case 0:   //点温
                        temp = Cruise_SpotAnalyseHotPic(taskIndex, irCtrl, tvCtrl, IRImagefilePath);
                        break;
                    case 1:   //线温
                        Cruise_LineAnalyseHotPic(taskIndex, prePosList[i].OneTask.GetValueType, irCtrl, tvCtrl);
                        break;
                    case 2:  //区域测温
                        Cruise_AreaAnalyseHotPic(taskIndex, prePosList[i].OneTask.GetValueType, irCtrl, tvCtrl);
                        break;
                    case 3:  //多边形测温
                        Cruise_PolygonAnalyseHotPic(taskIndex, prePosList[i].OneTask.GetValueType, irCtrl, tvCtrl);
                        break;
                    case 4:   //圆形测温
                        Cruise_CircleAnalyseHotPic(taskIndex, prePosList[i].OneTask.GetValueType, irCtrl, tvCtrl);
                        break;
                }

                //Create Temptrue Record
                
                TempMeasure oneTempMeaRecord = new TempMeasure();
                oneTempMeaRecord.MeasureTask_Index = taskIndex;
                oneTempMeaRecord.MonDev_Index = 1;
                oneTempMeaRecord.MeasureValueMin = temp;
                oneTempMeaRecord.MeasureValueAvg = temp;
                oneTempMeaRecord.MeasureValueMax = temp;
                oneTempMeaRecord.AirTemperature = 37.00;
                oneTempMeaRecord.RecordTime = DateTime.Now;
                oneTempMeaRecord.TVFilePath = "";
                oneTempMeaRecord.IRFilePath = "";
                oneTempMeaRecord.VTVFilePath = "";
                oneTempMeaRecord.IRAFilePath = "";
                oneTempMeaRecord.VIRFilePath = "";
                oneTempMeaRecord.Readed = false;

                

                //Upload FTP File          
                string TVUploadToFtpPath = null;
                string IRUploadToFtpPath = null;
                UploadPicFilesToFTP(TVImagefilePath,IRImagefilePath,out TVUploadToFtpPath,out IRUploadToFtpPath);
                gLogWriter.WriteLog("巡检进行中--上传图片到FTP", "");
                cruiseLogMsgs.Add("巡检进行中--上传图片到FTP");
                string regex = "IR";
                string[] TVPath = TVUploadToFtpPath.Split(new string[] { regex }, StringSplitOptions.None);              
                string[] IRPath = IRUploadToFtpPath.Split(new string[] { regex }, StringSplitOptions.None);
                oneTempMeaRecord.TVVImgPath = regex+ TVPath[1];
                oneTempMeaRecord.IRVImgPath = Constant.ftp_autoCruiseFilePath+"20200225/红外视频截图-测试.jpg";
                oneTempMeaRecord.IRHImgPath = regex + IRPath[1];

                if (AddMeaTempRecords(oneTempMeaRecord))
                {
                    gLogWriter.WriteLog("巡检进行中--生成一条温度测量记录到数据库", "");
                    cruiseLogMsgs.Add("巡检进行中--生成一条温度测量记录到数据库");
                }

                //Analyse AlarmRecord;
                AlarmRecord alarmRecord = new AlarmRecord();
                Random rd = new Random();
                alarmRecord.TempMeasure_Index = rd.Next(1,15);
                alarmRecord.Rule_Index = rd.Next(1, 20);
                alarmRecord.Rule_Type = rd.Next(1, 4);
                alarmRecord.AlarmType = 1;
                alarmRecord.AlarmRecordTime = DateTime.Now;
                alarmRecord.RelMeaVal = temp;
                alarmRecord.RefPreAlarmVal = 20.00;
                alarmRecord.RefAlarmVal = 25;
                alarmRecord.RefSuperAlarmVal = 30;
                alarmRecord.Readed = false;
                if(AddAlarmRecords(alarmRecord))
                {
                    gLogWriter.WriteLog("巡检进行中--生成一条报警记录到数据库", "");
                    cruiseLogMsgs.Add("巡检进行中--生成一条报警记录到数据库");
                }
                
                //Upload FTP File
                //gLogWriter.WriteLog("巡检进行中--上传图片到FTP", "");
                //cruiseLogMsgs.Add("巡检进行中--上传图片到FTP");
                //UploadPicFilesToFTP();
                //check File 
                gLogWriter.WriteLog("巡检进行中--检查FTP服务器图片", "");
                cruiseLogMsgs.Add("巡检进行中--检查FTP服务器图片");
                CheckFTPPicFiles();
            }

        }
            
        
        /// <summary>
        /// 查询巡检任务点的索引
        /// </summary>
        /// <param name="intPosIndex"></param>
        /// <param name="mlstintTastTable"></param>
        /// <returns></returns>
        public bool GetCruiseTasks(int intPosIndex, out List<int> mlstintTastTable)
        {
            //返回任务点的Index索引
            mlstintTastTable = new List<int>();
            mlstintTastTable = taskSetDao.GetMeasureTaskIndex();
            return true;
        }

        public List<PrePositionSet> GetPrePosInfos(int taskIndex)
        {
           return prePosSetDao.GetPrePosIndexByTaskIndex(taskIndex);
        }

        private void CruiseSnapPictures(int taskIndex, int positionIndex, string PrePositionName, IRCtrl irCtrl, TVCtrl tvCtrl, out string IRImagefilePath, out string TVImagefilePath)
        {
            string days = DateTime.Now.ToString("yyyyMMdd");
            String imageSavePath = Constant.imageSavePath+@"\"+days;
            if (!Directory.Exists(imageSavePath))
            {
                Directory.CreateDirectory(imageSavePath);
            }
            string createTime =DateUtil.DateToString(DateTime.Now);
            IRImagefilePath = imageSavePath+@"\" + "红外热图_" + positionIndex + "_" + taskIndex + "_" + PrePositionName + "_" + createTime + ".jpg";
            TVImagefilePath = imageSavePath+ @"\"+ "可见光图_" + positionIndex + "_" + taskIndex + "_" + PrePositionName + "_" + createTime + ".JPG";
            //if (!irCtrl.SaveIRHotImage(IRImagefilePath))
            //{
            //    IRImagefilePath = null;
            //}
            //if (!tvCtrl.SaveImage(TVImagefilePath))
            //{
            //    TVImagefilePath = null;
            //}
        }

        public void CruisePicsAnalyse(int meaType,string IRImagefilePath,string TVImagefilePath, IRCtrl irCtrl, TVCtrl tvCtrl)
        {
           
        }


        public bool AddMeaTempRecords(TempMeasure oneTempMeaRecord)
        {
            return tempMeasureDao.AddOneTempMeasureRecord(oneTempMeaRecord);
        }

        public bool AddAlarmRecords(AlarmRecord alarmRecord)
        {
            return alarmRecordDao.AddOneAlarmRecord(alarmRecord);
        }

        public void UploadPicFilesToFTP(string TVImagefilePath,string IRImagefilePath,out string TVUploadPath,out string IRUploadPath)
        {
            string ftp_ip = INIUtil.Read("Ftp","ip",Constant.IniFilePath);
            string ftp_port = INIUtil.Read("Ftp", "port", Constant.IniFilePath);
            string ftpParentPath = "ftp://" + ftp_ip + ":" + ftp_port + "/"+Constant.ftp_autoCruiseFilePath;
            TVUploadPath = "";
            IRUploadPath = "";
            string[] strTVPathArrays = TVImagefilePath.Split('\\');
            string[] strIRPathArrays= IRImagefilePath.Split('\\');
            if (!string.IsNullOrEmpty(TVImagefilePath))
            {
                 TVUploadPath = ftpParentPath+ strTVPathArrays[3]+"/"+ strTVPathArrays[4];
               // FtpUtil.UploadFile(TVImagefilePath.Split('\\')[1], ftpParentPath, "root", "root");
            }
            if (!string.IsNullOrEmpty(IRImagefilePath))
            {
                IRUploadPath = ftpParentPath + strIRPathArrays[3] + "/" + strIRPathArrays[4];
                //  FtpUtil.UploadFile(IRImagefilePath.Split('\\')[1], ftpParentPath, "root", "root");
            }
        }

        public void CheckFTPPicFiles()
        {

        }

        private double Cruise_SpotAnalyseHotPic(int taskIndex, IRCtrl irCtrl, TVCtrl tvCtrl, string IRImagefileName)
        {
            MySqlConnection conn = GlobalCtrl.GetSqlConnection();
            double temperature = 0;
            string strSql = "select * from cruise_spotset where MeasureTask_Index = @taskIndex and Active=1";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@taskIndex", taskIndex));
            DataTable spotDatas = SqlHelper.QueryData(conn, strSql, parameters.ToArray());
            if (spotDatas.Rows.Count != 0)
            {
                for (var i = 0; i < spotDatas.Rows.Count; i++)
                {
                  //  irCtrl.GetAnaSpotTemp(i, ref temperature);
                }
            }
            return 25.00;
        }
        private void Cruise_LineAnalyseHotPic(int taskIndex, int tempValueType, IRCtrl irCtrl, TVCtrl tvCtrl)
        {
            MySqlConnection conn = GlobalCtrl.GetSqlConnection();
            string strSql = "select * from cruise_lineset where MeasureTask_Index = @taskIndex";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@taskIndex", taskIndex));
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
        private void Cruise_AreaAnalyseHotPic(int taskIndex, int tempValueType, IRCtrl irCtrl, TVCtrl tvCtrl)
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
        private void Cruise_PolygonAnalyseHotPic(int taskIndex, int tempValueType, IRCtrl irCtrl, TVCtrl tvCtrl)
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
        private void Cruise_CircleAnalyseHotPic(int taskIndex, int tempValueType, IRCtrl irCtrl, TVCtrl tvCtrl)
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

