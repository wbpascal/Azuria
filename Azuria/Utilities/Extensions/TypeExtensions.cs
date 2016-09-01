using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Azuria.Utilities.Extensions
{
    internal static class TypeExtensions
    {
        #region

        internal static bool HasParameterlessConstructor([NotNull] this Type type)
        {
            foreach (ConstructorInfo ctor in type.GetTypeInfo().DeclaredConstructors)
                if (!ctor.IsPrivate && (ctor.GetParameters().Length == 0)) return true;
            return false;
        }

        internal static bool ImplementsGenericInterface([NotNull] this Type generic, [NotNull] Type @interface)
        {
            return generic.GetTypeInfo().IsGenericType &&
                   generic.GetGenericTypeDefinition()
                       .GetTypeInfo()
                       .ImplementedInterfaces.Any(type => type.Name.Equals(@interface.Name));
        }

        internal static bool ImplementsInterface([NotNull] this Type type, [NotNull] Type interfaceToCheck)
        {
            return type.GetTypeInfo().ImplementedInterfaces.Contains(interfaceToCheck);
        }

        #endregion
    }
}