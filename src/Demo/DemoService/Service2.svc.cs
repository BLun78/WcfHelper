namespace DemoService
{
    using System.Threading.Tasks;

    using DemoServiceContract;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service2" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service2.svc or Service2.svc.cs at the Solution Explorer and start debugging.
    public class Service2 : IService2
    {
        private readonly Service1 service1;
        public Service2()
        {
            this.service1 = new Service1();
        }

        public async Task<string> GetData2Async(int value)
        {
            return await Task.FromResult(this.service1.GetData(value));
        }

        public async Task<CompositeType> GetDataUsingDataContract2Async(CompositeType composite)
        {
            return await Task.FromResult(this.service1.GetDataUsingDataContract(composite));
        }
    }
}
