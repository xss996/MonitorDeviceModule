using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.entities
{
    public class SpotSet
    {
        public int Spot_Index { get; set; }
        public int MeasureTask_Index { get; set; }
        public int IRSpotNO { get; set; }
        public double Pos_x { get; set; }
        public double Pos_y { get; set; }
        public bool Use_Local { get; set; }
        public double Emissivity { get; set; }
        public double Distance { get; set; }
        public double Temperature { get; set; }
        public double Humitidy { get; set; }
        public bool Pt_Enable { get; set; }
        public bool Active { get; set; }
    }
}
