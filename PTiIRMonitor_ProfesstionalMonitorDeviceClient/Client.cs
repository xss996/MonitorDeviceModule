using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime;
using System.ComponentModel;
using System.Windows.Forms;
using System.Security.Cryptography;
namespace Peiport_commandManegerSystem
{
    public class EventArgs_Recv : EventArgs
    {
        public string sRecvTime;
        public string sRecvMsg;
        public byte[] btRecvByteBuf;
    }
    public class EventArgs_Connect : EventArgs
    {
        public string sConnTime;
        public string sConnMsg;
    }
    public class EventArgs_Disconnect : EventArgs
    {
        public string sDisconnTime;
        public string sDisconnMsg;
    }

    public class Client
    {
        private const int MAXSOCKETBUFLEN = 1024;
        private IPEndPoint ServerInfo;
        private Socket ClientSocket;
        private bool bRunRecv = true;
        private bool bConnected = false;
        private string sLocalPort = null;
        public delegate void EventHandler_Recv(object source, EventArgs_Recv e);
        public event EventHandler_Recv OnRecvMsg;
        public delegate void EventHandler_Disconnect(object source, EventArgs_Disconnect e);

        public event EventHandler_Disconnect OnDisconnect;
        public delegate void EventHandler_Connect(object source, EventArgs_Connect e);
        public event EventHandler_Connect OnConnect;
        EventArgs_Disconnect argss = new EventArgs_Disconnect();
        public Client(string ServerIP, int ServerPort)
        {
            try
            { //定义一个IPV4，TCP模式的Socket
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress sIPAddr;
                IPAddress.TryParse(ServerIP, out sIPAddr);
                ServerInfo = new IPEndPoint(sIPAddr, ServerPort);

            }
            catch (System.Exception ex)
            {
                bConnected = false;
                Debug.WriteLine("初始化失败!" + ex.Message.ToString());
            }
        }
        public bool getConnectState()
        {
            return bConnected;
        }
        public short ConnectServer()//接收信息
        {
            try
            {
                ClientSocket.Connect(ServerInfo);
                sLocalPort = ClientSocket.LocalEndPoint.ToString();
                Thread th = new Thread(ReceiveMsg);
                th.Name = ClientSocket.LocalEndPoint.ToString() + "接收服务器消息线程";
                th.IsBackground = true;
                th.Start();//
                bConnected = true;
                //产生事件
                EventArgs_Connect args = new EventArgs_Connect();
                args.sConnMsg = "";
                args.sConnTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                OnConnect(this, args);
                return 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("登录服务器失败!" + ex.Message.ToString());
                return -1;
            }
        }
        //接收服务器的消息
        void ReceiveMsg()
        {
            while (bRunRecv)
            {
                try
                {
                    byte[] buffer = new byte[MAXSOCKETBUFLEN];
                    int length = ClientSocket.Receive(buffer);

                    if (length <= 0) continue;
                    string strSRecMsg = "";
                    if (OnRecvMsg != null)
                    {

                        EventArgs_Recv args = new EventArgs_Recv();
                        args.btRecvByteBuf = buffer;
                        strSRecMsg = Encoding.Unicode.GetString(args.btRecvByteBuf);
                        args.sRecvMsg = strSRecMsg;
                        args.sRecvTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        OnRecvMsg(this, args);
                    }

                }
                catch (SocketException ex)
                {
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss,fff") + "  接收服务器的消息失败!" + ex.Message.ToString() + "\t" + ex.ErrorCode.ToString());
                    DisconnectServer();
                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss,fff") + "  接收服务器的消息失败!" + ex.Message.ToString());
                    continue;
                }
            }
        }
       
        public short SendToServer(byte[] btSendMsg)//发送信息
        {

            try
            {
                if (getConnectState() == true)
                {
                    ClientSocket.Send(btSendMsg);
                }
                else
                {
                    return -1;
                }
                return 0;
            }
            catch (Exception)
            {

                return -1;
            }
        }
        public void DisconnectServer()
        {
            try
            {
                bRunRecv = false;
                bConnected = false;
                //禁用发送和接受
                ClientSocket.Shutdown(SocketShutdown.Both);
                //关闭套接字，不允许重用
                ClientSocket.Disconnect(false);
                //产生事件
                argss.sDisconnMsg = "";
                argss.sDisconnTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                OnDisconnect(this, argss);

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }
            finally
            {
                ClientSocket.Close();
            }
        }
        public string GetLocalInfo()
        {
            return sLocalPort;
        }
        public short SendToServer(string SendMsg)
        {
            try
            {
                if (bConnected)
                {
                    Byte[] byteSend = Encoding.Default.GetBytes(SendMsg);
                    ClientSocket.Send(byteSend);
                    return 0;
                }
                else
                    return 1;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("发送至服务器的消息失败!" + ex.Message.ToString());
                return -1;
            }
        }

    }
}