using System.Collections.Generic;
using Azuria.Enums.Info;
using Azuria.Enums.List;

namespace Azuria.Api.v1.Input
{
    /// <summary>
    /// </summary>
    public class SearchInput
    {
        #region Properties

        /// <summary>
        /// </summary>
        public bool? FilterSpoilerTags { get; set; } = true;

        /// <summary>
        /// </summary>
        public bool FilterUnratedTags { get; set; } = true;

        /// <summary>
        /// </summary>
        public IEnumerable<Fsk> Fsk { get; set; }

        /// <summary>
        /// </summary>
        public IEnumerable<Genre> GenreExclude { get; set; }

        /// <summary>
        /// </summary>
        public IEnumerable<Genre> GenreInclude { get; set; }

        /// <summary>
        /// </summary>
        public Language? Language { get; set; } = null;

        /// <summary>
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// </summary>
        public LengthLimit LengthLimit { get; set; } = LengthLimit.Down;

        /// <summary>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        public SearchResultSort Sort { get; set; } = SearchResultSort.Relevance;

        /// <summary>
        /// </summary>
        public IEnumerable<int> TagsExclude { get; set; }

        /// <summary>
        /// </summary>
        public IEnumerable<int> TagsInclude { get; set; }

        /// <summary>
        /// </summary>
        public SearchMediaType Type { get; set; } = SearchMediaType.All;

        #endregion
    }
}