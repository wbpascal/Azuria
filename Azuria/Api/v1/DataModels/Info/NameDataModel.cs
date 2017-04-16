using Azuria.Api.v1.Converters.Info;
using Azuria.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class NameDataModel : IDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("type", ItemConverterType = typeof(NameTypeConverter))]
        public MediaNameType Type { get; set; }

        #endregion
    }
}