using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Threading;
using Peiport_commandManegerSystem;
namespace Peiport_commandManegerSystem
{
    public class ListenClient
    {
        private const int MAXMSGCOUNT = 256;
        private const int MAXHEARTBEAT = 3;
        private const int HEARTTIMER = 10;//以秒为单位
        private string sRemoteEndPoint;
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
        public void RunRecv()   //启动连接（供Server类接收到新连接时调用）
        {
            Thread thread = new Thread(Thread_RecvFromClient);
            thread.Name = "客户端";// +sRemoteEndPoint + "接收消息线程";
            thread.IsBackground = true;//设置为后台线程，随着主线程退出而退出                      
            thread.Start(); //启动线程  
        }
        public ListenClient(Server server, Socket socket) //创建（需传递入相应的变量)
        {
            ListenSocket = socket;
            myServer = server;
            sRemoteEndPoint = socket.RemoteEndPoint.ToString();
            ipaEndIP = ((IPEndPoint)(socket.RemoteEndPoint)).Address;
            intEndPort = ((IPEndPoint)(socket.RemoteEndPoint)).Port;
            dtUpateLoadTime = DateTime.Now;
            strLoginUserName = "";
        }
        void Thread_RecvFromClient() //接收到客户端消息线程  
        {
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
                    myServer.RecvChange(this, arrRecvMsg);//  strSRecMsg);
                    Thread.Sleep(100);
                }
                catch (SocketException socketEx)
                {
                    CloseSocket();
                    break;

                }
                catch (Exception ex)//
                {
                    CloseSocket();
                    break;

                }
            }
        }
        public void CloseSocket()//断开连接
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
            myServer.DisconnectChange(this);  //产生断开事件
        }
        public int sendMegToCilent(byte[] btMsgBuf)  //发送信息，供外部使用
        {
            int nLen = btMsgBuf.Length;
            if (nLen < 0) return 0;
            myServer.SendChange(this, btMsgBuf);

            int ret = ListenSocket.Send(btMsgBuf);
            if (ret <= 0)
            {
            }
            return ret;
        }
    }
}
