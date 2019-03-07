using Azuria.Api.v1.Input.Converter;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class RelationsInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the entry.
        /// </summary>
        [InputData("id")]
        public int EntryId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [InputData("isH", Converter = typeof(ToLowerConverter), Optional = true)]
        public bool? ContainsH { get; set; }
    }
}