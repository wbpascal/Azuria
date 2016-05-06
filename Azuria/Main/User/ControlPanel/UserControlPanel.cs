using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Main.Minor;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using Azuria.Utilities.Properties;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.Main.User.ControlPanel
{
    /// <summary>
    /// </summary>
    public class UserControlPanel
    {
        private readonly Senpai _senpai;

        /// <summary>
        /// </summary>
        /// <exception cref="NotLoggedInException">Raised when <paramref name="senpai" /> is not logged in.</exception>
        /// <param name="senpai"></param>
        public UserControlPanel([NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            if (!this._senpai.IsLoggedIn) throw new NotLoggedInException(this._senpai);

            this.Anime = new InitialisableProperty<IEnumerable<AnimeMangaUcpObject<Anime>>>(this.InitAnime);
            this.AnimeBookmarks =
                new InitialisableProperty<IEnumerable<AnimeMangaBookmarkObject<Anime>>>(this.InitBookmarks);
            this.Chronic = new InitialisableProperty<IEnumerable<AnimeMangaChronicObject>>(this.InitChronic);
            this.Favourites =
                new InitialisableProperty<IEnumerable<AnimeMangaFavouriteObject>>(this.InitFavourites);
            this.Manga = new InitialisableProperty<IEnumerable<AnimeMangaUcpObject<Manga>>>(this.InitManga);
            this.MangaBookmarks =
                new InitialisableProperty<IEnumerable<AnimeMangaBookmarkObject<Manga>>>(this.InitBookmarks);
        }

        #region Properties

        /// <summary>
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaUcpObject<Anime>>> Anime { get; }

        /// <summary>
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaBookmarkObject<Anime>>> AnimeBookmarks { get; }

        /// <summary>
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaChronicObject>> Chronic { get; }

        /// <summary>
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaFavouriteObject>> Favourites { get; }

        /// <summary>
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaUcpObject<Manga>>> Manga { get; }

        /// <summary>
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaBookmarkObject<Manga>>> MangaBookmarks { get; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="animeMangaContent"></param>
        /// <returns></returns>
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
                        this._senpai.ErrHandler,
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
                                    o.ContentObject.ParentObject.Id == animeMangaContent.ParentObject.Id &&
                                    o.ContentObject.GeneralLanguage == animeMangaContent.GeneralLanguage &&
                                    o.ContentObject.ContentIndex == animeMangaContent.ContentIndex) as
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
                                    o.ContentObject.ParentObject.Id == animeMangaContent.ParentObject.Id &&
                                    o.ContentObject.GeneralLanguage == animeMangaContent.GeneralLanguage &&
                                    o.ContentObject.ContentIndex == animeMangaContent.ContentIndex) as
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

        internal void DeleteEntry<T>(int entryId) where T : IAnimeMangaObject
        {
            if (typeof(T) == typeof(Anime) && this.Anime.IsInitialisedOnce)
            {
                List<AnimeMangaUcpObject<Anime>> lAnimeList =
                    this.Anime.GetObjectIfInitialised(new AnimeMangaUcpObject<Anime>[0]).ToList();
                if (!lAnimeList.Any()) return;

                lAnimeList.RemoveAll(ucpObject => ucpObject?.EntryId == entryId);
                this.Anime.SetInitialisedObject(lAnimeList);
            }
            else if (typeof(T) == typeof(Manga) && this.Manga.IsInitialisedOnce)
            {
                List<AnimeMangaUcpObject<Manga>> lMangaList =
                    this.Manga.GetObjectIfInitialised(new AnimeMangaUcpObject<Manga>[0]).ToList();
                if (!lMangaList.Any()) return;

                lMangaList.RemoveAll(ucpObject => ucpObject?.EntryId == entryId);
                this.Manga.SetInitialisedObject(lMangaList);
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
                        this._senpai.ErrHandler,
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
                        this._senpai.ErrHandler,
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
                        AnimeMangaBookmarkObject<Anime>.ParseNode(lBookmarkNode, this._senpai);
                    ProxerResult<AnimeMangaBookmarkObject<Manga>> lMangaParseResult =
                        AnimeMangaBookmarkObject<Manga>.ParseNode(lBookmarkNode, this._senpai);

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
            List<AnimeMangaChronicObject> lChronicObjects = new List<AnimeMangaChronicObject>();

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
                        this._senpai.ErrHandler,
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
                    ProxerResult<AnimeMangaChronicObject> lParseResult =
                        AnimeMangaChronicObject.GetChronicObjectFromNode(chronicNode, this._senpai, true);

                    if (lParseResult.Success) lChronicObjects.Add(lParseResult.Result);
                    else lFailedParses++;
                }

                if (lParses == lFailedParses) return new ProxerResult {Success = false};
            }
            catch
            {
                return
                    new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            this.Chronic.SetInitialisedObject(lChronicObjects);
            return new ProxerResult();
        }

        [ItemNotNull]
        private async Task<ProxerResult> InitFavourites()
        {
            HtmlDocument lDocument = new HtmlDocument();
            List<AnimeMangaFavouriteObject> lAnimeMangaFavouriteObjects = new List<AnimeMangaFavouriteObject>();

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
                        this._senpai.ErrHandler,
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
                    ProxerResult<AnimeMangaFavouriteObject> lParseResult = this.ParseFavourite<Anime>(lListNode);
                    if (lParseResult.Success) lAnimeMangaFavouriteObjects.Add(lParseResult.Result);
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
                    ProxerResult<AnimeMangaFavouriteObject> lParseResult = this.ParseFavourite<Manga>(lListNode);
                    if (lParseResult.Success) lAnimeMangaFavouriteObjects.Add(lParseResult.Result);
                    else lFailedParses++;
                }

                if (lParses == lFailedParses) return new ProxerResult {Success = false};
            }
            catch
            {
                return
                    new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            this.Favourites.SetInitialisedObject(lAnimeMangaFavouriteObjects);
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
                        this._senpai.ErrHandler,
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

        private ProxerResult<AnimeMangaFavouriteObject> ParseFavourite<T>([NotNull] HtmlNode htmlNode)
            where T : IAnimeMangaObject
        {
            try
            {
                HtmlNode lInfoNode =
                    htmlNode.ChildNodes.First(
                        node => node.Name == "a" && node.GetAttributeValue("href", "").StartsWith("/info"));

                if (lInfoNode == null) return new ProxerResult<AnimeMangaFavouriteObject>(new Exception[0]);

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
                    ? new ProxerResult<AnimeMangaFavouriteObject>(new Exception[0])
                    : new ProxerResult<AnimeMangaFavouriteObject>(new AnimeMangaFavouriteObject(lEntryId,
                        lAnimeMangaObject, this._senpai));
            }
            catch
            {
                return new ProxerResult<AnimeMangaFavouriteObject>(new Exception[0]);
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