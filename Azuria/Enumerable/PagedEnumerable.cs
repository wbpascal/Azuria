using System.Collections;
using System.Collections.Generic;

namespace Azuria.Enumerable
{
    /// <summary>
    /// Represents an enumeration of objects that are fetched from a paged source.
    /// </summary>
    /// <typeparam name="T">The type of the objects.</typeparam>
    public abstract class PagedEnumerable<T> : IEnumerable<T>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the value that indicates how many times the program should retry fetching a new page before throwing an
        /// exception.
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