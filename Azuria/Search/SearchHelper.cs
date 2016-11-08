using System;
using Azuria.Media;
using Azuria.Search.Input;

namespace Azuria.Search
{
    /// <summary>
    /// </summary>
    public static class SearchHelper
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static EntryListEnumerable<T> EntryList<T>(EntryListInput input) where T : class, IMediaObject
        {
            if (input == null) throw new ArgumentException(nameof(input));
            return new EntryListEnumerable<T>(input);
        }

        /// <summary>
        /// </summary>
        /// <param name="inputFactory"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static EntryListEnumerable<T> EntryList<T>(Action<EntryListInput> inputFactory)
            where T : class, IMediaObject
        {
            EntryListInput lInput = new EntryListInput();
            inputFactory.Invoke(lInput);
            return EntryList<T>(lInput);
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static SearchResultEnumerable<T> Search<T>(SearchInput input) where T : IMediaObject
        {
            if (input == null) throw new ArgumentException(nameof(input));
            return new SearchResultEnumerable<T>(input);
        }

        /// <summary>
        /// </summary>
        /// <param name="inputFactory"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static SearchResultEnumerable<T> Search<T>(Action<SearchInput> inputFactory) where T : IMediaObject
        {
            SearchInput lInput = new SearchInput();
            inputFactory.Invoke(lInput);
            return new SearchResultEnumerable<T>(lInput);
        }

        #endregion
    }
}