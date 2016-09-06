using System;
using System.Collections.Generic;

namespace Azuria.Api.v1
{
    internal class ApiRequest<T> : ApiRequest
    {
        internal ApiRequest(Uri address) : base(address)
        {
        }
    }

    internal class ApiRequest
    {
        internal ApiRequest(Uri address)
        {
            this.Address = address;
        }

        #region Properties

        internal Uri Address { get; set; }

        internal bool CheckLogin { get; set; }

        internal IEnumerable<KeyValuePair<string, string>> PostArguments { get; set; } =
            new Dictionary<string, string>();

        internal Senpai Senpai { get; set; }

        #endregion
    }
}