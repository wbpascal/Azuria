using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Azuria.Search
{
    /// <summary>
    ///     Represents a class that helps to fetch following search results after the inital search.
    /// </summary>
    /// <typeparam name="T">The type of the search results.</typeparam>
    public class SearchResult<T> : IEnumerable<T> where T : ISearchableObject
    {
        private readonly string _link;
        private readonly Senpai _senpai;

        internal SearchResult(string link, [NotNull] Senpai senpai)
        {
            this._link = link;
            this._senpai = senpai;
        }

        #region Inherited

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new SearchResultEnumerator<T>(this._link, this._senpai);
        }

        #endregion
    }
}