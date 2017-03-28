using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Messenger
{
    /// <summary>
    /// </summary>
    public class ConferenceInfoDataModel : IDataModel
    {
        #region Properties

        /// <summary>
        /// </summary>
        [JsonProperty("conference")]
        public ConferenceInfoMainDataModel MainInfo { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("users")]
        public ConferenceInfoParticipantDataModel[] ParticipantsInfo { get; set; }

        #endregion
    }
}