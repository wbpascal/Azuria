using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.Main.User.ControlPanel
{
    /// <summary>
    /// </summary>
    public class AnimeMangaBookmarkObject
    {
        private readonly Senpai _senpai;

        internal AnimeMangaBookmarkObject([NotNull] IAnimeMangaObject animeMangaObject, int number, bool isOnline,
            int entryId,
            [NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.AnimeMangaObject = animeMangaObject;
            this.Number = number;
            this.IsOnline = isOnline;
            this.EntryId = entryId;
        }

        #region Properties

        /// <summary>
        /// </summary>
        [NotNull]
        public IAnimeMangaObject AnimeMangaObject { get; }

        /// <summary>
        /// </summary>
        public int EntryId { get; }

        /// <summary>
        /// </summary>
        public bool IsOnline { get; }

        /// <summary>
        ///     Die Kapitel- oder Episodennummber.
        /// </summary>
        public int Number { get; }

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
                        "https://proxer.me/ucp?format=json&type=deleteReminder&id=" + this.EntryId,
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

        [NotNull]
        [ItemCanBeNull]
        internal static ProxerResult<AnimeMangaBookmarkObject> ParseNode([NotNull] HtmlNode node,
            [NotNull] Senpai senpai)
        {
            try
            {
                int lEntryId = Convert.ToInt32(node.GetAttributeValue("id", "entry-1").Substring("entry".Length));
                int lAnimeMangaId =
                    Convert.ToInt32(
                        node.FirstChild.GetAttributeValue("href", "/watch/-1/").GetTagContents("/", "/")[1]);
                string lAnimeMangaTitle = node.FirstChild.ChildNodes[1].InnerText;

                int lNumber =
                    Convert.ToInt32(
                        node.FirstChild.GetAttributeValue("href", lAnimeMangaId + "/-1/")
                            .GetTagContents(lAnimeMangaId + "/", "/")
                            .First());
                bool lIsOnline = node.FirstChild.FirstChild.GetAttributeValue("src", "").Contains("onlineicon");

                IAnimeMangaObject lAnimeMangaObject = null;

                string lAnimeMangaType = node.FirstChild.ChildNodes[2].InnerText;
                switch (lAnimeMangaType)
                {
                    case "Animeserie":
                    case "Movie":
                    case "OVA":
                        lAnimeMangaObject = new Anime(lAnimeMangaTitle, lAnimeMangaId, senpai);
                        break;
                    case "Mangaserie":
                    case "One-Shot":
                        lAnimeMangaObject = new Manga(lAnimeMangaTitle, lAnimeMangaId, senpai);
                        break;
                }

                return lAnimeMangaObject == null
                    ? new ProxerResult<AnimeMangaBookmarkObject>(new Exception[] {new WrongResponseException()})
                    : new ProxerResult<AnimeMangaBookmarkObject>(new AnimeMangaBookmarkObject(lAnimeMangaObject, lNumber,
                        lIsOnline, lEntryId, senpai));
            }
            catch
            {
                return new ProxerResult<AnimeMangaBookmarkObject>(new[] {new WrongResponseException()});
            }
        }

        #endregion
    }
}