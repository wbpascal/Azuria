using Azuria.Api.v1.RequestBuilder;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Ucp
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteToptenInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the topten entry that should be deleted. 
        /// The topten id of an entry can be accessed through <see cref="UcpRequestBuilder.GetTopten"/>.
        /// </summary>
        [InputData("id")]
        public int ToptenId { get; set; }
    }
}