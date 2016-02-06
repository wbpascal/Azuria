using Azuria.Main;
using Azuria.Main.Search;
using System.Collections.Generic;

namespace Azuria.Utilities
{
    internal class SearchUtility
    {
        internal static Dictionary<SearchHelper.AnimeMangaType, string> AnimeMangaTypeToString => new Dictionary<SearchHelper.AnimeMangaType, string>
        {
            {SearchHelper.AnimeMangaType.AllManga, "all-manga"},
            {SearchHelper.AnimeMangaType.Mangaseries, "mangaseries"},
            {SearchHelper.AnimeMangaType.OneShot, "oneshot"},
            {SearchHelper.AnimeMangaType.Animeseries, "animeseries"},
            {SearchHelper.AnimeMangaType.Movie, "movie"},
            {SearchHelper.AnimeMangaType.AllAnime, "all-anime"},
            {SearchHelper.AnimeMangaType.Ova, "ova"}
        };

        internal static Dictionary<SearchHelper.SortAnimeManga, string> SortAnimeMangaToString => new Dictionary<SearchHelper.SortAnimeManga, string>
        {
            {SearchHelper.SortAnimeManga.Bewertung, "rating"},
            {SearchHelper.SortAnimeManga.EpisodenZahl, "count"},
            {SearchHelper.SortAnimeManga.Name, "name"},
            {SearchHelper.SortAnimeManga.Relevanz, "relevance"},
            {SearchHelper.SortAnimeManga.Zugriffe, "clicks"},
        };
    }
}
