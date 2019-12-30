using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.entities
{
    public class Position
    {
        public int Position_Index { get; set; }
        public int Station_Index { get; set; }
        public string PositionName { get; set; }
        public double GPSLng { get; set; }
        public double GPSLat { get; set; }
        public double GPSHeight { get; set; }
        public double GeoPoint { get; set; }
        public int CruiseInterval { get; set; }
        public int CruiseType { get; set; }
        public DateTime StartCruiseTime { get; set; }
        public double RefPosX { get; set; }
        public double RefPosY { get; set; }
        public double AbsHeight { get; set; }
        public string Remark { get; set; }


    }
}
