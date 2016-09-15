using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.Converters;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    internal class RelationDataModel : EntryDataModel
    {
        #region Properties

        [JsonProperty("language")]
        [JsonConverter(typeof(LanguageCommaCollectionConverter))]
        internal AnimeMangaLanguage[] AvailableLanguages { get; set; }

        #endregion
    }
}