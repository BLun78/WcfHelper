namespace WcfHelper.Mock
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.ServiceModel;
    using System.ServiceModel.Security;

    using JetBrains.Annotations;

    public sealed class MockErrorClientInvoker<TChannel, TClientBase> : IClientInvoker<TChannel, TClientBase>
            where TChannel : class
            where TClientBase : class, TChannel, ICommunicationObject, IDisposable, new()
    {
        #region Ctor

        [NotNull]
        private readonly Func<MockErrorClientInvoker<TChannel, TClientBase>, bool> mockExceptionMethod;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public MockErrorClientInvoker([NotNull] Func<MockErrorClientInvoker<TChannel, TClientBase>, bool> mockExceptionMethod)
        {
            this.mockExceptionMethod = mockExceptionMethod;
        }

        #endregion

        #region Events

        public event EventHandler Closed;
        public event EventHandler Closing;
        public event EventHandler Faulted;
        public event EventHandler Opened;
        public event EventHandler Opening;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseClosed([NotNull] string comment)
        {
            this.RaiseEventHandler(this.Closed, comment);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseClosing([NotNull] string comment)
        {
            this.RaiseEventHandler(this.Closing, comment);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseFaulted([NotNull] string comment)
        {
            this.RaiseEventHandler(this.Faulted, comment);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseOpened([NotNull] string comment)
        {
            this.RaiseEventHandler(this.Opened, comment);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseOpening([NotNull] string comment)
        {
            this.RaiseEventHandler(this.Opening, comment);
        }

        private void RaiseEventHandler([CanBeNull]EventHandler eventHandler,
                                       [NotNull] string comment)
        {
            if (eventHandler != null)
            {
                var mockEventArgs = this.MockEventArgs;
                if (mockEventArgs != null)
                {
                    mockEventArgs.Comment = comment;
                    eventHandler(this, mockEventArgs);
                }
            }
        }

        public MockEventArgs<TChannel, TClientBase> MockEventArgs { get; private set; }

        #endregion

        #region CreateMockEventsArgs

        private static MockEventArgs<TChannel, TClientBase>
            CreateMockEventsArgs([NotNull] string comment,
                                 [Optional, CanBeNull] Func<TClientBase, bool> invokeFunction,
                                 [Optional, CanBeNull] WindowsClientCredential windowsClientCredential,
                                 [Optional, CanBeNull] UserNamePasswordClientCredential userNamePasswordClientCredential,
                                 [Optional, CanBeNull] string endpointConfigurationName)
        {
            if (windowsClientCredential != null)
            {
                return new MockEventArgs<TChannel, TClientBase>(comment, invokeFunction, windowsClientCredential, endpointConfigurationName);
            }
            if (userNamePasswordClientCredential != null)
            {
                return new MockEventArgs<TChannel, TClientBase>(comment, invokeFunction, userNamePasswordClientCredential, endpointConfigurationName);
            }
            if (!string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                return new MockEventArgs<TChannel, TClientBase>(comment, invokeFunction, endpointConfigurationName);
            }
            return new MockEventArgs<TChannel, TClientBase>(comment, invokeFunction);
        }

        private static MockEventArgs<TChannel, TClientBase>
           CreateMockEventsArgs([NotNull] string comment,
                                [Optional, CanBeNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                [Optional, CanBeNull] WindowsClientCredential windowsClientCredential,
                                [Optional, CanBeNull] UserNamePasswordClientCredential userNamePasswordClientCredential,
                                [Optional, CanBeNull] string endpointConfigurationName)
        {
            if (windowsClientCredential != null)
            {
                return new MockEventArgs<TChannel, TClientBase>(comment, invokeFunctionAsync, windowsClientCredential, endpointConfigurationName);
            }
            if (userNamePasswordClientCredential != null)
            {
                return new MockEventArgs<TChannel, TClientBase>(comment, invokeFunctionAsync, userNamePasswordClientCredential, endpointConfigurationName);
            }
            if (!string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                return new MockEventArgs<TChannel, TClientBase>(comment, invokeFunctionAsync, endpointConfigurationName);
            }
            return new MockEventArgs<TChannel, TClientBase>(comment, invokeFunctionAsync);
        }

        #endregion

        #region Impl

        internal bool InvokeRequestImpl([NotNull] string comment,
                                        [CanBeNull] Func<TClientBase, bool> invokeFunction,
                                        [Optional, CanBeNull] string endpointConfigurationName,
                                        [Optional, CanBeNull] WindowsClientCredential windowsClientCredential,
                                        [Optional, CanBeNull] UserNamePasswordClientCredential userNamePasswordClientCredential)
        {
            this.MockEventArgs = CreateMockEventsArgs(comment,
                                                      invokeFunction,
                                                      windowsClientCredential,
                                                      userNamePasswordClientCredential,
                                                      endpointConfigurationName);
            return this.mockExceptionMethod(this);
        }
        internal async Task<bool> InvokeRequestImplAsync([NotNull] string comment,
                                                   [CanBeNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                                   [Optional, CanBeNull] string endpointConfigurationName,
                                                   [Optional, CanBeNull] WindowsClientCredential windowsClientCredential,
                                                   [Optional, CanBeNull] UserNamePasswordClientCredential userNamePasswordClientCredential)
        {
            this.MockEventArgs = CreateMockEventsArgs(comment,
                                                      invokeFunctionAsync,
                                                      windowsClientCredential,
                                                      userNamePasswordClientCredential,
                                                      endpointConfigurationName);
            return await Task.FromResult(this.mockExceptionMethod(this));
        }

        #endregion

        #region InvokeRequest

        public bool InvokeRequest([CanBeNull] Func<TClientBase, bool> invokeFunction)
        {
            return this.InvokeRequestImpl(@"bool InvokeRequest(Func<TClientBase, bool> invokeFunction)",
                                          invokeFunction);
        }

        public bool InvokeRequest([CanBeNull] Func<TClientBase, bool> invokeFunction,
                                  [CanBeNull] string endpointConfigurationName)
        {
            return this.InvokeRequestImpl(@"bool InvokeRequest(Func<TClientBase, bool> invokeFunction, string endpointConfigurationName)",
                                        invokeFunction,
                                        endpointConfigurationName);
        }

        public bool InvokeRequest([CanBeNull] Func<TClientBase, bool> invokeFunction,
                                  [CanBeNull] WindowsClientCredential windowsClientCredential,
                                  [CanBeNull] string endpointConfigurationName)
        {
            return this.InvokeRequestImpl(@"bool InvokeRequest(Func<TClientBase, bool> invokeFunction, WindowsClientCredential windowsClientCredential, string endpointConfigurationName)",
                                        invokeFunction,
                                        endpointConfigurationName,
                                        windowsClientCredential);
        }

        public bool InvokeRequest([CanBeNull] Func<TClientBase, bool> invokeFunction,
                                  [CanBeNull] UserNamePasswordClientCredential userNamePasswordClientCredential,
                                  [CanBeNull] string endpointConfigurationName)
        {
            return this.InvokeRequestImpl(@"bool InvokeRequest(Func<TClientBase, bool> invokeFunction, UserNamePasswordClientCredential userNamePasswordClientCredential, string endpointConfigurationName)",
                                        invokeFunction,
                                        endpointConfigurationName,
                                        userNamePasswordClientCredential: userNamePasswordClientCredential);
        }

        #endregion

        #region InvokeRequestAsync

        public async Task<bool> InvokeRequestAsync([CanBeNull] Func<TClientBase, Task<bool>> invokeFunctionAsync)
        {
            return await this.InvokeRequestImplAsync(@"Task<bool> InvokeRequestAsync(Func<TClientBase, Task<bool>> invokeFunctionAsync)",
                                                     invokeFunctionAsync);
        }

        public async Task<bool> InvokeRequestAsync([CanBeNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                                   [CanBeNull] string endpointConfigurationName)
        {
            return await this.InvokeRequestImplAsync(@"Task<bool> InvokeRequestAsync(Func<TClientBase, Task<bool>> invokeFunctionAsync, string endpointConfigurationName)",
                                                     invokeFunctionAsync,
                                                     endpointConfigurationName);
        }

        public async Task<bool> InvokeRequestAsync([CanBeNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                                   [CanBeNull] WindowsClientCredential windowsClientCredential,
                                                   [CanBeNull] string endpointConfigurationName)
        {
            return await this.InvokeRequestImplAsync(@"Task<bool> InvokeRequestAsync(Func<TClientBase, Task<bool>> invokeFunctionAsync, WindowsClientCredential windowsClientCredential, string endpointConfigurationName)",
                                                     invokeFunctionAsync,
                                                     endpointConfigurationName,
                                                     windowsClientCredential);
        }

        public async Task<bool> InvokeRequestAsync([CanBeNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                                                   [CanBeNull] UserNamePasswordClientCredential userNamePasswordClientCredential,
                                                   [CanBeNull] string endpointConfigurationName)
        {
            return await this.InvokeRequestImplAsync(@"Task<bool> InvokeRequestAsync(Func<TClientBase, Task<bool>> invokeFunctionAsync, UserNamePasswordClientCredential userNamePasswordClientCredential, string endpointConfigurationName)",
                                                     invokeFunctionAsync,
                                                     endpointConfigurationName,
                                                     userNamePasswordClientCredential: userNamePasswordClientCredential);
        }

        #endregion
    }
}
