/////////////////////////////////////////////////////
////名称：
////描述：
////创建：
////修改：2018-05-09
////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using System.Xml;
using System.Diagnostics;

namespace PTiIRMonitor_MonitorManagerApp
{
    public class EventArgs_Connect : EventArgs
    {

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
        public string sClientIP;
        public int iClientPort;
        public string sLogionName;   //登陆的用户名

    }

    public class EventArgs_Send : EventArgs
    {
        public string sSendTime;
        public string sSendMsg;
        //public string sRemoteEndPoint;
        public string sLogionName;   //登陆的用户名
    }

    public class Server
    {
        private const int MAXCLIENTNUM = 16;            //最大监听客户端数
        private const int MAXBUFFERLEN = 1024;

        private IPAddress ipaIP;//定义的IP
        private int iServerPort; //Socket端口
        private bool bWatchAcceptFlag = true;
        private Socket g_SocketServer = null;//服务端运行的SOCKET
        public Thread Thread_Server = null;//服务端运行的线程
        private Thread Thread_MonitorConnect = null;// 监控连接

        public delegate void EventHandler_Connect(object source, EventArgs_Connect e);
        public delegate void EventHandler_Disconnect(object source, EventArgs_Disconnect e);
        public delegate void EventHandler_Recv(object source, EventArgs_Recv e);
        public delegate void EventHandler_Send(object source, EventArgs_Send e);
        public event EventHandler_Connect OnConnect;
        public event EventHandler_Disconnect OnDisconnect;
        public event EventHandler_Recv OnRecvMsg;
        public event EventHandler_Send OnSendMsg;




        public List<ListenClient> g_lstConnectClientTab;



        public Server(string ip, int ServerPort0)    //创建类
        {
            if (IPAddress.TryParse(ip, out ipaIP) == false) ;
            this.iServerPort = ServerPort0;
            g_lstConnectClientTab = new List<ListenClient>();// List<g_lstConnectClientTab>();
                                                             // g_SocketServer = new Socket();

        }
        public bool getServerSetupState()
        {
            bool blbuf;
            if (Thread_Server != null)
                blbuf = true;
            else
                blbuf = false;
            return blbuf;
        }
        public short StartServer()          //启动服务器
        {
            if (g_SocketServer != null)
                return 1;
            try
            {
                g_SocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ServerInfo = new IPEndPoint(ipaIP, iServerPort);// this.GetPort());

                g_SocketServer.Bind(ServerInfo);//将SOCKET接口和IP端口绑定
                g_SocketServer.Listen(MAXCLIENTNUM);//开始监听，并且挂起数为16
                bWatchAcceptFlag = true;
                Thread_Server = new Thread(new ThreadStart(Thread_WatchConnecting));//将接受客户端连接的方法委托给线程
                Thread_Server.Start();//线程开始运行
                Thread_Server.IsBackground = true;
                Thread_Server.Name = "监听客户端线程";
                return 0;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("start server error" + ex.Message.ToString());

                return -1;
            }
        }

        private void Thread_WatchConnecting()        //监听客户端连接的方法
        {
            string strip;
            int intport;
            DateTime dt;

            while (bWatchAcceptFlag)
            {
                try
                {
                    Socket connection = g_SocketServer.Accept();
                    ListenClient client = new ListenClient(this, connection);

                    if (OnConnect != null)   //产生连接事件,，给具体的监听类
                    {

                        g_lstConnectClientTab.Add(client);   //增加收到的连接新客户端信息
                        client.RunRecv();   //输入监听启动事件

                        //产生连接事件给外部
                        EventArgs_Connect args = new EventArgs_Connect();
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
        public void DisconnectChange(ListenClient lClient)    //产生断开单一个连接的事件（供给监听类回调用）
        {


            g_lstConnectClientTab.Remove(lClient);

            EventArgs_Disconnect args = new EventArgs_Disconnect();

            args.sImg = "";
            OnDisconnect(this, args);
        }
        public void RecvChange(ListenClient ltClient, string strSRecMsg)     //接收到回信息，供监控类回调用
        {
            if (OnRecvMsg != null)
            {
                EventArgs_Recv args = new EventArgs_Recv();
                args.sRecvMsg = strSRecMsg;
                args.sRecvTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                //args.sRemoteEndPoint = socketServer.RemoteEndPoint.ToString();
                args.sLogionName = ltClient.strLoginUserName;
                args.sClientIP = ltClient.ipaEndIP.ToString();
                args.iClientPort = ltClient.intEndPort;
                OnRecvMsg(this, args);
            }
        }
        public int SendOneClientMsg(int intLstIndex, string strCmd)        //发送给一个客户端信息,供外部使用
        {
            int intre = -1;
            if ((intLstIndex >= 0) && (intLstIndex < g_lstConnectClientTab.Count))
            {
                intre = g_lstConnectClientTab[intLstIndex].sendMegToCilent(strCmd);
            }
            return intre;
        }
        public int SendOneClientMsg(string strUserName, string strCmd)        //发送给一个客户端信息,采用用户名,供外部使用
        {
            int intre = -1;
            int i;
            if (strUserName != "")
            {
                for (i = 0; i < g_lstConnectClientTab.Count; i++)
                {
                    if (g_lstConnectClientTab[i].strLoginUserName == strUserName)
                    {

                        intre = g_lstConnectClientTab[i].sendMegToCilent(strCmd);
                        break;
                    }
                }
            }


            return intre;
        }
        public void SendChange(ListenClient ltClient, string strSRecMsg)    //发送到信息，供监控类回调用
        {
            if (OnSendMsg != null)
            {
                EventArgs_Send args = new EventArgs_Send();
                args.sSendMsg = strSRecMsg;
                args.sSendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                //args.sRemoteEndPoint = ltClient.RemoteEndPoint.ToString();
                args.sLogionName = ltClient.strLoginUserName;

                OnSendMsg(this, args);
            }
        }

        public void StopServer()   //停止服务器
        {
            try
            {

                if (g_SocketServer != null)  //关闭Socket类
                {
                    //停止所有监听线程
                    foreach (ListenClient ltc in g_lstConnectClientTab)//
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
    }
}

