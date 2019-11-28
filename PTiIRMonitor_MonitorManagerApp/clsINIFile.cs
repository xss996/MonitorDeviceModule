using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace PTiIRMonitor_MonitorManagerApp
{
    public class clsINIFileOP
    {
        #region API函数声明
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key, string defaultvalue, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string section, string key, int defaultvalue, string filePath);

        
        #endregion
        #region 初始化
        string INIFileName;
        public bool bFileExist = false;
        clsINIFileOP()
        {

        }
        public clsINIFileOP(string fileName)
        {
            bFileExist = false;
            INIFileName = fileName;
            if (File.Exists(INIFileName))
                bFileExist = true;
            else
                bFileExist = false;
        }

        #endregion
        #region 写入
        public bool WriteKeyValue(string section, string key, string val)
        {
            long OpStation = WritePrivateProfileString(section, key, val, INIFileName);
            if (OpStation == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
        #region 读取
        public string ReadKeyValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            long opRet = GetPrivateProfileString(section, key, "", temp, 1024, INIFileName);
            if (opRet == 0)
                return "";
            else
                return temp.ToString();
        }
        #endregion
    }
}
