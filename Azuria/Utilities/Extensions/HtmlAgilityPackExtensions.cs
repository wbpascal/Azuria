using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Utilities.Extensions
{
    internal static class HtmlAgilityPackExtensions
    {
        #region

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
                    .Where(x => x.Attributes.Contains(attribute) && (x.Attributes[attribute].Value == value));
        }

        #endregion
    }
}