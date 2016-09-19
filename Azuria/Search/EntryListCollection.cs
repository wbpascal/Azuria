using System;
using System.Collections;
using System.Collections.Generic;
using Azuria.Media;
using Azuria.Search.Input;

namespace Azuria.Search
{
    internal class EntryListCollection<T> : IEnumerable<T> where T : class, IAnimeMangaObject
    {
        private readonly EntryListInput _input;

        internal EntryListCollection(EntryListInput input)
        {
            if ((typeof(T) != typeof(Anime)) && (typeof(T) != typeof(Manga))) throw new ArgumentException(nameof(T));
            this._input = input;
        }

        #region Methods

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return new EntryListEnumerator<T>(this._input);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}