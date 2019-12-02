using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using PTiIRMonitor_MonitorDeviceModule.ctrl;
using System.Diagnostics;

namespace Peiport_pofessionalMonitorDeviceClient
{
    public class clsCmdMonitorClientOpt
    {
        public Form1 frmThis;
        Client g_Client = null;
        public clsCmdJsonMonitor Pjson = new clsCmdJsonMonitor();
        public GlobalCtrl globalCtrl = new GlobalCtrl();
        public bool bl_MonitorClientControlSetup = false;//
        public bool bl_UserClientControlSetup = false;
        public bool LoginStatus = false;
        private int intHeartCount = 0;
        public bool StartHeartBeat = false;
        public class ParamListItem
        {
            public string param { get; set; }
            public string value { get; set; }
        }
        public class JsonItem
        {
            public string seq { get; set; }
            public string cmdType { get; set; }
            public string cmdAction { get; set; }
            public string result { get; set; }
            public List<ParamListItem> paramList { get; set; }
            public string sender { get; set; }
            public string receiver { get; set; }
        }
        public struct stuConnectImf
        {
            public IPAddress strIP;//ip
            public int intPort;//端口
            public string strLoginName;//用户名
            public DateTime dtLastUpdateDateTime;//连接时间
        }
        #region 监控头服务器相关
        clsOneMonOptFun fun = new clsOneMonOptFun();
        int inName = 0;
        ///////////////////
        public int Number_ID()
        {
            inName = fun.funIninumber_ID();
            return inName;
        }
        public void funCondition()//关闭
        {
            fun.funIniCondition();
        }
        public void funSecondScan()//监控头心跳
        {
            fun.funMonitorSecondScan();
        }
        //////////////////
        public void funEnterMonitorServerPar(string strIP, int intPort)//输入IP、端口号等参数
        {
            g_Client = new Client(strIP, intPort);
            Pjson.m_clsClient = g_Client;
        }
        public void funSetupMonitorServer()   //启动，只接供启动变量，不对启动进行监管
        {
           // Pjson.clsClJsonCrcVarInit();
            Pjson.OptJsonConnectServer();
        }
        public void funStopMonitorServer()  // 停止，只接供停止变量，不对内部进行控制
        {
            g_Client.DisconnectServer();
        }
        public void funQuestMonitorServerStatus(out int intMStatus)  //查询监控的工作状态 0-关闭，1-打开，2-出错
        {
            if (bl_MonitorClientControlSetup == true)
            {
                intMStatus = 1;
                return;
            }
            else
                if (bl_MonitorClientControlSetup == false)
            {
                intMStatus = 0;
                return;
            }
            intMStatus = 2;
        }
        public void funOptbtuUser(string name, string password) //登录
        {
            Pjson.clsClJsonCrcVarInit();
           // Pjson.OptJsonLogin(name, password);
            string strjson = globalCtrl.sysCtrl.UserLogin(name, password);
            Pjson.funSendOneFramCmd(strjson);
            Pjson.funCheckStaus(true, 100);

        }

        public void funboutOptOut(string srt) //退出登录
        {
            Pjson.OptJsonLogout(srt);
        }
        public void funbutLoginStatus() //查看状态
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
        public void funSendToUserOneStrCmd(string strCmd) //发送信息
        {
            Pjson.funSendOneFramCmd(strCmd);
            //if (bl_MonitorClientControlSetup == true)
            //{
                
            //    return 0;
            //}
            //else
            //{

            //}
            //return -1;
        }

