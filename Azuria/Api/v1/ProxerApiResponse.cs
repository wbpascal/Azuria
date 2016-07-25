using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1
{
    internal class ProxerInfoLanguageResponse : ProxerApiResponse
    {
        #region Properties

        [JsonProperty("data"), JsonConverter(typeof(LanguageConverter))]
        internal AnimeMangaLanguage[] Data { get; set; }

        #endregion
    }

    internal class ProxerApiResponse<T> : ProxerApiResponse
    {
        #region Properties

        [JsonProperty("data")]
        internal T Data { get; set; }

        #endregion
    }

    internal class ProxerApiResponse
    {
        #region Properties

        [JsonProperty("error")]
        internal bool Error { get; set; }

        [JsonProperty("code")]
        internal ErrorCode ErrorCode { get; set; } = ErrorCode.NoError;

        [JsonProperty("message")]
        internal string Message { get; set; }

        #endregion
    }
}