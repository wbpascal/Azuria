using System;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Utilities.Extensions
{
    /// <summary>
    /// </summary>
    public static class ProxerResultExtensions
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="onError"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns></returns>
        public static TOut OnError<T, TOut>(this T result, TOut onError) where T : IProxerResult<TOut>
        {
            return result.Success ? result.Result : onError;
        }

        /// <summary>
        /// </summary>
        /// <param name="task"></param>
        /// <param name="onError"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns></returns>
        public static async Task<TOut> OnError<T, TOut>(this Task<T> task, TOut onError) where T : IProxerResult<TOut>
        {
            T lResult = await task.ConfigureAwait(false);
            return lResult.Success ? lResult.Result : onError;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<T> ThrowFirstForNonSuccess<T>(this Task<IProxerResult<T>> task)
        {
            IProxerResult<T> lResult = await task.ConfigureAwait(false);
            if (!lResult.Success) throw lResult.Exceptions.Any() ? lResult.Exceptions.First() : new Exception();

            return lResult.Result;
        }

        /// <summary>
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task ThrowFirstForNonSuccess(this Task<IProxerResult> task)
        {
            IProxerResult lResult = await task.ConfigureAwait(false);
            if (!lResult.Success) throw lResult.Exceptions.Any() ? lResult.Exceptions.First() : new Exception();
        }

        #endregion
    }
}