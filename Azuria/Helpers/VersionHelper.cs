using System;
using System.Reflection;

namespace Azuria.Helpers
{
    internal static class VersionHelper
    {
        internal static Version GetAssemblyVersion(Type typeOfAssembly)
        {
            return typeOfAssembly.GetTypeInfo().Assembly.GetName().Version;
        }
    }
}