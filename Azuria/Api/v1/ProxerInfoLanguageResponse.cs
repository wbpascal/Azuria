using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.Converters.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1
{
    internal class ProxerInfoLanguageResponse : ProxerApiResponse
    {
        #region Properties

        [JsonProperty("data")]
        [JsonConverter(typeof(LanguageCollectionConverter))]
        internal AnimeMangaLanguage[] Data { get; set; }

        #endregion
    }
}