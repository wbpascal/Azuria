using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Net;
using HtmlAgilityPack;

namespace Azuria.Main.Minor
{
    /// <summary>
    ///     Eine Klasse, die ein Kommentar eines <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> darstellt.
    /// </summary>
    public class Comment
    {
        internal Comment(Azuria.User author, int animeMangaId, int sterne, string kommentar)
        {
            this.AnimeMangaId = animeMangaId;
            this.Author = author;
            this.Stars = sterne;
            this.Content = kommentar;
        }

        #region Properties

        /// <summary>
        ///     Gibt die Id des <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> zurück, auf die der
        ///     <see cref="Comment">Kommentar</see> verweist.
        /// </summary>
        public int AnimeMangaId { get; private set; }

        /// <summary>
        ///     Gibt den Autor des <see cref="Comment">Kommentars</see> zurück.
        /// </summary>
        public Azuria.User Author { get; private set; }

        /// <summary>
        ///     Gibt die Sterne weiterer Bewertungskategorien des <see cref="Comment">Kommentars</see> zurück.
        /// </summary>
        public Dictionary<string, int> CategoryStars { get; private set; }

        /// <summary>
        ///     Gibt den Inhalt des <see cref="Comment">Kommentars</see> zurück.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        ///     Gibt die Sterne der Gesamtwertung des <see cref="Comment">Kommentars</see> zurück.
        /// </summary>
        public int Stars { get; private set; }

        #endregion

        #region

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
                        if (htmlNode.Attributes.Contains("class") &&
                            htmlNode.Attributes["class"].Value.Equals("spoiler"))
                            lString += "<spoiler>" + ContentToString(htmlNode.ChildNodes[1].ChildNodes) + "</spoiler>";
                        else goto default;

                        break;
                    default:
                        lString += htmlNode.OuterHtml;
                        break;
                }
            }

            return lString.Trim();
        }

        private static ProxerResult<Comment> GetCommentFromNode(HtmlNode commentNode, Senpai senpai
            , bool isUserPage, Azuria.User author = null, int animeMangaId = -1)
        {
            HtmlNode[] lTableNodes =
                commentNode.ChildNodes.FindFirst("tr").ChildNodes.Where(node => node.Name.Equals("td")).ToArray();

            try
            {
                Azuria.User lAuthor = author ?? Azuria.User.System;
                int lAnimeMangaId = animeMangaId;

                if (isUserPage)
                {
                    lAnimeMangaId =
                        Convert.ToInt32(
                            commentNode.ChildNodes.FindFirst("a").Attributes["href"].Value.GetTagContents("/info/",
                                "#top").First());
                }
                else
                {
                    lAuthor = new Azuria.User(lTableNodes[0].InnerText.Replace("/t", ""), Convert.ToInt32(
                        lTableNodes[0].ChildNodes.FindFirst("a").Attributes[
                            "href"].Value.GetTagContents("/user/", "#top")
                            .First()), senpai);
                }

                Dictionary<string, int> lSubRatings = new Dictionary<string, int>();
                if (lTableNodes[1].ChildNodes.Any(node => node.Name.Equals("table")))
                {
                    foreach (HtmlNode ratingNode in lTableNodes[1].ChildNodes.FindFirst("table").ChildNodes)
                    {
                        lSubRatings.Add(ratingNode.FirstChild.InnerText,
                            ratingNode.ChildNodes[1].ChildNodes.Count(
                                node =>
                                    node.Name.Equals("img") && node.Attributes.Contains("src") &&
                                    node.Attributes["src"].Value.Equals("/images/misc/stern.png")));
                    }
                    lTableNodes[1].ChildNodes.Remove(lTableNodes[1].ChildNodes.FindFirst("table"));
                }

                string lContent = ContentToString(lTableNodes[1].ChildNodes);

                int lStars =
                    lTableNodes[2].ChildNodes.FindFirst("p").ChildNodes.Count(
                        starNode =>
                            starNode.Name.Equals("img") && starNode.Attributes.Contains("src") &&
                            starNode.Attributes["src"].Value.Equals("/images/misc/stern.png"));

                return
                    new ProxerResult<Comment>(new Comment(lAuthor, lAnimeMangaId, lStars, lContent)
                    {
                        CategoryStars = lSubRatings
                    });
            }
            catch
            {
                return new ProxerResult<Comment>(new[] {new WrongResponseException()});
            }
        }

        internal static async Task<ProxerResult<IEnumerable<Comment>>> GetCommentsFromUrl(int startIndex, int count,
            string url, string sort, Senpai senpai, bool isUserPage = false, Azuria.User author = null,
            int animeMangaId = -1)
        {
            const int lKommentareProSeite = 25;

            List<Comment> lReturn = new List<Comment>();
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
                               url + lStartSeite + "?format=raw&sort=" + sort,
                               senpai.LoginCookies, senpai.ErrHandler, senpai, new[] {lCheckFunc}))
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
                                GetCommentFromNode(commentNode, senpai, isUserPage, author, animeMangaId)
                                    .OnError(new Comment(Azuria.User.System, -1, -1, "ERROR")));
                            count--;
                        }

                        i++;
                    }
                    lStartSeite++;
                }
                catch
                {
                    return
                        new ProxerResult<IEnumerable<Comment>>(
                            (await ErrorHandler.HandleError(senpai, lResult.Result, false)).Exceptions);
                }
            }

            return new ProxerResult<IEnumerable<Comment>>(lReturn);
        }

        #endregion
    }
}