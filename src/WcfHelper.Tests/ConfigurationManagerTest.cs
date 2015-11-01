namespace WcfHelper.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WcfHelper.Mock;

    [TestClass]
    [DeploymentItem("WcfHelper.Mock.dll.config")]
    [DeploymentItem("WcfHelper.Tests.dll.config")]
    public class ConfigurationManagerTest
    {
        [TestMethod]
        public void Test1_System_Configuration_ConfigurationManager_Ok()
        {
            //arrange
            var key = @"Test1";
            var expected = @"Test1_Right";

            //act
            var result = System.Configuration.ConfigurationManager.AppSettings[key];

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test2_WcfHelper_ConfigurationManager_Ok()
        {
            //arrange
            var key = @"Test2";
            var expected = @"Test2_Right";
            var result = string.Empty;

            //act
            var config = new WcfHelper.ConfigurationManager(typeof(OperationContextMock));
            if (config.Configuration.HasFile)
            {
                result = config.AppSettings.Settings[key].Value;
            }

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Test3_WcfHelper_ConfigurationManager_Key_Fails()
        {
            //arrange
            var key = @"Test3";
            //var expected = @"Test3_Faild";
            var result = string.Empty;

            //act
            var config = new WcfHelper.ConfigurationManager(typeof(OperationContextMock));
            if (config.Configuration.HasFile)
            {
                try
                {
                    result = config.AppSettings.Settings[key].Value;
                }
                catch (NullReferenceException)
                {
                    return;
                }

            }

            //assert
            Assert.Fail();
        }
    }
}
