using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Newtonsoft.Json;

namespace Azuria.Serialization
{
    /// <summary>
    ///
    /// </summary>
    public class JsonDeserializer : IJsonDeserializer
    {
        /// <inheritdoc />
        public Task<IProxerResult<T>> Deserialize<T>(
            string json, JsonSerializerSettings settings, CancellationToken token = default(CancellationToken))
        {
            try
            {
                return Task<IProxerResult<T>>.Factory.StartNew(
                    () =>
                        new ProxerResult<T>(
                            JsonConvert.DeserializeObject<T>(
                                WebUtility.HtmlDecode(json), settings ?? new JsonSerializerSettings()
                            )
                        ), token
                );
            }
            catch (Exception ex)
            {
                return Task.FromResult((IProxerResult<T>) new ProxerResult<T>(ex));
            }
        }
    }
}