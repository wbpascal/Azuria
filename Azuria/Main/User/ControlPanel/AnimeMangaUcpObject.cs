using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Main.User.Comment;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Properties;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Main.User.ControlPanel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AnimeMangaUcpObject<T> : AnimeMangaProgressObject<T> where T : IAnimeMangaObject
    {
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
        public AnimeMangaUcpObject([NotNull] Azuria.User user, [NotNull] T animeMangaObject, int entryId,
            AnimeMangaProgress progress, AnimeMangaProgressState progressState, [NotNull] Senpai senpai)
            : base(user, animeMangaObject, entryId, progress, progressState, senpai)
        {
            this.Comment = new InitialisableProperty<EditableComment>(this.GetEditableComment);
            this.Progress = new EditableAnimeMangaProgress(progress.CurrentProgress, progress.MaxProgress,
                this.GetCurrentProgress, this.SetCurrentProgress);
        }

        private AnimeMangaUcpObject([NotNull] AnimeMangaProgressObject<T> baseClass, [NotNull] Senpai senpai)
            : this(
                baseClass.User, baseClass.AnimeMangaObject, baseClass.EntryId, baseClass.Progress,
                baseClass.ProgressState, senpai)
        {
        }

        #region Properties

        /// <summary>
        /// </summary>
        [NotNull]
        public new InitialisableProperty<EditableComment> Comment { get; }

        /// <summary>
        /// </summary>
        [NotNull]
        public new EditableAnimeMangaProgress Progress { get; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<ProxerResult> DeleteEntry()
        {
            ProxerResult<EditableComment> lEditableCommentFetchResult = await this.Comment.GetObject();
            if (!lEditableCommentFetchResult.Success || lEditableCommentFetchResult.Result == null)
                return new ProxerResult(lEditableCommentFetchResult.Exceptions);

            return await lEditableCommentFetchResult.Result.Delete();
        }

        private async Task<ProxerResult<int>> GetCurrentProgress()
        {
            ProxerResult<EditableComment> lEditableCommentFetchResult = await this.Comment.GetObject();
            if (!lEditableCommentFetchResult.Success || lEditableCommentFetchResult.Result == null)
                return new ProxerResult<int>(lEditableCommentFetchResult.Exceptions);

            return await lEditableCommentFetchResult.Result.Progress.CurrentProgress.Get();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private async Task<ProxerResult> GetEditableComment()
        {
            ProxerResult<EditableComment> lCommentFetchResult =
                await EditableComment.GetEditableComment(this.EntryId, this.AnimeMangaObject.Id, this.Senpai);
            if (!lCommentFetchResult.Success || lCommentFetchResult.Result == null)
                return new ProxerResult(lCommentFetchResult.Exceptions);

            this.Comment.SetInitialisedObject(lCommentFetchResult.Result);
            return new ProxerResult();
        }

        [NotNull]
        internal new static ProxerResult<AnimeMangaUcpObject<T>> ParseFromHtmlNode([NotNull] HtmlNode node,
            Azuria.User user,
            T animeMangaObject, AnimeMangaProgressState progress,
            [NotNull] Senpai senpai)
        {
            try
            {
                return new ProxerResult<AnimeMangaUcpObject<T>>(new AnimeMangaUcpObject<T>(
                    AnimeMangaProgressObject<T>.ParseFromHtmlNode(node, user, animeMangaObject, progress, senpai),
                    senpai));
            }
            catch
            {
                return new ProxerResult<AnimeMangaUcpObject<T>>(new[] {new WrongResponseException()});
            }
        }

        private async Task<ProxerResult> SetCurrentProgress(int newProgress)
        {
            ProxerResult<EditableComment> lEditableCommentFetchResult = await this.Comment.GetObject();
            if (!lEditableCommentFetchResult.Success || lEditableCommentFetchResult.Result == null)
                return new ProxerResult(lEditableCommentFetchResult.Exceptions);

            return await lEditableCommentFetchResult.Result.Progress.CurrentProgress.Set(newProgress);
        }

        #endregion
    }
}