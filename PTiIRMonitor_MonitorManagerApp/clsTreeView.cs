using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
namespace PTiIRMonitor_MonitorManagerApp
{
    public class clsTreeView
    {
        public List<GlbMonitorVar> IniMonitor = new List<GlbMonitorVar>();
        public bool CmdServerState = false;  //服务器状态

        //启动命令服务器界面
        public bool ConnectServerIni()//
        {
            clsINIFileOP op = new clsINIFileOP(Application.StartupPath + "\\MonitorP.ini");
            string strAddress;
            if (1 == Convert.ToInt32(op.ReadKeyValue("CmdServerToClient", "ServerNot")))//判断是否启动_1启动_0停止
            {
                try
                {
                    strAddress = op.ReadKeyValue("CmdServerToClient", "pathServer");
                    ProcessStartInfo psi = new ProcessStartInfo(@strAddress);
                    psi.RedirectStandardOutput = true;
                    psi.UseShellExecute = false;
                    Process myProcess = Process.Start(@strAddress);//启动外部进程
                    return CmdServerState = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "服务器文件地址错误!");
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        //启动数据分析服务器
        //关闭



        //启动ini配置多少个(多个)监控头
        public void MonitorDeviceIni()
        {
            clsINIFileOP op = new clsINIFileOP(Application.StartupPath + "\\MonitorP.ini");
            string strAddress;
            int int1, ID, inType, Pid, MonitorState = 0;
            if (1 == Convert.ToInt32(op.ReadKeyValue("MonitorDeviceModule", "DeviceNot")))
            {
                int1 = Convert.ToInt32(op.ReadKeyValue("MonitorDeviceModule", "DeviceAmount"));
                for (int i = 0; i < int1; i++)
                {
                    try
                    {
                        strAddress = op.ReadKeyValue("MonitorDeviceModule", "PathDevice");
                        ProcessStartInfo psi = new ProcessStartInfo(@strAddress);

                        psi.RedirectStandardOutput = true;
                        psi.UseShellExecute = false;
                        Process myProcess = Process.Start(@strAddress);//启动外部进程
                        Pid = myProcess.Id;//
                        inType = Convert.ToInt32(op.ReadKeyValue("MonitorDeviceModule", "DeviceType"));//获取监控头类型

                        MonitorState++;//监控ID
                        GlbMonitorVar Glb = new GlbMonitorVar(inType, Pid, MonitorState);

                        IniMonitor.Add(Glb);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "监控头文件地址错误！");
                        throw;
                    }
                }
            }
            else
            {
                return;
            }
        }
        //启动单个监控头
        public void MonintorDevice(string strName)
        {
            clsINIFileOP op = new clsINIFileOP(Application.StartupPath + "\\MonitorP.ini");
            string strAddress;
            for (int i = 0; i < IniMonitor.Count; i++)
            {
                if (IniMonitor[i].strName == strName)
                {
                    strAddress = op.ReadKeyValue("MonitorDeviceModule", "PathDevice");
                    ProcessStartInfo psi = new ProcessStartInfo(@strAddress);
                    psi.RedirectStandardOutput = true;
                    psi.UseShellExecute = false;
                    Process myProcess = Process.Start(@strAddress);//启动外部进程
                    IniMonitor[i].inPID = myProcess.Id;//
                }
            }
        }
        //关闭单个监控头
        public void MonitorDeviceKill(string strPid)
        {
            try
            {
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.Id.Equals(Int32.Parse(strPid)))
                    {
                        if (!p.CloseMainWindow())
                        {
                            p.Kill();
                        }
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
