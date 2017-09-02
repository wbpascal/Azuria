using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.List
{
    /// <summary>
    /// </summary>
    public class TranslatorProjectDataModel : ProjectDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("type")]
        public TranslationStatus Status { get; set; }
    }
}