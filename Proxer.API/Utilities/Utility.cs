using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="startTag"></param>
        /// <param name="endTag"></param>
        /// <returns></returns>
        public static List<string> GetTagContents(string Source, string startTag, string endTag)
        {
            List<string> StringsFound = new List<string>();
            int Index = Source.IndexOf(startTag) + startTag.Length;

            try
            {
                while (Index != startTag.Length - 1)
                {
                    StringsFound.Add(Source.Substring(Index, Source.IndexOf(endTag, Index) - Index));
                    Index = Source.IndexOf(startTag, Index) + startTag.Length;
                }
            }
            catch (Exception)
            {

            }
            return StringsFound;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="errHandler"></param>
        /// <returns></returns>
        public static bool checkForCorrectResponse(string response, ErrorHandler errHandler)
        {
            foreach (string curErrorResponse in errHandler.WrongHtml)
            {
                if (iLD(response, curErrorResponse) <= 15) return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="parseErrors"></param>
        /// <returns></returns>
        public static string tryFixParseErrors(string html, IEnumerable<HtmlParseError> parseErrors)
        {
            if (parseErrors.Count() > 0)
            {
                foreach (HtmlAgilityPack.HtmlParseError curError in parseErrors)
                {
                    html = html.Remove(curError.StreamPosition, curError.SourceText.Length);
                }
            }

            return html;
        }


        /// <summary>
        /// Compute Levenshtein distance 
        /// Memory efficient version
        /// By: Sten Hjelmqvist
        /// </summary>
        /// <param name="sRow"></param>
        /// <param name="sCol"></param>
        /// <returns>0==perfect match | 100==totaly different</returns>
        public static int iLD(String sRow, String sCol)
        {
            int RowLen = sRow.Length;
            int ColLen = sCol.Length;
            int RowIdx;
            int ColIdx;
            char Row_i;
            char Col_j;
            int cost;

            if (Math.Max(sRow.Length, sCol.Length) > Math.Pow(2, 31))
                throw (new Exception("\nMaximum string length in Levenshtein.iLD is " + Math.Pow(2, 31) + ".\nYours is " + Math.Max(sRow.Length, sCol.Length) + "."));

            if (RowLen == 0)
            {
                return ColLen;
            }
            if (ColLen == 0)
            {
                return RowLen;
            }

            int[] v0 = new int[RowLen + 1];
            int[] v1 = new int[RowLen + 1];
            int[] vTmp;

            for (RowIdx = 1; RowIdx <= RowLen; RowIdx++)
            {
                v0[RowIdx] = RowIdx;
            }

            for (ColIdx = 1; ColIdx <= ColLen; ColIdx++)
            {
                v1[0] = ColIdx;
                Col_j = sCol[ColIdx - 1];
                for (RowIdx = 1; RowIdx <= RowLen; RowIdx++)
                {
                    Row_i = sRow[RowIdx - 1];
                    if (Row_i == Col_j)
                    {
                        cost = 0;
                    }
                    else
                    {
                        cost = 1;
                    }

                    int m_min = v0[RowIdx] + 1;
                    int b = v1[RowIdx - 1] + 1;
                    int c = v0[RowIdx - 1] + cost;

                    if (b < m_min)
                    {
                        m_min = b;
                    }
                    if (c < m_min)
                    {
                        m_min = c;
                    }
                    v1[RowIdx] = m_min;
                }
                vTmp = v0;
                v0 = v1;
                v1 = vTmp;
            }

            // Value between 0 - 100
            // 0==perfect match 100==totaly different
            int max = System.Math.Max(RowLen, ColLen);
            return ((100 * v0[RowLen]) / max);
        }
    }
}
