using System;
using System.Collections.Generic;

namespace Azuria.Api.v1
{
    internal class ApiRequest<T> : ApiRequest
    {
        internal ApiRequest(Senpai senpai) : base(senpai)
        {
        }
    }

    internal class ApiRequest
    {
        internal ApiRequest(Senpai senpai)
        {
            this.Senpai = senpai;
        }

        #region Properties

        internal Uri Address { get; set; }

        internal bool CheckLogin { get; set; }

        internal Dictionary<string, string> PostArguments { get; set; } = new Dictionary<string, string>();

        internal Senpai Senpai { get; }

        #endregion
    }
}