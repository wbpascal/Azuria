using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class ListInfoInput : PagedInputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the entry.
        /// </summary>
        [InputData("id")]
        public int EntryId { get; set; }
    }
}