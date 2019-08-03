using Azuria.Api.v1.Input.Converter;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Anime
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the input data for requests that return the streams of an episode.
    /// </summary>
    public class StreamListInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the episode of which the streams should be returned.
        /// </summary>
        [InputData("episode")]
        public int Episode { get; set; }

        /// <summary>
        /// Gets or sets the anime id the epsiode is from.
        /// </summary>
        [InputData("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the language of the episode.
        /// 
        /// Invalid Values:
        /// * <see cref="AnimeLanguage.Unknown"/>
        /// </summary>
        [InputData(
            "language", Converter = typeof(ToLowerConverter), ForbiddenValues = new object[] {AnimeLanguage.Unknown})]
        public AnimeLanguage Language { get; set; }
    }
}