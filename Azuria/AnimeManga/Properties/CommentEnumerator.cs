using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Exceptions;
using Azuria.User.Comment;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.AnimeManga.Properties
{
    /// <summary>
    /// </summary>
    public sealed class CommentEnumerator<T> : IEnumerator<Comment<T>> where T : IAnimeMangaObject
    {
        private const int ResultsPerPage = 100;
        private readonly T _animeMangaObject;
        private readonly Senpai _senpai;
        private readonly string _sort;
        private Comment<T>[] _currentPageContent = new Comment<T>[0];
        private int _currentPageContentIndex = -1;
        private int _nextPage;

        internal CommentEnumerator(T animeMangaObject, string sort, Senpai senpai)
        {
            this._animeMangaObject = animeMangaObject;
            this._sort = sort;
            this._senpai = senpai;
        }

        #region Properties

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        [NotNull]
        public Comment<T> Current => this._currentPageContent[this._currentPageContentIndex];

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => this.Current;

        #endregion

        #region Inherited

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._currentPageContent = new Comment<T>[0];
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
            this._currentPageContent = new Comment<T>[0];
            this._currentPageContentIndex = ResultsPerPage - 1;
            this._nextPage = 0;
        }

        #endregion

        #region

        [ItemNotNull]
        private async Task<ProxerResult> GetNextPage()
        {
            ProxerResult<ProxerApiResponse<CommentDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.BuildForGetComments(this._animeMangaObject.Id,
                        this._nextPage, ResultsPerPage, this._sort, this._senpai));
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);

            this._currentPageContent = (from commentDataModel in lResult.Result.Data
                select new Comment<T>(commentDataModel, this._animeMangaObject, this._senpai)).ToArray();

            return new ProxerResult();
        }

        #endregion
    }
}