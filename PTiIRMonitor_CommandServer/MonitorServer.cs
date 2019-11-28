using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
namespace Peiport_commandManegerSystem
{
    public class MonitorServer
    {
        public class EventArgs_Connect : EventArgs
        {
            public string sConnMsg;
            public string sImg;
        }
        public class EventArgs_Disconnect : EventArgs
        {
            public string sImg;
        }
        public class EventArgs_Recv : EventArgs
        {
            public string sRecvTime;
            public string sRecvMsg;
            public byte[] btRecevByte;
            public string sClientIP;
            public int iClientPort;
            public string sLogionName;   //登陆的用户名
        }
        public class EventArgs_Send : EventArgs
        {
            public string sSendTime;
            public byte[] btSendMsg;
            public string sLogionName;   //登陆的用户名
        }
        private const int MAXCLIENTNUM = 16;            //最大监听客户端数
        private const int MAXBUFFERLEN = 1024;
        private IPAddress ipaIP;//定义的IP
        private int iServerPort; //Socket端口=
        private bool bWatchAcceptFlag = true;
        private Socket g_SocketServer = null;//服务端运行的SOCKET
        public Thread Thread_Server = null;//服务端运行的线程
        private Thread Thread_MonitorConnect = null;// 监控连接
        public delegate void EventHandler_Recv(object source, EventArgs_Recv e);
        public event EventHandler_Recv OnRecvMsg;
        public delegate void EventHandler_Connect(object source, EventArgs_Connect e);
        public event EventHandler_Connect OnConnect;
        public delegate void EventHandler_Disconnect(object source, EventArgs_Disconnect e);
        public event EventHandler_Disconnect OnDisconnect;
        public delegate void EventHandler_Send(object source, EventArgs_Send e);
        public event EventHandler_Send OnSendMsg;
        public List<ListMonitor> g_lstConnectMonitorTab;
        public Form1 frmMain;
        public MonitorServer(IPAddress ip, int ServerPort0)//创建类
        {
            ipaIP = ip;
            this.iServerPort = ServerPort0;
            g_lstConnectMonitorTab = new List<ListMonitor>();
        }
        JsonUtils m_ClJsonCtrl = new JsonUtils();
        public short StartServer() //启动服务器
        {
            if (g_SocketServer != null)
                return 1;
            try
            {
                g_SocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ServerInfo = new IPEndPoint(ipaIP, iServerPort);

                g_SocketServer.Bind(ServerInfo);//将SOCKET接口和IP端口绑定
                g_SocketServer.Listen(MAXCLIENTNUM);//开始监听，并且挂起数为16
                bWatchAcceptFlag = true;
                Thread_Server = new Thread(new ThreadStart(Thread_WatchConnecting));//将接受客户端连接的方法委托给线程
                Thread_Server.Start();//线程开始运行
                Thread_Server.IsBackground = true;
                Thread_Server.Name = "监听客户端线程";

                Thread_MonitorConnect = new Thread(new ThreadStart(Thread_ScanInvalidConnect));
                Thread_MonitorConnect.Start();
                Thread_MonitorConnect.IsBackground = true;
                Thread_MonitorConnect.Name = "扫描无效连接";

                return 0;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return -1;
            }
        }
        public bool getServerSetupState()//判断是否连接成功
        {
            bool blbuf;
            if (Thread_Server != null)
                blbuf = true;
            else
                blbuf = false;
            return blbuf;
        }
        private void Thread_WatchConnecting() //监听客户端连接的方法
        {
            string str1;

            while (bWatchAcceptFlag)
            {
                Thread.Sleep(100);
                try
                {
                    Socket connection = g_SocketServer.Accept();
                    str1 = connection.RemoteEndPoint.ToString();

                    ListMonitor client = new ListMonitor(this, connection);
                    g_lstConnectMonitorTab.Add(client);   //增加收到的连接新客户端信息

                    client.RunRecv();//输入监听启动事件
                    //产生连接事件给外部
                    EventArgs_Connect args = new EventArgs_Connect();

                    if (OnConnect != null)   //产生连接事件,，给具体的监听类
                    {
                        OnConnect(this, args);
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    continue;
                }
            }
        }
        public List<ListMonitor> GetClientList()
        {
            List<ListMonitor> lstClient = new List<ListMonitor>();
            return g_lstConnectMonitorTab;
        }
        public void Thread_ScanInvalidConnect()   //监控扫描连接线程
        {
            //while (true)
            //{
            //    try
            //    {
            //        //监控是否有定时注册
            //        DateTime dt1 = DateTime.Now;
            //        foreach (ListMonitor ltc in g_lstConnectClientTab)
            //        {
            //            TimeSpan ts = dt1 - ltc.dtUpateLoadTime;
            //            if (ts.TotalSeconds > 300)  //大于300秒无更新即自动断开(5分钟)
            //            {
            //                ltc.CloseSocket();
            //                g_lstConnectClientTab.Remove(ltc);
            //            }
            //            Thread.Sleep(1);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message.ToString());
            //    }
            //}

        }
        public void closeOneClient(int iLstIndex)   //断开一个客户连接
        {
            if ((iLstIndex >= 0) && (iLstIndex < g_lstConnectMonitorTab.Count))
            {
                ListMonitor sclient = g_lstConnectMonitorTab[iLstIndex];
                sclient.CloseSocket();
            }

        }
        public void RecvChange(ListMonitor ltClient, byte[] btReceiMsgBuf)//接收到回信息，供监控类回调用
        {
            if (OnRecvMsg != null)
            {
                EventArgs_Recv args = new EventArgs_Recv();
                args.btRecevByte = btReceiMsgBuf;
                args.sRecvMsg = Encoding.Default.GetString(btReceiMsgBuf, 0, btReceiMsgBuf.Length);//接收的信息
                args.sRecvTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                args.sLogionName = ltClient.strLoginUserName;
                args.sClientIP = ltClient.ipaEndIP.ToString();
                args.iClientPort = ltClient.intEndPort;
                OnRecvMsg(this, args);
            }
        }
        public void DisconnectChange(ListMonitor lClient)    //产生断开单一个连接的事件（供给监听类回调用）
        {
            try
            {
                g_lstConnectMonitorTab.Remove(lClient);
                if (OnDisconnect != null)
                {
                    EventArgs_Disconnect args = new EventArgs_Disconnect();
                    args.sImg = "";
                    OnDisconnect(this, args);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
        public void StopServer()   //停止服务器
        {
            try
            {
                if (g_SocketServer != null)  //关闭Socket类
                {
                    //停止所有监听线程
                    foreach (ListMonitor ltc in g_lstConnectMonitorTab)//
                    {
                        ltc.CloseSocket();
                    }
                    Thread.Sleep(100);
                    g_SocketServer.Shutdown(SocketShutdown.Both);
                }
                if (Thread_Server != null)
                {
                    bWatchAcceptFlag = false;
                    Thread_Server.Abort();
                    Thread_Server = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                g_SocketServer.Close();//关闭socket
            }
        }
        public int SendOneClientMsgByIndex(int intLstIndex, byte[] btCmdBuf)//发送给一个客户端信息,供外部使用
        {
            int intre = -1;
            if ((intLstIndex >= 0) && (intLstIndex < g_lstConnectMonitorTab.Count))
            {
                intre = g_lstConnectMonitorTab[intLstIndex].sendMegToCilent(btCmdBuf);
            }
            return intre;
        }
        public void SendChange(ListMonitor ltClient, byte[] btRecMsg)//发送到信息，供监控类回调用
        {
            if (OnSendMsg != null)
            {
                EventArgs_Send args = new EventArgs_Send();
                args.btSendMsg = btRecMsg;
                args.sSendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                args.sLogionName = ltClient.strLoginUserName;
                OnSendMsg(this, args);
            }
        }
    }
}
