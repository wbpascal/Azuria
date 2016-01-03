﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Main.Minor;
using HtmlAgilityPack;

namespace Azuria.Utilities
{
    internal static class Utility
    {
        #region

        internal static bool CheckForCorrectResponse(string response, ErrorHandler errHandler)
        {
            //return errHandler.WrongHtml.All(curErrorResponse => ILd(response, curErrorResponse) > 15);
            return true;
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

        internal static ProxerResult<Comment> GetCommentFromNode(HtmlNode commentNode, Senpai senpai)
        {
            commentNode = commentNode.FirstChild;

            try
            {
                User lAuthor = new User(commentNode.FirstChild.InnerText, Convert.ToInt32(
                    GetTagContents(commentNode.FirstChild.ChildNodes[1].Attributes["href"].Value, "/user/", "#top")
                        .First()), senpai);

                commentNode.ChildNodes[1].ChildNodes.RemoveAt(0);
                string lContent = commentNode.ChildNodes[1].InnerHtml;

                int lStars =
                    commentNode.ChildNodes[2].FirstChild.ChildNodes.Count(
                        starNode =>
                            starNode.Name.Equals("img") && starNode.Attributes.Contains("src") &&
                            starNode.Attributes["src"].Value.Equals("/images/misc/stern.png"));

                return new ProxerResult<Comment>(new Comment(lAuthor, lStars, lContent));
            }
            catch
            {
                return new ProxerResult<Comment>(new[] {new WrongResponseException()});
            }
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
                throw new Exception("Maximum string length in Levenshtein.iLD is " + Math.Pow(2, 31) + ".\nYours is " +
                                    Math.Max(sRow.Length, sCol.Length) + ".");

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
            return 100*v0[rowLen]/max;
        }

        internal static IEnumerable<HtmlNode> SelectNodesUtility(this HtmlNode node, string attribute, string value)
        {
            return
                GetAllHtmlNodes(node.ChildNodes)
                    .Where(x => x.Attributes.Contains(attribute) && x.Attributes[attribute].Value == value);
        }

        internal static DateTime ToDateTime(string strFdate, string format = "dd.MM.yyyy")
        {
            return DateTime.ParseExact(
                strFdate,
                format,
                CultureInfo.InvariantCulture);
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

        internal static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        #endregion
    }
}