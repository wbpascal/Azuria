using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Main.Minor;
using Azuria.Utilities;

namespace Azuria.Main.Search
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public enum SortAnimeManga
        {
            /// <summary>
            /// 
            /// </summary>
            Relevanz,
            /// <summary>
            /// 
            /// </summary>
            Name,
            /// <summary>
            /// 
            /// </summary>
            Bewertung,
            /// <summary>
            /// 
            /// </summary>
            Zugriffe,
            /// <summary>
            /// 
            /// </summary>
            EpisodenZahl
        }

        /// <summary>
        ///     
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="ArgumentNullException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn <paramref name="senpai" /> null (oder Nothing in Visual Basic) ist.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="name">Die Name des <see cref="Main.Anime">Anime</see>, nach dem gesucht werden soll.</param>
        /// <param name="type"></param>
        /// <param name="sprache"></param>
        /// <param name="genreContains"></param>
        /// <param name="genreExcludes"></param>
        /// <param name="fskContains"></param>
        /// <param name="sort"></param>
        /// <param name="senpai">Der Benutzer. (Muss eingeloggt sein)</param>
        /// <returns>Anime oder Manga der ID (Typecast erforderlich)</returns>
        public static async Task<ProxerResult<SearchResult<Anime>>> SearchAnime(string name, Senpai senpai,
            Anime.AnimeType? type = null, IEnumerable<GenreObject> genreContains = null,
            IEnumerable<GenreObject> genreExcludes = null, IEnumerable<Fsk> fskContains = null,
            Language? sprache = null, SortAnimeManga? sort = null)
        {
            if (string.IsNullOrEmpty(name))
                return new ProxerResult<SearchResult<Anime>>(new Exception[] { new ArgumentNullException(nameof(name)) });

            string lAnimeType = type == null ? "all-anime" : SearchUtility.AnimeTypeToString[type.Value];
            string lLanguage = sprache == null ? "alle" : sprache.Value == Language.Deutsch ? "de" : "en";
            string lGenreContains = "";
            if (genreContains != null)
            {
                foreach (GenreObject curGenre in genreContains)
                {
                    lGenreContains += curGenre.Genre + "+";
                }
            }
            string lGenreExludes = "";
            if (genreExcludes != null)
            {
                foreach (GenreObject curGenre in genreExcludes)
                {
                    lGenreExludes += curGenre.Genre + "+";
                }
            }
            string lFskContains = "";
            if (fskContains != null)
            {
                foreach (Fsk curFsk in fskContains)
                {
                    if (FskHelper.FskToStringDictionary.ContainsKey(curFsk))
                        lFskContains += FskHelper.FskToStringDictionary[curFsk] + "+";
                }
            }
            string lSortAnime = sort == null ? "" : SearchUtility.SortAnimeMangaToString.ContainsKey(sort.Value)
                                                    ? SearchUtility.SortAnimeMangaToString[sort.Value] : "";

            SearchResult<Anime> lSearchResultObject 
                    = new SearchResult<Anime>("search?s=search&name=" 
                                    + name + "&sprache=" + lLanguage + "&typ=" + lAnimeType
                                    + "&genre=" + lGenreContains + "&nogenre=" + lGenreExludes
                                    + "&fsk=" + lFskContains + "&sort=" + lSortAnime, senpai);
            ProxerResult<IEnumerable<Anime>> lGetSearchResult = await lSearchResultObject.getNextSearchResults();
            if (!lGetSearchResult.Success) return new ProxerResult<SearchResult<Anime>>(lGetSearchResult.Exceptions);

            return new ProxerResult<SearchResult<Anime>>(lSearchResultObject);
        }
    }
}
