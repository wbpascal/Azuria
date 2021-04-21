using System;
using Azuria.Api.v1.Converter;
using Azuria.Enums;
using Azuria.Helpers;
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
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the additional message returned by the API.
        /// </summary>
        /// <value></value>
        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }

        /// <inheritdoc cref="ProxerResultBase.Success" />
        [JsonProperty("error", Required = Required.Always)]
        [JsonConverter(typeof(InvertBoolConverter))]
        public new bool Success { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ErrorCode GetErrorCode() => ErrorCodeHelper.GetErrorCodeFromInt(this.ErrorCode);
    }
}