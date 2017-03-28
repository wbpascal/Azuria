using System;
using System.Reflection;

namespace Azuria.Api.Helpers
{
    internal static class VersionHelpers
    {
        #region Methods

        internal static Version GetAssemblyVersion(Type typeOfAssembly)
        {
            return typeOfAssembly.GetTypeInfo().Assembly.GetName().Version;
        }

        #endregion
    }
}