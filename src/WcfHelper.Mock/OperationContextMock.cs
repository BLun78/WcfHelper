
namespace WcfHelper.Mock
{
    using System.ServiceModel;
    using System.ServiceModel.Fakes;

    using Microsoft.QualityTools.Testing.Fakes.Instances;

    public sealed class OperationContextMock : ContextMockExtensionInternal
    {
        protected override void InitializeMock()
        {
            ShimOperationContext.BehaveAsCurrent();
            var operationContext = new ShimOperationContext();
            ShimOperationContext.CurrentGet = () => (operationContext as IInstanced<OperationContext>).Instance;
        }
    }
}
