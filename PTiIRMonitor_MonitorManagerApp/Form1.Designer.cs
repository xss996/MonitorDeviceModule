namespace PTiIRMonitor_MonitorManagerApp
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.treeView3 = new System.Windows.Forms.TreeView();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.关闭ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重启ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.显示ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvwOneMonDevErrorStateTab = new System.Windows.Forms.ListView();
            this.label7 = new System.Windows.Forms.Label();
            this.txbOneMonDevWorkStateUpateTime = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txbOneMonDevTypeName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txbOneMonDevWorkState = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txbOneMonDevIndex = new System.Windows.Forms.TextBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeView3);
            this.groupBox1.Controls.Add(this.treeView2);
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(358, 521);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "启动状态";
            // 
            // treeView3
            // 
            this.treeView3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.treeView3.Location = new System.Drawing.Point(32, 317);
            this.treeView3.Name = "treeView3";
            this.treeView3.Size = new System.Drawing.Size(301, 188);
            this.treeView3.TabIndex = 12;
            this.treeView3.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView3_AfterSelect);
            this.treeView3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView3_MouseDown);
            // 
            // treeView2
            // 
            this.treeView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.treeView2.Location = new System.Drawing.Point(32, 52);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(304, 83);
            this.treeView2.TabIndex = 11;
            this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            this.treeView2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView2_MouseDown);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.treeView1.Location = new System.Drawing.Point(29, 169);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(304, 114);
            this.treeView1.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(30, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "命令服务器状态";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "数据分析服务器状态";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 297);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "监控头在线状态";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 194);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "故障情况";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关闭ToolStripMenuItem,
            this.重启ToolStripMenuItem,
            this.刷新ToolStripMenuItem1,
            this.显示ToolStripMenuItem1,
            this.隐藏ToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 114);
            // 
            // 关闭ToolStripMenuItem
            // 
            this.关闭ToolStripMenuItem.Name = "关闭ToolStripMenuItem";
            this.关闭ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.关闭ToolStripMenuItem.Text = "关闭";
            this.关闭ToolStripMenuItem.Click += new System.EventHandler(this.关闭ToolStripMenuItem_Click);
            // 
            // 重启ToolStripMenuItem
            // 
            this.重启ToolStripMenuItem.Name = "重启ToolStripMenuItem";
            this.重启ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.重启ToolStripMenuItem.Text = "重启";
            this.重启ToolStripMenuItem.Click += new System.EventHandler(this.重启ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem1
            // 
            this.刷新ToolStripMenuItem1.Name = "刷新ToolStripMenuItem1";
            this.刷新ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.刷新ToolStripMenuItem1.Text = "刷新";
            this.刷新ToolStripMenuItem1.Click += new System.EventHandler(this.刷新ToolStripMenuItem1_Click);
            // 
            // 显示ToolStripMenuItem1
            // 
            this.显示ToolStripMenuItem1.Name = "显示ToolStripMenuItem1";
            this.显示ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.显示ToolStripMenuItem1.Text = "显示";
            // 
            // 隐藏ToolStripMenuItem1
            // 
            this.隐藏ToolStripMenuItem1.Name = "隐藏ToolStripMenuItem1";
            this.隐藏ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.隐藏ToolStripMenuItem1.Text = "隐藏";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvwOneMonDevErrorStateTab);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txbOneMonDevWorkStateUpateTime);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txbOneMonDevTypeName);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txbOneMonDevWorkState);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txbOneMonDevIndex);
            this.groupBox2.Location = new System.Drawing.Point(376, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(548, 521);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "状态详情";
            // 
            // lvwOneMonDevErrorStateTab
            // 
            this.lvwOneMonDevErrorStateTab.Location = new System.Drawing.Point(84, 186);
            this.lvwOneMonDevErrorStateTab.Name = "lvwOneMonDevErrorStateTab";
            this.lvwOneMonDevErrorStateTab.Size = new System.Drawing.Size(437, 319);
            this.lvwOneMonDevErrorStateTab.TabIndex = 18;
            this.lvwOneMonDevErrorStateTab.UseCompatibleStateImageBehavior = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(251, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "心跳时间";
            // 
            // txbOneMonDevWorkStateUpateTime
            // 
            this.txbOneMonDevWorkStateUpateTime.Location = new System.Drawing.Point(306, 138);
            this.txbOneMonDevWorkStateUpateTime.Name = "txbOneMonDevWorkStateUpateTime";
            this.txbOneMonDevWorkStateUpateTime.Size = new System.Drawing.Size(218, 21);
            this.txbOneMonDevWorkStateUpateTime.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "当前状态";
            // 
            // txbOneMonDevTypeName
            // 
            this.txbOneMonDevTypeName.Location = new System.Drawing.Point(306, 87);
            this.txbOneMonDevTypeName.Name = "txbOneMonDevTypeName";
            this.txbOneMonDevTypeName.Size = new System.Drawing.Size(218, 21);
            this.txbOneMonDevTypeName.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(272, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "类型";
            // 
            // txbOneMonDevWorkState
            // 
            this.txbOneMonDevWorkState.Location = new System.Drawing.Point(86, 137);
            this.txbOneMonDevWorkState.Name = "txbOneMonDevWorkState";
            this.txbOneMonDevWorkState.Size = new System.Drawing.Size(149, 21);
            this.txbOneMonDevWorkState.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "ID";
            // 
            // txbOneMonDevIndex
            // 
            this.txbOneMonDevIndex.Location = new System.Drawing.Point(86, 86);
            this.txbOneMonDevIndex.Name = "txbOneMonDevIndex";
            this.txbOneMonDevIndex.Size = new System.Drawing.Size(149, 21);
            this.txbOneMonDevIndex.TabIndex = 9;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.显示ToolStripMenuItem,
            this.隐藏ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(101, 114);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem1.Text = "关闭";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem2.Text = "重启";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.显示ToolStripMenuItem.Text = "显示";
            this.显示ToolStripMenuItem.Click += new System.EventHandler(this.显示ToolStripMenuItem_Click);
            // 
            // 隐藏ToolStripMenuItem
            // 
            this.隐藏ToolStripMenuItem.Name = "隐藏ToolStripMenuItem";
            this.隐藏ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.隐藏ToolStripMenuItem.Text = "隐藏";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(935, 537);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "彼岸IR监控管理软件";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 关闭ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重启ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvwOneMonDevErrorStateTab;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txbOneMonDevWorkStateUpateTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txbOneMonDevTypeName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbOneMonDevWorkState;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txbOneMonDevIndex;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 隐藏ToolStripMenuItem1;
        private System.Windows.Forms.TreeView treeView3;
        private System.Windows.Forms.Timer timer1;
    }
}

