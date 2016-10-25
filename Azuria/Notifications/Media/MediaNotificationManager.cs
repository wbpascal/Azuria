using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.Notifications.Media
{
    /// <summary>
    /// </summary>
    public class MediaNotificationManager : INotificationManager
    {
        private readonly List<AnimeNotificationEventHandler> _animeNotificationEventHandlers =
            new List<AnimeNotificationEventHandler>();

        private readonly List<MangaNotificationEventHandler> _mangaNotificationEventHandlers =
            new List<MangaNotificationEventHandler>();

        private readonly List<MediaNotificationEventHandler> _mediaNotificationEventHandlers =
            new List<MediaNotificationEventHandler>();

        private readonly Senpai _senpai;

        private MediaNotificationManager(Senpai senpai)
        {
            this._senpai = senpai;
            this.Notifications = new MediaNotificationCollection<IMediaObject>(senpai);
            this.NotificationsAnime = new MediaNotificationCollection<Anime>(senpai);
            this.NotificationsManga = new MediaNotificationCollection<Manga>(senpai);
        }

        #region Properties

        /// <summary>
        /// </summary>
        public MediaNotificationCollection<IMediaObject> Notifications { get; }

        /// <summary>
        /// </summary>
        public MediaNotificationCollection<Anime> NotificationsAnime { get; }

        /// <summary>
        /// </summary>
        public MediaNotificationCollection<Manga> NotificationsManga { get; }

        Senpai INotificationManager.Senpai => this._senpai;

        #endregion

        #region Events

        /// <summary>
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void AnimeNotificationEventHandler(Senpai sender, IEnumerable<MediaNotification<Anime>> e);

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ExceptionThrownNotificationFetchEventHandler(
            MediaNotificationManager sender, Exception e);

        /// <summary>
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void MangaNotificationEventHandler(Senpai sender, IEnumerable<MediaNotification<Manga>> e);

        /// <summary>
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void MediaNotificationEventHandler(
            Senpai sender, IEnumerable<MediaNotification<IMediaObject>> e);

        /// <summary>
        /// </summary>
        public event ExceptionThrownNotificationFetchEventHandler ExceptionThrownNotificationFetch;

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
        public event MediaNotificationEventHandler MediaNotificationRecieved
        {
            add
            {
                if (this._mediaNotificationEventHandlers.Contains(value)) return;
                this._mediaNotificationEventHandlers.Add(value);
                NotificationCountManager.CheckNotificationsForNewEvent().ConfigureAwait(false);
            }
            remove { this._mediaNotificationEventHandlers.Remove(value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static MediaNotificationManager Create(Senpai senpai)
        {
            return NotificationCountManager.GetOrAddManager(senpai, new MediaNotificationManager(senpai));
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<ProxerResult> DeleteAllReadNotification()
        {
            ProxerResult<ProxerApiResponse> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.NotificationDelete(this._senpai));
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public async Task<ProxerResult> DeleteNotification(int notificationId)
        {
            ProxerResult<ProxerApiResponse> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.NotificationDelete(this._senpai, notificationId));
            return lResult.Success ? new ProxerResult() : new ProxerResult(lResult.Exceptions);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnAnimeNotificationRecieved(Senpai sender, IEnumerable<MediaNotification<Anime>> e)
        {
            IEnumerable<MediaNotification<Anime>> lNotifications =
                e as MediaNotification<Anime>[] ?? e.ToArray();

            foreach (AnimeNotificationEventHandler newsNotificationEventHandler in this._animeNotificationEventHandlers)
                newsNotificationEventHandler?.Invoke(sender, lNotifications);
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnExceptionThrownNotificationFetch(Exception e)
        {
            this.ExceptionThrownNotificationFetch?.Invoke(this, e);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMangaNotificationRecieved(Senpai sender, IEnumerable<MediaNotification<Manga>> e)
        {
            IEnumerable<MediaNotification<Manga>> lNotifications =
                e as MediaNotification<Manga>[] ?? e.ToArray();

            foreach (MangaNotificationEventHandler newsNotificationEventHandler in this._mangaNotificationEventHandlers)
                newsNotificationEventHandler?.Invoke(sender, lNotifications);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMediaNotificationRecieved(Senpai sender,
            IEnumerable<MediaNotification<IMediaObject>> e)
        {
            IEnumerable<MediaNotification<IMediaObject>> lNotifications =
                e as MediaNotification<IMediaObject>[] ?? e.ToArray();

            foreach (
                MediaNotificationEventHandler newsNotificationEventHandler in
                this._mediaNotificationEventHandlers)
                newsNotificationEventHandler?.Invoke(sender, lNotifications);
        }

        void INotificationManager.OnNewNotificationsAvailable(NotificationCountDataModel notificationsCounts)
        {
            if (notificationsCounts.OtherMedia == 0) return;

            try
            {
                MediaNotification<IMediaObject>[] lMediaNotifications =
                    new MediaNotificationCollection<IMediaObject>(this._senpai,
                        notificationsCounts.OtherMedia).Take(
                        notificationsCounts.OtherMedia).ToArray();

                MediaNotification<Anime>[] lAnimeNotifications =
                    new MediaNotificationCollection<Anime>(this._senpai, notificationsCounts.OtherMedia).Take(
                        notificationsCounts.OtherMedia).ToArray();
                MediaNotification<Manga>[] lMangaNotifications =
                    new MediaNotificationCollection<Manga>(this._senpai, notificationsCounts.OtherMedia).Take(
                        notificationsCounts.OtherMedia).ToArray();

                if (lMediaNotifications.Length > 0)
                    this.OnMediaNotificationRecieved(this._senpai, lMediaNotifications);
                if (lAnimeNotifications.Length > 0)
                    this.OnAnimeNotificationRecieved(this._senpai, lAnimeNotifications);
                if (lMangaNotifications.Length > 0)
                    this.OnMangaNotificationRecieved(this._senpai, lMangaNotifications);
            }
            catch (Exception ex)
            {
                this.OnExceptionThrownNotificationFetch(ex);
            }
        }

        #endregion
    }
}