using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.User;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.User.Comment
{
    /// <summary>
    ///     Represents a comment for an <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see>.
    /// </summary>
    public class Comment<T> where T : IAnimeMangaObject
    {
        internal Comment([NotNull] CommentDataModel dataModel, T animeMangaObject, [CanBeNull] User user = null)
        {
            this.AnimeMangaObject = animeMangaObject;
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

        internal Comment([NotNull] ListDataModel dataModel, [NotNull] User author, [NotNull] T animeMangaObject)
        {
            this.AnimeMangaObject = animeMangaObject;
            this.Author = author;
            this.Content = dataModel.CommentContent;
            this.Id = dataModel.CommentId;
            this.Progress = dataModel.CommentContentIndex;
            this.ProgressState = dataModel.AuthorState;
            this.Rating = dataModel.Rating;
            this.SubRatings = dataModel.CommentSubRatings;
        }

        #region Properties

        /// <summary>
        ///     Gets the <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> this comment is for.
        /// </summary>
        public T AnimeMangaObject { get; }

        /// <summary>
        ///     Gets the author of this comment.
        /// </summary>
        [NotNull]
        public User Author { get; }

        /// <summary>
        ///     Gets the content of this comment.
        /// </summary>
        [NotNull]
        public string Content { get; }

        /// <summary>
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// </summary>
        public int Progress { get; private set; }

        /// <summary>
        ///     Gets the category the author has put his progress of the <see cref="Anime" /> or <see cref="Manga" /> in.
        /// </summary>
        public AnimeMangaProgressState ProgressState { get; private set; }

        /// <summary>
        ///     Gets the overall rating the <see cref="Author" /> gave. Returns -1 if no rating was found.
        /// </summary>
        public int Rating { get; }

        /// <summary>
        ///     Gets the rating of all subcategories.
        /// </summary>
        [NotNull]
        public Dictionary<RatingCategory, int> SubRatings { get; }

        /// <summary>
        /// </summary>
        public int Upvotes { get; } = -1;

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public async Task<ProxerResult> SetProgress(int progress, Senpai senpai)
        {
            if (senpai.Me == null) return new ProxerResult(new[] {new ArgumentNullException(nameof(senpai.Me))});
            if (senpai.Me.Id != this.Author.Id)
                return
                    new ProxerResult(new[]
                    {new ArgumentException($"{nameof(senpai)} is not the author of this comment!")});
            if (progress < 0) return new ProxerResult(new[] {new ArgumentException(nameof(progress))});

            ProxerResult<ProxerApiResponse> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.UcpSetProgress(this.Id, progress, senpai));
            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);

            if (progress >= await this.AnimeMangaObject.ContentCount.GetObject(int.MaxValue))
            {
                this.Progress = await this.AnimeMangaObject.ContentCount.GetObject(int.MaxValue);
                this.ProgressState = AnimeMangaProgressState.Finished;
            }

            return new ProxerResult();
        }

        #endregion
    }
}