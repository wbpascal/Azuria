using System;
using Azuria.Api.v1.Converters;
using Azuria.Api.v1.Converters.Info;
using Azuria.Enums.Info;
using Newtonsoft.Json;

#pragma warning disable 1591

namespace Azuria.Api.v1.DataModels.Info
{
    /// <summary>
    /// 
    /// </summary>
    public class CharacterInfoDataModel : DataModelBase
    {
        [JsonProperty("birthday")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? Birthday { get; set; }

        [JsonProperty("bloodtype")] public string Bloodtype { get; set; }

        [JsonProperty("description")] public DescriptionDataModel[] Description { get; set; }

        [JsonProperty("eye_color")] public string EyeColorHex { get; set; }

        [JsonProperty("gender")]
        [JsonConverter(typeof(CharacterGenderConverter), null)]
        public CharacterGender? Gender { get; set; }

        [JsonProperty("hair_color")] public string HairColorHex { get; set; }

        [JsonProperty("height")] public int? Height { get; set; }

        [JsonProperty("id")] public int Id { get; set; }

        public Uri Image => new Uri(ApiConstants.ProxerCdnUrl + $"/character/{this.Id}.jpg");

        [JsonProperty("links")] public LinkDataModel[] Links { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("names")] public NameDataModel[] Names { get; set; }

        [JsonProperty("persons")] public PersonDataModel[] Persons { get; set; }

        [JsonProperty("weight")] public int? Weight { get; set; }

        #region Subclasses

        /// <summary>
        /// 
        /// </summary>
        public class LinkDataModel : DataModelBase
        {
            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("eid")]
            public int EntryId { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("name")]
            public string EntryName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty("type")]
            [JsonConverter(typeof(CharacterRoleConverter))]
            public CharacterRole Role { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class PersonDataModel : Info.PersonDataModel
        {
            [JsonProperty("language")]
            [JsonConverter(typeof(LanguageConverter))]
            public Language Language { get; set; }
        }

        #endregion
    }
}