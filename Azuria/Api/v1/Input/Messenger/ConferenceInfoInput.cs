using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Messenger
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ConferenceInfoInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the conference that is to be returned.
        /// </summary>
        [InputData("conference_id")]
        public int ConferenceId { get; set; }
    }
}