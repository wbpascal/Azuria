using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Azuria.AnimeManga;
using Azuria.Api.v1.DataModels.Notifications;

namespace Azuria.Notifications.AnimeManga
{
    /// <summary>
    /// </summary>
    public class AnimeMangaNotificationManager : INotificationManager
    {
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
        public event AnimeNotificationEventHandler AnimeNotificationRecieved;

        /// <summary>
        /// </summary>
        public event MangaNotificationEventHandler MangaNotificationRecieved;

        /// <summary>
        /// </summary>
        public event AnimeMangaNotificationEventHandler AnimeMangaNotificationRecieved;

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
            this.AnimeMangaNotificationRecieved?.Invoke(sender, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAnimeNotificationRecieved(Senpai sender, IEnumerable<AnimeMangaNotification<Anime>> e)
        {
            this.AnimeNotificationRecieved?.Invoke(sender, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMangaNotificationRecieved(Senpai sender, IEnumerable<AnimeMangaNotification<Manga>> e)
        {
            this.MangaNotificationRecieved?.Invoke(sender, e);
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        void INotificationManager.OnNewNotificationsAvailable(NotificationCountDataModel notificationsCounts)
        {
            AnimeMangaNotification<IAnimeMangaObject>[] lAnimeMangaNotifications =
                new AnimeMangaNotificationCollection<IAnimeMangaObject>(this._senpai).Take(
                    notificationsCounts.OtherAnimeManga).ToArray();

            if (lAnimeMangaNotifications.Length > 0)
                this.OnAnimeMangaNotificationRecieved(this._senpai, lAnimeMangaNotifications);
            if (
                lAnimeMangaNotifications.Any(
                    notification => notification.GetType() == typeof(AnimeMangaNotification<Anime>)))
                this.OnAnimeNotificationRecieved(this._senpai,
                    lAnimeMangaNotifications.Where(
                            notification => notification.GetType() == typeof(AnimeMangaNotification<Anime>))
                        .Cast<AnimeMangaNotification<Anime>>());
            if (
                lAnimeMangaNotifications.Any(
                    notification => notification.GetType() == typeof(AnimeMangaNotification<Manga>)))
                this.OnMangaNotificationRecieved(this._senpai,
                    lAnimeMangaNotifications.Where(
                            notification => notification.GetType() == typeof(AnimeMangaNotification<Manga>))
                        .Cast<AnimeMangaNotification<Manga>>());
        }

        #endregion
    }
}