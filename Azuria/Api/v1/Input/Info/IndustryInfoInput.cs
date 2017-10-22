using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Info
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public sealed class IndustryInfoInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the industry (publisher/producer/studio etc.).
        /// </summary>
        [InputData("id")]
        public int IndustryId { get; set; }
    }
}