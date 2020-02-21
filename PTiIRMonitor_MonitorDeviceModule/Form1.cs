using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.ctrl;
using PTiIRMonitor_MonitorDeviceModule.entities;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

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
            gLogWriter.StartLog();
            M_ClientOpt.Pjson.frmMain = this;
            M_ClientOpt.frmThis = this;
            M_ClientOpt.DevMonitorInit();

            M_ClientOpt.globalCtrl.createCrusieThread();
            M_ClientOpt.globalCtrl.createHeartbeatThread();


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
                        // lbConnectState.Text = "已连接";
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
                        //  lbConnectState.Text = "未连接";
                    }
                }
                else
                {
                    MessageBox.Show("请检查网络,服务器ip和监控端ip是否在同一网关", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("ini文件读取失败,请检查...", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }


            Thread thread_server = new Thread(SocketServerScan);   //服务器
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

        }

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

        int connectServerCount = 0;

        public void SocketServerScan()
        {
            while (true)
            {
                Thread.Sleep(3000);
                Debug.WriteLine(">>>>>>>>>>>>>>>>>>>服务器状态监控线程:" + "服务器连接状态:" + M_ClientOpt.Pjson.OptJsonQuestConnectStatus() + ",用户登录状态:" + M_ClientOpt.GetLoginStatus() + ",心跳检测状态:" + M_ClientOpt.globalCtrl.HeartStatus);
              
                if (!M_ClientOpt.Pjson.OptJsonQuestConnectStatus())
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

        private void revVar_timer_tick(object sender, EventArgs e)
        {
            //服务器相关          
            label_server.Text = M_ClientOpt.Pjson.OptJsonQuestConnectStatus().ToString();
            label_loginStatus.Text = M_ClientOpt.GetLoginStatus().ToString();
            label_heatbeatStatus.Text = M_ClientOpt.globalCtrl.HeartStatus.ToString();          
            label12.Text = string.Format("当前服务器IP:{0},端口号:{1}", INIUtil.Read("Server", "ip", Constant.IniFilePath), INIUtil.Read("Server", "port", Constant.IniFilePath));
            label13.Text = string.Format("当前用户登录信息,用户名:{0},密码:{1}", INIUtil.Read("USER", "username", Constant.IniFilePath), INIUtil.Read("USER", "password", Constant.IniFilePath));

            label_IR.Text = M_ClientOpt.globalCtrl.IRScanState().ToString();
            label14.Text = string.Format("当前红外连接信息,连接IP:{0},端口:{1}", M_ClientOpt.globalCtrl.irCtrl.IP, M_ClientOpt.globalCtrl.irCtrl.port);
            label15.Text = string.Format("当前可见光连接信息,连接IP:{0},端口:{1},通道:{2},连接名:{3},密码:{4}", M_ClientOpt.globalCtrl.tvCtrl.IP, M_ClientOpt.globalCtrl.tvCtrl.port,
              M_ClientOpt.globalCtrl.tvCtrl.channel, M_ClientOpt.globalCtrl.tvCtrl.username, M_ClientOpt.globalCtrl.tvCtrl.password);
            label_FTP.Text = M_ClientOpt.globalCtrl.GetFtpConnectStatus().ToString();
            label_Database.Text = M_ClientOpt.globalCtrl.GetSqlConnectionStatus().ToString();
            label_cruise.Text = M_ClientOpt.globalCtrl.cruiseCtrl.cruiseStatus.ToString();
            int TVState = M_ClientOpt.globalCtrl.TVScanState();
            if (TVState == -1)
            {
                label_TV.Text = "已断线";
            }
            else if (TVState == 0)
            {
                label_TV.Text = "未注册";
            }
            else if (TVState == 1)
            {
                label_TV.Text = "登录未预览";
            }
            else if (TVState == 2)
            {
                label_TV.Text = "登录可预览";
            }


        }

        private void btn_startCruise_Click(object sender, EventArgs e)
        {
            JsonItem jsonObj = new JsonItem();
            jsonObj.seq = DateUtil.DateToString();
            jsonObj.cmdType = 1;
            jsonObj.cmdAction = "CruiseSet";
            jsonObj.sender = "CmdServer";
            jsonObj.receiver = "admin";
            JsonitemParam par1 = new JsonitemParam();
            par1.param = "setUp";
            par1.value = "0";
            jsonObj.paramList.Add(par1);

            M_ClientOpt.globalCtrl.ReceiveMsgScan(JObject.Parse(JsonConvert.SerializeObject(jsonObj)));
        }

        private void btn_stopCruise_Click(object sender, EventArgs e)
        {
            // string strjson ="{ "seq":"201910090236491","cmdType":1,"cmdAction":"CruiseSet","result":"","sender":"CmdServer","receiver":"admin","paramList":[{"param":"setUp","value":"0"}]}";
            JsonItem jsonObj = new JsonItem();
            jsonObj.seq = DateUtil.DateToString();
            jsonObj.cmdType = 1;
            jsonObj.cmdAction = "CruiseSet";
            jsonObj.sender = "CmdServer";
            jsonObj.receiver = "admin";
            JsonitemParam par1 = new JsonitemParam();
            par1.param = "setUp";
            par1.value = "1";
            jsonObj.paramList.Add(par1);

            M_ClientOpt.globalCtrl.ReceiveMsgScan(JObject.Parse(JsonConvert.SerializeObject(jsonObj)));
        }

        private void 红外调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(M_ClientOpt.globalCtrl.cruiseCtrl.cruiseStatus) > -1 && Convert.ToInt32(M_ClientOpt.globalCtrl.cruiseCtrl.cruiseStatus) < 2)
            {
                Hide();
                IRTestForm frm2 = new IRTestForm(this);
                frm2.Show();
            }
            else
            {
                MessageBox.Show("系统正在巡检,请先停止巡检再进行测试");
            }
        }

        private void 可见光调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(M_ClientOpt.globalCtrl.cruiseCtrl.cruiseStatus) > -1 && Convert.ToInt32(M_ClientOpt.globalCtrl.cruiseCtrl.cruiseStatus) < 2)
            {
                Hide();
                TVTestForm frm3 = new TVTestForm(this);
                frm3.Show();
            }
            else
            {
                MessageBox.Show("系统正在巡检,请先停止巡检再进行测试");
            }

        }

        private void btn_updateServerInfo_Click(object sender, EventArgs e)
        {          
            try
            {
                if(INIUtil.Write("Server", "ip", label_serverIP.Text, Constant.IniFilePath)>0 && INIUtil.Write("Server", "port", num_serverPort.Value.ToString(), Constant.IniFilePath) > 0)
                {
                    MessageBox.Show("服务器信息修改成功","提示");
                   
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("服务器信息修改失败,请重试!", "提示");
            }
            
            
        } 

        private void button1_Click(object sender, EventArgs e)
        {
            Position pos = new Position();
            pos.PositionName = pos_name.Text;
            pos.Station_Index = Convert.ToInt32(pos_stationIndex.Value);
            string sql=null;
            List<MySqlParameter> pamList = new List<MySqlParameter>();

            if (pos_cruiseType.Text.ToString().Equals("隔时巡检"))
            {
                pos.CruiseType = 0;
                pos.CruiseInterval = Convert.ToInt32(pos_interval.Value);
                sql = "insert into cruise_position(Station_Index,PositionName,CruiseInterval,CruiseType) values(@Station_Index,@PositionName,@CruiseInterval,@CruiseType)";
                pamList.Add(new MySqlParameter("@Station_Index", pos.Station_Index));
                pamList.Add(new MySqlParameter("@PositionName", pos.PositionName));
                pamList.Add(new MySqlParameter("@CruiseInterval", pos.CruiseInterval));
                pamList.Add(new MySqlParameter("@CruiseType", pos.CruiseType));

            }
            else if (pos_cruiseType.Text.ToString().Equals("定时巡检"))
            {
                pos.CruiseType = 1;
                pos.StartCruiseTime = pos_cruiseStartTime.Value;              
                sql = "insert into cruise_position(Station_Index, PositionName, StartCruiseTime, CruiseType) values(@Station_Index,@PositionName,@StartCruiseTime,@CruiseType)";
                pamList.Add(new MySqlParameter("@Station_Index",pos.Station_Index));
                pamList.Add(new MySqlParameter("@PositionName", pos.PositionName));
                pamList.Add(new MySqlParameter("@StartCruiseTime", pos.StartCruiseTime));
                pamList.Add(new MySqlParameter("@CruiseType", pos.CruiseType));               
            }
            MySqlConnection conn = GlobalCtrl.GetSqlConnection();
            if(SqlHelper.AddData(conn, sql,pamList.ToArray()))
            {
                sql = "select * from cruise_position where Position_Index = (select max(Position_Index) from cruise_position)";
                DataTable data =SqlHelper.QueryData(conn, sql, null);
                
                MessageBox.Show("固定点添加成功");
            }
            else
            {
                MessageBox.Show("固定点添加失败");
            }

            
        }

        int count = 0;
        private void ReceiveCruiseLog_timer_Tick(object sender, EventArgs e)
        {
            if (M_ClientOpt.globalCtrl.cruiseCtrl.cruiseLogMsgs.Count != 0)
            {
                for(var i=0;i< M_ClientOpt.globalCtrl.cruiseCtrl.cruiseLogMsgs.Count; i++)
                {
                    richTextBox1.Text += (M_ClientOpt.globalCtrl.cruiseCtrl.cruiseLogMsgs[0]+"\n");
                    M_ClientOpt.globalCtrl.cruiseCtrl.cruiseLogMsgs.RemoveAt(0);
                    count++;
                }
                Thread.Sleep(200);
                if (count >=25)
                {
                    richTextBox1.Clear();
                    count = 0;
                }
               
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}
