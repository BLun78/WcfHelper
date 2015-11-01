using System;
using System.Threading.Tasks;

namespace ServiceClient
{
    using System.Diagnostics;

    using DemoServiceContract;

    using ServiceClient.Client;

    using WcfHelper;

    public class Program
    {
        /// <summary>
        /// starts the programm
        /// </summary>
        /// <param name="args">not used</param>
        public static void Main(string[] args)
        {
            // wait for starting servcice
            Task.Delay(5000);

            WriteLine(Demo1());

            WriteLine(Demo2());

            WriteLine(Demo3());

            WriteLine(Demo4());

            Console.ReadLine();
        }

        /// <summary>
        /// sync call Demo1
        /// </summary>
        /// <returns>result</returns>
        private static string Demo1()
        {
            //Init Client
            var client1 = new ClientInvoker<IService1, Service1Client>(); // Config: GlobalAssemblyConfig -> DemoServiceClient.exe.config        

            //invoke Request 
            var data = string.Empty;
            var result = client1.InvokeRequest(
                (clientBase) =>
                {
                    data = clientBase.GetData(1);

                    return !string.IsNullOrWhiteSpace(data);
                });

            return "Demo1() => " + nameof(result) + " -> " + result + " | " + nameof(data) + " -> " + data;
        }

        /// <summary>
        /// async call Demo2
        /// </summary>
        /// <returns>result</returns>
        private static string Demo2()
        {
            //Init Client
            var client2 = new ClientInvoker<IService2, Service2Client>(); // Config: AssemblyConfig -> DemoServiceContract.dll.config  

            //invoke Async Request
            var dataAsync = string.Empty;
            var resultAsync = client2.InvokeRequestAsync(
                async (clientBase) =>
                {
                    dataAsync = await clientBase.GetData2Async(2);

                    return !string.IsNullOrWhiteSpace(dataAsync);
                });
            resultAsync.Wait();

            return "Demo2() => " + nameof(resultAsync) + " -> " + resultAsync.Result + " | " + nameof(dataAsync) + " -> " + dataAsync;
        }

        /// <summary>
        /// sync call Demo3
        /// </summary>
        /// <returns>result</returns>
        private static string Demo3()
        {
            //Init Client with an abstraction of ClientBaseInvoker<TChannel, TClientBase> 
            var client3 = new Service1ClientInvoker(); // Config: GlobalAssemblyConfig -> DemoServiceClient.exe.config        

            //invoke Request 
            var data = string.Empty;
            var result = client3.InvokeRequest(
                (clientBase) =>
                {
                    data = clientBase.GetData(3);

                    return !string.IsNullOrWhiteSpace(data);
                });

            return "Demo3() => " + nameof(result) + " -> " + result + " | " + nameof(data) + " -> " + data;
        }

        /// <summary>
        /// async call Demo2
        /// </summary>
        /// <returns>result</returns>
        private static string Demo4()
        {
            //Init Client
            var client4 = new ClientInvoker<ServiceClient.ServiceReferenceDemo4.IService1, Service1ClientDemo4>(); // Config: GlobalAssemblyConfig -> DemoServiceClient.exe.config    over static/const field/property called 'DefaultEndpointConfigurationName'

            //invoke Async Request
            var dataAsync = string.Empty;
            var resultAsync = client4.InvokeRequestAsync(
                async (clientBase) =>
                {
                    dataAsync = await clientBase.GetDataAsync(4);

                    return !string.IsNullOrWhiteSpace(dataAsync);
                });
            resultAsync.Wait();

            return "Demo4() => " + nameof(resultAsync) + " -> " + resultAsync.Result + " | " + nameof(dataAsync) + " -> " + dataAsync;
        }

        /// <summary>
        /// write line in debug.trace and console
        /// </summary>
        /// <param name="line">text for write line</param>
        private static void WriteLine(string line)
        {
            Console.WriteLine(line);
            Debug.WriteLine(line);
        }
    }
}
