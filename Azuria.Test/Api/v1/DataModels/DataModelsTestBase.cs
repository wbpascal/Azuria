using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.Converter;
using Azuria.Api.v1.DataModels;
using Azuria.ErrorHandling;
using Azuria.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels
{
    public abstract class DataModelsTestBase<T> where T : class, IDataModel
    {
        protected DataModelsTestBase()
        {
            this.Client = ProxerClient.Create(new char[32]);
            this.Deserializer = new JsonDeserializer();
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
                return new JsonSerializerSettings {Converters = new List<JsonConverter>(new[] {converter})};
            }

            IProxerResult<ProxerApiResponse<T1>> lResult =
                this.Deserializer.Deserialize<ProxerApiResponse<T1>>(json, GetSettingsWithConverter());
            CheckSuccessResult(lResult);
            CheckSuccessResult(lResult.Result);
            return lResult.Result;
        }

        private static void CheckSuccessResult<T1>(IProxerResult<T1> response)
        {
            (bool success, IEnumerable<Exception> exceptions, T1 result) = response;
            Assert.True(success, GetExceptionMessage(exceptions));
            Assert.NotNull(exceptions);
            Assert.IsEmpty(exceptions);
            Assert.NotNull(result);
        }

        private static string GetExceptionMessage(IEnumerable<Exception> exceptions)
        {
            return exceptions.Aggregate("", (s, exception) => s + exception.ToString() + "\n");
        }
    }
}