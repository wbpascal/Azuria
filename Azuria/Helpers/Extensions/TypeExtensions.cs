using System;
using System.Reflection;

namespace Azuria.Helpers.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsSameOrSubclass(this Type derived, Type @base)
        {
            return derived == @base || derived.GetTypeInfo().IsSubclassOf(@base);
        }
    }
}