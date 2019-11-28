using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
   public class IRAnalysis
    {
        //public void SetEmissivity(double emiss)    //范围0.00-1.0
        //{
        //    Debug.WriteLine(string.Format("当前辐射率设置为:{0}",emiss));
        //}

        //public void GetEmissivity()
        //{
        //    Debug.WriteLine(string.Format("当前辐射率获取..."));
        //}

        //public void SetRefTemp(double fValue)
        //{
        //    Debug.WriteLine(string.Format("当前反射温度设置为:{0}",fValue));
        //}
        //public void GetRefTemp()
        //{
        //    Debug.WriteLine(string.Format("当前反射温度获取"));
        //}

        //public void SetAirTemp(double fValue)
        //{
        //    Debug.WriteLine(string.Format("当前空气温度设置为:{0}", fValue));
        //}
        //public void GetAirTemp()
        //{
        //    Debug.WriteLine(string.Format("当前空气温度获取"));
        //}

        //public void SetAirHum(double fValue)
        //{
        //    Debug.WriteLine(string.Format("当前空气湿度设置为:{0}", fValue));
        //}
        //public void GetAirHum()
        //{
        //    Debug.WriteLine(string.Format("当前空气湿度获取"));
        //}

        //public void SetDistance(double fValue)
        //{
        //    Debug.WriteLine(string.Format("当前距离设置为:{0}", fValue));
        //}
        //public void GetDistance()
        //{
        //    Debug.WriteLine(string.Format("当前距离获取"));
        //}

        //public void SetWinTemp(double fValue)
        //{
        //    Debug.WriteLine(string.Format("当前窗口温度设置为:{0}", fValue));
        //}
        //public void GetWinTemp()
        //{
        //    Debug.WriteLine(string.Format("当前窗口温度获取"));
        //}

        //public void SetWinTrmRate(double fValue)
        //{
        //    Debug.WriteLine(string.Format("当前窗口穿透率设置为:{0}", fValue));
        //}
        //public void GetWinTrmRate()
        //{
        //    Debug.WriteLine(string.Format("当前窗口穿透率获取"));
        //}

        //public void GetAnaState()                   //type,0-无操作，1点温，2-线温，3-矩形，4-多边形，5-圆形; NO,表示序号
        //{
        //    Debug.WriteLine(string.Format("当前测温状态获取:{0}",0));
        //}

        //public void ClearAnaAll()
        //{
        //    Debug.WriteLine(string.Format("清除所有的测温点"));
        //}
        //#region 点温相关
        //public void SetAnaSpotPos(int uNO,double fLeftPer,double fTopPer)
        //{
        //    Debug.WriteLine("编号为{0}的点温位置参数为:{1},{2}",uNO,fLeftPer,fTopPer);
        //}

        //public void GetAnaSpotPos(int uNO)
        //{
        //    Debug.WriteLine("获取编号为{0}的点温位置参数...",uNO);
        //}

        //public void SetAnaSpotParam(IRAnaParamItem paramObj)
        //{
        //    Debug.WriteLine("点温设置相关参数为:{0}", paramObj);
        //}
        //public void GetAnaSpotParam(int uNO)
        //{
        //    IRAnaParamItem paramObj = new IRAnaParamItem();
        //    paramObj.uNO = uNO;
        //    Debug.WriteLine("点温获取相关参数为:{0}", paramObj);
        //}

        //public void GetAnaSpotTemp(int uNO)
        //{
        //    Debug.WriteLine("当前{0}编号的点温为:xxx",uNO);

        //}
        //#endregion
        //#region 线温相关
        //public void SetAnaLinePos(int uNO,double fStartXPer,double fStartYPer,double fEndXPer,double fEndYPer)
        //{
        //    Debug.WriteLine("线温位置参数设置:{0},{1},{2},{3},{4}", uNO, fStartXPer, fStartYPer, fEndXPer, fEndYPer);
        //}

        //public void GetAnaLinePos(int uNO)
        //{
        //    Debug.WriteLine("线温位置{0}参数获取", uNO);
        //}

        //public void SetAnaLineParam(IRAnaParamItem paramObj)
        //{
        //    Debug.WriteLine("线温设置相关参数为:{0}", paramObj);
        //}
        //public void GetAnaLineParam(int uNO)
        //{
        //    IRAnaParamItem paramObj = new IRAnaParamItem();
        //    paramObj.uNO = uNO;
        //    Debug.WriteLine("线温获取相关参数为:{0}", paramObj);
        //}

        //public void GetAnaLineTemp(int uNO)
        //{
        //    Debug.WriteLine("当前{0}编号的线温为:xxx", uNO);

        //}

        //#endregion
        //#region 矩形测温相关
        //public void SetAnaAreaPos(int uNO,double fStartXPer,double fStartYPer,double fToWidthPer,double fHeightPer)
        //{
        //    Debug.WriteLine("编号为{0}的矩形位置参数为:{1},{2},{3},{4}", uNO, fStartXPer, fStartYPer, fToWidthPer, fHeightPer);
        //}
        //public void GetAnaAreaPos(int uNO)
        //{
        //    Debug.WriteLine("编号为{0}的矩形位置参数为:");
        //}

        //public void SetAnaAreaParam(IRAnaParamItem paramObj)
        //{
        //    Debug.WriteLine("矩形测温设置相关参数为:{0}", paramObj);
        //}
        //public void GetAnaAreaParam(int uNO)
        //{
        //    IRAnaParamItem paramObj = new IRAnaParamItem();
        //    paramObj.uNO = uNO;
        //    Debug.WriteLine("矩形测温获取相关参数为:{0}", paramObj);
        //}

        //public void GetAnaAreaTemp(int uNO)
        //{
        //    Debug.WriteLine("当前{0}编号的矩形测温为:xxx", uNO);

        //}

        //#endregion
        //#region 多边形测温相关
        //public void SetAnaPolyPos(int uNO, Dictionary<string, int> dictionary)
        //{
        //    Debug.WriteLine("编号为{0}的多边形测温参数设置为:{1}", uNO, dictionary);
        //}
        //public void GetAnaPolyPos(int uNO)
        //{
        //    Debug.WriteLine("编号为{0}的多边形测温参数设置为:xxx", uNO);
        //}

        //public void SetAnaPolyParam(IRAnaParamItem paramObj)
        //{
        //    Debug.WriteLine("多边形测温设置相关参数为:{0}", paramObj);
        //}
        //public void GetAnaPolyParam(int uNO)
        //{
        //    IRAnaParamItem paramObj = new IRAnaParamItem();
        //    paramObj.uNO = uNO;
        //    Debug.WriteLine("编号为{0}多边形测温获取相关参数为:{1}",uNO, paramObj);   
        //}
        //public void GetAnaPolyTemp(int uNO)
        //{
        //    Debug.WriteLine("当前{0}编号的多边形测温温度为:xxx", uNO);

        //}
        //#endregion
        //#region 圆形测温
        //public void SetAnaCirclePos(int uNO, int fCenterLeftPer,int fCenterTopPer,int fRadiusWidthPer)
        //{
        //    Debug.WriteLine("编号为{0}的圆形测温位置参数为:{1},{2},{3}", uNO, fCenterLeftPer, fCenterTopPer, fRadiusWidthPer);
        //}

        //public void GetAnaCirclePos(int uNO)
        //{
        //    Debug.WriteLine("获取编号为{0}的圆形测温位置参数...", uNO);
        //}

        //public void SetAnaCircleParam(IRAnaParamItem paramObj)
        //{
        //    Debug.WriteLine("圆形测温设置相关参数为:{0}", paramObj);
        //}
        //public void GetAnaCircleParam(int uNO)
        //{
        //    IRAnaParamItem paramObj = new IRAnaParamItem();
        //    paramObj.uNO = uNO;
        //    Debug.WriteLine("圆形测温获取相关参数为:{0}", paramObj);
        //}

        //public void GetAnaCircleTemp(int uNO)
        //{
        //    Debug.WriteLine("当前{0}编号的圆形测温为:xxx", uNO);

        //}
      // #endregion
    }
}
