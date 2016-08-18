using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.AnimeManga.Properties;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Web;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Search
{
    /// <summary>
    /// </summary>
    public sealed class SearchResultEnumerator<T> : IEnumerator<T> where T : ISearchableObject
    {
        private readonly string _link;
        private readonly Senpai _senpai;
        private T[] _currentPageContent = new T[0];
        private int _currentPageContentIndex = -1;
        private int _nextPage = 1;
        private bool _searchFinished;

        internal SearchResultEnumerator(string link, Senpai senpai)
        {
            this._link = link;
            this._senpai = senpai;
        }

        #region Properties

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public T Current => this._currentPageContent[this._currentPageContentIndex];

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => this.Current;

        #endregion

        #region Inherited

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._currentPageContent = new T[0];
        }

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>
        ///     true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
        ///     end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            if (this._searchFinished) return false;
            if (this._currentPageContentIndex >= this._currentPageContent.Length - 1)
            {
                ProxerResult lGetSearchResult = Task.Run(this.GetNextSearchResults).Result;
                if (!lGetSearchResult.Success)
                    throw lGetSearchResult.Exceptions.FirstOrDefault() ?? new WrongResponseException();
                this._currentPageContentIndex = -1;
            }
            this._currentPageContentIndex++;
            return !this._searchFinished;
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public void Reset()
        {
            this._currentPageContent = new T[0];
            this._currentPageContentIndex = -1;
            this._nextPage = 1;
        }

        #endregion

        #region

        [ItemNotNull]
        private async Task<ProxerResult> GetNextSearchResults()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/" + this._link + "&format=raw&p=" + this._nextPage),
                        new Func<string, ProxerResult>[0], this._senpai, checkLogin: typeof(T) != typeof(User.User));

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);
                HtmlNode lNode = lDocument.GetElementbyId("box-table-a");
                if (lNode == null || !lNode.ChildNodes.Any() ||
                    lNode.ChildNodes.Count(node => node.Name.Equals("tr")) == 1)
                {
                    this._searchFinished = true;
                    return new ProxerResult();
                }
                lNode.InnerHtml = lNode.InnerHtml.Replace("\t", "").Replace("\r", "");

                List<T> lSearchResults = new List<T>();
                foreach (HtmlNode childNode in lNode.ChildNodes.Where(node => node.Name.Equals("tr")).Skip(1))
                {
                    if (typeof(T) == typeof(IAnimeMangaObject) ||
                        typeof(T).ImplementsInterface(typeof(IAnimeMangaObject)))
                    {
                        IAnimeMangaObject lAnimeMangaObject = this.GetSearchResultObjectAnimeManga(childNode);
                        if (lAnimeMangaObject != null) lSearchResults.Add((T) lAnimeMangaObject);
                    }
                    else if (typeof(T) == typeof(User.User))
                    {
                        User.User lUserObject = this.GetSearchResultObjectUser(childNode);
                        if (lUserObject != null) lSearchResults.Add((T) Convert.ChangeType(lUserObject, typeof(T)));
                    }
                }
                this._currentPageContent = lSearchResults.ToArray();

                this._nextPage++;
                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult(ErrorHandler.HandleError(lResponse).Exceptions);
            }
        }

        [CanBeNull]
        private IAnimeMangaObject GetSearchResultObjectAnimeManga(HtmlNode node)
        {
            int lId = node.Attributes.Contains("class")
                ? Convert.ToInt32(node.Attributes["class"].Value.Substring("entry".Length))
                : -1;
            string lName = node.ChildNodes[1].InnerText;
            List<GenreType> lGenreList = new List<GenreType>();
            foreach (string curGenre in node.ChildNodes[2].InnerText.Split(' '))
            {
                lGenreList.Add(GenreHelper.StringToGenreDictionary[curGenre]);
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
                        lStatus = AnimeMangaStatus.Cancelled;
                        break;
                }
            }
            Type lType = typeof(object);
            AnimeMedium lAnimeMedium = AnimeMedium.Unknown;
            MangaMedium lMangaMedium = MangaMedium.Unknown;
            switch (node.ChildNodes[3].InnerText)
            {
                case "Animeserie":
                    lType = typeof(Anime);
                    lAnimeMedium = AnimeMedium.Series;
                    break;
                case "OVA":
                    lType = typeof(Anime);
                    lAnimeMedium = AnimeMedium.Ova;
                    break;
                case "Movie":
                    lType = typeof(Anime);
                    lAnimeMedium = AnimeMedium.Movie;
                    break;
                case "Mangaserie":
                    lType = typeof(Manga);
                    lMangaMedium = MangaMedium.Series;
                    break;
                case "One-Shot":
                    lType = typeof(Manga);
                    lMangaMedium = MangaMedium.OneShot;
                    break;
            }

            if ((typeof(T) == typeof(Anime) || typeof(T) == typeof(IAnimeMangaObject)) && lType == typeof(Anime))
                return new Anime(lName, lId, lGenreList, lStatus, lAnimeMedium);
            if ((typeof(T) == typeof(Manga) || typeof(T) == typeof(IAnimeMangaObject)) && lType == typeof(Manga))
                return new Manga(lName, lId, lGenreList, lStatus, lMangaMedium);
            return null;
        }

        [CanBeNull]
        private User.User GetSearchResultObjectUser(HtmlNode node)
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

            return new User.User(lUsername, lUserId, lAvatar);
        }

        #endregion
    }
}