using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Net;
namespace Peiport_commandManegerSystem
{
    public partial class Form1 : Form
    {
        public string strMessageDispBuf = "";
        public Form1()
        {
            InitializeComponent();
            ClientOpt.Pjson.frmMain = this;
        }
        public void guiFormChangeDeal() 
        {
            int intHeight, intWidth;
            intHeight = TabControl1.Height;
            intWidth = TabControl1.Width;
            if (intHeight < 600)
                intHeight = 600;
            if (intWidth < 800)
                intWidth = 800;
            groupBox1.Width = intWidth - groupBox1.Left - 30;
            groupBox1.Height = intHeight / 2 - groupBox1.Top - 180;
            groupBox2.Top = groupBox1.Top + groupBox1.Height + 20;
            groupBox2.Width = groupBox1.Width;
            groupBox2.Height = intHeight - groupBox2.Top - 50;
        }
        ///////////////////////////////////////////////////////
        clsCmdClietnOpt ClientOpt = new clsCmdClietnOpt();
        //////////////////////////////////////////////////////
        private void btn_ConnectServer_Click(object sender, EventArgs e)//连接
        {
            int intport;
            IPAddress ipaIP;
            if (int.TryParse(txbJosnServerPort.Text, out intport) == false)
                return;
            if (IPAddress.TryParse(txbJosnServerIP.Text, out ipaIP) == false)
            {
                return;
            }

            ClientOpt.funEnterUserClientPar(ipaIP.ToString(), intport);
            ClientOpt.funSetupUserClient();
            ClientOpt.UpdateIniBack(ipaIP.ToString(),intport);//写入ini
        }

        private void btnDisConnectServer_Click(object sender, EventArgs e)//断开
        {
            ClientOpt.funStopUserClient();
        }

        private void btn_JosnServerLogin_Click(object sender, EventArgs e)//登录
        {
            string str1 = txbJsonUserName.Text;
            string str2 = txbJsonPassword.Text;
            if ((str1 != "") && (str2 != ""))
            {
                ClientOpt.funOptbtuUser(str1, str2);
            }
        }

        private void btn_JosnServerLogin_out_Click(object sender, EventArgs e)//退出登录
        {
            btn_QuestLoginStatus_Click(sender, e);
            if (ClientOpt.LoginStatus == true)
            {
                string str = txbJsonUserName.Text;
                if (str != "")
                {
                    ClientOpt.funboutOptOut(str);
                }
            }
        }

        private void btn_Status_QuestConnectStatus_Click(object sender, EventArgs e)//查看连接状态
        {
            ClientOpt.funbutLoginStatus();
            if (ClientOpt.bl_UserClientControlSetup == true)
            {
                label5.Text = "已连接";
            }
            else
            {
                label5.Text = "未连接";
            }
        }

        private void btn_QuestLoginStatus_Click(object sender, EventArgs e)//查看登录状态
        {
            if (ClientOpt.bl_UserClientControlSetup == true)
            {
                ClientOpt.funbutLoginStatus();
                if (ClientOpt.LoginStatus == true)
                {
                    label6.Text = "已登录";
                }
                else
                {
                    label6.Text = "未登录";
                }
            }
            else
            {
                MessageBox.Show("连接已经断开，请重新连接后登录!!!");
            }
        }

        private void btn_ServerJson_Click(object sender, EventArgs e)//转换json格式
        {
            string strDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");//标识
            string strCmdType = Tex_CmdType.Text;//类别
            string strCmdAction = Tex_CmdAction.Text;//命令名称
            string strresuLt = Tex_resuLt.Text;//返回参数
            string strParamList = Tex_ParamList.Text;//参数
            string strUser = txbJsonUserName.Text;//本地用户地址
            string strreceiver = Tex_receiver.Text;//选择发送的地址
            JObject JobCmd = new JObject();
            JobCmd.Add(new JProperty("seq", strDatetime));
            JobCmd.Add(new JProperty("cmdType", strCmdType));
            JobCmd.Add(new JProperty("cmdAction", strCmdAction));
            JobCmd.Add(new JProperty("result", strresuLt));
            JobCmd.Add(new JProperty("sender", strUser));
            JobCmd.Add(new JProperty("receiver", strreceiver));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("value", strParamList));
            jarr.Add(jobbuf);
            JobCmd.Add(new JProperty("paramList", jarr));
            string strjson;
            strjson = JsonConvert.SerializeObject(JobCmd);
            Tex_SendBuf.Text = strjson;
        }

