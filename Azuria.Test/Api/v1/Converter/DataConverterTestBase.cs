using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Autofac;
using Azuria.Api.v1.Converters;
using Azuria.ErrorHandling;
using Azuria.Serialization;
using Newtonsoft.Json;
using Xunit;

namespace Azuria.Test.Api.v1.Converter
{
    public abstract class DataConverterTestBase
    {
        protected DataConverterTestBase()
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            this.JsonDeserializer = lClient.Container.Resolve<IJsonDeserializer>();
        }
        
        public IJsonDeserializer JsonDeserializer { get; set; }

        public JsonSerializerSettings GetSerializerSettings<T>(DataConverter<T> converter)
        {
            return new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>(new[] {converter})
            };
        }

        public T DeserializeValue<T>(string value, DataConverter<T> converter)
        {
            IProxerResult<Dictionary<string, T>> lResult = this.JsonDeserializer.Deserialize<Dictionary<string, T>>(
                this.GetTestJsonString(value), this.GetSerializerSettings(converter)
            );
            Assert.True(lResult.Success);
            Assert.Empty(lResult.Exceptions);
            Assert.NotNull(lResult.Result);
            Assert.True(lResult.Result.ContainsKey("data"));
            return lResult.Result["data"];
        }

        public string GetTestJsonString(string value)
        {
            return $"{{'data':{value}}}";
        }
    }
}