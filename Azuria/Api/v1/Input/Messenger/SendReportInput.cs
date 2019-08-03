using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Messenger
{
    /// <summary>
    /// 
    /// </summary>
    public class SendReportInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the conference that the user wants to report.
        /// </summary>
        [InputData("conference_id")]
        public int ConferenceId { get; set; }

        /// <summary>
        /// Gets or sets a string that represents the reason the user is reporting the conference.
        /// </summary>
        [InputData("text")]
        public string Reason { get; set; }
    }
}