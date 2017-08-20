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
            if (!typeof(T).GetTypeInfo().IsEnum) throw new ArgumentException("The type parameter must be an enum");
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(arg => arg.GetDescription(), arg => arg);
        }
    }
}