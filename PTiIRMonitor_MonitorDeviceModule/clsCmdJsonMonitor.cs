using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SocketClientTest;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Deployment.Application;
using PTiIRMonitor_MonitorDeviceModule.ctrl;
using PTiIRMonitor_MonitorDeviceModule.util;
using System.Diagnostics;

namespace Peiport_pofessionalMonitorDeviceClient
{
    public class clsCmdJsonMonitor
    {
        public Form1 frmMain;
        public class JsonName
        {
            public int intSID;
            public byte[] bt4_SID;      //通讯过程JSON用的变量
            public byte[] bt2_SOI;      //通讯过程JSON用的变量

            public string strIP;        //服务器IP
            public int intPort;         //服务器端口号

            public string strUserName;      //用户名：
            public string strPassword;      //密码：
            public int intConnectStatus;  // 0-断开，1-连接，2-重连状态完成
            public int intLoginStatus;//0-未登陆，1-登陆成功
            public int intLoginSend;
            public int intPalpitate = 0;//心跳成功1，心跳失败0

            public int intControlAutoConnect;   //控制为自动连接状态
            public int intAutoReConnectTimes;   //自动重连间隔时间
            public int intAutoDisConnectCount; //当前断开状态计数
            public int intSendCmdConitinueErrorCount;     //连接发送命令出错计数
            public int intReceiCmdReulstConitinueErrorCount; //接收命令回传失败
        }
        public void clsClJsonCrcVarInit()//
        {
            m_stuSystemVar.bt4_SID = new byte[4];
            m_stuSystemVar.bt4_SID[0] = 0x0;
            m_stuSystemVar.bt4_SID[1] = 0x0;
            m_stuSystemVar.bt4_SID[2] = 0x0;
            m_stuSystemVar.bt4_SID[3] = 0x0;
            m_stuSystemVar.bt2_SOI = new byte[2];
            m_stuSystemVar.bt2_SOI[0] = 0xA5;
            m_stuSystemVar.bt2_SOI[1] = 0x5A;
            m_stuSystemVar.intControlAutoConnect = 0;
            m_stuSystemVar.intLoginStatus = 0;
        }
        public struct stuReceiReturnCmdFormat
        {
            public string strResult;
            public string strMsg;
            public string strUser;
            public string strSeq;
            public string strSid;
        }
        public struct stuWholeCmdBuf
        {
            public byte bt_RandCode;
            public byte[] bt4_SID;
            public int intContentLen;
            public byte[] bt_ReByteArray;
            public JObject jobjOneContent;
        }
        public struct stuBaseNumFrame
        {
            public byte[] bt2_SOI;
            public byte bt_RandCode;
            public byte[] bt4_SID;
            public byte[] bt4_ContentSize;  //发送命令时才计算
            public byte[] bt_Content;
            public byte bt_EOI;             //发送命令时才计算
        }
        //////////////////////////////////////////////////////
        public JsonName m_stuSystemVar = new JsonName();
        public Client m_clsClient = null;
        public List<stuReceiReturnCmdFormat> m_lststuReturn = new List<stuReceiReturnCmdFormat>();
        stuWholeCmdBuf stu_ReceiProcessVar = new stuWholeCmdBuf(); //接收到中间变量 内部用
        public List<stuWholeCmdBuf> lststu_ReceiCmdBuf = new List<stuWholeCmdBuf>();  //接收到的命令缓冲
        public List<stuWholeCmdBuf> lststu_ReceiCmdBufS = new List<stuWholeCmdBuf>();
        List<byte> lstbt_ReByteBuf = new List<byte>();  //接收命令字节缓冲，内部使用
        public List<string> lsts_CmdStrBuf = new List<string>();//接收string类型命令
        public List<string> lsts_Cmduser = new List<string>();//存储用户
       
