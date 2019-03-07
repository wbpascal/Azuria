using Azuria.Api.v1.Converters;
using Azuria.Enums.Info;
using Newtonsoft.Json;

#pragma warning disable 1591

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class NameDataModel : DataModelBase
    {
        [JsonProperty("alternative")]
        public string Alternative { get; set; }
        
        [JsonProperty("display_name")]
        [JsonConverter(typeof(IntToBoolConverter))]
        public bool IsDisplayName { get; set; }
        
        [JsonProperty("language")]
        [JsonConverter(typeof(LanguageConverter))]
        public Language Language { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}