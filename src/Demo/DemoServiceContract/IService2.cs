namespace DemoServiceContract
{
    using System.ServiceModel;
    using System.Threading.Tasks;

    [ServiceContract]
    public interface IService2
    {
        [OperationContract]
        Task<string> GetData2Async(int value);

        [OperationContract]
        Task<CompositeType> GetDataUsingDataContract2Async(CompositeType composite);
    }
}