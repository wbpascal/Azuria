using System;
using System.Linq;
using System.Reflection;

namespace Azuria.Utilities.Extensions
{
    internal static class TypeExtensions
    {
        #region Methods

        internal static bool HasParameterlessConstructor(this Type type)
        {
            foreach (ConstructorInfo ctor in type.GetTypeInfo().DeclaredConstructors)
                if (!ctor.IsPrivate && (ctor.GetParameters().Length == 0)) return true;
            return false;
        }

        internal static bool ImplementsGenericInterface(this Type generic, Type @interface)
        {
            return generic.GetTypeInfo().IsGenericType &&
                   generic.GetGenericTypeDefinition()
                       .GetTypeInfo()
                       .ImplementedInterfaces.Any(type => type.Name.Equals(@interface.Name));
        }

        internal static bool ImplementsInterface(this Type type, Type interfaceToCheck)
        {
            return type.GetTypeInfo().ImplementedInterfaces.Contains(interfaceToCheck);
        }

        #endregion
    }
}