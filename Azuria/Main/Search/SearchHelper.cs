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
        /// </summary>
        public enum AnimeMangaType
        {
            /// <summary>
            /// 
            /// </summary>
            AllAnime,
            /// <summary>
            /// 
            /// </summary>
            Animeseries,
            /// <summary>
            /// 
            /// </summary>
            Ova,
            /// <summary>
            /// 
            /// </summary>
            Movie,
            /// <summary>
            /// 
            /// </summary>
            AllManga,
            /// <summary>
            /// 
            /// </summary>
            Mangaseries,
            /// <summary>
            /// 
            /// </summary>
            OneShot
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
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static async Task<ProxerResult<SearchResult<T>>> Search<T>(string name, Senpai senpai) where T : ISearchableObject
        {
            if (typeof(T) == typeof(IAnimeMangaObject) || (typeof(T).HasParameterlessConstructor() && Activator.CreateInstance(typeof(T), true) is IAnimeMangaObject))
            {
                SearchResult<T> lSearchResultObject = new SearchResult<T>("search?s=search&name=" + name, senpai);
                ProxerResult<IEnumerable<T>> lGetSearchResult = await lSearchResultObject.getNextSearchResults();
                return lGetSearchResult.Success ? new ProxerResult<SearchResult<T>>(lSearchResultObject) 
                    : new ProxerResult<SearchResult<T>>(lGetSearchResult.Exceptions);
            }
            else if (typeof(T) == typeof(Azuria.User))
            {
                SearchResult<T> lSearchResultObject = new SearchResult<T>("users?search=" + name, senpai);
                ProxerResult<IEnumerable<T>> lGetSearchResult = await lSearchResultObject.getNextSearchResults();
                return lGetSearchResult.Success ? new ProxerResult<SearchResult<T>>(lSearchResultObject)
                    : new ProxerResult<SearchResult<T>>(lGetSearchResult.Exceptions);
            }

            return new ProxerResult<SearchResult<T>>(new Exception[0]);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="senpai"></param>
        /// <param name="genreContains"></param>
        /// <param name="genreExcludes"></param>
        /// <param name="fskContains"></param>
        /// <param name="sprache"></param>
        /// <param name="sort"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static async Task<ProxerResult<SearchResult<T>>> SearchAnimeManga<T>(string name, Senpai senpai,
            AnimeMangaType? type = null, IEnumerable<GenreObject> genreContains = null,
            IEnumerable<GenreObject> genreExcludes = null, IEnumerable<Fsk> fskContains = null,
            Language? sprache = null, SortAnimeManga? sort = null) where T : IAnimeMangaObject
        {
            if (string.IsNullOrEmpty(name))
                return new ProxerResult<SearchResult<T>>(new Exception[] { new ArgumentNullException(nameof(name)) });

            string lType = type == null ? "all" : SearchUtility.AnimeMangaTypeToString.ContainsKey(type.Value)
                                                    ? SearchUtility.AnimeMangaTypeToString[type.Value] : "all";
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

            SearchResult<T> lSearchResultObject
                    = new SearchResult<T>("search?s=search&name=" + name + "&sprache=" + lLanguage
                                    + "&genre=" + lGenreContains + "&nogenre=" + lGenreExludes
                                    + "&fsk=" + lFskContains + "&sort=" + lSortAnime + "&typ=" + lType, senpai);
            ProxerResult<IEnumerable<T>> lGetSearchResult = await lSearchResultObject.getNextSearchResults();
            if (!lGetSearchResult.Success) return new ProxerResult<SearchResult<T>>(lGetSearchResult.Exceptions);

            return new ProxerResult<SearchResult<T>>(lSearchResultObject);
        }
    }
}
