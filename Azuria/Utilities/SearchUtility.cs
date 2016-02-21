using System.Collections.Generic;
using Azuria.Main.Search;

namespace Azuria.Utilities
{
    internal class SearchUtility
    {
        #region Properties

        internal static Dictionary<SearchHelper.AnimeMangaType, string> AnimeMangaTypeToString
            => new Dictionary<SearchHelper.AnimeMangaType, string>
            {
                {SearchHelper.AnimeMangaType.AllManga, "all-manga"},
                {SearchHelper.AnimeMangaType.Mangaseries, "mangaseries"},
                {SearchHelper.AnimeMangaType.OneShot, "oneshot"},
                {SearchHelper.AnimeMangaType.Animeseries, "animeseries"},
                {SearchHelper.AnimeMangaType.Movie, "movie"},
                {SearchHelper.AnimeMangaType.AllAnime, "all-anime"},
                {SearchHelper.AnimeMangaType.Ova, "ova"}
            };

        internal static Dictionary<SearchHelper.SortAnimeManga, string> SortAnimeMangaToString
            => new Dictionary<SearchHelper.SortAnimeManga, string>
            {
                {SearchHelper.SortAnimeManga.Rating, "rating"},
                {SearchHelper.SortAnimeManga.EpisodeCount, "count"},
                {SearchHelper.SortAnimeManga.Name, "name"},
                {SearchHelper.SortAnimeManga.Relevance, "relevance"},
                {SearchHelper.SortAnimeManga.Hits, "clicks"}
            };

        #endregion
    }
}