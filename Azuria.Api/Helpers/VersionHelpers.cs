using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Azuria.Api.Helpers
{
    internal static class VersionHelpers
    {
        internal static Version GetAssemblyVersion(Type typeOfAssembly)
        {
            return typeOfAssembly.GetTypeInfo().Assembly.GetName().Version;
        }
    }
}
