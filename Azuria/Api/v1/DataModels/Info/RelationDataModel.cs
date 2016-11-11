using System.Collections.Generic;
using Azuria.Api.v1.Converters;
using Azuria.Media.Properties;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    internal class RelationDataModel : EntryDataModel
    {
        #region Properties

        [JsonProperty("language")]
        [JsonConverter(typeof(LanguageCommaCollectionConverter))]
        internal IEnumerable<MediaLanguage> AvailableLanguages { get; set; }

        #endregion
    }
}