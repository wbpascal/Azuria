using System;
using Azuria.Api.v1.Converters;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Ucp
{
    /// <summary>
    /// </summary>
    public class HistoryDataModelBase : UcpEntryInfoDataModelBase
    {
        /// <summary>
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime TimeStamp { get; set; }
    }
}