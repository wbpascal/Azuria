using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Proxer.API.Exceptions;
using Proxer.API.Main;
using Proxer.API.Utilities.Net;
using RestSharp;

namespace Proxer.API.Utilities
{
    /// <summary>
    ///     In dieser Klasse sind alle Funktionen zusammengefasst,
    ///     die Hilfestellungen anbieten
    /// </summary>
    public class Utility
    {
        #region

        /// <summary>
        ///     Gibt ein Objekt zurück, dass einen Anime oder Manga
        ///     der spezifizierten ID repräsentiert.
        /// </summary>
        /// <param name="id">Die ID des <see cref="Main.Anime">Anime</see> oder <see cref="Main.Manga">Manga</see>.</param>
        /// <param name="senpai">Der Benutzer. (Muss eingeloggt sein)</param>
        /// <returns>Anime oder Manga der ID (Typecast erforderlich)</returns>
        public static async Task<ProxerResult<IAnimeMangaObject>> GetAnimeManga(int id, Senpai senpai)
        {
            HtmlDocument lDocument = new HtmlDocument();
            string lResponse;

            IRestResponse lResponseObject =
                await HttpUtility.GetWebRequestResponse("https://proxer.me/info/" + id, senpai.LoginCookies);
            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                lResponse = System.Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
            else return new ProxerResult<IAnimeMangaObject>(new[] { new WrongResponseException(), lResponseObject.ErrorException });

            if (string.IsNullOrEmpty(lResponse) || !CheckForCorrectResponse(lResponse, senpai.ErrHandler))
                return new ProxerResult<IAnimeMangaObject>(new Exception[] { new WrongResponseException() });

            if (!CheckForCorrectResponse(lResponse, senpai.ErrHandler)) return null;
            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lNode =
                    lDocument.DocumentNode.ChildNodes[1].ChildNodes[2].ChildNodes[2].ChildNodes[2].ChildNodes[1]
                        .ChildNodes[1];
                if (lNode.InnerText.Equals("Episoden"))
                {
                    return
                        new ProxerResult<IAnimeMangaObject>(new Anime(
                            lDocument.DocumentNode.ChildNodes[1].ChildNodes[2].ChildNodes[2].ChildNodes[2]
                                .ChildNodes[5].ChildNodes[2].FirstChild.ChildNodes[1].FirstChild.ChildNodes[1]
                                .ChildNodes[1].InnerText, id, senpai));
                }

                if (lNode.InnerText.Equals("Kapitel"))
                {
                    return
                        new ProxerResult<IAnimeMangaObject>(new Manga(
                            lDocument.DocumentNode.ChildNodes[1].ChildNodes[2].ChildNodes[2].ChildNodes[2]
                                .ChildNodes[5].ChildNodes[2].FirstChild.ChildNodes[1].FirstChild.ChildNodes[1]
                                .ChildNodes[1].InnerText, id, senpai));
                }
            }
            catch
            {
                return new ProxerResult<IAnimeMangaObject>((await ErrorHandler.HandleError(senpai, lResponse, false)).Exceptions);
            }

            return new ProxerResult<IAnimeMangaObject>(new Exception[] {new WrongResponseException()});
        }


        internal static IEnumerable<HtmlNode> GetAllHtmlNodes(HtmlNodeCollection htmlNodeCollection)
        {
            List<HtmlNode> lHtmlNodes = new List<HtmlNode>();
            foreach (HtmlNode htmlNode in htmlNodeCollection)
            {
                lHtmlNodes.Add(htmlNode);
                if (htmlNode.HasChildNodes)
                    lHtmlNodes = lHtmlNodes.Concat(GetAllHtmlNodes(htmlNode.ChildNodes)).ToList();
            }
            return lHtmlNodes;
        }

        internal static List<string> GetTagContents(string source, string startTag, string endTag)
        {
            List<string> stringsFound = new List<string>();
            int index = source.IndexOf(startTag, StringComparison.Ordinal) + startTag.Length;

            try
            {
                while (index != startTag.Length - 1)
                {
                    stringsFound.Add(source.Substring(index,
                        source.IndexOf(endTag, index, StringComparison.Ordinal) - index));
                    index = source.IndexOf(startTag, index, StringComparison.Ordinal) + startTag.Length;
                }
            }
            catch
            {
                // ignored
            }
            return stringsFound;
        }

        internal static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        internal static bool CheckForCorrectResponse(string response, ErrorHandler errHandler)
        {
            //return errHandler.WrongHtml.All(curErrorResponse => ILd(response, curErrorResponse) > 15);
            return true;
        }

        internal static string TryFixParseErrors(string html, IEnumerable<HtmlParseError> parseErrors)
        {
            IEnumerable<HtmlParseError> htmlParseErrors = parseErrors as HtmlParseError[] ?? parseErrors.ToArray();
            if (htmlParseErrors.Any())
            {
                html = htmlParseErrors.Aggregate(html,
                    (current, curError) => current.Remove(curError.StreamPosition, curError.SourceText.Length));
            }

            return html;
        }

        /// <summary>
        ///     Compute Levenshtein distance
        ///     Memory efficient version
        ///     By: Sten Hjelmqvist
        /// </summary>
        /// <param name="sRow"></param>
        /// <param name="sCol"></param>
        /// <returns>0==perfect match | 100==totaly different</returns>
        internal static int ILd(string sRow, string sCol)
        {
            int rowLen = sRow.Length;
            int colLen = sCol.Length;
            int rowIdx;
            int colIdx;

            if (Math.Max(sRow.Length, sCol.Length) > Math.Pow(2, 31))
                throw (new Exception("Maximum string length in Levenshtein.iLD is " + Math.Pow(2, 31) + ".\nYours is " +
                                     Math.Max(sRow.Length, sCol.Length) + "."));

            if (rowLen == 0)
            {
                return colLen;
            }
            if (colLen == 0)
            {
                return rowLen;
            }

            int[] v0 = new int[rowLen + 1];
            int[] v1 = new int[rowLen + 1];

            for (rowIdx = 1; rowIdx <= rowLen; rowIdx++)
            {
                v0[rowIdx] = rowIdx;
            }

            for (colIdx = 1; colIdx <= colLen; colIdx++)
            {
                v1[0] = colIdx;
                char colJ = sCol[colIdx - 1];
                for (rowIdx = 1; rowIdx <= rowLen; rowIdx++)
                {
                    char rowI = sRow[rowIdx - 1];
                    int cost = rowI == colJ ? 0 : 1;

                    int mMin = v0[rowIdx] + 1;
                    int b = v1[rowIdx - 1] + 1;
                    int c = v0[rowIdx - 1] + cost;

                    if (b < mMin)
                    {
                        mMin = b;
                    }
                    if (c < mMin)
                    {
                        mMin = c;
                    }
                    v1[rowIdx] = mMin;
                }
                int[] vTmp = v0;
                v0 = v1;
                v1 = vTmp;
            }

            // Value between 0 - 100
            // 0==perfect match 100==totaly different
            int max = Math.Max(rowLen, colLen);
            return ((100 * v0[rowLen]) / max);
        }

        internal static DateTime ToDateTime(string strFdate, string format = "dd.MM.yyyy")
        {
            return DateTime.ParseExact(
                strFdate,
                format,
                CultureInfo.InvariantCulture);
        }

        #endregion
    }
}