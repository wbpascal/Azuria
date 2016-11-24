using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Utilities;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Properties;

namespace Azuria.UserInfo.Comment
{
    internal class CommentEnumerator<T> : PageEnumerator<Comment<T>> where T : class, IMediaObject
    {
        private const int ResultsPerPage = 25;
        private readonly T _mediaObject;
        private readonly Senpai _senpai;
        private readonly string _sort;
        private readonly User _user;

        internal CommentEnumerator(T mediaObject, string sort) : base(ResultsPerPage)
        {
            this._mediaObject = mediaObject;
            this._sort = sort;
        }

        internal CommentEnumerator(User user, Senpai senpai) : base(ResultsPerPage)
        {
            this._user = user;
            this._senpai = senpai;
        }

        #region Methods

        internal override async Task<IProxerResult<IEnumerable<Comment<T>>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<CommentDataModel[]> lResult =
                await RequestHandler.ApiRequest(this._user == null
                        ? ApiRequestBuilder.InfoGetComments(this._mediaObject.Id,
                            nextPage, ResultsPerPage, this._sort)
                        : ApiRequestBuilder.UserGetLatestComments(this._user.Id, nextPage, ResultsPerPage,
                            typeof(T).GetTypeInfo().Name.ToLower(), 0, this._senpai))
                    .ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<Comment<T>>>(lResult.Exceptions);
            CommentDataModel[] lData = lResult.Result;

            if ((this._user != null) && lData.Any()) this.InitialiseUserValues(lData.First());
            return new ProxerResult<IEnumerable<Comment<T>>>(this.ToCommentList(lData).ToArray());
        }

        private void InitialiseUserValues(CommentDataModel dataModel)
        {
            if (!this._user.UserName.IsInitialised)
                (this._user.UserName as InitialisableProperty<string>)?.Set(dataModel.Username);
            if (!this._user.Avatar.IsInitialised)
                (this._user.Avatar as InitialisableProperty<Uri>)?.Set(
                    new Uri("http://cdn.proxer.me/avatar/" + dataModel.Avatar));
        }

        private IEnumerable<Comment<T>> ToCommentList(IEnumerable<CommentDataModel> dataModels)
        {
            List<Comment<T>> lCommentList = new List<Comment<T>>();
            foreach (CommentDataModel commentDataModel in dataModels)
            {
                T lMediaObject = this._mediaObject;
                if (typeof(T) == typeof(Anime))
                    lMediaObject = lMediaObject ?? new Anime(commentDataModel.EntryId) as T;
                if (typeof(T) == typeof(Manga))
                    lMediaObject = lMediaObject ?? new Manga(commentDataModel.EntryId) as T;

                lCommentList.AddIf(new Comment<T>(commentDataModel, lMediaObject, this._user),
                    comment => lMediaObject != null);
            }
            return lCommentList;
        }

        #endregion
    }
}