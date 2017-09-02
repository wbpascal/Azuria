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
    public class SearchInput : InputDataModel
    {
        /// <summary>
        /// </summary>
        [InputData("tagspoilerfilter", ConverterMethodName = nameof(GetTagSpoilerFilterString), Optional = true)]
        public bool? FilterSpoilerTags { get; set; } = true;

        /// <summary>
        /// </summary>
        [InputData("tagratefilter", ConverterMethodName = nameof(GetTagRateFilterString), Optional = true)]
        public bool FilterUnratedTags { get; set; } = true;

        /// <summary>
        /// </summary>
        [InputData(
            "fsk", ConverterMethodName = nameof(FskToString), ForbiddenValues = new object[] {""},
            Optional = true)]
        public IEnumerable<Fsk> Fsk { get; set; }

        /// <summary>
        /// </summary>
        [InputData(
            "nogenre", ConverterMethodName = nameof(GenresToString), ForbiddenValues = new object[] {""},
            Optional = true)]
        public IEnumerable<Genre> GenreExclude { get; set; }

        /// <summary>
        /// </summary>
        [InputData(
            "genre", ConverterMethodName = nameof(GenresToString), ForbiddenValues = new object[] {""},
            Optional = true)]
        public IEnumerable<Genre> GenreInclude { get; set; }

        /// <summary>
        /// </summary>
        [InputData("language", Converter = typeof(ToShortStringConverter), Optional = true)]
        public Language? Language { get; set; } = null;

        /// <summary>
        /// </summary>
        [InputData("length", Optional = true)]
        public int? Length { get; set; }

        /// <summary>
        /// </summary>
        [InputData("length-limit", Converter = typeof(ToLowerConverter), Optional = true)]
        public LengthLimit LengthLimit { get; set; } = LengthLimit.Down;

        /// <summary>
        /// </summary>
        [InputData("name", Optional = true)]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [InputData("sort", Converter = typeof(ToLowerConverter), Optional = true)]
        public SearchResultSort Sort { get; set; } = SearchResultSort.Relevance;

        /// <summary>
        /// </summary>
        [InputData("notags", Converter = typeof(SpaceSeparatedEnumerationConverter), Optional = true)]
        public IEnumerable<int> TagsExclude { get; set; }

        /// <summary>
        /// </summary>
        [InputData("tags", Converter = typeof(SpaceSeparatedEnumerationConverter), Optional = true)]
        public IEnumerable<int> TagsInclude { get; set; }

        /// <summary>
        /// </summary>
        [InputData("type", ConverterMethodName = nameof(TypeToString), Optional = true)]
        public SearchMediaType Type { get; set; } = SearchMediaType.All;

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