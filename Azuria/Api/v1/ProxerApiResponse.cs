using Newtonsoft.Json;

namespace Azuria.Api.v1
{
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

        [JsonProperty("error", Required = Required.Always)]
        internal bool Error { get; set; }

        [JsonProperty("code")]
        internal ErrorCode ErrorCode { get; set; } = ErrorCode.NoError;

        [JsonProperty("message", Required = Required.Always)]
        internal string Message { get; set; }

        #endregion
    }
}