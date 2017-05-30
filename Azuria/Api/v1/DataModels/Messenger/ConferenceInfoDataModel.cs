using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    /// <summary>
    /// </summary>
    public class ConferenceInfoDataModel : IDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("conference")]
        public ConferenceInfoMainDataModel MainInfo { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("users")]
        public ConferenceInfoParticipantDataModel[] ParticipantsInfo { get; set; }
    }
}