using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.dao;
using PTiIRMonitor_MonitorDeviceModule.entities;
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
            //  bool isCon = FtpUtil.Connect("ftp://192.168.123.128:21", "root", "root");    
            string errmsg = "";
            bool isConnectde = FtpUtil.CheckFtpConnectStatus("192.168.123.128", "root", "root",out errmsg);
        }

        [TestMethod]
        public void TestMethod5()
        {
            string ip = INIUtil.Read("DATABASE", "ip", Constant.IniFilePath);
            string strPort = INIUtil.Read("DATABASE", "port", Constant.IniFilePath);
            string username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
            string password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
            string databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);
            MySqlConnection conn = SqlHelper.GetConnection(ip, Convert.ToInt32(strPort), username, password, databaseName);
            string strSql = "SELECT m.MeasureTask_Index,m.PrePosSet_Index,m.DeviceInfo_Index,m.MeaType,m.TaskName,m.GetValueType," +
            "p.Position_Index,p.PrePositionNO,p.PrePositionName,p.PrePosType,p.TVZoom,p.IRFocus,p.GoPrePosDelays,p.PanAngle,p.TiltAngle,p.PTZHorAngle,p.PTZVerAngle,p.PaletteType  " +
            "from cruise_measuretaskset m, cruise_prepositionset p where m.PrePosSet_Index = p.PrePosSet_Index and p.EnbleMeasure = 1 ";
            //string strSql = "select * from cruise_measuretaskset";

            DataTable datas = SqlHelper.QueryData(conn, strSql, null);
            int count = datas.Rows.Count;
        }

        [TestMethod]
        public void TestMethod6()
        {
            string ip = INIUtil.Read("DATABASE", "ip", Constant.IniFilePath);
            string strPort = INIUtil.Read("DATABASE", "port", Constant.IniFilePath);
            string username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
            string password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
            string databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);
            MySqlConnection conn = SqlHelper.GetConnection(ip, Convert.ToInt32(strPort), username, password, databaseName);
            string strSql = "select * from cruise_spotset where MeasureTask_Index =1";
            DataTable dt= SqlHelper.QueryData(conn, strSql, null);
            int count = dt.Rows.Count;
            List<SpotSet> spots =new List<SpotSet>();
        }

        [TestMethod]
        public void TestMethod7()
        {
            //MeasureTaskSetDao dao1 = new MeasureTaskSetDao();
            //List<int> IndexList = dao1.GetMeasureTaskIndex();
            // int i = IndexList.Count;

            //  PrePositionSetDao dao2 = new PrePositionSetDao();
            // List< PrePositionSet> prePositionList= dao2.GetPrePosIndexByTaskIndex(1);
            string sql = "select * from record_tempmeasure where TempMeasure_Index=1";
           MySqlConnection conn = SqlHelper.getConnection();
           DataTable dt = SqlHelper.QueryData(conn, sql, null);
           for(int i = 0; i < dt.Rows.Count; i++)
            {
                TempMeasure tempMeasure = new TempMeasure();
                tempMeasure.TVFilePath = dt.Rows[i]["TVFilePath"].ToString();
            }
        }

    }
}
