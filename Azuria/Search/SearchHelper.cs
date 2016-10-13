using System;
using System.Collections.Generic;
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
        public static IEnumerable<T> EntryList<T>(EntryListInput input) where T : class, IAnimeMangaObject
        {
            if (input == null) throw new ArgumentException(nameof(input));
            return new EntryListCollection<T>(input);
        }

        /// <summary>
        /// </summary>
        /// <param name="inputFactory"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> EntryList<T>(Action<EntryListInput> inputFactory)
            where T : class, IAnimeMangaObject
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
        public static IEnumerable<T> Search<T>(SearchInput input) where T : IAnimeMangaObject
        {
            if (input == null) throw new ArgumentException(nameof(input));
            return new SearchResultCollection<T>(input);
        }

        /// <summary>
        /// </summary>
        /// <param name="inputFactory"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Search<T>(Action<SearchInput> inputFactory) where T : IAnimeMangaObject
        {
            SearchInput lInput = new SearchInput();
            inputFactory.Invoke(lInput);
            return new SearchResultCollection<T>(lInput);
        }

        #endregion
    }
}