using Azuria.Api.v1.Converters;
using Azuria.Enums;
using Newtonsoft.Json;

namespace Azuria.ErrorHandling
{
    /// <summary>
    /// Base class representing all Proxer API responses.
    /// </summary>
    public abstract class ProxerApiResponseBase : ProxerResultBase
    {
        /// <summary>
        /// Gets or sets the ErrorCode returned by the API.
        /// </summary>
        /// <value></value>
        [JsonProperty("code")]
        public ErrorCode ErrorCode { get; set; } = ErrorCode.NoError;

        /// <summary>
        /// Gets or sets the additional message returned by the API.
        /// </summary>
        /// <value></value>
        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }

        /// <inheritdoc />
        [JsonProperty("error", Required = Required.Always)]
        [JsonConverter(typeof(InvertBoolConverter))]
        public new bool Success { get; set; }
    }
}