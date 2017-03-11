using System.Collections.Generic;
using Azuria.Api.v1.Converters;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class RelationDataModel : EntryDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("language")]
        [JsonConverter(typeof(LanguageCommaCollectionConverter))]
        public MediaLanguage[] AvailableLanguages { get; set; }

        #endregion
    }
}