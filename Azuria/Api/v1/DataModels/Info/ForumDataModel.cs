using System;
using Azuria.Api.v1.Converter;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class ForumDataModel : DataModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("category_id")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("category_name")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("first_post_time")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime FirstPostTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("first_post_userid")]
        public int FirstPostUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("first_post_guest_name")]
        public int FirstPostUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("hits")]
        public int Hits { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("last_post_time")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime LastPostTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("lat_post_userid")]
        public int LastPostUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("last_post_guest_name")]
        public int LastPostUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("posts")]
        public int Posts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }
    }
}