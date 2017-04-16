using System;
using Azuria.Api.v1.Converters;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.List
{
    /// <summary>
    /// 
    /// </summary>
    public class TranslatorDataModel : IDataModel
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("country", ItemConverterType = typeof(CountryConverter))]
        public Country Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("image")]
        public Uri Image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion
    }
}