using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.List
{
    /// <summary>
    /// 
    /// </summary>
    public class TagDataModel : IDataModel
    {
        #region Properties

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
        [JsonProperty("tag")]
        public string Name { get; set; }

        #endregion
    }
}