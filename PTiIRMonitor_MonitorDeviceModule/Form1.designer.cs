namespace Peiport_pofessionalMonitorDeviceClient
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txbReceiBuf = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.txbSendBuf = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button35 = new System.Windows.Forms.Button();
            this.button34 = new System.Windows.Forms.Button();
            this.lb_Status_LoginStatus_Disp = new System.Windows.Forms.Label();
            this.button33 = new System.Windows.Forms.Button();
            this.button32 = new System.Windows.Forms.Button();
            this.btn_ConnectServer = new System.Windows.Forms.Button();
            this.txbServerPort = new System.Windows.Forms.TextBox();
            this.txbServerIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.butUser = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lbConnectState = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.texPassww = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdRevMsg_timer = new System.Windows.Forms.Timer(this.components);
            this.showMsg_timer = new System.Windows.Forms.Timer(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.devStatus_timer = new System.Windows.Forms.Timer(this.components);
            this.heartBeat_timer = new System.Windows.Forms.Timer(this.components);
            this.Server_timer = new System.Windows.Forms.Timer(this.components);
            this.cruise_timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.textBox8);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.textBox7);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txbReceiBuf);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.txbSendBuf);
            this.groupBox2.Location = new System.Drawing.Point(19, 164);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1063, 387);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "发送信息";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(450, 34);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(107, 12);
            this.label15.TabIndex = 50;
            this.label15.Text = "返回参数ok和error";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(764, 47);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 34);
            this.button3.TabIndex = 48;
            this.button3.Text = "转换为json格式";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(565, 55);
            this.textBox8.Multiline = true;
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(145, 26);
            this.textBox8.TabIndex = 44;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(601, 34);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 43;
            this.label12.Text = "命令返回参数";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(462, 55);
            this.textBox7.Multiline = true;
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(86, 26);
            this.textBox7.TabIndex = 42;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(376, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 41;
            this.label11.Text = "命令名称";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(357, 55);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(86, 26);
            this.textBox6.TabIndex = 40;
            this.textBox6.Text = "Logout";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(269, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 39;
            this.label10.Text = "命令类别";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(253, 52);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(86, 26);
            this.textBox5.TabIndex = 38;
            this.textBox5.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "监控头";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(15, 37);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 150);
            this.textBox3.TabIndex = 20;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(963, 157);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(87, 34);
            this.button5.TabIndex = 19;
            this.button5.Text = "清空";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "接收命令";
            // 
            // txbReceiBuf
            // 
            this.txbReceiBuf.Location = new System.Drawing.Point(22, 222);
            this.txbReceiBuf.Multiline = true;
            this.txbReceiBuf.Name = "txbReceiBuf";
            this.txbReceiBuf.Size = new System.Drawing.Size(1013, 165);
            this.txbReceiBuf.TabIndex = 18;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(963, 117);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(87, 34);
            this.button4.TabIndex = 17;
            this.button4.Text = "发送";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // txbSendBuf
            // 
            this.txbSendBuf.Location = new System.Drawing.Point(142, 113);
            this.txbSendBuf.Multiline = true;
            this.txbSendBuf.Name = "txbSendBuf";
            this.txbSendBuf.Size = new System.Drawing.Size(814, 86);
            this.txbSendBuf.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button35);
            this.groupBox1.Controls.Add(this.button34);
            this.groupBox1.Controls.Add(this.lb_Status_LoginStatus_Disp);
            this.groupBox1.Controls.Add(this.button33);
            this.groupBox1.Controls.Add(this.button32);
            this.groupBox1.Controls.Add(this.btn_ConnectServer);
            this.groupBox1.Controls.Add(this.txbServerPort);
            this.groupBox1.Controls.Add(this.txbServerIP);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.butUser);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lbConnectState);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.texPassww);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(21, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1063, 128);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "连接";
            // 
            // button35
            // 
            this.button35.Location = new System.Drawing.Point(342, 86);
            this.button35.Name = "button35";
            this.button35.Size = new System.Drawing.Size(87, 31);
            this.button35.TabIndex = 23;
            this.button35.Text = "启动心跳";
            this.button35.UseVisualStyleBackColor = true;
            this.button35.Click += new System.EventHandler(this.button35_Click);
            // 
            // button34
            // 
            this.button34.Location = new System.Drawing.Point(225, 86);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(87, 31);
            this.button34.TabIndex = 22;
            this.button34.Text = "登录状态";
            this.button34.UseVisualStyleBackColor = true;
            this.button34.Click += new System.EventHandler(this.button34_Click);
            // 
            // lb_Status_LoginStatus_Disp
            // 
            this.lb_Status_LoginStatus_Disp.AutoSize = true;
            this.lb_Status_LoginStatus_Disp.Location = new System.Drawing.Point(245, 64);
            this.lb_Status_LoginStatus_Disp.Name = "lb_Status_LoginStatus_Disp";
            this.lb_Status_LoginStatus_Disp.Size = new System.Drawing.Size(41, 12);
            this.lb_Status_LoginStatus_Disp.TabIndex = 21;
            this.lb_Status_LoginStatus_Disp.Text = "label3";
            // 
            // button33
            // 
            this.button33.Location = new System.Drawing.Point(225, 20);
            this.button33.Name = "button33";
            this.button33.Size = new System.Drawing.Size(87, 31);
            this.button33.TabIndex = 20;
            this.button33.Text = "退出";
            this.button33.UseVisualStyleBackColor = true;
            this.button33.Click += new System.EventHandler(this.button33_Click);
            // 
            // button32
            // 
            this.button32.Location = new System.Drawing.Point(847, 77);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(87, 31);
            this.button32.TabIndex = 19;
            this.button32.Text = "查看状态";
            this.button32.UseVisualStyleBackColor = true;
            this.button32.Click += new System.EventHandler(this.button32_Click);
            // 
            // btn_ConnectServer
            // 
            this.btn_ConnectServer.Location = new System.Drawing.Point(754, 59);
            this.btn_ConnectServer.Name = "btn_ConnectServer";
            this.btn_ConnectServer.Size = new System.Drawing.Size(87, 31);
            this.btn_ConnectServer.TabIndex = 18;
            this.btn_ConnectServer.Text = "连接";
            this.btn_ConnectServer.UseVisualStyleBackColor = true;
            this.btn_ConnectServer.Click += new System.EventHandler(this.button2_Click);
            // 
            // txbServerPort
            // 
            this.txbServerPort.Location = new System.Drawing.Point(625, 68);
            this.txbServerPort.Name = "txbServerPort";
            this.txbServerPort.Size = new System.Drawing.Size(123, 21);
            this.txbServerPort.TabIndex = 17;
            this.txbServerPort.Text = "11573";
            // 
            // txbServerIP
            // 
            this.txbServerIP.Location = new System.Drawing.Point(496, 68);
            this.txbServerIP.Name = "txbServerIP";
            this.txbServerIP.Size = new System.Drawing.Size(123, 21);
            this.txbServerIP.TabIndex = 16;
            this.txbServerIP.Text = "192.168.123.128";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(664, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "端口";
            // 
            // butUser
            // 
            this.butUser.Location = new System.Drawing.Point(142, 56);
            this.butUser.Name = "butUser";
            this.butUser.Size = new System.Drawing.Size(87, 31);
            this.butUser.TabIndex = 16;
            this.butUser.Text = "登录";
            this.butUser.UseVisualStyleBackColor = true;
            this.butUser.Click += new System.EventHandler(this.butUser_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(528, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "IP";
            // 
            // lbConnectState
            // 
            this.lbConnectState.AutoSize = true;
            this.lbConnectState.Location = new System.Drawing.Point(950, 65);
            this.lbConnectState.Name = "lbConnectState";
            this.lbConnectState.Size = new System.Drawing.Size(41, 12);
            this.lbConnectState.TabIndex = 13;
            this.lbConnectState.Text = "label3";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(847, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 31);
            this.button1.TabIndex = 11;
            this.button1.Text = "断开";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // texPassww
            // 
            this.texPassww.Location = new System.Drawing.Point(15, 92);
            this.texPassww.Name = "texPassww";
            this.texPassww.Size = new System.Drawing.Size(123, 21);
            this.texPassww.TabIndex = 10;
            this.texPassww.Text = "admin123";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(15, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(123, 21);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "admin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "密码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "账号";
            // 
            // cmdRevMsg_timer
            // 
            this.cmdRevMsg_timer.Enabled = true;
            this.cmdRevMsg_timer.Interval = 300;
            this.cmdRevMsg_timer.Tick += new System.EventHandler(this.cmdRevMsg_timer_Tick);
            // 
            // showMsg_timer
            // 
            this.showMsg_timer.Interval = 600;
            this.showMsg_timer.Tick += new System.EventHandler(this.showMsg_timer_Tick);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(63, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 21);
            this.label7.TabIndex = 12;
            this.label7.Text = "label7";
            // 
            // devStatus_timer
            // 
            this.devStatus_timer.Enabled = true;
            this.devStatus_timer.Interval = 2000;
            this.devStatus_timer.Tick += new System.EventHandler(this.devStatus_timer_Tick);
            // 
            // heartBeat_timer
            // 
            this.heartBeat_timer.Enabled = true;
            this.heartBeat_timer.Interval = 3000;
            this.heartBeat_timer.Tick += new System.EventHandler(this.heartBeat_timer_tick);
            // 
            // Server_timer
            // 
            this.Server_timer.Interval = 2000;
            this.Server_timer.Tick += new System.EventHandler(this.Server_timer_Tick);
            // 
            // cruise_timer
            // 
            this.cruise_timer.Interval = 6000;
            this.cruise_timer.Tick += new System.EventHandler(this.cruise_timer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1111, 577);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "彼岸IR监控设备模块";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbReceiBuf;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txbSendBuf;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button35;
        private System.Windows.Forms.Button button34;
        private System.Windows.Forms.Label lb_Status_LoginStatus_Disp;
        private System.Windows.Forms.Button button33;
        private System.Windows.Forms.Button button32;
        private System.Windows.Forms.Button btn_ConnectServer;
        private System.Windows.Forms.TextBox txbServerPort;
        private System.Windows.Forms.TextBox txbServerIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button butUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbConnectState;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox texPassww;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer cmdRevMsg_timer;
        private System.Windows.Forms.Timer showMsg_timer;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Timer devStatus_timer;
        private System.Windows.Forms.Timer heartBeat_timer;
        private System.Windows.Forms.Timer Server_timer;
        private System.Windows.Forms.Timer cruise_timer;
    }
}

