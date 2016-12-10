using System.Collections;
using System.Collections.Generic;

namespace Azuria.Enumerable
{
    /// <summary>
    /// </summary>
    public abstract class RetryableEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// </summary>
        /// <param name="retryCount"></param>
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
        /// </summary>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        public abstract bool MoveNext(int retryCount);

        /// <inheritdoc />
        public abstract void Reset();

        #endregion
    }
}