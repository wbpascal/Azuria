using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Enumerable
{
    /// <summary>
    /// </summary>
    public abstract class PagedEnumerator<T> : IEnumerator<T>
    {
        private readonly int _resultsPerPage;
        private T[] _currentPageContent = new T[0];
        private int _currentPageContentIndex = -1;
        private int _nextPage;

        /// <summary>
        /// </summary>
        /// <param name="resultsPerPage"></param>
        protected PagedEnumerator(int resultsPerPage = 50)
        {
            this._resultsPerPage = resultsPerPage;
        }

        #region Properties

        /// <inheritdoc />
        public T Current => this._currentPageContent[this._currentPageContentIndex];

        /// <inheritdoc />
        object IEnumerator.Current => this.Current;

        #endregion

        #region Methods

        /// <inheritdoc />
        public virtual void Dispose()
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

        internal abstract Task<IProxerResult<IEnumerable<T>>> GetNextPage(int nextPage);

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (this._currentPageContentIndex >= this._currentPageContent.Length - 1)
            {
                if (this._currentPageContent.Length%this._resultsPerPage != 0) return false;
                IProxerResult<IEnumerable<T>> lGetSearchResult = Task.Run(() => this.GetNextPage(this._nextPage)).Result;
                if (!lGetSearchResult.Success || (lGetSearchResult.Result == null))
                    throw lGetSearchResult.Exceptions.FirstOrDefault() ?? new Exception("Unkown error");
                this._currentPageContent = lGetSearchResult.Result as T[] ?? lGetSearchResult.Result.ToArray();
                this._nextPage++;
                this._currentPageContentIndex = -1;
            }
            this._currentPageContentIndex++;
            return this._currentPageContentIndex < this._currentPageContent.Length;
        }

        /// <inheritdoc />
        public virtual void Reset()
        {
            this._currentPageContent = new T[0];
            this._currentPageContentIndex = this._resultsPerPage - 1;
            this._nextPage = 0;
        }

        #endregion
    }
}