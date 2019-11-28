using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.util
{
   public class DateUtil
    {
        private DateUtil() { }
        /// <summary>
        /// 获取当前时间,精确到毫秒,并转化成字符串
        /// </summary>
        /// <returns></returns>
        public static string DateToString()
        {
            DateTime date = DateTime.Now;
            return date.ToString("yyyyMMddHHmmssfff");
        }
    }
}
