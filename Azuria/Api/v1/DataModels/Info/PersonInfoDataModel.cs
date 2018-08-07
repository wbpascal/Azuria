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
    public class PersonInfoDataModel : DataModelBase
    {
        [JsonProperty("birthday")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Birthday { get; set; }
        
        [JsonProperty("birthplace")]
        public string Birthplace { get; set; }
        
        [JsonProperty("bloodtype")]
        public string Bloodtype { get; set; }
        
        [JsonProperty("characters")]
        public CharacterDataModel[] Characters { get; set; }
        
        [JsonProperty("description")]
        public DescriptionDataModel[] Description { get; set; }
        
        [JsonProperty("entries")]
        public EntryDataModel[] Entries { get; set; }
        
        [JsonProperty("gender")]
        [JsonConverter(typeof(CharacterGenderConverter))]
        public CharacterGender Gender { get; set; }
        
        [JsonProperty("id")]
        public int Id { get; set; }

        public Uri Image => new Uri(ApiConstants.ProxerCdnUrl + $"/person/{this.Id}.jpg");
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("names")]
        public NameDataModel[] Names { get; set; }
        
        [JsonProperty("nationality")]
        public string Nationality { get; set; }
        
        [JsonProperty("occupations")]
        public Occupation[] Occupations { get; set; }
        
        [JsonProperty("residence")]
        public string Residence { get; set; }
        
        [JsonProperty("sites")]
        public SiteDataModel[] Sites { get; set; }

        #region Subclasses
        
        public class CharacterDataModel : Info.CharacterDataModel
        {
            [JsonProperty("cid")]
            public new int Id { get; set; }
        
            [JsonProperty("language")]
            [JsonConverter(typeof(LanguageConverter))]
            public Language Language { get; set; }
        }
        
        public class EntryDataModel : IDataModel
        {
            [JsonProperty("eid")]
            public int EntryId { get; set; }
        
            [JsonProperty("name")]
            public string EntryName { get; set; }
        
            [JsonProperty("type")]
            [JsonConverter(typeof(PersonTypeConverter))]
            public PersonType Type { get; set; }
        }

        public class Occupation : DataModelBase
        {
            [JsonProperty("type")]
            [JsonConverter(typeof(OccupationTypeConverter))]
            public Enums.Info.Occupation Type { get; set; }
        }

        public class SiteDataModel : DataModelBase
        {
            [JsonProperty("link")]
            internal string LinkText { get; set; }

            public Uri Link => new Uri(this.LinkText);
            
            [JsonProperty("type")]
            public string Type { get; set; }
        }

        #endregion
    }
}