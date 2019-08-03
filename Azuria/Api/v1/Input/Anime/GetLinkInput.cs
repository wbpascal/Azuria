using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Anime
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class GetLinkInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the stream.
        /// </summary>
        [InputData("id")]
        public int Id { get; set; }
    }
}