using System.Collections.Generic;
using System.Linq;
using Azuria.AnimeManga;
using Azuria.Api.v1.DataModels.Notifications;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    /// </summary>
    public class AnimeMangaNotificationManager : INotificationManager
    {
        private readonly List<AnimeMangaNotificationEventHandler> _animeMangaNotificationEventHandlers =
            new List<AnimeMangaNotificationEventHandler>();

        private readonly List<AnimeNotificationEventHandler> _animeNotificationEventHandlers =
            new List<AnimeNotificationEventHandler>();

        private readonly List<MangaNotificationEventHandler> _mangaNotificationEventHandlers =
            new List<MangaNotificationEventHandler>();

        private readonly Senpai _senpai;

        private AnimeMangaNotificationManager(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Properties

        Senpai INotificationManager.Senpai => this._senpai;

        #endregion

        #region Events

        /// <summary>
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void AnimeMangaNotificationEventHandler(
            Senpai sender, IEnumerable<AnimeMangaNotification<IAnimeMangaObject>> e);

        /// <summary>
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void AnimeNotificationEventHandler(Senpai sender, IEnumerable<AnimeMangaNotification<Anime>> e);

        /// <summary>
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void MangaNotificationEventHandler(Senpai sender, IEnumerable<AnimeMangaNotification<Manga>> e);

        /// <summary>
        /// </summary>
        public event AnimeNotificationEventHandler AnimeNotificationRecieved
        {
            add
            {
                if (this._animeNotificationEventHandlers.Contains(value)) return;
                this._animeNotificationEventHandlers.Add(value);
                NotificationCountManager.CheckNotificationsForNewEvent().ConfigureAwait(false);
            }
            remove { this._animeNotificationEventHandlers.Remove(value); }
        }

        /// <summary>
        /// </summary>
        public event MangaNotificationEventHandler MangaNotificationRecieved
        {
            add
            {
                if (this._mangaNotificationEventHandlers.Contains(value)) return;
                this._mangaNotificationEventHandlers.Add(value);
                NotificationCountManager.CheckNotificationsForNewEvent().ConfigureAwait(false);
            }
            remove { this._mangaNotificationEventHandlers.Remove(value); }
        }

        /// <summary>
        /// </summary>
        public event AnimeMangaNotificationEventHandler AnimeMangaNotificationRecieved
        {
            add
            {
                if (this._animeMangaNotificationEventHandlers.Contains(value)) return;
                this._animeMangaNotificationEventHandlers.Add(value);
                NotificationCountManager.CheckNotificationsForNewEvent().ConfigureAwait(false);
            }
            remove { this._animeMangaNotificationEventHandlers.Remove(value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static AnimeMangaNotificationManager Create(Senpai senpai)
        {
            return NotificationCountManager.GetOrAddManager(senpai, new AnimeMangaNotificationManager(senpai));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAnimeMangaNotificationRecieved(Senpai sender,
            IEnumerable<AnimeMangaNotification<IAnimeMangaObject>> e)
        {
            IEnumerable<AnimeMangaNotification<IAnimeMangaObject>> lNotifications =
                e as AnimeMangaNotification<IAnimeMangaObject>[] ?? e.ToArray();

            foreach (
                AnimeMangaNotificationEventHandler newsNotificationEventHandler in
                this._animeMangaNotificationEventHandlers)
                newsNotificationEventHandler?.Invoke(sender, lNotifications);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAnimeNotificationRecieved(Senpai sender, IEnumerable<AnimeMangaNotification<Anime>> e)
        {
            IEnumerable<AnimeMangaNotification<Anime>> lNotifications =
                e as AnimeMangaNotification<Anime>[] ?? e.ToArray();

            foreach (AnimeNotificationEventHandler newsNotificationEventHandler in this._animeNotificationEventHandlers)
                newsNotificationEventHandler?.Invoke(sender, lNotifications);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMangaNotificationRecieved(Senpai sender, IEnumerable<AnimeMangaNotification<Manga>> e)
        {
            IEnumerable<AnimeMangaNotification<Manga>> lNotifications =
                e as AnimeMangaNotification<Manga>[] ?? e.ToArray();

            foreach (MangaNotificationEventHandler newsNotificationEventHandler in this._mangaNotificationEventHandlers)
                newsNotificationEventHandler?.Invoke(sender, lNotifications);
        }

        void INotificationManager.OnNewNotificationsAvailable(NotificationCountDataModel notificationsCounts)
        {
            AnimeMangaNotification<IAnimeMangaObject>[] lAnimeMangaNotifications =
                new AnimeMangaNotificationCollection<IAnimeMangaObject>(this._senpai,
                    notificationsCounts.OtherAnimeManga).Take(
                    notificationsCounts.OtherAnimeManga).ToArray();

            AnimeMangaNotification<Anime>[] lAnimeNotifications =
                new AnimeMangaNotificationCollection<Anime>(this._senpai, notificationsCounts.OtherAnimeManga).Take(
                    notificationsCounts.OtherAnimeManga).ToArray();
            AnimeMangaNotification<Manga>[] lMangaNotifications =
                new AnimeMangaNotificationCollection<Manga>(this._senpai, notificationsCounts.OtherAnimeManga).Take(
                    notificationsCounts.OtherAnimeManga).ToArray();

            if (lAnimeMangaNotifications.Length > 0)
                this.OnAnimeMangaNotificationRecieved(this._senpai, lAnimeMangaNotifications);
            if (lAnimeNotifications.Length > 0)
                this.OnAnimeNotificationRecieved(this._senpai, lAnimeNotifications);
            if (lMangaNotifications.Length > 0)
                this.OnMangaNotificationRecieved(this._senpai, lMangaNotifications);
        }

        #endregion
    }
}