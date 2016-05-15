using System;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;
using Azuria.Utilities.Properties;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Main.User
{
    /// <summary>
    ///     Eine Klasse, die den Fortschritt eines <see cref="Azuria.User">Benutzer</see> bei einem
    ///     <see cref="Anime">Anime</see>
    ///     oder <see cref="Manga">Manga</see> darstellt.
    /// </summary>
    public class AnimeMangaProgressObject<T> where T : IAnimeMangaObject
    {
        /// <summary>
        /// </summary>
        [NotNull] protected readonly Senpai Senpai;

        /// <summary>
        ///     Initialisiert das Objekt.
        /// </summary>
        /// <param name="user">Der Benutzer, der mit dem Fortschritt zusammenhängt.</param>
        /// <param name="animeMangaObject">
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see>, mit dem das Objekt
        ///     zusammenhängt.
        /// </param>
        /// <param name="entryId"></param>
        /// <param name="progress">Der aktuelle Fortschritt.</param>
        /// <param name="progressState">
        ///     Die Kategorie, in der der <paramref name="user">Benutzer</paramref> seinen Fortschritt
        ///     einsortiert hat.
        /// </param>
        /// <param name="senpai"></param>
        public AnimeMangaProgressObject([NotNull] Azuria.User user, [NotNull] T animeMangaObject,
            int entryId, AnimeMangaProgress progress, AnimeMangaProgressState progressState, [NotNull] Senpai senpai)
        {
            this.Senpai = senpai;
            this.User = user;
            this.AnimeMangaObject = animeMangaObject;
            this.EntryId = entryId;
            this.Progress = progress;
            this.ProgressState = progressState;

            this.Comment = new InitialisableProperty<Comment.Comment>(this.InitComment);
        }

        #region Properties

        /// <summary>
        ///     Gibt den <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> zurück, mit dem das Objekt zusammenhängt.
        /// </summary>
        [NotNull]
        public T AnimeMangaObject { get; }

        /// <summary>
        /// </summary>
        [NotNull]
        public InitialisableProperty<Comment.Comment> Comment { get; }

        /// <summary>
        /// </summary>
        public int EntryId { get; set; }

        /// <summary>
        ///     Gibt den aktuellen Fortschritt aus.
        /// </summary>
        public AnimeMangaProgress Progress { get; private set; }

        /// <summary>
        ///     Gibt die Kategorie, in der der <see cref="User">Benutzer</see> seinen Fortschritt einsortiert hat zurück.
        /// </summary>
        public AnimeMangaProgressState ProgressState { get; }

        /// <summary>
        ///     Gibt den <see cref="Azuria.User">Benutzer</see> zurück, mit dem der Fortschritt zusammenhängt.
        /// </summary>
        [NotNull]
        public Azuria.User User { get; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <returns></returns>
        protected async Task<ProxerResult<Comment.Comment>> GetComment()
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
                return new ProxerResult<Comment.Comment>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lInnerDiv = lDocument.GetElementbyId("inner");
                if (lInnerDiv == null) return new ProxerResult<Comment.Comment>(new[] {new WrongResponseException()});

                ProxerResult<Comment.Comment> lParseResult =
                    Main.User.Comment.Comment.ParseComment(lInnerDiv.ChildNodes.FindFirst("table"), this.Senpai, true,
                        this.User);

                if (!lParseResult.Success || lParseResult.Result == null)
                    return new ProxerResult<Comment.Comment>(lParseResult.Exceptions);

                return new ProxerResult<Comment.Comment>(lParseResult.Result);
            }
            catch
            {
                return
                    new ProxerResult<Comment.Comment>(
                        (await ErrorHandler.HandleError(this.Senpai, lResponse, false)).Exceptions);
            }
        }

        private async Task<ProxerResult> InitComment()
        {
            ProxerResult<Comment.Comment> lCommentFetchResult = await this.GetComment();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animeMangaObject"></param>
        /// <param name="user"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static async Task<AnimeMangaProgressObject<T>> GetEmptyBookmarkObjectFromAnimeMangaObject(
            [NotNull] T animeMangaObject, [NotNull] Azuria.User user, [NotNull] Senpai senpai)
        {
            return new AnimeMangaProgressObject<T>(user, animeMangaObject, -1,
                new AnimeMangaProgress(0, await animeMangaObject.ContentCount.GetObject(-1)),
                AnimeMangaProgressState.Unknown, senpai);
        }
    }
}