using Azuria.Enumerable;
using Azuria.Media;
using Azuria.Search.Input;

namespace Azuria.Search
{
    /// <summary>
    /// Represents a class that helps to fetch following search results after the inital search.
    /// </summary>
    /// <typeparam name="T">The type of the search results.</typeparam>
    public class SearchResultEnumerable<T> : PagedEnumerable<T> where T : IMediaObject
    {
        private readonly SearchInput _input;

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        internal SearchResultEnumerable(SearchInput input)
        {
            this._input = input;
        }

        #region Methods

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public override PagedEnumerator<T> GetEnumerator()
        {
            return new SearchResultEnumerator<T>(this._input);
        }

        #endregion
    }
}