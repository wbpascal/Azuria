using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class Utility
    {
        private static List<Func<string, bool>> checkHtmlActions = new List<Func<string, bool>> { checkForFirewall, checkForLogin, checkForEligibility, checkFor404 };
        private static List<Func<string, bool>> checkJsonActions = new List<Func<string, bool>> { checkForFirewall, checkForLogin, checkForEligibility };

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
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html">Die HTML-Seite als string</param>
        /// <returns></returns>
        public static bool checkForCorrectHTML(string html)
        {
            foreach (Func<string, bool> curCheck in checkHtmlActions)
            {
                if (!curCheck(html)) return false;
            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json">Die JSON-Datei als string</param>
        /// <returns></returns>
        public static bool checkForCorrectJson(string json)
        {
            foreach (Func<string, bool> curCheck in checkJsonActions)
            {
                if (!curCheck(json)) return false;
            }

            return true;
        }
        #region checkCorrectHtml/Json Funktionen
        private static bool checkForFirewall(string html)
        {
            return !html.ToLower().Contains("please turn javascript on and reload the page");
        }
        private static bool checkForEligibility(string html)
        {
            return !html.ToLower().Contains("du hast keine berechtigung") || !html.ToLower().Contains("dieses profil wurde noch nicht aktiviert");
        }
        private static bool checkForLogin(string html)
        {
            return !html.ToLower().Contains("du bist nicht eingeloggt") || !html.ToLower().Contains("bitte logge dich ein");
        }
        private static bool checkFor404(string html)
        {
            return !html.ToLower().Contains("diese seite wurde nicht gefunden");
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="parseErrors"></param>
        /// <returns></returns>
        public static string tryFixParseErrors(string html, IEnumerable<HtmlAgilityPack.HtmlParseError> parseErrors)
        {
            if (parseErrors.Count() > 0)
            {
                foreach(HtmlAgilityPack.HtmlParseError curError in parseErrors)
                {
                    html = html.Remove(curError.StreamPosition, curError.SourceText.Length);
                }
            }

            return html;
        }
    }
}
