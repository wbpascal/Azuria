using System.Collections;
using System.Collections.Generic;

namespace Azuria.Enumerable
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PagedEnumerable<T> : IEnumerable<T>
    {
        #region Properties

        /// <summary>
        /// </summary>
        public int RetryCount { get; set; } = 2;

        #endregion

        #region Methods

        /// <inheritdoc />
        public abstract PagedEnumerator<T> GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}