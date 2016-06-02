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
    ///     Represents an editable comment for an <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see>.
    /// </summary>
    public class EditableComment<T> : Comment<T> where T : IAnimeMangaObject
    {
        private readonly int _entryId;
        [NotNull] private readonly Senpai _senpai;

        internal EditableComment(int entryId, Comment<T> baseObject, AnimeMangaProgress progress, Senpai senpai)
            : base(
                baseObject.Author, baseObject.AnimeMangaObject, baseObject.Rating, baseObject.Content,
                baseObject.SubRatings,
                baseObject.ProgressState)
        {
            this.Progress = new EditableAnimeMangaProgress(progress.CurrentProgress, progress.MaxProgress,
                this.IncrementCurrentProgress);
            this._entryId = entryId;
            this._senpai = senpai;
        }

        #region Properties

        /// <summary>
        ///     Gets or sets the content of this comment.
        ///     If the content was set the method <see cref="Save()" /> must be called to send the information to the server.
        /// </summary>
        [NotNull]
        public new string Content
        {
            get { return base.Content; }
            set { base.Content = value; }
        }

        /// <summary>
        ///     Gets a object with the help of which the progress can be set or retrieved.
        ///     Calling <see cref="Save()" /> is not necessary after changing the progress.
        /// </summary>
        [NotNull]
        public EditableAnimeMangaProgress Progress { get; }

        /// <summary>
        ///     Gets or sets the category the user has categorised his progress in.
        ///     If the content was set the method <see cref="Save()" /> must be called to send the information to the server.
        /// </summary>
        public new AnimeMangaProgressState ProgressState
        {
            get { return base.ProgressState; }
            set { base.ProgressState = value; }
        }

        /// <summary>
        ///     Gets or sets the overall rating the author gave. Returns -1 if no rating was found.
        ///     If the content was set the method <see cref="Save()" /> must be called to send the information to the server.
        /// </summary>
        public new int Rating
        {
            get { return base.Rating; }
            set { base.Rating = value; }
        }

        /// <summary>
        ///     Gets or sets the rating of all subcategories.
        ///     If the content was set the method <see cref="Save()" /> must be called to send the information to the server.
        /// </summary>
        [NotNull]
        public new Dictionary<RatingCategory, int> SubRatings
        {
            get { return base.SubRatings; }
            set { base.SubRatings = value; }
        }

        #endregion

        #region

        /// <summary>
        ///     Deletes the whole entry from the server.
        /// </summary>
        /// <returns>If the action was successfull.</returns>
        public async Task<ProxerResult> Delete()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri($"https://proxer.me/comment?format=json&json=delete&id={this._entryId}"),
                        this._senpai.LoginCookies,
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

        internal static async Task<ProxerResult<EditableComment<T>>> GetEditableComment(int entryId,
            T animeMangaObject, Senpai senpai)
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
                        senpai,
                        new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult<EditableComment<T>>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lInnerDiv = lDocument.GetElementbyId("inner");
                if (lInnerDiv == null)
                    return new ProxerResult<EditableComment<T>>(new[] {new WrongResponseException()});

                if (lInnerDiv.ChildNodes.FindFirst("table").ChildNodes.Count == 1)
                    return new ProxerResult<EditableComment<T>>(new[] {new NoAccessException()});

                return new ProxerResult<EditableComment<T>>(ParseEditabelComment(
                    lInnerDiv.ChildNodes.FindFirst("table").ChildNodes.First(node => node.ChildNodes.Count == 3),
                    entryId, animeMangaObject, senpai));
            }
            catch
            {
                return
                    new ProxerResult<EditableComment<T>>(
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

        private async Task<ProxerResult> IncrementCurrentProgress(int newProgress)
        {
            Dictionary<string, string> lPostArgs = this.GetPostArgs();
            lPostArgs["episode"] = newProgress.ToString();
            return await this.Save(lPostArgs);
        }

        private static EditableComment<T> ParseEditabelComment(HtmlNode node, int entryId, T animeMangaObject,
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

            return new EditableComment<T>(entryId,
                new Comment<T>(senpai.Me ?? Azuria.User.System, animeMangaObject, lRating, lContent, lSubRatings,
                    lProgressState), lProgress, senpai);
        }

        /// <summary>
        ///     Saves the current entry to the server.
        /// </summary>
        /// <returns>If the action was successfull.</returns>
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

        #endregion
    }
}