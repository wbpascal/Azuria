using Azuria.Api.v1.Converters.Info;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class PersonDataModel : DataModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("pid")]
        public int Id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(PersonTypeConverter))]
        public PersonType Type { get; set; }
    }
}