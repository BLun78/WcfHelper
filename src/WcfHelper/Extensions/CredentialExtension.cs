namespace WcfHelper.Extensions
{
    using System.ServiceModel.Description;
    using System.ServiceModel.Security;

    using JetBrains.Annotations;

    internal static class CredentialExtension
    {
        public static void UserNamePasswordClientCredentialMappingToClientCredentials(
                                        [NotNull] this ClientCredentials clientCredential,
                                        [NotNull] UserNamePasswordClientCredential userNamePasswordClientCredential)
        {
            clientCredential.CheckArgumentForNull(nameof(clientCredential));
            userNamePasswordClientCredential.CheckArgumentForNull(nameof(userNamePasswordClientCredential));

            var userName = clientCredential.UserName;
            userName.Password = userNamePasswordClientCredential.Password;
            userName.UserName = userNamePasswordClientCredential.UserName;
        }

        public static void WindowsClientCredentialMappingToClientCredentials(
                                        [NotNull] this ClientCredentials clientCredential,
                                        [NotNull] WindowsClientCredential windowsClientCredential)
        {
            clientCredential.CheckArgumentForNull(nameof(clientCredential));
            windowsClientCredential.CheckArgumentForNull(nameof(windowsClientCredential));

            var windows = clientCredential.Windows;
#pragma warning disable 618 //
            windows.AllowNtlm = windowsClientCredential.AllowNtlm;
#pragma warning restore 618
            windows.AllowedImpersonationLevel = windowsClientCredential.AllowedImpersonationLevel;

            windows.ClientCredential.UserName = windowsClientCredential.ClientCredential.UserName;
            windows.ClientCredential.Password = windowsClientCredential.ClientCredential.Password;
            windows.ClientCredential.Domain = windowsClientCredential.ClientCredential.Domain;
        }
    }
}
