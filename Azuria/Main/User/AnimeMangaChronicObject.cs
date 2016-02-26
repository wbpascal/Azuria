using System;
using System.Linq;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Main.Minor;
using Azuria.Utilities;
using HtmlAgilityPack;

namespace Azuria.Main.User
{
    /// <summary>
    /// </summary>
    public class AnimeMangaChronicObject
    {
        internal AnimeMangaChronicObject(IAnimeMangaObject animeMangaObject, Language language, int number)
        {
            this.AnimeMangaObject = animeMangaObject;
            this.Language = language;
            this.Number = number;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public IAnimeMangaObject AnimeMangaObject { get; }

        /// <summary>
        /// </summary>
        public AnimeMangaType AnimeMangaType
            =>
                this.AnimeMangaObject is Anime
                    ? AnimeMangaType.Anime
                    : this.AnimeMangaObject is Manga ? AnimeMangaType.Manga : AnimeMangaType.Unknown;

        /// <summary>
        /// </summary>
        public Language Language { get; private set; }

        /// <summary>
        ///     Gibt die <see cref="Anime.Episode">Episoden-</see> oder <see cref="Manga.Chapter">Kapitel</see>nummer zurück.
        /// </summary>
        public int Number { get; private set; }

        #endregion

        #region

        internal static ProxerResult<AnimeMangaChronicObject> GetChronicObjectFromNode(HtmlNode node, Senpai senpai)
        {
            try
            {
                IAnimeMangaObject lAnimeMangaObject = null;
                if (node.ChildNodes[4].InnerText.Contains("Anime"))
                {
                    lAnimeMangaObject = new Anime(node.ChildNodes[0].InnerText,
                        Convert.ToInt32(
                            node.ChildNodes[4].FirstChild.Attributes["href"].Value.GetTagContents("info/", "#top")
                                .First()), senpai);
                }
                else if (node.ChildNodes[4].InnerText.Contains("Manga"))
                {
                    lAnimeMangaObject = new Manga(node.ChildNodes[0].InnerText,
                        Convert.ToInt32(
                            node.ChildNodes[4].FirstChild.Attributes["href"].Value.GetTagContents("info/", "#top")
                                .First()), senpai);
                }

                int lNumber = Convert.ToInt32(node.ChildNodes[1].InnerText);
                Language lLanguage = Language.Unkown;
                if (node.ChildNodes[2].InnerText.StartsWith("Ger") || node.ChildNodes[2].InnerText.Equals("Deutsch"))
                    lLanguage = Language.German;
                else if (node.ChildNodes[2].InnerText.StartsWith("Eng"))
                    lLanguage = Language.English;

                return
                    new ProxerResult<AnimeMangaChronicObject>(new AnimeMangaChronicObject(lAnimeMangaObject, lLanguage,
                        lNumber));
            }
            catch
            {
                return new ProxerResult<AnimeMangaChronicObject>(new Exception[] {new WrongResponseException()});
            }
        }

        #endregion
    }
}