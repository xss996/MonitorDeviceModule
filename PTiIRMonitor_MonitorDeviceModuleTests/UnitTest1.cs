using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.ctrl;
using PTiIRMonitor_MonitorDeviceModule.entities;
using PTiIRMonitor_MonitorDeviceModule.util;

namespace PTiIRMonitor_MonitorDeviceModuleTests
{
    [TestClass]
    public class UnitTest1
    {
        GlobalCtrl ctrl = new GlobalCtrl();
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                string ip = INIUtil.Read("DATABASE", "ip", Constant.IniFilePath);
                string strPort = INIUtil.Read("DATABASE", "port", Constant.IniFilePath);
                string username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
                string password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
                string databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);
                MySqlConnection conn = SqlHelper.GetConnection(ip, Convert.ToInt32(strPort), username, password, databaseName);
                conn.Open();
              //  DatabaseStatus = true;
                Debug.WriteLine("数据库连接成功...");
            }
            catch (Exception ex)
            {
              //  DatabaseStatus = false;
                Debug.WriteLine("数据库连接失败...");
            }
        }
        [TestMethod]
        public void TestMethod2()
        {
             long ret = INIUtil.Write("USER", "username", "admin123", @"E:\projects\Net项目备份\变电站无人值守系统\Output\Monitor.ini"); 
            //string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.ini");
        }

        [TestMethod]
        public void TestMethod3()
        {
            // bool flag = TelnetUtil.PingIpOrDomainName("192.168.123.126");
            JsonItem item = new JsonItem();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("xxx", "123");
            dictionary.Add("xcsjd", "wher");
            string strJson = JsonConvert.SerializeObject(dictionary);
            Console.WriteLine(strJson);
        }
    }
}
