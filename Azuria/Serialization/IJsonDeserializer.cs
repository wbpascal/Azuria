using System.Threading;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Newtonsoft.Json;

namespace Azuria.Serialization
{
    /// <summary>
    /// </summary>
    public interface IJsonDeserializer
    {
        /// <summary>
        /// </summary>
        /// <param name="json"></param>
        /// <param name="settings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IProxerResult<T> Deserialize<T>(string json, JsonSerializerSettings settings);
    }
}