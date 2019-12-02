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

        /// <summary>
        /// 日期截取时分部分换算成总共多少分钟
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetSumMinutes(DateTime dateTime)
        {
            if(dateTime != null)
            {
                string strTime = dateTime.ToString("yyyyMMddHHmmssfff");
                int hours = Convert.ToInt32(strTime.Substring(8, 2));
                int minutes = Convert.ToInt32(strTime.Substring(10, 2));

                return hours * 60 + minutes;
            }

            return -1;
            
        }
    }
}
