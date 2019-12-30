namespace PTiIRMonitor_MonitorDeviceModule.entities
{
    public class MonitorDevice
    {
        public int MonDev_Index { get; set; }
        public int? Position_Index { get; set; }
        public int? Room_Index { get; set; }
        public int MonDev_ID { get; set; }
        public string MonDev_Name { get; set; }
        public string SerialNum { get; set; }
        /// <summary>
        /// 1）A310，云台预置位
        ///2）A615，云台预置位
        ///3）A310，云台角度控制
        ///4) A615，云台角度控制
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// true 启用 ,false 不启用
        /// </summary>
        public bool Status { get; set; }
        public string Remark { get; set; }

    }
}
