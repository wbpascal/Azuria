using System;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Manga;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Manga
{
    internal class ChapterDataModel
    {
        #region Properties

        [JsonProperty("cid")]
        internal int ChapterId { get; set; }

        [JsonProperty("title")]
        internal string ChapterTitle { get; set; }

        [JsonProperty("eid")]
        internal int EntryId { get; set; }

        [JsonProperty("pages")]
        [JsonConverter(typeof(PagesConverter))]
        internal PageDataModel[] Pages { get; set; }

        [JsonProperty("server")]
        internal int ServerId { get; set; }

        [JsonProperty("tid")]
        internal int? TranslatorId { get; set; }

        [JsonProperty("tname")]
        internal string TranslatorName { get; set; }

        [JsonProperty("uploader")]
        internal int UploaderId { get; set; }

        [JsonProperty("username")]
        internal string UploaderName { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixToDateTimeConverter))]
        internal DateTime UploadTimestamp { get; set; }

        #endregion
    }
}