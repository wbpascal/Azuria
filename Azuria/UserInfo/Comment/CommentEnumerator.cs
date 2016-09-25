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
    internal class CommentEnumerator<T> : PageEnumerator<Comment<T>> where T : IAnimeMangaObject
    {
        private const int ResultsPerPage = 25;
        private readonly T _animeMangaObject;
        private readonly string _sort;
        private readonly User _user;

        internal CommentEnumerator(T animeMangaObject, string sort) : base(ResultsPerPage)
        {
            this._animeMangaObject = animeMangaObject;
            this._sort = sort;
        }

        internal CommentEnumerator(User user)
        {
            this._user = user;
        }

        #region Methods

        internal override async Task<ProxerResult<IEnumerable<Comment<T>>>> GetNextPage(int nextPage)
        {
            ProxerResult<ProxerApiResponse<CommentDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(this._user == null
                        ? ApiRequestBuilder.InfoGetComments(this._animeMangaObject.Id,
                            nextPage, ResultsPerPage, this._sort)
                        : ApiRequestBuilder.UserGetLatestComments(this._user.Id, nextPage, ResultsPerPage,
                            typeof(T).GetTypeInfo().Name.ToLower(), 0));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<Comment<T>>>(lResult.Exceptions);
            CommentDataModel[] lData = lResult.Result.Data;

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
                T lAnimeMangaObject = this._animeMangaObject;
                if (lAnimeMangaObject == null)
                {
                    if (typeof(T) == typeof(Anime))
                        lAnimeMangaObject =
                            (T) Convert.ChangeType(new Anime(commentDataModel.EntryId), typeof(T));
                    if (typeof(T) == typeof(Manga))
                        lAnimeMangaObject =
                            (T) Convert.ChangeType(new Manga(commentDataModel.EntryId), typeof(T));
                }
                lCommentList.Add(new Comment<T>(commentDataModel, lAnimeMangaObject, this._user));
            }
            return lCommentList;
        }

        #endregion
    }
}