namespace WcfHelper.Extensions
{
    using System;

    using JetBrains.Annotations;

    internal static class ObjectExtension
    {
        public static T CheckArgumentForNull<T>([CanBeNull]this T input, string argumentName)
            where T : class
        {
            if (default(T) == input)
            {
                throw new ArgumentNullException(argumentName);
            }
            return input;
        }
    }
}
