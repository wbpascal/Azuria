using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Info;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.Enums.User;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class ListInfoDataModel : DataModelBase
    {
        /// <summary>
        /// </summary>
        [JsonProperty("kat")]
        [JsonConverter(typeof(CategoryConverter))]
        public MediaEntryType Category { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("episodes")]
        public MediaContentDataModel[] ContentObjects { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("end")]
        public int EndIndex { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("lang")]
        [JsonConverter(typeof(MediaLanguageCollectionConverter))]
        public MediaLanguage[] Languages { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("start")]
        public int StartIndex { get; set; }

        /// <summary>
        /// Progress of the logged in user. 0 if none is logged in?
        /// TODO: Check values of this
        /// </summary>
        [JsonProperty("state")]
        public MediaProgressState State { get; set; }
    }
}