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
    public partial class Form1 : Form
    {
        clsCmdMonitorOpt MonOpt = new clsCmdMonitorOpt();

        //   public int int1 = 0;//服务器状态操作
        bool MoWaitingone = false;// 监控头状操作
        public Form1()
        {

            InitializeComponent();
            MonOpt.frmThis = this;
            KeyDown += new KeyEventHandler(Form1_KeyDown);
        }
        private void Form1_Load(object sender, EventArgs e)//
        {
            updateGuiDispServerDevLst();//命令服务器
            MonOpt.ConnectServer();//启动命令服务器窗口
            MonOpt.MonitorDevice();//启动监控头窗口
            updateGuiDispMonitorDevLst();
            MonOpt.OptJsonConnectServer();


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
            //txbOneMonDevIndex.Text = MonOpt.Tree.IniMonitor[
            //txbOneMonDevTypeName.Text = "监控头";
            //txbOneMonDevWorkState.Text = "空闲";
            //txbOneMonDevWorkStateUpateTime.Text = DateTime.Now.ToString();
        }
        public void updateGuiDispServer() //服务器
        {
            txbOneMonDevIndex.Text = "1";
            txbOneMonDevTypeName.Text = "命令服务器";
        }
        public void updateGuiDispMonitor()  //监控端
        {

        }
        private void treeView2_MouseDown(object sender, MouseEventArgs e)
        {
            if (treeView2.SelectedNode == null)
                treeView2.ContextMenuStrip = null;
            else
                treeView2.ContextMenuStrip = contextMenuStrip2;
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            updateGuiDispServer();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)//关闭命令服务器
        {

            MonOpt.CmdServerOut();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)//重启
        {
            MonOpt.CmdServerOut();//重启先关闭
            MonOpt.ConnectServer();
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
                MonOpt.CmdServerOut();//关闭命令服务器
                MonOpt.funDeviceKill();//关闭监控头
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
                Thread.Sleep(50);
                DateTime strTime = DateTime.Now;
                for (int i = 0; i < MonOpt.Tree.IniMonitor.Count; i++)
                {
                    if (MonOpt.Tree.IniMonitor[i].strName == "监控头_1")
                    {
                        DateTime strDate = MonOpt.Tree.IniMonitor[i].dateTine;
                        TimeSpan span = strTime - strDate;
                        int n = Convert.ToInt32(span.TotalSeconds);
                        if (n > 14)
                        {
                            MessageBox.Show("监控头_1心跳异常，断开连接");
                            break;
                        }
                    }
                    else if (MonOpt.Tree.IniMonitor[i].strName == "监控头_2")
                    {
                        DateTime strDate = MonOpt.Tree.IniMonitor[i].dateTine;
                        TimeSpan span = strTime - strDate;
                        int n = Convert.ToInt32(span.TotalSeconds);
                        if (n > 14)
                        {
                            MessageBox.Show("监控头_2心跳异常，断开连接");
                            break;
                        }
                    }
                }
            }
            //
            //while (true)
            //{
            //    //Application.DoEvents();
            //    Thread.Sleep(10);
            //    DateTime date2 = DateTime.Now;
            //    DateTime date1 = Convert.ToDateTime(Strtime);
            //    TimeSpan span = date2 - date1;
            //    int n = Convert.ToInt32(span.TotalSeconds);//获取时间的相差值
            //    if (n < 5)
            //    {
            //        MoWaitingone = false;
            //    }
            //    else if ((n >= 20) && (MoWaitingone == false))
            //    {
            //        MessageBox.Show(strName + "心跳异常，断开连接，重新启动!!");
            //        break;
            //    }
            //    else if ((n <= 20) && (MoWaitingone == true))
            //    {
            //      // MessageBox.Show("心跳正常");
            //    }
            //}
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
                        txbOneMonDevWorkState.Text = "在线";
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
                        txbOneMonDevWorkState.Text = "在线";
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
                    if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_3") && (MonOpt.Tree.IniMonitor[i].MoState == 0))
                    {
                        txbOneMonDevWorkState.Text = "停止";
                    }
                    else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_3") && (MonOpt.Tree.IniMonitor[i].MoState == 1))
                    {
                        txbOneMonDevWorkState.Text = "停止";
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
                    if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_4") && (MonOpt.Tree.IniMonitor[i].MoState == 0))
                    {
                        txbOneMonDevWorkState.Text = "停止";
                    }
                    else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_4") && (MonOpt.Tree.IniMonitor[i].MoState == 18))
                    {
                        txbOneMonDevWorkState.Text = "停止";
                    }
                    else if ((MonOpt.Tree.IniMonitor[i].strName == "监控头_4") && (MonOpt.Tree.IniMonitor[i].MoState == 17))
                    {
                        txbOneMonDevWorkState.Text = "在线";
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
                if ((strName == MonOpt.Tree.IniMonitor[i].strName) && (MonOpt.Tree.IniMonitor[i].inPID != 0))//判断是监控头重启还是监控头启动
                {
                    MonOpt.MonitorOut(MonOpt.Tree.IniMonitor[i].inPID);
                    MonOpt.Tree.IniMonitor[i].MoState = 0;//将此监控头的状态清空，0表示停止
                    MonOpt.Tree.IniMonitor[i].inPID = 0;
                    MonOpt.funMonintorDevice(strName);
                }
                else if ((strName == MonOpt.Tree.IniMonitor[i].strName) && (MonOpt.Tree.IniMonitor[i].inPID == 0))
                {
                    MonOpt.funMonintorDevice(strName);
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)//定时接收心跳命令
        {
            string str1;
            for (int i = 0; i < MonOpt.lststrReceiCmd.Count; i++)
            {
                Thread.Sleep(50);
                str1 = MonOpt.lststrReceiCmd[i];
                if (str1 == "一号")//表示一号监控头
                {
                    for (int j = 0; j < MonOpt.Tree.IniMonitor.Count; j++)
                    {
                        if (MonOpt.Tree.IniMonitor[j].strName == "监控头_1")
                        {
                            MonOpt.Tree.IniMonitor[j].MoState = 1;//写入状态，1表示在线，0表示停止运行
                            MonOpt.Tree.IniMonitor[j].dateTine = DateTime.Now;//显示心跳时间
                            MoWaitingone = true;
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
                            MonOpt.Tree.IniMonitor[j].dateTine = DateTime.Now;//显示心跳时间
                        }
                    }
                }
                else if (str1 == "三号")
                {
                    for (int j = 0; j < MonOpt.Tree.IniMonitor.Count; j++)
                    {
                        if (MonOpt.Tree.IniMonitor[j].strName == "监控头_3")
                        {
                            MonOpt.Tree.IniMonitor[j].dateTine = DateTime.Now;//显示心跳时间
                        }
                    }
                }
                else if (str1 == "四号")
                {
                    for (int j = 0; j < MonOpt.Tree.IniMonitor.Count; j++)
                    {
                        if (MonOpt.Tree.IniMonitor[j].strName == "监控头_4")
                        {
                            MonOpt.Tree.IniMonitor[j].dateTine = DateTime.Now;//显示心跳时间
                        }
                    }
                }
            }
            MonOpt.lststrReceiCmd.Clear();
        }

        private void button2_Click(object sender, EventArgs e)//心跳服务器状态
        {
            Thread sat = new Thread(getTimeSpan);
            sat.Start();
            //int inState;
            //MonOpt.funQuestUserServerStatus(out inState);
            //if (inState==1)
            //{
            //    label3.Text = "启动成功";
            //}
            //else if (inState==0)
            //{
            //    label3.Text = "停止";
            //}
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
