using Azuria.Api.v1.RequestBuilder;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Ucp
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SetCommentProgressInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the comment of which the progress will be changed.
        /// The comment id of the entry can be aquired from <see cref="UcpRequestBuilder.GetList"/>.
        /// </summary>
        [InputData("id")]
        public int CommentId { get; set; }
        
        /// <summary>
        /// Gets or sets the progress the comment will be set to.
        /// </summary>
        [InputData("value")]
        public int Progress { get; set; }
    }
}