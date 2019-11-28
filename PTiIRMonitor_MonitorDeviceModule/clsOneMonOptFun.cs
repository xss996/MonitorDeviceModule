using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using System.Threading;
namespace Peiport_pofessionalMonitorDeviceClient
{
    public class clsOneMonOptFun
    {
        int inID;

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr wnd, int msg, IntPtr wP, IntPtr lP);

        public static void UpdateIniBackServe_Client(string strip, int intPort)//写入
        {
            bool blFlag;
            string strSection, strKey;
            string strFileName;
            strFileName = "\\Monitor" + GlbMonitorVar.g_intMonDevIniIndex.ToString("00") + ".ini";

            clsINIFileOP op = new clsINIFileOP(Application.StartupPath + strFileName);
            if (op.bFileExist)
            {
                try
                {
                    //写入
                    strSection = "CmdServerToMonitor";
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

        public int funIninumber_ID()//启动监控头
        {
            clsINIFileOP op = new clsINIFileOP(Application.StartupPath + "\\Monitor.ini");
          //  ProcessStartInfo psInfo = new ProcessStartInfo(@"E:\IR\变电站无人值守系统\变电站无人值守系统\PTiIRMonitor_MonitorManagerApp\bin\Debug\PTiIRMonitor_MonitorManagerApp.exe");

            try
            {
                Thread.Sleep(4000);
                if ((1 == Convert.ToInt32(op.ReadKeyValue("Device1", "DeviceNumber1"))) && (0 == Convert.ToInt32(op.ReadKeyValue("Device1", "DeviceCondition1"))))//判断一号的监控头是否已经启动
                {
                    op.WriteKeyValue("Device1", "DeviceCondition1", 1.ToString());//给一号监控头写入状态1表示在线0表示下线
                    //Process[] pcs = Process.GetProcesses();
                    //foreach (Process p in pcs)
                    //{
                    //    if (p.ProcessName == "PTiIRMonitor_MonitorManagerApp")
                    //    {
                    //        p.StartInfo = psInfo;
                    //        IntPtr hWnd = p.MainWindowHandle; //获取看门狗.exe主窗口句柄
                    //        int data = Convert.ToInt32(11); //发送11,表示窗口已经打开
                    //        SendMessage(hWnd, 0x0100, (IntPtr)data, (IntPtr)0); //发送消息
                            inID = 1;
                    //        Thread.Sleep(4000);
                    //    }
                    //}
                    return 1;
                }
                else if ((2 == Convert.ToInt32(op.ReadKeyValue("Device1", "DeviceNumber2"))) && (0 == Convert.ToInt32(op.ReadKeyValue("Device1", "DeviceCondition2"))))
                {
                    op.WriteKeyValue("Device1", "DeviceCondition2", 1.ToString());//给二号监控头写入状态1表示在线0表示下线
                    //Process[] pcs = Process.GetProcesses();
                    //foreach (Process p in pcs)
                    //{
                    //    if (p.ProcessName == "PTiIRMonitor_MonitorManagerApp")
                    //    {
                    //        p.StartInfo = psInfo;
                    //        IntPtr hWnd = p.MainWindowHandle; //获取看门狗.exe主窗口句柄
                    //        int data = Convert.ToInt32(13); //发送13,表示二号窗口已经打开
                    //        SendMessage(hWnd, 0x0100, (IntPtr)data, (IntPtr)0); //发送消息
                            inID = 2;
                    //        Thread.Sleep(4000);
                    //    }
                    //}
                    return 2;
                }
                else if ((3 == Convert.ToInt32(op.ReadKeyValue("Device1", "DeviceNumber3"))) && (0 == Convert.ToInt32(op.ReadKeyValue("Device1", "DeviceCondition3"))))
                {
                    op.WriteKeyValue("Device1", "DeviceCondition3", 1.ToString());//给三号监控头写入状态1表示在线0表示下线
                    //Process[] pcs = Process.GetProcesses();
                    //foreach (Process p in pcs)
                    //{
                    //    if (p.ProcessName == "PTiIRMonitor_MonitorManagerApp")
                    //    {
                    //        p.StartInfo = psInfo;
                    //        IntPtr hWnd = p.MainWindowHandle; //获取看门狗.exe主窗口句柄
                    //        int data = Convert.ToInt32(15); //发送15,表示窗口已经打开
                    //        SendMessage(hWnd, 0x0100, (IntPtr)data, (IntPtr)0); //发送消息
                            inID = 3;
                    //        Thread.Sleep(4000);
                    //    }
                    //}
                    return 3;
                }
                else if ((4 == Convert.ToInt32(op.ReadKeyValue("Device1", "DeviceNumber4"))) && (0 == Convert.ToInt32(op.ReadKeyValue("Device1", "DeviceCondition4"))))
                {
                    op.WriteKeyValue("Device1", "DeviceCondition4", 1.ToString());//给四号监控头写入状态1表示在线 0表示下线
                    //Process[] pcs = Process.GetProcesses();
                    //foreach (Process p in pcs)
                    //{
                    //    if (p.ProcessName == "PTiIRMonitor_MonitorManagerApp")
                    //    {

                    //        p.StartInfo = psInfo;
                    //        IntPtr hWnd = p.MainWindowHandle; //获取看门狗.exe主窗口句柄
                    //        int data = Convert.ToInt32(17); //发送17,表示窗口已经打开
                    //        SendMessage(hWnd, 0x0100, (IntPtr)data, (IntPtr)0); //发送消息
                            inID = 4;
                    //        Thread.Sleep(4000);
                    //    }
                    //}
                    return 4;
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void funIniCondition()//监控头关闭，将自己的状态修改为0
        {
            clsINIFileOP op = new clsINIFileOP(Application.StartupPath + "\\Monitor.ini");
            ProcessStartInfo psInfo = new ProcessStartInfo(@"E:\IR\变电站无人值守系统\变电站无人值守系统\PTiIRMonitor_MonitorManagerApp\bin\Debug\PTiIRMonitor_MonitorManagerApp.exe");
            if (inID == 1)//一号监控头发送关闭信息
            {
                op.WriteKeyValue("Device1", "DeviceCondition1", 0.ToString());//写入一号监控头状态，0停止
                //Process[] pcs = Process.GetProcesses();
                //foreach (Process p in pcs)
                //{
                //    if (p.ProcessName == "PTiIRMonitor_MonitorManagerApp")
                //    {
                //        p.StartInfo = psInfo;
                //        IntPtr hWnd = p.MainWindowHandle; //获取看门狗.exe主窗口句柄
                //        int data = Convert.ToInt32(12); //发送12,表示已经停止
                //        SendMessage(hWnd, 0x0100, (IntPtr)data, (IntPtr)0); //发送消息
                //    }
                //}
            }
            else if (inID == 2)
            {
                op.WriteKeyValue("Device1", "DeviceCondition2", 0.ToString());//写入一号监控头状态，0停止
                //Process[] pcs = Process.GetProcesses();
                //foreach (Process p in pcs)
                //{
                //    if (p.ProcessName == "PTiIRMonitor_MonitorManagerApp")
                //    {
                //        p.StartInfo = psInfo;
                //        IntPtr hWnd = p.MainWindowHandle; //获取看门狗.exe主窗口句柄
                //        int data = Convert.ToInt32(14); //发送14,表示二已经停止
                //        SendMessage(hWnd, 0x0100, (IntPtr)data, (IntPtr)0); //发送消息
                //    }
                //}
            }
            else if (inID == 3)
            {
                op.WriteKeyValue("Device1", "DeviceCondition3", 0.ToString());//写入一号监控头状态，0停止
                //Process[] pcs = Process.GetProcesses();
                //foreach (Process p in pcs)
                //{
                //    if (p.ProcessName == "PTiIRMonitor_MonitorManagerApp")
                //    {
                //        p.StartInfo = psInfo;
                //        IntPtr hWnd = p.MainWindowHandle; //获取看门狗.exe主窗口句柄
                //        int data = Convert.ToInt32(16); //发送14,表示二已经停止
                //        SendMessage(hWnd, 0x0100, (IntPtr)data, (IntPtr)0); //发送消息
                //    }
                //}
            }
            else if (inID == 4)
            {
                op.WriteKeyValue("Device1", "DeviceCondition4", 0.ToString());//写入一号监控头状态，0停止
                //Process[] pcs = Process.GetProcesses();
                //foreach (Process p in pcs)
                //{
                //    if (p.ProcessName == "PTiIRMonitor_MonitorManagerApp")
                //    {
                //        p.StartInfo = psInfo;
                //        IntPtr hWnd = p.MainWindowHandle; //获取看门狗.exe主窗口句柄
                //        int data = Convert.ToInt32(18); //发送14,表示二已经停止
                //        SendMessage(hWnd, 0x0100, (IntPtr)data, (IntPtr)0); //发送消息
                //    }
                //}
            }
        }
        //定时发送心跳,相隔60秒发送一下，
        public void funMonitorSecondScan()
        {
            ProcessStartInfo psInfo = new ProcessStartInfo(@"E:\IR\变电站无人值守系统\变电站无人值守系统\PTiIRMonitor_MonitorManagerApp\bin\Debug\PTiIRMonitor_MonitorManagerApp.exe");
            if (inID == 1)
            {
                //Process[] pcs = Process.GetProcesses();
                //foreach (Process p in pcs)
                //{
                //    if (p.ProcessName == "PTiIRMonitor_MonitorManagerApp")
                //    {

                //        p.StartInfo = psInfo;
                //        IntPtr hWnd = p.MainWindowHandle; //获取看门狗.exe主窗口句柄
                //        int data = Convert.ToInt32(11); //发送11,表示一号窗口已经打开
                //        SendMessage(hWnd, 0x0100, (IntPtr)data, (IntPtr)0); //发送消息
                //    }
                //}
            }
            else if (inID == 2)
            {

            }
            else if (inID == 3)
            {

            }
            else if (inID == 4)
            {

            }

        }
    }
}
