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
        /// <param name="style"></param>
        /// <returns></returns>
        public static async Task<ProxerResult<HeaderInfo>> GetRandomHeader(HeaderStyle style)
        {
            ProxerResult<ProxerApiResponse<HeaderDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.MediaGetRandomHeader(HeaderStyleToString(style)));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult<HeaderInfo>(lResult.Exceptions);

            return
                new ProxerResult<HeaderInfo>(lResult.Result.Data == null
                    ? HeaderInfo.None
                    : new HeaderInfo(lResult.Result.Data));
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