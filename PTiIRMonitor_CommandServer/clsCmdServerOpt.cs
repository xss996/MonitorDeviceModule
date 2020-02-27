using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Forms;
using System.Text;
using System.Net.Sockets;
using System.Net;
//用于全系统操作
namespace Peiport_commandManegerSystem
{

    public class clsCmdServerOpt
    {
        public Form1 frmThis;
        IPAddress ip;

        public bool bl_UserServerControlSetup = false;
        public bool bl_UserServerPalpitate = false;
        public bool bl_MonitorServerControlSetup = false;
        public List<string> bl_UserServerPalpiName = new List<string>();

        public stuConnectImf Imf = new stuConnectImf();
        public stuMonitorImf Ilf = new stuMonitorImf();
        public List<stuConnectImf> ClientTab = new List<stuConnectImf>();//客户端
        public List<stuMonitorImf> MonitorTab = new List<stuMonitorImf>();//监控端

        private int intHeartCount = 0;//定时检查心跳

        public struct stuConnectImf
        {
            public IPAddress strIP;//ip
            public int intPort;//端口
            public string strLoginName;//用户名
            public DateTime dtLastUpdateDateTime;//连接时间
        }
        public struct stuMonitorImf
        {
            public IPAddress strIP;//ip
            public int intPort;//端口
            public string strLoginName;//用户名
            public DateTime dtLastUpdateDateTime;//连接时间
        }

