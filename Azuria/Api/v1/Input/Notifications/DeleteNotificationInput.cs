using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Notifications
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteNotificationInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the notification that should be deleted.
        /// Optional, if omitted, null or 0, all notification that are marked as read will be deleted.
        /// </summary>
        [InputData("nid", Optional = true)]
        public int? NotificationId { get; set; }
    }
}