using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SocketClientTest
{
    static public class gLogWriter
    {
        static private string gsWorkLogFileName;
        private const int RESERVEDAYS = 7;

        static gLogWriter()
        {
            ClearAndSetup();
            StartLog();
        }

        static public void ClearAndSetup()
        {
            try
            {
                string directoryPath = Directory.GetCurrentDirectory() + @"\WorkLog";
                if (Directory.Exists(directoryPath))
                {
                    List<string> lstLogFile = Directory.GetFiles(directoryPath).ToList();
                    foreach (string strLogFile in lstLogFile)
                    {
                        string strfileName = Path.GetFileNameWithoutExtension(strLogFile);
                        string strDate = strfileName.Substring(7);
                        DateTime dtFile = DateTime.ParseExact(strDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                        TimeSpan ts = DateTime.Now - dtFile;
                        if (ts.TotalDays >= RESERVEDAYS)
                            File.Delete(strLogFile);
                    }
                }
                else
                    Directory.CreateDirectory(directoryPath);

                gsWorkLogFileName = directoryPath + @"\Worklog" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        static public void WriteLog(string sType, string sItem)
        {
            try
            {
                string strNowDate = DateTime.Now.ToString("yyyyMMdd");
                string strfileName = Path.GetFileNameWithoutExtension(gsWorkLogFileName);
                string strDate0 = strfileName.Substring(7);
                if (strNowDate != strDate0)
                    ClearAndSetup();

                string str1 = DateTime.Now.ToString("HH:mm:ss,fff") + "\t\t" + sType + "\t\t" + sItem + "\r\n";
                File.AppendAllText(gsWorkLogFileName, str1, Encoding.Default);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        static public void StartLog()
        {
            try
            {
                string str1 = DateTime.Now.ToString("HH:mm:ss,fff") + "\t\t" + "启动运行日志记录" + "\r\n";
                File.AppendAllText(gsWorkLogFileName, str1, Encoding.Default);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        static public void StopLog()
        {
            try
            {
                string str1 = DateTime.Now.ToString("HH:mm:ss,fff") + "\t\t" + "程序正常退出，停止运行日志记录" + "\r\n\r\n";
                File.AppendAllText(gsWorkLogFileName, str1, Encoding.Default);
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
