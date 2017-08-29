using System.Collections.Generic;
using System.Linq.Expressions;
using Azuria.Api.v1.Input.Ucp;
using Azuria.Helpers.Attributes;

namespace Azuria.Api.v1.Input.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserGetListInput : UcpGetListInput
    {
        /// <summary>
        /// If this is given the value of <see cref="Username"/> is ignored.
        /// </summary>
        [InputData("uid", Optional = true)]
        public int? UserId { get; set; }
        
        /// <summary>
        /// If <see cref="UserId"/> is given this value will be ignored.
        /// </summary>
        [InputData("username", ConverterMethodName = nameof(GetUsernameString), Optional = true)]
        public string Username { get; set; }

        internal string GetUsernameString(string username)
        {
            return this.UserId == null ? username : null;
        }
    }
}