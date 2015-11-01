namespace WcfHelper.Mock
{
    using System;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.ServiceModel.Security;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    [DebuggerDisplay("Comment={comment}")]
    public sealed class MockEventArgs<TChannel, TClientBase> : EventArgs
        where TChannel : class
        where TClientBase : class, TChannel, ICommunicationObject, IDisposable, new()
    {
        #region Ctor Sync

        public MockEventArgs([NotNull] string comment,
                             [CanBeNull] Func<TClientBase, bool> invokeFunction) : base()
        {
            this.comment = comment;
            this.InvokeFunction = invokeFunction;
        }

        public MockEventArgs([NotNull] string comment,
                             [CanBeNull] Func<TClientBase, bool> invokeFunction,
                             [CanBeNull] string endpointConfigurationName)
            : this(comment, invokeFunction)
        {
            this.EndpointConfigurationName = endpointConfigurationName;

        }

        public MockEventArgs([NotNull] string comment,
                             [CanBeNull] Func<TClientBase, bool> invokeFunction,
                             [CanBeNull] WindowsClientCredential windowsClientCredential,
                             [CanBeNull] string endpointConfigurationName)
           : this(comment, invokeFunction, endpointConfigurationName)
        {
            this.WindowsClientCredential = windowsClientCredential;
        }

        public MockEventArgs([NotNull] string comment,
                             [CanBeNull] Func<TClientBase, bool> invokeFunction,
                             [CanBeNull] UserNamePasswordClientCredential userNamePasswordClientCredential,
                             [CanBeNull] string endpointConfigurationName)
            : this(comment, invokeFunction, endpointConfigurationName)
        {
            this.UserNamePasswordClientCredential = userNamePasswordClientCredential;
        }

        #endregion

        #region Ctor Async

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public MockEventArgs([NotNull] string comment,
                             [CanBeNull] Func<TClientBase, Task<bool>> invokeFunctionAsync) : base()
        {
            this.comment = comment;
            this.InvokeFunctionAsync = invokeFunctionAsync;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public MockEventArgs([NotNull] string comment,
                             [CanBeNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                             [CanBeNull] string endpointConfigurationName)
            : this(comment, invokeFunctionAsync)
        {
            this.EndpointConfigurationName = endpointConfigurationName;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public MockEventArgs([NotNull] string comment,
                             [CanBeNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                             [CanBeNull] WindowsClientCredential windowsClientCredential,
                             [CanBeNull] string endpointConfigurationName)
           : this(comment, invokeFunctionAsync, endpointConfigurationName)
        {
            this.WindowsClientCredential = windowsClientCredential;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public MockEventArgs([NotNull] string comment,
                             [CanBeNull] Func<TClientBase, Task<bool>> invokeFunctionAsync,
                             [CanBeNull] UserNamePasswordClientCredential userNamePasswordClientCredential,
                             [CanBeNull] string endpointConfigurationName)
            : this(comment, invokeFunctionAsync, endpointConfigurationName)
        {
            this.UserNamePasswordClientCredential = userNamePasswordClientCredential;
        }

        #endregion

        #region Prop

        [CanBeNull]
        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                this.comment = this.comment + DoublePipe + value;
            }
        }
        private string comment;
        private const string DoublePipe = @" || ";

        #endregion

        #region Auto Prop

        [CanBeNull]
        public Func<TClientBase, bool> InvokeFunction { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        [CanBeNull]
        public Func<TClientBase, Task<bool>> InvokeFunctionAsync { get; private set; }

        [CanBeNull]
        public WindowsClientCredential WindowsClientCredential { get; private set; }

        [CanBeNull]
        public UserNamePasswordClientCredential UserNamePasswordClientCredential { get; private set; }

        [CanBeNull]
        public string EndpointConfigurationName { get; private set; }

        #endregion
    }
}
