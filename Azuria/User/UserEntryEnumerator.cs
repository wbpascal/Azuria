using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.User
{
    internal class UserEntryEnumerator<T> : IEnumerator<UserProfileEntry<T>> where T : class, IAnimeMangaObject
    {
        private const int ResultsPerPage = 100;
        private readonly User _user;
        private UserProfileEntry<T>[] _currentPageContent = new UserProfileEntry<T>[0];
        private int _currentPageContentIndex = -1;
        private int _nextPage;

        internal UserEntryEnumerator(User user)
        {
            this._user = user;
        }

        #region Properties

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        [NotNull]
        public UserProfileEntry<T> Current => this._currentPageContent[this._currentPageContentIndex];

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => this.Current;

        #endregion

        #region Inherited

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._currentPageContent = new UserProfileEntry<T>[0];
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
            this._currentPageContent = new UserProfileEntry<T>[0];
            this._currentPageContentIndex = ResultsPerPage - 1;
            this._nextPage = 0;
        }

        #endregion

        #region

        [ItemNotNull]
        private async Task<ProxerResult> GetNextPage()
        {
            ProxerResult<ProxerApiResponse<ListDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.UserGetList(this._user.Id,
                        typeof(T).Name.ToLowerInvariant(), this._nextPage, ResultsPerPage));
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);

            this._currentPageContent = (from listDataModel in lResult.Result.Data
                select new UserProfileEntry<T>(listDataModel, this._user)).ToArray();

            return new ProxerResult();
        }

        #endregion
    }
}