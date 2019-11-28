using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peiport_commandManegerSystem
{
    public class GlbMonitorVar
    {
        public static int g_intMonDevIniIndex;  //记载INI文件序号
        public static stuMonitorDevImf g_OneMonitorDev = new stuMonitorDevImf();  //只保存设备的一些参数
        public struct stuMonitorDevImf       //定义服务器Ip
        {
            //数据库相关信息
            public int DB_iIndex;
            public uint DB_uDevType;  //设置类型  0-预置位云台，1-角度云台，2-机器人

            public uint DB_uDevID;       //设备号  
            public string DB_sDevName;    //设备名
            public string DB_sDevSN;     //设备系列号 
            public int DB_iStatus;
            public int DB_iCruiseRouteIndex;
        }

    }
}
