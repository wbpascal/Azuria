using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class SimpleIdInput : InputDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        [InputData("id")]
        public int Id { get; set; }
    }
}