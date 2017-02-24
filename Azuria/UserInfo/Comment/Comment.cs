using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.User;
using Azuria.Api.v1.RequestBuilder;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.UserInfo.Comment
{
    /// <summary>
    /// Represents a comment for an <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see>.
    /// </summary>
    public class Comment<T> : IComment where T : IMediaObject
    {
        internal Comment(CommentDataModel dataModel, T mediaObject, User user = null)
        {
            this.MediaObject = mediaObject;
            this.Author = user ?? new User(dataModel.Username, dataModel.UserId,
                new Uri("https://cdn.proxer.me/avatar/" + dataModel.Avatar));
            this.Content = dataModel.CommentContent;
            this.Id = dataModel.CommentId;
            this.Progress = dataModel.ContentIndex;
            this.ProgressState = dataModel.State;
            this.Rating = dataModel.OverallRating;
            this.SubRatings = dataModel.SubRatings;
            this.Upvotes = dataModel.Upvotes;
        }

        internal Comment(ListDataModel dataModel, User author, T mediaObject)
        {
            this.MediaObject = mediaObject;
            this.Author = author;
            this.Content = dataModel.CommentContent;
            this.Id = dataModel.CommentId;
            this.Progress = dataModel.CommentContentIndex;
            this.ProgressState = dataModel.CommentState;
            this.Rating = dataModel.Rating;
            this.SubRatings = dataModel.CommentSubRatings;
        }

        #region Properties

        /// <summary>
        /// Gets the author of this comment.
        /// </summary>
        public User Author { get; }

        /// <summary>
        /// Gets the content of this comment.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> this comment is for.
        /// </summary>
        public T MediaObject { get; }

        IMediaObject IComment.MediaObject => this.MediaObject;

        /// <summary>
        /// </summary>
        public int Progress { get; private set; }

        /// <summary>
        /// Gets the category the author has put his progress of the <see cref="Anime" /> or <see cref="Manga" /> in.
        /// </summary>
        public MediaProgressState ProgressState { get; private set; }

        /// <summary>
        /// Gets the overall rating the <see cref="Author" /> gave. Returns -1 if no rating was found.
        /// </summary>
        public int Rating { get; }

        /// <summary>
        /// Gets the rating of all subcategories.
        /// </summary>
        public Dictionary<RatingCategory, int> SubRatings { get; }

        /// <summary>
        /// </summary>
        public int Upvotes { get; } = -1;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public async Task<IProxerResult> SetProgress(int progress, Senpai senpai)
        {
            if (senpai.Me == null) return new ProxerResult(new[] {new ArgumentNullException(nameof(senpai.Me))});
            if (senpai.Me.Id != this.Author.Id)
                return new ProxerResult(
                    new[] {new ArgumentException($"{nameof(senpai)} is not the author of this comment!")});
            if (progress < 0) return new ProxerResult(new[] {new ArgumentException(nameof(progress))});

            ProxerApiResponse lResult = await RequestHandler.ApiRequest(
                    UcpRequestBuilder.SetCommentState(this.Id, progress, senpai))
                .ConfigureAwait(false);
            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);

            if (progress < await this.MediaObject.ContentCount.Get(int.MaxValue).ConfigureAwait(false))
                return new ProxerResult();

            this.Progress = await this.MediaObject.ContentCount.Get(int.MaxValue).ConfigureAwait(false);
            this.ProgressState = MediaProgressState.Finished;

            return new ProxerResult();
        }

        #endregion
    }
}