using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.entities
{
    public class AlarmRecord
    {
        public int AlarmRecord_Index { get; set; }
        public int TempMeasure_Index { get; set; }
        public int Rule_Index { get; set; }
        public int Rule_Type { get; set; }
        public int AlarmType { get; set; }
        public DateTime AlarmRecordTime { get; set; }
        public double RelMeaVal { get; set; }
        public double RefPreAlarmVal { get; set; }
        public double RefAlarmVal { get; set; }
        public double RefSuperAlarmVal { get; set; }
        public bool Readed { get; set; }
    }
}
