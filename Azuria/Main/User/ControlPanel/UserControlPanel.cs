using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
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
        }

        #region

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<AnimeMangaBookmarkObject>>> GetBookmarks()
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
                        "https://proxer.me/ucp?s=reminder&device=mobile&format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai,
                        new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult<IEnumerable<AnimeMangaBookmarkObject>>(lResult.Exceptions);

            try
            {
                foreach (
                    HtmlNode lBookmarkNode in
                        lDocument.DocumentNode.Descendants()
                            .First(
                                node =>
                                    node.Name.Equals("ul") &&
                                    node.GetAttributeValue("data-split-icon", "").Equals("delete")).ChildNodes)
                {
                    ProxerResult<AnimeMangaBookmarkObject> lParseResult =
                        AnimeMangaBookmarkObject.ParseNode(lBookmarkNode, this._senpai);

                    if (lParseResult.Success && lParseResult.Result != null)
                        lAnimeMangaBookmarkObjects.Add(lParseResult.Result);
                }
            }
            catch
            {
                return
                    new ProxerResult<IEnumerable<AnimeMangaBookmarkObject>>(
                        (await ErrorHandler.HandleError(this._senpai, lResult.Result, false)).Exceptions);
            }
            return new ProxerResult<IEnumerable<AnimeMangaBookmarkObject>>(lAnimeMangaBookmarkObjects);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<AnimeMangaChronicObject>>> GetChronic()
        {
            HtmlDocument lDocument = new HtmlDocument();

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
                    HttpUtility.GetResponseErrorHandling("https://proxer.me/ucp?s=history&format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult<IEnumerable<AnimeMangaChronicObject>>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lTableNode = lDocument.GetElementbyId("box-table-a");
                lTableNode.ChildNodes.RemoveAt(0);
                List<AnimeMangaChronicObject> lChronicObjects = new List<AnimeMangaChronicObject>();
                foreach (HtmlNode chronicNode in lTableNode.ChildNodes)
                {
                    ProxerResult<AnimeMangaChronicObject> lParseResult =
                        AnimeMangaChronicObject.GetChronicObjectFromNode(chronicNode, this._senpai, true);
                    if (lParseResult.Success) lChronicObjects.Add(lParseResult.Result);
                }

                return new ProxerResult<IEnumerable<AnimeMangaChronicObject>>(lChronicObjects);
            }
            catch
            {
                return
                    new ProxerResult<IEnumerable<AnimeMangaChronicObject>>(
                        (await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<AnimeMangaFavouriteObject>>> GetFavourites()
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
                        "https://proxer.me/ucp?s=topten&device=mobile&format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai,
                        new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult<IEnumerable<AnimeMangaFavouriteObject>>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
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
                    HtmlNode lInfoNode =
                        lListNode.ChildNodes.First(
                            node => node.Name == "a" && node.GetAttributeValue("href", "").StartsWith("/info"));

                    if (lInfoNode == null) continue;

                    int lAnimeId =
                        Convert.ToInt32(lInfoNode.GetAttributeValue("href", "/info/-1").Substring("/info/".Length));
                    string lAnimeName = lInfoNode.ChildNodes.FindFirst("h2")?.InnerText ?? "";
                    lAnimeMangaFavouriteObjects.Add(
                        new AnimeMangaFavouriteObject(
                            Convert.ToInt32(lListNode.GetAttributeValue("id", "entry-1").Substring("entry".Length)),
                            new Anime(lAnimeName, lAnimeId, this._senpai), this._senpai));
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
                    HtmlNode lInfoNode =
                        lListNode.ChildNodes.First(
                            node => node.Name == "a" && node.GetAttributeValue("href", "").StartsWith("/info"));

                    if (lInfoNode == null) continue;

                    int lMangaId =
                        Convert.ToInt32(lInfoNode.GetAttributeValue("href", "/info/-1").Substring("/info/".Length));
                    string lMangaName = lInfoNode.ChildNodes.FindFirst("h2")?.InnerText ?? "";
                    lAnimeMangaFavouriteObjects.Add(
                        new AnimeMangaFavouriteObject(
                            Convert.ToInt32(lListNode.GetAttributeValue("id", "entry-1").Substring("entry".Length)),
                            new Manga(lMangaName, lMangaId, this._senpai), this._senpai));
                }
            }
            catch
            {
                return
                    new ProxerResult<IEnumerable<AnimeMangaFavouriteObject>>(
                        (await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
            return new ProxerResult<IEnumerable<AnimeMangaFavouriteObject>>(lAnimeMangaFavouriteObjects);
        }

        #endregion
    }
}