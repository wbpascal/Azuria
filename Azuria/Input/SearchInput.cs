using System.Collections.Generic;
using Azuria.Enums.Info;
using Azuria.Enums.List;

namespace Azuria.Input
{
    /// <summary>
    /// </summary>
    public class SearchInput
    {
        #region Properties

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
        public bool IsFilteringSpoilerTags { get; set; }

        /// <summary>
        /// </summary>
        public bool IsFilteringUnratedTags { get; set; } = true;

        /// <summary>
        /// </summary>
        public Language Language { get; set; } = Language.Unkown;

        /// <summary>
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// </summary>
        public LengthLimit LengthLimit { get; set; } = LengthLimit.Down;

        /// <summary>
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// </summary>
        public SearchResultSort Sort { get; set; } = SearchResultSort.Relevance;

        /// <summary>
        /// </summary>
        public IEnumerable<string> TagsExclude { get; set; }

        /// <summary>
        /// </summary>
        public IEnumerable<string> TagsInclude { get; set; }

        /// <summary>
        /// </summary>
        public MediaSearchType Type { get; set; } = MediaSearchType.All;

        #endregion
    }
}