using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.entities
{
    public class MeasureTaskSet
    {
        public int MeasureTask_Index { get; set; }
        public int PrePosSet_Index { get; set; }
        public int DeviceInfo { get; set; }
        public int MeaType { get; set; }
        public int CamMeasureNO { get; set; }
        public string TaskName { get; set; }
        public int GetValueType { get; set; }
        public string Remark { get; set; }


    }
}
