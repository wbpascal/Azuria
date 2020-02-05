using Azuria.Api.v1.Converters;
using Azuria.Enums;
using Newtonsoft.Json;

namespace Azuria.ErrorHandling
{
    public abstract class ProxerApiResponseBase : ProxerResultBase
    {
        [JsonProperty("code")] public ErrorCode ErrorCode { get; set; } = ErrorCode.NoError;

        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }

        /// <inheritdoc />
        [JsonProperty("error", Required = Required.Always)]
        [JsonConverter(typeof(InvertBoolConverter))]
        public new bool Success { get; set; }
    }
}