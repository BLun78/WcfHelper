namespace DemoServiceContract
{
    using System.Threading.Tasks;

    using WcfHelper;

    public class Service2Client : ConfigurationClientBase<IService2>, IService2
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
}