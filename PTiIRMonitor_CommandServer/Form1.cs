using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using Peiport_commandManegerSystem;
namespace Peiport_commandManegerSystem
{
    public partial class Form1 : Form
    {
        public Server g_myServer;
        public MonitorServer C_myServer;
        public JsonUtils m_ClJsonCtrl = new JsonUtils();
        public JsonMonitor m_MOJsonCtrl = new JsonMonitor();
        public List<ListenClient> lstClient = new List<ListenClient>();//客户端
        public List<ListMonitor> listMonitor = new List<ListMonitor>();//监控头
        bool blReceiMsg = false;
        public string strMessageDispBuf = "";
        public string strMonitorDisBuf = "";
        public List<clsCmdServerOpt.stuConnectImf> ClientTab = new List<clsCmdServerOpt.stuConnectImf>();
        public clsCmdServerOpt mGuiSysOppt = new clsCmdServerOpt();
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr wnd, int msg, IntPtr wP, IntPtr lP);
        ProcessStartInfo psInfo = new ProcessStartInfo(@"E:\IR\变电站无人值守系统\变电站无人值守系统\PTiIRMonitor_MonitorManagerApp\bin\Debug\PTiIRMonitor_MonitorManagerApp.exe");
        /// <summary>
        /// //////////////////////////////////////////////////////////
        /// </summary>
        public void guiFormChangeDeal()
        {
            int intHeight, intWidth;
            intHeight = this.Height;
            intWidth = this.Width;
            if (intHeight < 600)
                intHeight = 600;
            if (intWidth < 800)
                intWidth = 800;
            /////////////////////////////////////////
            //调整两个groupBox
            gpbServerForUser.Width = intWidth - gpbServerForUser.Left - 30;
            gpbServerForUser.Height = intHeight / 2 - gpbServerForUser.Top - 30;
            gpbServerForMonitor.Top = gpbServerForUser.Top + gpbServerForUser.Height + 10;
            gpbServerForMonitor.Width = gpbServerForUser.Width;
            gpbServerForMonitor.Height = intHeight - gpbServerForMonitor.Top - 50;
            //调整gpbUserServer 内部
            txbUserServerSendMsgBuf.Width = gpbServerForUser.Width - txbUserServerSendMsgBuf.Left - 20;
            btnUserServerSendMsg.Left = txbUserServerSendMsgBuf.Left + txbUserServerSendMsgBuf.Width - btnUserServerSendMsg.Width;
            btnClsUserServerReceiDisp.Left = btnUserServerSendMsg.Left - btnClsUserServerReceiDisp.Width - 20;
            lsbUserServerConnecClienttTab.Height = gpbServerForUser.Height - lsbUserServerConnecClienttTab.Top - 20;
            txbUserServerReceiMesg.Top = lsbUserServerConnecClienttTab.Top;
            txbUserServerReceiMesg.Width = gpbServerForUser.Width - txbUserServerReceiMesg.Left - 20;
            txbUserServerReceiMesg.Height = gpbServerForUser.Height - txbUserServerReceiMesg.Top - 20;

            //调整gpbMonitorServer内部
            txbMonitorServerSendMsgBuf.Width = gpbServerForMonitor.Width - txbMonitorServerSendMsgBuf.Left - 20;
            btnMonitorServerSendMsg.Left = txbMonitorServerSendMsgBuf.Left + txbMonitorServerSendMsgBuf.Width - btnMonitorServerSendMsg.Width;
            btnClsMonitorServerReceiDisp.Left = btnMonitorServerSendMsg.Left - btnClsMonitorServerReceiDisp.Width - 20;

            lsbMonitorServerConnecClienttTab.Height = gpbServerForMonitor.Height - lsbMonitorServerConnecClienttTab.Top - 20;
            txbMonitorServerReceiMesg.Top = lsbMonitorServerConnecClienttTab.Top;
            txbMonitorServerReceiMesg.Width = gpbServerForMonitor.Width - txbMonitorServerReceiMesg.Left - 20;
            txbMonitorServerReceiMesg.Height = gpbServerForMonitor.Height - txbMonitorServerReceiMesg.Top - 20;
        }
        public Form1()
        {
            InitializeComponent();
            mGuiSysOppt.frmThis = this;
            m_ClJsonCtrl.frmMain = this;
            m_MOJsonCtrl.frmMainMo = this;
            IPAddress ip;
            string s = "192.168.31.186";
            int i = 11574;
            int n = 11573;
            if (IPAddress.TryParse(s, out ip) == false)
                g_myServer = new Server(ip, i);
            mGuiSysOppt.frmThis.g_myServer = new Server(ip, i);
            C_myServer = new MonitorServer(ip, n);
        }
        private void btnSetupServer_Click(object sender, EventArgs e)//启动服务器_客户端
        {
            int intport;
            IPAddress ip;
            string str1;
            str1 = "192.168.31.186";
            intport = 11574;
            str1 = txbUserServerIP.Text;
            if (IPAddress.TryParse(txbUserServerIP.Text, out ip) == false)
            {
                MessageBox.Show("ip错误，请重新输入");
                return;
            }
            int.TryParse(txbUserServerport.Text, out intport);
            if (intport > 0)
            {
                clsOneMonDevOptFun.UpdateIniBackServe_Client(str1, intport);//写入ini配置文件
            }
            else
            {
                MessageBox.Show("启动失败");
                return;
            }
            mGuiSysOppt.funEnterUserServerPar(ip.ToString(), intport);
            mGuiSysOppt.funSetupUserServer();
        }

