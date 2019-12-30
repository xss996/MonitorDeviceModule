using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Runtime;
using System.IO;
using System.Xml;
namespace PTiIRMonitor_MonitorManagerApp
{
    public partial class 彼岸IR监控管理软件 : Form
    {
        clsCmdMonitorOpt MonOpt = new clsCmdMonitorOpt();
        clsTreeView.stuCmdServer stu = new clsTreeView.stuCmdServer();
        public 彼岸IR监控管理软件()
        {

            InitializeComponent();
            KeyDown += new KeyEventHandler(Form1_KeyDown);
        }
        private void Form1_Load(object sender, EventArgs e)//
        {
            updateGuiDispServerDevLst();//命令服务器

            MonOpt.ConnectCmdServer();//启动命令服务器窗口

            MonOpt.MonitorDevice();//启动监控头窗口

            updateGuiDispMonitorDevLst();


        }
        public void Satrt()
        {
            Thread sat = new Thread(getTimeSpan);
            sat.Start();
        }
        //
        public void updateGuiDispServerDevLst()  //更新显示命令服务器列表
        {
            string str1 = "命令服务器";
            treeView2.Nodes.Clear();
            TreeNode tnbuf = new TreeNode();
            tnbuf.Text = str1;
            treeView2.Nodes.Add(tnbuf);
        }

        public void updateGuiDispMonitorDevLst()  //更新显示监控头列表
        {
            string str1;
            int inID, inType;
            for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
            {
                inID = MonOpt.Tree.IniMonitor[i].MonitorID;
                inType = MonOpt.Tree.IniMonitor[i].intType;
                if (inType == 1)
                {
                    str1 = "监控头_" + inID;
                    MonOpt.Tree.IniMonitor[i].strName = str1;//
                    treeView3.Nodes.Add(str1);
                }
            }
        }
        private void tvwMonDevLst_AfterSelect(object sender, TreeViewEventArgs e)
        {
            updateGuiDispOneMonDevStatus();
        }
        public void updateGuiDispOneMonDevStatus()//监控头
        {
        }
        public void updateGuiDispServer() //服务器
        {
            txbOneMonDevIndex.Text = "1";
            txbOneMonDevTypeName.Text = "命令服务器";
        }
        public void updateGuiDispMonitor()  //监控端
        {

        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)//关闭命令服务器
        {
            MonOpt.CmdServerKill();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)//重启
        {
            MonOpt.CmdServerKill();//重启先关闭
            MonOpt.ConnectCmdServer();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)//服务器刷新
        {
            updateGuiDispServer();
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            treeView3.ContextMenuStrip = contextMenuStrip1;
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)//显示
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)//系统提示
        {
            //if   判断服务器与监控端的状态是否是忙碌，忙碌提示！！
            DialogResult dr = MessageBox.Show("提示！", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)   //如果单击“是”按钮
            {
                MonOpt.CmdServerKill();//关闭命令服务器
                MonOpt.funManagerDeviceKill();//关闭监控头
                e.Cancel = false;                 //关闭窗体

            }
            else if (dr == DialogResult.Cancel)
            {
                e.Cancel = true;                  //不执行操作
            }
        }

