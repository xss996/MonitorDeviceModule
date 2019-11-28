using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace Peiport_commandManegerSystem
{
    public class clsCmdClietnOpt
    {
        Client g_Client = new Client("192.168.123.126", 11573);//
        public clsPJsonCrc Pjson = new clsPJsonCrc();
        public Form1 frmThis;
        public bool bl_UserClientControlSetup = false;

        public bool LoginStatus = false;
        private int intHeartCount = 0;
        public class ParamListItem
        {
            public string param { get; set; }
            public string value { get; set; }
        }
        public class Root
        {
            public string seq { get; set; }
            public string cmdType { get; set; }
            public string cmdAction { get; set; }
            public string result { get; set; }
            public List<ParamListItem> paramList { get; set; }
            public string sender { get; set; }
            public string receiver { get; set; }
        }
        public void funEnterUserClientPar(string strIP, int intPort)  //输入IP、端口号等参数
        {
            g_Client = new Client(strIP, intPort);
        }
        public void funSetupUserClient()   //连接
        {
            Pjson.clsClJsonCrcVarInit();
            Pjson.OptJsonConnectServer();
        }
        public void funStopUserClient()  // 断开
        {
            g_Client.DisconnectServer();
            //  bl_UserClientControlSetup = false;
        }
        public void funQuestUserClientStatus(out int intStatus) //查询客户端工作状态 0-关闭，1-打开，2-出错
        {
            if (bl_UserClientControlSetup == true)
            {
                intStatus = 1;
                return;
            }
            else if (bl_UserClientControlSetup == false)
            {
                intStatus = 0;
                return;
            }
            intStatus = 2;
        }

        //返回值，0-正常，1-服务器未启动，2-用户不存在，3-其它
        public int funSendToUserOneCmd(string strCmd) //发送信息
        {
            if (bl_UserClientControlSetup == true)
            {
                Pjson.funSendOneFramCmd(strCmd);
                return 0;
            }
            else
            {

            }
            return -1;
        }
        public int funSendToUserOneStrCmd(string strCmd) //发送信息
        {
            if (bl_UserClientControlSetup == true)
            {
                Pjson.funSendOneFramCmd(strCmd);
                return 0;
            }
            else
            {

            }
            return -1;
        }
        public void funbutLoginStatus() //查看连接状态
        {
            Pjson.OptJsonQuestConnectStatus();
            if (Pjson.m_stuSystemVar.intConnectStatus > 0)
            {
                bl_UserClientControlSetup = true;
            }
            else
            {
                bl_UserClientControlSetup = false;
            }
            if (Pjson.m_stuSystemVar.intLoginStatus > 0)
            {
                LoginStatus = true;
            }
            else
            {
                LoginStatus = false;
            }
        }
        public void funUserClientReceiCmdDealScan()//定时100ms接收返回命令
        {
            Pjson.funReceiCmdAnalyseScan();
        }
        #region 系统
        public void funOptbtuUser(string name, string port) //登录
        {
            Pjson.funCmdJson_Login(name, port);
        }
        public void funboutOptOut(string srt) //退出登录
        {
            Pjson.funCmdJson_Logout(srt);
        }
        //定时级别扫描，检验有没有心跳更新而断开连接,
        //1)检查客户端是否异常，异常即重连
        //2)检查用户是否有心跳，无心跳断开
        public void funUserClientSecondScan(string struser)//////?
        {
            //
            Pjson.OptJsonServerAutoConnectScan();//检测是否异常
            while (true)
            {
                Application.DoEvents();
                intHeartCount++;
                if (intHeartCount > 60)
                {
                    intHeartCount = 0;
                    Pjson.funCmdJson_Palpitate(struser);//发送心跳命令
                    Application.DoEvents();
                    Thread.Sleep(1000);
                    if (Pjson.m_stuSystemVar.intPalpitate == 1)
                    {
                        Pjson.m_stuSystemVar.intPalpitate = 0;
                        MessageBox.Show("心跳成功");
                        // break;
                    }
                    else if (Pjson.m_stuSystemVar.intPalpitate == 0)//
                    {
                        Application.DoEvents();
                        Thread.Sleep(2000);
                        if (Pjson.m_stuSystemVar.intPalpitate == 1)
                        {
                            MessageBox.Show("心跳成功");//
                            Pjson.m_stuSystemVar.intPalpitate = 0;
                        }
                        else if(Pjson.m_stuSystemVar.intPalpitate == 0)
                        {
                            Pjson.m_stuSystemVar.intPalpitate = 0;
                            funStopUserClient();
                            MessageBox.Show("无心跳,断开连接");
                        }
                    }
                }

            }
        }
        public void funCmdClientSend_CruiseSet(int inUp, string strUser, string strMouser) //启动/停止巡检
        {
            Pjson.funCmdJson_CruiseSet(inUp, strUser, strMouser);
        }
        public void funCmdClientSend_CruiseStateGet(string strUser, string strMouser) //查巡检状态
        {
            Pjson.funCmdJson_CruiseStateGet(strUser, strMouser);
        }
        public void funCmdClientSend_ObjStateGet(int inObjType, string strUser, string strMouser) //状态查询
        {
            Pjson.funCmdJson_ObjStateGet(inObjType, strUser, strMouser);
        }
        public void funCmdClientSend_MonDevRestart(string strNo, string strUser, string strMouser) //监控头重启
        {
            Pjson.funCmdJson_MonDevRestart(strNo, strUser, strMouser);
        }
        #endregion
        #region 云台
        public void funCmdClientSend_PTZMove(int inxVelo, int inyVelo, string strUser, string strMouser) //云台转动
        {
            Pjson.funCmdJson_PtzMove(inxVelo, inyVelo, strUser, strMouser);//
        }
        public void funCmdClientSend_PTZMoveAngleSet(int inHonAngle, int inVerAngle, string strUser, string strMouser) //云台角度
        {
            Pjson.funCmdJson_PTZMoveAngleSet(inHonAngle, inVerAngle, strUser, strMouser);
        }
        public void funCmdClientSend_PrePosSet(int inNo, string strUser, string strMouser) //云台设置预置位
        {
            Pjson.funCmdJson_PrePosSet(inNo, strUser, strMouser);
        }
        public void funCmdClientSend_PrePosInvoke(int inNo, string strUser, string strMouser) //云台调用预置位
        {
            Pjson.funCmdJson_PrePosInvoke(inNo, strUser, strMouser);
        }
        public void funCmdClientSend_PTZAngleGet(string strUser, string strMouser)//查云台角度
        {
            Pjson.funCmdJson_PTZAngleGet(strUser, strMouser);
        }
        public void funCmdClientSend_PTZInit(string strUser, string strMouser) //云台初始化
        {
            Pjson.funCmdJson_PTZInit(strUser, strMouser);
        }
        #endregion
        #region 可见光
        public void funCmdClientSend_ZoomOpt(string strZoomType, string strUser, string strMouser) //手动变焦
        {
            Pjson.funCmdJson_ZoomOpt(strZoomType, strUser, strMouser);
            //    return 1;
        }
        public void funCmdClientSend_ZoomPosSet(int infValue, string strUser, string strMouser) //直接变焦位置
        {
            Pjson.funCmdJson_ZoomPosSet(infValue, strUser, strMouser);
        }
        public void funCmdClientSend_ZoomPosGet(string strUser, string strMouser)//变焦位置查询
        {
            Pjson.funCmdJson_ZoomPosGet(strUser, strMouser);
        }
        public void funCmdClientSend_SaveImg(string strUser, string strMouser) //存图
        {
            Pjson.funCmdJson_SaveImg(strUser, strMouser);
        }
        #endregion
        #region 红外控制
        public void funCmdClientSend_ManualFocus(string strfocusType, string strUser, string strMouser) //手动聚焦
        {
            Pjson.funCmdJson_ManualFocus(strfocusType, strUser, strMouser);
        }
        public void funCmdClientSend_AutoFocus(string strUser, string strMouser)//自动聚焦 
        {
            Pjson.funCmdJson_AutoFocus(strUser, strMouser);
        }
        public void funCmdClientSend_FocusPosSet(string strUser, string strMouser)//直接聚焦位置 
        {
            Pjson.funCmdJson_FocusPosSet(strUser, strMouser);
        }
        public void funCmdClientSend_FocusPosGet(string strUser, string strMouser)// 聚焦位置查询
        {
            Pjson.funCmdJson_FocusPosGet(strUser, strMouser);
        }
        public void funCmdClientSend_PaletteSet(string strpalType, string strUser, string strMouser)//色板控制 
        {
            Pjson.funCmdJson_PaletteSet(strpalType, strUser, strMouser);
        }
        public void funCmdClientSend_DigZoomSet(int inValue, string strUser, string strMouser)//数字变焦设置 
        {
            Pjson.funCmdJson_DigZoomSet(inValue, strUser, strMouser);
        }
        public void funCmdClientSend_DigZoomGet(string strUser, string strMouser) //数字变焦获取
        {
            Pjson.funCmdJson_DigZoomGet(strUser, strMouser);
        }
        public void funCmdClientSend_AdjustModeSet(string strMode, string srtUser, string strMouser) //调节模式
        {
            Pjson.funCmdJson_AdjustModeSet(strMode, srtUser, strMouser);
        }
        public void funCmdClientSend_ManualAdjustSet(int infMax, int infMin, string strUser, string strMouser) //手动调节
        {
            Pjson.funCmdJson_ManualAdjustSet(infMax, infMin, strUser, strMouser);
        }
        public void funCmdClientSend_SaveIRHotImg(string strUser, string strMouser) //红外热图保存
        {
            Pjson.funCmdJson_SaveIRHotImg(strUser, strMouser);
        }
        public void funCmdClientSend_SaveVideoImg(string strUser, string strMouser)//存视频图 
        {
            Pjson.funCmdJson_SaveVideoImg(strUser, strMouser);
        }
        #endregion
        #region 红外分析
        public void funCmdClientSend_EmissivitySet(float inemiss, string strUser, string strMouser) //辐射率设置
        {
            Pjson.funCmdJson_EmissivitySet(inemiss, strUser, strMouser);
        }
        public void funCmdClientSend_EmissivityGet(string strUser, string strMouser)//辐射率读取 
        {
            Pjson.funCmdJson_EmissivityGet(strUser, strMouser);
        }
        public void funCmdClientSend_RefTempSet(float infValue, string strUser, string strMouser)//反射温度设置 
        {
            Pjson.funCmdJson_RefTempSet(infValue, strUser, strMouser);
        }
        public void funCmdClientSend_RefTempGet(string strUser, string strMouser)//反射温度读取 
        {
            Pjson.funCmdJson_RefTempGet(strUser, strMouser);
        }
        public void funCmdClientSend_AirTempSet(float infValue, string strUser, string strMouser)// 空气温度设置
        {
            Pjson.funCmdJson_AirTempSet(infValue, strUser, strMouser);
        }
        public void funCmdClientSend_AirTempGet(string strUser, string strMouser)//空气温度读取 
        {
            Pjson.funCmdJson_AirTempGet(strUser, strMouser);
        }
        public void funCmdClientSend_AirHumSet(float infValue, string strUser, string strMouser)//空气湿度设置 
        {
            Pjson.funCmdJson_AirHumSet(infValue, strUser, strMouser);
        }
        public void funCmdClientSend_AirHumGet(string strUser, string strMouser)//空气湿度读取 
        {
            Pjson.funCmdJson_AirHumGet(strUser, strMouser);
        }
        public void funCmdClientSend_DistanceSet(float infValue, string strUser, string strMouser) //距离设置
        {
            Pjson.funCmdJson_DistanceSet(infValue, strUser, strMouser);
        }
        public void funCmdClientSend_DistanceGet(string strUser, string strMouser)//距离读取 
        {
            Pjson.funCmdJson_DistanceGet(strUser, strMouser);
        }
        public void funCmdClientSend_WinTempSet(float infValue, string strUser, string strMouser) //窗口温度设置
        {
            Pjson.funCmdJson_WinTempSet(infValue, strUser, strMouser);
        }
        public void funCmdClientSend_WinTempGet(string strUser, string strMouser)//窗口温度读取 
        {
            Pjson.funCmdJson_WinTempGet(strUser, strMouser);
        }
        public void funCmdClientSend_WinTrmRateSet(int infValue, string strUser, string strMouser) //窗口穿透率设置
        {
            Pjson.funCmdJson_WinTrmRateSet(infValue, strUser, strMouser);
        }
        public void funCmdClientSend_WinTrmRateGet(string strUser, string strMouser)//窗口穿透率读取 
        {
            Pjson.funCmdJson_WinTrmRateGet(strUser, strMouser);
        }
        public void funCmdClientSned_AnaStateGet(string strUser, string strMouser)//查当前测温状态
        {
            Pjson.funCmdJson_AnaStateGet(strUser, strMouser);
        }
        public void fnnCmdClientSend_AnaClearAll(string strUser, string strMouser)//清除所有测温点 
        {
            Pjson.funCmdJson_AnaClearAll(strUser, strMouser);
        }
        public void funCmdClientSend_AnaSpotPosSet(int inuNO, int infLeftPer, int infTopPer, string strUser, string strMouser) //点温位置设置
        {
            Pjson.funCmdJson_AnaSpotPosSet(inuNO, infLeftPer, infTopPer, strUser, strMouser);
        }
        public void funCmdClientSend_AnaSpotPosGet(int inuNo, string strUser, string strMouser)//点温位置读取 
        {
            Pjson.funCmdJson_AnaSpotPosGet(inuNo, strUser, strMouser);
        }
        public void funCmdClientSend_AnaSpotParamSet(int inuNo, string struActive, string struUseLocal, int infEmiss, int infDis, string strUser, string strMouser) //点温参数设置
        {
            Pjson.funCmdJson_AnaSpotParamSet(inuNo, struActive, struUseLocal, infEmiss, infDis, strUser, strMouser);
        }
        public void funCmdClientSend_AnaSpotParamGet(int inuNo, string strUser, string strMouser)//点温参数读取 
        {
            Pjson.funCmdJson_AnaSpotParamGet(inuNo, strUser, strMouser);
        }
        public void funCmdClientSend_AnaSpotTempGet(int inuNo, string strUser, string strMouser)//点温度获取 
        {
            Pjson.funCmdJson_AnaSpotTempGet(inuNo, strUser, strMouser);
        }
        public void funCmdClientSend_AnaLinePosSet(int inuNO, int infStartXPer, int infStartYPer, int infEndXPer, int infEndYPer, string strUser, string strMouser) //线温位置设置
        {
            Pjson.funCmdJson_AnaLinePosSet(inuNO, infStartXPer, infStartYPer, infEndXPer, infEndYPer, strUser, strMouser);
        }
        public void funCmdClientSend_AnaLinePosGet(int inuNo, string strUser, string strMouser)//线温位置读取 
        {
            Pjson.funCmdJson_AnaLinePosGet(inuNo, strUser, strMouser);
        }
        public void funCmdClientSend_AnaLineParamSet(int inuNO, string struActive, string struUseLocal, int infEmiss, int infDis, string strUser, string strMouser) //线温参数设置
        {
            Pjson.funCmdJson_AnaLineParamSet(inuNO, struActive, struUseLocal, infEmiss, infDis, strUser, strMouser);
        }
        public void funCmdClientSend_AnaLineParamGet(int inuNo, string strUser, string strMouser) //线温参数读取
        {
            Pjson.funCmdJson_AnaLineParamGet(inuNo, strUser, strMouser);
        }
        public void funCmdClientSend_AnaLineTempGet(int inuNo, string strUser, string strMouser)//线温获取 
        {
            Pjson.funCmdJson_AnaLineTempGet(inuNo, strUser, strMouser);
        }
        public void funCmdClientSend_AnaAreaPosSet(int inuNo, float fStartXPer, float fStartYPer, float fToWidthPer, float fHeightPer, string strUser, string strMouser) //矩形测温位置设置
        {
            Pjson.funCmdJson_AnaAreaPosSet(inuNo, fStartXPer, fStartYPer, fToWidthPer, fHeightPer, strUser, strMouser);
        }
        public void funCmdClientSend_AnaAreaPosGet(int inuNo, string strUser, string strMouser)//矩形测温位置读取 
        {
            Pjson.funCmdJson_AnaAreaPosGet(inuNo, strUser, strMouser);
        }
        public void funCmdClientSend_AnaAreaParamSet(int inuNO, string struActive, string struUseLocal, int infEmiss, int infDis, string strUser, string strMouser) //矩形参温参数设置
        {
            Pjson.funCmdJson_AnaAreaParamSet(inuNO, struActive, struUseLocal, infEmiss, infDis, strUser, strMouser);
        }
        public void funCmdClientSend_AnaAreaParamGet(int inuNO, string strUser, string strMouser) //矩形参温参数获取
        {
            Pjson.funCmdJson_AnaAreaParamGet(inuNO, strUser, strMouser);
        }
        public void funCmdClientSend_AnaAreaTempGet(int inuNO, string strUser, string strMouser)//矩形测温温度获取 
        {
            Pjson.funCmdJson_AnaAreaTempGet(inuNO, strUser, strMouser);
        }
        public void funCmdClientSend_AnaPolyPosSet(int inuNo, int infLeftPer1, int infTopPer1, int infLeftPerN, int infTopPerN, string strUser, string strMouser) //多边形温位置设置
        {
            Pjson.funCmdJson_AnaPolyPosSet(inuNo, infLeftPer1, infTopPer1, infLeftPerN, infTopPerN, strUser, strMouser);
        }
        public void funCmdClientSend_AnaPolyPosGet(int inuNO, string strUser, string strMouser) //多边形温位置读取
        {
            Pjson.funCmdJson_AnaPolyPosGet(inuNO, strUser, strMouser);
        }
        public void funCmdClientSend_AnaPolyParamSet(int inuNo, string struActive, string struUseLocal, int infEmiss, int infDis, string strUser, string strMouser) //多边形温参数设置
        {
            Pjson.funCmdJson_AnaPolyParamSet(inuNo, struActive, struUseLocal, infEmiss, infDis, strUser, strMouser);
        }
        public void funCmdClientSend_AnaPolyParamGet(int inuNo, string strUser, string strMouser)//多边形温参数读取
        {
            Pjson.funCmdJson_AnaPolyParamGet(inuNo, strUser, strMouser);
        }
        public void funCmdClientSend_AnaPolyTempGet(int inuNO, string strUser, string strMouser) //多边形测温温度获取
        {
            Pjson.funCmdJson_AnaPolyTempGet(inuNO, strUser, strMouser);
        }
        public void funCmdClientSend_AnaCirclePosSet(int inuNo, int infCenterLeftPer, int infCenterTopPer, int infRadiusWidthPer, string strUser, string strMouser) //圆形温位置设置
        {
            Pjson.funCmdJson_AnaCirclePosSet(inuNo, infCenterLeftPer, infCenterTopPer, infRadiusWidthPer, strUser, strMouser);
        }
        public void funCmdClientSend_AnaCirclePosGet(int inuNO, string strUser, string strMouser) //读圆形温位置
        {
            Pjson.funCmdJson_AnaCirclePosGet(inuNO, strUser, strMouser);
        }
        public void funCmdClientSend_AnaCircleParamSet(int inuNo, string struActive, string struUseLocal, int infEmiss, int infDis, string strUser, string strMouser) //圆形温参数
        {
            Pjson.funCmdJson_AnaCircleParamSet(inuNo, struActive, struUseLocal, infEmiss, infDis, strUser, strMouser);
        }
        public void funCmdClientSend_AnaCircleParamGet(int inuNO, string strUser, string strMouser) //读圆形温参数
        {
            Pjson.funCmdJson_AnaCircleParamGet(inuNO, strUser, strMouser);
        }
        public void funCmdClientSend_AnaCircleTempGet(int inuNO, string strUser, string strMouser)// 读圆形温
        {
            Pjson.funCmdJson_AnaCircleTempGet(inuNO, strUser, strMouser);
        }
        #endregion
        #region 温湿度
        public void funCmdClientSend_TempHumGet(string strUser, string strMouser)//温湿度获取
        {
            Pjson.funCmdJsob_TempHumGet(strUser, strMouser);
        }
        #endregion
        #region 机器人
        public void funCmdClientSend_WorkStateGet(string strUser, string strMouser) //查状态
        {
            Pjson.fumCmdJson_WorkStateGet(strUser, strMouser);
        }
        public void funCmdClientSend_UrgencyStop(int inflag, string strUser, string strMouser)// 急停
        {
            Pjson.funCmdJson_UrgencyStop(inflag, strUser, strMouser);
        }
        ///////////////////////////////////
        public void funCmdClientSend_ROBMove() //移动
        {

        }
        //////////////////////////////////
        public void funCmdClientSend_CurrentPosGet(string strUser, string strMouser) //查实时位置
        {
            Pjson.funCmdJson_CurrentPosGet(strUser, strMouser);
        }
        public void funCmdClientSend_ManualMovePosSet(int infRefPosX, int infRefPosY, int infAbsAngle, string strUser, string strMouser) //手动走位置
        {
            Pjson.funCmdJson_ManualMovePosSet(infRefPosX, infRefPosY, infAbsAngle, strUser, strMouser);
        }
        public void funCmdClientSend_MoveSpePosSet(string strPosType, string strUser, string strMouser)//走特定位 
        {
            Pjson.funCmdJson_MoveSpePosSet(strPosType, strUser, strMouser);
        }
        public void funCmdClientSend_PowersetUpSet(int insetUp, string strUser, string strMouser) //充电开关
        {
            Pjson.funCmdJson_PowersetUpSet(insetUp, strUser, strMouser);
        }
        #endregion
        #region ini配置
        public void UpdateIniBack(string strIP,int intPort)//写入ini配置，IP端口
        {
            clsOneMonOptFun.UpdateIniBackServe_Client(strIP,intPort);
        }
        #endregion

    }
}

