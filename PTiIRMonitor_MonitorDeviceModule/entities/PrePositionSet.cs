using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.entities
{
    public class PrePositionSet
    {
        public int PrePosSet_Index { get; set; }
        public int Position_Index { get; set; }
        public int PrePositionNO { get; set; }
        public string PrePositionName { get; set; }
        public int PrePosType { get; set; }
        public int TVZoom { get; set; }
        public int IRFocus { get; set; }
        public int GoPrePosDelays { get; set; }
        public bool EnableMeasure { get; set; }
        public double PanAngle { get; set; }
        public double TiltAngle { get; set; }
        public double PTZHorAngle { get; set; }
        public double PTZVerAngle { get; set; }
        public int PaletteType { get; set; }
        public double TrackPos { get; set; }
        public string Remark { get; set; }

    }
}
