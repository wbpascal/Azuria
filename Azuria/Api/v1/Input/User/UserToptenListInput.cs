using Azuria.Api.v1.Input.Converter;
using Azuria.Enums;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.User
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UserToptenListInput : InputDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        [InputData("kat", Converter = typeof(ToLowerConverter), Optional = true)]
        public MediaEntryType? Category { get; set; }

        /// <summary>
        /// </summary>
        [InputData("uid", Optional = true)]
        public int? UserId { get; set; }

        /// <summary>
        /// Ignored if <see cref="UserId"/> is given.
        /// </summary>
        [InputData("username", ConverterMethodName = nameof(GetUsernameString), Optional = true)]
        public string Username { get; set; }

        internal string GetUsernameString(string username)
        {
            return this.UserId == null ? username : null;
        }
    }
}