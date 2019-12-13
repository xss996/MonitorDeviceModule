using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.ctrl;
using PTiIRMonitor_MonitorDeviceModule.entities;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
    public class CruiseCtrl
    {
        public Constant.CruiseState CruiseStatus = Constant.CruiseState.STOP;
        public bool isStartCruise = false;


        DateTime tempTime;
        public void StartCruise(IRCtrl irCtrl,TVCtrl tvctrl, CruiseObj cruiseObj)
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
                            //1.抓图分析
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

        public void StopCruise()
        {
            isStartCruise = false;
        }
    }
}
