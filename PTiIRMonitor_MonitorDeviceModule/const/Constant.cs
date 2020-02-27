namespace PTiIRMonitor_MonitorDeviceModule.constant
{
    /// <summary>
    /// 全局常量类
    /// </summary>
    public class Constant
    {

        /// ini文件存放路径
        public static readonly string IniFilePath = @"E:\projects\Net项目备份\变电站无人值守系统\Output\Monitor.ini";
        public static readonly string imageSavePath = @"D:\Cruise_images\";

        public static readonly string ftp_autoCruiseFilePath = @"IRMonitorSystemFile/AutoCruiseFile/";
        public static readonly string ftp_userManualFilePath = @"IRMonitorSystemFile/UserManualFile/";

        //返回结果
        public static readonly string Result_OK = "OK";
        public static readonly string Result_ERROR = "error";


        /// <summary>
        /// 巡检状态描述
        /// </summary>
        public enum CruiseState
        {
            STOP = 0,  //未启动巡检
            FREE = 1,   //空闲状态
            RUNNING = 2,  //巡检中
            ERROR = -1    //巡检出错
        }     
    
    }

}
