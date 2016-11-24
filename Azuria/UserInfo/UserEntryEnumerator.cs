using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Utilities;

namespace Azuria.UserInfo
{
    internal class UserEntryEnumerator<T> : PageEnumerator<UserProfileEntry<T>> where T : class, IMediaObject
    {
        private const int ResultsPerPage = 100;
        private readonly Senpai _senpai;
        private readonly User _user;

        internal UserEntryEnumerator(User user, Senpai senpai) : base(ResultsPerPage)
        {
            this._user = user;
            this._senpai = senpai;
        }

        #region Methods

        internal override async Task<IProxerResult<IEnumerable<UserProfileEntry<T>>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<ListDataModel[]> lResult = await RequestHandler.ApiRequest(
                    ApiRequestBuilder.UserGetList(this._user.Id, typeof(T).Name.ToLowerInvariant(),
                        nextPage, ResultsPerPage, this._senpai))
                .ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<UserProfileEntry<T>>>(lResult.Exceptions);

            return new ProxerResult<IEnumerable<UserProfileEntry<T>>>(from listDataModel in lResult.Result
                select new UserProfileEntry<T>(listDataModel, this._user));
        }

        #endregion
    }
}