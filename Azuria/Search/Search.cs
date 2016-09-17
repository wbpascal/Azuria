using System;
using System.Collections;
using System.Collections.Generic;
using Azuria.Media;
using Azuria.Search.Input;

namespace Azuria.Search
{
    /// <summary>
    ///     Represents a class that helps to fetch following search results after the inital search.
    ///     TODO: Implement GetEntryList
    /// </summary>
    /// <typeparam name="T">The type of the search results.</typeparam>
    public class Search<T> : IEnumerable<T> where T : IAnimeMangaObject
    {
        private readonly SearchInput _input;

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        public Search(SearchInput input)
        {
            this._input = input;
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        public Search(Action<SearchInput> input)
        {
            this._input = new SearchInput();
            input.Invoke(this._input);
        }

        #region Methods

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
            return new SearchResultEnumerator<T>(this._input);
        }

        #endregion
    }
}