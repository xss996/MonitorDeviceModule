using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.util
{
   public class FtpUtil
    {
        private FtpUtil() { }

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
                // 根据uri创建FtpWebRequest对象
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(path));

                //指定命令
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;

                // 指定数据传输类型
                reqFTP.UseBinary = true;

                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);

                //
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
        public static string DownloadFile(string fileDownPath, string fileName, string FtpPath, string username, string password)
        {
            try
            {
                string onlyFileName = Path.GetFileName(fileName);

                string newFileName = fileDownPath + onlyFileName;

                if (File.Exists(newFileName))

                {

                    string errorinfo = string.Format("文件{0}在该目录下已存在,无法下载", fileName);

                    return errorinfo;
                }

                string uri = "ftp://" + Path.Combine(FtpPath, fileName);

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

                //Successinfo

                return string.Format("服务器文件{0}已成功下载", fileName);

            }

            catch (Exception ex)

            {
                //errorinfo
                return string.Format("因{0},无法下载", ex.Message);

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
        public static string DeleteFile(string fileName, string FtpPath, string Login, string Password)
        {
            try
            {
                FileInfo fileInf = new FileInfo(fileName);

                string uri = "ftp://" + Path.Combine(FtpPath, fileInf.Name);

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

                //Successinfo

                return string.Format("文件{0}已成功删除", fileInf.Name);
            }

            catch (Exception ex)
            {
                //ErrorInfo 
                return string.Format("文件因{0},无法删除", ex.Message);
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
        public static string UploadFile(string filename, string FtpPath, string Login, string Password)
        {
            try

            {
                FileInfo fileInf = new FileInfo(filename);

                //判断是否有上级目录

                string uri = "ftp://" + Path.Combine(FtpPath, fileInf.Name);

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
                //Successinfo
                return string.Format("本地文件{0}已成功上传", fileInf.Name);
            }

            catch (Exception ex)

            {
                //ErrorInfo
                return "上传失败" + ex.Message;
            }

        }
    }
}
