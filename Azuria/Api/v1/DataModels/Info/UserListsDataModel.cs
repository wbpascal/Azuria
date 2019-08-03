using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class UserListsDataModel : DataModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("canceled")]
        public bool IsCanceled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("finished")]
        public bool IsFinished { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("topten")]
        public bool IsInTopten { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("marked")]
        public bool IsMarked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("noted")]
        public bool IsNoted { get; set; }
    }
}