        #region 用户服务器相关
        public void funEnterUserServerPar(string strIP, int intPort)  //输入IP、端口号等参数
        {
            if (IPAddress.TryParse(strIP, out ip) == false)
                frmThis.g_myServer = new Server(ip, intPort);
        }
        public void funSetupUserServer()   //启动，只接供启动变量，不对启动进行监管
        {
            frmThis.m_ClJsonCtrl.OptJsonConnectServer();//启动服务器
        }
        public void funStopUserServer()  //停止，只接供停止变量，不对内部进行控制
        {
            frmThis.g_myServer.StopServer();//停止服务器
        }
        public void funQuestUserServerStatus(out int intStatus)  //查询服务器的工作状态 0-关闭，1-打开，2-出错
        {
            if (frmThis.g_myServer.Thread_Server != null)
            {
                intStatus = 1;
                return;
            }
            else
            {
                intStatus = 0;
                return;
            }
            //if (bl_UserServerControlSetup == true)
            //{
            //    intStatus = 1;
            //    return;
            //}
            //else
            //    if (bl_UserServerControlSetup == false)
            //    {
            //        intStatus = 0;
            //        return;
            //    }
            //intStatus = 2;
        }
        //定时级别扫描，检验有没有心跳更新而断开连接,
        //检查客户端是否异常，异常即重连
        //检查用户是否有心跳，无心跳断开
        public void funUserServerSecondScan()
        {

        }
        ///  返回值，0-正常，1-服务器未启动，2-用户不存在，3-其它
        /// </summary>
        /// <param name="strFromName">本地用户名</param>
        /// <param name="strToName">对方用户名</param>
        /// <param name="strCmd">命令</param>
        /// <returns></returns>
        /// 
        public void SendOneMonitorMsg(int intLstIndex, string strCmd)        //发送给一个客户端信息,供外部使用
        {

            frmThis.m_MOJsonCtrl.funSendMonitor(intLstIndex, strCmd);

        }
        public void sendOneClientMsg(int intLstIndex, string strCmd)       //发送给一个客户端信息，供外部使用
        {
            frmThis.m_ClJsonCtrl.funSendClient(intLstIndex, strCmd);
        }
        //
        public int funSendToUserOneCmd(string strFromName, string strToName, string strCmd)
        {



            //if (bl_UserServerControlSetup == true)
            //{
            //    for (int i = 0; i < frmThis.g_myServer.g_lstConnectClientTab.Count; i++)
            //    {
            //        if ((strToName == frmThis.g_myServer.g_lstConnectClientTab[0].strLoginUserName))
            //        {
            //            frmThis.m_ClJsonCtrl.funSendOneFramCmd(strToName, strCmd);
            //        }
            //        else
            //        {
            //            return 2;
            //        }
            //    }
            //}
            //else if (bl_UserServerControlSetup == false)
            //{
            //    return 1;
            //}
            return -1;
        }
        public void funForceDisOneUserConnect(string strIP, int strPort, string strUser)  //断开_客户端
        {
            funGetUserServerConnectTab(out ClientTab);
            for (int i = 0; i < ClientTab.Count; i++)
            {
                if ((ClientTab[i].strIP.ToString() == strIP) && (ClientTab[i].intPort == strPort) && (ClientTab[i].strLoginName == strUser))
                {
                    frmThis.g_myServer.closeOneClient(i);
                }
            }
        }
        public void funForceDisOneUserConnectFo(string strIP, int strPort, int inNo)  //断开_客户端
        {
            funGetUserServerConnectTab(out ClientTab);
            for (int i = 0; i < ClientTab.Count; i++)
            {
                if ((ClientTab[i].strIP.ToString() == strIP) && (ClientTab[i].intPort == strPort))
                {
                    frmThis.g_myServer.closeOneClient(inNo);
                }
            }
        }
        public void funGetUserServerConnectTab(out List<stuConnectImf> mlststuClientTab)//返回结果打印在列表上  
        {
            mlststuClientTab = new List<stuConnectImf>();
            for (int i = 0; i < frmThis.g_myServer.g_lstConnectClientTab.Count; i++)
            {
                Imf.strIP = frmThis.g_myServer.g_lstConnectClientTab[i].ipaEndIP;
                Imf.intPort = frmThis.g_myServer.g_lstConnectClientTab[i].intEndPort;
                Imf.dtLastUpdateDateTime = frmThis.g_myServer.g_lstConnectClientTab[i].dtUpateLoadTime;
                Imf.strLoginName = frmThis.g_myServer.g_lstConnectClientTab[i].strLoginUserName;
                mlststuClientTab.Add(Imf);
            }
            return;
        }
        public void funUserServerReceiCmdDealScan() //定时扫描处理接收到的命令 ，返回结果扫印在接收文本框内
        {
            frmThis.m_ClJsonCtrl.funReceiCmdAnalyseScan();//
        }
        #endregion
        #region 监控头服务器相关
        public void funEnterMonitorServerPar(string strIP, int intPort)  //输入IP、端口号等参数
        {
            if (IPAddress.TryParse(strIP, out ip) == false)
                frmThis.C_myServer = new MonitorServer(ip, intPort);
        }
        public void funSetupMonitorServer()   //启动，只接供启动变量，不对启动进行监管
        {
            frmThis.m_MOJsonCtrl.clsClJsonCrcVarInit();
            frmThis.m_MOJsonCtrl.OptJsonConnectServer();//启动监控端服务器
        }
        public void funStopMonitorServer()  // 停止，只接供停止变量，不对内部进行控制
        {
            frmThis.C_myServer.StopServer();//停止服务器
        }
        public void funQuestMonitorServerStatus(out int intMStatus)  //查询监控的工作状态 0-关闭，1-打开，2-出错
        {
            if (frmThis.C_myServer.Thread_Server != null)
            {
                intMStatus = 1;
                return;
            }
            else
            {
                intMStatus = 0;
                return;
            }
            //if (bl_MonitorServerControlSetup == true)
            //{
            //    intMStatus = 1;
            //    return;
            //}
            //else
            //    if (bl_MonitorServerControlSetup == false)
            //    {
            //        intMStatus = 0;
            //        return;
            //    }
            //intMStatus = 2;
        }
        //定时级别扫描，检验有没有心跳更新而断开连接,
        // 1) 检查服务器是否异常，异常即重连
        //2）检查用户是否有心跳，无心跳断开
        public void funMonitorServerSecondScan()
        {

        }
        //
        public void funForceDisOneMonitorConnect(string strIP, int inPort, string strUser)  //断开一个连接
        {
            funGetMonitorServerConnectTab(out MonitorTab);
            for (int i = 0; i < MonitorTab.Count; i++)
            {
                if ((MonitorTab[i].strIP.ToString() == strIP) && (MonitorTab[i].intPort == inPort) && (MonitorTab[i].strLoginName == strUser))
                {
                    frmThis.C_myServer.closeOneClient(i);
                }
            }
        }
        public void funForceDisOneMonitorConnectFo(string strIP, int strPort, int in1)  //断开一个连接
        {
            funGetMonitorServerConnectTab(out MonitorTab);
            for (int i = 0; i < MonitorTab.Count; i++)
            {
                if ((MonitorTab[i].strIP.ToString() == strIP) && (MonitorTab[i].intPort == strPort))
                {
                    frmThis.C_myServer.closeOneClient(in1);
                }
            }
        }
        //返回值，0-正常，1-服务器未启动，2-用户不存在，3-其它
        public int funSendToMonitorOneCmd(string strFromName, string strToName, string strCmd)
        {
            if (bl_MonitorServerControlSetup == true)
            {
                for (int i = 0; i < frmThis.C_myServer.g_lstConnectMonitorTab.Count; i++)
                {
                    if ((strToName == frmThis.C_myServer.g_lstConnectMonitorTab[0].strLoginUserName))
                    {
                        frmThis.m_MOJsonCtrl.funSendOneFramCmd(strToName, strCmd);
                        return 0;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            else if (bl_MonitorServerControlSetup == false)
            {
                return 1;
            }
            return -1;
        }
        public void funGetMonitorServerConnectTab(out List<stuMonitorImf> mlststuMonitorClientTab)   //返回结果打印在列表上  
        {
            mlststuMonitorClientTab = new List<stuMonitorImf>();
            for (int i = 0; i < frmThis.C_myServer.g_lstConnectMonitorTab.Count; i++)
            {
                Ilf.strIP = frmThis.C_myServer.g_lstConnectMonitorTab[i].ipaEndIP;
                Ilf.intPort = frmThis.C_myServer.g_lstConnectMonitorTab[i].intEndPort;
                Ilf.dtLastUpdateDateTime = frmThis.C_myServer.g_lstConnectMonitorTab[i].dtUpateLoadTime;
                Ilf.strLoginName = frmThis.C_myServer.g_lstConnectMonitorTab[i].strLoginUserName;
                mlststuMonitorClientTab.Add(Ilf);
            }
            return;
        }
        public void funMonitorServerReceiCmdDealScan() //定时扫描处理接收到的命令 ，返回结果扫印在接收文本框内
        {
            frmThis.m_MOJsonCtrl.funReceiCmdAnalyseScan();//
        }
        #endregion


        #region 看门狗心跳命令
        Client C_Palpitate = new Client("192.168.31.186", 8900);
        public void funPateServer()//启动心跳客户端
        {
            C_Palpitate.ConnectServer();
        }
        public void funSendToServerPate()//定时发送心跳
        {
            C_Palpitate.SendToServer("命令");//发送
        }
        #endregion

    }
}
