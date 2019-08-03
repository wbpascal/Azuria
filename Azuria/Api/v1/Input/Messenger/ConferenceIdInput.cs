using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Messenger
{
    /// <summary>
    /// 
    /// </summary>
    public class ConferenceIdInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the conference.
        /// </summary>
        [InputData("conference_id")]
        public int ConferenceId { get; set; }
    }
}