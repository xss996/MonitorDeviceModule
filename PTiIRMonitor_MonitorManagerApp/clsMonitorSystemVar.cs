using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTiIRMonitor_MonitorManagerApp
{
    public class GlbMonitorVar
    {
        public string strName;      //监控头编号
        public int intType;    //设备类型
        public int inPID;      //监控进程Pid
        public int inFoName;     //给窗口编号
        public int MonitorID;
        public int MoState;    //监控头状态
        public DateTime dateTine;  //监控头发送心跳的状态的时间

        public static uint g_uTotalMonitorDev;//从INI中取

        public int devProcessServerIndex;    //命令服务器进程的index

        public int[] devProcessMonitorIndex;

        public GlbMonitorVar( int Type, int Pid, int MonState)  //定义监控头的相关信息
        {
            intType = Type;
            inPID = Pid;
            MonitorID = MonState;
        }
        public class stuMonServer  //服务器的相关ip
        {
            int inFormState;     //状态
            string strCliServerIP;    //客户端IP
            int inCliServerPort;
            string strStateCl;
            string strMonServerIP;    //监控端IP
            int inMonServerPort;
            string strStateMO;
        }

    }
}
