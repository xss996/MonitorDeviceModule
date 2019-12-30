using System.Runtime.InteropServices;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.util
{
    public class INIUtil
    {
        private INIUtil()
        {

        }
        /// <summary>
        /// 修改INI配置文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">关键字</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件完整路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

        /// <summary>
        /// 读INI配置文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def">缺省值</param>
        /// <param name="retval"></param>
        /// <param name="size">指定装载到lpReturnedString缓冲区的最大字符数量</param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        #region 二次封装
        public static string Read(string section, string key, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            GetPrivateProfileString(section, key, "", sb, 1024, filePath);
            return sb.ToString();
        }

        public static long Write(string section, string key, string val, string filepath)
        {
            return WritePrivateProfileString(section, key, val, filepath);
        }
        #endregion
    }
}
