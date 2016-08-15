using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BookmarkEnumerator<T> : IEnumerator<AnimeMangaBookmarkObject<T>> where T : class, IAnimeMangaObject
    {
        private const int ResultsPerPage = 100;
        private readonly UserControlPanel _controlPanel;
        private readonly Senpai _senpai;
        private AnimeMangaBookmarkObject<T>[] _currentPageContent = new AnimeMangaBookmarkObject<T>[0];
        private int _currentPageContentIndex = -1;
        private int _nextPage;

        internal BookmarkEnumerator(Senpai senpai, UserControlPanel controlPanel)
        {
            this._senpai = senpai;
            this._controlPanel = controlPanel;
        }

        #region Properties

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        [NotNull]
        public AnimeMangaBookmarkObject<T> Current => this._currentPageContent[this._currentPageContentIndex];

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => this.Current;

        #endregion

        #region Inherited

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._currentPageContent = new AnimeMangaBookmarkObject<T>[0];
        }

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>
        ///     true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
        ///     end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            if (this._currentPageContentIndex >= this._currentPageContent.Length - 1)
            {
                if (this._currentPageContent.Length%ResultsPerPage != 0) return false;
                ProxerResult lGetSearchResult = Task.Run(this.GetNextPage).Result;
                if (!lGetSearchResult.Success)
                    throw lGetSearchResult.Exceptions.FirstOrDefault() ?? new WrongResponseException();
                this._nextPage++;
                this._currentPageContentIndex = -1;
            }
            this._currentPageContentIndex++;
            return true;
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public void Reset()
        {
            this._currentPageContent = new AnimeMangaBookmarkObject<T>[0];
            this._currentPageContentIndex = ResultsPerPage - 1;
            this._nextPage = 0;
        }

        #endregion

        #region

        [ItemNotNull]
        private async Task<ProxerResult> GetNextPage()
        {
            ProxerResult<ProxerApiResponse<BookmarkDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(
                        ApiRequestBuilder.BuildForGetReminder(typeof(T).GetTypeInfo().Name.ToLowerInvariant(),
                            this._nextPage, ResultsPerPage, this._senpai));
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);
            BookmarkDataModel[] lData = lResult.Result.Data;

            this._currentPageContent = (from bookmarkDataModel in lData
                select new AnimeMangaBookmarkObject<T>(
                    typeof(T) == typeof(Anime)
                        ? (IAnimeMangaContent<T>) new Anime.Episode(bookmarkDataModel, this._senpai)
                        : (IAnimeMangaContent<T>) new Manga.Chapter(bookmarkDataModel, this._senpai), bookmarkDataModel,
                    this._senpai, this._controlPanel)).ToArray();

            return new ProxerResult();
        }

        #endregion
    }
}