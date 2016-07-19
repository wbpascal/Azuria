using Newtonsoft.Json;

namespace Azuria.Api.v1
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProxerApiResponse<T> : ProxerApiResponse
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        #endregion
    }

    /// <summary>
    /// </summary>
    public class ProxerApiResponse
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("error")]
        public bool Error { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("code")]
        public ErrorCode ErrorCode { get; set; } = ErrorCode.NoError;

        [JsonProperty("message")]
        internal string Message { get; set; }

        #endregion
    }
}