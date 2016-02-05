﻿using Azuria.ErrorHandling;
using Azuria.Main.Minor;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azuria.Main.Search
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchResult<T> where T : ISearchableObject
    {
        private readonly Senpai _senpai;
        private readonly string _link;
        private int _curSite = 1;

        internal SearchResult(string link, Senpai senpai)
        {
            this._link = link;
            this._senpai = senpai;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> SearchResults { get; private set; } = new List<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ProxerResult<IEnumerable<T>>> getNextSearchResults()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "https://proxer.me/search?s=search" + this._link + "&format=raw&p=" + this._curSite,
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
                lNode.ChildNodes.RemoveAt(0);

                List<T> lSearchResults = new List<T>();
                foreach (HtmlNode childNode in lNode.ChildNodes)
                {
                    T lResultObject = (T)Activator.CreateInstance(typeof(T), true);
                    if (lResultObject is IAnimeMangaObject)
                    {
                        lSearchResults.Add((T)this.getSearchResultObjectAnimeManga(childNode, (IAnimeMangaObject)lResultObject));
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

        private IAnimeMangaObject getSearchResultObjectAnimeManga(HtmlNode node, IAnimeMangaObject containerObject)
        {
            int lId = node.Attributes.Contains("class")
                ? Convert.ToInt32(node.Attributes["class"].Value.Substring("entry".Length)) : -1;
            string lName = node.ChildNodes[1].InnerText;
            List<GenreObject> lGenreList = new List<GenreObject>();
            foreach (string curGenre in node.ChildNodes[2].InnerText.Split(' '))
            {
                lGenreList.Add(new GenreObject(curGenre));
            }
            AnimeMangaStatus lStatus = AnimeMangaStatus.Unbekannt;
            if (node.FirstChild.FirstChild.Attributes.Contains("title"))
            {
                switch (node.FirstChild.FirstChild.Attributes["title"].Value)
                {
                    case "Abgeschlossen":
                        lStatus = AnimeMangaStatus.Abgeschlossen;
                        break;
                    case "Nicht erschienen (Pre-Airing)":
                        lStatus = AnimeMangaStatus.PreAiring;
                        break;
                    case "Airing":
                        lStatus = AnimeMangaStatus.Airing;
                        break;
                    case "Abgebrochen":
                        lStatus = AnimeMangaStatus.Abgebrochen;
                        break;
                }
            }
            return containerObject is Anime ? new Anime(lName, lId, this._senpai, lGenreList, lStatus) : null;
        }
    }
}
