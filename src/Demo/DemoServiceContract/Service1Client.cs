namespace DemoServiceContract
{
    using WcfHelper;

    public class Service1Client : ConfigurationClientBase<IService1>, IService1
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
}
