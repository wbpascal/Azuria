using System;
using System.Reflection;

namespace Azuria.Helpers.Extensions
{
    internal static class TypeExtensions
    {
        public static bool CanBeNullAssigned(this Type info) =>
            !info.GetTypeInfo().IsValueType || Nullable.GetUnderlyingType(info) != null;
    }
}