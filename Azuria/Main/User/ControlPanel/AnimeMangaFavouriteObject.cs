using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.Main.User.ControlPanel
{
    /// <summary>
    /// </summary>
    public class AnimeMangaFavouriteObject
    {
        private readonly Senpai _senpai;

        internal AnimeMangaFavouriteObject(int entryId, IAnimeMangaObject animeMangaObject, [NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.EntryId = entryId;
            this.AnimeMangaObject = animeMangaObject;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public IAnimeMangaObject AnimeMangaObject { get; private set; }

        /// <summary>
        /// </summary>
        public int EntryId { get; set; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        public async Task<ProxerResult> DeleteEntry()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "https://proxer.me/ucp?format=json&type=deleteFavorite&id=" + this.EntryId,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                Dictionary<string, string> responseDes =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                return responseDes["error"].Equals("0") ? new ProxerResult() : new ProxerResult {Success = false};
            }
            catch
            {
                return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse).Exceptions);
            }
        }

        #endregion
    }
}