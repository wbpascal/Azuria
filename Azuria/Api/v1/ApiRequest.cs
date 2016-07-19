using System;
using System.Collections.Generic;

namespace Azuria.Api.v1
{
    internal class ApiRequest<T> : ApiRequest
    {
    }

    internal class ApiRequest
    {
        #region Properties

        internal Uri Address { get; set; }

        internal Dictionary<string, string> PostArguments { get; set; } = new Dictionary<string, string>();

        internal Senpai Senpai { get; set; }

        #endregion
    }
}