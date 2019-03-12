using System;

namespace Azuria.Helpers.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class InputDataAttribute : Attribute
    {
        /// <summary>
        /// Type has to implement <code>IInputDataConverter<T></code> where T is assignable to the
        /// Type of the Property
        /// </summary>
        public Type Converter { get; set; }

        /// <summary>
        /// Checks this first, if presents skips <see cref="Converter"/>.
        /// </summary>
        public string ConverterMethodName { get; set; }

        /// <summary>
        /// Gets or sets which values (after or before conversion of value to string!) are forbidden.
        /// If a forbidden value was detected and <see cref="Optional"/> is true, the whole property
        /// is ignored and not added to the dictionary. If <see cref="Optional"/> is false, a exception
        /// is thrown if the converted value is forbidden.
        /// </summary>
        public object[] ForbiddenValues { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Optional { get; set; }

        /// <inheritdoc />
        public InputDataAttribute(string key)
        {
            this.Key = key;
        }
    }
}