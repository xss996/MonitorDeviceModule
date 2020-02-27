using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.entities
{
   public  class TempMeasure
    {
        public int TempMeasure_Index { get; set; }
        public int MeasureTask_Index { get; set; }
        public int MonDev_Index { get; set; }
        public double MeasureValueMin { get; set; }
        public double MeasureValueAvg { get; set; }
        public double MeasureValueMax { get; set; }
        public double AirTemperature { get; set;  }
        public double Humidity { get; set; }
        public double Electricty { get; set; }
        public DateTime RecordTime { get; set; }
        public string TVFilePath { get; set; }
        public string IRFilePath { get; set; }
        public string VTVFilePath { get; set; }
        public string IRAFilePath { get; set; }
        public string VIRFilePath { get; set; }
        public string TVVImgPath { get; set; }
        public string IRVImgPath { get; set; }
        public string IRHImgPath { get; set; }
        public bool Readed { get; set; }

    }
}
