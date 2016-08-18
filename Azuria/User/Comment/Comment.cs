using System;
using System.Collections.Generic;
using Azuria.AnimeManga;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.User;
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
        public string Content { get; protected set; }

        /// <summary>
        /// </summary>
        public int Progress { get; }

        /// <summary>
        ///     Gets the category the author has put his progress of the <see cref="Anime" /> or <see cref="Manga" /> in.
        /// </summary>
        public AnimeMangaProgressState ProgressState { get; protected set; }

        /// <summary>
        ///     Gets the overall rating the <see cref="Author" /> gave. Returns -1 if no rating was found.
        /// </summary>
        public int Rating { get; protected set; }

        /// <summary>
        ///     Gets the rating of all subcategories.
        /// </summary>
        [NotNull]
        public Dictionary<RatingCategory, int> SubRatings { get; protected set; }

        /// <summary>
        /// </summary>
        public int Upvotes { get; } = -1;

        #endregion
    }
}