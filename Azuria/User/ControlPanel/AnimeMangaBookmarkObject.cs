using System;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    ///     Represents an <see cref="Anime" /> or <see cref="Manga" /> the user has bookmarked for himself.
    /// </summary>
    /// <typeparam name="T">Specifies if the bookmark is an <see cref="Anime" /> or <see cref="Manga" />.</typeparam>
    public class AnimeMangaBookmarkObject<T> where T : class, IAnimeMangaObject
    {
        private readonly Senpai _senpai;

        internal AnimeMangaBookmarkObject(IAnimeMangaContent<T> animeMangaContentObject, BookmarkDataModel dataModel,
            Senpai senpai, UserControlPanel controlPanel)
        {
            this._senpai = senpai;
            this.AnimeMangaContentObject = animeMangaContentObject;
            this.EntryId = dataModel.BookmarkId;
            this.UserControlPanel = controlPanel;
        }

        #region Properties

        /// <summary>
        ///     Gets the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> the user has bookmarked.
        /// </summary>
        [NotNull]
        public IAnimeMangaContent<T> AnimeMangaContentObject { get; }

        /// <summary>
        ///     Gets the entry id of this bookmark.
        /// </summary>
        public int EntryId { get; }

        /// <summary>
        /// </summary>
        public UserControlPanel UserControlPanel { get; }

        #endregion

        #region

        /// <summary>
        ///     Deletes the entry from the server and optionaly localy from an <see cref="UserControlPanel" />-object.
        /// </summary>
        /// <returns>If the action was successfull.</returns>
        [ItemNotNull]
        public async Task<ProxerResult> DeleteEntry()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}