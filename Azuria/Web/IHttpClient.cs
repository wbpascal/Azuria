using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Web
{
    /// <summary>
    /// </summary>
    public interface IHttpClient
    {
        #region

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        [ItemNotNull]
        Task<ProxerResult<string>> GetRequest(Uri url, [CanBeNull] Senpai senpai = null);

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="checkFuncs"></param>
        /// <param name="senpai"></param>
        /// <param name="useMobileCookies"></param>
        /// <param name="checkLogin"></param>
        /// <param name="recursion"></param>
        /// <returns></returns>
        [ItemNotNull]
        Task<ProxerResult<string>> GetRequest([NotNull] Uri url,
            [CanBeNull] Func<string, ProxerResult>[] checkFuncs, [CanBeNull] Senpai senpai = null,
            bool useMobileCookies = false, bool checkLogin = true, int recursion = 0);

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postArgs"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        [ItemNotNull]
        Task<ProxerResult<string>> PostRequest([NotNull] Uri url, [NotNull] Dictionary<string, string> postArgs,
            [NotNull] Senpai senpai);

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postArgs"></param>
        /// <param name="checkFuncs"></param>
        /// <param name="senpai"></param>
        /// <param name="useMobileCookies"></param>
        /// <param name="checkLogin"></param>
        /// <param name="recursion"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        [ItemNotNull]
        Task<ProxerResult<string>> PostRequest([NotNull] Uri url,
            [NotNull] Dictionary<string, string> postArgs, [CanBeNull] Func<string, ProxerResult>[] checkFuncs,
            [CanBeNull] Senpai senpai = null, bool useMobileCookies = false, bool checkLogin = true, int recursion = 0,
            Dictionary<string, string> header = null);

        #endregion
    }
}