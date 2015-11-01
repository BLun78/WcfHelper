namespace WcfHelper
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;

    using JetBrains.Annotations;

    using WcfHelper.ServiceModel;

    public abstract class ConfigurationClientBase<TChannel> :
            ICommunicationObject,
            IDisposable
        where TChannel : class
    {

        [NotNull]
        public TChannel Channel
        {
            get
            {
                if (this.channel == null)
                {
                    lock (this.syncLock)
                    {
                        if (this.channel == null)
                        {
                            this.channel = this.channelFactory.CreateChannel();
                        }
                    }
                }
                return this.channel;
            }
        }
        private TChannel channel;
        private volatile object syncLock = new object();

        [NotNull]
        public ClientCredentials ClientCredentials => this.channelFactory.ClientCredentials;

        private readonly HelperChannelFactory<TChannel> channelFactory;

        #region Ctor

        protected ConfigurationClientBase()
        {
            this.channelFactory = new HelperChannelFactory<TChannel>("*");
        }

        protected ConfigurationClientBase([CanBeNull] string endpointConfigurationName)
        {
            this.channelFactory = new HelperChannelFactory<TChannel>(endpointConfigurationName);
        }

        protected ConfigurationClientBase([NotNull]Type assemblyType)
        {
            this.channelFactory = new HelperChannelFactory<TChannel>("*", assemblyType);
        }

        protected ConfigurationClientBase([CanBeNull] string endpointConfigurationName, [NotNull]Type assemblyType)
        {
            this.channelFactory = new HelperChannelFactory<TChannel>(endpointConfigurationName, assemblyType);
        }

        #endregion

        #region ICommunicationObject implicit
        
        [NotNull]
        public CommunicationState State => this.channelFactory.State;

        public void Open()
        {
            this.channelFactory.Open();
        }
        
        public void Abort()
        {
            this.channelFactory.Abort();
        }

        public void Close()
        {
            this.channelFactory.Close();
        }

        #endregion

        #region ICommunicationObject explicit

        void ICommunicationObject.Close(TimeSpan timeout)
        {
            this.channelFactory.Close(timeout);
        }

        IAsyncResult ICommunicationObject.BeginClose(AsyncCallback callback, object state)
        {
            return this.channelFactory.BeginClose(callback, state);
        }

        IAsyncResult ICommunicationObject.BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return this.channelFactory.BeginClose(timeout, callback, state);
        }

        void ICommunicationObject.EndClose(IAsyncResult result)
        {
            this.channelFactory.EndClose(result);
        }

        void ICommunicationObject.Open(TimeSpan timeout)
        {
            this.channelFactory.Open(timeout);
        }

        IAsyncResult ICommunicationObject.BeginOpen(AsyncCallback callback, object state)
        {
            return this.channelFactory.BeginOpen(callback, state);
        }

        IAsyncResult ICommunicationObject.BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return this.channelFactory.BeginOpen(timeout, callback, state);
        }

        void ICommunicationObject.EndOpen(IAsyncResult result)
        {
            this.channelFactory.EndOpen(result);
        }
        
        event EventHandler ICommunicationObject.Closed
        {
            add
            {
                this.channelFactory.Closed += value;
            }
            remove
            {
                this.channelFactory.Closed -= value;
            }
        }

        event EventHandler ICommunicationObject.Closing
        {
            add
            {
                this.channelFactory.Closing += value;
            }
            remove
            {
                this.channelFactory.Closing -= value;
            }
        }

        event EventHandler ICommunicationObject.Faulted
        {
            add
            {
                this.channelFactory.Faulted += value;
            }
            remove
            {
                this.channelFactory.Faulted -= value;
            }
        }

        event EventHandler ICommunicationObject.Opened
        {
            add
            {
                this.channelFactory.Opened += value;
            }
            remove
            {
                this.channelFactory.Opened -= value;
            }
        }

        event EventHandler ICommunicationObject.Opening
        {
            add
            {
                this.channelFactory.Opening += value;
            }
            remove
            {
                this.channelFactory.Opening -= value;
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ConfigurationClientBase()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                this.Close();
            }

        }

        #endregion
    }
}