using Azuria.Api.v1.Converters.Info;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    internal class RelationDataModel : EntryDataModel
    {
        #region Properties

        [JsonProperty("language"), JsonConverter(typeof(LanguageKommaCollectionConverter))]
        internal AnimeMangaLanguage[] AvailableLanguages { get; set; }

        #endregion
    }
}