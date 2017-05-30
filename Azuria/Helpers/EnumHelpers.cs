using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Azuria.Helpers.Extensions;

namespace Azuria.Helpers
{
    internal static class EnumHelpers
    {
        internal static Dictionary<string, T> GetDescriptionDictionary<T>() where T : struct
        {
            Type lType = typeof(T);
            if (!lType.GetTypeInfo().IsEnum)
                throw new ArgumentException("The type parameter must be an enum", nameof(T));
            return Enum.GetValues(lType).Cast<T>().ToDictionary(arg => arg.GetDescription(), arg => arg);
        }
    }
}