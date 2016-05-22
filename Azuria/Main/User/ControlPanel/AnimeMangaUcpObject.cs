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
    ///     Represents an entry in the <see cref="UserControlPanel" /> of an <see cref="Anime" /> or <see cref="Manga" />.
    /// </summary>
    /// <typeparam name="T">Specifies if the entry is an <see cref="Anime" /> or <see cref="Manga" />.</typeparam>
    public class AnimeMangaUcpObject<T> : AnimeMangaProgressObject<T> where T : IAnimeMangaObject
    {
        internal AnimeMangaUcpObject([NotNull] Azuria.User user, [NotNull] T animeMangaObject, int entryId,
            AnimeMangaProgress progress, AnimeMangaProgressState progressState, [NotNull] Senpai senpai)
            : base(user, animeMangaObject, entryId, progress, progressState, senpai)
        {
            this.Comment = new InitialisableProperty<EditableComment<T>>(this.GetEditableComment);
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
        ///     Gets an editable comment associated with this entry.
        /// </summary>
        [NotNull]
        public new InitialisableProperty<EditableComment<T>> Comment { get; }

        /// <summary>
        ///     Gets a progress object that can be used to get and set the progress of the current entry.
        /// </summary>
        [NotNull]
        public new EditableAnimeMangaProgress Progress { get; }

        #endregion

        #region

        /// <summary>
        ///     Deletes the entry from the server and optionaly localy from an <see cref="UserControlPanel" />-object.
        /// </summary>
        /// <param name="userControlPanel">
        ///     An UCP-object of the senpai this object was created with to delete this entry from. This
        ///     parameter is optional. The entry will be deleted from the server anyway.
        /// </param>
        /// <returns>If the action was successfull.</returns>
        public async Task<ProxerResult> DeleteEntry([CanBeNull] UserControlPanel userControlPanel = null)
        {
            ProxerResult<EditableComment<T>> lEditableCommentFetchResult = await this.Comment.GetObject();
            if (!lEditableCommentFetchResult.Success || lEditableCommentFetchResult.Result == null)
                return new ProxerResult(lEditableCommentFetchResult.Exceptions);

            ProxerResult lDeleteResult = await lEditableCommentFetchResult.Result.Delete();
            if (!lDeleteResult.Success) return lDeleteResult;

            userControlPanel?.DeleteEntry(this);

            return new ProxerResult();
        }

        private async Task<ProxerResult<int>> GetCurrentProgress()
        {
            ProxerResult<EditableComment<T>> lEditableCommentFetchResult = await this.Comment.GetObject();
            if (!lEditableCommentFetchResult.Success || lEditableCommentFetchResult.Result == null)
                return new ProxerResult<int>(lEditableCommentFetchResult.Exceptions);

            return await lEditableCommentFetchResult.Result.Progress.CurrentProgress.Get();
        }

        private async Task<ProxerResult> GetEditableComment()
        {
            ProxerResult<EditableComment<T>> lCommentFetchResult =
                await EditableComment<T>.GetEditableComment(this.EntryId, this.AnimeMangaObject, this.Senpai);
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
            ProxerResult<EditableComment<T>> lEditableCommentFetchResult = await this.Comment.GetObject();
            if (!lEditableCommentFetchResult.Success || lEditableCommentFetchResult.Result == null)
                return new ProxerResult(lEditableCommentFetchResult.Exceptions);

            return await lEditableCommentFetchResult.Result.Progress.CurrentProgress.Set(newProgress);
        }

        #endregion
    }
}