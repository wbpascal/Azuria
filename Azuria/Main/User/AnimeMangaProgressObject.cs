using System;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Main.User.Comment;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;
using Azuria.Utilities.Properties;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Main.User
{
    /// <summary>
    ///     Represents a progress entry of a <see cref="Azuria.User" /> of an <see cref="Anime" /> or <see cref="Manga" />.
    /// </summary>
    public class AnimeMangaProgressObject<T> where T : IAnimeMangaObject
    {
        /// <summary>
        ///     The user that is issuing requests.
        /// </summary>
        [NotNull] protected readonly Senpai Senpai;

        /// <summary>
        ///     Intitialises a new instance of the <see cref="AnimeMangaProgressObject{T}" /> class.
        /// </summary>
        /// <param name="user">The <see cref="Azuria.User" /> which the entry belongs to.</param>
        /// <param name="animeMangaObject">
        ///     The <see cref="Anime" /> or <see cref="Manga" /> this entry is about.
        /// </param>
        /// <param name="entryId">The id of the entry.</param>
        /// <param name="progress">The current progress.</param>
        /// <param name="progressState">
        ///     The category the user has categorised his progress in.
        /// </param>
        /// <param name="senpai">The user that is issuing requests. Needs to be logged in if the comment needs to be fetched.</param>
        public AnimeMangaProgressObject([NotNull] Azuria.User user, [NotNull] T animeMangaObject,
            int entryId, AnimeMangaProgress progress, AnimeMangaProgressState progressState, [NotNull] Senpai senpai)
        {
            this.Senpai = senpai;
            this.User = user;
            this.AnimeMangaObject = animeMangaObject;
            this.EntryId = entryId;
            this.Progress = progress;
            this.ProgressState = progressState;

            this.Comment = new InitialisableProperty<Comment<T>>(this.InitComment);
        }

        #region Properties

        /// <summary>
        ///     Gets the <see cref="Anime" /> or <see cref="Manga" /> the entry is about.
        /// </summary>
        [NotNull]
        public T AnimeMangaObject { get; }

        /// <summary>
        ///     Gets the comment associated with the entry.
        /// </summary>
        [NotNull]
        public InitialisableProperty<Comment<T>> Comment { get; }

        /// <summary>
        ///     Gets the id of the entry.
        /// </summary>
        public int EntryId { get; set; }

        /// <summary>
        ///     Gets the current progress.
        /// </summary>
        public AnimeMangaProgress Progress { get; private set; }

        /// <summary>
        ///     Gets the category the user has categorised his progress in.
        /// </summary>
        public AnimeMangaProgressState ProgressState { get; }

        /// <summary>
        ///     Gets the user the entry belongs to.
        /// </summary>
        [NotNull]
        public Azuria.User User { get; }

        #endregion

        #region

        /// <summary>
        ///     Fetches the comment associated with the entry.
        /// </summary>
        /// <returns>If the action was successful and if it was the comment that was fetched.</returns>
        protected async Task<ProxerResult<Comment<T>>> GetComment()
        {
            HtmlDocument lDocument = new HtmlDocument();
            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\"><h3>Bitte logge dich ein.</h3></div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitComment))});

                return new ProxerResult();
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri($"https://proxer.me/comment?id={this.EntryId}&format=raw"),
                        this.Senpai.LoginCookies,
                        this.Senpai.ErrHandler,
                        this.Senpai,
                        new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult<Comment<T>>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lInnerDiv = lDocument.GetElementbyId("inner");
                if (lInnerDiv == null) return new ProxerResult<Comment<T>>(new[] {new WrongResponseException()});

                ProxerResult<Comment<T>> lParseResult =
                    await Comment<T>.ParseComment(lInnerDiv.ChildNodes.FindFirst("table"), this.Senpai,
                        this.AnimeMangaObject, true, this.User);

                if (!lParseResult.Success || lParseResult.Result == null)
                    return new ProxerResult<Comment<T>>(lParseResult.Exceptions);

                return new ProxerResult<Comment<T>>(lParseResult.Result);
            }
            catch
            {
                return
                    new ProxerResult<Comment<T>>(
                        (await ErrorHandler.HandleError(this.Senpai, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        ///     Creates an empty <see cref="AnimeMangaProgressObject{T}" />.
        /// </summary>
        /// <param name="animeMangaObject">The <see cref="Anime" /> or <see cref="Manga" /> of the entry.</param>
        /// <param name="user">The user the entry belongs to.</param>
        /// <param name="senpai">The user that is issuing the request.</param>
        /// <returns>Returns the empty <see cref="AnimeMangaProgressObject{T}" />.</returns>
        public static async Task<AnimeMangaProgressObject<T>> GetEmptyProgressObjectFromAnimeMangaObject(
            [NotNull] T animeMangaObject, [NotNull] Azuria.User user, [NotNull] Senpai senpai)
        {
            return new AnimeMangaProgressObject<T>(user, animeMangaObject, -1,
                new AnimeMangaProgress(0, await animeMangaObject.ContentCount.GetObject(-1)),
                AnimeMangaProgressState.Unknown, senpai);
        }

        private async Task<ProxerResult> InitComment()
        {
            ProxerResult<Comment<T>> lCommentFetchResult = await this.GetComment();
            if (!lCommentFetchResult.Success || lCommentFetchResult.Result == null)
                return new ProxerResult(lCommentFetchResult.Exceptions);

            this.Comment.SetInitialisedObject(lCommentFetchResult.Result);
            return new ProxerResult();
        }

        [NotNull]
        internal static AnimeMangaProgressObject<T> ParseFromHtmlNode([NotNull] HtmlNode node, Azuria.User user,
            T animeMangaObject, AnimeMangaProgressState progress,
            [NotNull] Senpai senpai)
        {
            return new AnimeMangaProgressObject<T>(user, animeMangaObject,
                Convert.ToInt32(node.GetAttributeValue("id", "entry-1").Substring("entry".Length)),
                new AnimeMangaProgress(Convert.ToInt32(
                    node.ChildNodes[4].ChildNodes.First(
                        htmlNode => htmlNode.GetAttributeValue("class", "").Equals("state")).InnerText.Split('/')[0]
                        .Trim()),
                    Convert.ToInt32(
                        node.ChildNodes[4].ChildNodes.First(
                            htmlNode => htmlNode.GetAttributeValue("class", "").Equals("state")).InnerText.Split('/')[1]
                            .Trim())),
                progress, senpai);
        }

        #endregion
    }
}