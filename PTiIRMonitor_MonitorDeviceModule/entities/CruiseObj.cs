using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.entities
{
    public class CruiseObj
    {
        public int CruiseType { get; set; }  //0隔时,1定时
        public DateTime StartTime { get; set; } //开始时间
        public int Interval { get; set; }  //隔时时长
        public List<DateTime> dateTimeList { get; set; }  //定时时间对象集合
        public int CruiseTime { get; set; }  //巡检时长  
    }
}
