using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azuria.Api.Enums.Info;
using Azuria.Api.v1.Converters;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.List
{
    /// <summary>
    /// 
    /// </summary>
    public class TranslatorDataModel : IDataModel
    {
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
        [JsonProperty("country")]
        [JsonConverter(typeof(CountryConverter))]
        public Country Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("image")]
        public Uri Image { get; set; }
    }
}
