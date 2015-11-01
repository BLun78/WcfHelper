namespace WcfHelper
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Security;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    /// the contract for request Service Operations
    /// </summary>
    /// <typeparam name="TChannel">TChannel provides ContractDescription</typeparam>
    /// <typeparam name="TClientBase">represent a 'WcfHelper.ConfigurationClientBase<TChannel>' or 'System.ServiceModel.ClientBase<TChannel>()'</typeparam>
    public interface IClientInvoker<TChannel, out TClientBase> : IClientInvokerCommunication
            where TChannel : class
            where TClientBase : class, TChannel, ICommunicationObject, IDisposable, new()
    {
        /// <summary>
        /// request with an lambda expression one/more service operation
        /// </summary>
        /// <param name="invokeFunction">the delegation for function wich would call with parameter(TClientBase) and result bool </param>
        /// <returns></returns>
        bool InvokeRequest([NotNull] Func<TClientBase, bool> invokeFunction);

        bool InvokeRequest([NotNull] Func<TClientBase, bool> invokeFunction,
                           [NotNull] string endpointConfigurationName);

        bool InvokeRequest([NotNull] Func<TClientBase, bool> invokeFunction,
                           [NotNull] WindowsClientCredential windowsClientCredential,
                           [NotNull] string endpointConfigurationName);

        bool InvokeRequest([NotNull] Func<TClientBase, bool> invokeFunction,
                           [NotNull] UserNamePasswordClientCredential userNamePasswordClientCredential,
                           [NotNull] string endpointConfigurationName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "For async/await pattern needed.")]
        Task<bool> InvokeRequestAsync([NotNull] Func<TClientBase, Task<bool>> invokeFunctionAsync);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "For async/await pattern needed.")]
        Task<bool> InvokeRequestAsync([NotNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                      [NotNull] string endpointConfigurationName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "For async/await pattern needed.")]
        Task<bool> InvokeRequestAsync([NotNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                      [NotNull] WindowsClientCredential windowsClientCredential,
                                      [NotNull] string endpointConfigurationName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "For async/await pattern needed.")]
        Task<bool> InvokeRequestAsync([NotNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                      [NotNull] UserNamePasswordClientCredential userNamePasswordClientCredential,
                                      [NotNull] string endpointConfigurationName);
    }
}