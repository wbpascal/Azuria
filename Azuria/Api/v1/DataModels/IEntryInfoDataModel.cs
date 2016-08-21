using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Enums;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels
{
    internal interface IEntryInfoDataModel : IDataModel
    {
        #region Properties

        int EntryId { get; }

        [JsonConverter(typeof(MediumConverter))]
        AnimeMangaMedium EntryMedium { get; }

        string EntryName { get; }

        [JsonConverter(typeof(CategoryConverter))]
        AnimeMangaEntryType EntryType { get; }

        #endregion
    }
}