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

        /// <summary>
        /// Parses an enum value from a string and converts the value to the right type.
        /// Throws an exception if the parsing fails.
        /// </summary>
        /// <param name="value">The string that will be parsed.</param>
        /// <typeparam name="T">The type of the enum to which the string should be parsed.</typeparam>
        /// <returns></returns>
        internal static T ParseFromString<T>(string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Parses an enum value from a string and converts the value to the right type.
        /// Returns <paramref name="defaultValue"/> if the parsing fails.
        /// </summary>
        /// <param name="value">The string that will be parsed.</param>
        /// <param name="defaultValue">The value that will be returned if the parsing fails.</param>
        /// <typeparam name="T">The type of the enum to which the string should be parsed.</typeparam>
        /// <returns></returns>
        internal static T ParseFromString<T>(string value, T defaultValue)
        {
            try
            {
                return ParseFromString<T>(value);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}