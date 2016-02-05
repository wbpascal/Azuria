using Azuria.Main;
using Azuria.Main.Search;
using System.Collections.Generic;

namespace Azuria.Utilities
{
    internal class SearchUtility
    {
        internal static Dictionary<Anime.AnimeType, string> AnimeTypeToString => new Dictionary<Anime.AnimeType, string> 
        {
            {Anime.AnimeType.Series, "animeseries"},
            {Anime.AnimeType.Movie, "movie"},
            {Anime.AnimeType.Ova, "ova"}
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
