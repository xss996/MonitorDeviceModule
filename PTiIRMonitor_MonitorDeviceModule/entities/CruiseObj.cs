using System;
using System.Collections.Generic;

namespace PTiIRMonitor_MonitorDeviceModule.entities
{
    public class CruiseObj
    {
        public int CruiseType { get; set; }  //0隔时,1定时
        public DateTime LastCruiseTime { get; set; } //上一次巡检时间
        //public int Interval { get; set; }  
        public List<DateTime> dateTimeList { get; set; }  //定时时间对象集合
        public int CruiseTime { get; set; }  //巡检时长
        

    }
}
