using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;

namespace Azuria.UserInfo
{
    internal class UserEntryEnumerator<T> : PageEnumerator<UserProfileEntry<T>> where T : class, IAnimeMangaObject
    {
        private const int ResultsPerPage = 100;
        private readonly User _user;

        internal UserEntryEnumerator(User user) : base(ResultsPerPage)
        {
            this._user = user;
        }

        #region Methods

        internal override async Task<ProxerResult<IEnumerable<UserProfileEntry<T>>>> GetNextPage(int nextPage)
        {
            ProxerResult<ProxerApiResponse<ListDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.UserGetList(this._user.Id,
                        typeof(T).Name.ToLowerInvariant(), nextPage, ResultsPerPage));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<UserProfileEntry<T>>>(lResult.Exceptions);

            return new ProxerResult<IEnumerable<UserProfileEntry<T>>>(from listDataModel in lResult.Result.Data
                select new UserProfileEntry<T>(listDataModel, this._user));
        }

        #endregion
    }
}