        private void button10_Click(object sender, EventArgs e)//清空
        {
            txbReceiBuf.Text = "";
        }

        private void btn_ServerJsonManegSend_Click(object sender, EventArgs e)//发送命令
        {
            string str1;
            str1 = Tex_SendBuf.Text;
            if (ClientOpt.bl_UserClientControlSetup == true)
            {
                if (str1 != "")
                {
                    ClientOpt.funSendToUserOneStrCmd(str1);
                }
            }
            else
            {
                MessageBox.Show("异常！！！！");
            }
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)//上
        {
            string struser = txbJsonUserName.Text;//本地地址
            string strMOuser = Tex_receiver.Text;//选择发送的地址
            ClientOpt.funCmdClientSend_PTZMove(0, tkb_JsonControl_PanTilt_Vole.Value, struser, strMOuser);
        }


        private void Down_MouseDown(object sender, MouseEventArgs e)//下
        {
            string struser = txbJsonUserName.Text;//本地地址
            string strMOuser = Tex_receiver.Text;//选择发送的地址
            ClientOpt.funCmdClientSend_PTZMove(0, -1 * tkb_JsonControl_PanTilt_Vole.Value, struser, strMOuser);
        }

        private void Left_MouseDown(object sender, MouseEventArgs e)//左
        {
            string struser = txbJsonUserName.Text;//本地地址
            string strMOuser = Tex_receiver.Text;//选择发送的地址
            ClientOpt.funCmdClientSend_PTZMove(-1 * tkb_JsonControl_PanTilt_Vole.Value, 0, struser, strMOuser);
        }

        private void Right_MouseDown(object sender, MouseEventArgs e)//右
        {
            string struser = txbJsonUserName.Text;//本地地址
            string strMOuser = Tex_receiver.Text;//选择发送的地址
            ClientOpt.funCmdClientSend_PTZMove(tkb_JsonControl_PanTilt_Vole.Value, 0, struser, strMOuser);
        }

