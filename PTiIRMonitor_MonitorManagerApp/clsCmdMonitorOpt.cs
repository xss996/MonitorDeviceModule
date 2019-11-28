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
        public Form1 frmThis;
        //启动命令服务器
        public void ConnectCmdServer()
        {
            Tree.ConnectServerIni();
        }
        //查询命令服务器状态
        public int CmdServerState()
        {
            return 0;
            //return Tree.CmdServerState;
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



        /////////////////////////////////////////////////////////////////////////////////////////
        //启动心跳服务器
        Server g_myServer = new Server("192.168.123.126", 8900);
        bool blConnectClientTabChange = false;
       public  List<string> lststrReceiCmd = new List<string>();
        bool blReceiMsg = false;
        //启动心跳服务器
        public void OptJsonConnectServer()
        {
            g_myServer = new Server("192.168.123.126", 8900);
            g_myServer.OnConnect += new Server.EventHandler_Connect(m_clsClient_OnConnect);//连接登录状态
            g_myServer.OnRecvMsg += new Server.EventHandler_Recv(myServer_OnRecvMsg);//接收
            g_myServer.StartServer();//启动服务器
        }

        public void m_clsClient_OnConnect(object obj, EventArgs_Connect e)//连接状态
        {
        }
        //接收心跳命令,分析状态
        public void myServer_OnRecvMsg(object source, EventArgs_Recv e)
        {
            //区分服务器心跳命令还是监控模块心跳命令........................

            lststrReceiCmd.Add(e.sRecvMsg);
            blReceiMsg = true;

        }



        //以下暂时无用

        public void funMonitorServerReceiCmdDealScan()//定时接收信息
        {
            string str1;
            blReceiMsg = false;
            for (int i = 0; i < lststrReceiCmd.Count; i++)
            {
                Thread.Sleep(50);
                str1 = lststrReceiCmd[i];
                if (str1 == "一号")//表示一号监控头
                {
                    for (int j = 0; j < Tree.IniMonitor.Count; j++)
                    {
                        if (Tree.IniMonitor[j].strName=="监控头_1")
                        {
                            Tree.IniMonitor[j].MoState = 1;//写入状态，1表示在线，0表示停止运行
                            Tree.IniMonitor[j].dateTine = DateTime.Now;//显示心跳时间

                        }
                    }
                }
                else if (str1 == "二号")//表示二号监控头
                {
                    for (int j = 0; j < Tree.IniMonitor.Count; j++)
                    {
                        if (Tree.IniMonitor[j].strName == "监控头_2")
                        {
                            Tree.IniMonitor[j].MoState = 1;//写入状态，1表示在线，0表示停止运行
                            Tree.IniMonitor[j].dateTine = DateTime.Now;//显示心跳时间
                        }
                    }
                }
                else if (str1 == "三号")
                {
                    for (int j = 0; j < Tree.IniMonitor.Count; j++)
                    {
                        if (Tree.IniMonitor[j].strName == "监控头_3")
                        {
                            Tree.IniMonitor[j].dateTine = DateTime.Now;//显示心跳时间
                        }
                    }
                }
                else if (str1 == "四号")
                {
                    for (int j = 0; j < Tree.IniMonitor.Count; j++)
                    {
                        if (Tree.IniMonitor[j].strName == "监控头_4")
                        {
                            Tree.IniMonitor[j].dateTine = DateTime.Now;//显示心跳时间
                        }
                    }
                }
               
            }
            lststrReceiCmd.Clear();
        }
        public void funQuestUserServerStatus(out int intStatus)  //查询服务器的工作状态 0-关闭，1-打开，2-出错
        {
            if (g_myServer.Thread_Server != null)
            {
                intStatus = 1;
                return;
            }
            else
            {
                intStatus = 0;
                return;
            }
        }
    }
}
