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
using System.Xml;
using System.Diagnostics;

namespace PTiIRMonitor_MonitorManagerApp
{
    public class ListenClient
    {
        private const int MAXMSGCOUNT = 256;
        private const int MAXHEARTBEAT = 3;
        private const int HEARTTIMER = 10;//以秒为单位

        public int iUserType = 0;//1,设备；2，客户
        public int iUserID = 0;
        public string sUserName = "";
        private string sUserPwd = "";
        
        public Queue<string> queMsg = new Queue<string>();
        public short iHeartBeat = -1;
        
        private string sRemoteEndPoint;
        private System.Timers.Timer tmHeartBeat;
        private bool bFirstLogin = false;


        
        ///////////////////////////////////////////////////////
        //供外部使用用的变量
        public int intEndPort;             //连接对方终端的端口号
        public IPAddress ipaEndIP;          //连接对方终端的IP
        public string strLoginUserName;     //注册的用户名
        public DateTime dtUpateLoadTime;    //更新时间


        //对内的变量
        Server myServer;            //监听主连接
        private Socket ListenSocket = null;   //主要连接监听的Socket

        private bool bRunRecv = true;


        public ListenClient(Server server, Socket socket)           //创建（需传递入相应的变量)
        {
            ListenSocket = socket ;
            myServer = server;
            sRemoteEndPoint = socket.RemoteEndPoint.ToString();
            ipaEndIP = ((IPEndPoint)(socket.RemoteEndPoint)).Address;
            intEndPort = ((IPEndPoint)(socket.RemoteEndPoint)).Port;
            dtUpateLoadTime = DateTime.Now;
            strLoginUserName = "";
        }
        public void CloseSocket()    //             断开连接
        {
            
            try
            {
                ListenSocket.Close();//关闭之前accept出来的和客户端进行通信的套接字
                ListenSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {

            }
            bRunRecv = false;
            Thread.Sleep(100);
            Debug.WriteLine(sRemoteEndPoint + " Listen client closed");
            //myServer.DisconnectChange(sRemoteEndPoint);  //产生断开事件
            myServer.DisconnectChange(this);  //产生断开事件
            //this. = null;
        }

        public void RunRecv()   //启动连接（供Server类接收到新连接时调用）
        {
            //ParameterizedThreadStart pts = new ParameterizedThreadStart(RecvFromClient);
            Thread thread = new Thread(Thread_RecvFromClient);
            thread.Name = "客户端" + sRemoteEndPoint + "接收消息线程";
            thread.IsBackground = true;//设置为后台线程，随着主线程退出而退出                      
            thread.Start(); //启动线程  
            
        }


        void Thread_RecvFromClient()        //接收到客户端消息线程  
        {
            //Socket socketServer = socketclientpara as Socket;
            while (bRunRecv)
            {
                try
                {
                    //创建一个内存缓冲区 其大小为1024*1024字节  即1M   
                    byte[] arrRecvMsg = new byte[1024];
                    //将接收到的信息存入到内存缓冲区,并返回其字节数组的长度  
                    int length = ListenSocket.Receive(arrRecvMsg);
                    if (length <= 0)
                    {
                        CloseSocket(); break;
                    }
                    //将机器接受到的字节数组转换为人可以读懂的字符串   
                    string strSRecMsg = Encoding.Default.GetString(arrRecvMsg, 0, length);

                    //myServer.RecvChange(ListenSocket, strSRecMsg);
                    myServer.RecvChange(this, strSRecMsg);
                    Thread.Sleep(10);  
                   

                    //--------------------------------
                }
                catch (SocketException socketEx)
                {
                    //                     Global.dicListenClient.Remove(sRemoteEndPoint);
                    //                     ListenSocket.Shutdown(SocketShutdown.Both);
                    //                     ListenSocket.Close();//关闭之前accept出来的和客户端进行通信的套接字  
                    //                     if (tmHeartBeat != null) tmHeartBeat.Enabled = false;
                    Debug.WriteLine(sRemoteEndPoint + " Listen client socketexception:\t" + socketEx.Message.ToString());
                    CloseSocket();
                    break;
                }
                catch (Exception ex)//
                {
                    Debug.WriteLine(sRemoteEndPoint + " Listen client exception:\t" + ex.Message.ToString());
                    CloseSocket();
                    continue;
                }
            }
        }
        public int sendMegToCilent(string msgstr)     //发送信息，供外部使用
        {
            int nLen = msgstr.Length;
            if (nLen < 0) return 0;
            Byte[] msg = new Byte[nLen];
            msg = System.Text.Encoding.Default.GetBytes(msgstr);
           // myServer.SendChange(ListenSocket, msgstr);
            myServer.SendChange(this, msgstr);
            int ret = ListenSocket .Send(msg);
            if (ret <= 0) Debug.WriteLine(sRemoteEndPoint + " Send Error");
            return ret;
        }
        
    }
}