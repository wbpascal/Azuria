using System.Collections.Generic;
using System.Linq;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Messenger
{
    /// <summary>
    /// 
    /// </summary>
    public class NewConferenceGroupInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the names of the users which will be added to the new conference. The maximum amount is limited by the message constants.
        /// </summary>
        public IEnumerable<string> Usernames { get; set; }

        /// <summary> 
        /// Optional, if omitted (or null) no message will be send to the conference group after creation.
        /// </summary>
        [InputData("text", Optional = true)]
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InputData("topic")]
        public string Topic { get; set; }

        /// <inheritdoc />
        public override IEnumerable<KeyValuePair<string, string>> Build()
        {
            var lReturn = new List<KeyValuePair<string, string>>(base.Build());
            lReturn.AddRange(this.Usernames?.Select(username => new KeyValuePair<string, string>("users[]", username)));
            return lReturn;
        }
    }
}