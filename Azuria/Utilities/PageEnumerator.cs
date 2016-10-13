using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Utilities
{
    /// <summary>
    /// </summary>
    public abstract class PageEnumerator<T> : IEnumerator<T>
    {
        private readonly int _resultsPerPage;
        private T[] _currentPageContent = new T[0];
        private int _currentPageContentIndex = -1;
        private int _nextPage;

        /// <summary>
        /// </summary>
        /// <param name="resultsPerPage"></param>
        protected PageEnumerator(int resultsPerPage = 50)
        {
            this._resultsPerPage = resultsPerPage;
        }

        #region Properties

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public T Current => this._currentPageContent[this._currentPageContentIndex];

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => this.Current;

        #endregion

        #region Methods

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._currentPageContent = null;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        protected T[] GetCurrentPage()
        {
            return this._currentPageContent;
        }

        internal abstract Task<ProxerResult<IEnumerable<T>>> GetNextPage(int nextPage);

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
        /// end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            if (this._currentPageContentIndex >= this._currentPageContent.Length - 1)
            {
                if (this._currentPageContent.Length%this._resultsPerPage != 0) return false;
                ProxerResult<IEnumerable<T>> lGetSearchResult = Task.Run(() => this.GetNextPage(this._nextPage)).Result;
                if (!lGetSearchResult.Success || (lGetSearchResult.Result == null))
                    throw lGetSearchResult.Exceptions.FirstOrDefault() ?? new Exception("Unkown error");
                this._currentPageContent = lGetSearchResult.Result as T[] ?? lGetSearchResult.Result.ToArray();
                this._nextPage++;
                this._currentPageContentIndex = -1;
            }
            this._currentPageContentIndex++;
            return this._currentPageContentIndex < this._currentPageContent.Length;
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public void Reset()
        {
            this._currentPageContent = new T[0];
            this._currentPageContentIndex = this._resultsPerPage - 1;
            this._nextPage = 0;
        }

        #endregion
    }
}