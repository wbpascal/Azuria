using System;
using System.Linq;
using Azuria.Exceptions;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;
using HtmlAgilityPack;

namespace Azuria.Main.User.ControlPanel
{
    /// <summary>
    /// </summary>
    public class AnimeMangaBookmarkObject
    {
        private readonly Senpai _senpai;

        internal AnimeMangaBookmarkObject(IAnimeMangaObject animeMangaObject, int number, bool isOnline, int entryId,
            Senpai senpai)
        {
            this._senpai = senpai;
            this.AnimeMangaObject = animeMangaObject;
            this.Number = number;
            this.IsOnline = isOnline;
            this.EntryId = entryId;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public IAnimeMangaObject AnimeMangaObject { get; }

        /// <summary>
        /// </summary>
        public int EntryId { get; }

        /// <summary>
        /// </summary>
        public bool IsOnline { get; }

        /// <summary>
        ///     Die Kapitel- oder Episodennummber.
        /// </summary>
        public int Number { get; }

        #endregion

        #region

        internal static ProxerResult<AnimeMangaBookmarkObject> ParseNode(HtmlNode node, Senpai senpai)
        {
            try
            {
                int lEntryId = Convert.ToInt32(node.GetAttributeValue("id", "entry-1").Substring("entry".Length));
                int lAnimeMangaId =
                    Convert.ToInt32(
                        node.FirstChild.GetAttributeValue("href", "/watch/-1/").GetTagContents("/", "/")[1]);
                string lAnimeMangaTitle = node.FirstChild.ChildNodes[1].InnerText;

                int lNumber =
                    Convert.ToInt32(
                        node.FirstChild.GetAttributeValue("href", lAnimeMangaId + "/-1/")
                            .GetTagContents(lAnimeMangaId + "/", "/")
                            .First());
                bool lIsOnline = node.FirstChild.FirstChild.GetAttributeValue("src", "").Contains("onlineicon");

                IAnimeMangaObject lAnimeMangaObject = null;

                string lAnimeMangaType = node.FirstChild.ChildNodes[2].InnerText;
                switch (lAnimeMangaType)
                {
                    case "Animeserie":
                    case "Movie":
                    case "OVA":
                        lAnimeMangaObject = new Anime(lAnimeMangaTitle, lAnimeMangaId, senpai);
                        break;
                    case "Mangaserie":
                    case "One-Shot":
                        lAnimeMangaObject = new Manga(lAnimeMangaTitle, lAnimeMangaId, senpai);
                        break;
                }

                return
                    new ProxerResult<AnimeMangaBookmarkObject>(new AnimeMangaBookmarkObject(lAnimeMangaObject, lNumber,
                        lIsOnline, lEntryId, senpai));
            }
            catch
            {
                return new ProxerResult<AnimeMangaBookmarkObject>(new[] {new WrongResponseException()});
            }
        }

        #endregion
    }
}