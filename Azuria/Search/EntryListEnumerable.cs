using System;
using Azuria.Enumerable;
using Azuria.Media;
using Azuria.Search.Input;

namespace Azuria.Search
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntryListEnumerable<T> : PagedEnumerable<T> where T : class, IMediaObject
    {
        private readonly EntryListInput _input;

        internal EntryListEnumerable(EntryListInput input)
        {
            if (typeof(T) != typeof(Anime) && typeof(T) != typeof(Manga)) throw new ArgumentException(nameof(T));
            this._input = input;
        }

        #region Methods

        /// <inheritdoc />
        public override PagedEnumerator<T> GetEnumerator()
        {
            return new EntryListEnumerator<T>(this._input);
        }

        #endregion
    }
}