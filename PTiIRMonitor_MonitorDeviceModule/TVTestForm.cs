using PTiIRMonitor_MonitorDeviceModule.ctrl;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Windows.Forms;

namespace Peiport_pofessionalMonitorDeviceClient
{
    public partial class TVTestForm : Form
    {
        private Form1 frm1;
        private GlobalCtrl globalCtrl;
        public TVTestForm(Form1 frm1)
        {
            this.frm1 = frm1;
            globalCtrl = frm1.M_ClientOpt.globalCtrl;
            InitializeComponent();
            tBox_ip.Text = globalCtrl.tvCtrl.IP;
            tBox_port.Text = globalCtrl.tvCtrl.port.ToString();
            label4.Text = globalCtrl.tvCtrl.GetTVStatus().ToString();
            tBox_username.Text = globalCtrl.tvCtrl.username;
            tBox_psw.Text = globalCtrl.tvCtrl.password;
            tBox_channel.Text = globalCtrl.tvCtrl.channel.ToString();
        }

        private void frm_close(object sender, FormClosingEventArgs e)
        {
            frm1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = globalCtrl.tvCtrl.GetTVStatus().ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            globalCtrl.tvCtrl.DisConnect();
            label4.Text = globalCtrl.tvCtrl.GetTVStatus().ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string fileName = "HKTVPIC" + DateUtil.DateToString() + ".jpg";
            if (globalCtrl.tvCtrl.SaveImage(fileName))
            {
                MessageBox.Show(string.Format("抓图成功:图片名称:{0}", fileName), "提示");
            }
            else
            {
                MessageBox.Show(string.Format("抓图图片失败"), "提示");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (globalCtrl.tvCtrl.Connect(false))
            {
                MessageBox.Show(string.Format("操作成功"), "提示");
            }
            else
            {
                MessageBox.Show(string.Format("操作失败"), "提示");
            }
            label4.Text = globalCtrl.tvCtrl.GetTVStatus().ToString();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (globalCtrl.tvCtrl.Connect(true))
            {
                MessageBox.Show(string.Format("操作成功"), "提示");
            }
            else
            {
                MessageBox.Show(string.Format("操作失败"), "提示");
            }
            label4.Text = globalCtrl.tvCtrl.GetTVStatus().ToString();

        }

        private void button11_Click(object sender, EventArgs e)
        {
            globalCtrl.tvCtrl.SetPrePos(Convert.ToInt32(num_Index.Value));
        }

        private void button10_Click(object sender, EventArgs e)
        {
            globalCtrl.tvCtrl.InvokePrePos(Convert.ToInt32(num_Index.Value));
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label9.Text = "速度:" + trackBar1.Value.ToString("000");
        }

        private void button15_MouseDown(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(-1 * trackBar1.Value, trackBar1.Value);
        }

        private void button15_MouseUp(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(0, 0);
        }

        private void button17_MouseDown(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(0, trackBar1.Value);
        }

        private void button17_MouseUp(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(0, 0);
        }

        private void button14_MouseDown(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(trackBar1.Value, trackBar1.Value);
        }

        private void button14_MouseUp(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(0, 0);
        }

        private void button19_MouseDown(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(-1 * trackBar1.Value, 0);
        }

        private void button19_MouseUP(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(0, 0);
        }

        private void button18_MouseDown(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(trackBar1.Value, 0);
        }

        private void button18_MouseUp(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(0, 0);
        }

        private void button13_MouseDown(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(-1 * trackBar1.Value, -1 * trackBar1.Value);
        }

        private void button13_MouseUp(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(0, 0);
        }

        private void button16_MouseDown(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(0, -1 * trackBar1.Value);
        }

        private void button16_MouseUp(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(0, 0);
        }

        private void button12_MouseDown(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(trackBar1.Value, -1 * trackBar1.Value);
        }

        private void button12_MouseUp(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.PTZMove(0, 0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (globalCtrl.tvCtrl.SetZoomOpt(-2))
            {
                MessageBox.Show("缩小倍数:" + 2, "提示");
            }
            else
            {
                MessageBox.Show("缩小操作失败", "提示");
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (globalCtrl.tvCtrl.SetZoomOpt(2))
            {
                MessageBox.Show("放大倍数:" + 2, "提示");
            }
            else
            {
                MessageBox.Show("放大操作失败", "提示");
            }
        }

        private void button9_MouseDown(object sender, MouseEventArgs e)
        {
            if (globalCtrl.tvCtrl.FacusOpt(1))
            {
                MessageBox.Show("焦距拉远");
            }
            else
            {
                MessageBox.Show("操作失败");
            }
        }

        private void button9_MosueUp(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.FacusOpt(0);
        }

        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
            if (globalCtrl.tvCtrl.FacusOpt(-1))
            {
                MessageBox.Show("焦距拉近");
            }
            else
            {
                MessageBox.Show("操作失败");
            }
        }

        private void button8_MouseUp(object sender, MouseEventArgs e)
        {
            globalCtrl.tvCtrl.FacusOpt(0);
        }
    }
}
