using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enumerable;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.UserInfo
{
    internal class UserEntryEnumerator<T> : PagedEnumerator<UserProfileEntry<T>> where T : class, IMediaObject
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

        protected override async Task<IProxerResult<IEnumerable<UserProfileEntry<T>>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<ListDataModel[]> lResult = await RequestHandler.ApiRequest(
                    UserRequestBuilder.GetList(this._user.Id, typeof(T).Name.ToLowerInvariant(),
                        nextPage, ResultsPerPage, senpai: this._senpai))
                .ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<IEnumerable<UserProfileEntry<T>>>(lResult.Exceptions);

            return new ProxerResult<IEnumerable<UserProfileEntry<T>>>(from listDataModel in lResult.Result
                select new UserProfileEntry<T>(listDataModel, this._user));
        }

        #endregion
    }
}