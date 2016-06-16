using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using Azuria.Utilities.Properties;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    ///     Represents the User-Control-Panel of a specified user.
    /// </summary>
    public class UserControlPanel
    {
        private readonly Senpai _senpai;

        /// <summary>
        ///     Inititalises a new instance of the <see cref="UserControlPanel" /> class with a specified user.
        /// </summary>
        /// <exception cref="NotLoggedInException">Raised when <paramref name="senpai" /> is not logged in.</exception>
        /// <param name="senpai">The user that owns this User-Control-Panel.</param>
        public UserControlPanel([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            if (!this._senpai.IsLoggedIn) throw new NotLoggedInException(this._senpai);

            this.Anime = new InitialisableProperty<IEnumerable<AnimeMangaUcpObject<Anime>>>(this.InitAnime);
            this.AnimeBookmarks =
                new InitialisableProperty<IEnumerable<AnimeMangaBookmarkObject<Anime>>>(this.InitBookmarks);
            this.AnimeChronic = new InitialisableProperty<IEnumerable<AnimeMangaChronicObject<Anime>>>(this.InitChronic);
            this.MangaChronic = new InitialisableProperty<IEnumerable<AnimeMangaChronicObject<Manga>>>(this.InitChronic);
            this.AnimeFavourites =
                new InitialisableProperty<IEnumerable<AnimeMangaFavouriteObject<Anime>>>(this.InitFavourites);
            this.MangaFavourites =
                new InitialisableProperty<IEnumerable<AnimeMangaFavouriteObject<Manga>>>(this.InitFavourites);
            this.Manga = new InitialisableProperty<IEnumerable<AnimeMangaUcpObject<Manga>>>(this.InitManga);
            this.MangaBookmarks =
                new InitialisableProperty<IEnumerable<AnimeMangaBookmarkObject<Manga>>>(this.InitBookmarks);
        }

        #region Properties

        /// <summary>
        ///     Gets all <see cref="AnimeManga.Anime">Anime</see> progress entries of the user.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaUcpObject<Anime>>> Anime { get; }

        /// <summary>
        ///     Gets all bookmarks of the user that are <see cref="AnimeManga.Anime">Anime</see>.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaBookmarkObject<Anime>>> AnimeBookmarks { get; }

        /// <summary>
        ///     Gets the chronic entries of the 50 most recent of the user that are <see cref="AnimeManga.Anime">Anime</see>.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaChronicObject<Anime>>> AnimeChronic { get; }

        /// <summary>
        ///     Gets all favourites of the user that are <see cref="AnimeManga.Anime">Anime</see>.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaFavouriteObject<Anime>>> AnimeFavourites { get; }

        /// <summary>
        ///     Gets all <see cref="AnimeManga.Manga">Manga</see> progress entries of the user.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaUcpObject<Manga>>> Manga { get; }

        /// <summary>
        ///     Gets all bookmarks of the user that are <see cref="AnimeManga.Manga">Manga</see>.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaBookmarkObject<Manga>>> MangaBookmarks { get; }

        /// <summary>
        ///     Gets the chronic entries of the 50 most recent of the user that are <see cref="AnimeManga.Manga">Manga</see>.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaChronicObject<Manga>>> MangaChronic { get; }

        /// <summary>
        ///     Gets all favourites of the user that are <see cref="AnimeManga.Manga">Manga</see>.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaFavouriteObject<Manga>>> MangaFavourites { get; }

        #endregion

        #region

        /// <summary>
        ///     Adds a specified <see cref="AnimeManga.Anime.Episode">Episode</see> or <see cref="AnimeManga.Manga.Chapter">Chapter</see> to
        ///     the bookmarks.
        /// </summary>
        /// <typeparam name="T">Whether an <see cref="AnimeManga.Anime">Anime</see> or <see cref="AnimeManga.Manga">Manga</see> is added.</typeparam>
        /// <param name="animeMangaContent">
        ///     The <see cref="AnimeManga.Anime.Episode">Episode</see> or
        ///     <see cref="AnimeManga.Manga.Chapter">Chapter</see> that is added.
        /// </param>
        /// <returns>If the action was successful and if it was, the entry that was added.</returns>
        public async Task<ProxerResult<AnimeMangaBookmarkObject<T>>> AddToBookmarks<T>(
            [NotNull] IAnimeMangaContent<T> animeMangaContent) where T : IAnimeMangaObject
        {
            string lLanguageString = "";
            string lCategoryString = "";

            if (typeof(T) == typeof(Anime))
            {
                lLanguageString = (animeMangaContent as Anime.Episode)?.Language.ToString().ToLower();
                lCategoryString = "watch";
            }
            else if (typeof(T) == typeof(Manga))
            {
                lLanguageString = animeMangaContent.GeneralLanguage == Language.English
                    ? "en"
                    : animeMangaContent.GeneralLanguage == Language.German ? "de" : "";
                lCategoryString = "chapter";
            }

            if (string.IsNullOrEmpty(lLanguageString) || string.IsNullOrEmpty(lCategoryString))
                return
                    new ProxerResult<AnimeMangaBookmarkObject<T>>(new[]
                    {new ArgumentException(nameof(animeMangaContent))});

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri(
                            $"https://proxer.me/{lCategoryString}/{animeMangaContent.ParentObject.Id}/{animeMangaContent.ContentIndex}/{lLanguageString}?format=json&type=reminder&title=reminder_this"),
                        this._senpai.LoginCookies,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult<AnimeMangaBookmarkObject<T>>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                Dictionary<string, string> lDeserialisedResponse =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (lDeserialisedResponse.ContainsKey("msg") &&
                    lDeserialisedResponse["msg"].StartsWith("Du bist nicht eingeloggt"))
                    return new ProxerResult<AnimeMangaBookmarkObject<T>>(new[] {new NotLoggedInException()});

                if (
                    !(lDeserialisedResponse.ContainsKey("title") && lDeserialisedResponse["title"].Equals("Watchlist") ||
                      lDeserialisedResponse["title"].Equals("Readlist")))
                    return new ProxerResult<AnimeMangaBookmarkObject<T>>(new[] {new WrongResponseException(lResponse)});

                if (typeof(T) == typeof(Anime))
                {
                    AnimeMangaBookmarkObject<T> lBookmarkReturn =
                        (await this.AnimeBookmarks.GetNewObject(new AnimeMangaBookmarkObject<Anime>[0]))
                            .FirstOrDefault(
                                o =>
                                    o.AnimeMangaContentObject.ParentObject.Id == animeMangaContent.ParentObject.Id &&
                                    o.AnimeMangaContentObject.GeneralLanguage == animeMangaContent.GeneralLanguage &&
                                    o.AnimeMangaContentObject.ContentIndex == animeMangaContent.ContentIndex) as
                            AnimeMangaBookmarkObject<T>;

                    return lBookmarkReturn == null
                        ? new ProxerResult<AnimeMangaBookmarkObject<T>>(new Exception[0])
                        : new ProxerResult<AnimeMangaBookmarkObject<T>>(lBookmarkReturn);
                }
                if (typeof(T) == typeof(Manga))
                {
                    AnimeMangaBookmarkObject<T> lBookmarkReturn =
                        (await this.MangaBookmarks.GetNewObject(new AnimeMangaBookmarkObject<Manga>[0]))
                            .FirstOrDefault(
                                o =>
                                    o.AnimeMangaContentObject.ParentObject.Id == animeMangaContent.ParentObject.Id &&
                                    o.AnimeMangaContentObject.GeneralLanguage == animeMangaContent.GeneralLanguage &&
                                    o.AnimeMangaContentObject.ContentIndex == animeMangaContent.ContentIndex) as
                            AnimeMangaBookmarkObject<T>;

                    return lBookmarkReturn == null
                        ? new ProxerResult<AnimeMangaBookmarkObject<T>>(new Exception[0])
                        : new ProxerResult<AnimeMangaBookmarkObject<T>>(lBookmarkReturn);
                }

                return new ProxerResult<AnimeMangaBookmarkObject<T>>(new Exception[0]);
            }
            catch
            {
                return
                    new ProxerResult<AnimeMangaBookmarkObject<T>>(
                        (await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        ///     Adds a specified <see cref="AnimeManga.Anime">Anime</see> or <see cref="AnimeManga.Manga">Manga</see> to the planned list.
        /// </summary>
        /// <typeparam name="T">Whether an <see cref="AnimeManga.Anime">Anime</see> or <see cref="AnimeManga.Manga">Manga</see> is added.</typeparam>
        /// <param name="animeMangaObject">
        ///     The <see cref="AnimeManga.Anime">Anime</see> or <see cref="AnimeManga.Manga">Manga</see> that is
        ///     added.
        /// </param>
        /// <returns>If the action was successful and if it was, the entry that was added.</returns>
        public async Task<ProxerResult<AnimeMangaUcpObject<T>>> AddToPlanned<T>(
            [NotNull] T animeMangaObject) where T : IAnimeMangaObject
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(
                        new Uri(
                            $"https://proxer.me/info/{animeMangaObject.Id}?format=json&json=note"),
                        new Dictionary<string, string> {{"checkPost", "1"}},
                        this._senpai.LoginCookies,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult<AnimeMangaUcpObject<T>>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                Dictionary<string, string> lDeserialisedResponse =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (lDeserialisedResponse.ContainsKey("msg") &&
                    lDeserialisedResponse["msg"].StartsWith("Du bist nicht eingeloggt"))
                    return new ProxerResult<AnimeMangaUcpObject<T>>(new[] {new NotLoggedInException()});

                if (typeof(T) == typeof(Anime))
                {
                    AnimeMangaUcpObject<T> lUcpObject =
                        (await this.Anime.GetNewObject(new AnimeMangaUcpObject<Anime>[0])).FirstOrDefault(
                            o => o.AnimeMangaObject.Id == animeMangaObject.Id) as AnimeMangaUcpObject<T>;

                    return lUcpObject == null
                        ? new ProxerResult<AnimeMangaUcpObject<T>>(new Exception[0])
                        : new ProxerResult<AnimeMangaUcpObject<T>>(lUcpObject);
                }
                if (typeof(T) == typeof(Manga))
                {
                    AnimeMangaUcpObject<T> lUcpObject =
                        (await this.Manga.GetNewObject(new AnimeMangaUcpObject<Manga>[0])).FirstOrDefault(
                            o => o.AnimeMangaObject.Id == animeMangaObject.Id) as AnimeMangaUcpObject<T>;

                    return lUcpObject == null
                        ? new ProxerResult<AnimeMangaUcpObject<T>>(new Exception[0])
                        : new ProxerResult<AnimeMangaUcpObject<T>>(lUcpObject);
                }

                return new ProxerResult<AnimeMangaUcpObject<T>>(new Exception[0]);
            }
            catch
            {
                return
                    new ProxerResult<AnimeMangaUcpObject<T>>(
                        (await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        ///     Deletes an entry from <see cref="AnimeBookmarks" /> or <see cref="MangaBookmarks" /> and simultaneously from the
        ///     server.
        /// </summary>
        /// <typeparam name="T">Whether an <see cref="AnimeManga.Anime">Anime</see> or <see cref="AnimeManga.Manga">Manga</see> is deleted.</typeparam>
        /// <param name="animeMangaBookmarkObject">The entry that is to be deleted.</param>
        public void DeleteBookmark<T>(AnimeMangaBookmarkObject<T> animeMangaBookmarkObject) where T : IAnimeMangaObject
        {
            if (typeof(T) == typeof(Anime) && this.Anime.IsInitialisedOnce)
            {
                List<AnimeMangaBookmarkObject<Anime>> lAnimeList =
                    this.AnimeBookmarks.GetObjectIfInitialised(new AnimeMangaBookmarkObject<Anime>[0]).ToList();
                if (!lAnimeList.Any()) return;

                lAnimeList.RemoveAll(favouriteObject => favouriteObject?.EntryId == animeMangaBookmarkObject.EntryId);
                this.AnimeBookmarks.SetInitialisedObject(lAnimeList);
            }
            else if (typeof(T) == typeof(Manga) && this.Manga.IsInitialisedOnce)
            {
                List<AnimeMangaBookmarkObject<Manga>> lMangaList =
                    this.MangaBookmarks.GetObjectIfInitialised(new AnimeMangaBookmarkObject<Manga>[0]).ToList();
                if (!lMangaList.Any()) return;

                lMangaList.RemoveAll(favouriteObject => favouriteObject?.EntryId == animeMangaBookmarkObject.EntryId);
                this.MangaBookmarks.SetInitialisedObject(lMangaList);
            }
        }

        /// <summary>
        ///     Deletes an entry from <see cref="Anime" /> or <see cref="Manga" /> and simultaneously from the server.
        /// </summary>
        /// <typeparam name="T">Whether an <see cref="AnimeManga.Anime">Anime</see> or <see cref="AnimeManga.Manga">Manga</see> is deleted.</typeparam>
        /// <param name="animeMangaUcpObject">The entry that is to be deleted.</param>
        public void DeleteEntry<T>(AnimeMangaUcpObject<T> animeMangaUcpObject) where T : IAnimeMangaObject
        {
            if (typeof(T) == typeof(Anime) && this.Anime.IsInitialisedOnce)
            {
                List<AnimeMangaUcpObject<Anime>> lAnimeList =
                    this.Anime.GetObjectIfInitialised(new AnimeMangaUcpObject<Anime>[0]).ToList();
                if (!lAnimeList.Any()) return;

                lAnimeList.RemoveAll(ucpObject => ucpObject?.EntryId == animeMangaUcpObject.EntryId);
                this.Anime.SetInitialisedObject(lAnimeList);
            }
            else if (typeof(T) == typeof(Manga) && this.Manga.IsInitialisedOnce)
            {
                List<AnimeMangaUcpObject<Manga>> lMangaList =
                    this.Manga.GetObjectIfInitialised(new AnimeMangaUcpObject<Manga>[0]).ToList();
                if (!lMangaList.Any()) return;

                lMangaList.RemoveAll(ucpObject => ucpObject?.EntryId == animeMangaUcpObject.EntryId);
                this.Manga.SetInitialisedObject(lMangaList);
            }
        }

        /// <summary>
        ///     Deletes an entry from <see cref="AnimeFavourites" /> or <see cref="MangaFavourites" /> and simultaneously from the
        ///     server.
        /// </summary>
        /// <typeparam name="T">Whether an <see cref="AnimeManga.Anime">Anime</see> or <see cref="AnimeManga.Manga">Manga</see> is deleted.</typeparam>
        /// <param name="animeMangaFavouriteObject">The entry that is to be deleted.</param>
        public void DeleteFavourite<T>(AnimeMangaFavouriteObject<T> animeMangaFavouriteObject)
            where T : IAnimeMangaObject
        {
            if (typeof(T) == typeof(Anime) && this.Anime.IsInitialisedOnce)
            {
                List<AnimeMangaFavouriteObject<Anime>> lAnimeList =
                    this.AnimeFavourites.GetObjectIfInitialised(new AnimeMangaFavouriteObject<Anime>[0]).ToList();
                if (!lAnimeList.Any()) return;

                lAnimeList.RemoveAll(favouriteObject => favouriteObject?.EntryId == animeMangaFavouriteObject.EntryId);
                this.AnimeFavourites.SetInitialisedObject(lAnimeList);
            }
            else if (typeof(T) == typeof(Manga) && this.Manga.IsInitialisedOnce)
            {
                List<AnimeMangaFavouriteObject<Manga>> lMangaList =
                    this.MangaFavourites.GetObjectIfInitialised(new AnimeMangaFavouriteObject<Manga>[0]).ToList();
                if (!lMangaList.Any()) return;

                lMangaList.RemoveAll(favouriteObject => favouriteObject?.EntryId == animeMangaFavouriteObject.EntryId);
                this.MangaFavourites.SetInitialisedObject(lMangaList);
            }
        }

        [ItemNotNull]
        private async Task<ProxerResult> InitAnime()
        {
            HtmlDocument lDocument = new HtmlDocument();

            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\"><h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3></div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitManga))});

                return new ProxerResult();
            };

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(new Uri("https://proxer.me/ucp?s=anime&format=raw"),
                        this._senpai.LoginCookies,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                ProxerResult<IEnumerable<AnimeMangaUcpObject<Anime>>> lProcessResult =
                    this.ProcessAnimeMangaProgressNodes<Anime>(lDocument.LoadHtmlUtility(lResponse));
                if (!lProcessResult.Success) return new ProxerResult(lProcessResult.Exceptions);
                this.Anime.SetInitialisedObject(lProcessResult.Result);
                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        [ItemNotNull]
        private async Task<ProxerResult> InitBookmarks()
        {
            HtmlDocument lDocument = new HtmlDocument();
            List<AnimeMangaBookmarkObject<Anime>> lAnimeBookmarkObjects = new List<AnimeMangaBookmarkObject<Anime>>();
            List<AnimeMangaBookmarkObject<Manga>> lMangaBookmarkObjects = new List<AnimeMangaBookmarkObject<Manga>>();

            Func<string, ProxerResult> lCheckFunc = s =>
            {
                lDocument.LoadHtml(s);
                HtmlNode lInnerDiv = lDocument.DocumentNode.Descendants()
                    .FirstOrDefault(
                        node => node.Name.Equals("div") && node.GetAttributeValue("class", "").Equals("inner"));
                if (lInnerDiv != null && lInnerDiv.FirstChild.InnerText.ToLower().StartsWith("du bist nicht eingeloggt"))
                    return new ProxerResult(new Exception[] {new NoAccessException()});

                return new ProxerResult();
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/ucp?s=reminder&format=raw"),
                        this._senpai.MobileLoginCookies,
                        this._senpai,
                        new[] {lCheckFunc});

            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);

            try
            {
                int lFailedParses = 0;
                int lParses = 0;
                foreach (
                    HtmlNode lBookmarkNode in
                        lDocument.DocumentNode.Descendants()
                            .First(
                                node =>
                                    node.Name.Equals("ul") &&
                                    node.GetAttributeValue("data-split-icon", "").Equals("delete")).ChildNodes)
                {
                    lParses++;
                    ProxerResult<AnimeMangaBookmarkObject<Anime>> lAnimeParseResult =
                        AnimeMangaBookmarkObject<Anime>.ParseNodeFromUcp(lBookmarkNode, this._senpai);
                    ProxerResult<AnimeMangaBookmarkObject<Manga>> lMangaParseResult =
                        AnimeMangaBookmarkObject<Manga>.ParseNodeFromUcp(lBookmarkNode, this._senpai);

                    if (lAnimeParseResult.Success && lAnimeParseResult.Result != null)
                        lAnimeBookmarkObjects.Add(lAnimeParseResult.Result);
                    else if (lMangaParseResult.Success && lMangaParseResult.Result != null)
                        lMangaBookmarkObjects.Add(lMangaParseResult.Result);
                    else lFailedParses++;
                }

                if (lParses == lFailedParses) return new ProxerResult {Success = false};
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResult.Result, false)).Exceptions);
            }

            this.AnimeBookmarks.SetInitialisedObject(lAnimeBookmarkObjects);
            this.MangaBookmarks.SetInitialisedObject(lMangaBookmarkObjects);
            return new ProxerResult();
        }

        [ItemNotNull]
        private async Task<ProxerResult> InitChronic()
        {
            HtmlDocument lDocument = new HtmlDocument();
            List<AnimeMangaChronicObject<Anime>> lAnimeChronicObjects = new List<AnimeMangaChronicObject<Anime>>();
            List<AnimeMangaChronicObject<Manga>> lMangaChronicObjects = new List<AnimeMangaChronicObject<Manga>>();

            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\">\n<h3>Du bist nicht eingeloggt. Bitte logge dich ein um diese Aktion durchführen zu können.</h3>\n</div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException()});

                return new ProxerResult();
            };

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(new Uri("https://proxer.me/ucp?s=history&format=raw"),
                        this._senpai.LoginCookies,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                int lFailedParses = 0;
                int lParses = 0;
                foreach (HtmlNode chronicNode in lDocument.GetElementbyId("box-table-a").ChildNodes.Skip(1))
                {
                    lParses++;
                    ProxerResult<AnimeMangaChronicObject<Anime>> lAnimeParseResult =
                        AnimeMangaChronicObject<Anime>.GetChronicObjectFromNode(chronicNode, this._senpai,
                            true);
                    ProxerResult<AnimeMangaChronicObject<Manga>> lMangaParseResult =
                        AnimeMangaChronicObject<Manga>.GetChronicObjectFromNode(chronicNode, this._senpai,
                            true);

                    if (lAnimeParseResult.Success && lAnimeParseResult.Result != null)
                    {
                        lAnimeChronicObjects.Add(lAnimeParseResult.Result);
                    }
                    else if (lMangaParseResult.Success && lMangaParseResult.Result != null)
                    {
                        lMangaChronicObjects.Add(lMangaParseResult.Result);
                    }
                    else lFailedParses++;
                }

                if (lParses == lFailedParses) return new ProxerResult {Success = false};
            }
            catch
            {
                return
                    new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            this.AnimeChronic.SetInitialisedObject(lAnimeChronicObjects);
            this.MangaChronic.SetInitialisedObject(lMangaChronicObjects);
            return new ProxerResult();
        }

        [ItemNotNull]
        private async Task<ProxerResult> InitFavourites()
        {
            HtmlDocument lDocument = new HtmlDocument();
            List<AnimeMangaFavouriteObject<Anime>> lAnimeFavouriteObjects = new List<AnimeMangaFavouriteObject<Anime>>();
            List<AnimeMangaFavouriteObject<Manga>> lMangaFavouriteObjects = new List<AnimeMangaFavouriteObject<Manga>>();

            Func<string, ProxerResult> lCheckFunc = s =>
            {
                lDocument.LoadHtml(s);
                HtmlNode lInnerDiv =
                    lDocument.DocumentNode.Descendants()
                        .FirstOrDefault(
                            node => node.Name.Equals("div") && node.GetAttributeValue("class", "").Equals("inner"));
                if (lInnerDiv != null && lInnerDiv.FirstChild.InnerText.ToLower().StartsWith("du bist nicht eingeloggt"))
                    return new ProxerResult(new Exception[] {new NoAccessException()});

                return new ProxerResult();
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/ucp?s=topten&format=raw"),
                        this._senpai.MobileLoginCookies,
                        this._senpai,
                        new[] {lCheckFunc});

            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                int lFailedParses = 0;
                int lParses = 0;
                foreach (
                    HtmlNode lListNode in
                        lDocument.DocumentNode.Descendants()
                            .First(
                                node =>
                                    node.Name.Equals("ul") &&
                                    node.GetAttributeValue("data-split-icon", "").Equals("delete"))
                            .ChildNodes.TakeWhile(node => !node.InnerText.ToLower().Equals("top 10 manga"))
                            .Where(node => node.GetAttributeValue("id", "").StartsWith("entry")))
                {
                    lParses++;
                    ProxerResult<AnimeMangaFavouriteObject<Anime>> lParseResult = this.ParseFavourite<Anime>(lListNode);
                    if (lParseResult.Success) lAnimeFavouriteObjects.Add(lParseResult.Result);
                    else lFailedParses++;
                }

                foreach (
                    HtmlNode lListNode in
                        lDocument.DocumentNode.Descendants()
                            .First(
                                node =>
                                    node.Name.Equals("ul") &&
                                    node.GetAttributeValue("data-split-icon", "").Equals("delete"))
                            .ChildNodes.SkipWhile(node => !node.InnerText.ToLower().Equals("top 10 manga"))
                            .Where(node => node.GetAttributeValue("id", "").StartsWith("entry")))
                {
                    lParses++;
                    ProxerResult<AnimeMangaFavouriteObject<Manga>> lParseResult = this.ParseFavourite<Manga>(lListNode);
                    if (lParseResult.Success) lMangaFavouriteObjects.Add(lParseResult.Result);
                    else lFailedParses++;
                }

                if (lParses == lFailedParses) return new ProxerResult {Success = false};
            }
            catch
            {
                return
                    new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            this.AnimeFavourites.SetInitialisedObject(lAnimeFavouriteObjects);
            this.MangaFavourites.SetInitialisedObject(lMangaFavouriteObjects);
            return new ProxerResult();
        }

        [ItemNotNull]
        private async Task<ProxerResult> InitManga()
        {
            HtmlDocument lDocument = new HtmlDocument();

            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\"><h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3></div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitManga))});

                return new ProxerResult();
            };

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(new Uri("https://proxer.me/ucp?s=manga&format=raw"),
                        this._senpai.LoginCookies,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                ProxerResult<IEnumerable<AnimeMangaUcpObject<Manga>>> lProcessResult =
                    this.ProcessAnimeMangaProgressNodes<Manga>(lDocument.LoadHtmlUtility(lResponse));
                if (!lProcessResult.Success) return new ProxerResult(lProcessResult.Exceptions);
                this.Manga.SetInitialisedObject(lProcessResult.Result);
                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        private ProxerResult<AnimeMangaFavouriteObject<T>> ParseFavourite<T>([NotNull] HtmlNode htmlNode)
            where T : IAnimeMangaObject
        {
            try
            {
                HtmlNode lInfoNode =
                    htmlNode.ChildNodes.First(
                        node => node.Name == "a" && node.GetAttributeValue("href", "").StartsWith("/info"));

                if (lInfoNode == null) return new ProxerResult<AnimeMangaFavouriteObject<T>>(new Exception[0]);

                int lAnimeMangaId =
                    Convert.ToInt32(lInfoNode.GetAttributeValue("href", "/info/-1").Substring("/info/".Length));
                string lAnimeMangaName = lInfoNode.ChildNodes.FindFirst("h2")?.InnerText ?? "";
                int lEntryId = Convert.ToInt32(htmlNode.GetAttributeValue("id", "entry-1").Substring("entry".Length));

                IAnimeMangaObject lAnimeMangaObject = typeof(T) == typeof(Anime)
                    ? new Anime(lAnimeMangaName, lAnimeMangaId, this._senpai)
                    : typeof(T) == typeof(Manga)
                        ? (IAnimeMangaObject) new Manga(lAnimeMangaName, lAnimeMangaId, this._senpai)
                        : null;

                return lAnimeMangaObject == null
                    ? new ProxerResult<AnimeMangaFavouriteObject<T>>(new Exception[0])
                    : new ProxerResult<AnimeMangaFavouriteObject<T>>(new AnimeMangaFavouriteObject<T>(lEntryId,
                        (T) lAnimeMangaObject, this._senpai));
            }
            catch
            {
                return new ProxerResult<AnimeMangaFavouriteObject<T>>(new Exception[0]);
            }
        }

        [NotNull]
        private ProxerResult<IEnumerable<AnimeMangaUcpObject<T>>> ProcessAnimeMangaProgressNodes<T>(
            [NotNull] HtmlDocument htmlDocument)
            where T : IAnimeMangaObject
        {
            try
            {
                ConstructorInfo lConstructorToInvoke = null;
                foreach (
                    ConstructorInfo lDeclaredConstructor in
                        from lDeclaredConstructor in typeof(T).GetTypeInfo().DeclaredConstructors
                        let lParameters = lDeclaredConstructor.GetParameters()
                        where lParameters.Length == 3 && lParameters[0].ParameterType == typeof(string) &&
                              lParameters[1].ParameterType == typeof(int) &&
                              lParameters[2].ParameterType == typeof(Senpai)
                        select lDeclaredConstructor)
                {
                    lConstructorToInvoke = lDeclaredConstructor;
                }

                if (lConstructorToInvoke == null)
                    return new ProxerResult<IEnumerable<AnimeMangaUcpObject<T>>>(new Exception[0]);

                #region Process Nodes

                List<AnimeMangaUcpObject<T>> lReturnList = new List<AnimeMangaUcpObject<T>>();
                foreach (
                    HtmlNode animeMangaNode in
                        htmlDocument.GetElementbyId("accordion").ChildNodes[7].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    object[] lActivatorParameters =
                    {
                        animeMangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeMangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1")
                                .Split(':')[1]),
                        this._senpai
                    };

                    T lAnimeManga = (T) lConstructorToInvoke.Invoke(lActivatorParameters);

                    if (lAnimeManga != null)
                    {
                        ProxerResult<AnimeMangaUcpObject<T>> lParseResult =
                            AnimeMangaUcpObject<T>.ParseFromHtmlNode(animeMangaNode, this._senpai.Me,
                                lAnimeManga,
                                AnimeMangaProgressState.Finished, this._senpai);

                        if (lParseResult.Success && lParseResult.Result != null) lReturnList.Add(lParseResult.Result);
                    }
                }

                foreach (
                    HtmlNode animeMangaNode in
                        htmlDocument.GetElementbyId("accordion").ChildNodes[11].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    object[] lActivatorParameters =
                    {
                        animeMangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeMangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1")
                                .Split(':')[1]),
                        this._senpai
                    };

                    T lAnimeManga = (T) lConstructorToInvoke.Invoke(lActivatorParameters);

                    if (lAnimeManga != null)
                    {
                        ProxerResult<AnimeMangaUcpObject<T>> lParseResult =
                            AnimeMangaUcpObject<T>.ParseFromHtmlNode(animeMangaNode, this._senpai.Me,
                                lAnimeManga,
                                AnimeMangaProgressState.InProgress, this._senpai);

                        if (lParseResult.Success && lParseResult.Result != null) lReturnList.Add(lParseResult.Result);
                    }
                }

                foreach (
                    HtmlNode animeMangaNode in
                        htmlDocument.GetElementbyId("accordion").ChildNodes[15].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    object[] lActivatorParameters =
                    {
                        animeMangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeMangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1")
                                .Split(':')[1]),
                        this._senpai
                    };

                    T lAnimeManga = (T) lConstructorToInvoke.Invoke(lActivatorParameters);

                    if (lAnimeManga != null)
                    {
                        ProxerResult<AnimeMangaUcpObject<T>> lParseResult =
                            AnimeMangaUcpObject<T>.ParseFromHtmlNode(animeMangaNode, this._senpai.Me,
                                lAnimeManga,
                                AnimeMangaProgressState.Planned, this._senpai);

                        if (lParseResult.Success && lParseResult.Result != null) lReturnList.Add(lParseResult.Result);
                    }
                }

                foreach (
                    HtmlNode animeMangaNode in
                        htmlDocument.GetElementbyId("accordion").ChildNodes[19].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    object[] lActivatorParameters =
                    {
                        animeMangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeMangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1")
                                .Split(':')[1]),
                        this._senpai
                    };

                    T lAnimeManga = (T) lConstructorToInvoke.Invoke(lActivatorParameters);

                    if (lAnimeManga != null)
                    {
                        ProxerResult<AnimeMangaUcpObject<T>> lParseResult =
                            AnimeMangaUcpObject<T>.ParseFromHtmlNode(animeMangaNode, this._senpai.Me,
                                lAnimeManga,
                                AnimeMangaProgressState.Aborted, this._senpai);

                        if (lParseResult.Success && lParseResult.Result != null) lReturnList.Add(lParseResult.Result);
                    }
                }

                #endregion

                return new ProxerResult<IEnumerable<AnimeMangaUcpObject<T>>>(lReturnList);
            }
            catch
            {
                return
                    new ProxerResult<IEnumerable<AnimeMangaUcpObject<T>>>(new Exception[] {new WrongResponseException()});
            }
        }

        #endregion
    }
}