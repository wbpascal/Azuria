using System;
using System.Collections.Generic;
using System.Text;

namespace Azuria.Api
{
    public interface IProxerUser
    {
        void UsedCookies();
        bool IsProbablyLoggedIn { get; }
        char[] LoginToken { get; set; }
    }
}
