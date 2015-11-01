

namespace ServiceClient.Client
{
    using IService1 = ServiceReferenceDemo4.IService1;
    using DemoServiceContract;

    using System.Threading.Tasks;

    using JetBrains.Annotations;
    using WcfHelper;

    /// <summary>
    /// Example 4 - using a wcf service referenc generatet Interface
    /// </summary>
    public class Service1ClientDemo4 : ConfigurationClientBase<IService1>, IService1
    {
        public Service1ClientDemo4() : base()
        {
        }

        public Service1ClientDemo4([CanBeNull] string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Default endpoint configuration name, used for finding the default EndpointName per Reflection
        /// it can be a static/const field, static Property or static Basetype.Property (only one inheritance)
        /// </summary>
        public const string DefaultEndpointConfigurationName = @"BasicHttpBinding_IService1_ServiceReference";

        public string GetData(int value)
        {
            return this.Channel.GetData(value);
        }

        public async Task<string> GetDataAsync(int value)
        {
            return await this.Channel.GetDataAsync(value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            return this.Channel.GetDataUsingDataContract(composite);
        }

        public async Task<CompositeType> GetDataUsingDataContractAsync(CompositeType composite)
        {
            return await this.Channel.GetDataUsingDataContractAsync(composite);
        }
    }
}
