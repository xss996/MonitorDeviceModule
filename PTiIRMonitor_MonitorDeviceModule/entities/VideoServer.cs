using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.entities
{
   public  class VideoServer
    {
        public int VideoServer_Index { get; set; }
        public int MonDev_Index { get; set; }
        public int TVStreamType { get; set; }  //1.A310;2.A615
        public string TVStreamIP { get; set; }
        public int TVStreamPort { get; set; }
        public string TVDomain { get; set; }
        public string TVUserName { get; set; }
        public string TVUserPwd { get; set; }
        public int IRStreamType { get; set; }  //1.A310;2.A615
        public string IRStreamIP { get; set; }
        public int IRStreamPort { get; set; }
        public string IRDomain { get; set; }
        public string IRUserName { get; set; }
        public string IRUserPwd { get; set; }
        public string Remark { get; set; }


    }
}
