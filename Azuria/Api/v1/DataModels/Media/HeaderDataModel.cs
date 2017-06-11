using System;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Media
{
    /// <summary>
    /// </summary>
    public class HeaderDataModel : IDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("imgfilename")]
        public string HeaderFileName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("gid")]
        public int HeaderId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("catpath")]
        public string HeaderPath { get; set; }

        /// <summary>
        /// </summary>
        public Uri HeaderUrl => new Uri($"{ApiConstants.ProxerHeaderCdnUrl}/{this.HeaderPath}/{this.HeaderFileName}");
    }
}