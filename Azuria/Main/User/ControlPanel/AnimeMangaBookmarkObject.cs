using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Main.Minor;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.Main.User.ControlPanel
{
    /// <summary>
    ///     Represents an <see cref="Anime" /> or <see cref="Manga" /> the user has bookmarked for himself.
    /// </summary>
    /// <typeparam name="T">Specifies if the bookmark is an <see cref="Anime" /> or <see cref="Manga" />.</typeparam>
    public class AnimeMangaBookmarkObject<T> where T : IAnimeMangaObject
    {
        private readonly Senpai _senpai;

        internal AnimeMangaBookmarkObject([NotNull] IAnimeMangaContent<T> animeMangaContentObject, int entryId,
            [NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.AnimeMangaContentObject = animeMangaContentObject;
            this.EntryId = entryId;
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
                        new Uri("https://proxer.me/ucp?format=json&type=deleteReminder&id=" + this.EntryId),
                        this._senpai.LoginCookies,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                Dictionary<string, string> responseDes =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (!responseDes["error"].Equals("0")) return new ProxerResult {Success = false};

                userControlPanel?.DeleteBookmark(this);
                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse).Exceptions);
            }
        }

        [NotNull]
        internal static ProxerResult<AnimeMangaBookmarkObject<T>> ParseNodeFromUcp([NotNull] HtmlNode node,
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

                IAnimeMangaContentBase lAnimeMangaObject = null;

                string lAnimeMangaType = node.FirstChild.ChildNodes[2].InnerText;
                switch (lAnimeMangaType)
                {
                    case "Animeserie":
                    case "Movie":
                    case "OVA":
                        AnimeLanguage lAnimeLanguage = AnimeLanguage.Unknown;
                        switch (node.FirstChild.ChildNodes[3].InnerText.ToLower())
                        {
                            case "engsub":
                                lAnimeLanguage = AnimeLanguage.EngSub;
                                break;
                            case "engdub":
                                lAnimeLanguage = AnimeLanguage.EngDub;
                                break;
                            case "gersub":
                                lAnimeLanguage = AnimeLanguage.GerSub;
                                break;
                            case "gerdub":
                                lAnimeLanguage = AnimeLanguage.GerDub;
                                break;
                        }
                        if (lAnimeLanguage == AnimeLanguage.Unknown)
                            return new ProxerResult<AnimeMangaBookmarkObject<T>>(new Exception[0]);
                        lAnimeMangaObject = new Anime.Episode(new Anime(lAnimeMangaTitle, lAnimeMangaId, senpai),
                            lNumber, lAnimeLanguage, lIsOnline, senpai);
                        break;
                    case "Mangaserie":
                    case "One-Shot":
                        Language lMangaLanguage = Language.Unkown;
                        switch (node.FirstChild.ChildNodes[3].InnerText.ToLower())
                        {
                            case "deutsch":
                                lMangaLanguage = Language.German;
                                break;
                            case "englisch":
                                lMangaLanguage = Language.English;
                                break;
                        }
                        lAnimeMangaObject = new Manga.Chapter(new Manga(lAnimeMangaTitle, lAnimeMangaId, senpai),
                            lNumber, lMangaLanguage, lIsOnline, senpai);
                        break;
                }

                if (lAnimeMangaObject == null) return new ProxerResult<AnimeMangaBookmarkObject<T>>(new Exception[0]);
                if ((typeof(T) == typeof(Anime) && lAnimeMangaObject is Anime.Episode) ||
                    (typeof(T) == typeof(Manga) && lAnimeMangaObject is Manga.Chapter))
                    return
                        new ProxerResult<AnimeMangaBookmarkObject<T>>(
                            new AnimeMangaBookmarkObject<T>((IAnimeMangaContent<T>) lAnimeMangaObject, lEntryId, senpai));

                return new ProxerResult<AnimeMangaBookmarkObject<T>>(new Exception[0]);
            }
            catch
            {
                return new ProxerResult<AnimeMangaBookmarkObject<T>>(new[] {new WrongResponseException()});
            }
        }

        #endregion
    }
}