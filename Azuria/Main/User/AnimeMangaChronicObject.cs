using System;
using System.Linq;
using Azuria.Exceptions;
using Azuria.Main.Minor;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Main.User
{
    /// <summary>
    /// </summary>
    public class AnimeMangaChronicObject<T> where T : IAnimeMangaObject
    {
        internal AnimeMangaChronicObject([NotNull] T animeMangaObject, Language language, int number)
        {
            this.AnimeMangaObject = animeMangaObject;
            this.Language = language;
            this.Number = number;
        }

        #region Properties

        /// <summary>
        /// </summary>
        [NotNull]
        public T AnimeMangaObject { get; }

        /// <summary>
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// </summary>
        public Language Language { get; }

        /// <summary>
        ///     Gibt die <see cref="Anime.Episode">Episoden-</see> oder <see cref="Manga.Chapter">Kapitel</see>nummer zurück.
        /// </summary>
        public int Number { get; }

        #endregion

        #region

        [NotNull]
        internal static ProxerResult<AnimeMangaChronicObject<T>> GetChronicObjectFromNode([NotNull] HtmlNode node,
            [NotNull] Senpai senpai,
            bool extended = false)
        {
            try
            {
                DateTime lDateTime;
                IAnimeMangaObject lAnimeMangaObject = null;
                if (node.ChildNodes.Last().InnerText.Contains("Anime") ||
                    node.ChildNodes.Last().InnerText.Contains("Episode"))
                {
                    lAnimeMangaObject = new Anime(node.ChildNodes[0].InnerText,
                        Convert.ToInt32(
                            node.ChildNodes.Last().FirstChild.Attributes["href"].Value.GetTagContents(
                                extended ? "watch/" : "info/", extended ? "/" : "#top")
                                .First()), senpai);
                }
                else if (node.ChildNodes.Last().InnerText.Contains("Manga") ||
                         node.ChildNodes.Last().InnerText.Contains("Kapitel"))
                {
                    lAnimeMangaObject = new Manga(node.ChildNodes[0].InnerText,
                        Convert.ToInt32(
                            node.ChildNodes.Last().FirstChild.Attributes["href"].Value.GetTagContents(
                                extended ? "chapter/" : "info/", extended ? "/" : "#top")
                                .First()), senpai);
                }

                int lNumber = Convert.ToInt32(node.ChildNodes[1].InnerText);
                Language lLanguage = Language.Unkown;
                if (node.ChildNodes[2].InnerText.StartsWith("Ger") || node.ChildNodes[2].InnerText.Equals("Deutsch"))
                    lLanguage = Language.German;
                else if (node.ChildNodes[2].InnerText.StartsWith("Eng"))
                    lLanguage = Language.English;

                return !(lAnimeMangaObject is T)
                    ? new ProxerResult<AnimeMangaChronicObject<T>>(new Exception[] {new WrongResponseException()})
                    : new ProxerResult<AnimeMangaChronicObject<T>>(new AnimeMangaChronicObject<T>(
                        (T) lAnimeMangaObject, lLanguage,
                        lNumber)
                    {
                        DateTime =
                            DateTime.TryParse(node.ChildNodes[4].InnerText, out lDateTime)
                                ? lDateTime
                                : DateTime.MinValue
                    });
            }
            catch
            {
                return new ProxerResult<AnimeMangaChronicObject<T>>(new Exception[] {new WrongResponseException()});
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="instance"></param>
        public static explicit operator AnimeMangaChronicObject<T>(AnimeMangaChronicObject<IAnimeMangaObject> instance)
        {
            return new AnimeMangaChronicObject<T>((T) instance.AnimeMangaObject, instance.Language, instance.Number)
            {
                DateTime = instance.DateTime
            };
        }

        #endregion
    }
}