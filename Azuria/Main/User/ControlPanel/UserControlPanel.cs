using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Initialisation;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Main.User.ControlPanel
{
    /// <summary>
    /// </summary>
    public class UserControlPanel
    {
        private readonly Senpai _senpai;

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        public UserControlPanel([NotNull] Senpai senpai)
        {
            this._senpai = senpai;

            this.Anime = new InitialisableProperty<IEnumerable<AnimeMangaUcpObject<Anime>>>(this.InitAnime);
            this.Bookmarks = new InitialisableProperty<IEnumerable<AnimeMangaBookmarkObject>>(this.InitBookmarks);
            this.Chronic = new InitialisableProperty<IEnumerable<AnimeMangaChronicObject>>(this.InitChronic);
            this.Favourites =
                new InitialisableProperty<IEnumerable<AnimeMangaFavouriteObject>>(this.InitFavourites);
            this.Manga = new InitialisableProperty<IEnumerable<AnimeMangaUcpObject<Manga>>>(this.InitManga);
        }

        #region Properties

        /// <summary>
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaUcpObject<Anime>>> Anime { get; }

        /// <summary>
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaBookmarkObject>> Bookmarks { get; }

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

        #endregion

        #region

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
            List<AnimeMangaBookmarkObject> lAnimeMangaBookmarkObjects = new List<AnimeMangaBookmarkObject>();

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
                    ProxerResult<AnimeMangaBookmarkObject> lParseResult =
                        AnimeMangaBookmarkObject.ParseNode(lBookmarkNode, this._senpai);

                    if (lParseResult.Success) lAnimeMangaBookmarkObjects.Add(lParseResult.Result);
                    else lFailedParses++;
                }

                if (lParses == lFailedParses) return new ProxerResult {Success = false};
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResult.Result, false)).Exceptions);
            }

            this.Bookmarks.SetInitialisedObject(lAnimeMangaBookmarkObjects);
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

        [ItemNotNull]
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
                                AnimeMangaProgress.Finished, this._senpai);

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
                                AnimeMangaProgress.InProgress, this._senpai);

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
                                AnimeMangaProgress.Planned, this._senpai);

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
                                AnimeMangaProgress.Aborted, this._senpai);

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