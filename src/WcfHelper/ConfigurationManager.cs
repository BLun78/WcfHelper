using GlobalAssemblyConfiguration = System.Configuration.ConfigurationManager;

namespace WcfHelper
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using System.Runtime.InteropServices;

    using JetBrains.Annotations;
    using WcfHelper.Extensions;

    /// <summary>
    /// Configuration Manager how helps open assembly configs
    /// </summary>
    public sealed class ConfigurationManager : IConfigurationManager
    {
        /// <summary>
        /// the configuration
        /// </summary>
        public Configuration Configuration { get; private set; }

        /// <summary>
        /// an appsettingssection
        /// </summary>
        public AppSettingsSection AppSettings => this.Configuration.AppSettings;

        /// <summary>
        /// connectionstringsection
        /// </summary>
        public ConnectionStringsSection ConnectionStrings => this.Configuration.ConnectionStrings;

        /// <summary>
        /// Get the path over the assembly path wich is given or it takes the defaul the globalassemblyconfig.OpenMachineConfiguration
        /// </summary>
        /// <param name="configurationAssemblyPath">an optional string for the path of the assembly</param>
        public ConfigurationManager([Optional, CanBeNull]string configurationAssemblyPath)
        {
            this.Configuration = configurationAssemblyPath == null
                ? GlobalAssemblyConfiguration.OpenMachineConfiguration()
                : GlobalAssemblyConfiguration.OpenExeConfiguration(configurationAssemblyPath);
        }

        /// <summary>
        /// Get the configuration over an assembly the assembly path
        /// </summary>
        /// <param name="configurationAssembly">a assembly wich you would read there config</param>
        public ConfigurationManager([NotNull] Assembly configurationAssembly)
            : this((Type)(configurationAssembly.CheckArgumentForNull(nameof(configurationAssembly))).GetType())
        {
        }

        /// <summary>
        /// Get the configuration over a type of his assembly path 
        /// </summary>
        /// <param name="configurationAssemblyType">a type wich you would take the config</param>
        public ConfigurationManager([NotNull] Type configurationAssemblyType)
            : this((string)(configurationAssemblyType.CheckArgumentForNull(nameof(configurationAssemblyType))).GetAssemblyPath())
        {
        }

        #region Factory

        public static IConfigurationManager CreateInstance([NotNull] Assembly configurationAssembly)
        {
            return new ConfigurationManager(configurationAssembly);
        }


        public static IConfigurationManager CreateInstance([NotNull]Type configurationAssemblyType)
        {
            return new ConfigurationManager(configurationAssemblyType);
        }

        public static IConfigurationManager CreateInstance([Optional, CanBeNull] string configurationAssemblyPath)
        {
            return new ConfigurationManager(configurationAssemblyPath);
        }

        #endregion
    }
}
