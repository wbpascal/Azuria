using System;
using System.Collections.Generic;

namespace Azuria.Api.v1
{
    internal class ApiRequest<T> : ApiRequest
    {
        internal ApiRequest(Senpai senpai, Uri address) : base(senpai, address)
        {
        }
    }

    internal class ApiRequest
    {
        internal ApiRequest(Senpai senpai, Uri address)
        {
            this.Senpai = senpai;
            this.Address = address;
        }

        #region Properties

        internal Uri Address { get; set; }

        internal bool CheckLogin { get; set; }

        internal Dictionary<string, string> PostArguments { get; set; } = new Dictionary<string, string>();

        internal Senpai Senpai { get; }

        #endregion
    }
}