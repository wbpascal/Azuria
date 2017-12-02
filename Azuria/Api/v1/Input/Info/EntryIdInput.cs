using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Info
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the input data for requests that only require the id of an entry.
    /// </summary>
    public sealed class EntryIdInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the entry.
        /// </summary>
        [InputData("id")]
        public int EntryId { get; set; }
    }
}