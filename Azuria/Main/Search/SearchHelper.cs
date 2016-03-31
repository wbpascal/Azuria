using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Main.Minor;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Main.Search
{
    /// <summary>
    ///     Eine Klasse, mithilfe dieser eine Suche auf Proxer ausgeführt werden kann.
    /// </summary>
    public class SearchHelper
    {
        /// <summary>
        ///     Eine Enumeration, die den Typen des <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> darstellt,
        ///     nach dem gesucht wird.
        /// </summary>
        public enum AnimeMangaType
        {
            /// <summary>
            ///     Stellt alle <see cref="Anime">Anime-</see> und <see cref="Manga">Manga-</see>Typen dar.
            /// </summary>
            All,

            /// <summary>
            ///     Stellt alle <see cref="Anime">Anime-</see>Typen dar.
            /// </summary>
            AllAnime,

            /// <summary>
            ///     Stellt eine <see cref="Anime">Anime</see>serie dar.
            /// </summary>
            Animeseries,

            /// <summary>
            ///     Stellt eine OVA oder ein Special eines <see cref="Anime">Anime</see> dar.
            /// </summary>
            Ova,

            /// <summary>
            ///     Stellt einen Film eines <see cref="Anime">Anime</see> dar.
            /// </summary>
            Movie,

            /// <summary>
            ///     Stellt alle <see cref="Manga">Manga-</see>Typen dar.
            /// </summary>
            AllManga,

            /// <summary>
            ///     Stellt eine <see cref="Manga">Manga</see>serie dar.
            /// </summary>
            Mangaseries,

            /// <summary>
            ///     Stellt einen One-Shot <see cref="Manga">Manga</see> dar.
            /// </summary>
            OneShot
        }

        /// <summary>
        ///     Eine Enumeration, die die Reihenfolge darstellt, in der die Objekte bei
        ///     einer <see cref="Anime">Anime-</see> oder <see cref="Manga">Manga-</see>Suche zurückgegeben werden.
        /// </summary>
        public enum SortAnimeManga
        {
            /// <summary>
            ///     Stellt die Sortierung nach Relevanz dar.
            /// </summary>
            Relevance,

            /// <summary>
            ///     Stellt die Sortierung nach Namen dar.
            /// </summary>
            Name,

            /// <summary>
            ///     Stellt die Sortierung nach Bewertung dar.
            /// </summary>
            Rating,

            /// <summary>
            ///     Stellt die Sortierung nach Zugriffen dar.
            /// </summary>
            Hits,

            /// <summary>
            ///     Stellt die Sortierung nach Episodenanzahl dar.
            /// </summary>
            EpisodeCount
        }

        #region

        /// <summary>
        ///     Gibt die Ergebnisse einer Proxer-Suche zurück.
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <exception cref="ArgumentNullException">
        ///     Wird ausgelöst, wenn <paramref name="name" /> null (oder Nothing in Visual
        ///     Basic) ist.
        /// </exception>
        /// <typeparam name="T">Ein Typ, der von <see cref="ISearchableObject" /> erbt.</typeparam>
        /// <param name="name">Der String, nachdem gesucht werden soll.</param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        [ItemNotNull]
        public static async Task<ProxerResult<SearchResult<T>>> Search<T>([NotNull] string name, [NotNull] Senpai senpai)
            where T : ISearchableObject
        {
            if (string.IsNullOrEmpty(name))
                return new ProxerResult<SearchResult<T>>(new Exception[] {new ArgumentNullException(nameof(name))});

            if (typeof (T) == typeof (IAnimeMangaObject) ||
                (typeof (T).HasParameterlessConstructor() &&
                 Activator.CreateInstance(typeof (T), true) is IAnimeMangaObject))
            {
                SearchResult<T> lSearchResultObject = new SearchResult<T>("search?s=search&name=" + name, senpai);
                ProxerResult<IEnumerable<T>> lGetSearchResult = await lSearchResultObject.GetNextSearchResults();
                return lGetSearchResult.Success
                    ? new ProxerResult<SearchResult<T>>(lSearchResultObject)
                    : new ProxerResult<SearchResult<T>>(lGetSearchResult.Exceptions);
            }
            if (typeof (T) == typeof (Azuria.User))
            {
                SearchResult<T> lSearchResultObject = new SearchResult<T>("users?search=" + name, senpai);
                ProxerResult<IEnumerable<T>> lGetSearchResult = await lSearchResultObject.GetNextSearchResults();
                return lGetSearchResult.Success
                    ? new ProxerResult<SearchResult<T>>(lSearchResultObject)
                    : new ProxerResult<SearchResult<T>>(lGetSearchResult.Exceptions);
            }

            return new ProxerResult<SearchResult<T>>(new Exception[0]);
        }

        /// <summary>
        ///     Gibt die Ergebnisse einer <see cref="Anime">Anime-</see> oder <see cref="Manga">Manga-</see>Suche auf Proxer
        ///     zurück.
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <exception cref="ArgumentNullException">
        ///     Wird ausgelöst, wenn <paramref name="name" /> null (Nothing in Visual Basic)
        ///     oder leer ist.
        /// </exception>
        /// <typeparam name="T">Ein Typ, der von <see cref="IAnimeMangaObject" /> erbt.</typeparam>
        /// <param name="name">Der String, nach dem gesucht werden soll.</param>
        /// <param name="senpai">Der Benutzer, der die Suche ausführt</param>
        /// <param name="genreContains">Alle <see cref="GenreObject">Genre</see>, die die Suchergebnisse enthalten sollen.</param>
        /// <param name="genreExcludes">
        ///     Alle <see cref="GenreObject">Genre</see>, die aus den Suchergebnissen ausgeschlossen werden
        ///     sollen.
        /// </param>
        /// <param name="fskContains">Alle <see cref="Fsk">Fsk-</see>Kategorien, die die Suchergebnisse enthalten sollen.</param>
        /// <param name="sprache">Die <see cref="Language">Sprache</see>, in der die Suchergebnisse verfügbar sein sollen.</param>
        /// <param name="sort">Die Reihenfolge, in der Suchergebnisse zurückgegeben werden sollen.</param>
        /// <param name="type"></param>
        /// <returns>Eine Auflistung aller Suchergebnisse, vom Typ <typeparamref name="T" /></returns>
        [ItemNotNull]
        public static async Task<ProxerResult<SearchResult<T>>> SearchAnimeManga<T>([NotNull] string name,
            [NotNull] Senpai senpai,
            AnimeMangaType? type = null, IEnumerable<GenreObject> genreContains = null,
            IEnumerable<GenreObject> genreExcludes = null, IEnumerable<Fsk> fskContains = null,
            Language? sprache = null, SortAnimeManga? sort = null) where T : IAnimeMangaObject
        {
            if (string.IsNullOrEmpty(name))
                return new ProxerResult<SearchResult<T>>(new Exception[] {new ArgumentNullException(nameof(name))});

            string lType = type == null
                ? "all"
                : SearchUtility.AnimeMangaTypeToString.ContainsKey(type.Value)
                    ? SearchUtility.AnimeMangaTypeToString[type.Value]
                    : "all";
            string lLanguage = sprache == null ? "alle" : sprache.Value == Language.German ? "de" : "en";
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
            string lSortAnime = sort == null
                ? ""
                : SearchUtility.SortAnimeMangaToString.ContainsKey(sort.Value)
                    ? SearchUtility.SortAnimeMangaToString[sort.Value]
                    : "";

            SearchResult<T> lSearchResultObject
                = new SearchResult<T>("search?s=search&name=" + name + "&sprache=" + lLanguage
                                      + "&genre=" + lGenreContains + "&nogenre=" + lGenreExludes
                                      + "&fsk=" + lFskContains + "&sort=" + lSortAnime + "&typ=" + lType, senpai);
            ProxerResult<IEnumerable<T>> lGetSearchResult = await lSearchResultObject.GetNextSearchResults();

            return !lGetSearchResult.Success
                ? new ProxerResult<SearchResult<T>>(lGetSearchResult.Exceptions)
                : new ProxerResult<SearchResult<T>>(lSearchResultObject);
        }

        #endregion
    }
}