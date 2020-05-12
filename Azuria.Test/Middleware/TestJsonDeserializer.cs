using System;
using Azuria.ErrorHandling;
using Azuria.Serialization;
using Newtonsoft.Json;

namespace Azuria.Test.Middleware
{
    public class TestJsonDeserializer : IJsonDeserializer
    {
        public static string TEST_MESSAGE = "test";

        public IProxerResult<T> Deserialize<T>(string json, JsonSerializerSettings settings)
        {
            throw new Exception(TEST_MESSAGE);
        }
    }
}
