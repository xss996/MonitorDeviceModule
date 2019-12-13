using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace PTiIRMonitor_MonitorDeviceModuleTests
{
    [TestClass]
    public class UnitTest1
    {
        class User
        {
           public  Result res { get; set; }
        }
        public enum Result
        {
            OK=1,ERROR=-1
        }
        [TestMethod]
        public void TestMethod1()
        {
            User user = new User();
            user.res = Result.OK;
            String str = JsonConvert.SerializeObject(user);
            Console.WriteLine(Result.OK);
        }
    }
}
