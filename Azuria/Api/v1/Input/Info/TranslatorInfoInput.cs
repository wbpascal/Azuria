using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Info
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TranslatorInfoInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the translator group.
        /// </summary>
        [InputData("id")]
        public int TranslatorId { get; set; }
    }
}