using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTiIRMonitor_MonitorDeviceModule.constant;
using PTiIRMonitor_MonitorDeviceModule.entities;
using PTiIRMonitor_MonitorDeviceModule.util;
using System.Diagnostics;

namespace PTiIRMonitor_MonitorDeviceModule.ctrl
{
    public class SysCtrl
    {
        public bool cruiseState = false;

        public string username;

        public void Init()
        {
            string username = INIUtil.Read("USER", "username", Constant.IniFilePath);
            this.username = username;
            Debug.WriteLine("用户登录信息初始化成功");
        }
        public bool StartCruise(int setUp)
        {
            if (setUp == 0)
            {
                Debug.WriteLine("启动巡检...");
                cruiseState = true;
            }
            else
            {
                Debug.WriteLine("停止巡检...");
                cruiseState = false;
            }
            return cruiseState;
        }



        public void ReStartMonDev(int No)
        {
            Debug.WriteLine("重新启动编号为" + No + "的监控头");
        }

        public string SendHeartbeatCmd()
        {
            Debug.WriteLine("发送心跳指令到客户端...");
            JsonItem jsonItem = new JsonItem();
            jsonItem.seq = DateUtil.DateToString();
            jsonItem.cmdType = 1;
            jsonItem.cmdAction = "Palpitate";
            jsonItem.sender = username;
            jsonItem.receiver = "CmdServer";
            return JsonConvert.SerializeObject(jsonItem);
        }

        public string UserLogin(string username, string password)
        {
            JObject jobCmd = new JObject();
            jobCmd.Add(new JProperty("seq", DateUtil.DateToString()));
            jobCmd.Add(new JProperty("cmdType", 1));
            jobCmd.Add(new JProperty("cmdAction", "Login"));
            jobCmd.Add(new JProperty("result", ""));
            jobCmd.Add(new JProperty("sender", username));
            jobCmd.Add(new JProperty("receiver", "CmdServer"));
            JArray jarr = new JArray();
            JObject jobbuf = new JObject();
            jobbuf.Add(new JProperty("username", username));
            jobbuf.Add(new JProperty("password", password));
            jarr.Add(jobbuf);
            jobCmd.Add(new JProperty("paramList", jarr));
            return JsonConvert.SerializeObject(jobCmd);
        }
    }
}