        public void getTimeSpan()//判断监控模块是否正常
        {

            while (true)
            {
                Thread.Sleep(20);
                DateTime strTime = DateTime.Now;
                for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
                {
                    if (MonOpt.Tree.IniMonitor[i].strName == "监控头_1")
                    {
                        DateTime strDate = MonOpt.Tree.IniMonitor[i].dateTine;
                        TimeSpan span = strTime - strDate;
                        int n = Convert.ToInt32(span.TotalSeconds);
                        if (n > 15)
                        {
                            MessageBox.Show("监控头_1心跳异常，断开连接");
                            break;
                        }
                    }
                    //else if (MonOpt.Tree.IniMonitor[i].strName == "监控头_2")
                    //{
                    //    DateTime strDate = MonOpt.Tree.IniMonitor[i].dateTine;
                    //    TimeSpan span = strTime - strDate;
                    //    int n = Convert.ToInt32(span.TotalSeconds);
                    //    if (n > 15)
                    //    {
                    //        MessageBox.Show("监控头_2心跳异常，断开连接");
                    //        break;
                    //    }
                    //}
                }
            }
        }
        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)//
        {
            string str = this.treeView3.SelectedNode.Text;
            for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
            {
                if (str == MonOpt.Tree.IniMonitor[i].strName)
                {
                    MonOpt.MonitorOut(MonOpt.Tree.IniMonitor[i].inPID);
                    MonOpt.Tree.IniMonitor[i].MoState = 0;//将此监控头的状态清空，0表示停止
                    MonOpt.Tree.IniMonitor[i].inPID = 0;
                }
            }
        }
        int intHeartCount = 0;
        public void funMonitor()
        {
            intHeartCount = 0;
            while (true)
            {
                intHeartCount++;
                Thread.Sleep(600);
                if (intHeartCount >= 120)
                {
                    MessageBox.Show("断开连接，重新启动");
                    break;
                }
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue > 0)//服务器状态——1~~10————1表示服务器启动，2表示服务器停止，.......................
            {
                //////服务器状态
                if (e.KeyValue == 1)//命令服务器启动
                {
                    //  int1 = 1;
                }
                else if (e.KeyValue == 2)//停止
                {
                    //   int1 = 2;
                }
                /////////////////
                /////////////////
                for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
                {
                    if (e.KeyValue == 11)//表示一号在线
                    {
                        if (MonOpt.Tree.IniMonitor[i].strName == "监控头_1")
                        {
                            MonOpt.Tree.IniMonitor[i].MoState = 11;//11表示在线
                            MonOpt.Tree.IniMonitor[i].dateTine = DateTime.Now;//一号的心跳时间
                        }
                    }
                    else if (e.KeyValue == 12)//表示一号停止
                    {
                        if (MonOpt.Tree.IniMonitor[i].strName == "监控头_1")
                        {
                            MonOpt.Tree.IniMonitor[i].MoState = 12;
                            MonOpt.Tree.IniMonitor[i].dateTine = DateTime.Now;//
                        }
                    }
                    else if (e.KeyValue == 13)//表示二号在线
                    {
                        if (MonOpt.Tree.IniMonitor[i].strName == "监控头_2")
                        {
                            MonOpt.Tree.IniMonitor[i].MoState = 13;
                            MonOpt.Tree.IniMonitor[i].dateTine = DateTime.Now;//二号的心跳时间
                        }
                    }
                    else if (e.KeyValue == 14)//表示二号监控头停止
                    {
                        if (MonOpt.Tree.IniMonitor[i].strName == "监控头_2")
                        {
                            MonOpt.Tree.IniMonitor[i].MoState = 14;
                            MonOpt.Tree.IniMonitor[i].dateTine = DateTime.Now;//
                        }
                    }
                    else if (e.KeyValue == 15)//表示三号监控头启动
                    {
                        if (MonOpt.Tree.IniMonitor[i].strName == "监控头_3")
                        {
                            MonOpt.Tree.IniMonitor[i].MoState = 15;
                            MonOpt.Tree.IniMonitor[i].dateTine = DateTime.Now;//三号的心跳时间
                        }
                    }
                    else if (e.KeyValue == 16)//停止
                    {
                        if (MonOpt.Tree.IniMonitor[i].strName == "监控头_3")
                        {
                            MonOpt.Tree.IniMonitor[i].MoState = 16;
                            MonOpt.Tree.IniMonitor[i].dateTine = DateTime.Now;//
                        }
                    }
                    else if (e.KeyValue == 17) //启动四号监控头
                    {
                        if (MonOpt.Tree.IniMonitor[i].strName == "监控头_4")
                        {
                            MonOpt.Tree.IniMonitor[i].MoState = 17;
                            MonOpt.Tree.IniMonitor[i].dateTine = DateTime.Now;//
                        }
                    }
                    else if (e.KeyValue == 18)//四号停止
                    {
                        if (MonOpt.Tree.IniMonitor[i].strName == "监控头_4")
                        {
                            MonOpt.Tree.IniMonitor[i].MoState = 18;
                            MonOpt.Tree.IniMonitor[i].dateTine = DateTime.Now;//
                        }
                    }
                }
            }
        }
        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string str = this.treeView3.SelectedNode.Text;
            if (str == "监控头_1")
            {
                txbOneMonDevIndex.Text = "1";//监控头编号
                txbOneMonDevTypeName.Text = "监控头";//类型
                for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
                {
                    if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_1") && (MonOpt.Tree.IniMonitor[i].MoState == 0))
                    {
                        txbOneMonDevWorkState.Text = "停止";
                    }
                    else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_1") && (MonOpt.Tree.IniMonitor[i].MoState == 1))//一号监控头--11表示在线
                    {
                        txbOneMonDevWorkState.Text = "启动";
                        txbOneMonDevWorkStateUpateTime.Text = MonOpt.Tree.IniMonitor[i].dateTine.ToString();
                    }
                }
            }
            else if (str == "监控头_2")
            {
                txbOneMonDevIndex.Text = "2";//监控头编号
                txbOneMonDevTypeName.Text = "监控头";//类型
                for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
                {
                    if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_2") && (MonOpt.Tree.IniMonitor[i].MoState == 1))//二号监控头--表示在线
                    {
                        txbOneMonDevWorkState.Text = "启动";
                        txbOneMonDevWorkStateUpateTime.Text = MonOpt.Tree.IniMonitor[i].dateTine.ToString();
                    }
                    else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_2") && (MonOpt.Tree.IniMonitor[i].MoState == 0))//表示停止
                    {
                        txbOneMonDevWorkState.Text = "停止";
                    }
                }
            }
            else if (str == "监控头_3")
            {
                txbOneMonDevIndex.Text = "3";//监控头编号
                txbOneMonDevTypeName.Text = "监控头";//类型
                txbOneMonDevWorkStateUpateTime.Text = DateTime.Now.ToString();//时间
                for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
                {
                    if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_3_0") && (MonOpt.Tree.IniMonitor[i].MoState == 0))
                    {
                        txbOneMonDevWorkState.Text = "停止";
                    }
                    else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_1_1") && (MonOpt.Tree.IniMonitor[i].MoState == 1))
                    {
                        txbOneMonDevWorkState.Text = "启动";
                    }
                }
            }
            else if (str == "监控头_4")
            {
                txbOneMonDevIndex.Text = "3";//监控头编号
                txbOneMonDevTypeName.Text = "监控头";//类型
                txbOneMonDevWorkStateUpateTime.Text = DateTime.Now.ToString();//时间
                for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
                {
                    if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_4_0") && (MonOpt.Tree.IniMonitor[i].MoState == 0))
                    {
                        txbOneMonDevWorkState.Text = "停止";
                    }
                    else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_4_1") && (MonOpt.Tree.IniMonitor[i].MoState == 1))
                    {
                        txbOneMonDevWorkState.Text = "启动";
                    }
                }
            }
        }
        private void treeView3_MouseDown(object sender, MouseEventArgs e)
        {
            if (treeView3.SelectedNode == null)
                treeView3.ContextMenuStrip = null;
            else
                treeView3.ContextMenuStrip = contextMenuStrip1;
        }

        private void 刷新ToolStripMenuItem1_Click(object sender, EventArgs e)//监控头刷新
        {
            for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
            {
                if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_1") && (MonOpt.Tree.IniMonitor[i].MoState == 0) && (MonOpt.Tree.IniMonitor[i].MoState == 12))
                {
                    txbOneMonDevWorkState.Text = "停止";
                }
                else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_2") && (MonOpt.Tree.IniMonitor[i].MoState == 0) && (MonOpt.Tree.IniMonitor[i].MoState == 14))
                {
                    txbOneMonDevWorkState.Text = "停止";
                }
                else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_3") && (MonOpt.Tree.IniMonitor[i].MoState == 0) && (MonOpt.Tree.IniMonitor[i].MoState == 16))
                {
                    txbOneMonDevWorkState.Text = "停止";
                }
                else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_4") && (MonOpt.Tree.IniMonitor[i].MoState == 0) && (MonOpt.Tree.IniMonitor[i].MoState == 18))
                {
                    txbOneMonDevWorkState.Text = "停止";
                }
                else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_1") && (MonOpt.Tree.IniMonitor[i].MoState == 11))
                {
                    txbOneMonDevWorkState.Text = "在线";
                }
                else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_2") && (MonOpt.Tree.IniMonitor[i].MoState == 13))
                {
                    txbOneMonDevWorkState.Text = "在线";
                }
                else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_3") && (MonOpt.Tree.IniMonitor[i].MoState == 15))
                {
                    txbOneMonDevWorkState.Text = "在线";
                }
                else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_4") && (MonOpt.Tree.IniMonitor[i].MoState == 17))
                {
                    txbOneMonDevWorkState.Text = "在线";
                }
            }
        }
        private void 重启ToolStripMenuItem_Click(object sender, EventArgs e)//启动监控头
        {
            string strName = this.treeView3.SelectedNode.Text;
            for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
            {
                if ((strName == MonOpt.Tree.IniMonitor[i].strName) && (MonOpt.Tree.IniMonitor[i].inPID != 0))//判断监控头的状态是启动还是停止
                {
                    MonOpt.MonitorOut(MonOpt.Tree.IniMonitor[i].inPID);
                    MonOpt.Tree.IniMonitor[i].MoState = 0;//将此监控头的状态清空，0表示停止
                    MonOpt.Tree.IniMonitor[i].inPID = 0;
                    MonOpt.MonitorDeviceStart(strName);
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)//定时接收心跳命令
        {
            string str1;
            for (int i = 0; i < MonOpt.Tree.lststrReceiCmd.Count; i++)
            {
                Thread.Sleep(50);
                str1 = MonOpt.Tree.lststrReceiCmd[i];
                if (str1 == "一号")//表示一号监控头
                {
                    for (int j = 0; j < MonOpt.Tree.IniMonitor.Count; j++)
                    {
                        if (MonOpt.Tree.IniMonitor[j].strName == "监控头_1")
                        {
                            MonOpt.Tree.IniMonitor[j].MoState = 1;//写入状态，1表示在线，0表示停止运行
                        }
                    }
                }
                else if (str1 == "二号")//表示二号监控头
                {
                    for (int j = 0; j < MonOpt.Tree.IniMonitor.Count; j++)
                    {
                        if (MonOpt.Tree.IniMonitor[j].strName == "监控头_2")
                        {
                            MonOpt.Tree.IniMonitor[j].MoState = 1;//写入状态，1表示在线，0表示停止运行
                        }
                    }
                }
                else if (str1 == "三号")
                {
                    for (int j = 0; j < MonOpt.Tree.IniMonitor.Count; j++)
                    {
                        if (MonOpt.Tree.IniMonitor[j].strName == "监控头_3")
                        {
                        }
                    }
                }
                else if (str1 == "四号")
                {
                    for (int j = 0; j < MonOpt.Tree.IniMonitor.Count; j++)
                    {
                        if (MonOpt.Tree.IniMonitor[j].strName == "监控头_4")
                        {
                        }
                    }
                }
            }
            MonOpt.Tree.lststrReceiCmd.Clear();
        }

      
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData; //可以是任意值
            public int cbData;    //指定lpData内存区域的字节数
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData; //发送给目录窗口所在进程的数据
        }

        const int WM_COPYDATA = 0x004A;
        protected override void DefWndProc(ref System.Windows.Forms.Message m)//心跳命令
        {
            if (m.Msg == WM_COPYDATA)
            {
                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                Type t = cds.GetType();
                cds = (COPYDATASTRUCT)m.GetLParam(t);
                string receiveInfo = cds.lpData;//接收心跳命令
                if ((receiveInfo == "CmdServerStart") || (receiveInfo == "CmdServerStop"))//命令服务器——CmdServerStart启动——CmdServerStop停止 命令
                {
                    //启动
                    stu.strName = "命令服务器";
                    stu.strCmd = receiveInfo;
                    stu.strDateTime = DateTime.Now.ToString();
                }
                else if (receiveInfo == "监控头_1")//监控模块的心跳命令
                {
                    for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
                    {
                        if ((MonOpt.Tree.IniMonitor[i].strName == receiveInfo) || (MonOpt.Tree.IniMonitor[i].strName == receiveInfo))
                        {
                            MonOpt.Tree.IniMonitor[i].strName = receiveInfo;   //赋
                            MonOpt.Tree.IniMonitor[i].dateTine = DateTime.Now;  //心跳时间
                            MonOpt.Tree.IniMonitor[i].MoState = 1;
                        }
                    }
                }
                else if ((receiveInfo == "") || (receiveInfo == ""))    //数据分析服务器心跳命令
                {

                }
            }
            else
            {
                base.DefWndProc(ref m);
            }
        }
        //


        public void funMonitorCmd(string strNamestate)//
        {

        }
        private void treeView2_MouseDown_1(object sender, MouseEventArgs e)
        {

            if (treeView2.SelectedNode == null)
                treeView2.ContextMenuStrip = null;
            else
                treeView2.ContextMenuStrip = contextMenuStrip2;
        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (stu.strCmd == "CmdServerStart")
            {
                txbOneMonDevIndex.Text = "1";
                txbOneMonDevTypeName.Text = "命令服务器";
                txbOneMonDevWorkState.Text = "启动";
                txbOneMonDevWorkStateUpateTime.Text = stu.strDateTime;
            }
            else
            {
                txbOneMonDevIndex.Text = "1";
                txbOneMonDevTypeName.Text = "命令服务器";
                txbOneMonDevWorkState.Text = "停止";
                txbOneMonDevWorkStateUpateTime.Text = stu.strDateTime;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //启动检测心跳的进程
            Satrt();
        }
    }
}
