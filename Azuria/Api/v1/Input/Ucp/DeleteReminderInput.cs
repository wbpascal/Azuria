using Azuria.Api.v1.RequestBuilder;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Ucp
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteReminderInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the reminder that will be deleted. 
        /// The reminder id can be accessed through <see cref="UcpRequestBuilder.GetReminder"/>.
        /// </summary>
        [InputData("id")]
        public int ReminderId { get; set; }
    }
}