using PTiIRMonitor_MonitorDeviceModule.ctrl;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Peiport_pofessionalMonitorDeviceClient
{
    public partial class IRTestForm : Form
    {
        private Form1 frm1;
        private GlobalCtrl globalCtrl;

        public IRTestForm(Form1 frm1)
        {
            this.frm1 = frm1;
            globalCtrl = frm1.M_ClientOpt.globalCtrl;
            InitializeComponent();
            tBox_ip.Text = globalCtrl.irCtrl.IP;
            tBox_port.Text = globalCtrl.irCtrl.port.ToString();
            label4.Text = globalCtrl.IRScanState().ToString();
        }

        private void frm_close(object sender, FormClosingEventArgs e)
        {
            frm1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = globalCtrl.IRScanState().ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label4.Text = globalCtrl.irCtrl.DisConnect().ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int value = 1024;
            value = Convert.ToInt32(textBox1.Text);
            if (globalCtrl.irCtrl.SetFocusPos(value))
            {
                MessageBox.Show("焦距设置成功", "提示");
            }
            else
            {
                MessageBox.Show("焦距设置失败", "提示");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int value = -1;
            if (globalCtrl.irCtrl.GetFocusPos(ref value))
            {
                MessageBox.Show("当前焦距值为:" + value, "提示");
            }
            else
            {
                MessageBox.Show("焦距获取失败", "提示");
            }
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            globalCtrl.irCtrl.SetManualFocus(1);
        }

        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            globalCtrl.irCtrl.SetManualFocus(0);
        }

        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            globalCtrl.irCtrl.SetManualFocus(2);
        }

        private void button6_MouseUp(object sender, MouseEventArgs e)
        {
            globalCtrl.irCtrl.SetManualFocus(0);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (globalCtrl.irCtrl.SetAutoFocus())
            {
                MessageBox.Show("自动聚焦成功!", "提示");
            }
            else
            {
                MessageBox.Show("自动聚焦失败!", "提示");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            globalCtrl.irCtrl.ClearAnaAll();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (globalCtrl.irCtrl.SetMeaParam((float)num_refTemp.Value, (float)num_envTemp.Value, (float)num_hum.Value, (float)num_winTemp.Value,
                    (float)num_winTrans.Value, (float)num_dist.Value, (float)num_emis.Value))
            {
                MessageBox.Show("参数设置成功!", "提示");
            }
            else
            {
                MessageBox.Show("参数设置失败!", "提示");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            float fRefTemp = -1; float fEnvTemp = -1; float fRelHum = -1; float fWinTemp = -1; float fWinTrans = -1;
            float fDist = -1; float fEmis = -1;

            if (globalCtrl.irCtrl.GetMeaParam(ref fRefTemp, ref fEnvTemp, ref fRelHum, ref fWinTemp, ref fWinTrans, ref fDist, ref fEmis))
            {
                MessageBox.Show(string.Format("当前参数:反射温度:{0},环境温度:{1},湿度:{2},窗口温度:{3},窗口反射率:{4},距离:{5},辐射率:{6}", fRefTemp,
                    fEnvTemp, fRelHum, fWinTemp, fWinTrans, fDist, fEmis), "提示");
            }
            else
            {
                MessageBox.Show("参数读取失败!", "提示");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (globalCtrl.irCtrl.SetPalette())
            {
                MessageBox.Show("色板更换成功!", "提示");
            }
            else
            {
                MessageBox.Show("色板更换失败!", "提示");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string fileName = "IRHotPic-" + DateUtil.DateToString() + ".jpg";

            if (globalCtrl.irCtrl.SaveIRHotImage(fileName))
            {
                MessageBox.Show("红外热图抓取成功!", "提示");
            }
            else
            {
                MessageBox.Show("红外热图抓取失败!", "提示");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            globalCtrl.irCtrl.NucCheck();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (button14.Text.Equals("图像自动"))
            {
                if (globalCtrl.irCtrl.SetImageAdjustMode(true, 0, 0))
                {
                    button14.Text = "图像手动";
                    MessageBox.Show("图像自动调节操作成功!", "提示");
                }
                else
                {
                    MessageBox.Show("图像自动调节操作失败!", "提示");
                }
            }
            else
            {
                Random rnd = new Random();
                int iBrigntess = rnd.Next(0, 50);
                int iContrast = rnd.Next(0, 50);
                if (globalCtrl.irCtrl.SetImageAdjustMode(false, iBrigntess, iContrast))
                {
                    button14.Text = "图像自动";
                    MessageBox.Show("图像手动调节操作成功!", "提示");

                }
                else
                {
                    MessageBox.Show("图像手动调节操作失败!", "提示");

                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (globalCtrl.irCtrl.SetAnaSpotPos((int)num_Index.Value, (int)X_axis.Value, (int)Y_axis.Value, (double)num_emissivity.Value, (double)num_distance.Value))
            {
                MessageBox.Show("点区域设置成功!", "提示");
            }
            else
            {
                MessageBox.Show("点区域设置失败,请重试!", "提示");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            double temp = -1;
            if (globalCtrl.irCtrl.GetAnaSpotTemp((int)num_Index.Value, ref temp))
            {
                MessageBox.Show(string.Format("索引{0}的点温为:{1}", num_Index.Value, temp), "提示");
            }
            else
            {
                MessageBox.Show("点温获取失败,请重试!", "提示");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            globalCtrl.irCtrl.SetLevel((double)num_level.Value);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            globalCtrl.irCtrl.SetSpan((double)num_span.Value);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            globalCtrl.irCtrl.SetAnaAreaPos((int)num_areaIndex.Value, (int)num_areaX.Value, (int)num_areaY.Value, (int)num_areaWidth.Value, (int)num_areaHeight.Value, (double)num_areaDist.Value, (double)num_areaEmiss.Value);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Dictionary<string, Object> dict = globalCtrl.irCtrl.GetAnaAreaTemp((int)num_areaIndex.Value);
            MessageBox.Show(string.Format("编号为{0}的区域温度信息:最高温度:{1},最低温度:{2},平均温度:{3},横坐标:{4},纵坐标:{5}", num_areaIndex.Value,
                dict["maxTemp"], dict["minTemp"], dict["avgTemp"], dict["maxAreaX"], dict["maxAreaY"]), "提示");
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (globalCtrl.irCtrl.SetAnaLinePos((int)num_lineIndex.Value, (int)num_lineX1.Value, (int)num_lineY1.Value, (int)num_lineX2.Value, (int)num_lineY2.Value,
                (double)num_lineDist.Value, (double)num_lineEmiss.Value))
            {
                MessageBox.Show("线温设置成功", "提示");
            }
            else
            {
                MessageBox.Show("线温设置失败,请重新设置", "提示");
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> dict = globalCtrl.irCtrl.GetAnaLineTemp((int)num_lineIndex.Value);
            MessageBox.Show(string.Format("编号为{0}的线温温度信息:最高温度:{1},最低温度:{2},平均温度:{3},横坐标:{4},纵坐标:{5}", num_areaIndex.Value,
                dict["maxTemp"], dict["minTemp"], dict["avgTemp"], dict["maxLineX"], dict["maxLineY"]), "提示");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            string fileName = "TVPic-" + DateUtil.DateToString() + ".jpg";
            if (globalCtrl.irCtrl.SaveVideoImage(fileName))
            {
                MessageBox.Show("抓图成功!");
            }
            else
            {
                MessageBox.Show("抓图失败!");
            }
        }
    }
}
