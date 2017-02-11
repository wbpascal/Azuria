using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Media;
using Azuria.Api.v1.RequestBuilder;
using Azuria.ErrorHandling;

namespace Azuria.Media.Headers
{
    /// <summary>
    /// Represents a helper class that is used to get the current headers of Proxer.Me.
    /// </summary>
    public static class HeaderHelper
    {
        #region Methods

        /// <summary>
        /// Gets an enumeration of all current headers.
        /// 
        /// Required api permissions:
        /// * Media - Level 0
        /// </summary>
        /// <returns>An asynchronous Task of an <see cref="IProxerResult" /> object that returns an enumeration of headers.</returns>
        public static async Task<IProxerResult<IEnumerable<HeaderInfo>>> GetHeaderList()
        {
            ProxerApiResponse<HeaderDataModel[]> lResult = await RequestHandler.ApiRequest(
                MediaRequestBuilder.GetHeaderList()).ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<IEnumerable<HeaderInfo>>(lResult.Exceptions);

            return
                new ProxerResult<IEnumerable<HeaderInfo>>(from headerDataModel in lResult.Result
                    select new HeaderInfo(headerDataModel));
        }

        /// <summary>
        /// Gets a random header with a specified style.
        /// 
        /// Required api permissions:
        /// * Media - Level 0
        /// </summary>
        /// <param name="style">The style of the returned header.</param>
        /// <returns>An asynchronous Task of an <see cref="IProxerResult" /> object that returns a single header.</returns>
        public static async Task<IProxerResult<HeaderInfo>> GetRandomHeader(HeaderStyle style)
        {
            ProxerApiResponse<HeaderDataModel> lResult = await RequestHandler.ApiRequest(
                    MediaRequestBuilder.GetRandomHeader(HeaderStyleToString(style)))
                .ConfigureAwait(false);
            if (!lResult.Success) return new ProxerResult<HeaderInfo>(lResult.Exceptions);

            return new ProxerResult<HeaderInfo>(lResult.Result == null
                ? HeaderInfo.None
                : new HeaderInfo(lResult.Result));
        }

        private static string HeaderStyleToString(HeaderStyle style)
        {
            switch (style)
            {
                case HeaderStyle.OldBlue:
                    return "old_blue";
                default:
                    return style.ToString().ToLowerInvariant();
            }
        }

        #endregion
    }
}