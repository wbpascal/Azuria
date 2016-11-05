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
using Azuria.Utilities.Properties;

namespace Azuria.UserInfo.Comment
{
    internal class CommentEnumerator<T> : PageEnumerator<Comment<T>> where T : IMediaObject
    {
        private const int ResultsPerPage = 25;
        private readonly T _mediaObject;
        private readonly string _sort;
        private readonly User _user;

        internal CommentEnumerator(T mediaObject, string sort) : base(ResultsPerPage)
        {
            this._mediaObject = mediaObject;
            this._sort = sort;
        }

        internal CommentEnumerator(User user)
        {
            this._user = user;
        }

        #region Methods

        internal override async Task<IProxerResult<IEnumerable<Comment<T>>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<CommentDataModel[]> lResult =
                await RequestHandler.ApiRequest(this._user == null
                    ? ApiRequestBuilder.InfoGetComments(this._mediaObject.Id,
                        nextPage, ResultsPerPage, this._sort)
                    : ApiRequestBuilder.UserGetLatestComments(this._user.Id, nextPage, ResultsPerPage,
                        typeof(T).GetTypeInfo().Name.ToLower(), 0));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<Comment<T>>>(lResult.Exceptions);
            CommentDataModel[] lData = lResult.Result;

            if ((this._user != null) && lData.Any()) this.InitialiseUserValues(lData.First());
            return new ProxerResult<IEnumerable<Comment<T>>>(this.ToCommentList(lData).ToArray());
        }

        private void InitialiseUserValues(CommentDataModel dataModel)
        {
            if (!this._user.UserName.IsInitialisedOnce)
                (this._user.UserName as InitialisableProperty<string>)?.SetInitialisedObject(dataModel.Username);
            if (!this._user.Avatar.IsInitialisedOnce)
                (this._user.Avatar as InitialisableProperty<Uri>)?.SetInitialisedObject(
                    new Uri("http://cdn.proxer.me/avatar/" + dataModel.Avatar));
        }

        private IEnumerable<Comment<T>> ToCommentList(IEnumerable<CommentDataModel> dataModels)
        {
            List<Comment<T>> lCommentList = new List<Comment<T>>();
            foreach (CommentDataModel commentDataModel in dataModels)
            {
                T lMediaObject = this._mediaObject;
                if (lMediaObject == null)
                {
                    if (typeof(T) == typeof(Anime))
                        lMediaObject =
                            (T) Convert.ChangeType(new Anime(commentDataModel.EntryId), typeof(T));
                    if (typeof(T) == typeof(Manga))
                        lMediaObject =
                            (T) Convert.ChangeType(new Manga(commentDataModel.EntryId), typeof(T));
                }
                lCommentList.Add(new Comment<T>(commentDataModel, lMediaObject, this._user));
            }
            return lCommentList;
        }

        #endregion
    }
}