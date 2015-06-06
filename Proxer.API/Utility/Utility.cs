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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return StringsFound;
        }
    }
}
