using Azuria.Api.v1.Converters;
using Azuria.Enums.Info;
using Newtonsoft.Json;

#pragma warning disable 1591

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class DescriptionDataModel : DataModelBase
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(MediaLanguageConverter))]
        public Language Language { get; set; }
        
        [JsonProperty("subject")]
        public string Subject { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}