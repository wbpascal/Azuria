using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Enums;
using Azuria.Search.Input;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels
{
    internal interface IEntryInfoDataModel : IDataModel
    {
        #region Properties

        int EntryId { get; }

        [JsonConverter(typeof(MediumConverter))]
        MediaMedium EntryMedium { get; }

        string EntryName { get; }

        [JsonConverter(typeof(CategoryConverter))]
        MediaEntryType EntryType { get; }

        #endregion
    }
}