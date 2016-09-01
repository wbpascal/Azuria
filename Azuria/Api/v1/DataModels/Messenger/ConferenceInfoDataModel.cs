using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    internal class ConferenceInfoDataModel : IDataModel
    {
        #region Properties

        [JsonProperty("conference")]
        internal ConferenceInfoMainDataModel MainInfo { get; set; }

        [JsonProperty("users")]
        internal ConferenceInfoParticipantDataModel[] ParticipantsInfo { get; set; }

        #endregion
    }
}