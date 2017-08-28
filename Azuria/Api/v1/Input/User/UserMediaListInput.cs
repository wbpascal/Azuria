using System.Collections.Generic;
using Azuria.Api.v1.Input.Ucp;

namespace Azuria.Api.v1.Input.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserMediaListInput : UcpGetListInput
    {
        /// <inheritdoc />
        public new Dictionary<string, string> Build()
        {
            Dictionary<string, string> lReturn = base.Build();
            throw new System.NotImplementedException();
        }
    }
}