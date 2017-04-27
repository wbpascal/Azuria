using System;
using Azuria.Api.v1.Converters;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.List
{
    /// <summary>
    /// 
    /// </summary>
    public class IndustryDataModel : IDataModel
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
        public Uri CoverImage => new Uri($"https://cdn.proxer.me/industry/{this.Id}.jpg");

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type", ItemConverterType = typeof(IndustryTypeConverter))]
        public IndustryType Type { get; set; }

        #endregion
    }
}