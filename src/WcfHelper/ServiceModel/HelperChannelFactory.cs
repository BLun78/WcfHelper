namespace WcfHelper.ServiceModel
{
    using System;
    using System.Runtime.InteropServices;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Configuration;
    using System.ServiceModel.Description;

    using JetBrains.Annotations;

    using WcfHelper.Extensions;
    using WcfHelper.Rescource;

    using Configuration = System.Configuration.Configuration;

    internal sealed class HelperChannelFactory<TChannel> :
            IDisposable,
            IChannelFactory<TChannel>,
            IChannelFactory,
            ICommunicationObject
    {
        #region Field, Property 

        [NotNull]
        private readonly IChannelFactory<TChannel> configurationChannelFactory;

        [NotNull]
        // ReSharper disable once AssignNullToNotNullAttribute this never would be null after ctor
        public ClientCredentials ClientCredentials => ((ChannelFactory)this.configurationChannelFactory)?.Credentials;

        #endregion

        #region CTOR

        public HelperChannelFactory([CanBeNull] string endpointConfigurationName)
            : this(endpointConfigurationName, (EndpointAddress)null, (Configuration)null, HelperChannelFactory<TChannel>.GetAssemblyPath(typeof(TChannel)))
        {
        }
        public HelperChannelFactory(string endpointConfigurationName, Type assemblyType)
            : this(endpointConfigurationName, (EndpointAddress)null, (Configuration)null, HelperChannelFactory<TChannel>.GetAssemblyPath(assemblyType))
        {
        }

        #region NotSupported Ctor

        public HelperChannelFactory(string endpointConfigurationName, string exePath)
            : this(endpointConfigurationName, (EndpointAddress)null, (Configuration)null, exePath)
        {
        }

        public HelperChannelFactory(string endpointConfigurationName, Configuration configuration)
           : this(endpointConfigurationName, (EndpointAddress)null, configuration, (string)null)
        {
        }

        public HelperChannelFactory(string endpointConfigurationName, EndpointAddress remoteAddress, Type assemblyType)
            : this(endpointConfigurationName, remoteAddress, (Configuration)null, HelperChannelFactory<TChannel>.GetAssemblyPath(assemblyType))
        {
        }

        public HelperChannelFactory(string endpointConfigurationName, EndpointAddress remoteAddress, string exePath)
            : this(endpointConfigurationName, remoteAddress, (Configuration)null, exePath)
        {
        }

        public HelperChannelFactory(string endpointConfigurationName, EndpointAddress remoteAddress, Configuration configuration)
               : this(endpointConfigurationName, remoteAddress, configuration, (string)null)
        {
        }

        #endregion


        private HelperChannelFactory([CanBeNull]string endpointConfigurationName,
                                     [Optional, CanBeNull] EndpointAddress remoteAddress,
                                     [Optional, CanBeNull] Configuration configuration,
                                     [Optional, CanBeNull] string exePath)
        {
            var internalEndpointConfigurationName = endpointConfigurationName;
            if (string.IsNullOrWhiteSpace(internalEndpointConfigurationName))
            {
                internalEndpointConfigurationName = "*";
            }
            var config = CreateConfig(configuration, exePath);

            this.configurationChannelFactory = GetChannelFactory(internalEndpointConfigurationName, config, remoteAddress);

            this.InitEvents();
        }

        #endregion

        #region Private

        private static Configuration CreateConfig([CanBeNull] Configuration configuration,
                                                  [CanBeNull] string exePath)
        {
            string exePath1;
            var config = default(Configuration);
            if (string.IsNullOrWhiteSpace(exePath) && configuration == null)
            {
                exePath1 = GetAssemblyPath(typeof(TChannel));
            }
            else
            {
                exePath1 = exePath;
            }
            if (!string.IsNullOrWhiteSpace(exePath1) && configuration == null)
            {
                config = new ConfigurationManager(exePath1).Configuration;
            }
            else if (configuration != default(Configuration))
            {
                config = configuration;
            }
            return config;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "The EndpointAddress is disposed from the ChannelFactory.")]
        private static IChannelFactory<TChannel> GetChannelFactory([CanBeNull] string endpointConfigurationName,
                                                                   [NotNull] Configuration configuration,
                                                                   [Optional, CanBeNull] EndpointAddress remoteAddress)
        {
            InvalidOperationException invalidChannelFactoryException;

            if (configuration == default(Configuration))
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            try
            {
                return new ConfigurationChannelFactory<TChannel>(
                    endpointConfigurationName,
                    configuration,
                    remoteAddress);
            }
            catch (InvalidOperationException ioe)
            {
                invalidChannelFactoryException = ioe;

                if (!@"System.ServiceModel".Equals(ioe.Source, StringComparison.OrdinalIgnoreCase))
                {
                    throw;
                }
            }
            try
            {
                return (remoteAddress == null)
                           ? new ChannelFactory<TChannel>(endpointConfigurationName)
                           : new ChannelFactory<TChannel>(endpointConfigurationName,
                                                          remoteAddress);
            }
            catch (InvalidOperationException ioe)
            {
                if (@"System.ServiceModel".Equals(ioe.Source, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidChannelFactoryException(
                                    WcfHelperText.InvalidConfiguration,
                                    invalidChannelFactoryException,
                                    ioe);
                }

                throw;
            }
        }

        /// <summary>
        /// Get the Path off the assembly codebase location
        /// </summary>
        /// <param name="assemblyType">a Type out of the assembly, you would read the config file</param>
        /// <returns>path uri as string</returns>
        private static string GetAssemblyPath(Type assemblyType)
        {
            return assemblyType.GetAssemblyPath();
        }

        private void InitEvents()
        {
            this.configurationChannelFactory.Closed += this.Closed;
            this.configurationChannelFactory.Closing += this.Closing;
            this.configurationChannelFactory.Faulted += this.Faulted;
            this.configurationChannelFactory.Opened += this.Opened;
            this.configurationChannelFactory.Opening += this.Opening;
        }

        #endregion

        #region IChannelFactory<TChannel>, IChannelFactory, ICommunicationObject 

        public void Abort()
        {
            this.configurationChannelFactory.Abort();
        }

        public void Close()
        {
            this.configurationChannelFactory.Close();
        }

        public void Close(TimeSpan timeout)
        {
            this.configurationChannelFactory.Close(timeout);
        }

        public IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            return this.configurationChannelFactory.BeginClose(callback, state);
        }

        public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return this.configurationChannelFactory.BeginClose(timeout, callback, state);
        }

        public void EndClose(IAsyncResult result)
        {
            this.configurationChannelFactory.EndClose(result);
        }

        public void Open()
        {
            this.configurationChannelFactory.Open();
        }

        public void Open(TimeSpan timeout)
        {
            this.configurationChannelFactory.Open(timeout);
        }

        public IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            return this.configurationChannelFactory.BeginOpen(callback, state);
        }

        public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return this.configurationChannelFactory.BeginOpen(timeout, callback, state);
        }

        public void EndOpen(IAsyncResult result)
        {
            this.configurationChannelFactory.EndOpen(result);
        }

        public CommunicationState State => this.configurationChannelFactory.State;

        public event EventHandler Closed;
        public event EventHandler Closing;
        public event EventHandler Faulted;
        public event EventHandler Opened;
        public event EventHandler Opening;

        public T GetProperty<T>() where T : class
        {
            return this.configurationChannelFactory.GetProperty<T>();
        }

        public TChannel CreateChannel()
        {
            return ((System.ServiceModel.ChannelFactory<TChannel>)this.configurationChannelFactory).CreateChannel();
        }

        public TChannel CreateChannel(EndpointAddress to)
        {
            return this.configurationChannelFactory.CreateChannel(to);
        }

        public TChannel CreateChannel(EndpointAddress to, Uri via)
        {
            return this.configurationChannelFactory.CreateChannel(to, via);
        }

        #endregion

        #region IDisposable

        void IDisposable.Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~HelperChannelFactory()
        {
            this.Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((IDisposable)this.configurationChannelFactory)?.Dispose();
            }
        }

        #endregion
    }
}