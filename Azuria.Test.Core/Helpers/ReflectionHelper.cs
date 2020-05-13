using System;
using System.Reflection;

namespace Azuria.Test.Core.Helpers
{
    /// <summary>
    /// Provides Helper functions for reflection access.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Tries to get the value of a given private field with the name <paramref name="fieldName">fieldName</paramref> 
        /// of an <paramref name="from">object</paramref> and returns it as an instance of type 
        /// <typeparamref name="T">T</typeparamref>. If the value of the field is not of type <typeparamref name="T">T</typeparamref> 
        /// or the field cannot be accessed (e.g. wrong <paramref name="fieldName">fieldName</paramref>) then the default value 
        /// of type <typeparamref name="T">T</typeparamref> is returned
        /// </summary>
        /// <typeparam name="T">The type of the instance to return</typeparam>
        public static T GetPrivateFieldValueOrDefault<T>(this Type type, string fieldName, object from) where T : class
        {
            const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
            try
            {
                FieldInfo field = type.GetField(fieldName, bindFlags);
                return field.GetValue(from) as T ?? default(T);
            }
            catch (System.Exception)
            {
                return default(T);
            }
        }
    }
}