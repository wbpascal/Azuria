using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.User.Comment
{
    /// <summary>
    ///     Represents a comment for an <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see>.
    /// </summary>
    public class Comment<T> where T : IAnimeMangaObject
    {
        internal Comment([NotNull] User author, T animeMangaObject, int rating, [NotNull] string comment,
            AnimeMangaProgressState progressState)
        {
            this.AnimeMangaObject = animeMangaObject;
            this.Author = author;
            this.Rating = rating;
            this.Content = comment;
            this.ProgressState = progressState;
            this.SubRatings = new Dictionary<RatingCategory, int>();
        }

        internal Comment([NotNull] User author, T animeMangaObject, int rating, [NotNull] string comment,
            Dictionary<RatingCategory, int> subRatings, AnimeMangaProgressState progressState)
            : this(author, animeMangaObject, rating, comment, progressState)
        {
            this.SubRatings = subRatings;
        }

        #region Properties

        /// <summary>
        ///     Gets the <see cref="Anime">Anime</see> or <see cref="Manga">Manga</see> this comment is for.
        /// </summary>
        public T AnimeMangaObject { get; }

        /// <summary>
        ///     Gets the author of this comment.
        /// </summary>
        [NotNull]
        public User Author { get; }

        /// <summary>
        ///     Gets the content of this comment. Spoilers are wrapped in tags.
        /// </summary>
        [NotNull]
        public string Content { get; protected set; }

        /// <summary>
        ///     Gets the category the author has put his progress of the <see cref="Anime" /> or <see cref="Manga" /> in.
        /// </summary>
        public AnimeMangaProgressState ProgressState { get; protected set; }

        /// <summary>
        ///     Gets the overall rating the <see cref="Author" /> gave. Returns -1 if no rating was found.
        /// </summary>
        public int Rating { get; protected set; }

        /// <summary>
        ///     Gets the rating of all subcategories.
        /// </summary>
        [NotNull]
        public Dictionary<RatingCategory, int> SubRatings { get; protected set; }

        #endregion

        #region

        [NotNull]
        private static string ContentToString(HtmlNodeCollection content)
        {
            string lString = string.Empty;

            foreach (HtmlNode htmlNode in content)
            {
                switch (htmlNode.Name)
                {
                    case "#text":
                        lString += htmlNode.InnerText;
                        break;
                    case "br":
                        lString += "\n";
                        break;
                    case "div":
                        if (htmlNode.GetAttributeValue("class", "error").Equals("spoiler"))
                            lString += "[SPOILER]" + ContentToString(htmlNode.ChildNodes[1].ChildNodes) + "[/SPOILER]";
                        else if (htmlNode.GetAttributeValue("style", "error").Equals("text-align:center;"))
                            lString += "[CENTER]" + ContentToString(htmlNode.ChildNodes) + "[/CENTER]";
                        else if (htmlNode.GetAttributeValue("style", "error").Equals("text-align:left;"))
                            lString += "[LEFT]" + ContentToString(htmlNode.ChildNodes) + "[/LEFT]";
                        else if (htmlNode.GetAttributeValue("style", "error").Equals("text-align:right;"))
                            lString += "[RIGHT]" + ContentToString(htmlNode.ChildNodes) + "[/RIGHT]";
                        else goto default;
                        break;
                    case "b":
                        lString += "[B]" + ContentToString(htmlNode.ChildNodes) + "[/B]";
                        break;
                    case "i":
                        lString += "[I]" + ContentToString(htmlNode.ChildNodes) + "[/I]";
                        break;
                    case "u":
                        lString += "[U]" + ContentToString(htmlNode.ChildNodes) + "[/U]";
                        break;
                    case "font":
                        lString += $"[SIZE={htmlNode.GetAttributeValue("size", "3")}]" +
                                   ContentToString(htmlNode.ChildNodes) + "[/SIZE]";
                        break;
                    default:
                        lString += htmlNode.OuterHtml;
                        break;
                }
            }

            return lString.Trim().Replace("<br>", "\n");
        }

        [ItemNotNull]
        internal static async Task<ProxerResult<IEnumerable<Comment<T>>>> GetCommentsFromUrl(int startIndex, int count,
            [NotNull] string url, [NotNull] string sort, [NotNull] Senpai senpai, T animeMangaObject,
            bool isUserPage = false,
            User author = null)
        {
            const int lKommentareProSeite = 25;

            List<Comment<T>> lReturn = new List<Comment<T>>();
            int lStartSeite = Convert.ToInt32(startIndex/lKommentareProSeite + 1);
            HtmlDocument lDocument = new HtmlDocument();
            Func<string, ProxerResult> lCheckFunc = s =>
            {
                lDocument = new HtmlDocument();
                lDocument.LoadHtml(s);

                HtmlNode lNode;

                if ((lNode = lDocument.DocumentNode.ChildNodes.FirstOrDefault(node =>
                    node.Name.Equals("p") && node.Attributes.Contains("align") &&
                    node.Attributes["align"].Value.Equals("center"))) != default(HtmlNode))
                {
                    if (lNode.InnerText.Equals("Es existieren bisher keine Kommentare."))
                        return new ProxerResult(new[] {new WrongResponseException {Response = s}});
                }

                if ((lNode = lDocument.DocumentNode.ChildNodes.FirstOrDefault(node =>
                    node.Name.Equals("div") && node.Attributes.Contains("class") &&
                    node.Attributes["class"].Value.Equals("inner"))) == default(HtmlNode)) return new ProxerResult();

                return lNode.InnerText.Equals("Dieser Benutzer hat bislang keine Kommentare.")
                    ? new ProxerResult(new[] {new WrongResponseException {Response = s}})
                    : new ProxerResult();
            };

            ProxerResult<string> lResult;

            while (count > 0 &&
                   (lResult =
                       await
                           HttpUtility.GetResponseErrorHandling(
                               new Uri(url + lStartSeite + "?format=raw&sort=" + sort),
                               senpai.LoginCookies, senpai, new[] {lCheckFunc}))
                       .Success)
            {
                try
                {
                    int i = 0;
                    foreach (HtmlNode commentNode in lDocument.DocumentNode.ChildNodes.Where(
                        node =>
                            node.Name.Equals("table") && node.Attributes.Contains("class") &&
                            // ReSharper disable once AccessToModifiedClosure
                            node.Attributes["class"].Value.Equals("details")).TakeWhile(node => count > 0))
                    {
                        if (i >= startIndex%lKommentareProSeite)
                        {
                            lReturn.Add(
                                (await ParseComment(commentNode, senpai, animeMangaObject, isUserPage, author))
                                    .OnError(new Comment<T>(User.System, animeMangaObject, -1, "ERROR",
                                        AnimeMangaProgressState.Unknown)));
                            count--;
                        }

                        i++;
                    }
                    lStartSeite++;
                }
                catch
                {
                    return
                        new ProxerResult<IEnumerable<Comment<T>>>(
                            (await ErrorHandler.HandleError(senpai, lResult.Result, false)).Exceptions);
                }
            }

            return new ProxerResult<IEnumerable<Comment<T>>>(lReturn);
        }

        [NotNull]
        internal static async Task<ProxerResult<Comment<T>>> ParseComment([NotNull] HtmlNode commentNode,
            [NotNull] Senpai senpai,
            T animeMangaObject
            , bool isUserPage, User author = null)
        {
            HtmlNode[] lTableNodes =
                commentNode.ChildNodes.FindFirst("tr").ChildNodes.Where(node => node.Name.Equals("td")).ToArray();

            try
            {
                User lAuthor = author ?? User.System;
                IAnimeMangaObject lAnimeMangaObject = animeMangaObject;

                if (isUserPage)
                {
                    int lAnimeMangaId = Convert.ToInt32(
                        commentNode.ChildNodes.FindFirst("a").Attributes["href"].Value.GetTagContents("/info/",
                            "#top").First());
                    string lAnimeMangaTitle =
                        commentNode.ChildNodes.FindFirst("a").InnerText.Trim();
                    lAnimeMangaObject = typeof(T) == typeof(Anime)
                        ? new Anime(lAnimeMangaTitle, lAnimeMangaId, senpai)
                        : typeof(T) == typeof(Manga)
                            ? new Manga(lAnimeMangaTitle, lAnimeMangaId, senpai)
                            : typeof(T) == typeof(IAnimeMangaObject)
                                ? (await ProxerClass.GetAnimeMangaById(lAnimeMangaId, senpai)).OnError(null)
                                : null;
                }
                else
                {
                    lAuthor = new User(lTableNodes[0].InnerText.Replace("/t", ""), Convert.ToInt32(
                        lTableNodes[0].ChildNodes.FindFirst("a").Attributes[
                            "href"].Value.GetTagContents("/user/", "#top")
                            .First()), senpai);
                }

                Dictionary<RatingCategory, int> lSubRatings = new Dictionary<RatingCategory, int>();
                if (lTableNodes[1].ChildNodes.Any(node => node.Name.Equals("table")))
                {
                    foreach (HtmlNode ratingNode in lTableNodes[1].ChildNodes.FindFirst("table").ChildNodes)
                    {
                        RatingCategory lCategory = RatingCategory.Unknown;
                        switch (ratingNode.FirstChild.InnerText)
                        {
                            case "Genre":
                                lCategory = RatingCategory.Genre;
                                break;
                            case "Story":
                                lCategory = RatingCategory.Story;
                                break;
                            case "Animation/Bilder":
                                lCategory = RatingCategory.Animation;
                                break;
                            case "Charaktere":
                                lCategory = RatingCategory.Characters;
                                break;
                            case "Musik":
                                lCategory = RatingCategory.Music;
                                break;
                        }
                        lSubRatings.Add(lCategory,
                            ratingNode.ChildNodes[1].ChildNodes.Count(
                                node =>
                                    node.Name.Equals("img") && node.Attributes.Contains("src") &&
                                    node.Attributes["src"].Value.Equals("/images/misc/stern.png")));
                    }
                    lTableNodes[1].ChildNodes.Remove(lTableNodes[1].ChildNodes.FindFirst("table"));
                }

                string lContent = ContentToString(lTableNodes[1].ChildNodes);

                int lStars = lTableNodes[2].ChildNodes.FindFirst("p").InnerText.Equals("Keine Bewertung")
                    ? -1
                    : lTableNodes[2].ChildNodes.FindFirst("p").ChildNodes.Count(
                        starNode =>
                            starNode.Name.Equals("img") && starNode.Attributes.Contains("src") &&
                            starNode.Attributes["src"].Value.Equals("/images/misc/stern.png"));

                AnimeMangaProgressState lProgressState = AnimeMangaProgressState.Unknown;
                string lProgressString = lTableNodes[2].ChildNodes.FindFirst("b").NextSibling.InnerText;
                if (lProgressString.StartsWith("Geschaut") || lProgressString.StartsWith("Gelesen"))
                    lProgressState = AnimeMangaProgressState.Finished;
                else if (lProgressString.StartsWith("Am Schauen") || lProgressString.StartsWith("Am Lesen"))
                    lProgressState = AnimeMangaProgressState.InProgress;
                else if (lProgressString.StartsWith("Wird noch geschaut") ||
                         lProgressString.StartsWith("Wird noch gelesen"))
                    lProgressState = AnimeMangaProgressState.Planned;
                else if (lProgressString.StartsWith("Abgebrochen"))
                    lProgressState = AnimeMangaProgressState.Aborted;

                return
                    new ProxerResult<Comment<T>>(new Comment<T>(lAuthor, (T) lAnimeMangaObject, lStars, lContent,
                        lSubRatings,
                        lProgressState));
            }
            catch
            {
                return new ProxerResult<Comment<T>>(new[] {new WrongResponseException()});
            }
        }

        #endregion
    }
}