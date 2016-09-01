using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    internal class AnimeMangaContentDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("no", Required = Required.Always)]
        internal int ContentIndex { get; set; }

        [JsonProperty("typ", Required = Required.Always)]
        [JsonConverter(typeof(LanguageConverter))]
        internal AnimeMangaLanguage Language { get; set; }

        [JsonProperty("types")]
        [JsonConverter(typeof(StreamPartnerConverter))]
        internal StreamPartner[] StreamPartners { get; set; }

        /// <summary>
        ///     Only available for chapters
        /// </summary>
        [JsonProperty("title")]
        internal string Title { get; set; }

        #endregion
    }
}