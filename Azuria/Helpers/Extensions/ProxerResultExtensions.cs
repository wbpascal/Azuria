using System;
using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Helpers.Extensions
{
    /// <summary>
    /// </summary>
    public static class ProxerResultExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="onError"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns></returns>
        public static TOut OnError<T, TOut>(this T result, TOut onError) where T : IProxerResult<TOut>
        {
            return result.OnError(() => onError);
        }
        
        /// <summary>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="onError"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns></returns>
        public static TOut OnError<T, TOut>(this T result, Func<TOut> onError) where T : IProxerResult<TOut>
        {
            return result.Success ? result.Result : onError.Invoke();
        }

        /// <summary>
        /// </summary>
        /// <param name="task"></param>
        /// <param name="onError"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns></returns>
        public static Task<TOut> OnError<T, TOut>(this Task<T> task, TOut onError)
            where T : IProxerResult<TOut>
        {
            return task.OnError(() => Task.FromResult(onError));
        }

        /// <summary>
        /// </summary>
        /// <param name="task"></param>
        /// <param name="onError"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns></returns>
        public static async Task<TOut> OnError<T, TOut>(this Task<T> task, Func<Task<TOut>> onError)
            where T : IProxerResult<TOut>
        {
            T lResult = await task.ConfigureAwait(false);
            return lResult.Success ? lResult.Result : await onError.Invoke();
        }
    }
}