        public bool funByteContentAnaToJsonObject(byte[] btBuf, ref JObject jobjRe)   //分解命令帧
        {
            bool blReFlag = false;

            jobjRe = new JObject();  //清空

            string strBuf = System.Text.Encoding.UTF8.GetString(btBuf);
            string strBufStr = JsonConvert.DeserializeObject(strBuf).ToString();
            lsts_CmdStrBuf.Add(strBufStr);
            try
            {
                jobjRe = JObject.Parse(strBufStr);

                blReFlag = true;
            }
            catch (Exception)
            {

            }
            return blReFlag;
        }
        public void CheckStaus(string strUser, string strName)//收到了命令立马回复，检验是否收到命令
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("Server", "A163"));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strName));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("", ""));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
        }
        public bool funRecieveOneByteToBuf(byte btReBuf)   //收到一个字节内容
        {
            bool blRe = false;
            byte btbuf;
            lstbt_ReByteBuf.Add(btReBuf);
            int intReByteCount = lstbt_ReByteBuf.Count;
            if (intReByteCount <= 0)
            {

            }
            else if (intReByteCount == 1)
            {
                btbuf = 0xA5;// 0xA5;接收字头
                if (lstbt_ReByteBuf[0] != btbuf)
                {
                    lstbt_ReByteBuf.Clear();
                }
            }
            else if (intReByteCount == 2)
            {
                btbuf = 0x5A;// 0x5A;
                if (lstbt_ReByteBuf[1] != btbuf)
                {
                    lstbt_ReByteBuf.Clear();
                }

            }
            else if (intReByteCount == 3)  //接收随机码
            {
                stu_ReceiProcessVar = new stuWholeCmdBuf();
                stu_ReceiProcessVar.bt_RandCode = lstbt_ReByteBuf[2];
                stu_ReceiProcessVar.bt4_SID = new byte[4];
            }
            else if (intReByteCount < 8) //接收SID
            {
                stu_ReceiProcessVar.bt4_SID[intReByteCount - 4] = lstbt_ReByteBuf[intReByteCount - 1];
            }
            else if (intReByteCount == 11) //接收长度
            {
                long lng1;
                lng1 = (int)lstbt_ReByteBuf[10];
                lng1 = lng1 * 256 + (int)lstbt_ReByteBuf[9];
                lng1 = lng1 * 256 + (int)lstbt_ReByteBuf[8];
                lng1 = lng1 * 256 + (int)lstbt_ReByteBuf[7];
                if (lng1 > 65536)
                {

                    lstbt_ReByteBuf.Clear();
                }
                else
                {
                    stu_ReceiProcessVar.intContentLen = (int)lng1;
                }
            }
            else if (intReByteCount == (stu_ReceiProcessVar.intContentLen + 12))//收到最后一个数据
            {
                //校验一帧
                int intCrc, intCrc1;
                intCrc = 0x00;
                for (int i = 2; i < intReByteCount - 1; i++)
                {
                    intCrc = intCrc ^ lstbt_ReByteBuf[i];
                }
                intCrc = intCrc % 256;
                intCrc1 = (int)lstbt_ReByteBuf[intReByteCount - 1];
                if (intCrc == intCrc1)  //异域校验成功
                {
                    // 进入Json命令分解
                    int intCmdContentLen;
                    intCmdContentLen = stu_ReceiProcessVar.intContentLen;
                    if ((intCmdContentLen > 0) && (intCmdContentLen < 65536))
                    {
                        m_stuSystemVar.bt4_SID[0] = stu_ReceiProcessVar.bt4_SID[0];
                        m_stuSystemVar.bt4_SID[1] = stu_ReceiProcessVar.bt4_SID[1];
                        m_stuSystemVar.bt4_SID[2] = stu_ReceiProcessVar.bt4_SID[2];
                        m_stuSystemVar.bt4_SID[3] = stu_ReceiProcessVar.bt4_SID[3];

                        byte[] btbuf1 = new byte[intCmdContentLen];
                        for (int i = 0; i < intCmdContentLen; i++)
                        {
                            btbuf1[i] = (byte)(lstbt_ReByteBuf[i + 11] ^ stu_ReceiProcessVar.bt_RandCode);
                        }
                        JObject jobre = new JObject();
                        stu_ReceiProcessVar.bt_ReByteArray = btbuf1;

                        if (funByteContentAnaToJsonObject(btbuf1, ref jobre) == true)
                        {
                            stu_ReceiProcessVar.jobjOneContent = jobre;
                        }
                        lststu_ReceiCmdBuf.Add(stu_ReceiProcessVar);
                        lststu_ReceiCmdBufS.Add(stu_ReceiProcessVar);
                        blRe = true;
                    }
                }
                //重新清一帧
                lstbt_ReByteBuf.Clear();
            }
            else if (intReByteCount > 50000)  //超长
            {
                //重新清一帧
                lstbt_ReByteBuf.Clear();
            }

            /////////////////
            return blRe;
        }
        public void funReceiCmdAnalyseScan(GlobalCtrl ctrl)   //定时100ms接收到命令解析 用于在中断线程内调用,解析完后，分别生对应的接收命令表
        {
            try
            {
                while (true)
                {
                    
                    if (lststu_ReceiCmdBuf.Count > 0)
                    {
                        string str1;
                        stuWholeCmdBuf mOneCmd = lststu_ReceiCmdBuf[0];                    
                        lststu_ReceiCmdBuf.RemoveAt(0);
                        JObject job1 = mOneCmd.jobjOneContent;
                        string strer = job1["receiver"].ToString();
                        string strName = job1["sender"].ToString();
                        str1 = JsonConvert.SerializeObject(job1);
                        string str2 = "接收到返回信息";
                        #region
                        JObject jobCmd = new JObject();
                        jobCmd.Add(new JProperty("Server", "A5x0"));
                        jobCmd.Add(new JProperty("sender", "CmdServer"));
                        jobCmd.Add(new JProperty("receiver", strer));
                        JArray jarr = new JArray();
                        JObject jobbuf = new JObject();
                        jobbuf.Add(new JProperty("", ""));
                        jarr.Add(jobbuf);

                        jobCmd.Add(new JProperty("", jarr));
                        string strjsons = JsonConvert.SerializeObject(jobCmd);
                        #endregion
                        if (!str1.Equals(strjsons))
                        {
                            frmMain.strMessageDispBuf = str2 + str1 + "\r\n";
                            gLogWriter.WriteLog(str2, str1);
                            lsts_CmdStrBuf.Clear();//清空
                        }
                        string strUser = job1["receiver"].ToString();//获取本地地址
                        string strjson = JsonConvert.SerializeObject(job1).ToString();
                        lsts_Cmduser.Add(strUser);
                        string str4 = "登录命令";
                        frmMain.strMessageDispBuf = str4 + strjson + "\r\n";
                        //  m_stuSystemVar.intLoginStatus = 1;//用户登录成功
                        string strJson = ctrl.ReceiveMsgScan(job1);
                        //if (!job1.ToString().Contains("Server"))
                        //{
                        //    strJson = ctrl.ReceiveMsgScan(job1);
                        //}
                        if (!string.IsNullOrEmpty(strJson))
                        {
                            funSendOneFramCmd(strJson);
                        }
                      //  m_stuSystemVar.intLoginStatus = ctrl.LoginStatus;//用户登录成功
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("提示:命令解析回传过程中出错,异常信息:"+ex.Message);
               // throw ex;

            }
            return;
        }
        public bool funSendOneFramCmd(string strjson)//发送信息
        {
            bool flag = false;
            try
            {
                stuBaseNumFrame stuBf = new stuBaseNumFrame();
                stuBf = funOneContentToBaseFrame(strjson);
                byte[] strByte;
                strByte = funOneBaseNumFrameToByteArray(stuBf);
                string str2 = "发送命令";
               // gLogWriter.WriteLog(str2, strjson);//写入日志
               if(m_clsClient != null && m_clsClient.getConnectState())
                {
                    m_clsClient.SendToServer(strByte);
                    flag =  true;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                Debug.WriteLine("发送指令到服务器出错"+ex.Message);
              //  throw e;
            }
            return flag;
        }
        public stuBaseNumFrame funOneContentToBaseFrame(string jobjBuf)  //一个Josn帧转发为基础帧
        {
            int i;
            stuBaseNumFrame stuRe_BaseNumeFram = new stuBaseNumFrame();
            stuRe_BaseNumeFram.bt2_SOI = new byte[2];
            stuRe_BaseNumeFram.bt2_SOI[0] = m_stuSystemVar.bt2_SOI[0];// 0x85;
            stuRe_BaseNumeFram.bt2_SOI[1] = m_stuSystemVar.bt2_SOI[1];// 0x66;

            Random rd = new Random();
            i = (int)(rd.Next() % 256);
            stuRe_BaseNumeFram.bt_RandCode = (byte)i;
            stuRe_BaseNumeFram.bt4_SID = new byte[4];
            for (i = 0; i < 4; i++)
            {
                stuRe_BaseNumeFram.bt4_SID[i] = m_stuSystemVar.bt4_SID[i];
            }
            string strContentBuf = "";
            strContentBuf = JsonConvert.SerializeObject(jobjBuf);
            stuRe_BaseNumeFram.bt_Content = System.Text.Encoding.UTF8.GetBytes(strContentBuf);
            return stuRe_BaseNumeFram;
        }
        public byte[] funOneBaseNumFrameToByteArray(stuBaseNumFrame m_stuOneBaseNumFramBuf)   //一帧数据转发化字节数组
        {
            int i, intCrc;
            intCrc = 0x00;
            int intlen = m_stuOneBaseNumFramBuf.bt_Content.Length;
            byte[] btRe = new byte[intlen + 12];
            //给字头
            btRe[0] = m_stuOneBaseNumFramBuf.bt2_SOI[0];
            btRe[1] = m_stuOneBaseNumFramBuf.bt2_SOI[1];
            //给随机码
            btRe[2] = m_stuOneBaseNumFramBuf.bt_RandCode;
            intCrc = intCrc ^ btRe[2];
            //给SID
            //for (i = 0; i < 4; i++)
            //{
            //    btRe[3 + i] = m_stuOneBaseNumFramBuf.bt4_SID[i];
            //    intCrc = intCrc ^ btRe[3 + i];
            //}
            //////给长度
            btRe[7] = (byte)(intlen % 256);
            btRe[8] = (byte)(intlen / 256);
            btRe[9] = 0x00;
            btRe[10] = 0x00;
            //给校验

            intCrc = intCrc ^ btRe[7];
            intCrc = intCrc ^ btRe[8];
            intCrc = intCrc ^ btRe[9];
            intCrc = intCrc ^ btRe[10];

            /////给内容
            for (i = 0; i < intlen; i++)
            {
                btRe[11 + i] = (byte)(m_stuOneBaseNumFramBuf.bt_Content[i] ^ m_stuOneBaseNumFramBuf.bt_RandCode);
                intCrc = intCrc ^ btRe[11 + i];
            }

            //给校验
            btRe[intlen + 11] = (byte)(intCrc);

            return btRe;
        }
        public void funCheckStaus(bool blClearReceiBufFirst, int intMaxWaitTimeMs)
        {
            if (blClearReceiBufFirst)
                lststu_ReceiCmdBufS = new List<stuWholeCmdBuf>();
            DateTime dt = new DateTime();
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(500);//
                if (lststu_ReceiCmdBufS.Count > 0)//对方收到
                {
                    lststu_ReceiCmdBufS = new List<stuWholeCmdBuf>();
                    break;
                }
                TimeSpan ts = DateTime.Now - dt;
                if ((ts.TotalMilliseconds > intMaxWaitTimeMs) && (lststu_ReceiCmdBufS.Count == 0))
                {
                    break;
                }
            }
        }
        public void funWaitForResult(bool blClearReceiBufFirst, int intMaxWaitTimeMs)    //发命令等待返回函数
        {
            if (blClearReceiBufFirst)
                m_lststuReturn = new List<stuReceiReturnCmdFormat>();
            DateTime dt1 = DateTime.Now;
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(50);
                if (m_lststuReturn.Count > 0)
                {
                    break;
                }
                TimeSpan ts = DateTime.Now - dt1;
                if ((ts.TotalMilliseconds > intMaxWaitTimeMs) || (ts.TotalMilliseconds > 5000))
                {
                    break;
                }
            }
        }
        public void OptJson_CruiseSet(string strResu, string struser, string strport)//返回巡检命令
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", DateUtil.DateToString()));
            jobCmd.Add(new JProperty("cmdType", 1));
            jobCmd.Add(new JProperty("cmdAction", "CruiseSet"));
            jobCmd.Add(new JProperty("result", strResu));
            jobCmd.Add(new JProperty("sender", struser));
            jobCmd.Add(new JProperty("receiver", strport));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("value", "1"));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funWaitForResult(true, 100);
        }

     
    
        #region 系统

        #region 仅对服务器操作
        //public void OptJsonLogin(string strName, string strPasswor)//用户登录
        //{
        //    string strjson = globalCtrl.sysCtrl.UserLogin(strName,strPasswor);
        //    funSendOneFramCmd(strjson);
        //    funCheckStaus(true, 100);
        //}
        #endregion

        #region 退出用户
        public void OptJsonLogout(string name)
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", DateUtil.DateToString()));
            jobCmd.Add(new JProperty("cmdType", 1));
            jobCmd.Add(new JProperty("cmdAction", "Logout"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", name));
            jobCmd.Add(new JProperty("receiver", "CmdServer"));
            //JArray jarr = new JArray();
            //JObject jobbuf = new JObject();
            //jobbuf.Add(new JProperty("username", name));
            //jarr.Add(jobbuf);
            //jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        #endregion
       
        #region   对于服务器的连接
        public void m_clsClient_OnRecvMsg(object obj, Client.EventArgs_Recv e)
        {
            int i;
            for (i = 0; i < e.btRecvByteBuf.Length; i++)
            {
                byte btbuf;
                btbuf = e.btRecvByteBuf[i];
                if (funRecieveOneByteToBuf(btbuf) == true)
                {

                }
            }
        }
        public void OptJsonServerAutoConnectScan()   //服务器自动重连函数，需定时1秒扫描一次
        {
            OptJsonQuestConnectStatus();  //检查服务器连接情况
            if (m_stuSystemVar.intControlAutoConnect > 0) //当前为启动自动连接状态
            {
                if (m_stuSystemVar.intConnectStatus > 0)  //当前已连接状态
                {
                    m_stuSystemVar.intAutoDisConnectCount = 0;
                    if ((m_stuSystemVar.intSendCmdConitinueErrorCount > 20) || (m_stuSystemVar.intReceiCmdReulstConitinueErrorCount > 20))
                    {
                        OptJsonDisConnectServer();  //断开与重连
                        m_stuSystemVar.intSendCmdConitinueErrorCount = 0;
                        m_stuSystemVar.intReceiCmdReulstConitinueErrorCount = 0;
                    }
                }
                else  //当前为断开状态
                {
                    if (m_stuSystemVar.intAutoDisConnectCount < 10000)
                        m_stuSystemVar.intAutoDisConnectCount++;
                    if (m_stuSystemVar.intAutoDisConnectCount > m_stuSystemVar.intAutoReConnectTimes)  //使重连
                    {
                        m_stuSystemVar.intAutoDisConnectCount = 0;
                        //重新连接
                        OptJsonServerConnectSet(m_stuSystemVar.strIP, m_stuSystemVar.intPort);
                        OptJsonConnectServer();
                    }
                }
            }
            else   //当前为关闭连接状态
            {
                m_stuSystemVar.intAutoDisConnectCount = 0;
            }
        }
        public bool OptJsonServerConnectSet(string strIP, int intPort)
        {
            if (m_clsClient.getConnectState() == true)
                return false;

            IPAddress ipep;
            if (IPAddress.TryParse(strIP, out ipep) == false)
                return false;
            if (intPort < 0)
                return false;

            string str1 = ipep.ToString();
            m_clsClient = new Client(str1, intPort);
            m_stuSystemVar.strIP = str1;
            m_stuSystemVar.intPort = intPort;
            str1 = "IP地址:" + strIP + "，端口号:" + intPort.ToString();
            gLogWriter.WriteLog("连接Json服务器", str1);
            return true;
        }
        public void m_clsClient_OnConnect(object obj, Client.EventArgs_Connect e)//连接状态
        {
            if (m_stuSystemVar.intLoginStatus > 0)
            {
                m_stuSystemVar.intConnectStatus = 2;
                m_stuSystemVar.intLoginStatus = 0;
            }
            else
            {
                m_stuSystemVar.intConnectStatus = 1;
            }
        }
        public void m_clsClient_OnDisconnect(object obj, Client.EventArgs_Disconnect e)
        {
            m_stuSystemVar.intConnectStatus = 0;
            m_stuSystemVar.intLoginStatus = 0;
        }
        public bool OptJsonConnectServer()
        {
            if (m_clsClient.getConnectState() == true)
            {
                OptJsonDisConnectServer();
            }
            m_clsClient.OnRecvMsg += new Client.EventHandler_Recv(m_clsClient_OnRecvMsg);
            m_clsClient.OnConnect += new Client.EventHandler_Connect(m_clsClient_OnConnect);
            m_clsClient.OnDisconnect += new Client.EventHandler_Disconnect(m_clsClient_OnDisconnect);
            m_clsClient.ConnectServer();
            OptJsonQuestConnectStatus();
            return false;
        }
        public bool OptJsonDisConnectServer()
        {
            m_clsClient.DisconnectServer();
            m_clsClient.OnRecvMsg -= new Client.EventHandler_Recv(m_clsClient_OnRecvMsg);
            m_clsClient.OnConnect -= new Client.EventHandler_Connect(m_clsClient_OnConnect);
            m_clsClient.OnDisconnect -= new Client.EventHandler_Disconnect(m_clsClient_OnDisconnect);
            return true;
        }
        public bool OptJsonQuestConnectStatus()
        {
            if (m_clsClient.getConnectState() == true)
            {
                m_stuSystemVar.intConnectStatus = 1;
                return true;
            }
            else
            {
                m_stuSystemVar.intConnectStatus = 0;
                m_stuSystemVar.intLoginStatus = 0;
                return false;
            }
        }

        public void DisConnectServer()
        {
            if (m_clsClient.getConnectState())
            {
                m_clsClient.DisconnectServer();

            }
        }

        #endregion
        #endregion


        public bool GetSocketState()
        {
            return m_clsClient.GetSocketState();
        }
    }
}

