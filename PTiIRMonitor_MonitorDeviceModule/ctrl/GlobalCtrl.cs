using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Peiport.Monitor.IRTelnetCtrl;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.entities;
using PTiIRMonitor_MonitorDeviceModule.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
    public class GlobalCtrl
    {
        //状态全局变量
        public int TVConnectStatus = 0;
        public bool IRConnectStatus = false;    
        public bool CruiseStatus = false;
        public int LoginStatus = 0;
        public bool DatabaseStatus = false;
        public bool FtpStatus = false;

        public bool HeartStatus = true;
        public int HeartBeatCount = 0;

        //连接控制全局变量
        bool TVConnectCtrl = false;
        bool IRConnectCtrl = false;
        bool PTZConnectCtrl = false;
        bool CruiseCtrl = false;

        public SysCtrl sysCtrl = new SysCtrl();
        public TVCtrl tvCtrl = new TVCtrl();
        public IRCtrl irCtrl = new IRCtrl();
        TempHumCtrl tempHumCtrl = new TempHumCtrl();
        RobotCtrl robotCtrl = new RobotCtrl();

        IRTelnetCtrl ir;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {          
            sysCtrl.Init();
            irCtrl.Init();          
            GetFtpConnect();  
        }
        #region 连接
        public bool TVConnect()
        {
           return tvCtrl.Connect(true);
        }
        public bool IRConnect()
        {
            IRConnectStatus = true;
           return irCtrl.Connect();
        }
        #endregion
        /// <summary>
        /// 退出系统
        /// </summary>
        public void Exit()
        {

        }       
        
        /// <summary>
        /// 红外监控状态扫描
        /// </summary>
        public bool IRScanState()
        {
            return irCtrl.GetIRConnectStatus();
        }
        /// <summary>
        /// 可将光监控状态扫描
        /// </summary>
        public int TVScanState()
        {
           
            if (tvCtrl.GetTVStatus() == -1)
            {
                TVConnectStatus = -1;
            }
            else if(tvCtrl.GetTVStatus() == 0)
            {
                TVConnectStatus = 0;
            }
            else if (tvCtrl.GetTVStatus() == 1)
            {
                TVConnectStatus = 1;
            }
            else if (tvCtrl.GetTVStatus() == 2)
            {
                TVConnectStatus = 2;
            }
            return TVConnectStatus;
        }
        /// <summary>
        /// 云台监控扫描
        /// </summary>
      
        /// <summary>
        /// 自动巡检监控扫描
        /// </summary>
        public void CruiseStatusScan()
        {

        }
        /// <summary>
        /// 检查通讯扫命令扫描（或委托）
        /// </summary>
        public string ReceiveMsgScan(JObject job)
        {
            string strJson = null;
            if (job != null)
            {
                string seq = job["seq"].ToString();
                int cmdType = Convert.ToInt32(job["cmdType"].ToString());
                string cmdAction = job["cmdAction"].ToString();
                string sender = job["receiver"].ToString();
                string receiver = job["sender"].ToString();

                JsonItem jsonItem = new JsonItem();
                jsonItem.seq = seq;
                jsonItem.cmdAction = cmdAction;
                jsonItem.cmdType = cmdType;
                jsonItem.sender = sender;
                jsonItem.receiver = receiver;

                try
                {
                    #region 系统
                    if (job["cmdType"].ToString() == "1")        //SYS
                    {
                        if (job["cmdAction"].ToString() == "Login")  //登录
                        {

                            if (job["paramList"] != null)
                            {
                                LoginStatus = 1;
                                //List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                //string par1 = "";
                                //foreach (JsonitemParam par in pars)
                                //{
                                //    if (par.param.Equals("state"))
                                //    {
                                //        par1 = par.value;
                                //    }
                                //}
                                //if (par1.Equals("1"))
                                //{
                                //    LoginStatus = 1;
                                //}
                                //else
                                //{
                                //    LoginStatus = 0;
                                //}
                            }
                        }
                        if (job["cmdAction"].ToString() == "Palpitate")         //心跳
                        {
                            if (job["result"].ToString() == Constant.Result_OK)
                            {

                            }
                            else
                            {

                            }
                        }
                        if (job["cmdAction"].ToString() == "Logout")         //退出
                        {

                        }
                        if (job["cmdAction"].ToString() == "CruiseSet")         //巡检设置
                        {
                            if (job["paramList"] != null)
                            {
                                List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                string par1 = "";
                                foreach (JsonitemParam par in pars)
                                {
                                    if (par.param.Equals("setUp"))
                                    {
                                        par1 = par.value;
                                    }
                                }
                                if (par1 == "0")
                                {
                                    CruiseStatus = sysCtrl.StartCruise(0);
                                }
                                else
                                {
                                    CruiseStatus = sysCtrl.StartCruise(1);
                                }
                            }
                        }
                        if (job["cmdAction"].ToString() == "CruiseStateGet")  //巡检状态获取
                        {
                            JsonitemParam par = new JsonitemParam();
                            par.param = "state";

                            if (sysCtrl.cruiseState)
                            {
                                par.value = "0";
                                Debug.WriteLine("系统正在巡检...");
                            }
                            else
                            {
                                par.value = "1";
                                Debug.WriteLine("系统未巡检...");
                            }
                            jsonItem.paramList.Add(par);
                            strJson = JsonConvert.SerializeObject(jsonItem);
                        }
                        if (job["cmdAction"].ToString() == "MonDevRestart")
                        {
                            if (job["paramList"] != null)
                            {
                                List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                string par1 = "";
                                foreach (JsonitemParam par in pars)
                                {
                                    if (par.param.Equals("NO"))
                                    {
                                        par1 = par.value;
                                    }
                                }
                                sysCtrl.ReStartMonDev(Convert.ToInt32(par1));
                            }
                        }
                    }

                    #endregion
                    if (!CruiseStatus)
                    {
                        #region 云台
                        if (job["cmdType"].ToString() == "2")  //PTZ
                        {
                            if (job["cmdAction"].ToString() == "PTZMove")
                            {
                                if (job["paramList"] != null)
                                {
                                    List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                    string par1 = "0", par2 = "0";
                                    foreach (JsonitemParam par in pars)
                                    {
                                        if (par.param.Equals("xVelo"))
                                        {
                                            par1 = par.value;
                                        }
                                        if (par.param.Equals("yVelo"))
                                        {
                                            par2 = par.value;
                                        }
                                    }
                                    tvCtrl.PTZMove(Convert.ToInt32(par1), Convert.ToInt32(par2));
                                    jsonItem.result = Constant.Result_OK;
                                    jsonItem.paramList = pars;
                                    strJson = JsonConvert.SerializeObject(jsonItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "PTZMoveAngleSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                    string par1 = "0", par2 = "0";
                                    foreach (JsonitemParam par in pars)
                                    {
                                        if (par.param.Equals("honAngle"))
                                        {
                                            par1 = par.value;
                                        }
                                        if (par.param.Equals("verAngle"))
                                        {
                                            par2 = par.value;
                                        }
                                    }
                                    tvCtrl.SetAngle(Convert.ToInt32(par1), Convert.ToInt32(par2));
                                    jsonItem.paramList = pars;
                                    jsonItem.result = Constant.Result_OK;
                                    strJson = JsonConvert.SerializeObject(jsonItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "PrePosSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                    string par1 = "0";
                                    foreach (JsonitemParam par in pars)
                                    {
                                        if (par.param.Equals("NO"))
                                        {
                                            par1 = par.value;
                                        }
                                    }
                                    tvCtrl.SetPrePos(Convert.ToInt32(par1));
                                    JsonitemParam par2 = new JsonitemParam();
                                    par2.param = "state";
                                    par2.value = "1";
                                    pars.Add(par2);
                                    jsonItem.paramList = pars;
                                    jsonItem.result = Constant.Result_OK;
                                    strJson = JsonConvert.SerializeObject(jsonItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "PrePosInvoke")
                            {
                                if (job["paramList"] != null)
                                {
                                    List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                    string par1 = "0";
                                    foreach (JsonitemParam par in pars)
                                    {
                                        if (par.param.Equals("NO"))
                                        {
                                            par1 = par.value;
                                        }
                                    }
                                    tvCtrl.InvokePrePos(Convert.ToInt32(par1));
                                    JsonitemParam par2 = new JsonitemParam();
                                    par2.param = "state";
                                    par2.value = "1";
                                    pars.Add(par2);
                                    jsonItem.paramList = pars;
                                    jsonItem.result = Constant.Result_OK;
                                    strJson = JsonConvert.SerializeObject(jsonItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "PTZAngleGet")
                            {
                                int honAngle = 0, verAngle = 0;
                                tvCtrl.GetAngle(ref honAngle, ref verAngle);
                                jsonItem.result = Constant.Result_OK;
                                List<JsonitemParam> pars = new List<JsonitemParam>();
                                JsonitemParam par1 = new JsonitemParam();
                                par1.param = "honAngle";
                                par1.value = honAngle.ToString();
                                pars.Add(par1);
                                JsonitemParam par2 = new JsonitemParam();
                                par2.param = "verAngle";
                                par2.value = verAngle.ToString();
                                pars.Add(par2);

                                jsonItem.paramList = pars;
                                jsonItem.result = Constant.Result_OK;
                                strJson = JsonConvert.SerializeObject(jsonItem);
                            }
                            if (job["cmdAction"].ToString() == "PTZInit")
                            {
                                tvCtrl.PTZInit();
                                jsonItem.result = Constant.Result_OK;
                                strJson = JsonConvert.SerializeObject(jsonItem);
                            }
                        }
                        #endregion
                        #region 可见光
                        if (job["cmdType"].ToString() == "3")  //TV
                        {

                            if (job["cmdAction"].ToString() == "ZoomOpt")
                            {
                                if (job["paramList"] != null)
                                {
                                    List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                    string par1 = "0";
                                    foreach (JsonitemParam par in pars)
                                    {
                                        if (par.param.Equals("zoomType"))
                                        {
                                            par1 = par.value;
                                        }
                                    }
                                    if (tvCtrl.SetZoomOpt(Convert.ToInt32(par1)))
                                    {
                                        jsonItem.paramList = pars;
                                        jsonItem.result = Constant.Result_OK;
                                    }
                                    else
                                    {
                                        jsonItem.result = Constant.Result_ERROR;
                                    }
                                    strJson = JsonConvert.SerializeObject(jsonItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "ZoomPosSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                    string par1 = "0";
                                    foreach (JsonitemParam par in pars)
                                    {
                                        if (par.param.Equals("fValue"))
                                        {
                                            par1 = par.value;
                                        }
                                    }
                                    if (tvCtrl.SetZoomPos(Convert.ToInt32(par1)))
                                    {
                                        jsonItem.paramList = pars;
                                        jsonItem.result = Constant.Result_OK;
                                    }
                                    else
                                    {
                                        jsonItem.result = Constant.Result_ERROR;
                                    }
                                    strJson = JsonConvert.SerializeObject(jsonItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "ZoomPosGet")
                            {
                                float value = tvCtrl.GetZoomPos();
                                JsonitemParam par1 = new JsonitemParam();
                                par1.param = "fvalue";
                                par1.value = value.ToString();
                                jsonItem.result = Constant.Result_OK;
                                jsonItem.paramList.Add(par1);
                                strJson = JsonConvert.SerializeObject(jsonItem);
                            }
                            if (job["cmdAction"].ToString() == "SaveImg")
                            {
                                string fileName = "dev01_01_" + DateUtil.DateToString();
                                JsonitemParam par1 = new JsonitemParam();
                                if (tvCtrl.SaveImage(fileName))
                                {
                                    jsonItem.result = Constant.Result_OK;
                                    par1.param = "fileName";
                                    par1.value = fileName;
                                    jsonItem.paramList.Add(par1);
                                }
                                else
                                {
                                    jsonItem.result = Constant.Result_ERROR;
                                }
                                strJson = JsonConvert.SerializeObject(jsonItem);
                            }

                        }
                        #endregion
                        #region 红外控制
                        if (job["cmdType"].ToString() == "4")  //IRC
                        {
                            if (job["cmdAction"].ToString() == "ManualFocus")
                            {
                                if (job["paramList"] != null)
                                {
                                    List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                    string par1 = "0";
                                    foreach (JsonitemParam par in pars)
                                    {
                                        if (par.param.Equals("focusType"))
                                        {
                                            par1 = par.value;
                                        }
                                    }
                                    if (irCtrl.SetManualFocus(Convert.ToInt32(par1)))
                                    {
                                        jsonItem.paramList = pars;
                                        jsonItem.result = Constant.Result_OK;
                                    }
                                    else
                                    {
                                        jsonItem.result = Constant.Result_ERROR;
                                    }
                                    strJson = JsonConvert.SerializeObject(jsonItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "AutoFocus")
                            {
                                if (irCtrl.SetAutoFocus())
                                {
                                    jsonItem.result = Constant.Result_OK;
                                }
                                else
                                {
                                    jsonItem.result = Constant.Result_ERROR;
                                }
                                strJson = JsonConvert.SerializeObject(jsonItem);

                            }
                            if (job["cmdAction"].ToString() == "FocusPosSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                    string par1 = "";
                                    foreach (JsonitemParam par in pars)
                                    {
                                        if (par.param.Equals("fValue"))
                                        {
                                            par1 = par.value;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(par1))
                                    {
                                        if (irCtrl.SetFocusPos(Convert.ToInt32(par1)))
                                        {
                                            jsonItem.paramList = pars;
                                            jsonItem.result = Constant.Result_OK;
                                        }
                                        else
                                        {
                                            jsonItem.result = Constant.Result_ERROR;
                                        }
                                    }

                                    strJson = JsonConvert.SerializeObject(jsonItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "FocusPosGet")
                            {
                                int value = -1;
                                if (irCtrl.GetFocusPos(ref value))
                                {
                                    jsonItem.result = Constant.Result_OK;
                                    JsonitemParam par1 = new JsonitemParam();
                                    par1.param = "fValue";
                                    par1.value = value.ToString();
                                }
                                else
                                {
                                    jsonItem.result = Constant.Result_ERROR;
                                    JsonitemParam par1 = new JsonitemParam();
                                    par1.param = "fValue";
                                    par1.value = "-1";
                                }
                                strJson = JsonConvert.SerializeObject(jsonItem);
                            }
                            if (job["cmdAction"].ToString() == "PaletteSet")
                            {
                                if (irCtrl.SetPalette())
                                {
                                    jsonItem.result = Constant.Result_OK;
                                }
                                else
                                {
                                    jsonItem.result = Constant.Result_ERROR;
                                }
                                strJson = JsonConvert.SerializeObject(jsonItem);
                            }
                            if (job["cmdAction"].ToString() == "DigZoomSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    //  irCtrl.SetDigZoom(Convert.ToInt32(job["paramList"][0]["value"].ToString()));

                                }
                            }
                            if (job["cmdAction"].ToString() == "DigZoomGet")
                            {
                                irCtrl.GetDigZoom();
                            }
                            if (job["cmdAction"].ToString() == "AutoAdjustSet")
                            {
                                if (irCtrl.SetImageAdjustMode(true, 0, 0))
                                {
                                    jsonItem.result = Constant.Result_OK;
                                }
                                else
                                {
                                    jsonItem.result = Constant.Result_ERROR;
                                }
                                strJson = JsonConvert.SerializeObject(jsonItem);
                            }
                            if (job["cmdAction"].ToString() == "ManualAdjustSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                    string par1 = "", par2 = "";
                                    foreach (JsonitemParam par in pars)
                                    {
                                        if (par.param.Equals("Brightness"))
                                        {
                                            par1 = par.value;
                                        }
                                        if (par.param.Equals("Contrast"))
                                        {
                                            par2 = par.value;
                                        }
                                    }
                                    if (irCtrl.SetImageAdjustMode(false, Convert.ToInt32(par1), Convert.ToInt32(par2)))
                                    {
                                        jsonItem.paramList = pars;
                                        jsonItem.result = Constant.Result_OK;
                                    }
                                    else
                                    {
                                        jsonItem.result = Constant.Result_ERROR;
                                    }
                                    strJson = JsonConvert.SerializeObject(jsonItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "SaveIRHotImg")
                            {
                                IRCtrl ctrl = irCtrl;
                                string fileName = "IRHotPic.img";
                                //bool flag = ir.ConnectIRTelnet();
                                //bool sss = ir.SaveHotPic(fileName);
                                //Debug.WriteLine("irCtrl=" + ir.ToString());
                                if (irCtrl.SaveIRHotImage(fileName))
                                {
                                    jsonItem.result = Constant.Result_OK;
                                    JsonitemParam par1 = new JsonitemParam();
                                    par1.param = "strFileName";
                                    par1.value = fileName;
                                    jsonItem.paramList.Add(par1);
                                }
                                else
                                {
                                    jsonItem.result = Constant.Result_ERROR;
                                }
                                strJson = JsonConvert.SerializeObject(jsonItem);
                            }
                            if (job["cmdAction"].ToString() == "SaveVideoImg")
                            {
                                string fileName = "IRVideoPic";
                                if (irCtrl.SaveVideoImage(fileName))
                                {
                                    jsonItem.result = Constant.Result_OK;
                                    JsonitemParam par1 = new JsonitemParam();
                                    par1.param = "strFileName";
                                    par1.value = fileName;
                                    jsonItem.paramList.Add(par1);
                                }
                                else
                                {
                                    jsonItem.result = Constant.Result_ERROR;
                                }
                                strJson = JsonConvert.SerializeObject(jsonItem);
                            }
                        }
                        #endregion
                        #region 红外分析
                        if (job["cmdType"].ToString() == "5")  //IRA
                        {
                            if (job["cmdAction"].ToString() == "EmissivitySet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.SetEmissivity(Convert.ToDouble(job["paramList"][0]["emiss"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "EmissivityGet")
                            {
                                irCtrl.GetEmissivity();
                            }
                            if (job["cmdAction"].ToString() == "RefTempSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.SetRefTemp(Convert.ToDouble(job["paramList"][0]["fValue"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "RefTempGet")
                            {
                                irCtrl.GetRefTemp();
                            }
                            if (job["cmdAction"].ToString() == "AirTempSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.SetAirTemp(Convert.ToDouble(job["paramList"][0]["fValue"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AirTempGet")
                            {
                                irCtrl.GetAirTemp();
                            }
                            if (job["cmdAction"].ToString() == "AirHumSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.SetAirHum(Convert.ToDouble(job["paramList"][0]["fValue"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AirHumGet")
                            {
                                irCtrl.GetAirHum();
                            }
                            if (job["cmdAction"].ToString() == "DistanceSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.SetDistance(Convert.ToDouble(job["paramList"][0]["fValue"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "DistanceGet")
                            {
                                irCtrl.GetDistance();
                            }
                            if (job["cmdAction"].ToString() == "WinTempSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.SetWinTemp(Convert.ToDouble(job["paramList"][0]["fValue"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "WinTempGet")
                            {
                                irCtrl.GetWinTemp();
                            }
                            if (job["cmdAction"].ToString() == "WinTrmRateSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.SetWinTrmRate(Convert.ToDouble(job["paramList"][0]["fValue"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "WinTrmRateGet")
                            {
                                irCtrl.GetWinTrmRate();
                            }
                            if (job["cmdAction"].ToString() == "AnaStateGet")
                            {
                                irCtrl.GetAnaState();
                            }
                            if (job["cmdAction"].ToString() == "AnaClearAll")
                            {
                                irCtrl.ClearAnaAll();
                                jsonItem.result = Constant.Result_OK;
                                strJson = JsonConvert.SerializeObject(jsonItem);
                            }
                            #region 点温相关
                            if (job["cmdAction"].ToString() == "AnaSpotPosSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                    string uNO = "-1", SpotX = "0", SpotY = "0", fDist = "", fEmiss = "";
                                    foreach (JsonitemParam par in pars)
                                    {
                                        if (par.param.Equals("uNO"))
                                        {
                                            uNO = par.value;
                                        }
                                        if (par.param.Equals("SpotX"))
                                        {
                                            SpotX = par.value;
                                        }
                                        if (par.param.Equals("SpotY"))
                                        {
                                            SpotY = par.value;
                                        }
                                        if (par.param.Equals("fDist"))
                                        {
                                            fDist = par.value;
                                        }
                                        if (par.param.Equals("fEmiss"))
                                        {
                                            fEmiss = par.value;
                                        }
                                    }
                                    if (irCtrl.SetAnaSpotPos(Convert.ToInt32(uNO), Convert.ToInt32(SpotX), Convert.ToInt32(SpotY), Convert.ToDouble(fDist), Convert.ToDouble(fEmiss)))
                                    {
                                        jsonItem.paramList = pars;
                                        jsonItem.result = Constant.Result_OK;
                                    }
                                    else
                                    {
                                        jsonItem.result = Constant.Result_ERROR;
                                    }
                                    strJson = JsonConvert.SerializeObject(jsonItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaSpotPosGet")
                            {
                                List<JsonitemParam> pars = JsonConvert.DeserializeObject<List<JsonitemParam>>(job["paramList"].ToString());
                                string uNO = "-1";
                                foreach (JsonitemParam par in pars)
                                {
                                    if (par.param.Equals("uNO"))
                                    {
                                        uNO = par.value;
                                    }
                                }
                                irCtrl.GetAnaSpotPos(Convert.ToInt32(uNO));
                            }
                            if (job["cmdAction"].ToString() == "AnaSpotParamSet")
                            {
                                if (job["paramList"] != null)
                                {

                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaSpotParamGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaSpotPos(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaSpotTempGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaSpotTemp(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            #endregion
                            #region 线温
                            if (job["cmdAction"].ToString() == "AnaLinePosSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.SetAnaLinePos(Convert.ToInt32(job["paramList"][0]["uNO"]), Convert.ToInt32(job["paramList"][0]["x1"]), Convert.ToInt32(job["paramList"][0]["y1"]),
                                       Convert.ToInt32(job["paramList"][0]["x2"]), Convert.ToInt32(job["paramList"][0]["y2"]), Convert.ToDouble(job["paramList"][0]["fDist"]),
                                       Convert.ToDouble(job["paramList"][0]["fEmiss"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaLinePosGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaLinePos(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaLineParamSet")
                            {
                                if (job["paramList"] != null)
                                {

                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaLineParamGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaLinePos(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaLineTempGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaLineTemp(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            #endregion
                            #region 矩形区域测温
                            if (job["cmdAction"].ToString() == "AnaAreaPosSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.SetAnaAreaPos(Convert.ToInt32(job["paramList"][0]["uNO"]), Convert.ToInt32(job["paramList"][0]["fStartX"]), Convert.ToInt32(job["paramList"][0]["fStartY"]),
                                       Convert.ToInt32(job["paramList"][0]["fWidth"]), Convert.ToInt32(job["paramList"][0]["fHeight"]), Convert.ToDouble(job["paramList"][0]["fDist"]),
                                       Convert.ToDouble(job["paramList"][0]["fEmiss"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaAreaPosGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaAreaPos(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaAreaParamSet")
                            {
                                if (job["paramList"] != null)
                                {

                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaAreaParamGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaAreaPos(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaAreaTempGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaAreaTemp(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            #endregion
                            #region 多边形测温
                            if (job["cmdAction"].ToString() == "AnaPolyPosSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    Dictionary<string, int> dictionary = new Dictionary<string, int>();
                                    string[] paramArray = job["paramList"].ToString().Split(',');

                                    for (int i = 1; i <= paramArray.Length / 2; i++)
                                    {
                                        dictionary.Add("fLeftPer" + i, Convert.ToInt32(job["paramList"][0]["fLeftPer" + i].ToString()));
                                        dictionary.Add("fTopPer" + i, Convert.ToInt32(job["paramList"][0]["fTopPer" + i].ToString()));
                                    }

                                    irCtrl.SetAnaPolyPos(Convert.ToInt32(job["paramList"][0]["uNO"].ToString()), dictionary);
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaPolyPosGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaPolyPos(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaPolyParamSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    IRAnaParamItem paramItem = new IRAnaParamItem();
                                    paramItem.uNO = Convert.ToInt32(job["paramList"][0]["uNO"]);
                                    paramItem.uActive = Convert.ToBoolean(job["paramList"][0]["uActive"]);
                                    paramItem.uUseLocal = Convert.ToBoolean(job["paramList"][0]["uUseLocal"]);
                                    paramItem.fEmiss = Convert.ToDouble(job["paramList"][0]["fEmiss"]);
                                    paramItem.fDis = Convert.ToDouble(job["paramList"][0]["fDis"]);

                                    irCtrl.SetAnaPolyParam(paramItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaPolyParamGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaPolyParam(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaPolyTempGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaPolyTemp(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            #endregion
                            #region 圆形区域测温
                            if (job["cmdAction"].ToString() == "AnaCirclePosSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.SetAnaCirclePos(Convert.ToInt32(job["paramList"][0]["uNO"]), Convert.ToInt32(job["paramList"][0]["fCenterLeftPer"]),
                                        Convert.ToInt32(job["paramList"][0]["fCenterTopPer"]), Convert.ToInt32(job["paramList"][0]["fRadiusWidthPer"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaCirclePosGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaCirclePos(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaCircleParamSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    IRAnaParamItem paramItem = new IRAnaParamItem();
                                    paramItem.uNO = Convert.ToInt32(job["paramList"][0]["uNO"]);
                                    paramItem.uActive = Convert.ToBoolean(job["paramList"][0]["uActive"]);
                                    paramItem.uUseLocal = Convert.ToBoolean(job["paramList"][0]["uUseLocal"]);
                                    paramItem.fEmiss = Convert.ToDouble(job["paramList"][0]["fEmiss"]);
                                    paramItem.fDis = Convert.ToDouble(job["paramList"][0]["fDis"]);

                                    irCtrl.SetAnaCircleParam(paramItem);
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaCircleParamGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaCircleParam(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "AnaCircleTempGet")
                            {
                                if (job["paramList"] != null)
                                {
                                    irCtrl.GetAnaCircleTemp(Convert.ToInt32(job["paramList"][0]["uNO"]));
                                }
                            }
                            #endregion

                        }
                        #endregion
                        #region 温湿度
                        if (job["cmdType"].ToString() == "6")  //SEN
                        {
                            if (job["cmdAction"].ToString() == "TempHumGet")
                            {
                                tempHumCtrl.GetTempHum();
                            }

                        }
                        #endregion
                        #region 机器人
                        if (job["cmdType"].ToString() == "7")  //ROB
                        {
                            if (job["cmdAction"].ToString() == "WorkStateGet")
                            {
                                robotCtrl.GetWorkState();
                            }
                            if (job["cmdAction"].ToString() == "UrgencyStop")
                            {
                                if (job["paramList"] != null)
                                {
                                    if (Convert.ToInt32(job["paramList"][0]["fMin"].ToString()) == 0)
                                    {
                                        robotCtrl.Stop(false);
                                    }
                                    else
                                    {
                                        robotCtrl.Stop(true);
                                    }
                                }
                            }
                            if (job["cmdAction"].ToString() == "ROBMove")
                            {
                                if (job["paramList"] != null)
                                {
                                    robotCtrl.Move(Convert.ToDouble(job["paramList"][0]["iForVelo"]), Convert.ToDouble(job["paramList"][0]["iRotaVelo"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "CurrentPosGet")
                            {
                                robotCtrl.GetCurrentPos();
                            }
                            if (job["cmdAction"].ToString() == "ManualMovePosSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    robotCtrl.SetManualMovePosParam(Convert.ToDouble(job["paramList"][0]["fRefPosX"]), Convert.ToDouble(job["paramList"][0]["fRefPosY"]),
                                    Convert.ToDouble(job["paramList"][0]["fAbsAngle"]));
                                }
                            }
                            if (job["cmdAction"].ToString() == "MoveSpePosSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    robotCtrl.SetMoveSpePos(job["paramList"][0]["PosType"].ToString());
                                }
                            }
                            if (job["cmdAction"].ToString() == "PowersetUpSet")
                            {
                                if (job["paramList"] != null)
                                {
                                    if (Convert.ToInt32(job["paramList"][0]["setUp"]) == 0)
                                    {
                                        robotCtrl.SetPower(false);
                                    }
                                    else
                                    {
                                        robotCtrl.SetPower(true);
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                }
                catch(Exception e)
                {
                    Debug.WriteLine("异常信息:" + e.Message);
                }
                
            }
            return strJson;
        }

        public void GetFtpConnect()
        {
            string ftp_ip = INIUtil.Read("Ftp", "ip", Constant.IniFilePath);
            string rootDirectory = "ftp://" + ftp_ip;
            string ftp_username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
            string ftp_password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);

            try
            {
                if (FtpUtil.Connect(rootDirectory, ftp_username, ftp_password))
                {
                    FtpStatus = true;
                    Debug.WriteLine("ftp连接成功...");
                }
                else
                {
                    FtpStatus = false;
                    Debug.WriteLine("ftp连接失败...");
                }
            }
            catch(Exception e)
            {
                FtpStatus = false;
            }
            
        }

        public void GetSqlConnection()
        {
            MySqlConnection conn = null;
            try
            {
                string ip = INIUtil.Read("DATABASE", "ip", Constant.IniFilePath);
                string strPort = INIUtil.Read("DATABASE", "port", Constant.IniFilePath);
                string username = INIUtil.Read("DATABASE", "username", Constant.IniFilePath);
                string password = INIUtil.Read("DATABASE", "password", Constant.IniFilePath);
                string databaseName = INIUtil.Read("DATABASE", "databaseName", Constant.IniFilePath);
                conn = SqlHelper.GetConnection(ip, Convert.ToInt32(strPort), username, password, databaseName);
                conn.Open();
                DatabaseStatus = true;
                Debug.WriteLine("数据库连接成功...");
            }
            catch(Exception ex)
            {
                DatabaseStatus = false;
                Debug.WriteLine("数据库连接失败...");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
