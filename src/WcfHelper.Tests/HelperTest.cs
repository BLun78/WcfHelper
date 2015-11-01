namespace WcfHelper.Tests
{
    using System;
    using System.Security.Principal;
    using System.ServiceModel.Description;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WcfHelper.Extensions;

    [TestClass]
    public class HelperTest
    {
        [TestMethod]
        public void Test1_WindowsClientCredentialMappingToClientCredentials_Ok()
        {
            //arrange
            var clientCredentials = new ClientCredentials();
            var windows = new ClientCredentials().Windows;
            var userName = "TestUsername";
            var domain = "TestDomain";
            var password = "TestPassword";
            var level = TokenImpersonationLevel.Identification;
            windows.AllowedImpersonationLevel = level;
            windows.ClientCredential.Password = password;
            windows.ClientCredential.Domain = domain;
            windows.ClientCredential.UserName = userName;

            //act
            clientCredentials.WindowsClientCredentialMappingToClientCredentials(windows);

            //assert
            var result = clientCredentials.Windows;
            Assert.AreEqual(level, result.AllowedImpersonationLevel);
            Assert.AreEqual(password, result.ClientCredential.Password);
            Assert.AreEqual(domain, result.ClientCredential.Domain);
            Assert.AreEqual(userName, result.ClientCredential.UserName);
        }

        [TestMethod]
        public void Test2_UserNamePasswordClientCredentialMappingToClientCredentials_Ok()
        {
            //arrange
            var clientCredentials = new ClientCredentials();
            var windows = new ClientCredentials().UserName;
            var userName = "TestUsername";
            var password = "TestPassword";
            windows.Password = password;
            windows.UserName = userName;

            //act
            clientCredentials.UserNamePasswordClientCredentialMappingToClientCredentials(windows);

            //assert
            var result = clientCredentials.UserName;
            Assert.AreEqual(password, result.Password);
            Assert.AreEqual(userName, result.UserName);
        }

        [TestMethod]
        public void Test3_NullObject_ArgumenttNullException()
        {
            //arrange
            var nullObject = default(object);

            //act
            try
            {
                nullObject.CheckArgumentForNull(nameof(nullObject));
            }
            catch (Exception)
            {
                return;
            }
            //assert
            Assert.Fail();
        }

        [TestMethod]
        public void Test4_Not_NullObject_ArgumenttNullException()
        {
            //arrange
            var nullObject = new object();

            //act
            try
            {
                nullObject.CheckArgumentForNull(nameof(nullObject));
                return;
            }
            catch (ArgumentNullException)
            {
                //assert
                Assert.Fail();
            }
            Assert.Fail();
        }

        [TestMethod]
        public void Test5_GetPathOfObjectAssembly_Contains_Windows()
        {
            //arrange
            var type = typeof(object);

            //act
            var path = type.GetAssemblyPath();

            //assert
            Assert.IsTrue(path.Contains(@"Windows\Microsoft.NET\Framework\"));
            Assert.IsTrue(path.Contains(@"\mscorlib.dll"));

        }
    }
}
