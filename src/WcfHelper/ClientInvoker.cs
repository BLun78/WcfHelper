using System;
using System.Threading.Tasks;

namespace WcfHelper
{
    using System.Runtime.InteropServices;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.ServiceModel.Security;

    using JetBrains.Annotations;

    using WcfHelper.Extensions;
    using WcfHelper.Rescource;

    public class ClientInvoker<TChannel, TClientBase> : IClientInvoker<TChannel, TClientBase>
            where TChannel : class
            where TClientBase : class, TChannel, ICommunicationObject, IDisposable, new()
    {
        #region Private

        internal static ClientCredentials GetClientCredentials([NotNull]TClientBase clientBase)
        {
            var clientCredentialsPropertyInfo = typeof(TClientBase).GetProperty(@"ClientCredentials");
            return (ClientCredentials)clientCredentialsPropertyInfo?.GetValue(clientBase);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public const string DefaultEndpointConfigurationName = @"DefaultEndpointConfigurationName";

        internal static TClientBase InstantiateClientBase([Optional, CanBeNull]string endpointConfigurationName)
        {
            if (!string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                return (TClientBase)Activator.CreateInstance(typeof(TClientBase), endpointConfigurationName);
            }

            var field = typeof(TClientBase).GetField(DefaultEndpointConfigurationName);
            if (field != null)
            {
                return (TClientBase)Activator.CreateInstance(typeof(TClientBase), field.GetValue(null));
            }

            var property = typeof(TClientBase).GetProperty(DefaultEndpointConfigurationName);
            if (property != null)
            {
                return (TClientBase)Activator.CreateInstance(typeof(TClientBase), property.GetValue(null, null));
            }

            var baseType = typeof(TClientBase).BaseType;
            if (baseType != null)
            {
                var baseTypeProperty = baseType.GetProperty(DefaultEndpointConfigurationName);
                var baseTypeValue = baseTypeProperty?.GetValue(null, null);
                if (baseTypeValue != null)
                {
                    return (TClientBase)Activator.CreateInstance(typeof(TClientBase), baseTypeValue);
                }
            }

            return (TClientBase)Activator.CreateInstance(typeof(TClientBase));
        }

        internal void FinallyClientBase([NotNull] TClientBase clientBase)
        {
            try
            {
                // Faulted do abort
                if (clientBase.State == CommunicationState.Faulted)
                {
                    clientBase.Abort();
                }
                else
                {
                    //normaly close
                    clientBase.Close();
                }
            }
            // if Exceptions abort cientbase
            catch (TimeoutException)
            {
                clientBase.Abort();
            }
            catch (CommunicationObjectFaultedException)
            {
                clientBase.Abort();
            }
            this.UnRegisterEvents(clientBase);
        }

        internal bool InvokeRequestImpl([NotNull] Func<TClientBase, bool> invokeFunction,
                                        [Optional, CanBeNull] string endpointConfigurationName,
                                        [Optional, CanBeNull] WindowsClientCredential windowsClientCredential,
                                        [Optional, CanBeNull] UserNamePasswordClientCredential userNamePasswordClientCredential)
        {
            if (invokeFunction == null) throw new ArgumentNullException(nameof(invokeFunction));

            var clientBase = default(TClientBase);

            try
            {
                // instatiate wcf-client
                clientBase = InstantiateClientBase(endpointConfigurationName);
                this.RegisterEvents(clientBase);

                //Set ClientCredentials if they set
                SetClientCredentials(clientBase, windowsClientCredential, userNamePasswordClientCredential);

                // invoke request
                return invokeFunction(clientBase);
            }
            finally
            {
                // channel close
                if (clientBase != default(TClientBase))
                {
                    this.FinallyClientBase(clientBase);
                }
            }
        }

        internal async Task<bool> InvokeRequestImplAsync([NotNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                                         [Optional, CanBeNull] string endpointConfigurationName,
                                                         [Optional, CanBeNull] WindowsClientCredential windowsClientCredential,
                                                         [Optional, CanBeNull] UserNamePasswordClientCredential userNamePasswordClientCredential)
        {
            if (invokeFunctionAsync == null) throw new ArgumentNullException(nameof(invokeFunctionAsync));

            var clientBase = default(TClientBase);

            try
            {
                // instatiate wcf-client
                clientBase = InstantiateClientBase(endpointConfigurationName);
                this.RegisterEvents(clientBase);

                // Set ClientCredentials if they set
                SetClientCredentials(clientBase, windowsClientCredential, userNamePasswordClientCredential);

                // invoke request
                return await invokeFunctionAsync(clientBase);
            }
            finally
            {
                // channel close
                if (clientBase != default(TClientBase))
                {
                    this.FinallyClientBase(clientBase);
                }
            }

        }

        internal static void SetClientCredentials([NotNull] TClientBase clientBase,
                                                  [CanBeNull] WindowsClientCredential windowsClientCredential,
                                                  [CanBeNull] UserNamePasswordClientCredential userNamePasswordClientCredential)
        {
            var clientCredential = GetClientCredentials(clientBase);
            if (windowsClientCredential != null)
            {
                CheckForNullClientCredential(clientCredential);
                clientCredential.WindowsClientCredentialMappingToClientCredentials(windowsClientCredential);
            }
            if (userNamePasswordClientCredential != null)
            {
                CheckForNullClientCredential(clientCredential);
                clientCredential.UserNamePasswordClientCredentialMappingToClientCredentials(userNamePasswordClientCredential);
            }
        }

        internal static void CheckForNullClientCredential([CanBeNull] ClientCredentials clientCredential)
        {
            if (clientCredential == null)
            {
                throw new InvalidOperationException(WcfHelperText.ClientCredentialsAreNull);
            }
        }

        #endregion

        #region ClientBase

        public bool InvokeRequest([NotNull] Func<TClientBase, bool> invokeFunction)
        {
            return this.InvokeRequestImpl(invokeFunction);
        }

        public bool InvokeRequest([NotNull] Func<TClientBase, bool> invokeFunction,
                                  [NotNull]  string endpointConfigurationName)
        {
            if (string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                throw new ArgumentNullException(nameof(endpointConfigurationName));
            }
            return this.InvokeRequestImpl(invokeFunction, endpointConfigurationName);
        }

        public bool InvokeRequest([NotNull] Func<TClientBase, bool> invokeFunction,
                                  [NotNull] WindowsClientCredential windowsClientCredential,
                                  [NotNull] string endpointConfigurationName)
        {
            if (windowsClientCredential == null)
            {
                throw new ArgumentNullException(nameof(windowsClientCredential));
            }
            if (string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                throw new ArgumentNullException(nameof(endpointConfigurationName));
            }
            return this.InvokeRequestImpl(invokeFunction, endpointConfigurationName, windowsClientCredential);
        }

        public bool InvokeRequest([NotNull] Func<TClientBase, bool> invokeFunction,
                                  [NotNull] UserNamePasswordClientCredential userNamePasswordClientCredential,
                                  [NotNull] string endpointConfigurationName)
        {
            if (userNamePasswordClientCredential == null)
            {
                throw new ArgumentNullException(nameof(userNamePasswordClientCredential));
            }
            if (string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                throw new ArgumentNullException(nameof(endpointConfigurationName));
            }
            return this.InvokeRequestImpl(invokeFunction, endpointConfigurationName, userNamePasswordClientCredential: userNamePasswordClientCredential);
        }

        public async Task<bool> InvokeRequestAsync([NotNull] Func<TClientBase, Task<bool>> invokeFunctionAsync)
        {
            return await this.InvokeRequestImplAsync(invokeFunctionAsync);
        }

        public async Task<bool> InvokeRequestAsync([NotNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                                   [NotNull] string endpointConfigurationName)
        {
            if (string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                throw new ArgumentNullException(nameof(endpointConfigurationName));
            }
            return await this.InvokeRequestImplAsync(invokeFunctionAsync, endpointConfigurationName);
        }

        public async Task<bool> InvokeRequestAsync([NotNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                                   [NotNull] WindowsClientCredential windowsClientCredential,
                                                   [NotNull] string endpointConfigurationName)
        {
            if (windowsClientCredential == null)
            {
                throw new ArgumentNullException(nameof(windowsClientCredential));
            }
            if (string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                throw new ArgumentNullException(nameof(endpointConfigurationName));
            }
            return await this.InvokeRequestImplAsync(invokeFunctionAsync, endpointConfigurationName, windowsClientCredential);
        }

        public async Task<bool> InvokeRequestAsync([NotNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                                   [NotNull] UserNamePasswordClientCredential userNamePasswordClientCredential,
                                                   [NotNull] string endpointConfigurationName)
        {
            if (userNamePasswordClientCredential == null)
            {
                throw new ArgumentNullException(nameof(userNamePasswordClientCredential));
            }
            if (string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                throw new ArgumentNullException(nameof(endpointConfigurationName));
            }
            return await this.InvokeRequestImplAsync(invokeFunctionAsync, endpointConfigurationName, userNamePasswordClientCredential: userNamePasswordClientCredential);
        }

        #endregion

        #region Events

        private void RegisterEvents([NotNull] TClientBase clientBase)
        {
            clientBase.Closed += this.Closed;
            clientBase.Closing += this.Closing;
            clientBase.Faulted += this.Faulted;
            clientBase.Opened += this.Opened;
            clientBase.Opening += this.Opening;
        }

        private void UnRegisterEvents([NotNull] TClientBase clientBase)
        {
            clientBase.Closed -= this.Closed;
            clientBase.Closing -= this.Closing;
            clientBase.Faulted -= this.Faulted;
            clientBase.Opened -= this.Opened;
            clientBase.Opening -= this.Opening;
        }

        public event EventHandler Closed;
        public event EventHandler Closing;
        public event EventHandler Faulted;
        public event EventHandler Opened;
        public event EventHandler Opening;

        #endregion
    }
}
