﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Main.Minor;
using Azuria.Utilities;
using Azuria.Utilities.Net;
using HtmlAgilityPack;

namespace Azuria.Main.Search
{
    /// <summary>
    ///     Eine Klasse, die die Resultate einer Suche auf Proxer zurückgibt.
    /// </summary>
    public class SearchResult<T> where T : ISearchableObject
    {
        private readonly string _link;
        private readonly Senpai _senpai;
        private int _curSite = 1;

        internal SearchResult(string link, Senpai senpai)
        {
            this._link = link;
            this._senpai = senpai;
        }

        #region Properties

        /// <summary>
        ///     Gibt zurück, ob die Suche vollendet ist, das heißt, dass keine weiteren Suchergebnisse mehr geladen werden können.
        /// </summary>
        public bool SearchFinished { get; private set; }

        /// <summary>
        ///     Gibt die Suchergebnisse zurück.
        /// </summary>
        public IEnumerable<T> SearchResults { get; private set; } = new List<T>();

        #endregion

        #region

        /// <summary>
        ///     Läd die nächste Seite der Suchergebnisse und fügt sie <see cref="SearchResult{T}.SearchResults" /> hinzu.
        /// </summary>
        /// <returns>Die Suchergebnisse der nächsten Seite.</returns>
        public async Task<ProxerResult<IEnumerable<T>>> GetNextSearchResults()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "https://proxer.me/" + this._link + "&format=raw&p=" + this._curSite,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult<IEnumerable<T>>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);
                HtmlNode lNode = lDocument.GetElementbyId("box-table-a");
                if (lNode == null)
                {
                    this.SearchFinished = true;
                    return new ProxerResult<IEnumerable<T>>(new T[0]);
                }
                lNode.ChildNodes.RemoveAt(0);

                List<T> lSearchResults = new List<T>();
                foreach (HtmlNode childNode in lNode.ChildNodes)
                {
                    if (typeof (T) == typeof (IAnimeMangaObject) ||
                        (typeof (T).HasParameterlessConstructor() &&
                         Activator.CreateInstance(typeof (T), true) is IAnimeMangaObject))
                    {
                        IAnimeMangaObject lAnimeMangaObject = this.GetSearchResultObjectAnimeManga(childNode);
                        if (lAnimeMangaObject != null) lSearchResults.Add((T) lAnimeMangaObject);
                    }
                    else if (typeof (T) == typeof (Azuria.User))
                    {
                        Azuria.User lUserObject = this.GetSearchResultObjectUser(childNode);
                        if (lUserObject != null) lSearchResults.Add((T) Convert.ChangeType(lUserObject, typeof (T)));
                    }
                }
                List<T> lCopyList = this.SearchResults.ToList();
                lCopyList.AddRange(lSearchResults);
                this.SearchResults = lCopyList;

                this._curSite++;
                return new ProxerResult<IEnumerable<T>>(lSearchResults);
            }
            catch
            {
                return new ProxerResult<IEnumerable<T>>(ErrorHandler.HandleError(this._senpai, lResponse).Exceptions);
            }
        }

        private IAnimeMangaObject GetSearchResultObjectAnimeManga(HtmlNode node)
        {
            int lId = node.Attributes.Contains("class")
                ? Convert.ToInt32(node.Attributes["class"].Value.Substring("entry".Length))
                : -1;
            string lName = node.ChildNodes[1].InnerText;
            List<GenreObject> lGenreList = new List<GenreObject>();
            foreach (string curGenre in node.ChildNodes[2].InnerText.Split(' '))
            {
                lGenreList.Add(new GenreObject(curGenre));
            }
            AnimeMangaStatus lStatus = AnimeMangaStatus.Unknown;
            if (node.FirstChild.FirstChild.Attributes.Contains("title"))
            {
                switch (node.FirstChild.FirstChild.Attributes["title"].Value)
                {
                    case "Abgeschlossen":
                        lStatus = AnimeMangaStatus.Completed;
                        break;
                    case "Nicht erschienen (Pre-Airing)":
                        lStatus = AnimeMangaStatus.PreAiring;
                        break;
                    case "Airing":
                        lStatus = AnimeMangaStatus.Airing;
                        break;
                    case "Abgebrochen":
                        lStatus = AnimeMangaStatus.Canceled;
                        break;
                }
            }
            Type lType = typeof (object);
            Anime.AnimeType lAnimeType = Anime.AnimeType.Unbekannt;
            Manga.MangaType lMangaType = Manga.MangaType.Unbekannt;
            switch (node.ChildNodes[3].InnerText)
            {
                case "Animeserie":
                    lType = typeof (Anime);
                    lAnimeType = Anime.AnimeType.Series;
                    break;
                case "OVA":
                    lType = typeof (Anime);
                    lAnimeType = Anime.AnimeType.Ova;
                    break;
                case "Movie":
                    lType = typeof (Anime);
                    lAnimeType = Anime.AnimeType.Movie;
                    break;
                case "Mangaserie":
                    lType = typeof (Manga);
                    lMangaType = Manga.MangaType.Series;
                    break;
                case "One-Shot":
                    lType = typeof (Manga);
                    lMangaType = Manga.MangaType.OneShot;
                    break;
            }

            if ((typeof (T) == typeof (Anime) || typeof (T) == typeof (IAnimeMangaObject)) && lType == typeof (Anime))
                return new Anime(lName, lId, this._senpai, lGenreList, lStatus, lAnimeType);
            if ((typeof (T) == typeof (Manga) || typeof (T) == typeof (IAnimeMangaObject)) && lType == typeof (Manga))
                return new Manga(lName, lId, this._senpai, lGenreList, lStatus, lMangaType);
            return null;
        }

        private Azuria.User GetSearchResultObjectUser(HtmlNode node)
        {
            Uri lAvatar = node.FirstChild.FirstChild.Attributes.Contains("src")
                ? new Uri("https:" + node.FirstChild.FirstChild.Attributes["src"].Value)
                : null;
            string lUsername = node.ChildNodes[1].InnerText;
            int lUserId;
            int lPunkte;
            if (!(node.ChildNodes[1].FirstChild.Attributes.Contains("href")
                  &&
                  int.TryParse(
                      node.ChildNodes[1].FirstChild.Attributes["href"].Value.GetTagContents("/user/", "#top").First(),
                      out lUserId))
                || !int.TryParse(node.ChildNodes[7].InnerText, out lPunkte)) return null;

            return new Azuria.User(lUsername, lUserId, lAvatar, lPunkte, this._senpai);
        }

        #endregion
    }
}