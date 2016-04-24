using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Main.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.Main.User.Comment
{
    /// <summary>
    /// </summary>
    public class EditableComment : Comment
    {
        private readonly int _entryId;
        [NotNull] private readonly Senpai _senpai;

        internal EditableComment(int entryId, Comment baseObject, AnimeMangaProgress progress, Senpai senpai)
            : base(
                baseObject.Author, baseObject.AnimeMangaId, baseObject.Rating, baseObject.Content, baseObject.SubRatings,
                baseObject.ProgressState)
        {
            this.Progress = new EditableAnimeMangaProgress(progress.CurrentProgress, progress.MaxProgress,
                this.IncrementCurrentProgress);
            this._entryId = entryId;
            this._senpai = senpai;
        }

        #region Properties

        /// <summary>
        ///     Gibt den Inhalt des <see cref="Comment">Kommentars</see> zurück.
        /// </summary>
        [NotNull]
        public new string Content
        {
            get { return base.Content; }
            set { base.Content = value; }
        }

        /// <summary>
        /// </summary>
        [NotNull]
        public EditableAnimeMangaProgress Progress { get; }

        /// <summary>
        ///     Gibt die Sterne der Gesamtwertung des <see cref="Comment">Kommentars</see> zurück. Es wird -1 zurückgegeben, wenn
        ///     keine Bewertung abgegeben wurde.
        /// </summary>
        public new int Rating
        {
            get { return base.Rating; }
            set { base.Rating = value; }
        }

        /// <summary>
        ///     Gibt die Sterne weiterer Bewertungskategorien des <see cref="Comment">Kommentars</see> zurück.
        /// </summary>
        [NotNull]
        public new Dictionary<RatingCategory, int> SubRatings
        {
            get { return base.SubRatings; }
            set { base.SubRatings = value; }
        }

        #endregion

        #region

        private async Task<ProxerResult> IncrementCurrentProgress(int newProgress)
        {
            Dictionary<string, string> lPostArgs = this.GetPostArgs();
            lPostArgs["episode"] = newProgress.ToString();
            return await this.Save(lPostArgs);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<ProxerResult> Delete()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri($"https://proxer.me/comment?format=json&json=delete&id={this._entryId}"),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                Dictionary<string, string> lDeserialisedResponse =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (lDeserialisedResponse.ContainsKey("msg") &&
                    lDeserialisedResponse["msg"].StartsWith("Du bist nicht eingeloggt"))
                    return new ProxerResult(new[] {new NotLoggedInException()});

                return new ProxerResult
                {
                    Success = lDeserialisedResponse.ContainsKey("error") && lDeserialisedResponse["error"].Equals("0")
                };
            }
            catch
            {
                return
                    new ProxerResult(
                        (await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        internal static async Task<ProxerResult<EditableComment>> GetEditableComment(int entryId,
            int animeMangaObject, Senpai senpai)
        {
            HtmlDocument lDocument = new HtmlDocument();
            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\"><h3>Bitte logge dich ein.</h3></div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException()});

                return new ProxerResult();
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri($"https://proxer.me/comment?id={entryId}&s=edit&format=raw"),
                        senpai.LoginCookies,
                        senpai.ErrHandler,
                        senpai,
                        new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult<EditableComment>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lInnerDiv = lDocument.GetElementbyId("inner");
                if (lInnerDiv == null) return new ProxerResult<EditableComment>(new[] {new WrongResponseException()});

                if (lInnerDiv.ChildNodes.FindFirst("table").ChildNodes.Count == 1)
                    return new ProxerResult<EditableComment>(new[] {new NoAccessException()});

                return new ProxerResult<EditableComment>(ParseEditabelComment(
                    lInnerDiv.ChildNodes.FindFirst("table").ChildNodes.First(node => node.ChildNodes.Count == 3),
                    entryId, animeMangaObject, senpai));
            }
            catch
            {
                return
                    new ProxerResult<EditableComment>(
                        (await ErrorHandler.HandleError(senpai, lResponse, false)).Exceptions);
            }
        }

        private Dictionary<string, string> GetPostArgs()
        {
            Dictionary<string, string> lReturn = new Dictionary<string, string>
            {
                {"comment", this.Content},
                {"episode", this.Progress.CurrentProgress.ToString()},
                {"rating", this.Rating.ToString()},
                {"state", ((int) this.ProgressState).ToString()}
            };
            foreach (KeyValuePair<RatingCategory, int> subRating in this.SubRatings)
            {
                lReturn.Add($"misc[{subRating.Key.ToString().ToLower()}]", subRating.Value.ToString());
            }

            return lReturn;
        }

        private static EditableComment ParseEditabelComment(HtmlNode node, int entryId, int animeMangaId,
            Senpai senpai)
        {
            int lRating =
                Convert.ToInt32(
                    node.FirstChild.ChildNodes.First(
                        htmlNode =>
                            htmlNode.GetAttributeValue("name", "error").Equals("rating") &&
                            htmlNode.Attributes.Contains("checked")).GetAttributeValue("value", "-1"));

            AnimeMangaProgressState lProgressState =
                (AnimeMangaProgressState)
                    Convert.ToInt32(
                        node.FirstChild.ChildNodes.First(
                            htmlNode =>
                                htmlNode.GetAttributeValue("name", "error").Equals("type") &&
                                htmlNode.Attributes.Contains("checked")).GetAttributeValue("value", "-1"));

            string lContent = node.ChildNodes[1].ChildNodes.FindFirst("textarea").InnerText;

            HtmlNode lEpisodeSelectNode =
                node.ChildNodes[2].ChildNodes.First(
                    htmlNode =>
                        htmlNode.Name.Equals("select") && htmlNode.GetAttributeValue("name", "error").Equals("state"));
            AnimeMangaProgress lProgress =
                new AnimeMangaProgress(
                    Convert.ToInt32(
                        lEpisodeSelectNode.ChildNodes.FirstOrDefault(
                            htmlNode => htmlNode.Attributes.Contains("selected"))?
                            .GetAttributeValue("value", "-1") ?? "-1"),
                    lEpisodeSelectNode.ChildNodes.Count(htmlNode => htmlNode.Name.Equals("option")) - 1);

            Dictionary<RatingCategory, int> lSubRatings = new Dictionary<RatingCategory, int>();

            HtmlNode lGenreSelectNode =
                node.ChildNodes[2].ChildNodes.First(
                    htmlNode =>
                        htmlNode.Name.Equals("select") && htmlNode.GetAttributeValue("name", "error").Equals("genre"));
            if (lGenreSelectNode.ChildNodes.Any(htmlNode => htmlNode.Attributes.Contains("selected")))
                lSubRatings.Add(RatingCategory.Genre,
                    Convert.ToInt32(
                        lGenreSelectNode.ChildNodes.First(htmlNode => htmlNode.Attributes.Contains("selected"))
                            .GetAttributeValue("value", "-1")));

            HtmlNode lStorySelectNode =
                node.ChildNodes[2].ChildNodes.First(
                    htmlNode =>
                        htmlNode.Name.Equals("select") && htmlNode.GetAttributeValue("name", "error").Equals("story"));
            if (lStorySelectNode.ChildNodes.Any(htmlNode => htmlNode.Attributes.Contains("selected")))
                lSubRatings.Add(RatingCategory.Story,
                    Convert.ToInt32(
                        lStorySelectNode.ChildNodes.First(htmlNode => htmlNode.Attributes.Contains("selected"))
                            .GetAttributeValue("value", "-1")));

            HtmlNode lAnimationSelectNode =
                node.ChildNodes[2].ChildNodes.First(
                    htmlNode =>
                        htmlNode.Name.Equals("select") &&
                        htmlNode.GetAttributeValue("name", "error").Equals("animation"));
            if (lAnimationSelectNode.ChildNodes.Any(htmlNode => htmlNode.Attributes.Contains("selected")))
                lSubRatings.Add(RatingCategory.Animation,
                    Convert.ToInt32(
                        lAnimationSelectNode.ChildNodes.First(htmlNode => htmlNode.Attributes.Contains("selected"))
                            .GetAttributeValue("value", "-1")));

            HtmlNode lCharactersSelectNode =
                node.ChildNodes[2].ChildNodes.First(
                    htmlNode =>
                        htmlNode.Name.Equals("select") &&
                        htmlNode.GetAttributeValue("name", "error").Equals("characters"));
            if (lCharactersSelectNode.ChildNodes.Any(htmlNode => htmlNode.Attributes.Contains("selected")))
                lSubRatings.Add(RatingCategory.Characters,
                    Convert.ToInt32(
                        lCharactersSelectNode.ChildNodes.First(htmlNode => htmlNode.Attributes.Contains("selected"))
                            .GetAttributeValue("value", "-1")));

            HtmlNode lMusicSelectNode =
                node.ChildNodes[2].ChildNodes.First(
                    htmlNode =>
                        htmlNode.Name.Equals("select") && htmlNode.GetAttributeValue("name", "error").Equals("music"));
            if (lMusicSelectNode.ChildNodes.Any(htmlNode => htmlNode.Attributes.Contains("selected")))
                lSubRatings.Add(RatingCategory.Music,
                    Convert.ToInt32(
                        lMusicSelectNode.ChildNodes.First(htmlNode => htmlNode.Attributes.Contains("selected"))
                            .GetAttributeValue("value", "-1")));

            return new EditableComment(entryId,
                new Comment(senpai.Me ?? Azuria.User.System, animeMangaId, lRating, lContent, lSubRatings,
                    lProgressState), lProgress, senpai);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<ProxerResult> Save()
        {
            return await this.Save(this.GetPostArgs());
        }

        private async Task<ProxerResult> Save(Dictionary<string, string> postArgs)
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(
                        new Uri($"https://proxer.me/comment?format=json&json=edit&id={this._entryId}"),
                        postArgs,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                Dictionary<string, string> lDeserialisedResponse =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (lDeserialisedResponse.ContainsKey("msg") &&
                    lDeserialisedResponse["msg"].StartsWith("Du bist nicht eingeloggt"))
                    return new ProxerResult(new[] { new NotLoggedInException() });

                return new ProxerResult
                {
                    Success = lDeserialisedResponse.ContainsKey("error") && lDeserialisedResponse["error"].Equals("0")
                };
            }
            catch
            {
                return
                    new ProxerResult(
                        (await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        #endregion
    }
}