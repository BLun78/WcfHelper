using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WcfHelper.Tests
{
    using System.Diagnostics;
    using System.Linq;

    [TestClass]
    public class MockErrorClientInvokerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var version = "1.1.1.1";
            var arr = version.Split('.');
            var build = (int)(DateTime.UtcNow - new DateTime(2015, 10, 22)).TotalDays;
            var revision = (int)(DateTime.UtcNow - DateTime.UtcNow.Date).TotalMinutes;
            arr[2] = build.ToString();
            arr[3] = revision.ToString();
            var result = string.Empty;
            foreach (var str in arr)
            {
                if (result.Length == 0)
                {
                    result = str;
                }
                else
                {
                    result += '.' + str;
                }
            }
            
            Trace.WriteLine(result);
        }
    }
}
