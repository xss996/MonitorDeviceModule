namespace Peiport_commandManegerSystem
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gpbServerForUser = new System.Windows.Forms.GroupBox();
            this.txbUserServerReceiMesg = new System.Windows.Forms.TextBox();
            this.lbUserSeverWorkState = new System.Windows.Forms.Label();
            this.btnGetUserServerStatus = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStopUserServer = new System.Windows.Forms.Button();
            this.txbUserServerport = new System.Windows.Forms.TextBox();
            this.txbUserServerIP = new System.Windows.Forms.TextBox();
            this.btnSetupUserServer = new System.Windows.Forms.Button();
            this.btnUpdateUserConnectStatus = new System.Windows.Forms.Button();
            this.btnForceDisconnectOneUser = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClsUserServerReceiDisp = new System.Windows.Forms.Button();
            this.btnUserServerSendMsg = new System.Windows.Forms.Button();
            this.txbUserServerSendMsgBuf = new System.Windows.Forms.TextBox();
            this.lsbUserServerConnecClienttTab = new System.Windows.Forms.ListBox();
            this.gpbServerForMonitor = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Actrenovate = new System.Windows.Forms.Button();
            this.btnForceDisconnectOneMonitor = new System.Windows.Forms.Button();
            this.lbMonitorServerStatus = new System.Windows.Forms.Label();
            this.btnGetMonitorServerStatus = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btnStopMonitorServer = new System.Windows.Forms.Button();
            this.txbMonitorServerPort = new System.Windows.Forms.TextBox();
            this.txbMonitorServerIP = new System.Windows.Forms.TextBox();
            this.btnSetupMonitorServer = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnClsMonitorServerReceiDisp = new System.Windows.Forms.Button();
            this.btnMonitorServerSendMsg = new System.Windows.Forms.Button();
            this.txbMonitorServerSendMsgBuf = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lsbMonitorServerConnecClienttTab = new System.Windows.Forms.ListBox();
            this.txbMonitorServerReceiMesg = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.tmrOneSecondScan = new System.Windows.Forms.Timer(this.components);
            this.tmrScanReceiCmd = new System.Windows.Forms.Timer(this.components);
            this.gpbServerForUser.SuspendLayout();
            this.gpbServerForMonitor.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpbServerForUser
            // 
            this.gpbServerForUser.Controls.Add(this.txbUserServerReceiMesg);
            this.gpbServerForUser.Controls.Add(this.lbUserSeverWorkState);
            this.gpbServerForUser.Controls.Add(this.btnGetUserServerStatus);
            this.gpbServerForUser.Controls.Add(this.label2);
            this.gpbServerForUser.Controls.Add(this.label1);
            this.gpbServerForUser.Controls.Add(this.btnStopUserServer);
            this.gpbServerForUser.Controls.Add(this.txbUserServerport);
            this.gpbServerForUser.Controls.Add(this.txbUserServerIP);
            this.gpbServerForUser.Controls.Add(this.btnSetupUserServer);
            this.gpbServerForUser.Controls.Add(this.btnUpdateUserConnectStatus);
            this.gpbServerForUser.Controls.Add(this.btnForceDisconnectOneUser);
            this.gpbServerForUser.Controls.Add(this.label6);
            this.gpbServerForUser.Controls.Add(this.label4);
            this.gpbServerForUser.Controls.Add(this.label5);
            this.gpbServerForUser.Controls.Add(this.btnClsUserServerReceiDisp);
            this.gpbServerForUser.Controls.Add(this.btnUserServerSendMsg);
            this.gpbServerForUser.Controls.Add(this.txbUserServerSendMsgBuf);
            this.gpbServerForUser.Controls.Add(this.lsbUserServerConnecClienttTab);
            this.gpbServerForUser.Location = new System.Drawing.Point(9, 7);
            this.gpbServerForUser.Name = "gpbServerForUser";
            this.gpbServerForUser.Size = new System.Drawing.Size(1216, 331);
            this.gpbServerForUser.TabIndex = 91;
            this.gpbServerForUser.TabStop = false;
            this.gpbServerForUser.Text = "服务端";
            // 
            // txbUserServerReceiMesg
            // 
            this.txbUserServerReceiMesg.Location = new System.Drawing.Point(554, 98);
            this.txbUserServerReceiMesg.Multiline = true;
            this.txbUserServerReceiMesg.Name = "txbUserServerReceiMesg";
            this.txbUserServerReceiMesg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbUserServerReceiMesg.Size = new System.Drawing.Size(600, 202);
            this.txbUserServerReceiMesg.TabIndex = 102;
            // 
            // lbUserSeverWorkState
            // 
            this.lbUserSeverWorkState.AutoSize = true;
            this.lbUserSeverWorkState.Location = new System.Drawing.Point(518, 32);
            this.lbUserSeverWorkState.Name = "lbUserSeverWorkState";
            this.lbUserSeverWorkState.Size = new System.Drawing.Size(29, 12);
            this.lbUserSeverWorkState.TabIndex = 101;
            this.lbUserSeverWorkState.Text = "断开";
            // 
            // btnGetUserServerStatus
            // 
            this.btnGetUserServerStatus.Location = new System.Drawing.Point(433, 23);
            this.btnGetUserServerStatus.Name = "btnGetUserServerStatus";
            this.btnGetUserServerStatus.Size = new System.Drawing.Size(79, 29);
            this.btnGetUserServerStatus.TabIndex = 100;
            this.btnGetUserServerStatus.Text = "查看状态";
            this.btnGetUserServerStatus.UseVisualStyleBackColor = true;
            this.btnGetUserServerStatus.Click += new System.EventHandler(this.btnStatusServer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 99;
            this.label2.Text = "端口";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 98;
            this.label1.Text = "IP";
            // 
            // btnStopUserServer
            // 
            this.btnStopUserServer.Location = new System.Drawing.Point(348, 24);
            this.btnStopUserServer.Name = "btnStopUserServer";
            this.btnStopUserServer.Size = new System.Drawing.Size(79, 29);
            this.btnStopUserServer.TabIndex = 97;
            this.btnStopUserServer.Text = "停止";
            this.btnStopUserServer.UseVisualStyleBackColor = true;
            this.btnStopUserServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // txbUserServerport
            // 
            this.txbUserServerport.Location = new System.Drawing.Point(160, 31);
            this.txbUserServerport.Name = "txbUserServerport";
            this.txbUserServerport.Size = new System.Drawing.Size(91, 21);
            this.txbUserServerport.TabIndex = 96;
            this.txbUserServerport.Text = "11574";
            // 
            // txbUserServerIP
            // 
            this.txbUserServerIP.Location = new System.Drawing.Point(15, 31);
            this.txbUserServerIP.Name = "txbUserServerIP";
            this.txbUserServerIP.Size = new System.Drawing.Size(136, 21);
            this.txbUserServerIP.TabIndex = 95;
            this.txbUserServerIP.Text = "192.168.123.128";
            // 
            // btnSetupUserServer
            // 
            this.btnSetupUserServer.Location = new System.Drawing.Point(260, 23);
            this.btnSetupUserServer.Name = "btnSetupUserServer";
            this.btnSetupUserServer.Size = new System.Drawing.Size(79, 29);
            this.btnSetupUserServer.TabIndex = 94;
            this.btnSetupUserServer.Text = "启动";
            this.btnSetupUserServer.UseVisualStyleBackColor = true;
            this.btnSetupUserServer.Click += new System.EventHandler(this.btnSetupServer_Click);
            // 
            // btnUpdateUserConnectStatus
            // 
            this.btnUpdateUserConnectStatus.Location = new System.Drawing.Point(348, 64);
            this.btnUpdateUserConnectStatus.Name = "btnUpdateUserConnectStatus";
            this.btnUpdateUserConnectStatus.Size = new System.Drawing.Size(79, 29);
            this.btnUpdateUserConnectStatus.TabIndex = 93;
            this.btnUpdateUserConnectStatus.Text = "刷新";
            this.btnUpdateUserConnectStatus.UseVisualStyleBackColor = true;
            this.btnUpdateUserConnectStatus.Click += new System.EventHandler(this.butrenovate_Click);
            // 
            // btnForceDisconnectOneUser
            // 
            this.btnForceDisconnectOneUser.Location = new System.Drawing.Point(260, 63);
            this.btnForceDisconnectOneUser.Name = "btnForceDisconnectOneUser";
            this.btnForceDisconnectOneUser.Size = new System.Drawing.Size(79, 29);
            this.btnForceDisconnectOneUser.TabIndex = 92;
            this.btnForceDisconnectOneUser.Text = "断开客户端";
            this.btnForceDisconnectOneUser.UseVisualStyleBackColor = true;
            this.btnForceDisconnectOneUser.Click += new System.EventHandler(this.butbreak_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 91;
            this.label6.Text = "用户在线状况";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(594, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 89;
            this.label4.Text = "发送信息";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(594, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 88;
            this.label5.Text = "接收信息";
            // 
            // btnClsUserServerReceiDisp
            // 
            this.btnClsUserServerReceiDisp.Location = new System.Drawing.Point(970, 65);
            this.btnClsUserServerReceiDisp.Name = "btnClsUserServerReceiDisp";
            this.btnClsUserServerReceiDisp.Size = new System.Drawing.Size(67, 28);
            this.btnClsUserServerReceiDisp.TabIndex = 87;
            this.btnClsUserServerReceiDisp.Text = "清空";
            this.btnClsUserServerReceiDisp.UseVisualStyleBackColor = true;
            this.btnClsUserServerReceiDisp.Click += new System.EventHandler(this.btnClsUserServerReceiDisp_Click);
            // 
            // btnUserServerSendMsg
            // 
            this.btnUserServerSendMsg.Location = new System.Drawing.Point(1087, 65);
            this.btnUserServerSendMsg.Name = "btnUserServerSendMsg";
            this.btnUserServerSendMsg.Size = new System.Drawing.Size(67, 28);
            this.btnUserServerSendMsg.TabIndex = 86;
            this.btnUserServerSendMsg.Text = "发送";
            this.btnUserServerSendMsg.UseVisualStyleBackColor = true;
            this.btnUserServerSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // txbUserServerSendMsgBuf
            // 
            this.txbUserServerSendMsgBuf.Location = new System.Drawing.Point(653, 20);
            this.txbUserServerSendMsgBuf.Multiline = true;
            this.txbUserServerSendMsgBuf.Name = "txbUserServerSendMsgBuf";
            this.txbUserServerSendMsgBuf.Size = new System.Drawing.Size(533, 37);
            this.txbUserServerSendMsgBuf.TabIndex = 85;
            // 
            // lsbUserServerConnecClienttTab
            // 
            this.lsbUserServerConnecClienttTab.FormattingEnabled = true;
            this.lsbUserServerConnecClienttTab.ItemHeight = 12;
            this.lsbUserServerConnecClienttTab.Location = new System.Drawing.Point(18, 99);
            this.lsbUserServerConnecClienttTab.Name = "lsbUserServerConnecClienttTab";
            this.lsbUserServerConnecClienttTab.Size = new System.Drawing.Size(482, 196);
            this.lsbUserServerConnecClienttTab.TabIndex = 84;
            // 
            // gpbServerForMonitor
            // 
            this.gpbServerForMonitor.Controls.Add(this.label3);
            this.gpbServerForMonitor.Controls.Add(this.Actrenovate);
            this.gpbServerForMonitor.Controls.Add(this.btnForceDisconnectOneMonitor);
            this.gpbServerForMonitor.Controls.Add(this.lbMonitorServerStatus);
            this.gpbServerForMonitor.Controls.Add(this.btnGetMonitorServerStatus);
            this.gpbServerForMonitor.Controls.Add(this.label11);
            this.gpbServerForMonitor.Controls.Add(this.btnStopMonitorServer);
            this.gpbServerForMonitor.Controls.Add(this.txbMonitorServerPort);
            this.gpbServerForMonitor.Controls.Add(this.txbMonitorServerIP);
            this.gpbServerForMonitor.Controls.Add(this.btnSetupMonitorServer);
            this.gpbServerForMonitor.Controls.Add(this.label8);
            this.gpbServerForMonitor.Controls.Add(this.label9);
            this.gpbServerForMonitor.Controls.Add(this.btnClsMonitorServerReceiDisp);
            this.gpbServerForMonitor.Controls.Add(this.btnMonitorServerSendMsg);
            this.gpbServerForMonitor.Controls.Add(this.txbMonitorServerSendMsgBuf);
            this.gpbServerForMonitor.Controls.Add(this.label7);
            this.gpbServerForMonitor.Controls.Add(this.lsbMonitorServerConnecClienttTab);
            this.gpbServerForMonitor.Controls.Add(this.txbMonitorServerReceiMesg);
            this.gpbServerForMonitor.Location = new System.Drawing.Point(9, 345);
            this.gpbServerForMonitor.Name = "gpbServerForMonitor";
            this.gpbServerForMonitor.Size = new System.Drawing.Size(1217, 414);
            this.gpbServerForMonitor.TabIndex = 90;
            this.gpbServerForMonitor.TabStop = false;
            this.gpbServerForMonitor.Text = "监控端";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 138;
            this.label3.Text = "IP";
            // 
            // Actrenovate
            // 
            this.Actrenovate.Location = new System.Drawing.Point(348, 76);
            this.Actrenovate.Name = "Actrenovate";
            this.Actrenovate.Size = new System.Drawing.Size(79, 29);
            this.Actrenovate.TabIndex = 137;
            this.Actrenovate.Text = "刷新";
            this.Actrenovate.UseVisualStyleBackColor = true;
            this.Actrenovate.Click += new System.EventHandler(this.Actrenovate_Click);
            // 
            // btnForceDisconnectOneMonitor
            // 
            this.btnForceDisconnectOneMonitor.Location = new System.Drawing.Point(263, 76);
            this.btnForceDisconnectOneMonitor.Name = "btnForceDisconnectOneMonitor";
            this.btnForceDisconnectOneMonitor.Size = new System.Drawing.Size(79, 29);
            this.btnForceDisconnectOneMonitor.TabIndex = 135;
            this.btnForceDisconnectOneMonitor.Text = "断开监控端";
            this.btnForceDisconnectOneMonitor.UseVisualStyleBackColor = true;
            this.btnForceDisconnectOneMonitor.Click += new System.EventHandler(this.btnForceDisconnectOneMonitor_Click);
            // 
            // lbMonitorServerStatus
            // 
            this.lbMonitorServerStatus.AutoSize = true;
            this.lbMonitorServerStatus.Location = new System.Drawing.Point(518, 42);
            this.lbMonitorServerStatus.Name = "lbMonitorServerStatus";
            this.lbMonitorServerStatus.Size = new System.Drawing.Size(29, 12);
            this.lbMonitorServerStatus.TabIndex = 136;
            this.lbMonitorServerStatus.Text = "断开";
            // 
            // btnGetMonitorServerStatus
            // 
            this.btnGetMonitorServerStatus.Location = new System.Drawing.Point(433, 33);
            this.btnGetMonitorServerStatus.Name = "btnGetMonitorServerStatus";
            this.btnGetMonitorServerStatus.Size = new System.Drawing.Size(79, 29);
            this.btnGetMonitorServerStatus.TabIndex = 134;
            this.btnGetMonitorServerStatus.Text = "查看状态";
            this.btnGetMonitorServerStatus.UseVisualStyleBackColor = true;
            this.btnGetMonitorServerStatus.Click += new System.EventHandler(this.btnGetMonitorServerStatus_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(206, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 133;
            this.label11.Text = "端口";
            // 
            // btnStopMonitorServer
            // 
            this.btnStopMonitorServer.Location = new System.Drawing.Point(348, 33);
            this.btnStopMonitorServer.Name = "btnStopMonitorServer";
            this.btnStopMonitorServer.Size = new System.Drawing.Size(79, 29);
            this.btnStopMonitorServer.TabIndex = 132;
            this.btnStopMonitorServer.Text = "停止";
            this.btnStopMonitorServer.UseVisualStyleBackColor = true;
            this.btnStopMonitorServer.Click += new System.EventHandler(this.btnStopMonitorServer_Click);
            // 
            // txbMonitorServerPort
            // 
            this.txbMonitorServerPort.Location = new System.Drawing.Point(164, 34);
            this.txbMonitorServerPort.Name = "txbMonitorServerPort";
            this.txbMonitorServerPort.Size = new System.Drawing.Size(91, 21);
            this.txbMonitorServerPort.TabIndex = 131;
            this.txbMonitorServerPort.Text = "11573";
            // 
            // txbMonitorServerIP
            // 
            this.txbMonitorServerIP.Location = new System.Drawing.Point(23, 33);
            this.txbMonitorServerIP.Name = "txbMonitorServerIP";
            this.txbMonitorServerIP.Size = new System.Drawing.Size(136, 21);
            this.txbMonitorServerIP.TabIndex = 130;
            this.txbMonitorServerIP.Text = "192.168.123.128";
            // 
            // btnSetupMonitorServer
            // 
            this.btnSetupMonitorServer.Location = new System.Drawing.Point(261, 33);
            this.btnSetupMonitorServer.Name = "btnSetupMonitorServer";
            this.btnSetupMonitorServer.Size = new System.Drawing.Size(79, 29);
            this.btnSetupMonitorServer.TabIndex = 129;
            this.btnSetupMonitorServer.Text = "启动";
            this.btnSetupMonitorServer.UseVisualStyleBackColor = true;
            this.btnSetupMonitorServer.Click += new System.EventHandler(this.btnSetupMonitorServer_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(585, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 128;
            this.label8.Text = "发送信息";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(587, 79);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 127;
            this.label9.Text = "接收信息";
            // 
            // btnClsMonitorServerReceiDisp
            // 
            this.btnClsMonitorServerReceiDisp.Location = new System.Drawing.Point(975, 74);
            this.btnClsMonitorServerReceiDisp.Name = "btnClsMonitorServerReceiDisp";
            this.btnClsMonitorServerReceiDisp.Size = new System.Drawing.Size(67, 28);
            this.btnClsMonitorServerReceiDisp.TabIndex = 126;
            this.btnClsMonitorServerReceiDisp.Text = "清空";
            this.btnClsMonitorServerReceiDisp.UseVisualStyleBackColor = true;
            this.btnClsMonitorServerReceiDisp.Click += new System.EventHandler(this.btnClsMonitorServerReceiDisp_Click);
            // 
            // btnMonitorServerSendMsg
            // 
            this.btnMonitorServerSendMsg.Location = new System.Drawing.Point(1088, 71);
            this.btnMonitorServerSendMsg.Name = "btnMonitorServerSendMsg";
            this.btnMonitorServerSendMsg.Size = new System.Drawing.Size(67, 28);
            this.btnMonitorServerSendMsg.TabIndex = 125;
            this.btnMonitorServerSendMsg.Text = "发送";
            this.btnMonitorServerSendMsg.UseVisualStyleBackColor = true;
            this.btnMonitorServerSendMsg.Click += new System.EventHandler(this.btnMonitorServerSendMsg_Click);
            // 
            // txbMonitorServerSendMsgBuf
            // 
            this.txbMonitorServerSendMsgBuf.Location = new System.Drawing.Point(642, 22);
            this.txbMonitorServerSendMsgBuf.Multiline = true;
            this.txbMonitorServerSendMsgBuf.Name = "txbMonitorServerSendMsgBuf";
            this.txbMonitorServerSendMsgBuf.Size = new System.Drawing.Size(544, 43);
            this.txbMonitorServerSendMsgBuf.TabIndex = 124;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 123;
            this.label7.Text = "监控头在线状况";
            // 
            // lsbMonitorServerConnecClienttTab
            // 
            this.lsbMonitorServerConnecClienttTab.FormattingEnabled = true;
            this.lsbMonitorServerConnecClienttTab.ItemHeight = 12;
            this.lsbMonitorServerConnecClienttTab.Location = new System.Drawing.Point(15, 109);
            this.lsbMonitorServerConnecClienttTab.Name = "lsbMonitorServerConnecClienttTab";
            this.lsbMonitorServerConnecClienttTab.Size = new System.Drawing.Size(485, 268);
            this.lsbMonitorServerConnecClienttTab.TabIndex = 121;
            // 
            // txbMonitorServerReceiMesg
            // 
            this.txbMonitorServerReceiMesg.Location = new System.Drawing.Point(547, 109);
            this.txbMonitorServerReceiMesg.Multiline = true;
            this.txbMonitorServerReceiMesg.Name = "txbMonitorServerReceiMesg";
            this.txbMonitorServerReceiMesg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbMonitorServerReceiMesg.Size = new System.Drawing.Size(607, 280);
            this.txbMonitorServerReceiMesg.TabIndex = 122;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 300;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Enabled = true;
            this.timer3.Interval = 600;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // tmrOneSecondScan
            // 
            this.tmrOneSecondScan.Enabled = true;
            this.tmrOneSecondScan.Interval = 1000;
            this.tmrOneSecondScan.Tick += new System.EventHandler(this.tmrOneSecondScan_Tick);
            // 
            // tmrScanReceiCmd
            // 
            this.tmrScanReceiCmd.Enabled = true;
            this.tmrScanReceiCmd.Interval = 300;
            this.tmrScanReceiCmd.Tick += new System.EventHandler(this.tmrScanReceiCmd_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1239, 763);
            this.Controls.Add(this.gpbServerForUser);
            this.Controls.Add(this.gpbServerForMonitor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "红外监控系统命令服务器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.gpbServerForUser.ResumeLayout(false);
            this.gpbServerForUser.PerformLayout();
            this.gpbServerForMonitor.ResumeLayout(false);
            this.gpbServerForMonitor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbServerForUser;
        private System.Windows.Forms.TextBox txbUserServerReceiMesg;
        private System.Windows.Forms.Label lbUserSeverWorkState;
        private System.Windows.Forms.Button btnGetUserServerStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStopUserServer;
        private System.Windows.Forms.TextBox txbUserServerport;
        private System.Windows.Forms.TextBox txbUserServerIP;
        private System.Windows.Forms.Button btnSetupUserServer;
        private System.Windows.Forms.Button btnUpdateUserConnectStatus;
        private System.Windows.Forms.Button btnForceDisconnectOneUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnClsUserServerReceiDisp;
        private System.Windows.Forms.Button btnUserServerSendMsg;
        private System.Windows.Forms.TextBox txbUserServerSendMsgBuf;
        private System.Windows.Forms.ListBox lsbUserServerConnecClienttTab;
        private System.Windows.Forms.GroupBox gpbServerForMonitor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Actrenovate;
        private System.Windows.Forms.Button btnForceDisconnectOneMonitor;
        private System.Windows.Forms.Label lbMonitorServerStatus;
        private System.Windows.Forms.Button btnGetMonitorServerStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnStopMonitorServer;
        private System.Windows.Forms.TextBox txbMonitorServerPort;
        private System.Windows.Forms.TextBox txbMonitorServerIP;
        private System.Windows.Forms.Button btnSetupMonitorServer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnClsMonitorServerReceiDisp;
        private System.Windows.Forms.Button btnMonitorServerSendMsg;
        private System.Windows.Forms.TextBox txbMonitorServerSendMsgBuf;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox lsbMonitorServerConnecClienttTab;
        private System.Windows.Forms.TextBox txbMonitorServerReceiMesg;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Timer tmrOneSecondScan;
        private System.Windows.Forms.Timer tmrScanReceiCmd;
    }
}

