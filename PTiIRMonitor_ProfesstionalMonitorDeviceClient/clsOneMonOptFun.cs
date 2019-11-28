using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Peiport_commandManegerSystem
{
    public class clsOneMonOptFun
    {
        public static void UpdateIniBackServe_Client(string strip, int intPort)//写入
        {
            bool blFlag;
            string strSection, strKey;
            string strFileName;
            strFileName = "\\Client" + GlbMonitorVar.g_intMonDevIniIndex.ToString("00") + ".ini";

            clsINIFileOP op = new clsINIFileOP(Application.StartupPath + strFileName);
            if (op.bFileExist)
            {
                try
                {
                    //写入
                    strSection = "CmdServerToClient";
                    strKey = "IP";
                    blFlag = op.WriteKeyValue(strSection, strKey, strip);
                    strKey = "Port";
                    blFlag = op.WriteKeyValue(strSection, strKey, intPort.ToString());
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
