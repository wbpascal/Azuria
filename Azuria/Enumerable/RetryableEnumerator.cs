using System.Collections;
using System.Collections.Generic;

namespace Azuria.Enumerable
{
    /// <summary>
    /// Represents an enumerator which retries their <see cref="MoveNext()" /> method a specified number of times.
    /// </summary>
    /// <typeparam name="T">The type of the objects this enumerator contains.</typeparam>
    public abstract class RetryableEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// Initialises a new instance of <see cref="RetryableEnumerator{T}" />.
        /// </summary>
        /// <param name="retryCount">A number that indicates how many times the <see cref="MoveNext()" /> method should be retried.</param>
        protected RetryableEnumerator(int retryCount = 2)
        {
            this.RetryCount = retryCount;
        }

        #region Properties

        /// <inheritdoc />
        public abstract T Current { get; }

        /// <inheritdoc />
        object IEnumerator.Current => this.Current;


        /// <summary>
        /// Get or sets a value that inicates how many times the <see cref="MoveNext()" /> method should be retried.
        /// </summary>
        public int RetryCount { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public abstract void Dispose();

        /// <inheritdoc />
        public bool MoveNext()
        {
            for (int i = 0; i <= this.RetryCount; i++)
                try
                {
                    return this.MoveNext(i);
                }
                catch
                {
                    if (i == this.RetryCount) throw;
                }
            return false;
        }

        /// <summary>
        /// Moves the pointer to the next element in the enumeration and returns if the action succeeded.
        /// </summary>
        /// <param name="retryCount">A number that indicates the current retry.</param>
        /// <returns>A boolean value that indicates whether the pointer could be moved to the next element.</returns>
        public abstract bool MoveNext(int retryCount);

        /// <inheritdoc />
        public abstract void Reset();

        #endregion
    }
}