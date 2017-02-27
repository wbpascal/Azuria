using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Enumerable
{
    /// <summary>
    /// Represents an enumerator that returns objects from a paged source.
    /// </summary>
    /// <typeparam name="T">The type of the objects.</typeparam>
    public abstract class PagedEnumerator<T> : RetryableEnumerator<T>
    {
        private readonly int _objectsPerPage;
        private T[] _currentPageContent = new T[0];
        private int _currentPageContentIndex = -1;
        private int _nextPage;

        /// <summary>
        /// Initialises a new instance of <see cref="PagedEnumerator{T}" />.
        /// </summary>
        /// <param name="objectsPerPage">A number indication how many objects the enumerator should expect per page.</param>
        /// <param name="retryCount">
        /// A number indicating how many times the program should retry fetching a new page before
        /// throwing an exception.
        /// </param>
        protected PagedEnumerator(int objectsPerPage = 50, int retryCount = 2) : base(retryCount)
        {
            this._objectsPerPage = objectsPerPage;
        }

        #region Properties

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        public override T Current => this._currentPageContent[this._currentPageContentIndex];

        #endregion

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            this._currentPageContent = null;
        }

        /// <summary>
        /// Returns an array that contains all elements of the current page.
        /// </summary>
        /// <returns>An array of object of type <typeparamref name="T" />.</returns>
        protected T[] GetCurrentPage()
        {
            return this._currentPageContent;
        }

        /// <summary>
        /// Creates the objects for the next page.
        /// </summary>
        /// <param name="nextPage">The index of the next page.</param>
        /// <returns>
        /// An asynchronous Task that returns a <see cref="IProxerResult" /> containing the enumeration of the objects of
        /// the next page.
        /// </returns>
        protected abstract Task<IProxerResult<IEnumerable<T>>> GetNextPage(int nextPage);

        private void LoadNextPage()
        {
            IProxerResult<IEnumerable<T>> lGetSearchResult = Task.Run(() => this.GetNextPage(this._nextPage)).Result;
            if (!lGetSearchResult.Success || lGetSearchResult.Result == null)
                throw lGetSearchResult.Exceptions.FirstOrDefault() ?? new Exception("Unkown error");
            this._currentPageContent = lGetSearchResult.Result as T[] ?? lGetSearchResult.Result.ToArray();
        }

        /// <summary>
        /// Moves the pointer to the next element in the enumeration and returns if the action succeeded.
        /// </summary>
        /// <param name="retryCount">A number that indicates the current retry.</param>
        /// <returns>
        /// A boolean value that indicates whether the pointer could be moved to the next element.
        /// </returns>
        public override bool MoveNext(int retryCount)
        {
            if (this._currentPageContentIndex >= this._currentPageContent.Length - 1)
            {
                if (this._currentPageContent.Length % this._objectsPerPage != 0) return false;
                this.LoadNextPage();
                this._nextPage++;
                this._currentPageContentIndex = -1;
            }
            this._currentPageContentIndex++;
            return this._currentPageContentIndex < this._currentPageContent.Length;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public override void Reset()
        {
            this._currentPageContent = new T[0];
            this._currentPageContentIndex = this._objectsPerPage - 1;
            this._nextPage = 0;
        }

        #endregion
    }
}