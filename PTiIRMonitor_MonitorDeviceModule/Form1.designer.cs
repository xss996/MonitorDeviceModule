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
            this.label_heatbeatStatus = new System.Windows.Forms.Label();
            this.label_loginStatus = new System.Windows.Forms.Label();
            this.label_server = new System.Windows.Forms.Label();
            this.label_socket = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.revVar_timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_stopCruise = new System.Windows.Forms.Button();
            this.btn_startCruise = new System.Windows.Forms.Button();
            this.label_cruise = new System.Windows.Forms.Label();
            this.label_Database = new System.Windows.Forms.Label();
            this.label_FTP = new System.Windows.Forms.Label();
            this.label_TV = new System.Windows.Forms.Label();
            this.label_IR = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.红外调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.可见光调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
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
            this.groupBox1.Controls.Add(this.label_heatbeatStatus);
            this.groupBox1.Controls.Add(this.label_loginStatus);
            this.groupBox1.Controls.Add(this.label_server);
            this.groupBox1.Controls.Add(this.label_socket);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 249);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器";
            // 
            // label_heatbeatStatus
            // 
            this.label_heatbeatStatus.AutoSize = true;
            this.label_heatbeatStatus.Location = new System.Drawing.Point(124, 111);
            this.label_heatbeatStatus.Name = "label_heatbeatStatus";
            this.label_heatbeatStatus.Size = new System.Drawing.Size(41, 12);
            this.label_heatbeatStatus.TabIndex = 7;
            this.label_heatbeatStatus.Text = "label7";
            // 
            // label_loginStatus
            // 
            this.label_loginStatus.AutoSize = true;
            this.label_loginStatus.Location = new System.Drawing.Point(122, 85);
            this.label_loginStatus.Name = "label_loginStatus";
            this.label_loginStatus.Size = new System.Drawing.Size(41, 12);
            this.label_loginStatus.TabIndex = 6;
            this.label_loginStatus.Text = "label6";
            // 
            // label_server
            // 
            this.label_server.AutoSize = true;
            this.label_server.Location = new System.Drawing.Point(122, 53);
            this.label_server.Name = "label_server";
            this.label_server.Size = new System.Drawing.Size(41, 12);
            this.label_server.TabIndex = 5;
            this.label_server.Text = "label5";
            // 
            // label_socket
            // 
            this.label_socket.AutoSize = true;
            this.label_socket.Location = new System.Drawing.Point(122, 28);
            this.label_socket.Name = "label_socket";
            this.label_socket.Size = new System.Drawing.Size(41, 12);
            this.label_socket.TabIndex = 4;
            this.label_socket.Text = "label5";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(8, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "心跳启动状态：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(6, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Socket通信状态 ：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(6, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "用户登录状态：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 54);
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
            this.groupBox2.Controls.Add(this.btn_stopCruise);
            this.groupBox2.Controls.Add(this.btn_startCruise);
            this.groupBox2.Controls.Add(this.label_cruise);
            this.groupBox2.Controls.Add(this.label_Database);
            this.groupBox2.Controls.Add(this.label_FTP);
            this.groupBox2.Controls.Add(this.label_TV);
            this.groupBox2.Controls.Add(this.label_IR);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(479, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(444, 249);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "监控设备";
            // 
            // btn_stopCruise
            // 
            this.btn_stopCruise.Location = new System.Drawing.Point(121, 178);
            this.btn_stopCruise.Name = "btn_stopCruise";
            this.btn_stopCruise.Size = new System.Drawing.Size(75, 23);
            this.btn_stopCruise.TabIndex = 11;
            this.btn_stopCruise.Text = "停止巡检";
            this.btn_stopCruise.UseVisualStyleBackColor = true;
            this.btn_stopCruise.Click += new System.EventHandler(this.btn_stopCruise_Click);
            // 
            // btn_startCruise
            // 
            this.btn_startCruise.Location = new System.Drawing.Point(22, 178);
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
            this.label_cruise.Location = new System.Drawing.Point(119, 135);
            this.label_cruise.Name = "label_cruise";
            this.label_cruise.Size = new System.Drawing.Size(47, 12);
            this.label_cruise.TabIndex = 9;
            this.label_cruise.Text = "label11";
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
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(20, 135);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 12);
            this.label10.TabIndex = 4;
            this.label10.Text = "巡检状态:";
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
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.红外调试ToolStripMenuItem,
            this.可见光调试ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1142, 25);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1142, 670);
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer cmdRevMsg_timer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_socket;
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
    }
}

