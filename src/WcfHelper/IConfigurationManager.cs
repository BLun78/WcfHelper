namespace WcfHelper
{
    using System.Configuration;

    public interface IConfigurationManager
    {
        AppSettingsSection AppSettings { get; }
        System.Configuration.Configuration Configuration { get; }
        ConnectionStringsSection ConnectionStrings { get; }
    }
}