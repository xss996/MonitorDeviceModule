using System.Collections.Generic;

namespace PTiIRMonitor_MonitorDeviceModule.entities
{
    public class JsonItem
    {
        public string seq { get; set; }
        public int cmdType { get; set; }
        public string cmdAction { get; set; }
        public string result { get; set; }
        public string sender { get; set; }
        public string receiver { get; set; }
        public List<JsonitemParam> paramList = new List<JsonitemParam>();

    }

    public class JsonitemParam
    {
        public string param { get; set; }
        public string value { get; set; }
    }
}
