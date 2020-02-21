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
            this.cmdRevMsg_timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.num_serverPort = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_updateServerInfo = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label_serverIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label_heatbeatStatus = new System.Windows.Forms.Label();
            this.label_loginStatus = new System.Windows.Forms.Label();
            this.label_server = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.revVar_timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label_Database = new System.Windows.Forms.Label();
            this.label_FTP = new System.Windows.Forms.Label();
            this.label_TV = new System.Windows.Forms.Label();
            this.label_IR = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_stopCruise = new System.Windows.Forms.Button();
            this.btn_startCruise = new System.Windows.Forms.Button();
            this.label_cruise = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.红外调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.可见光调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pos_cruiseStartTime = new System.Windows.Forms.DateTimePicker();
            this.label21 = new System.Windows.Forms.Label();
            this.pos_interval = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.pos_cruiseType = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.pos_name = new System.Windows.Forms.TextBox();
            this.pos_Index = new System.Windows.Forms.NumericUpDown();
            this.pos_stationIndex = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.ReceiveCruiseLog_timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_serverPort)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pos_interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pos_Index)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pos_stationIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdRevMsg_timer
            // 
            this.cmdRevMsg_timer.Enabled = true;
            this.cmdRevMsg_timer.Interval = 300;
            this.cmdRevMsg_timer.Tick += new System.EventHandler(this.cmdRevMsg_timer_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.num_serverPort);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.btn_updateServerInfo);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label_serverIP);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label_heatbeatStatus);
            this.groupBox1.Controls.Add(this.label_loginStatus);
            this.groupBox1.Controls.Add(this.label_server);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 249);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 135);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 16;
            this.label13.Text = "UserInfo";
            // 
            // num_serverPort
            // 
            this.num_serverPort.Location = new System.Drawing.Point(205, 102);
            this.num_serverPort.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.num_serverPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_serverPort.Name = "num_serverPort";
            this.num_serverPort.Size = new System.Drawing.Size(75, 21);
            this.num_serverPort.TabIndex = 15;
            this.num_serverPort.Value = new decimal(new int[] {
            11573,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 83);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 14;
            this.label12.Text = "ServerInfo";
            // 
            // btn_updateServerInfo
            // 
            this.btn_updateServerInfo.Location = new System.Drawing.Point(315, 102);
            this.btn_updateServerInfo.Name = "btn_updateServerInfo";
            this.btn_updateServerInfo.Size = new System.Drawing.Size(75, 23);
            this.btn_updateServerInfo.TabIndex = 12;
            this.btn_updateServerInfo.Text = "修改";
            this.btn_updateServerInfo.UseVisualStyleBackColor = true;
            this.btn_updateServerInfo.Click += new System.EventHandler(this.btn_updateServerInfo_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(164, 105);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 12);
            this.label11.TabIndex = 10;
            this.label11.Text = "端口:";
            // 
            // label_serverIP
            // 
            this.label_serverIP.Location = new System.Drawing.Point(31, 102);
            this.label_serverIP.Name = "label_serverIP";
            this.label_serverIP.Size = new System.Drawing.Size(111, 21);
            this.label_serverIP.TabIndex = 9;
            this.label_serverIP.Text = "192.168.123.128";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "IP:";
            // 
            // label_heatbeatStatus
            // 
            this.label_heatbeatStatus.AutoSize = true;
            this.label_heatbeatStatus.Location = new System.Drawing.Point(101, 53);
            this.label_heatbeatStatus.Name = "label_heatbeatStatus";
            this.label_heatbeatStatus.Size = new System.Drawing.Size(41, 12);
            this.label_heatbeatStatus.TabIndex = 7;
            this.label_heatbeatStatus.Text = "label7";
            // 
            // label_loginStatus
            // 
            this.label_loginStatus.AutoSize = true;
            this.label_loginStatus.Location = new System.Drawing.Point(303, 25);
            this.label_loginStatus.Name = "label_loginStatus";
            this.label_loginStatus.Size = new System.Drawing.Size(41, 12);
            this.label_loginStatus.TabIndex = 6;
            this.label_loginStatus.Text = "label6";
            // 
            // label_server
            // 
            this.label_server.AutoSize = true;
            this.label_server.Location = new System.Drawing.Point(128, 25);
            this.label_server.Name = "label_server";
            this.label_server.Size = new System.Drawing.Size(41, 12);
            this.label_server.TabIndex = 5;
            this.label_server.Text = "label5";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(12, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "心跳状态：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(187, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "用户登录状态：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器连接状态：";
            // 
            // revVar_timer
            // 
            this.revVar_timer.Enabled = true;
            this.revVar_timer.Interval = 2500;
            this.revVar_timer.Tick += new System.EventHandler(this.revVar_timer_tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label_Database);
            this.groupBox2.Controls.Add(this.label_FTP);
            this.groupBox2.Controls.Add(this.label_TV);
            this.groupBox2.Controls.Add(this.label_IR);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(479, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(861, 249);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "监控设备";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(185, 53);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 13;
            this.label15.Text = "TVInfo";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(185, 28);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 12;
            this.label14.Text = "IRInfo";
            // 
            // label_Database
            // 
            this.label_Database.AutoSize = true;
            this.label_Database.Location = new System.Drawing.Point(119, 111);
            this.label_Database.Name = "label_Database";
            this.label_Database.Size = new System.Drawing.Size(47, 12);
            this.label_Database.TabIndex = 8;
            this.label_Database.Text = "label11";
            // 
            // label_FTP
            // 
            this.label_FTP.AutoSize = true;
            this.label_FTP.Location = new System.Drawing.Point(117, 84);
            this.label_FTP.Name = "label_FTP";
            this.label_FTP.Size = new System.Drawing.Size(47, 12);
            this.label_FTP.TabIndex = 7;
            this.label_FTP.Text = "label11";
            // 
            // label_TV
            // 
            this.label_TV.AutoSize = true;
            this.label_TV.Location = new System.Drawing.Point(117, 53);
            this.label_TV.Name = "label_TV";
            this.label_TV.Size = new System.Drawing.Size(47, 12);
            this.label_TV.TabIndex = 6;
            this.label_TV.Text = "label11";
            // 
            // label_IR
            // 
            this.label_IR.AutoSize = true;
            this.label_IR.Location = new System.Drawing.Point(115, 27);
            this.label_IR.Name = "label_IR";
            this.label_IR.Size = new System.Drawing.Size(47, 12);
            this.label_IR.TabIndex = 5;
            this.label_IR.Text = "label11";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(18, 111);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "数据库连接状态:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(18, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "FTP连接状态:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(18, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "可见光连接状态:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(18, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "红外连接状态:";
            // 
            // btn_stopCruise
            // 
            this.btn_stopCruise.Location = new System.Drawing.Point(237, 20);
            this.btn_stopCruise.Name = "btn_stopCruise";
            this.btn_stopCruise.Size = new System.Drawing.Size(75, 23);
            this.btn_stopCruise.TabIndex = 11;
            this.btn_stopCruise.Text = "停止巡检";
            this.btn_stopCruise.UseVisualStyleBackColor = true;
            this.btn_stopCruise.Click += new System.EventHandler(this.btn_stopCruise_Click);
            // 
            // btn_startCruise
            // 
            this.btn_startCruise.Location = new System.Drawing.Point(145, 20);
            this.btn_startCruise.Name = "btn_startCruise";
            this.btn_startCruise.Size = new System.Drawing.Size(75, 23);
            this.btn_startCruise.TabIndex = 10;
            this.btn_startCruise.Text = "开始巡检";
            this.btn_startCruise.UseVisualStyleBackColor = true;
            this.btn_startCruise.Click += new System.EventHandler(this.btn_startCruise_Click);
            // 
            // label_cruise
            // 
            this.label_cruise.AutoSize = true;
            this.label_cruise.Location = new System.Drawing.Point(78, 29);
            this.label_cruise.Name = "label_cruise";
            this.label_cruise.Size = new System.Drawing.Size(47, 12);
            this.label_cruise.TabIndex = 9;
            this.label_cruise.Text = "label11";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(8, 29);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 12);
            this.label10.TabIndex = 4;
            this.label10.Text = "巡检状态:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.红外调试ToolStripMenuItem,
            this.可见光调试ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1446, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 红外调试ToolStripMenuItem
            // 
            this.红外调试ToolStripMenuItem.Name = "红外调试ToolStripMenuItem";
            this.红外调试ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.红外调试ToolStripMenuItem.Text = "红外调试";
            this.红外调试ToolStripMenuItem.Click += new System.EventHandler(this.红外调试ToolStripMenuItem_Click);
            // 
            // 可见光调试ToolStripMenuItem
            // 
            this.可见光调试ToolStripMenuItem.Name = "可见光调试ToolStripMenuItem";
            this.可见光调试ToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.可见光调试ToolStripMenuItem.Text = "可见光调试";
            this.可见光调试ToolStripMenuItem.Click += new System.EventHandler(this.可见光调试ToolStripMenuItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel2);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Controls.Add(this.label_cruise);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.btn_stopCruise);
            this.groupBox3.Controls.Add(this.btn_startCruise);
            this.groupBox3.Location = new System.Drawing.Point(12, 312);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1328, 382);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "巡检相关";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.richTextBox1);
            this.panel2.Location = new System.Drawing.Point(467, 21);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(855, 355);
            this.panel2.TabIndex = 26;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(4, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 27;
            this.button2.Text = "清除日志";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(4, 37);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(848, 315);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = " ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(332, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 25;
            this.button1.Text = "添加固定点";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pos_cruiseStartTime);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Controls.Add(this.pos_interval);
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.pos_cruiseType);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.label19);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.pos_name);
            this.panel1.Controls.Add(this.pos_Index);
            this.panel1.Controls.Add(this.pos_stationIndex);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Location = new System.Drawing.Point(10, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(428, 318);
            this.panel1.TabIndex = 24;
            // 
            // pos_cruiseStartTime
            // 
            this.pos_cruiseStartTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.pos_cruiseStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.pos_cruiseStartTime.Location = new System.Drawing.Point(242, 85);
            this.pos_cruiseStartTime.Name = "pos_cruiseStartTime";
            this.pos_cruiseStartTime.Size = new System.Drawing.Size(155, 21);
            this.pos_cruiseStartTime.TabIndex = 28;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(177, 87);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(59, 12);
            this.label21.TabIndex = 27;
            this.label21.Text = "开始时间:";
            // 
            // pos_interval
            // 
            this.pos_interval.Location = new System.Drawing.Point(86, 85);
            this.pos_interval.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.pos_interval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pos_interval.Name = "pos_interval";
            this.pos_interval.Size = new System.Drawing.Size(75, 21);
            this.pos_interval.TabIndex = 26;
            this.pos_interval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(9, 87);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(59, 12);
            this.label20.TabIndex = 25;
            this.label20.Text = "巡检时长:";
            // 
            // pos_cruiseType
            // 
            this.pos_cruiseType.FormattingEnabled = true;
            this.pos_cruiseType.Items.AddRange(new object[] {
            "隔时巡检",
            "定时巡检"});
            this.pos_cruiseType.Location = new System.Drawing.Point(254, 49);
            this.pos_cruiseType.Name = "pos_cruiseType";
            this.pos_cruiseType.Size = new System.Drawing.Size(100, 20);
            this.pos_cruiseType.TabIndex = 24;
            this.pos_cruiseType.Text = "隔时巡检";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(177, 15);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(71, 12);
            this.label17.TabIndex = 18;
            this.label17.Text = "固定点名称:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(177, 51);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(59, 12);
            this.label19.TabIndex = 23;
            this.label19.Text = "巡检类型:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(9, 15);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(71, 12);
            this.label16.TabIndex = 12;
            this.label16.Text = "固定点索引:";
            // 
            // pos_name
            // 
            this.pos_name.Location = new System.Drawing.Point(254, 12);
            this.pos_name.Name = "pos_name";
            this.pos_name.Size = new System.Drawing.Size(100, 21);
            this.pos_name.TabIndex = 22;
            // 
            // pos_Index
            // 
            this.pos_Index.Location = new System.Drawing.Point(86, 13);
            this.pos_Index.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.pos_Index.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pos_Index.Name = "pos_Index";
            this.pos_Index.ReadOnly = true;
            this.pos_Index.Size = new System.Drawing.Size(75, 21);
            this.pos_Index.TabIndex = 17;
            this.pos_Index.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // pos_stationIndex
            // 
            this.pos_stationIndex.Location = new System.Drawing.Point(86, 49);
            this.pos_stationIndex.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.pos_stationIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pos_stationIndex.Name = "pos_stationIndex";
            this.pos_stationIndex.Size = new System.Drawing.Size(75, 21);
            this.pos_stationIndex.TabIndex = 21;
            this.pos_stationIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(9, 51);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(71, 12);
            this.label18.TabIndex = 20;
            this.label18.Text = "变电站索引:";
            // 
            // ReceiveCruiseLog_timer
            // 
            this.ReceiveCruiseLog_timer.Enabled = true;
            this.ReceiveCruiseLog_timer.Interval = 200;
            this.ReceiveCruiseLog_timer.Tick += new System.EventHandler(this.ReceiveCruiseLog_timer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1446, 706);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "彼岸IR监控设备模块";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_serverPort)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pos_interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pos_Index)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pos_stationIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer cmdRevMsg_timer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_heatbeatStatus;
        private System.Windows.Forms.Label label_loginStatus;
        private System.Windows.Forms.Label label_server;
        private System.Windows.Forms.Timer revVar_timer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label_IR;
        private System.Windows.Forms.Label label_TV;
        private System.Windows.Forms.Label label_FTP;
        private System.Windows.Forms.Label label_Database;
        private System.Windows.Forms.Label label_cruise;
        private System.Windows.Forms.Button btn_stopCruise;
        private System.Windows.Forms.Button btn_startCruise;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 红外调试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 可见光调试ToolStripMenuItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox label_serverIP;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_updateServerInfo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown num_serverPort;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown pos_Index;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown pos_stationIndex;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox pos_name;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox pos_cruiseType;
        private System.Windows.Forms.NumericUpDown pos_interval;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker pos_cruiseStartTime;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer ReceiveCruiseLog_timer;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

