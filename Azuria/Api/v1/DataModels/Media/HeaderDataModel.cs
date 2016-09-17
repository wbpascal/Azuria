using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Media
{
    internal class HeaderDataModel
    {
        #region Properties

        [JsonProperty("imgfilename")]
        internal string HeaderFileName { get; set; }

        [JsonProperty("gid")]
        internal int HeaderId { get; set; }

        [JsonProperty("catpath")]
        internal string HeaderPath { get; set; }

        internal Uri HeaderUrl => new Uri($"{ApiConstants.ProxerHeaderCdnUrl}/{this.HeaderPath}/{this.HeaderFileName}");

        #endregion
    }
}