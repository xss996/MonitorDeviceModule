using MySql.Data.MySqlClient;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.entities;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        public bool CheckTimeIsNeedCruise()
        {
            bool result = false;
            CruiseObj cruiseObj =GetCruiseInfo();

            //if (!flag)
            //    return false;

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
                    if (CruiseSwtich == false)
                        break;
                    actionOneTaskCruise(mlstintTastTable[i], irCtrl, tvCtrl);
                }
            }
        }
        public void actionOneTaskCruise(int taskId, IRCtrl irCtrl, TVCtrl tvCtrl)
        {
            //Check  PrePosImf                      
            gLogWriter.WriteLog("巡检进行中--根据任务点查询对应的预置位,当前任务点ID:"+ taskId.ToString(), "");
            cruiseLogMsgs.Add("巡检进行中--根据任务点查询对应的预置位,当前任务点ID:" + taskId.ToString());
            GetPrePosInfos(taskId);
            //check IR Camera Seting
            gLogWriter.WriteLog("巡检进行中--检查红外相机的配置", "");
            cruiseLogMsgs.Add("巡检进行中--检查红外相机的配置");
            //Goto PrePos;
            for (var i = 0; i < 3; i++)
            {
                gLogWriter.WriteLog("巡检进行中--走预置位,预置位ID:" + i, "当前任务点ID=" + taskId);
                cruiseLogMsgs.Add("巡检进行中--走预置位,预置位ID:" + i+"当前任务点ID:" + taskId);
                tvCtrl.InvokePrePos(i+1);
                //Sleep Time wait for Pantilt to Position
                Thread.Sleep(50);
                //Autofocus Or Manual Focus;
                irCtrl.SetAutoFocus();
                gLogWriter.WriteLog("巡检进行中--红外聚焦", "");
                cruiseLogMsgs.Add("巡检进行中--红外聚焦");
                //sleep time wait for Auto Focus Finish
                Thread.Sleep(500);
                //Snap Picture;    
                gLogWriter.WriteLog("巡检进行中--可见光、红外抓图", "");
                cruiseLogMsgs.Add("巡检进行中--可见光、红外抓图");
                string IRImagefilePath = null;
                string TVImagefilePath = null;
                CruiseSnapPictures(taskId, 1, "测试预置位" + taskId + "-" + i, irCtrl, tvCtrl, out IRImagefilePath, out TVImagefilePath);
                //sleep wiait for FIle save finish
                Thread.Sleep(2000);
                //Analyse Hot Picture; 
                gLogWriter.WriteLog("巡检进行中--分析图片", "");
                cruiseLogMsgs.Add("巡检进行中--分析图片");
                CruisePicsAnalyse(IRImagefilePath, TVImagefilePath);
                //Create Temptrue Record
                gLogWriter.WriteLog("巡检进行中--生成温度测量记录", "");
                cruiseLogMsgs.Add("巡检进行中--生成温度测量记录");
                AddMeaTempRecords();
                //Analyse AlarmRecord;
                gLogWriter.WriteLog("巡检进行中--生成报警记录", "");
                cruiseLogMsgs.Add("巡检进行中--生成报警记录");
                AddAlarmRecords();
                //Upload FTP File
                gLogWriter.WriteLog("巡检进行中--上传图片到FTP", "");
                cruiseLogMsgs.Add("巡检进行中--上传图片到FTP");
                UploadPicFilesToFTP();
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
            //返回任务点的ID列表
            mlstintTastTable = new List<int>();
            for(var i = 0; i < 5; i++)
            {
                mlstintTastTable.Add(i + 1);
            }  
            return true;
        }

        public void GetPrePosInfos(int taskId)
        {

        }

        private void CruiseSnapPictures(int taskId, int positionIndex, string PrePositionName, IRCtrl irCtrl, TVCtrl tvCtrl, out string IRImagefilePath, out string TVImagefilePath)
        {
            string days = DateTime.Now.ToString("yyyyMMdd");
            String imageSavePath = Constant.imageSavePath+@"\"+days;
            if (!Directory.Exists(imageSavePath))
            {
                Directory.CreateDirectory(imageSavePath);
            }
            string createTime =DateUtil.DateToString(DateTime.Now);
            IRImagefilePath = imageSavePath+@"\" + "红外热图_" + positionIndex + "_" + taskId + "_" + PrePositionName + "_" + createTime + ".jpg";
            TVImagefilePath = imageSavePath+ @"\"+ "可见光图_" + positionIndex + "_" + taskId + "_" + PrePositionName + "_" + createTime;
            if (!irCtrl.SaveIRHotImage(IRImagefilePath))
            {
                IRImagefilePath = null;
            }
            if (!tvCtrl.SaveImage(TVImagefilePath))
            {
                TVImagefilePath = null;
            }
        }

        public void CruisePicsAnalyse(string IRImagefilePath,string TVImagefilePath)
        {

        }


        public void AddMeaTempRecords()
        {

        }

        public void AddAlarmRecords()
        {

        }

        public void UploadPicFilesToFTP()
        {

        }

        public void CheckFTPPicFiles()
        {

        }
    }
}

