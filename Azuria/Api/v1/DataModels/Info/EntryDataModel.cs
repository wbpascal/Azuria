using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Info;
using Azuria.Enums;
using Azuria.Enums.Info;
using Newtonsoft.Json;

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// </summary>
    public class EntryDataModel : DataModelBase, IEntryInfoDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("adaption_type")]
        [JsonConverter(typeof(AdaptionTypeConverter))]
        public AdaptionType? AdaptionType { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("adaption_value")]
        public string AdaptionValue { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("adaption_data")]
        public AdaptionDataModel AdaptionData { get; set; }
        
        /// <summary>
        /// </summary>
        [JsonProperty("clicks")]
        public int Clicks { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("count")]
        public int ContentCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <inheritdoc />
        [JsonProperty("id")]
        public int EntryId { get; set; }

        /// <inheritdoc />
        [JsonProperty("medium")]
        public MediaMedium EntryMedium { get; set; }

        /// <inheritdoc />
        [JsonProperty("name")]
        public string EntryName { get; set; }

        /// <inheritdoc />
        [JsonProperty("kat")]
        public MediaEntryType EntryType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("fsk")]
        [JsonConverter(typeof(FskConverter))]
        public Fsk[] Fsk { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("genre")]
        [JsonConverter(typeof(GenreConverter))]
        public Genre[] Genre { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("license")]
        [JsonConverter(typeof(IsLicensedConverter))]
        public bool? IsLicensed { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rate_count")]
        public int RatingsCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rate_sum")]
        public int RatingsSum { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("state")]
        public MediaStatus Status { get; set; }
    }
}