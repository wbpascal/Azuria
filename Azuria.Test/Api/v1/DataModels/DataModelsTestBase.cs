using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Autofac;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.DataModels;
using Azuria.ErrorHandling;
using Azuria.Serialization;
using Newtonsoft.Json;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels
{
    public abstract class DataModelsTestBase<T> where T : class, IDataModel
    {
        protected DataModelsTestBase()
        {
            this.Client = ProxerClient.Create(new char[32]);
            this.Deserializer = this.Client.Container.Resolve<IJsonDeserializer>();
        }
        
        public IProxerClient Client { get; }
        
        public IJsonDeserializer Deserializer { get; }

        public ProxerApiResponse<T[]> ConvertArray(string json, DataConverter<T[]> converter = null)
        {
            return this.Convert(json, converter);
        }

        public ProxerApiResponse<T> Convert(string json, DataConverter<T> converter = null)
        {
            return this.Convert<T>(json, converter);
        }

        private ProxerApiResponse<T1> Convert<T1>(string json, DataConverter<T1> converter = null)
        {
            JsonSerializerSettings GetSettingsWithConverter()
            {
                if (converter == null) return null;
                return new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>(new[] {converter})
                };
            }

            IProxerResult<ProxerApiResponse<T1>> lResult =
                this.Deserializer.Deserialize<ProxerApiResponse<T1>>(json, GetSettingsWithConverter());
            Assert.True(lResult.Success);
            Assert.NotNull(lResult.Exceptions);
            Assert.Empty(lResult.Exceptions);
            Assert.NotNull(lResult.Result);
            return lResult.Result;
        }

        public void CheckSuccessResponse<T1>(ProxerApiResponse<T1> response)
        {
            Assert.True(response.Success);
            Assert.NotNull(response.Exceptions);
            Assert.Empty(response.Exceptions);
            Assert.NotNull(response.Result);
        }
    }
}