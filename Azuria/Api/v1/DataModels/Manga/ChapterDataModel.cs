using System;
using Azuria.Api.v1.Converter;
using Azuria.Api.v1.Converter.Manga;
using Azuria.Api.v1.DataModels.Media;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Manga
{
    /// <summary>
    /// </summary>
    public class ChapterDataModel : UploadedMediaDataModel
    {
        /// <summary>
        /// </summary>
        [JsonProperty("cid")]
        public int ChapterId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("title")]
        public string ChapterTitle { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("eid")]
        public int EntryId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("name")]
        public string EntryName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("pages")]
        [JsonConverter(typeof(PagesConverter))]
        public PageDataModel[] Pages { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("server")]
        public int ServerId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime UploadTimestamp { get; set; }
    }
}