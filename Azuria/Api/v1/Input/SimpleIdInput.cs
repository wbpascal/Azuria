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
        public SimpleIdInput()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public SimpleIdInput(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        [InputData("id")]
        public int Id { get; set; }
    }
}