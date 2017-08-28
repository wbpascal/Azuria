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
        /// 
        /// </summary>
        public Type Converter { get; set; }
        
        /// <summary>
        /// Checks this first, if presents skips <see cref="Converter"/>.
        /// </summary>
        public string ConverterMethodName { get; set; }
        
        /// <summary>
        /// 
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