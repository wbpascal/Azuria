using System.Collections.Generic;
using Autofac;
using Azuria.Api.v1.Converters;
using Azuria.ErrorHandling;
using Azuria.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.Converter
{
    public abstract class DataConverterTestBase<TOut>
    {
        protected DataConverterTestBase(DataConverter<TOut> converter)
        {
            IProxerClient lClient = ProxerClient.Create(new char[32]);
            this.Converter = converter;
            this.JsonDeserializer = new JsonDeserializer();
        }

        public DataConverter<TOut> Converter { get; set; }

        public IJsonDeserializer JsonDeserializer { get; set; }

        public JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings {Converters = new List<JsonConverter>(new[] {this.Converter})};
        }

        public TOut DeserializeValue(string value)
        {
            IProxerResult<Dictionary<string, TOut>> lResult =
                this.JsonDeserializer.Deserialize<Dictionary<string, TOut>>(
                    this.GetTestJsonString(value), this.GetSerializerSettings()
                );
            Assert.True(lResult.Success);
            Assert.IsEmpty(lResult.Exceptions);
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