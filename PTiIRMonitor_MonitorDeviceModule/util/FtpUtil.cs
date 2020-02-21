using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PTiIRMonitor_MonitorDeviceModule.util
{
    public class FtpUtil
    {
        private FtpUtil() { }

        private static Socket socket = null;
        private static ManualResetEvent timeoutObject;
        private static bool isConnected = false;

        /// <summary>
        /// 连接ftp服务器
        /// </summary>
        /// <param name="path">地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static bool Connect(string path, string username, string password)
        {
            try
            {
                if (!path.EndsWith("/"))
                {
                    path += "/";
                }
                // 根据uri创建FtpWebRequest对象
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(path));
                //指定命令
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileDownPath">下载路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="FtpPath">ftp文件路径</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static bool DownloadFile(string fileDownPath, string fileName, string FtpPath, string username, string password)
        {
            try
            {
                string onlyFileName = Path.GetFileName(fileName);
                string newFileName = fileDownPath + onlyFileName;
                if (File.Exists(newFileName))
                {
                    string errorinfo = string.Format("文件{0}在该目录下已存在,无法下载", fileName);
                }

                string uri = FtpPath + fileName;
                if (!FtpPath.EndsWith("/"))
                {
                    uri = FtpPath + "/" + fileName;
                }

                // 根据uri创建FtpWebRequest对象
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                FileStream outputStream = new FileStream(newFileName, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="FtpPath"></param>
        /// <param name="Login"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static bool DeleteFile(string fileName, string FtpPath, string Login, string Password)
        {
            try
            {
                FileInfo fileInf = new FileInfo(fileName);
                string uri = FtpPath + fileName;
                if (!FtpPath.EndsWith("/"))
                {
                    uri = FtpPath + "/" + fileInf.Name; ;
                }
                // 根据uri创建FtpWebRequest对象
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(Login, Password);
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="FtpPath"></param>
        /// <param name="Login"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static bool UploadFile(string filename, string FtpPath, string Login, string Password)
        {
            try
            {
                FileInfo fileInf = new FileInfo(filename);
                string uri = FtpPath + fileInf.Name;
                if (!FtpPath.EndsWith("/"))
                {
                    uri = FtpPath + "/" + fileInf.Name; ;
                }

                // 根据uri创建FtpWebRequest对象
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(Login, Password);
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                // 上传文件时通知服务器文件的大小
                reqFTP.ContentLength = fileInf.Length;
                // 缓冲大小设置为kb 
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                // 打开一个文件流(System.IO.FileStream) 去读上传的文件
                FileStream fs = fileInf.OpenRead();
                // 把上传的文件写入流
                Stream strm = reqFTP.GetRequestStream();
                // 每次读文件流的kb 
                contentLen = fs.Read(buff, 0, buffLength);
                // 流内容没有结束
                while (contentLen != 0)
                {
                    // 把内容从file stream 写入upload stream 
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // 关闭两个流
                strm.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        /// <summary>
        /// 获取ftp服务器连接的状态
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="ftpuser">连接名,不为空</param>
        /// <param name="ftppas">密码,不为空</param>
        /// <param name="errmsg"></param>
        /// <param name="port">端口</param>
        /// <param name="timeout">超时</param>
        /// <returns>连接的状态</returns>
        public static bool CheckFtpConnectStatus(string ip, string ftpuser, string ftppsw, out string errmsg, int port = 21, int timeout = 2000)
        {
            #region 输入数据检查  
            errmsg = "";
            IPAddress address;
            try
            {
                address = IPAddress.Parse(ip);
            }
            catch
            {               
                return false;
            }
            #endregion
            isConnected = false;

            bool ret = false;
            byte[] result = new byte[1024];
            int pingStatus = 0, userStatus = 0, pasStatus = 0, exitStatus = 0; //连接返回,用户名返回,密码返回,退出返回
            timeoutObject = new ManualResetEvent(false);
            try
            {
                int receiveLength;

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = timeout;
                socket.ReceiveTimeout = timeout;//超时设置成2000毫秒

                try
                {
                    socket.BeginConnect(new IPEndPoint(address, port), new AsyncCallback(callBackMethod), socket); //开始异步连接请求
                    if (!timeoutObject.WaitOne(timeout, false))
                    {
                        socket.Close();
                        socket = null;
                        pingStatus = -1;
                    }
                    if (isConnected)
                    {
                        pingStatus = 200;
                    }
                    else
                    {
                        pingStatus = -1;
                    }
                }
                catch (Exception ex)
                {
                    pingStatus = -1;
                }

                if (pingStatus == 200) //状态码200 - TCP连接成功
                {
                    receiveLength = socket.Receive(result);
                    pingStatus = getFtpReturnCode(result, receiveLength); //连接状态
                    if (pingStatus == 220)//状态码220 - FTP返回欢迎语
                    {
                        socket.Send(Encoding.Default.GetBytes(string.Format("{0}{1}", "USER " + ftpuser, Environment.NewLine)));
                        receiveLength = socket.Receive(result);
                        userStatus = getFtpReturnCode(result, receiveLength);
                        if (userStatus == 331)//状态码331 - 要求输入密码
                        {
                            socket.Send(Encoding.Default.GetBytes(string.Format("{0}{1}", "PASS " + ftppsw, Environment.NewLine)));
                            receiveLength = socket.Receive(result);
                            pasStatus = getFtpReturnCode(result, receiveLength);
                            if (pasStatus == 230)//状态码230 - 登入因特网
                            {
                                errmsg = string.Format("FTP:{0}@{1}登陆成功", ip, port);
                                ret = true;
                                socket.Send(Encoding.Default.GetBytes(string.Format("{0}{1}", "QUIT", Environment.NewLine))); //登出FTP
                                receiveLength = socket.Receive(result);
                                exitStatus = getFtpReturnCode(result, receiveLength);
                            }
                            else
                            { // 状态码230的错误
                                errmsg = string.Format("FTP:{0}@{1}登陆失败,用户名或密码错误({2})", ip, port, pasStatus);
                            }
                        }
                        else
                        {// 状态码331的错误 
                            errmsg = string.Format("使用用户名:'{0}'登陆FTP:{1}@{2}时发生错误({3}),请检查FTP是否正常配置!", ftpuser, ip, port, userStatus);
                        }
                    }
                    else
                    {// 状态码220的错误 
                        errmsg = string.Format("FTP:{0}@{1}返回状态错误({2}),请检查FTP服务是否正常运行!", ip, port, pingStatus);
                    }
                }
                else
                {// 状态码200的错误
                    errmsg = string.Format("无法连接FTP服务器:{0}@{1},请检查FTP服务是否启动!", ip, port);
                }
            }
            catch (Exception ex)
            { //连接出错 
                errmsg = string.Format("FTP:{0}@{1}连接出错:", ip, port) + ex.Message;             
                ret = false;
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close(); //关闭socket
                    socket = null;
                }
            }
            return ret;
        }
      
        private static void callBackMethod(IAsyncResult asyncResult)
        {
            try
            {
                socket = asyncResult.AsyncState as Socket;
                if (socket != null)
                {
                    socket.EndConnect(asyncResult);
                    isConnected = true;
                }
            }
            catch (Exception ex)
            {
                isConnected = false;
            }
            finally
            {
                timeoutObject.Set();
            }
        }

        private static int getFtpReturnCode(byte[] retByte, int retLen)
        {
            try
            {
                string str = Encoding.ASCII.GetString(retByte, 0, retLen).Trim();
                return int.Parse(str.Substring(0, 3));
            }
            catch
            {
                return -1;
            }
        }
    }
}
