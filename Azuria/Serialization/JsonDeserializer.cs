using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Newtonsoft.Json;

namespace Azuria.Serialization
{
    /// <summary>
    /// </summary>
    public class JsonDeserializer : IJsonDeserializer
    {
        /// <inheritdoc />
        public IProxerResult<T> Deserialize<T>(string json, JsonSerializerSettings settings)
        {
            try
            {
                T lDeserializedObject = JsonConvert.DeserializeObject<T>(
                    WebUtility.HtmlDecode(json), settings ?? new JsonSerializerSettings()
                );

                return new ProxerResult<T>(lDeserializedObject);
            }
            catch (Exception ex)
            {
                return new ProxerResult<T>(ex);
            }
        }
    }
}