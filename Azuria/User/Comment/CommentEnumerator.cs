using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.User.Comment
{
    /// <summary>
    /// </summary>
    public sealed class CommentEnumerator<T> : IEnumerator<Comment<T>> where T : IAnimeMangaObject
    {
        private const int ResultsPerPage = 100;
        private readonly T _animeMangaObject;
        private readonly string _sort;
        private readonly User _user;
        private Comment<T>[] _currentPageContent = new Comment<T>[0];
        private int _currentPageContentIndex = -1;
        private int _nextPage;

        internal CommentEnumerator(T animeMangaObject, string sort)
        {
            this._animeMangaObject = animeMangaObject;
            this._sort = sort;
        }

        internal CommentEnumerator(User user)
        {
            this._user = user;
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
                    RequestHandler.ApiRequest(this._user == null
                        ? ApiRequestBuilder.InfoGetComments(this._animeMangaObject.Id,
                            this._nextPage, ResultsPerPage, this._sort)
                        : ApiRequestBuilder.UserGetLatestComments(this._user.Id, this._nextPage, ResultsPerPage,
                            typeof(T).GetTypeInfo().Name.ToLower(), 0));
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);
            CommentDataModel[] lData = lResult.Result.Data;

            if (this._user != null && lData.Any()) this.InitialiseUserValues(lData.First());
            this._currentPageContent = this.ToCommentList(lData).ToArray();

            return new ProxerResult();
        }

        private void InitialiseUserValues(CommentDataModel dataModel)
        {
            if (!this._user.UserName.IsInitialisedOnce)
                this._user.UserName.SetInitialisedObject(dataModel.Username);
            if (!this._user.Avatar.IsInitialisedOnce)
                this._user.Avatar.SetInitialisedObject(
                    new Uri("http://cdn.proxer.me/avatar/" + dataModel.Avatar));
        }

        private IEnumerable<Comment<T>> ToCommentList(IEnumerable<CommentDataModel> dataModels)
        {
            List<Comment<T>> lCommentList = new List<Comment<T>>();
            foreach (CommentDataModel commentDataModel in dataModels)
            {
                T lAnimeMangaObject = this._animeMangaObject;
                if (lAnimeMangaObject == null)
                {
                    if (typeof(T) == typeof(Anime))
                        lAnimeMangaObject =
                            (T) Convert.ChangeType(new Anime(commentDataModel.EntryId), typeof(T));
                    if (typeof(T) == typeof(Manga))
                        lAnimeMangaObject =
                            (T) Convert.ChangeType(new Manga(commentDataModel.EntryId), typeof(T));
                }
                lCommentList.Add(new Comment<T>(commentDataModel, lAnimeMangaObject, this._user));
            }
            return lCommentList;
        }

        #endregion
    }
}