using System;
using System.Linq;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Main.User
{
    /// <summary>
    ///     Eine Klasse, die den Fortschritt eines <see cref="Azuria.User">Benutzer</see> bei einem
    ///     <see cref="Anime">Anime</see>
    ///     oder <see cref="Manga">Manga</see> darstellt.
    /// </summary>
    public class AnimeMangaProgressObject<T> where T : IAnimeMangaObject
    {
        private readonly Senpai _senpai;

        /// <summary>
        ///     Initialisiert das Objekt.
        /// </summary>
        /// <param name="user">Der Benutzer, der mit dem Fortschritt zusammenhängt.</param>
        /// <param name="animeMangaObject">
        ///     Der <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see>, mit dem das Objekt
        ///     zusammenhängt.
        /// </param>
        /// <param name="entryId"></param>
        /// <param name="currentProgress">Der aktuelle Fortschritt.</param>
        /// <param name="maxCount">
        ///     Die maximale Anzahl der <see cref="Anime.Episode">Episoden</see> oder
        ///     <see cref="Manga.Chapter">Kapitel</see>.
        /// </param>
        /// <param name="progress">
        ///     Die Kategorie, in der der <paramref name="user">Benutzer</paramref> seinen Fortschritt
        ///     einsortiert hat.
        /// </param>
        /// <param name="senpai"></param>
        public AnimeMangaProgressObject([NotNull] Azuria.User user, [NotNull] T animeMangaObject,
            int entryId, int currentProgress,
            int maxCount, AnimeMangaProgress progress, [NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.User = user;
            this.AnimeMangaObject = animeMangaObject;
            this.EntryId = entryId;
            this.CurrentProgress = currentProgress;
            this.MaxCount = maxCount;
            this.Progress = progress;
        }

        #region Properties

        /// <summary>
        ///     Gibt den <see cref="Anime">Anime</see> oder <see cref="Manga">Manga</see> zurück, mit dem das Objekt zusammenhängt.
        /// </summary>
        [NotNull]
        public T AnimeMangaObject { get; }

        /// <summary>
        ///     Gibt den aktuellen Fortschritt aus oder legt diesen fest.
        /// </summary>
        public int CurrentProgress { get; set; }

        /// <summary>
        /// </summary>
        public int EntryId { get; set; }

        /// <summary>
        ///     Gibt die maximale Anzahl der <see cref="Anime.Episode">Episoden</see> oder <see cref="Manga.Chapter">Kapitel</see>
        ///     zurück, die durch das Objekt dargestellt werden.
        /// </summary>
        public int MaxCount { get; }

        /// <summary>
        ///     Gibt die Kategorie, in der der <see cref="User">Benutzer</see> seinen Fortschritt einsortiert hat zurück.
        /// </summary>
        public AnimeMangaProgress Progress { get; }

        /// <summary>
        ///     Gibt den <see cref="Azuria.User">Benutzer</see> zurück, mit dem der Fortschritt zusammenhängt.
        /// </summary>
        [NotNull]
        public Azuria.User User { get; }

        #endregion

        #region

        [NotNull]
        internal static AnimeMangaProgressObject<T> ParseFromHtmlNode([NotNull] HtmlNode node, Azuria.User user,
            T animeMangaObject, AnimeMangaProgress progress,
            [NotNull] Senpai senpai)
        {
            return new AnimeMangaProgressObject<T>(user, animeMangaObject,
                Convert.ToInt32(node.GetAttributeValue("id", "entry-1").Substring("entry".Length)),
                Convert.ToInt32(
                    node.ChildNodes[4].ChildNodes.First(
                        htmlNode => htmlNode.GetAttributeValue("class", "").Equals("state")).InnerText.Split('/')[0]
                        .Trim()),
                Convert.ToInt32(
                    node.ChildNodes[4].ChildNodes.First(
                        htmlNode => htmlNode.GetAttributeValue("class", "").Equals("state")).InnerText.Split('/')[1]
                        .Trim()),
                progress, senpai);
        }

        #endregion
    }
}