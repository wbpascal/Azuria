using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.Enums;
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