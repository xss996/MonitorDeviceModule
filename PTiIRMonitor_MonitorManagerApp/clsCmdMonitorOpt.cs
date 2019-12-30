using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Threading;
namespace PTiIRMonitor_MonitorManagerApp
{

    public class clsCmdMonitorOpt
    {
        public clsTreeView Tree = new clsTreeView();
        //启动命令服务器
        public void ConnectCmdServer()
        {
            Tree.ConnectServerIni();
        }
        //查询命令服务器状态
        public int CmdServerState()
        {

            return 0;
        }
        //关闭命令服务器
        public int CmdServerKill()
        {
            Process[] pcs = Process.GetProcesses();
            foreach (Process p in pcs)
            {
                if (p.ProcessName == "Peiport_commandManegerSystem") //关闭命令服务器进程
                {
                    p.Kill();//结束进程
                }
            }
            return 0;
        }

        #region 数据分析服务器
        //启动数据分析服务器
        public void ConnectDataServer()
        {

        }
        //查询数据分析服务器的状态
        public int DateServerState()
        {
            return 0;
        }
        //关闭数据分析服务器
        public void DateServerKill()
        {

        }
        #endregion

        //启动ini监控头
        public void MonitorDevice()
        {
            Tree.MonitorDeviceIni();
        }
        //关闭监控端,根据进程PID关闭
        public void MonitorOut(int inPid)//获取监控头编号
        {
            Tree.MonitorDeviceKill(Convert.ToString(inPid));
        }
        //启动单个监控头
        public void MonitorDeviceStart(string strName)
        {
            Tree.MonintorDevice(strName);
        }
        //关闭看门狗(对于看门狗关闭，所有连接软件也跟着关闭)
        public void funManagerDeviceKill()
        {
            try
            {
                Process[] pcs = Process.GetProcesses();
                foreach (Process p in pcs)
                {
                    if (p.ProcessName == "Peiport_pofessionalMonitorDeviceClient") //关闭命令服务器进程
                    {
                        p.Kill();//结束进程
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    
    }
}
