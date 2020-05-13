using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Azuria.ErrorHandling
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProxerApiResponse<T> : ProxerApiResponseBase, IProxerResult<T>
    {
        /// <inheritdoc />
        [JsonProperty("data")]
        public T Result { get; internal set; }

        /// <inheritdoc />
        public void Deconstruct(out bool success, out IEnumerable<Exception> exceptions, out T result)
        {
            success = this.Success;
            exceptions = this.Exceptions;
            result = this.Result;
        }
    }

    /// <summary>
    /// </summary>
    public class ProxerApiResponse : ProxerApiResponseBase, IProxerResult
    {
        /// <summary>
        /// Method used to desconstruct an api response into a tuple.
        /// </summary>
        /// <param name="success"></param>
        /// <param name="exceptions"></param>
        public void Deconstruct(out bool success, out IEnumerable<Exception> exceptions)
        {
            success = this.Success;
            exceptions = this.Exceptions;
        }
    }
}