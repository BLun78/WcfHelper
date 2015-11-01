namespace WcfHelper.Mock
{
    using System.ServiceModel.Web;
    using System.ServiceModel.Web.Fakes;

    using Microsoft.QualityTools.Testing.Fakes.Instances;

    public sealed class WebOperationContextMock : ContextMockExtensionInternal
    {
        protected override void InitializeMock()
        {
            ShimWebOperationContext.BehaveAsCurrent();
            var operationContext = new ShimWebOperationContext();
            ShimWebOperationContext.CurrentGet = () => (operationContext as IInstanced<WebOperationContext>).Instance;
        }
    }
}
