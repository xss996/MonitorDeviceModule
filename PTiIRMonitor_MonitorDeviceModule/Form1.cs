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
using Newtonsoft;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using PTiIRMonitor_MonitorDeviceModule.util;
using PTiIRMonitor_MonitorDeviceModule.constant;

namespace Peiport_pofessionalMonitorDeviceClient
{
    public partial class Form1 : Form
    {
        public string strMessageDispBuf = "";
        clsOneMonOptFun Fun = new clsOneMonOptFun();
        public clsCmdMonitorClientOpt M_ClientOpt = new clsCmdMonitorClientOpt();

      
        public Form1()                                          
        {
            InitializeComponent();
            M_ClientOpt.Pjson.frmMain = this;
            M_ClientOpt.frmThis = this;
            M_ClientOpt.DevMonitorInit();

            
            string server_ip = INIUtil.Read("Server", "ip", Constant.IniFilePath);
            string server_port = INIUtil.Read("Server", "port", Constant.IniFilePath);
            string username = INIUtil.Read("USER", "username", Constant.IniFilePath);
            string password = INIUtil.Read("USER", "password", Constant.IniFilePath);

            if (!string.IsNullOrEmpty(server_ip) && !string.IsNullOrEmpty(server_port))
            {
                //1.检查网络连接状态,是否能ping 通服务器
                if (TelnetUtil.PingIpOrDomainName(server_ip))
                {
                    //2.连接到服务器
                    M_ClientOpt.funEnterMonitorServerPar(server_ip, Convert.ToInt32(server_port));
                    M_ClientOpt.funSetupMonitorServer();

                    M_ClientOpt.funbutLoginStatus();
                    bool f = M_ClientOpt.bl_UserClientControlSetup;
                    if (M_ClientOpt.bl_UserClientControlSetup)
                    {
                        lbConnectState.Text = "已连接";
                        //3.服务器连接成功后,用户登录
                        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                        {
                            if (!M_ClientOpt.LoginStatus)
                            {
                                M_ClientOpt.funOptbtuUser(username, password);
                            }
                        }
                        else
                        {
                            MessageBox.Show("ini文件读取失败,请检查...", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        lbConnectState.Text = "未连接";
                    }
                }
                else
                {
                    MessageBox.Show("请检查网络,服务器ip和监控端ip是否在同一网关","提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }   
            }
            else
            {
                MessageBox.Show("ini文件读取失败,请检查...", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
           

            Thread thread_server = new Thread(ProxyScan);   //服务器
            thread_server.Start();

            Thread thread_heatbeat = new Thread(M_ClientOpt.SendHeartBeatCmd);
            thread_heatbeat.Start();

            Thread thread_monDev_IR = new Thread(M_ClientOpt.IRStatusScan);    //红外状态监控
            thread_monDev_IR.Start();

            Thread thread_monDev_TV = new Thread(M_ClientOpt.TVStatusScan);    //可见光状态监控
            thread_monDev_TV.Start();

            Thread thread_monDev_FTP = new Thread(M_ClientOpt.FtpStatusScan);   //ftp服务器状态监控
            thread_monDev_FTP.Start();

            Thread thread_monDev_SQL = new Thread(M_ClientOpt.SqlStatusScan);  //数据库状态监控
            thread_monDev_SQL.Start();

            //Thread thread_cruise = new Thread(M_ClientOpt.CruiseStatusScan);    //巡检
            //thread_cruise.Start();

            
            

        }

        public void GetLoginStatus()
        {
            if (M_ClientOpt.bl_UserClientControlSetup)
            {
                M_ClientOpt.funbutLoginStatus();
                if (M_ClientOpt.LoginStatus)
                {
                  //  lb_Status_LoginStatus_Disp.Text = "已登录";
                }
                else
                {
                  //  lb_Status_LoginStatus_Disp.Text = "未登录";
                }
            }
            //else
            //{
            //    MessageBox.Show("连接已经断开，请重新连接后登录!!!");
            //}
        }
        private void button2_Click(object sender, EventArgs e)//启动监控端,连接服务器
        {
            //int intport;
            //IPAddress ipaIP;
            //if (!int.TryParse(txbServerPort.Text, out intport)|| !IPAddress.TryParse(txbServerIP.Text, out ipaIP))
            //{
            //    return;

            //}
            //M_ClientOpt.funEnterMonitorServerPar(ipaIP.ToString(), intport);
            //M_ClientOpt.funSetupMonitorServer();
            //M_ClientOpt.funbutLoginStatus();

            //if (M_ClientOpt.bl_UserClientControlSetup)
            //{

            //    INIUtil.Write("Server", "ip", ipaIP.ToString(), Constant.IniFilePath);
            //    INIUtil.Write("Server", "port", intport.ToString(), Constant.IniFilePath);
            //    Server_timer.Enabled = true;
            //    lbConnectState.Text = "已连接";
            //}
            //else
            //{
            //    lbConnectState.Text = "未连接";
            //}
            


        }
        private void button1_Click(object sender, EventArgs e)//断开服务器连接
        {
             M_ClientOpt.funStopMonitorServer();
           
        }

        private void button32_Click(object sender, EventArgs e)  //查看连接状态
        {
            //M_ClientOpt.funbutLoginStatus();
            //if (M_ClientOpt.bl_UserClientControlSetup)
            //{
            //    lbConnectState.Text = "已连接";
            //}
            //else
            //{
            //    lbConnectState.Text = "未连接";
            //}
        }

        private void butUser_Click(object sender, EventArgs e)           //用户登录
        {
            string str1 = textBox1.Text;
            string str2 = texPassww.Text;
            if (!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2))
            {
                string username = str1;
                string password = str2;
                if (M_ClientOpt.bl_UserClientControlSetup)
                {
                    if (!M_ClientOpt.LoginStatus)
                    {
                        M_ClientOpt.funOptbtuUser(username, password);
                    }

                }
                else
                {
                    MessageBox.Show("服务器尚未连接");
                }
            }
        }

        private void button33_Click(object sender, EventArgs e)      //退出用户登录
        {
            if (M_ClientOpt.LoginStatus)
            {
                string str = textBox1.Text;
                if (!string.IsNullOrEmpty(str))
                {
                    M_ClientOpt.funboutOptOut(str);
                }

            }
            else
            {
                MessageBox.Show("请先登录用户");
            }
        }

        private void button34_Click(object sender, EventArgs e)    //查看用户登录状态
        {
            if (M_ClientOpt.bl_UserClientControlSetup )
            {
                M_ClientOpt.funbutLoginStatus();
                if (M_ClientOpt.LoginStatus )
                { 
                    lb_Status_LoginStatus_Disp.Text = "已登录";    
                }
                else
                {
                    lb_Status_LoginStatus_Disp.Text = "未登录";
                }
            }
            
        }

        private void button35_Click(object sender, EventArgs e)         //启动心跳
        {
            //string struser, str2;
            //struser = textBox1.Text;
            //if (!string.IsNullOrEmpty(struser))
            //{
            //  //  M_ClientOpt.funMonitorServerSecondScan(struser);
            //}
            if (!M_ClientOpt.StartHeartBeat)
            {
                M_ClientOpt.StartHeartBeat = true;
               
            }
        }

        private void button4_Click(object sender, EventArgs e)         //想服务器发送json命令
        {
            string str1;
            str1 = txbSendBuf.Text;//命令
            if (M_ClientOpt.Pjson.m_stuSystemVar.intLoginStatus > 0)
            {
                if (!string.IsNullOrEmpty(str1))
                {
                    M_ClientOpt.funSendToUserOneStrCmd(str1);
                }
                else
                {
                    MessageBox.Show("发送信息不能为空");
                }
            }
            else
            {
                MessageBox.Show("请先登录用户");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str1, str2, str3, str4, str5, str6, str7, str8;
            str1 = DateTime.Now.ToString("yyyyMMddHHmmssff");
            str2 = textBox5.Text;
            str3 = textBox6.Text;
            str4 = textBox7.Text;
            str5 = textBox8.Text;
            str6 = textBox1.Text;
            str7 = textBox3.Text;
            clsCmdMonitorClientOpt.JsonItem json = new clsCmdMonitorClientOpt.JsonItem();
            json.seq = str1;//标识
            json.cmdType = str2;//
            json.cmdAction = str3;
            json.result = str4;
            json.paramList = new List<clsCmdMonitorClientOpt.ParamListItem>();
            json.paramList.Add(new clsCmdMonitorClientOpt.ParamListItem { param = "", value = "" });
            json.sender = str6;
            json.receiver = str7;
            str8 = JsonConvert.SerializeObject(json);
            txbSendBuf.Text = str8;//
        }      //json格式化操作

        private void button5_Click(object sender, EventArgs e)
        {
            txbReceiBuf.Text = "";
        }
        int id;
        private void Form1_Load(object sender, EventArgs e)
        {
           

        }
       
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //判断监控端与服务器断断的状态是否是忙碌，忙碌提示！！
            M_ClientOpt.funCondition();
            //DialogResult dr = MessageBox.Show("提示！", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //if (dr == DialogResult.OK)   //如果单击“是”按钮
            //{

            //    e.Cancel = false;                 //关闭窗体

            //}
            //else if (dr == DialogResult.Cancel)
            //{
            //    e.Cancel = true;                  //不执行操作
            //}
        }
       
        private void button2_Click_1(object sender, EventArgs e)
        {
        }

        int connectServerCount = 0;
       

        public void ProxyScan()
        {
            while (true)
            {
                Thread.Sleep(1500);
                Debug.WriteLine(">>>>>>>>>>>>>>>>>>>服务器状态监控线程:" + Thread.CurrentThread.Name + ";socket状态:" + M_ClientOpt.GetSocketState() + ",服务器连接状态:" + M_ClientOpt.Pjson.OptJsonQuestConnectStatus() + ",用户登录状态:" + M_ClientOpt.GetLoginStatus() + ",心跳检测状态:" + M_ClientOpt.StartHeartBeat);
                if (!M_ClientOpt.GetSocketState())
                {
                    M_ClientOpt.SetLoginStatus();
                    string ip = INIUtil.Read("Server", "ip", Constant.IniFilePath);
                    string port = INIUtil.Read("Server", "port", Constant.IniFilePath);
                    M_ClientOpt.funEnterMonitorServerPar(ip, Convert.ToInt32(port));
                    M_ClientOpt.funSetupMonitorServer();
                    M_ClientOpt.funbutLoginStatus();
                    connectServerCount++;
                    if (connectServerCount > 20)
                    {
                        connectServerCount = 0;
                        MessageBox.Show("重连服务器失败,请检查服务器是否正常", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    connectServerCount = 0;
                }

                if (!M_ClientOpt.LoginStatus)
                {
                    string username = INIUtil.Read("USER", "username", Constant.IniFilePath);
                    string password = INIUtil.Read("USER", "password", Constant.IniFilePath);
                    M_ClientOpt.funOptbtuUser(username, password);
                    M_ClientOpt.funbutLoginStatus();

                    M_ClientOpt.StartHeartBeat = false;
                }
                else
                {
                    M_ClientOpt.StartHeartBeat = true;
                }
                M_ClientOpt.funbutLoginStatus();
            }
           
        }

        private void cmdRevMsg_timer_Tick(object sender, EventArgs e)  //定时接收服务信息
        {
            M_ClientOpt.funMonitorServerReceiCmdDealScan();
        }
    }
}
