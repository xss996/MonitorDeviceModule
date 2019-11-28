using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Peiport_commandManegerSystem
{
    class clsOneMonDevOptFun
    {
        public static void UpdateIniBackServe_Client(string strip, int intPort)//写入客户端ip端口
        {
            bool blFlag;
            string strSection, strKey, strVal;
            string strFileName;
            strFileName = "\\Server" + GlbMonitorVar.g_intMonDevIniIndex.ToString("00") + ".ini";

            clsINIFileOP op = new clsINIFileOP(Application.StartupPath + strFileName);
            if (op.bFileExist)
            {
                try
                {
                    //写
                    strSection = "CmdServerToClient";
                    strKey = "IP";
                    blFlag = op.WriteKeyValue(strSection, strKey, strip);
                    strKey = "Port";
                    blFlag = op.WriteKeyValue(strSection, strKey, intPort.ToString());
                }
                catch (Exception ex)
                {
                    return;
                }
            }

        }
        public static void UpdateIniBackServe_Monitor(string strIP, int intPort)//写入监控端ip端口
        {
            bool blFlag;
            string strSection, strKey, strVal;
            string strFileName;
            strFileName = "\\Server" + GlbMonitorVar.g_intMonDevIniIndex.ToString("00") + ".ini";

            clsINIFileOP op = new clsINIFileOP(Application.StartupPath + strFileName);
            if (op.bFileExist)
            {
                try
                {
                    //写
                    strSection = "CmdServerToMonDev";
                    strKey = "IP";
                    blFlag = op.WriteKeyValue(strSection, strKey, intPort.ToString());
                    strKey = "Port";
                    blFlag = op.WriteKeyValue(strSection, strKey, intPort.ToString());
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }
    }
}
