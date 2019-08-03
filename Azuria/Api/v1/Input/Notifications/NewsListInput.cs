using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Notifications
{
    /// <summary>
    /// 
    /// </summary>
    public class NewsListInput : PagedInputDataModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether all news up to the newest returned one should be marked as read. 
        /// This value will be ignored if no user is logged in. 
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("set_read", Optional = true)]
        public bool? MarkRead { get; set; }
    }
}