using System;
using Azuria.Api.v1.Converters;
using Newtonsoft.Json;

#pragma warning disable 1591

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class EntryForumThreadDataModel : IDataModel
    {
        [JsonProperty("category_id")]
        public int CategoryId { get; set; }
        
        [JsonProperty("category_name")]
        public string CategoryName { get; set; }
        
        [JsonProperty("first_post_time")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime FirstPostTime { get; set; }
        
        [JsonProperty("first_post_userid")]
        public int FirstPostUserId { get; set; }
        
        [JsonProperty("first_post_guest_name")]
        public string FirstPostUserName { get; set; }
        
        [JsonProperty("hits")]
        public int Hits { get; set; }
        
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("last_post_time")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime LastPostTime { get; set; }
        
        [JsonProperty("last_post_userid")]
        public int LastPostUserId { get; set; }
        
        [JsonProperty("last_post_guest_name")]
        public string LastPostUserName { get; set; }
        
        [JsonProperty("posts")]
        public int PostCount { get; set; }
        
        [JsonProperty("subject")]
        public string Subject { get; set; }
    }
}