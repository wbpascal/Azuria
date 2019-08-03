using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Info
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the input data for a request that returns a list of comments from an entry.
    /// </summary>
    public class CommentListInput : PagedInputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the entry.
        /// </summary>
        [InputData("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the category by which the returned elements will be sorted by. 
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("sort", Converter = typeof(ToLowerConverter), Optional = true)]
        public CommentSort? Sort { get; set; }
    }
}