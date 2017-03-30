using System;
using System.Reflection;

namespace Azuria.Helpers
{
    internal static class VersionHelper
    {
        #region Methods

        internal static Version GetAssemblyVersion(Type typeOfAssembly)
        {
            return typeOfAssembly.GetTypeInfo().Assembly.GetName().Version;
        }

        #endregion
    }
}