using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Main.User.ControlPanel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AnimeMangaUcpObject<T> : AnimeMangaProgressObject<T> where T : IAnimeMangaObject
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
        public AnimeMangaUcpObject([NotNull] Azuria.User user, [NotNull] T animeMangaObject, int entryId,
            int currentProgress, int maxCount, AnimeMangaProgress progress, [NotNull] Senpai senpai)
            : base(user, animeMangaObject, entryId, currentProgress, maxCount, progress, senpai)
        {
            this._senpai = senpai;
        }

        private AnimeMangaUcpObject([NotNull] AnimeMangaProgressObject<T> baseClass, [NotNull] Senpai senpai)
            : this(
                baseClass.User, baseClass.AnimeMangaObject, baseClass.EntryId, baseClass.CurrentProgress,
                baseClass.MaxCount, baseClass.Progress, senpai)
        {
        }

        #region

        [NotNull]
        internal new static ProxerResult<AnimeMangaUcpObject<T>> ParseFromHtmlNode([NotNull] HtmlNode node,
            Azuria.User user,
            T animeMangaObject, AnimeMangaProgress progress,
            [NotNull] Senpai senpai)
        {
            try
            {
                return new ProxerResult<AnimeMangaUcpObject<T>>(new AnimeMangaUcpObject<T>(
                    AnimeMangaProgressObject<T>.ParseFromHtmlNode(node, user, animeMangaObject, progress, senpai),
                    senpai));
            }
            catch
            {
                return new ProxerResult<AnimeMangaUcpObject<T>>(new[] {new WrongResponseException()});
            }
        }

        #endregion
    }
}