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
        /// <returns></returns>
        public static T OnError<T>(this IProxerResult<T> result, T onError)
        {
            return result.Success ? result.Result : onError;
        }

        /// <summary>
        /// </summary>
        /// <param name="task"></param>
        /// <param name="onError"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> OnError<T>(this Task<IProxerResult<T>> task, T onError)
        {
            IProxerResult<T> lResult = await task;
            return lResult.Success ? lResult.Result : onError;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<T> ThrowFirstForNonSuccess<T>(this Task<IProxerResult<T>> task)
        {
            IProxerResult<T> lResult = await task;
            if (!lResult.Success) throw lResult.Exceptions.Any() ? lResult.Exceptions.First() : new Exception();

            return lResult.Result;
        }

        /// <summary>
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task ThrowFirstForNonSuccess(this Task<IProxerResult> task)
        {
            IProxerResult lResult = await task;
            if (!lResult.Success)
                throw lResult.Exceptions.Any() ? lResult.Exceptions.First() : new Exception();
        }

        #endregion
    }
}