        private void btnStopServer_Click(object sender, EventArgs e)//停止服务器运行_客户端
        {
            mGuiSysOppt.funStopUserServer();
        }

        private void btnStatusServer_Click(object sender, EventArgs e)//查询状态_客户端
        {
            int iStatus;
            mGuiSysOppt.funQuestUserServerStatus(out iStatus);
            if (iStatus == 0)
            {
                lbUserSeverWorkState.Text = "停止";
            }
            else if (iStatus == 1)
            {
                lbUserSeverWorkState.Text = "启动成功";
            }
            else
            {
                lbUserSeverWorkState.Text = "出错";
            }

        }

        private void butbreak_Click(object sender, EventArgs e)//断开_客户端
        {
            string str1;
            int int2;
            if (lsbUserServerConnecClienttTab.Items.Count == 0)
                return;
            int int1 = -1;
            if (lsbUserServerConnecClienttTab.SelectedIndex != null)
                int1 = lsbUserServerConnecClienttTab.SelectedIndex;
            lstClient = g_myServer.GetClientList();
            for (int i = 0; i < lstClient.Count; i++)
            {
                str1 = lstClient[i].ipaEndIP.ToString();
                int2 = lstClient[i].intEndPort;
                if ((int1 < g_myServer.g_lstConnectClientTab.Count) && (int1 >= 0) && (str1 != "") && (int2 != ' '))
                {
                    mGuiSysOppt.funForceDisOneUserConnectFo(str1, int2, int1);
                    MessageBox.Show("断开成功");
                    butrenovate_Click(sender, e);//刷新连接
                }
                else
                {
                    MessageBox.Show("断开失败");
                }
                return;
            }
        }

