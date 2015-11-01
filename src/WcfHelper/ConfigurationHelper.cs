namespace WcfHelper
{
    using System;
    using System.Configuration;

    using JetBrains.Annotations;

    using WcfHelper.Extensions;

    public static class ConfigurationHelper
    {
        /// <summary>
        /// read from the configuration the appsetting with the key
        /// </summary>
        /// <param name="key">one key of the appsettings configuration</param>
        /// <exception cref="System.ArgumentNullException">if key null</exception>
        /// <returns>value as string</returns>
        public static string LoadAppSetting([NotNull]string key)
        {
            key.CheckArgumentForNull(nameof(key));
            return System.Configuration.ConfigurationManager.AppSettings[key];

        }

        public static ConnectionStringSettings LoadConnectionStrings([NotNull]string key)
        {
            key.CheckArgumentForNull(nameof(key));
            return System.Configuration.ConfigurationManager.ConnectionStrings[key];

        }
        /// <summary>
        /// Lädt die Appsettings.
        /// </summary>
        /// <param name="key">Wenn der Key nicht vorhanden ist, wird eine Exception geworfen.</param>
        /// <param name="assemblyType"></param>
        /// <returns>Value aus Appsettings</returns>
        public static string LoadAppSetting([NotNull]string key, [NotNull] Type assemblyType)
        {
            string result;
            try
            {
                var config = WcfHelper.ConfigurationManager.CreateInstance(assemblyType);
                result = config.Configuration.HasFile
                    ? config.AppSettings.Settings[key].Value
                    : LoadAppSetting(key);
            }
            catch (ConfigurationErrorsException)
            {
                result = LoadAppSetting(key);
            }
            return result;
        }

        public static ConnectionStringSettings LoadConnectionStrings([NotNull]string key, [NotNull] Type assemblyType)
        {
            key.CheckArgumentForNull(nameof(key));
            assemblyType.CheckArgumentForNull(nameof(assemblyType));

            ConnectionStringSettings result;
            try
            {
                var config = WcfHelper.ConfigurationManager.CreateInstance(assemblyType);
                result = config.Configuration.HasFile
                        ? config.ConnectionStrings.ConnectionStrings[key]
                        : LoadConnectionStrings(key);
            }
            catch (ConfigurationErrorsException)
            {
                result = LoadConnectionStrings(key);
            }
            return result;
        }
    }
}
