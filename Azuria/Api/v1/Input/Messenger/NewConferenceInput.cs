using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Messenger
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NewConferenceInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets a string that will be send to the conference after creating it. Commands will be ignored.
        /// </summary>
        [InputData("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the username of the user that will be added to the conference. 
        /// If there already exists a conference with this user, the string given in <see cref="Text"/> will be send 
        /// to this conference instead of creating a new one.
        /// </summary>
        [InputData("username")]
        public string Username { get; set; }
    }
}