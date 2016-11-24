using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Media;
using Azuria.ErrorHandling;

namespace Azuria.Media.Headers
{
    /// <summary>
    /// </summary>
    public static class HeaderHelper
    {
        #region Methods

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static async Task<IProxerResult<IEnumerable<HeaderInfo>>> GetHeaderList()
        {
            ProxerApiResponse<HeaderDataModel[]> lResult = await RequestHandler.ApiRequest(
                ApiRequestBuilder.MediaGetHeaderList()).ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<HeaderInfo>>(lResult.Exceptions);

            return
                new ProxerResult<IEnumerable<HeaderInfo>>(from headerDataModel in lResult.Result
                    select new HeaderInfo(headerDataModel));
        }

        /// <summary>
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static async Task<IProxerResult<HeaderInfo>> GetRandomHeader(HeaderStyle style)
        {
            ProxerApiResponse<HeaderDataModel> lResult = await RequestHandler.ApiRequest(
                    ApiRequestBuilder.MediaGetRandomHeader(HeaderStyleToString(style)))
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