        private void btnJson_PTZInit_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_PTZInit(struser, strMouser);
        }

        private void btnJson_PTZAngleGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_PTZAngleGet(struser, strMouser);
        }

        private void btnJson_PrePosInvoke_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(txb_PrePosNO.Text);//预置位
            ClientOpt.funCmdClientSend_PrePosInvoke(inuNo, struser, strMouser);
        }

        private void btnJson_PrePosSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(txb_PrePosNO.Text);//预置位
            ClientOpt.funCmdClientSend_PrePosSet(inuNo, struser, strMouser);
        }

        private void btnJson_PTZMoveAngleSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inHonAngle = Convert.ToInt32(tex_honAngle.Text);
            int inyVelo = Convert.ToInt32(tex_verAngle.Text);
            ClientOpt.funCmdClientSend_PTZMoveAngleSet(inHonAngle, inyVelo, struser, strMouser);
        }
        //四种类型：WhiteHot/BlackHot/Iron/Rain
        private void btnJson_WhiteHot_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            string strWhiteHot = "WhiteHot";
            ClientOpt.funCmdClientSend_PaletteSet(strWhiteHot, struser, strMouser);
        }

        private void btnJson_BlackHot_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            string strBlackHot = "BlackHot";
            ClientOpt.funCmdClientSend_PaletteSet(strBlackHot, struser, strMouser);
        }

        private void btnJson_Iron_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            string strIron = "Iron";
            ClientOpt.funCmdClientSend_PaletteSet(strIron, struser, strMouser);
        }

        private void btnJson_Rain_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            string strRain = "Rain";
            ClientOpt.funCmdClientSend_PaletteSet(strRain, struser, strMouser);
        }

        private void btnJson_DigZoomSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inValue = Convert.ToInt32(tex_value.Text);
            ClientOpt.funCmdClientSend_DigZoomSet(inValue, struser, strMouser);
        }

        private void btnJson_ManualFocus_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            string strfocusType = tex_focusType.Text;
            ClientOpt.funCmdClientSend_ManualFocus(strfocusType, struser, strMouser);
        }

        private void btnJson_AdjustModeSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            string strmode = tex_mode.Text;
            ClientOpt.funCmdClientSend_AdjustModeSet(strmode, struser, strMouser);
        }

        private void btnJson_AutoFocus_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_AutoFocus(struser, strMouser);
        }

        private void btnJson_FocusPosSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_FocusPosSet(struser, strMouser);
        }

        private void btnJson_SaveIRHotImg_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_SaveIRHotImg(struser, strMouser);
        }

        private void btnJson_FocusPosGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_FocusPosGet(struser, strMouser);
        }

        private void btnJson_SaveVideoImg_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_SaveVideoImg(struser, strMouser);
        }

        private void btnJson_DigZoomGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_DigZoomGet(struser, strMouser);
        }

        private void btnJson_ManualAdjustSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int infMax = Convert.ToInt32(tex_fMax.Text);
            int infMin = Convert.ToInt32(tex_fMin.Text);
            ClientOpt.funCmdClientSend_ManualAdjustSet(infMax, infMin, struser, strMouser);
        }

        private void btnJson_EmissivitySet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inmeiss = Convert.ToInt32(tex_meiss.Text);
            ClientOpt.funCmdClientSend_EmissivitySet(inmeiss, struser, strMouser);
        }

        private void btnJson_EmissivityGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_EmissivityGet(struser, strMouser);
        }
        private void btnJson_RefTempSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            float fFvalue;
            float.TryParse(tex_fValue.Text, out fFvalue);
            ClientOpt.funCmdClientSend_RefTempSet(fFvalue, struser, strMouser);
        }

        private void btnJson_RefTempGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_RefTempGet(struser, strMouser);
        }

        private void btnJson_DistanceSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            float fFvalue;
            float.TryParse(texDistanceSet_fValue.Text, out fFvalue);
            ClientOpt.funCmdClientSend_DistanceSet(fFvalue, struser, strMouser);
        }

        private void btnJson_DistanceGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_DistanceGet(struser, strMouser);
        }

        private void btnJson_ZoomOpt_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            string strZoomType = tex_zoomType.Text;
              ClientOpt.funCmdClientSend_ZoomOpt(strZoomType, struser, strMouser);
            //if (i != 0)
            //{
            //    MessageBox.Show("Send cmd error,Error Ma is " + i.ToString());
            //}
        }

        private void btnJson_ZoomPosSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int infValue = Convert.ToInt32(texZoomPosSet_fValue.Text);
            ClientOpt.funCmdClientSend_ZoomPosSet(infValue, struser, strMouser);
        }

        private void btnJson_ZoomPosGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_ZoomPosGet(struser, strMouser);
        }

        private void btnJson_SaveImg_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_SaveImg(struser, strMouser);
        }

        private void btnJson_AnaStateGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSned_AnaStateGet(struser, strMouser);
        }

        private void btnJson_AnaClearAll_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.fnnCmdClientSend_AnaClearAll(struser, strMouser);
        }

        private void btnJson_TempHumGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_TempHumGet(struser, strMouser);
        }

        private void btnJson_AirTempSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            float fFvalue = float.Parse(textBox17.Text);
            ClientOpt.funCmdClientSend_AirTempSet(fFvalue, struser, strMouser);
        }

        private void btnJson_AirTempGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_AirTempGet(struser, strMouser);
        }

        private void btnJson_AirHumSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            float fFvalue = float.Parse(textBox18.Text);
            ClientOpt.funCmdClientSend_AirHumSet(fFvalue, struser, strMouser);
        }
        private void btnJson_AirHumGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_AirHumGet(struser, strMouser);
        }

        private void btnJson_WinTempSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            //int infValue = Convert.ToInt32(textBox22.Text);
            float fFvalue;
            float.TryParse(textBox22.Text, out fFvalue);
            ClientOpt.funCmdClientSend_WinTempSet(fFvalue, struser, strMouser);
        }

        private void btnJson_WinTempGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_WinTempGet(struser, strMouser);
        }

        private void butJson_WinTrmRateSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int infValue = Convert.ToInt32(textBox21.Text);
            ClientOpt.funCmdClientSend_WinTrmRateSet(infValue, struser, strMouser);
        }

        private void btnJson_WinTrmRateGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_WinTrmRateGet(struser, strMouser);
        }

        private void btnJson_AnaSpotPosGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址x
            int inuNo = Convert.ToInt32(textBox69.Text);
            ClientOpt.funCmdClientSend_AnaSpotPosGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaSpotTempGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox69.Text);
            ClientOpt.funCmdClientSend_AnaSpotTempGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaSpotParamGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox69.Text);
            ClientOpt.funCmdClientSend_AnaSpotParamGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaSpotPosSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox69.Text);
            int infLeftPer = Convert.ToInt32(textBox30.Text);
            int infTopPer = Convert.ToInt32(textBox73.Text);
            ClientOpt.funCmdClientSend_AnaSpotPosSet(inuNo, infLeftPer, infTopPer, struser, strMouser);
        }

        private void btnJson_AnaSpotParamSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox69.Text);
            string struActive = textBox29.Text;
            string struUseLocal = textBox70.Text;
            int infEmiss = Convert.ToInt32(textBox71.Text);
            int infDis = Convert.ToInt32(textBox72.Text);
            ClientOpt.funCmdClientSend_AnaSpotParamSet(inuNo, struActive, struUseLocal, infEmiss, infDis, struser, strMouser);
        }

        private void btnJson_AnaLinePosSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox28.Text);
            int infStartXPer = Convert.ToInt32(textBox23.Text);
            int infStartYPer = Convert.ToInt32(textBox31.Text);
            int infEndXPer = Convert.ToInt32(textBox24.Text);
            int infEndYPer = Convert.ToInt32(textBox32.Text);
            ClientOpt.funCmdClientSend_AnaLinePosSet(inuNo, infStartXPer, infStartYPer, infEndXPer, infEndYPer, struser, strMouser);
        }

        private void btnJson_AnaLineParamSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox37.Text);
            string struActive = textBox36.Text;
            string struUseLocal = textBox34.Text;
            int infEmiss = Convert.ToInt32(textBox35.Text);
            int infDis = Convert.ToInt32(textBox33.Text);
            ClientOpt.funCmdClientSend_AnaLineParamSet(inuNo, struActive, struUseLocal, infEmiss, infDis, struser, strMouser);
        }

        private void btnJson_AnaLinePosGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox38.Text);
            ClientOpt.funCmdClientSend_AnaLinePosGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaLineTempGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox38.Text);
            ClientOpt.funCmdClientSend_AnaLineTempGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaLineParamGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox38.Text);
            ClientOpt.funCmdClientSend_AnaLineParamGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaAreaPosSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox48.Text);
            float fStartXPer = float.Parse(textBox47.Text);
            float fStartYPer = float.Parse(textBox45.Text);
            float fToWidthPer = float.Parse(textBox46.Text);
            float fHeightPer = float.Parse(textBox44.Text);
            ClientOpt.funCmdClientSend_AnaAreaPosSet(inuNo, fStartXPer, fStartYPer, fToWidthPer, fHeightPer, struser, strMouser);
        }

        private void btnJson_AnaAreaPosGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox25.Text);
            ClientOpt.funCmdClientSend_AnaAreaPosGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaAreaParamSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox43.Text);
            string struActive = textBox41.Text;
            string struUseLocal = textBox39.Text;
            int infEmiss = Convert.ToInt32(textBox40.Text);
            int infDis = Convert.ToInt32(textBox26.Text);
            ClientOpt.funCmdClientSend_AnaAreaParamSet(inuNo, struActive, struUseLocal, infEmiss, infDis, struser, strMouser);
        }

        private void btnJson_AnaAreaParamGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox25.Text);
            ClientOpt.funCmdClientSend_AnaAreaParamGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaAreaTempGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox25.Text);
            ClientOpt.funCmdClientSend_AnaAreaTempGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaPolyPosSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox57.Text);
            int infLeftPer1 = Convert.ToInt32(textBox56.Text);
            int infTopPer1 = Convert.ToInt32(textBox54.Text);
            int infLeftPerN = Convert.ToInt32(textBox55.Text);
            int infTopPerN = Convert.ToInt32(textBox53.Text);
            ClientOpt.funCmdClientSend_AnaPolyPosSet(inuNo, infLeftPer1, infTopPer1, infLeftPerN, infTopPerN, struser, strMouser);
        }

        private void bntJson_AnaPolyPosGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox27.Text);
            ClientOpt.funCmdClientSend_AnaPolyPosGet(inuNo, struser, strMouser);
        }

        private void bntJson_AnaPolyParamSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox52.Text);
            string struActive = textBox51.Text;
            string struUseLocal = textBox49.Text;
            int infEmiss = Convert.ToInt32(textBox50.Text);
            int infDis = Convert.ToInt32(textBox42.Text);
            ClientOpt.funCmdClientSend_AnaPolyParamSet(inuNo, struActive, struUseLocal, infEmiss, infDis, struser, strMouser);
        }

        private void btnJson_AnaPolyParamGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox27.Text);
            ClientOpt.funCmdClientSend_AnaPolyParamGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaPolyTempGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox27.Text);
            ClientOpt.funCmdClientSend_AnaPolyTempGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaCirclePosSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox68.Text);
            int infCenterLeftPer = Convert.ToInt32(textBox67.Text);
            int infCenterTopPer = Convert.ToInt32(textBox65.Text);
            int infRadiusWidthPer = Convert.ToInt32(textBox66.Text);
            ClientOpt.funCmdClientSend_AnaCirclePosSet(inuNo, infCenterLeftPer, infCenterTopPer, infRadiusWidthPer, struser, strMouser);
        }

        private void btnJson_AnaCirclePosGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox58.Text);
            ClientOpt.funCmdClientSend_AnaCirclePosGet(inuNo, struser, strMouser);
        }

        private void btnJson_AnaCircleParamSet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox63.Text);
            string struActive = textBox62.Text;
            string struUseLocal = textBox60.Text;
            int infEmiss = Convert.ToInt32(textBox61.Text);
            int infDis = Convert.ToInt32(textBox59.Text);
            ClientOpt.funCmdClientSend_AnaCircleParamSet(inuNo, struActive, struUseLocal, infEmiss, infDis, struser, strMouser);
        }

        private void btnJson_AnaCircleParamGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox58.Text);
            ClientOpt.funCmdClientSend_AnaCircleParamGet(inuNo, struser, strMouser);
        }

        private void bntJson_AnaCircleTempGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inuNo = Convert.ToInt32(textBox58.Text);
            ClientOpt.funCmdClientSend_AnaCircleTempGet(inuNo, struser, strMouser);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ClientOpt.funUserClientReceiCmdDealScan();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (strMessageDispBuf.Length > 0)
            {
                if (txbReceiBuf.Text.Length > 2000)
                    txbReceiBuf.Text = "";
                txbReceiBuf.Text = txbReceiBuf.Text + strMessageDispBuf;
                strMessageDispBuf = "";
            }
        }

        private void btn_JsonServerPalpitate_Click(object sender, EventArgs e)
        {
            string strName = txbJsonUserName.Text;
            ClientOpt.funUserClientSecondScan(strName);
        }

        private void butJson_CruiseSetOK_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_CruiseSet(1, struser, strMouser);
        }

        private void butJson_CruiseSetOut_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_CruiseSet(0, struser, strMouser);
        }

        private void butJson_CruiseStateGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            ClientOpt.funCmdClientSend_CruiseStateGet(struser, strMouser);
        }

        private void butJson_MonDevRestart_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            string strAo = textBox1.Text;
            ClientOpt.funCmdClientSend_MonDevRestart(strAo, struser, strMouser);
        }

        private void butJson_ObjStateGet_Click(object sender, EventArgs e)
        {
            string struser = txbJsonUserName.Text;//本地用户地址
            string strMouser = Tex_receiver.Text;//需要发送的地址
            int inObjType = Convert.ToInt32(textBox2.Text);
            ClientOpt.funCmdClientSend_ObjStateGet(inObjType, struser, strMouser);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guiFormChangeDeal();
        }

        private void Tex_SendBuf_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
