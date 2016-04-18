using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Initialisation;
using HtmlAgilityPack;
using JetBrains.Annotations;

// ReSharper disable LoopCanBeConvertedToQuery

namespace Azuria.Utilities
{
    internal static class Utility
    {
        #region

        internal static bool CheckForCorrectResponse([NotNull] string response, [NotNull] ErrorHandler errHandler)
        {
            //return errHandler.WrongHtml.All(curErrorResponse => ILd(response, curErrorResponse) > 15);
            return true;
        }

        /// <summary>
        ///     Compute Levenshtein distance
        ///     Memory efficient version
        ///     By: Sten Hjelmqvist
        /// </summary>
        /// <param name="sRow"></param>
        /// <param name="sCol"></param>
        /// <returns>0==perfect match | 100==totaly different</returns>
        internal static int ComputeLevenshtein([NotNull] string sRow, [NotNull] string sCol)
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

        [NotNull]
        internal static List<string> GetTagContents([NotNull] this string source, [NotNull] string startTag,
            [NotNull] string endTag)
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

        internal static bool HasParameterlessConstructor([NotNull] this Type type)
        {
            foreach (ConstructorInfo ctor in type.GetTypeInfo().DeclaredConstructors)
            {
                if (!ctor.IsPrivate && ctor.GetParameters().Length == 0) return true;
            }
            return false;
        }

        internal static bool ImplementsGenericInterface(this Type generic, Type toCheck)
        {
            return generic.GetTypeInfo().IsGenericType &&
                   generic.GetGenericTypeDefinition()
                       .GetTypeInfo()
                       .ImplementedInterfaces.Any(type => type.Name.Equals(toCheck.Name));
        }

        [ItemNotNull]
        internal static async Task<ProxerResult> InitInitalisableProperties(this object source)
        {
            int lFailedInits = 0, lInitialiseFunctions = 0;
            ProxerResult lReturn = new ProxerResult();
            foreach (PropertyInfo propertyInfo in source.GetType().GetRuntimeProperties())
            {
                if (propertyInfo.PropertyType.ImplementsGenericInterface(typeof (IInitialisableProperty<>)))
                {
                    lInitialiseFunctions++;
                    try
                    {
                        object lInitialisableObject = propertyInfo.GetMethod.Invoke(source, null);
                        ProxerResult lResult = await (Task<ProxerResult>) lInitialisableObject.GetType()
                            .GetTypeInfo()
                            .GetDeclaredMethod("FetchObject")
                            .Invoke(lInitialisableObject, null);

                        if (!lResult.Success)
                        {
                            lReturn.AddExceptions(lResult.Exceptions);
                            lFailedInits++;
                        }
                    }
                    catch
                    {
                        lFailedInits++;
                    }
                }
            }

            if (lFailedInits < lInitialiseFunctions)
                lReturn.Success = true;

            return lReturn;
        }

        internal static bool IsFullyInitialised(this object objectToTest)
        {
            int lInitialisedProperties = 0, lInitialiseFunctions = 0;
            foreach (PropertyInfo propertyInfo in objectToTest.GetType().GetRuntimeProperties())
            {
                if (propertyInfo.PropertyType.ImplementsGenericInterface(typeof (IInitialisableProperty<>)))
                {
                    lInitialiseFunctions++;
                    try
                    {
                        object lPropertyObject;
                        bool lIsInitialised =
                            (bool) (lPropertyObject = propertyInfo.GetValue(objectToTest))
                                .GetType()
                                .GetTypeInfo()
                                .GetDeclaredProperty("IsInitialisedOnce").GetValue(lPropertyObject);

                        if (lIsInitialised) lInitialisedProperties++;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return lInitialisedProperties == lInitialiseFunctions;
        }

        [NotNull]
        internal static HtmlDocument LoadHtmlUtility([NotNull] this HtmlDocument document, [NotNull] string html)
        {
            document.LoadHtml(html);
            return document;
        }

        [NotNull]
        internal static IEnumerable<HtmlNode> SelectNodesUtility([NotNull] this HtmlNode node,
            [NotNull] string attribute, [NotNull] string value)
        {
            return
                node.DescendantsAndSelf()
                    .Where(x => x.Attributes.Contains(attribute) && x.Attributes[attribute].Value == value);
        }

        internal static DateTime ToDateTime([NotNull] string strFdate, [NotNull] string format = "dd.MM.yyyy")
        {
            return DateTime.ParseExact(
                strFdate,
                format,
                CultureInfo.InvariantCulture);
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