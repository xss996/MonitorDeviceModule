using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peiport_commandManegerSystem
{
    public class clsGlobalVar
    {
        public class GlobalVar
        {
            public static ushort MAXEDGENUM = 10;
            public static ushort MINEDGENUM = 3;
            public const ushort RULE3MINTASK = 3;
            public const ushort RULE3MAXTASK = 9;
            public const ushort MAXPRENO = 255;
            public static string strTempFolder = "";

            #region 全局结构体函数
            public struct stuBackServerImf //socket服务器信息
            {
                public string sBackServerIP;
                public int iBackServerPort;
            }
            public static stuBackServerImf g_BSInfo = new stuBackServerImf();

            public struct stuDataBaseServerImf        //数据库信息
            {
                public string strDBIP;
                public uint uDBPort;
                public string strDBName;
                public string strUserName;
                public string strDBPsw;
            }
            public static stuDataBaseServerImf g_DBInfo = new stuDataBaseServerImf();

            public struct stuFtpServerImf        //Ftp服务器信息
            {
                public string sFtpIP;
                public uint iFtpPort;
                public string sUserName;
                public string sUserPwd;
                public string sFtpName;
            }
            public static stuFtpServerImf g_FtpInfo = new stuFtpServerImf();

            public struct stuVideoServerImf //视频推流服务器
            {
                public string sTVStreamType;
                public string sTVStreamIP;
                public uint iTVStreamPort;
                public string sTVDomain;
                public string sTVUserName;
                public string sTVUserPwd;
                public string sIRStreamType;
                public string sIRStreamIP;
                public uint iIRStreamPort;
                public string sIRDomain;
                public string sIRUserName;
                public string sIRUserPwd;
            }
            public static stuVideoServerImf g_VideoInfo = new stuVideoServerImf();

            public static int g_iCruisePosID = -1;
            public static string sPointList = "";
          
            #endregion
        }
    }
}
