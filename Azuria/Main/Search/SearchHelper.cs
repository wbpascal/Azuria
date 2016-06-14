using System;
using System.Collections.Generic;
using Azuria.Main.Minor;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using JetBrains.Annotations;

namespace Azuria.Main.Search
{
    /// <summary>
    ///     Represents a class with the help of which a search can be executed.
    /// </summary>
    public class SearchHelper
    {
        /// <summary>
        ///     An enumeration which describes the type of the <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> that
        ///     is being searched for.
        /// </summary>
        public enum AnimeMangaType
        {
            /// <summary>
            ///     Represents all <see cref="Anime">Anime-</see> and <see cref="Manga">Manga</see>types.
            /// </summary>
            All,

            /// <summary>
            ///     Represents all <see cref="Anime">Anime</see>types.
            /// </summary>
            AllAnime,

            /// <summary>
            ///     Represents an <see cref="Anime">Anime</see>series dar.
            /// </summary>
            Animeseries,

            /// <summary>
            ///     Represents an OVA or a special of an <see cref="Anime">Anime</see>.
            /// </summary>
            Ova,

            /// <summary>
            ///     Represents a movie of an <see cref="Anime">Anime</see>.
            /// </summary>
            Movie,

            /// <summary>
            ///     Represents all <see cref="Manga">Manga</see>types.
            /// </summary>
            AllManga,

            /// <summary>
            ///     Represents a <see cref="Manga">Manga</see>series.
            /// </summary>
            Mangaseries,

            /// <summary>
            ///     Represents a One-Shot <see cref="Manga">Manga</see>.
            /// </summary>
            OneShot
        }

        /// <summary>
        ///     An enumeration which describes the order in which the <see cref="Anime">Anime</see> or
        ///     <see cref="Manga">Manga</see> are being returned after a search.
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
        ///     Executes a search with the specified options.
        /// </summary>
        /// <typeparam name="T">A type that is derived from <see cref="ISearchableObject" />.</typeparam>
        /// <param name="name">The search term.</param>
        /// <param name="senpai">
        ///     The user that executes the search. He needs to be logged in if searching for an <see cref="Anime">Anime</see> or
        ///     <see cref="Manga">Manga</see>.
        /// </param>
        /// <returns>A collection of the search results.</returns>
        [NotNull]
        public static ProxerResult<SearchResult<T>> Search<T>([NotNull] string name, [NotNull] Senpai senpai)
            where T : ISearchableObject
        {
            if (string.IsNullOrEmpty(name))
                return new ProxerResult<SearchResult<T>>(new Exception[] {new ArgumentNullException(nameof(name))});

            if (typeof(T) == typeof(IAnimeMangaObject) ||
                (typeof(T).HasParameterlessConstructor() &&
                 Activator.CreateInstance(typeof(T), true) is IAnimeMangaObject))
            {
                return new ProxerResult<SearchResult<T>>(new SearchResult<T>("search?s=search&name=" + name, senpai));
            }

            return typeof(T) == typeof(Azuria.User)
                ? new ProxerResult<SearchResult<T>>(new SearchResult<T>("users?search=" + name, senpai))
                : new ProxerResult<SearchResult<T>>(new Exception[0]);
        }

        /// <summary>
        ///     Executes an <see cref="Anime">Anime-</see> or <see cref="Manga">Manga</see>search.
        /// </summary>
        /// <typeparam name="T">
        ///     A type that is derived from <see cref="IAnimeMangaObject" /> or <see cref="IAnimeMangaObject" />
        ///     itself if searching for both <see cref="Anime" /> and <see cref="Manga" />.
        /// </typeparam>
        /// <param name="name">The search term.</param>
        /// <param name="senpai">
        ///     The user that executes the search. He needs to be logged in.
        /// </param>
        /// <param name="genreContains">
        ///     The <see cref="GenreObject">genres</see> which every <see cref="Anime" /> Or
        ///     <see cref="Manga" /> has to contain.
        /// </param>
        /// <param name="genreExcludes">
        ///     The <see cref="GenreObject">genres</see> which every <see cref="Anime" /> Or <see cref="Manga" /> should NOT
        ///     contain.
        /// </param>
        /// <param name="fskContains">
        ///     The <see cref="Fsk">Fsk-</see>categories which every <see cref="Anime" /> Or
        ///     <see cref="Manga" /> has to contain.
        /// </param>
        /// <param name="sprache">
        ///     The <see cref="Language" /> that every <see cref="Anime" /> or <see cref="Manga" /> has to be
        ///     available in.
        /// </param>
        /// <param name="sort">The order in which the objects are returned.</param>
        /// <param name="type">The type of the <see cref="Anime" /> or <see cref="Manga" /> that is being searched for</param>
        /// <returns>A collection of the search results.</returns>
        [NotNull]
        public static ProxerResult<SearchResult<T>> SearchAnimeManga<T>([NotNull] string name,
            [NotNull] Senpai senpai,
            AnimeMangaType? type = null, IEnumerable<GenreObject.GenreType> genreContains = null,
            IEnumerable<GenreObject.GenreType> genreExcludes = null, IEnumerable<Fsk> fskContains = null,
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
                foreach (GenreObject.GenreType curGenre in genreContains)
                {
                    lGenreContains += curGenre + "+";
                }
                if (lGenreContains.EndsWith("+")) lGenreContains = lGenreContains.Remove(lGenreContains.Length - 1);
            }
            string lGenreExludes = "";
            if (genreExcludes != null)
            {
                foreach (GenreObject.GenreType curGenre in genreExcludes)
                {
                    lGenreExludes += curGenre + "+";
                }
                if (lGenreExludes.EndsWith("+")) lGenreExludes = lGenreExludes.Remove(lGenreExludes.Length - 1);
            }
            string lFskContains = "";
            if (fskContains != null)
            {
                foreach (Fsk curFsk in fskContains)
                {
                    if (FskHelper.FskToStringDictionary.ContainsKey(curFsk))
                        lFskContains += FskHelper.FskToStringDictionary[curFsk] + "+";
                }
                if (lFskContains.EndsWith("+")) lFskContains = lFskContains.Remove(lGenreExludes.Length - 1);
            }
            string lSortAnime = sort == null
                ? ""
                : SearchUtility.SortAnimeMangaToString.ContainsKey(sort.Value)
                    ? SearchUtility.SortAnimeMangaToString[sort.Value]
                    : "";

            return
                new ProxerResult<SearchResult<T>>(
                    new SearchResult<T>("search?s=search&name=" + name + "&sprache=" + lLanguage
                                        + "&genre=" + lGenreContains + "&nogenre=" + lGenreExludes
                                        + "&fsk=" + lFskContains + "&sort=" + lSortAnime + "&typ=" + lType, senpai));
        }

        #endregion
    }
}