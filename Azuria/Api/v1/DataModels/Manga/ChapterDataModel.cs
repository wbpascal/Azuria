using System;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Manga;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Manga
{
    /// <summary>
    /// </summary>
    public class ChapterDataModel
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
        [JsonProperty("pages")]
        [JsonConverter(typeof(PagesConverter))]
        public PageDataModel[] Pages { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("server")]
        public int ServerId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("tid")]
        public int? TranslatorId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("tname")]
        public string TranslatorName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("uploader")]
        public int UploaderId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("username")]
        public string UploaderName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        public DateTime UploadTimestamp { get; set; }
    }
}