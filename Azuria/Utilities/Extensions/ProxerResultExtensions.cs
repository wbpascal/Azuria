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
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<T> ThrowFirstForNonSuccess<T>(this Task<ProxerResult<T>> task)
        {
            ProxerResult<T> lResult = await task;
            if (!lResult.Success) throw lResult.Exceptions.Any() ? lResult.Exceptions.First() : new Exception();

            return lResult.Result;
        }

        /// <summary>
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task ThrowFirstForNonSuccess(this Task<ProxerResult> task)
        {
            ProxerResult lResult = await task;
            if (!lResult.Success)
                throw lResult.Exceptions.Any() ? lResult.Exceptions.First() : new Exception();
        }

        #endregion
    }
}