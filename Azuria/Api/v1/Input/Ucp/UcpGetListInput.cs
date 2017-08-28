using Azuria.Api.v1.Input.Converter;
using Azuria.Enums;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Ucp
{
    /// <summary>
    /// 
    /// </summary>
    public class UcpGetListInput : InputDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        [InputData("kat", Converter = typeof(ToLowerConverter))]
        public MediaEntryType Category { get; set; }

        internal string GetCategoryString(MediaEntryType category)
        {
            return this.Category.ToString().ToLowerInvariant();
        }
    }
}