        private void butrenovate_Click(object sender, EventArgs e)//刷新连接_客户端
        {
            string str1;
            int i;
            lsbUserServerConnecClienttTab.Items.Clear();
            if (g_myServer == null)
                return;
            lstClient = g_myServer.GetClientList();

            if (lstClient.Count > 0)
            {
                for (i = 0; i < lstClient.Count; i++)
                {
                    str1 = lstClient[i].ipaEndIP.ToString() + "_" + lstClient[i].intEndPort.ToString() + "_Name：" + lstClient[i].strLoginUserName + "_" + lstClient[i].dtUpateLoadTime.ToString();

                    lsbUserServerConnecClienttTab.Items.Add(str1);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)//查询状态_客户端
        {
            int iStatus;
            mGuiSysOppt.funQuestUserServerStatus(out iStatus);
            if (iStatus == 0)
            {
                lbUserSeverWorkState.Text = "停止";
            }
            else if (iStatus == 1)
            {
                lbUserSeverWorkState.Text = "启动成功";
            }
            else
            {
                lbUserSeverWorkState.Text = "出错";
                return;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)//定时接收信息_客户端
        {
        }

        private void btnSendMsg_Click(object sender, EventArgs e)//发送信息_客户端
        {
            string str1, strTo;
            // string strFrom = "CmdServer";
            str1 = txbUserServerSendMsgBuf.Text;
            if (str1 == "")
                return;
            if (lsbUserServerConnecClienttTab.Items.Count == 0)
                return;
            int int1 = -1;
            if (lsbUserServerConnecClienttTab.SelectedIndex != null)
                int1 = lsbUserServerConnecClienttTab.SelectedIndex;
            if ((int1 < g_myServer.g_lstConnectClientTab.Count) && (int1 >= 0))
            {
                strTo = g_myServer.g_lstConnectClientTab[0].strLoginUserName;
                //mGuiSysOppt.funSendToUserOneCmd(strFrom, strTo, str1);
                mGuiSysOppt.sendOneClientMsg(int1, str1);
                string str2;
                str2 = "发送到 IP:" + g_myServer.g_lstConnectClientTab[int1].ipaEndIP.ToString() + "端口：" + g_myServer.g_lstConnectClientTab[int1].intEndPort.ToString() + "信息:" + str1 + "\r\n";
                strMessageDispBuf = strMessageDispBuf + str2;
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //////////////////
            //发送信息给看门狗,发送1表示窗口已经打开，发送2表示客户端命令服务器已经开启，发送3........监控端开启..............
            Process[] pcs = Process.GetProcesses();
            foreach (Process p in pcs)
            {
                if (p.ProcessName == "PTiIRMonitor_MonitorManagerApp")
                {
                    p.StartInfo = psInfo;
                    IntPtr hWnd = p.MainWindowHandle; //获取看门狗.exe主窗口句柄
                    int data = Convert.ToInt32(1); //发送1,表示窗口已经打开
                    SendMessage(hWnd, 0x0100, (IntPtr)data, (IntPtr)0); //发送消息
                }
            }
            //////////////////
            m_ClJsonCtrl.clsClJsonCrcVarInit();
            guiFormChangeDeal();


            

        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            guiFormChangeDeal();
        }

        private void tmrOneSecondScan_Tick(object sender, EventArgs e)//
        {
            mGuiSysOppt.funUserServerSecondScan();
            mGuiSysOppt.funMonitorServerSecondScan();
        }

        private void tmrScanReceiCmd_Tick(object sender, EventArgs e)//定时接收命令
        {
            mGuiSysOppt.funUserServerReceiCmdDealScan();
            mGuiSysOppt.funMonitorServerReceiCmdDealScan();
        }

        private void btnClsUserServerReceiDisp_Click(object sender, EventArgs e)//刷新_客户端
        {
            txbUserServerReceiMesg.Text = "";
        }

        private void timer3_Tick(object sender, EventArgs e)  //定时接收信息_客户端
        {
            if (strMessageDispBuf.Length > 0)//客户
            {
                if (txbUserServerReceiMesg.Text.Length > 2000)
                    txbUserServerReceiMesg.Text = "";
                txbUserServerReceiMesg.Text = txbUserServerReceiMesg.Text + strMessageDispBuf;
                strMessageDispBuf = "";
            }
            if (strMonitorDisBuf.Length > 0)//监控
            {
                if (txbMonitorServerReceiMesg.Text.Length > 2000)

                    txbMonitorServerReceiMesg.Text = "";
                txbMonitorServerReceiMesg.Text = txbMonitorServerReceiMesg.Text + strMonitorDisBuf;
                strMonitorDisBuf = "";
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnSetupMonitorServer_Click(object sender, EventArgs e)
        {
            int intport;
            IPAddress ip;
            string str1;
            str1 = "";
            intport = 11573;
            str1 = txbMonitorServerIP.Text;
            if (IPAddress.TryParse(txbMonitorServerIP.Text, out ip) == false)
            {
                MessageBox.Show("ip错误，请重新输入");
                return;
            }
            int.TryParse(txbMonitorServerPort.Text, out intport);
            if (intport > 0)
            {
                clsOneMonDevOptFun.UpdateIniBackServe_Monitor(str1, intport);//写入ini配置文件
            }
            else
            {
                MessageBox.Show("启动失败");
                return;
            }
            mGuiSysOppt.funEnterMonitorServerPar(ip.ToString(), intport);
            mGuiSysOppt.funSetupMonitorServer();
        }

        private void btnGetMonitorServerStatus_Click(object sender, EventArgs e)//监控端
        {
            int iStatus;
            mGuiSysOppt.funQuestMonitorServerStatus(out iStatus);
            if (iStatus == 0)
            {
                lbMonitorServerStatus.Text = "停止";
            }
            else if (iStatus == 1)
            {
                lbMonitorServerStatus.Text = "启动成功";
            }
            else
            {
                lbMonitorServerStatus.Text = "出错";
            }
        }
        private void Actrenovate_Click(object sender, EventArgs e)//监控端刷新
        {
            string str1;
            int i;
            lsbMonitorServerConnecClienttTab.Items.Clear();
            if (C_myServer == null)
                return;
            listMonitor = C_myServer.GetClientList();
            if (listMonitor.Count > 0)
            {
                for (i = 0; i < listMonitor.Count; i++)
                {
                    str1 = listMonitor[i].ipaEndIP.ToString() + "_" + listMonitor[i].intEndPort.ToString() + "_Name：" + listMonitor[i].strLoginUserName + "_" + listMonitor[i].dtUpateLoadTime.ToString();

                    lsbMonitorServerConnecClienttTab.Items.Add(str1);
                }
            }
        }
        private void btnStopMonitorServer_Click(object sender, EventArgs e)
        {
            mGuiSysOppt.funStopMonitorServer();
        }

        private void btnMonitorServerSendMsg_Click(object sender, EventArgs e)//发送信息到监控端
        {

            string str1, strTo;
            string strFrom = "CmdServer";
            str1 = txbMonitorServerSendMsgBuf.Text;
            if (str1 == "")
                return;
            if (lsbMonitorServerConnecClienttTab.Items.Count == 0)
                return;
            int int1 = -1;
            if (lsbMonitorServerConnecClienttTab.SelectedIndex != null)
                int1 = lsbMonitorServerConnecClienttTab.SelectedIndex;
            if ((int1 < C_myServer.g_lstConnectMonitorTab.Count) && (int1 >= 0))
            {
                mGuiSysOppt.SendOneMonitorMsg(int1, str1);
                string str2;
                str2 = "发送到 IP:" + C_myServer.g_lstConnectMonitorTab[int1].ipaEndIP.ToString() + "端口：" + C_myServer.g_lstConnectMonitorTab[int1].intEndPort.ToString() + "信息:" + str1 + "\r\n";
                txbMonitorServerReceiMesg.Text = str2;
            }
        }

        private void btnClsMonitorServerReceiDisp_Click(object sender, EventArgs e)
        {
            txbMonitorServerReceiMesg.Text = "";
        }

        private void btnForceDisconnectOneMonitor_Click(object sender, EventArgs e)
        {
            string str1;
            int int2;
            if (lsbMonitorServerConnecClienttTab.Items.Count == 0)
                return;
            int int1 = -1;
            if (lsbMonitorServerConnecClienttTab != null)
                int1 = lsbMonitorServerConnecClienttTab.SelectedIndex;
            listMonitor = C_myServer.GetClientList();
            for (int i = 0; i < listMonitor.Count; i++)
            {
                str1 = listMonitor[i].ipaEndIP.ToString();
                int2 = listMonitor[i].intEndPort;
                if ((int1 < C_myServer.g_lstConnectMonitorTab.Count) && (int1 >= 0) && (str1 != "") && (int2 != ' '))
                {
                    mGuiSysOppt.funForceDisOneMonitorConnectFo(str1, int2, int1);
                    MessageBox.Show("断开成功");
                    Actrenovate_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("断开失败");
                }
                return;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("你确定要关闭吗！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                Process[] pcs = Process.GetProcesses();
                foreach (Process p in pcs)
                {
                    if (p.ProcessName == "PTiIRMonitor_MonitorManagerApp")
                    {
                        p.StartInfo = psInfo;
                        IntPtr hWnd = p.MainWindowHandle; //获取看门狗.exe主窗口句柄
                        int data = Convert.ToInt32(2); //发送2，表示窗口已经关闭
                        SendMessage(hWnd, 0x0100, (IntPtr)data, (IntPtr)0); //发送消息
                    }
                }
                e.Cancel = false;  //点击OK   
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
