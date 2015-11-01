namespace WcfHelper.Tests
{
    using System;
    using System.ServiceModel;
    using System.Threading.Tasks;

    using DemoServiceContract;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("WcfHelper.Tests.dll.config")]
    [DeploymentItem("DemoServiceContract.dll.config")]
    public class WcfTest
    {
        private class TestClient1 : ClientBase<IService1>, IService1
        {
            public string GetData(int value)
            {
                return this.Channel.GetData(value);
            }

            public CompositeType GetDataUsingDataContract(CompositeType composite)
            {
                return this.Channel.GetDataUsingDataContract(composite);
            }
        }

        private class TestClient2 : ClientBase<IService2>, IService2
        {
            public async Task<string> GetData2Async(int value)
            {
                return await this.Channel.GetData2Async(value);
            }

            public async Task<CompositeType> GetDataUsingDataContract2Async(CompositeType composite)
            {
                return await this.Channel.GetDataUsingDataContract2Async(composite);
            }
        }

        [TestMethod]
        public void Test1_Load_Wcf_TestClient1_Config_Ok()
        {
            //arrange

            //act
            try
            {
                using (var client = new TestClient1())
                {
                    return;
                }
            }
            catch (Exception)
            {
                //assert
                Assert.Fail();
            }
        }


        [TestMethod]
        public void Test2_Load_Wcf_TestClient2_Config_Failed()
        {
            //arrange

            //act
            try
            {
                var client = new TestClient2();
            }
            catch (InvalidOperationException)
            {
                return;
            }
            //assert
            Assert.Fail();
        }

        [TestMethod]
        public void Test3_Load_Service1Client_Config_Ok()
        {
            //arrange

            //act
            try
            {
                using (var client = new Service1Client())
                {
                    return;
                }
            }
            catch (Exception)
            {
                //assert
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Test4_Load_Service2Client_Config_Ok()
        {
            //arrange

            //act
            try
            {
                using (var client = new Service2Client())
                {
                    return;
                }
            }
            catch (Exception)
            {
                //assert
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Test5_Load_InstantiateClientBase_Config_Ok()
        {
            //arrange

            //act
            try
            {
                var client = ClientInvoker<IService1, Service1Client>.InstantiateClientBase();

                return;
            }
            catch (Exception)
            {
                //assert
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Test6_Load_InstantiateClientBase_Config_Ok()
        {
            //arrange

            //act
            try
            {
                var client = ClientInvoker<IService2, Service2Client>.InstantiateClientBase();

                return;
            }
            catch (Exception)
            {
                //assert
                Assert.Fail();
            }
        }


    }
}
