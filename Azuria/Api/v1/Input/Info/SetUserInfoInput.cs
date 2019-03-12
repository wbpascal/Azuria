using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class SetUserInfoInput : InputDataModel
    {
        /// <summary>
        /// Gets or sets the id of the entry that should be added to the list of the logged in user specified in .
        /// </summary>
        [InputData("id")]
        public int EntryId { get; set; }

        /// <summary>
        /// Gets or sets the list of the logged in user to which the entry should be added.
        /// </summary>
        [InputData("type", Converter = typeof(ToTypeStringConverter))]
        public UserList List { get; set; }
    }
}