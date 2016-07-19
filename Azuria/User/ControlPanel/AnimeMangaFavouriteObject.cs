using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Web;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    ///     Represents an <see cref="Anime" /> or <see cref="Manga" /> which is a favourite of the user.
    /// </summary>
    /// <typeparam name="T">Specifies if the favourite is an <see cref="Anime" /> or <see cref="Manga" />.</typeparam>
    public class AnimeMangaFavouriteObject<T> where T : IAnimeMangaObject
    {
        private readonly Senpai _senpai;

        internal AnimeMangaFavouriteObject(int entryId, [NotNull] T animeMangaObject, [NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.EntryId = entryId;
            this.AnimeMangaObject = animeMangaObject;
        }

        #region Properties

        /// <summary>
        ///     Gets the <see cref="Anime" /> or <see cref="Manga" /> which is a favourite of the user.
        /// </summary>
        public T AnimeMangaObject { get; }

        /// <summary>
        ///     Gets the entry id of this favourite.
        /// </summary>
        public int EntryId { get; }

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
        [ItemNotNull]
        public async Task<ProxerResult> DeleteEntry([CanBeNull] UserControlPanel userControlPanel = null)
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/ucp?format=json&type=deleteFavorite&id=" + this.EntryId),
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                Dictionary<string, string> responseDes =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (!responseDes["error"].Equals("0")) return new ProxerResult {Success = false};

                userControlPanel?.DeleteFavourite(this);
                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse).Exceptions);
            }
        }

        #endregion
    }
}