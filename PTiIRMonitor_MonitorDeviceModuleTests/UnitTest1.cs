using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTiIRMonitor_MonitorDeviceModule.util;

namespace PTiIRMonitor_MonitorDeviceModuleTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
           // bool isCon = FtpUtil.Connect("ftp://192.168.123.128:21/", "root", "root");
            bool result =  FtpUtil.UploadFile("D:/image/02.jpg", "ftp://192.168.123.128:21", "root", "root");
            if (result)
            {

            }
        }

        [TestMethod]
        public void TestMethod2()
        {
           // bool isCon = FtpUtil.Connect("ftp://192.168.123.128:21", "root", "root");
            bool result = FtpUtil.DeleteFile("01.jpg", "ftp://192.168.123.128:21", "root", "root");
            if (result)
            {

            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            // bool isCon = FtpUtil.Connect("ftp://192.168.123.128:21", "root", "root");
            bool result = FtpUtil.DownloadFile("E://Xiaomi/", "01.jpg", "ftp://192.168.123.128:21/test", "root", "root");
            if (result)
            {

            }
        }

        [TestMethod]
        public void TestMethod4()
        {
           bool isCon = FtpUtil.Connect("ftp://192.168.123.128:21", "root", "root");          
        }
    }
}
