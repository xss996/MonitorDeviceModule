using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.entities;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
    public class CruiseCtrl
    {
        public Constant.CruiseState CruiseStatus = Constant.CruiseState.STOP;
        public bool isStartCruise = false;


        DateTime tempTime;

        public void SetCruiseCtrl(bool state)
        {
            isStartCruise = state;
        }
        public void StartCruise(IRCtrl irCtrl, TVCtrl tvctrl, CruiseObj cruiseObj)
        {
            // CruiseObj cruiseObj = new CruiseObj();
            if (isStartCruise)
            {
                DateTime currentTime = DateTime.Now;  //获取当前时间

                if (cruiseObj.CruiseType == 0)  //隔时巡检
                {
                    if (cruiseObj.Interval > cruiseObj.CruiseTime)
                    {
                        if ((DateUtil.GetSumMinutes(currentTime) - DateUtil.GetSumMinutes(cruiseObj.StartTime)) % cruiseObj.Interval == 0)
                        {
                            /// goto 巡检操作
                            //  cruiseObj.StartTime = currentTime;
                            tempTime = currentTime;
                            CruiseStatus = Constant.CruiseState.RUNNING;
                            Debug.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                            Debug.WriteLine(string.Format(">>>>>>>>>>提示:系统开始执行隔时巡检,当前时间:{0}", currentTime));
                            //先查出巡检相关的预置位
                            //走预置位

                            //抓图
                            string monIndex = INIUtil.Read("MonDev", "Index", Constant.IniFilePath);
                            string ftpPath = "ftp://" + INIUtil.Read("Ftp", "ip", Constant.IniFilePath);
                            string ftp_loginName = INIUtil.Read("Ftp", "username", Constant.IniFilePath);
                            string ftp_password = INIUtil.Read("Ftp", "password", Constant.IniFilePath);
                            string fileName = Constant.imageSavePath + ("IRHotPic-" + (monIndex + "-") + DateUtil.DateToString(currentTime) + ".jpg");
                            string fileName2 = Constant.imageSavePath + ("TVPic-" + (monIndex + "-") + DateUtil.DateToString(currentTime));
                            if (irCtrl.SaveIRHotImage(fileName))
                            {
                                FtpUtil.UploadFile(fileName, ftpPath, ftp_loginName, ftp_password);
                            }

                            Debug.WriteLine("tvctrl=" + tvctrl.ToString());
                            if (tvctrl.SaveImage(fileName2))
                            {
                                FtpUtil.UploadFile(fileName2, ftpPath, ftp_loginName, ftp_password);
                            }
                            //生成结果报警记录
                        }
                        else
                        {
                            cruiseObj.StartTime = tempTime;
                            if ((DateUtil.GetSumMinutes(currentTime) - DateUtil.GetSumMinutes(cruiseObj.StartTime)) <= cruiseObj.CruiseTime)
                            {
                                Debug.WriteLine(string.Format(">>>>>>>>>>>>>>>>>>>>>系统正在巡检,当前时间:{0},开始巡检时间:{1}", currentTime, cruiseObj.StartTime));
                                CruiseStatus = Constant.CruiseState.RUNNING;
                            }
                            else
                            {
                                CruiseStatus = Constant.CruiseState.FREE;
                                Debug.WriteLine("提示:系统巡检空闲中..............................................................." + currentTime);
                            }
                        }
                    }
                }
                if (cruiseObj.CruiseType == 1)  //定时巡检
                {
                    foreach (DateTime dt in cruiseObj.dateTimeList)
                    {
                        if (DateUtil.GetSumMinutes(currentTime) == DateUtil.GetSumMinutes(dt))
                        {
                            /// goto 巡检操作
                            cruiseObj.StartTime = dt;
                            CruiseStatus = Constant.CruiseState.RUNNING;
                            Debug.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                            Debug.WriteLine(string.Format("提示:系统开始执行定时巡检,当前时间:{0}", currentTime));
                            //1.抓图分析
                            string monIndex = INIUtil.Read("MonDev", "Index", Constant.IniFilePath);
                            string ftpPath = "ftp://" + INIUtil.Read("Ftp", "ip", Constant.IniFilePath);
                            string ftp_loginName = INIUtil.Read("Ftp", "username", Constant.IniFilePath);
                            string ftp_password = INIUtil.Read("Ftp", "password", Constant.IniFilePath);
                            string fileName = Constant.imageSavePath + ("IRHotPic-" + (monIndex + "-") + DateUtil.DateToString(currentTime) + ".jpg");
                            string fileName2 = Constant.imageSavePath + ("TVPic-" + (monIndex + "-") + DateUtil.DateToString(currentTime) + ".jpg");
                            if (irCtrl.SaveIRHotImage(fileName))
                            {
                                FtpUtil.UploadFile(fileName, ftpPath, ftp_loginName, ftp_password);
                            }
                            if (irCtrl.SaveVideoImage(fileName2))
                            {
                                FtpUtil.UploadFile(fileName2, ftpPath, ftp_loginName, ftp_password);
                            }

                            //生成结果报警记录
                        }
                    }

                    if ((DateUtil.GetSumMinutes(currentTime) - DateUtil.GetSumMinutes(cruiseObj.StartTime)) <= cruiseObj.CruiseTime)
                    {
                        Debug.WriteLine(string.Format(">>>>>>>>>>>>>>>>>>>>>系统正在巡检,当前时间:{0},开始巡检时间:{1}", currentTime, cruiseObj.StartTime));
                        CruiseStatus = Constant.CruiseState.RUNNING;
                    }
                    else
                    {
                        Debug.WriteLine(string.Format(">>>>>>>>>>>>>>>>>>>>>系统未在巡检,当前时间:{0},开始巡检时间:{1}", currentTime, cruiseObj.StartTime));
                        CruiseStatus = Constant.CruiseState.FREE;
                    }
                }
            }
            Debug.WriteLine(string.Format(">>>>>>>>>>>当前巡检状态:{0}", CruiseStatus));
        }     

        public bool CheckTime()
        {
            if (Convert.ToInt32(CruiseStatus)>-1&&Convert.ToInt32(CruiseStatus) < 2)
            {
                List<DateTime> dateTimeList = new List<DateTime>();
                DateTime currentTime = DateTime.Now;
                int cruiseType = 0;
                foreach (DateTime time in dateTimeList)
                {
                    if (DateUtil.GetSumMinutes(currentTime) - DateUtil.GetSumMinutes(time) == 0)
                    {
                        CruiseStatus = Constant.CruiseState.RUNNING;
                        return true;
                    }

                }
            }
           
            return false;
        }

        public void CruiseExcute(IRCtrl irCtrl, TVCtrl tvctrl,GlobalCtrl globalCtrl, CruiseObj cruiseObj)
        {
           if(irCtrl.GetIRConnectStatus() && tvctrl.GetTVStatus() > 0 && globalCtrl.DatabaseStatus)
            {

            }
            else
            {
                Debug.WriteLine("提示:请检查红外连接,可将光连接,数据库连接是否正常");
                return;
            }
        }
    }
}
