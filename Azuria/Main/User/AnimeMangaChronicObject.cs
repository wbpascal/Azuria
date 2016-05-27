using System;
using System.Linq;
using Azuria.Exceptions;
using Azuria.Main.Minor;
using Azuria.Main.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Main.User
{
    /// <summary>
    ///     Represents an entry in the chronic of a <see cref="Azuria.User" />. Detailed information, like
    ///     <see cref="AnimeMangaChronicObject{T}.DateTime" /> are only supplied if fetched from a
    ///     <see cref="UserControlPanel" /> instance.
    /// </summary>
    /// <typeparam name="T">
    ///     Wether the content object this object points to is from an <see cref="Anime" /> or
    ///     <see cref="Manga" />.
    /// </typeparam>
    public class AnimeMangaChronicObject<T> where T : IAnimeMangaObject
    {
        internal AnimeMangaChronicObject([NotNull] IAnimeMangaContent<T> animeMangaContentObject,
            DateTime dateTime = default(DateTime))
        {
            this.AnimeMangaContentObject = animeMangaContentObject;
            this.DateTime = dateTime;
        }

        #region Properties

        /// <summary>
        ///     Gets the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> this entry is for.
        /// </summary>
        [NotNull]
        public IAnimeMangaContent<T> AnimeMangaContentObject { get; }

        /// <summary>
        ///     Gets the time when the entry was made. Only available if the object was fetched from a
        ///     <see cref="UserControlPanel" />.
        /// </summary>
        public DateTime DateTime { get; }

        #endregion

        #region

        [NotNull]
        internal static ProxerResult<AnimeMangaChronicObject<T>> GetChronicObjectFromNode([NotNull] HtmlNode node,
            [NotNull] Senpai senpai, bool extended = false)
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
                AnimeLanguage lAnimeLanguage = AnimeLanguage.Unknown;
                if (node.ChildNodes[2].InnerText.StartsWith("Ger") || node.ChildNodes[2].InnerText.Equals("Deutsch"))
                {
                    lLanguage = Language.German;
                    switch (node.ChildNodes[2].InnerText)
                    {
                        case "GerSub":
                            lAnimeLanguage = AnimeLanguage.GerSub;
                            break;
                        case "GerDub":
                            lAnimeLanguage = AnimeLanguage.GerDub;
                            break;
                    }
                }
                else if (node.ChildNodes[2].InnerText.StartsWith("Eng"))
                {
                    lLanguage = Language.English;
                    switch (node.ChildNodes[2].InnerText)
                    {
                        case "EngSub":
                            lAnimeLanguage = AnimeLanguage.EngSub;
                            break;
                        case "EngDub":
                            lAnimeLanguage = AnimeLanguage.EngDub;
                            break;
                    }
                }

                Anime lAnime = lAnimeMangaObject as Anime;
                IAnimeMangaContentBase lAnimeMangaContentBase = lAnime != null
                    ? new Anime.Episode(lAnime, lNumber, lAnimeLanguage, senpai)
                    : lAnimeMangaObject != null
                        ? (IAnimeMangaContentBase)
                            new Manga.Chapter((Manga) lAnimeMangaObject, lNumber, lLanguage, senpai)
                        : null;

                return lAnimeMangaContentBase == null
                    ? new ProxerResult<AnimeMangaChronicObject<T>>(new Exception[] {new WrongResponseException()})
                    : new ProxerResult<AnimeMangaChronicObject<T>>(
                        new AnimeMangaChronicObject<T>((IAnimeMangaContent<T>) lAnimeMangaContentBase,
                            DateTime.TryParse(node.ChildNodes[4].InnerText, out lDateTime)
                                ? lDateTime
                                : DateTime.MinValue));
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
            return new AnimeMangaChronicObject<T>((IAnimeMangaContent<T>) instance.AnimeMangaContentObject,
                instance.DateTime);
        }

        #endregion
    }
}