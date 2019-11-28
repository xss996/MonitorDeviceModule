using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
    public class PolyDotParamItem
    {
        public int fLeftPer { get; set; }
        public int fTopPer { get; set; }

        public override string ToString()
        {
            return string.Format("PolyDotParamItem=[fLeftPer={0},fTopPer={1}]", fLeftPer, fTopPer);
        }
    }
}
