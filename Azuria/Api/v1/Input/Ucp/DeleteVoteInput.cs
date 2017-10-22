using Azuria.Api.v1.RequestBuilder;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Ucp
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DeleteVoteInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the vote that will be deleted.
        /// The vote id can be accessed through <see cref="UcpRequestBuilder.GetVotes"/>.
        /// </summary>
        [InputData("id")]
        public int VoteId { get; set; }
    }
}