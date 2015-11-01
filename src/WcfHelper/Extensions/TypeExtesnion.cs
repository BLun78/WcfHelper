namespace WcfHelper.Extensions
{
    using System;

    using JetBrains.Annotations;

    internal static class TypeExtesnion
    {
        public static string GetAssemblyPath([NotNull]this Type assemblyType)
        {
            var codebase = new Uri(assemblyType.Assembly.CodeBase).LocalPath;
            return string.IsNullOrWhiteSpace(codebase)
                    ? assemblyType.Assembly.Location
                    : codebase;
        }
    }
}
