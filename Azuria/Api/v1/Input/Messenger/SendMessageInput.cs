using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Messenger
{
    /// <summary>
    /// 
    /// </summary>
    public class SendMessageInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the conference the message will be send to.
        /// </summary>
        [InputData("conference_id")]
        public int ConferenceId { get; set; }

        /// <summary>
        /// Gets or sets the message that is send to the given conference.
        /// </summary>
        [InputData("text")]
        public string Message { get; set; }
    }
}