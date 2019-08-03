using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.Input.Converter;
using Azuria.Enums.Info;
using Azuria.Enums.List;
using Azuria.Helpers.Attributes;
using Azuria.Helpers.Extensions;

namespace Azuria.Api.v1.Input.List
{
    /// <summary>
    /// </summary>
    public class SearchInput : PagedInputDataModel
    {
        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("tagspoilerfilter", ConverterMethodName = nameof(GetTagSpoilerFilterString), Optional = true)]
        public bool? FilterSpoilerTags { get; set; } = true;

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("tagratefilter", ConverterMethodName = nameof(GetTagRateFilterString), Optional = true)]
        public bool? FilterUnratedTags { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData(
            "fsk", ConverterMethodName = nameof(FskToString), ForbiddenValues = new object[] {""},
            Optional = true)]
        public IEnumerable<Fsk> Fsk { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData(
            "nogenre", ConverterMethodName = nameof(GenresToString), ForbiddenValues = new object[] {""},
            Optional = true)]
        public IEnumerable<Genre> GenreExclude { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData(
            "genre", ConverterMethodName = nameof(GenresToString), ForbiddenValues = new object[] {""},
            Optional = true)]
        public IEnumerable<Genre> GenreInclude { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("language", Converter = typeof(ToShortStringConverter), Optional = true)]
        public Language? Language { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("length", Optional = true)]
        public int? Length { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("length-limit", Converter = typeof(ToLowerConverter), Optional = true)]
        public LengthLimit? LengthLimit { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("name", Optional = true)]
        public string Name { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("sort", Converter = typeof(ToLowerConverter), Optional = true)]
        public SearchResultSort? Sort { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("notags", Converter = typeof(SpaceSeparatedEnumerationConverter), Optional = true)]
        public IEnumerable<int> TagsExclude { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("tags", Converter = typeof(SpaceSeparatedEnumerationConverter), Optional = true)]
        public IEnumerable<int> TagsInclude { get; set; }

        /// <summary>
        /// Optional, if omitted (or null) the default value of the api method will be used.
        /// </summary>
        [InputData("type", ConverterMethodName = nameof(TypeToString), Optional = true)]
        public SearchMediaType? Type { get; set; }

        private static string FskToString(IEnumerable<Fsk> fskTypes)
        {
            return fskTypes?.Aggregate(string.Empty, (s, fsk) => string.Concat(s, fsk.GetDescription(), " ")).Trim();
        }

        private static string GenresToString(IEnumerable<Genre> genres)
        {
            return genres?.Aggregate(
                string.Empty, (s, genre) => string.Concat(s, genre.GetDescription(), " ")
            ).Trim();
        }

        private static string GetTagSpoilerFilterString(bool? filtered)
        {
            switch (filtered)
            {
                case false:
                    return "spoiler_0";
                case true:
                    return "spoiler_1";
                default:
                    return "spoiler_10";
            }
        }

        private static string GetTagRateFilterString(bool filterUnratedTags)
            => filterUnratedTags ? "rate_1" : "rate_10";

        private static string TypeToString(SearchMediaType type)
        {
            switch (type)
            {
                case SearchMediaType.AllAnime:
                    return "all-anime";
                case SearchMediaType.AllManga:
                    return "all-manga";
                default:
                    return type.ToString().ToLowerInvariant();
            }
        }
    }
}