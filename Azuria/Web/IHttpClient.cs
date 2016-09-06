using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;

namespace Azuria.Web
{
    /// <summary>
    /// </summary>
    public interface IHttpClient
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        Task<ProxerResult<string>> GetRequest(Uri url, Senpai senpai = null);

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="checkFuncs"></param>
        /// <param name="senpai"></param>
        /// <param name="useMobileCookies"></param>
        /// <param name="checkLogin"></param>
        /// <param name="recursion"></param>
        /// <returns></returns>
        Task<ProxerResult<string>> GetRequest(Uri url, Func<string, ProxerResult>[] checkFuncs, Senpai senpai = null,
            bool useMobileCookies = false, bool checkLogin = true, int recursion = 0);

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postArgs"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        Task<ProxerResult<string>> PostRequest(Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            Senpai senpai);

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
        Task<ProxerResult<string>> PostRequest(Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            Func<string, ProxerResult>[] checkFuncs, Senpai senpai = null, bool useMobileCookies = false,
            bool checkLogin = true, int recursion = 0,
            Dictionary<string, string> header = null);

        #endregion
    }
}