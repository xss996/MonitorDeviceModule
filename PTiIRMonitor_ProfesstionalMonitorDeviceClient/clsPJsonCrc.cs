using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
namespace Peiport_commandManegerSystem
{
    public class clsPJsonCrc
    {
        public Form1 frmMain;
        
        public class Constant//消息类型常量类
        {
            public static int OTHER = 0;//其他类型指令
            public static int LOGIN = 1;//登录
            public static int PTZ = 2;//云台
            public static int IR = 3;//红外
            public static int VR = 4;//可见光
            public static int CRUISE = 5;//巡检
            public static int PTSP = 6;//rtsp视频播放
            //信息描述       ////{"cmdType":1,"params":{"password":"admin","username":"admin"},"sql":"20190722150941176"}
            public static string OK = "ok";
            public static string ERROR = "error";

            public int intSID;
        }
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
        public JsonName m_stuSystemVar = new JsonName();
        public struct stuReceiReturnCmdFormat
        {
            public string strResult;
            public string strMsg;
            public string strUser;
            public string strSeq;
            public string strSid;
        }
        public List<stuReceiReturnCmdFormat> m_lststuReturn = new List<stuReceiReturnCmdFormat>();
        stuWholeCmdBuf stu_ReceiProcessVar = new stuWholeCmdBuf(); //接收到中间变量 内部用
        public List<stuWholeCmdBuf> lststu_ReceiCmdBuf = new List<stuWholeCmdBuf>();  //接收到的命令缓冲
        List<stuWholeCmdBuf> lststu_ReceiCmdBufS = new List<stuWholeCmdBuf>();  //接收到的命令缓冲
        public List<string> lstsJsonCmdbuf = new List<string>();//命令
        List<byte> lstbt_ReByteBuf = new List<byte>();  //接收命令字节缓冲，内部使用
        public Client m_clsClient = new Client("192.168.123.126", 11574);
        public void m_clsClient_OnRecvMsg(object obj, EventArgs_Recv e)
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
        public void g_Client_OnRecvMsg(object source, EventArgs_Recv e)
        {
            string str1, str2;
            str2 = e.sRecvTime;
            str1 = e.sRecvMsg;
            str1 = str2 + " _" + str1 + "\r\n";
            str1 = e.sRecvMsg;
            string str3 = "接收到信息";
            frmMain.strMessageDispBuf = str3 + str1;
        }
        public void m_clsClient_OnConnect(object obj, EventArgs_Connect e)//连接状态
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
        public void m_clsClient_OnDisconnect(object obj, EventArgs_Disconnect e)
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
        public void funWaitForResult(bool blClearReceiBufFirst, int intMaxWaitTimeMs)    //发命令等待返回函数
        {
            if (blClearReceiBufFirst)
                m_lststuReturn = new List<stuReceiReturnCmdFormat>();
            DateTime dt1 = DateTime.Now;
            while (true)
            {
                Application.DoEvents();//等待
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
        public void funCheckStaus(bool blClearReceiBufFirst, int intMaxWaitTimeMs)
        {
            if (blClearReceiBufFirst)
                lststu_ReceiCmdBufS = new List<stuWholeCmdBuf>();
            DateTime dt = new DateTime();
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(600);//
                if (lststu_ReceiCmdBufS.Count > 0)//对方收到
                {
                    lststu_ReceiCmdBufS = new List<stuWholeCmdBuf>();
                    break;
                }
                TimeSpan ts = DateTime.Now - dt;
                if ((ts.TotalMilliseconds > intMaxWaitTimeMs) && (lststu_ReceiCmdBufS.Count == 0))
                {
                    MessageBox.Show("命令异常");
                    break;
                }
            }
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
        public bool funSendOneFramCmd(string strjson)//发送信息
        {
            try
            {
                stuBaseNumFrame stuBf = new stuBaseNumFrame();
                stuBf = funOneContentToBaseFrame(strjson);
                byte[] strByte;
                strByte = funOneBaseNumFrameToByteArray(stuBf);
                string str2 = "发送命令";
                gLogWriter.WriteLog(str2, strjson);//写入日志
                m_clsClient.SendToServer(strByte);
                return true;
            }
            catch (Exception)
            {
                return true;
                throw;
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
        public bool funByteContentAnaToJsonObject(byte[] btBuf, ref JObject jobjRe)   //分解命令帧
        {
            bool blReFlag = false;

            jobjRe = new JObject();  //清空

            string strBuf = System.Text.Encoding.UTF8.GetString(btBuf);

            string strBufStr = JsonConvert.DeserializeObject(strBuf).ToString();
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
        public string CheckStaus(string strjson, string strUser)//收到了命令立马回复，检验是否收到命令
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("Server", "A5x0"));
            jobCmd.Add(new JProperty("sender", "CmdServer"));
            jobCmd.Add(new JProperty("receiver", strUser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("", ""));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("", jarr));
            return strjson = JsonConvert.SerializeObject(jobCmd);
        }
        public void funReceiCmdAnalyseScan()   //定时100ms接收到命令解析 用于在中断线程内调用,解析完后，分别生对应的接收命令表
        {
            try
            {
                while (true)
                {
                    if (lststu_ReceiCmdBuf.Count > 0)
                    {
                        stuWholeCmdBuf mOneCmd = new stuWholeCmdBuf();
                        mOneCmd = lststu_ReceiCmdBuf[0];
                        lststu_ReceiCmdBuf.RemoveAt(0);
                        JObject job1 = mOneCmd.jobjOneContent;
                        string strUser = job1["receiver"].ToString();
                        string str1 = JsonConvert.SerializeObject(mOneCmd.jobjOneContent);
                        string str2 = "接收到返回信息";
                        #region
                        ///////////////////////////////////////////
                        JObject jobCmd = new JObject();
                        jobCmd.Add(new JProperty("Server", "A5x0"));
                        jobCmd.Add(new JProperty("sender", "CmdServer"));
                        jobCmd.Add(new JProperty("receiver", strUser));
                        JArray jarr = new JArray();
                        JObject jobbuf = new JObject();
                        jobbuf.Add(new JProperty("", ""));
                        jarr.Add(jobbuf);
                        jobCmd.Add(new JProperty("", jarr));
                        string strjson = JsonConvert.SerializeObject(jobCmd);
                        #endregion
                        ///////////////////////////////////////////
                        if (str1 != strjson)//过滤掉检验是否收到命令帧
                        {
                            frmMain.strMessageDispBuf = str2 + str1 + "\r\n";
                            gLogWriter.WriteLog(str2, str1);
                        }
                        #region 检验接收返回的命令
                        // 接收用户登录返回命令
                        if ((job1["cmdType"].ToString() == "1") && (job1["cmdAction"].ToString() == "Login") && (job1["Params"][0]["sender"].ToString() == "登录服务器成功"))
                        {
                            m_stuSystemVar.intLoginStatus = 1;
                        }
                        else if ((job1["cmdType"].ToString() == "1") && (job1["cmdAction"].ToString() == "Logout") && (job1["Params"][0]["sender"].ToString() == "退出成功"))
                        {
                            m_stuSystemVar.intLoginStatus = 0;
                        }
                        else if ((job1["cmdType"].ToString() == "1") && (job1["cmdAction"].ToString() == "Palpitate") && (job1["Params"][0]["sender"].ToString() == "心跳成功"))
                        {
                            m_stuSystemVar.intPalpitate = 1;
                        }
                        else if ((job1["cmdType"].ToString() == "1") && (job1["cmdAction"].ToString() == "Login") && (job1["Params"][0]["sender"].ToString() == "lisi"))
                        {
                            m_stuSystemVar.intLoginStatus = 1;
                        }
                        else if ((job1["cmdType"].ToString() == "1") && (job1["cmdAction"].ToString() == "Logout") && (job1["Params"][0]["sender"].ToString() == "登录服务器成功"))
                        {
                            m_stuSystemVar.intLoginStatus = 0;
                        }
                        #endregion
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return;
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
            for (i = 0; i < 4; i++)
            {
                btRe[3 + i] = m_stuOneBaseNumFramBuf.bt4_SID[i];
                // btRe[3 + i] = 0x00;
                intCrc = intCrc ^ btRe[3 + i];
            }
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
        #region  控制命令

        string strDatetime = DateTime.Now.ToString("yyyyMMddhhmmssfff");
        #region 系统
        #region 心跳检测
        public void funCmdJson_Palpitate(string user)
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 1));
            jobCmd.Add(new JProperty("cmdAction", "Palpitate"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", user));
            jobCmd.Add(new JProperty("receiver", "CmdServer"));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("result",user));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);//等待
        }
        #endregion
        #region 登录
        public void funCmdJson_Login(string strName, string strport)
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 1));
            jobCmd.Add(new JProperty("cmdAction", "Login"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strName));
            jobCmd.Add(new JProperty("receiver", "CmdServer"));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("param", strport));
            jobbuf.Add(new JProperty("value",strName));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);//等待
        }
        #endregion
        #region 退出用户
        public void funCmdJson_Logout(string name)
        {

            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 1));
            jobCmd.Add(new JProperty("cmdAction", "Logout"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", name));
            jobCmd.Add(new JProperty("receiver", "CmdServer"));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("username", name));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);//等待
        }
        #endregion
        #region 启动/停止巡检
        public void funCmdJson_CruiseSet(int inUp, string struser, string strMOuser)
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 1));
            jobCmd.Add(new JProperty("cmdAction", "CruiseSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", struser));
            jobCmd.Add(new JProperty("receiver", strMOuser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("setUp", inUp));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);//等待
            //   funWaitForResult(true, 100);
        }
        #endregion
        #region 查询巡检状态
        public void funCmdJson_CruiseStateGet(string struser, string strMOuser)
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 1));
            jobCmd.Add(new JProperty("cmdAction", "CruiseStateGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", struser));
            jobCmd.Add(new JProperty("receiver", strMOuser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty(" ", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            //   funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        #endregion
        #region 状态查询
        public void funCmdJson_ObjStateGet(int inObjType, string strUser, string strMouser)
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 1));
            jobCmd.Add(new JProperty("cmdAction", "ObjStateGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("ObjType", inObjType));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            //   funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        #endregion
        #region 监控头重启
        public void funCmdJson_MonDevRestart(string strNo, string strUser, string strMouser)
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 1));
            jobCmd.Add(new JProperty("cmdAction", "MonDevRestart"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("No", strNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            //funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        #endregion
        #endregion
        #region 云台
        public void funCmdJson_PtzMove(int iPanVelo, int iTiltVelo, string struser, string strMOuser)//云台相关参数设置——iPanVelo, iTiltVel0范围是（0-100）云台转动
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 2));
            jobCmd.Add(new JProperty("cmdAction", "PTZMove"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", struser));
            jobCmd.Add(new JProperty("receiver", strMOuser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("xVelo", iPanVelo));//xVelo":50;"yVelo":50
            jobbuf.Add(new JProperty("yVelo", iTiltVelo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            // funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        #region 云台角度
        public void funCmdJson_PTZMoveAngleSet(int inhonAngle, int inrverAngle, string strUser, string strMoUser)
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 2));
            jobCmd.Add(new JProperty("cmdAction", "PTZMoveAngleSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMoUser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("honAngle", inhonAngle));
            jobbuf.Add(new JProperty("verAngle", inrverAngle));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));

            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            // funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        #endregion
        #region 云台设置预置位
        public void funCmdJson_PrePosSet(int intuNO, string strUser, string strMoUser)//
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 2));
            jobCmd.Add(new JProperty("cmdAction", "PrePosSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMoUser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("No", intuNO));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            //  funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        #endregion
        #region 云台调用预置位
        public void funCmdJson_PrePosInvoke(int intuNO, string strUser, string strMoUser)// 
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 2));
            jobCmd.Add(new JProperty("cmdAction", "PrePosInvoke"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMoUser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("No", intuNO));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            //funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        #endregion
        #region 查云台角度
        public void funCmdJson_PTZAngleGet(string strUser, string strMoUser) //
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 2));
            jobCmd.Add(new JProperty("cmdAction", "PTZAngleGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMoUser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("", ""));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            // funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        #endregion
        #region 云台初始化
        public void funCmdJson_PTZInit(string strUser, string strMouser) //
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 2));
            jobCmd.Add(new JProperty("cmdAction", "PTZInit"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty(" ", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            //   funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待

        }
        #endregion
        #endregion
        #region  可见光
        public void funCmdJson_ZoomOpt(string strTele, string strUser, string strMouser) //
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 3));
            jobCmd.Add(new JProperty("cmdAction", "ZoomOpt"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("ZoomType", strTele));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            //funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        public void funCmdJson_ZoomPosSet(float fValue, string strUser, string strMouser)//
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 3));
            jobCmd.Add(new JProperty("cmdAction", "ZoomPosSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", fValue.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            // funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        public void funCmdJson_ZoomPosGet(string strUser, string strMouser) //变焦位置查询
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 3));
            jobCmd.Add(new JProperty("cmdAction", "ZoomPosGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty(" ", ""));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            //   funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        public void funCmdJson_SaveImg(string strUser, string strMouser) //存图
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 3));
            jobCmd.Add(new JProperty("cmdAction", "SaveImg"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty(" ", ""));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            // funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        #endregion
        #region 红外控制
        public void funCmdJson_ManualFocus(string Naer, string strUser, string strMouser) //手动聚焦
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 4));
            jobCmd.Add(new JProperty("cmdAction", "ManualFocus"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("focusType", Naer));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            // funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        public void funCmdJson_AutoFocus(string strUser, string strMouser) //自动聚焦
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 4));
            jobCmd.Add(new JProperty("cmdAction", "AutoFocus"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            // funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        public void funCmdJson_FocusPosSet(string strUser, string strMouser) //直接聚焦位置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 4));
            jobCmd.Add(new JProperty("cmdAction", "FocusPosSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            // funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        public void funCmdJson_FocusPosGet(string strUser, string strMouser)//聚焦位置查询 
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 4));
            jobCmd.Add(new JProperty("cmdAction", "FocusPosGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            // funWaitForResult(true, 100);
            funCheckStaus(true, 100);//等待
        }
        public void funCmdJson_PaletteSet(string strSet, string strUser, string strMouser)//色板设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 4));
            jobCmd.Add(new JProperty("cmdAction", "PaletteSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("palType", strSet));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_DigZoomSet(int inuvale, string strUser, string strMouser) //数字变焦设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 4));
            jobCmd.Add(new JProperty("cmdAction", "DigZoomSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("value", inuvale));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_DigZoomGet(string strUser, string strMouser) //数字变焦获取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 4));
            jobCmd.Add(new JProperty("cmdAction", "DigZoomGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AdjustModeSet(string strmode, string strUser, string strMouser) //调节模式
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 4));
            jobCmd.Add(new JProperty("cmdAction", "AdjustModeSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("mode", strmode));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_ManualAdjustSet(float fMax, float fMin, string strUser, string strMouser) //手动调节
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 4));
            jobCmd.Add(new JProperty("cmdAction", "ManualAdjustSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fMax", fMax.ToString("0.00")));
            jobbuf.Add(new JProperty("fMin", fMin.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_SaveIRHotImg(string strUser, string strMouser) //红外热图保存
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 4));
            jobCmd.Add(new JProperty("cmdAction", "SaveIRHotImg"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_SaveVideoImg(string strUser, string strMouser)//存视频图 
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 4));
            jobCmd.Add(new JProperty("cmdAction", "SaveVideoImg"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        #endregion
        #region 红外分析
        public void funCmdJson_EmissivitySet(float inemiss, string strUser, string strMouser) //辐射率设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "EmissivitySet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("emiss", inemiss.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_EmissivityGet(string strUser, string strMouser) //辐射率读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "EmissivityGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty(" ", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_RefTempSet(float infValue, string strUser, string strMouser) //反射温度设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "RefTempSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", infValue.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_RefTempGet(string strUser, string strMouser) //反射温度读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "RefTempGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AirTempSet(float infValue, string strUser, string strMouser)//空气温度设置 
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AirTempSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", infValue.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AirTempGet(string strUser, string strMouser) //空气温度读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AirTempGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AirHumSet(float infValue, string strUser, string strMouser) //空气湿度设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AirHumSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", infValue.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AirHumGet(string strUser, string strMouser) //空气湿度读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AirHumGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_DistanceSet(float infvalue, string strUser, string strMouser) //距离设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "DistanceSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", infvalue.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_DistanceGet(string strUser, string strMouser) //距离读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "DistanceGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_WinTempSet(float infvalue, string strUser, string strMouser) //窗口温度设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "WinTempSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", infvalue.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_WinTempGet(string strUser, string strMouser) //窗口温度读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "WinTempGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_WinTrmRateSet(int infvalue, string strUser, string strMouser) //窗口穿透率设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "WinTrmRateSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", infvalue.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_WinTrmRateGet(string strUser, string strMouser) //窗口穿透率读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "WinTrmRateGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fValue", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaStateGet(string strUser, string strMouser) //查当前测温状态
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaStateGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaClearAll(string strUser, string strMouser) //清除所有测温点
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaClearAll"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            // jobbuf.Add(new JProperty("fValue", " "));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        //"uNO":1,"fLeftPer":0.1,"fTopPer":0.25
        public void funCmdJson_AnaSpotPosSet(int inuNo, float infLeftper, float infToper, string strUser, string strMouser) //点温位置设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaSpotPosSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jobbuf.Add(new JProperty("fLeftper", infLeftper.ToString("0.00")));
            jobbuf.Add(new JProperty("fToper", infToper.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaSpotPosGet(int inuNo, string strUser, string strMouser)//点温位置读取 
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaSpotPosGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        //"uNO":1,"uActive":,"uUseLocal":true,"fEmiss":0.74,"fDis":20.00
        public void funCmdJson_AnaSpotParamSet(int inuNo, string struActive, string inuUseLocal, float infEmiss, float infDis, string strUser, string strMouser) //点温参数设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaSpotParamSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jobbuf.Add(new JProperty("uActive", struActive));
            jobbuf.Add(new JProperty("uUseLocal", inuUseLocal));
            jobbuf.Add(new JProperty("fEmiss", infEmiss.ToString("0.00")));
            jobbuf.Add(new JProperty("fDis", infDis.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaSpotParamGet(int inuNo, string strUser, string strMouser)//点温参数读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaSpotParamGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaSpotTempGet(int inuNo, string strUser, string strMouser)//点温度获取 
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaSpotTempGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        //"uNO":1,"fStartXPer":0.5,"fStartYPer":0.5,"fEndXPer":0.5,"fEndYPer":0.5
        public void funCmdJson_AnaLinePosSet(int inuNo, float infStartXPer, float infStartYPer, float infEndXPer, float infEndYPer, string strUser, string strMouser)//线温位置设置 
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaLinePosSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jobbuf.Add(new JProperty("fStartXPer", infStartXPer.ToString("0.00")));
            jobbuf.Add(new JProperty("fStartYPer", infStartYPer.ToString("0.00")));
            jobbuf.Add(new JProperty("fEndXPer", infEndXPer.ToString("0.00")));
            jobbuf.Add(new JProperty("fEndYPer", infEndYPer.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaLinePosGet(int inuNo, string strUser, string strMouser) //线温位置读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaLinePosGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        //"uNO":1,"uActive":,"uUseLocal":true,"fEmiss":0.75,"fDis":45
        public void funCmdJson_AnaLineParamSet(int inuNo, string struActive, string struUseLocal, float infEmiss, float infDis, string strUser, string strMouser) //线温参数设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaLineParamSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jobbuf.Add(new JProperty("uActive", struActive));
            jobbuf.Add(new JProperty("uUseLocal", struUseLocal));
            jobbuf.Add(new JProperty("fEmiss", infEmiss.ToString("0.00")));
            jobbuf.Add(new JProperty("fDis", infDis.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaLineParamGet(int inuNo, string strUser, string strMouser) //线温参数读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaLineParamGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaLineTempGet(int inuNo, string strUser, string strMouser) //线温获取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaLineTempGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        //"uNO";1,"fStartXPer":,"fStartYPer":,"fToWidthPer":,"fHeightPer":
        public void funCmdJson_AnaAreaPosSet(int inuNo, float fStartXPer, float fStartYPer, float fToWidthPer, float fHeightPer, string strUser, string strMouser) //矩形测温位置设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaLineTempGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jobbuf.Add(new JProperty("fStartXPer", fStartXPer.ToString("0.00")));
            jobbuf.Add(new JProperty("fStartYPer", fStartYPer.ToString("0.00")));
            jobbuf.Add(new JProperty("fToWidthPer", fToWidthPer.ToString("0.00")));
            jobbuf.Add(new JProperty("fHeightPer", fHeightPer.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaAreaPosGet(int inuNo, string strUser, string strMouser) //矩形测温位置读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaAreaPosGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        //"uNO":1,"uActive":"","uUseLocal":true,"fEmiss":.0.75,"fDis":20
        public void funCmdJson_AnaAreaParamSet(int inuNo, string struActive, string struUseLocal, float infEmiss, float infDis, string strUser, string strMouser) //矩形测温参数设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaAreaParamSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jobbuf.Add(new JProperty("uActive", struActive));
            jobbuf.Add(new JProperty("uUseLocal", struUseLocal));
            jobbuf.Add(new JProperty("fEmiss", infEmiss.ToString("0.00")));
            jobbuf.Add(new JProperty("fDis", infDis.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaAreaParamGet(int inuNo, string strUser, string strMouser)//矩形测温参数设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaAreaParamGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaAreaTempGet(int inuNo, string strUser, string strMouser) //矩形测温温度获取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaAreaTempGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        //"uNO":1,"fLeftPer1":12,"fTopPer1":20,....."fLeftPerN":15,"fTopPerN":45,
        public void funCmdJson_AnaPolyPosSet(int inuNo, float infLeftPer1, float infTopPer1, float infLeftPerN, float infTopPerN, string strUser, string strMouser) //多边形温位置设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaPolyPosSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jobbuf.Add(new JProperty("fLeftPer1", infLeftPer1.ToString("0.00")));
            jobbuf.Add(new JProperty("fTopPer1", infTopPer1.ToString("0.00")));
            jobbuf.Add(new JProperty("fLeftPerN", infLeftPerN.ToString("0.00")));
            jobbuf.Add(new JProperty("fTopPerN", infTopPerN.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaPolyPosGet(int inuNo, string strUser, string strMouser) //多边形温位置读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaPolyPosGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        //"uNO":1,"uActive":true,"uUseLocal":true,"fEmiss":.0.75,"fDis":20
        public void funCmdJson_AnaPolyParamSet(int inuNo, string struActive, string struUseLocal, float infEmiss, float infDis, string strUser, string strMouser)//多边形温参数设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaPolyParamSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jobbuf.Add(new JProperty("uActive", struActive));
            jobbuf.Add(new JProperty("uUseLocal", struUseLocal));
            jobbuf.Add(new JProperty("fEmiss", infEmiss.ToString("0.00")));
            jobbuf.Add(new JProperty("fDis", infDis.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaPolyParamGet(int inuNo, string strUser, string strMouser) //多边形温参数读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaPolyParamGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaPolyTempGet(int inuNo, string strUser, string strMouser) //多边形温温度获取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaPolyTempGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        //"uNO":1,"fCenterLeftPer":15,"fCenterTopPer":10,"fRadiusWidthPer":20
        public void funCmdJson_AnaCirclePosSet(int inuNo, float infCenterLeftPer, float infCenterTopPer, float infRadiusWidthPer, string strUser, string strMouser) //圆形温位置设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaCirclePosSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jobbuf.Add(new JProperty("fCenterLeftPer", infCenterLeftPer.ToString("0.00")));
            jobbuf.Add(new JProperty("fCenterTopPer", infCenterTopPer.ToString("0.00")));
            jobbuf.Add(new JProperty("fRadiusWidthPer", infRadiusWidthPer.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaCirclePosGet(int inuNo, string strUser, string strMouser) //圆形温读取设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaCirclePosGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        //"uNO":1,"uActive":"","uUseLocal":true,"fEmiss":.0.75,"fDis":20
        public void funCmdJson_AnaCircleParamSet(int inuNo, string struActive, string struUseLocal, float infEmiss, float infDis, string strUser, string strMouser) //圆形温参数设置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaCircleParamSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jobbuf.Add(new JProperty("uActive", struActive));
            jobbuf.Add(new JProperty("uUseLocal", struUseLocal));
            jobbuf.Add(new JProperty("fEmiss", infEmiss.ToString("0.00")));
            jobbuf.Add(new JProperty("fDis", infDis.ToString("0.00")));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaCircleParamGet(int inuNo, string strUser, string strMouser) //圆形温参数读取
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaCircleParamGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        public void funCmdJson_AnaCircleTempGet(int inuNo, string strUser, string strMouser)//读圆形温 
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 5));
            jobCmd.Add(new JProperty("cmdAction", "AnaCircleTempGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        #endregion
        #region 温湿度
        public void funCmdJsob_TempHumGet(string strUser, string strMouser)
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 6));
            jobCmd.Add(new JProperty("cmdAction", "TempHumGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
            funSendOneFramCmd(strjson);
            funCheckStaus(true, 100);
        }
        #endregion
        #region 机器人
        public void fumCmdJson_WorkStateGet(string strUser, string strMouser) //查状态
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 7));
            jobCmd.Add(new JProperty("cmdAction", "WorkStateGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            //  jobbuf.Add(new JProperty("uNo", inuNo));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
        }
        public void funCmdJson_UrgencyStop(int inflag, string strUser, string strMouser) //急停
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 7));
            jobCmd.Add(new JProperty("cmdAction", "UrgencyStop"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("flag", inflag));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
        }
        //iForVelo,iRotaVelo
        public void funCmdJson_ROBMove()//移动 
        {

        }
        public void funCmdJson_CurrentPosGet(string strUser, string strMouser) //查实时位置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 7));
            jobCmd.Add(new JProperty("cmdAction", "CurrentPosGet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            // jobbuf.Add(new JProperty("flag", inflag));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
        }
        //"fRefPosX":45.00:,"fRefPosY":50.00,fAbsAngle:24.01
        public void funCmdJson_ManualMovePosSet(int infRefPosX, int infRefPosY, int infAbsAngle, string strUser, string strMouser)//手动走位置
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 7));
            jobCmd.Add(new JProperty("cmdAction", "ManualMovePosSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("fRefPosX", infRefPosX));
            jobbuf.Add(new JProperty("fRefPosY", infRefPosY));
            jobbuf.Add(new JProperty("fAbsAngle", infAbsAngle));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
        }
        public void funCmdJson_MoveSpePosSet(string strPosType, string strUser, string strMouser) //走特定位
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 7));
            jobCmd.Add(new JProperty("cmdAction", "MoveSpePosSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("PosType", strPosType));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
        }
        public void funCmdJson_PowersetUpSet(int insetUp, string strUser, string strMouser) //充电开关
        {

            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", strDatetime));
            jobCmd.Add(new JProperty("cmdType", 7));
            jobCmd.Add(new JProperty("cmdAction", "PowersetUpSet"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", strUser));
            jobCmd.Add(new JProperty("receiver", strMouser));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("setUp", insetUp));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            string strjson = JsonConvert.SerializeObject(jobCmd);
        }
        #endregion
        #endregion
        //#region MD5校验相关
        //public string GetMD5String(string str)
        //{
        //    //创建 MD5对象
        //    MD5 md5 = MD5.Create();//new MD5();
        //    //开始加密
        //    //需要将字符串转换成字节数组
        //    byte[] buffer = Encoding.Default.GetBytes(str);
        //    //返回一个加密好的字节数组
        //    byte[] MD5Buffer = md5.ComputeHash(buffer);
        //    string strNew = "";
        //    strNew = BitConverter.ToString(MD5Buffer).Replace("-", "");
        //    return strNew;
        //}
        //#endregion
        //#region  启动巡检
        //public void simControlCmd_SetupCruise()
        //{
        //    RootCruiseStateGet json = new RootCruiseStateGet();
        //    json.seq = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //    json.cmdType = 1;
        //    json.cmdAction = "CruiseSet";
        //    json.result = "";
        //    json.paramList = new List<ParamCruiseStateGet>();
        //    json.paramList.Add(new ParamCruiseStateGet { setUp = 0 });
        //    json.sender = "user";
        //    json.receiver = "MXXX";
        //    string strjson = JsonConvert.SerializeObject(json);
        //    funSendOneFramCmd(strjson);
        //}
        //#endregion
        //#region 停止巡检
        //public void Stop_ControlCmd_SetupCruise()
        //{
        //    RootCruiseStateGet json = new RootCruiseStateGet();
        //    json.seq = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //    json.cmdType = 1;
        //    json.cmdAction = "CruiseSet";
        //    json.result = "";
        //    json.paramList = new List<ParamCruiseStateGet>();
        //    json.paramList.Add(new ParamCruiseStateGet { setUp = 0 });
        //    json.sender = "user";
        //    json.receiver = "MXXX";
        //    string strjson = JsonConvert.SerializeObject(json);
        //    funSendOneFramCmd(strjson);
        //}
        //#endregion
        //#region 查巡检状态
        //public void CruiseStateGets()
        //{
        //    CruiseStateGet json = new CruiseStateGet();
        //    json.seq = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //    json.cmdType = 1;
        //    json.cmdAction = "CruiseStateGet";
        //    json.paramList = new List<string>();
        //    json.sender = "user";
        //    json.receiver = "MXXX";
        //    string strjson = JsonConvert.SerializeObject(json);
        //    funSendOneFramCmd(strjson);
        //}
        //#endregion
        //#region 状态查询
        //public void ObjStateGest()
        //{
        //    Root_ObjStateGet json = new Root_ObjStateGet();
        //    json.seq = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //    json.cmdType = 1;
        //    json.cmdAction = "ObjStateGet";
        //    json.result = "";
        //    json.paramList = new List<ObjStateGet>();
        //    json.paramList.Add(new ObjStateGet { ObjType = 1 });
        //    json.sender = "admin";
        //    json.receiver = "CmdServer";
        //    string strjson = JsonConvert.SerializeObject(json);
        //    funSendOneFramCmd(strjson);
        //}
        //#endregion
        //#region 监控头重启
        //public void Root_MonDevRestarts()
        //{
        //    Root_MonDevRestart json = new Root_MonDevRestart();
        //    json.seq = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //    json.cmdType = 1;
        //    json.cmdAction = "MonDevRestart";
        //    json.result = "";
        //    json.paramList = new List<MonDevRestart>();
        //    json.paramList.Add(new MonDevRestart { NO = "A" });
        //    json.sender = "admin";
        //    json.receiver = "CmdServer";
        //    string strjson = JsonConvert.SerializeObject(json);
        //    funSendOneFramCmd(strjson);
        //}
        //#endregion

        //#region  可见光相关操作命令
        ////
        //#region 云台转动
        //public void Root_PTZMoves(int xV, int yV)
        //{
        //    Root_PTZMove json = new Root_PTZMove();
        //    json.seq = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //    json.cmdType = 1;
        //    json.cmdAction = "PTZMove";
        //    json.result = "";
        //    json.paramList = new List<PTZMove>();
        //    json.paramList.Add(new PTZMove { xVelo = xV, yVelo = yV });
        //    json.sender = "admin";
        //    json.receiver = "CmdServer";
        //    string strjson = JsonConvert.SerializeObject(json);
        //    funSendOneFramCmd(strjson);
        //}
        //#endregion
        //#region 云台角度
        //public void Root_PTZMoveAngleSesst(int HonAngle, int VerAngle)
        //{
        //    Root_PTZMoveAngleSet json = new Root_PTZMoveAngleSet();
        //    json.seq = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //    json.cmdType = 1;
        //    json.cmdAction = "PTZMoveAngleSet";
        //    json.result = "";
        //    json.paramList = new List<PTZMoveAngleSet>();
        //    json.paramList.Add(new PTZMoveAngleSet { honAngle = HonAngle, verAngle = VerAngle });
        //    json.sender = "admin";
        //    json.receiver = "CmdServer";
        //    string strjson = JsonConvert.SerializeObject(json);
        //    funSendOneFramCmd(strjson);
        //}
        //#endregion
        //#region 云台设置预置位
        //public void Root_PrePosSet(string no)
        //{
        //    Root_MonDevRestart json = new Root_MonDevRestart();
        //    json.seq = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //    json.cmdType = 1;
        //    json.cmdAction = "PrePosSet";
        //    json.result = "";
        //    json.paramList = new List<MonDevRestart>();
        //    json.paramList.Add(new MonDevRestart { NO = no });
        //    json.sender = "admin";
        //    json.receiver = "CmdServer";
        //    string strjson = JsonConvert.SerializeObject(json);
        //    funSendOneFramCmd(strjson);
        //}
        //#endregion
        //#region 云台调用预置位

        //#endregion
        ////
        //#endregion
    }
}