        //定时级别扫描，检验有没有心跳更新而断开连接,
        //1) 检查服务器是否异常，异常即重连
        public bool intPalpitate_S = false;//心跳成功true，心跳失败false
        //2）检查用户是否有心跳，无心跳断开
        public void funMonitorServerSecondScan(string struser)
        {
            /////////////////////////////
            Pjson.OptJsonServerAutoConnectScan();//检测是否异常
            ////////////////////////////
            while (true)
            {
                Application.DoEvents();
                intHeartCount++;
                if (intHeartCount > 60)
                {
                  //  Pjson.funCmdJson_Palpitate(struser);//发送心跳命令
                    Application.DoEvents();
                    Thread.Sleep(1000);
                    if (Pjson.m_stuSystemVar.intPalpitate == 1)
                    {
                        Pjson.m_stuSystemVar.intPalpitate = 0;
                        MessageBox.Show("心跳成功！！！");
                    }
                    else if (Pjson.m_stuSystemVar.intPalpitate == 0)//
                    {
                        Application.DoEvents();
                        Thread.Sleep(2000);
                        if (Pjson.m_stuSystemVar.intPalpitate == 1)
                        {

                            Pjson.m_stuSystemVar.intPalpitate = 0;
                            MessageBox.Show("心跳成功");
                        }
                        else if (Pjson.m_stuSystemVar.intPalpitate == 0)
                        {
                            Pjson.m_stuSystemVar.intPalpitate = 0;
                            funStopMonitorServer();
                            MessageBox.Show("无心跳,断开连接");
                        }
                    }
                }
            }
        }

        public void funForceDisOneMonitorConnect(string strIP, int strPort)  //断开一个连接
        {
            g_Client.DisconnectServer();
            bl_MonitorClientControlSetup = false;
        }
        //返回值，0-正常，1-监控端未启动，2-用户不存在，3-其它
        public int funSendToMonitorOneCmd(string strFromName, string strToName, string strCmd)
        {

            return -1;
        }
        public void funMonitorServerReceiCmdDealScan() //定时扫描处理接收到的命令 ，返回结果扫印在接收文本框内
        {
            Pjson.funReceiCmdAnalyseScan(globalCtrl);
        }     

        public bool GetSocketState()
        {
            return Pjson.GetSocketState();
        }

        public void SendHeartBeatCmd()   //发送心跳命令
        {
            if (StartHeartBeat)
            {
                string strjson = globalCtrl.sysCtrl.SendHeartbeatCmd();
                Pjson.funSendOneFramCmd(strjson);
            }
        }

        public void HeartBeatStatusScan()
        {
            if (StartHeartBeat)
            {
                Debug.WriteLine("心跳检测次数:" + globalCtrl.HeartBeatCount);
            }    
        }

        public void CruiseStatusScan()
        {
            globalCtrl.CruiseSetUp();
        }
        #endregion


        int i = 0;
        int TVReConnectCount = 0;
        int IRReConnectCount = 0;
        int FtpReConnnectCount = 0;
        int SqlReconnectCount = 0;


        public void DevMonitorInit()
        {
            globalCtrl.Init();
            bool b= globalCtrl.TVConnect();
            bool aaa= globalCtrl.IRConnect();
        }
        public void DevStatusScan()
        {           
            IRStatusScan();
            TVStatusScan();
            //FtpStatusScan();
            //SqlStatusScan();
            //Thread th = new Thread(IRStatusScan);
            //th.Start();

            //Thread th2 = new Thread(TVStatusScan);
            //th2.Start();

            //Thread th3 = new Thread(FtpStatusScan);
            //th3.Start();

            //Thread th4 = new Thread(SqlStatusScan);
            //th4.Start();

        }

       
        public void IRStatusScan()
        {
                Debug.WriteLine("红外连接状态:" + globalCtrl.IRScanState());
                if (!globalCtrl.IRScanState())
                {
                    IRReConnectCount++;
                     globalCtrl.IRConnect();
                }
                else
                {
                    IRReConnectCount = 0;
                }
                      
        }

        public void TVStatusScan()
        {
                Debug.WriteLine("可见光连接状态:" + globalCtrl.TVScanState());

                if (globalCtrl.TVScanState() < 1)
                {
                    TVReConnectCount++;
                   globalCtrl.TVConnect();
                }
                else
                {
                    TVReConnectCount = 0;
                }
            
            
        }

        public void FtpStatusScan()
        {
            Debug.WriteLine("Ftp连接状态:" + globalCtrl.FtpStatus);
            if (!globalCtrl.FtpStatus)
            {
                FtpReConnnectCount++;
                globalCtrl.GetFtpConnect();               
            }
            else
            {
                FtpReConnnectCount = 0;
            }

        }

        public void SqlStatusScan()
        {
            Debug.WriteLine("数据库状态:" + globalCtrl.DatabaseStatus);
            if (!globalCtrl.DatabaseStatus)
            {
                globalCtrl.GetSqlConnection();
            }
            else
            {
                SqlReconnectCount++;
            }
        }


    }
}
