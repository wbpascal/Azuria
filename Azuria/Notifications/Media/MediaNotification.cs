using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Media.Properties;

namespace Azuria.Notifications.Media
{
    /// <summary>
    /// Represents an <see cref="Anime" />- or <see cref="Manga" />-notification.
    /// </summary>
    public class MediaNotification<T> : INotification where T : IMediaObject
    {
        internal MediaNotification(int notificationId, T mediaObject, int contentIndex,
            MediaLanguage language, DateTime timeStamp, Senpai senpai)
        {
            this.MediaObject = mediaObject;
            this.ContentIndex = contentIndex;
            this.Language = language;
            this.NotificationId = notificationId;
            this.Senpai = senpai;
            this.TimeStamp = timeStamp;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public int ContentIndex { get; set; }

        /// <summary>
        /// </summary>
        public MediaLanguage Language { get; set; }

        /// <summary>
        /// </summary>
        public T MediaObject { get; set; }

        /// <summary>
        /// </summary>
        public int NotificationId { get; }

        string INotification.NotificationId => this.NotificationId.ToString();

        /// <inheritdoc />
        public Senpai Senpai { get; }

        /// <summary>
        /// </summary>
        public DateTime TimeStamp { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public Task<IProxerResult> Delete()
        {
            return new NotificationManager(this.Senpai).DeleteMediaNotification(this.NotificationId);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<IProxerResult<IMediaContent<T>>> GetContentObject()
        {
            if (this.MediaObject is Anime)
            {
                IProxerResult<IEnumerable<Anime.Episode>> lEpisodesResult =
                    await (this.MediaObject as Anime).GetEpisodes((AnimeLanguage) this.Language);
                if (!lEpisodesResult.Success || (lEpisodesResult.Result == null))
                    return new ProxerResult<IMediaContent<T>>(lEpisodesResult.Exceptions);

                return new ProxerResult<IMediaContent<T>>(lEpisodesResult.Result.FirstOrDefault(
                    episode => episode.ContentIndex == this.ContentIndex) as IMediaContent<T>);
            }
            if (!(this.MediaObject is Manga)) return new ProxerResult<IMediaContent<T>>(new Exception[0]);

            IProxerResult<IEnumerable<Manga.Chapter>> lChaptersResult =
                await (this.MediaObject as Manga).GetChapters((Language) this.Language);
            if (!lChaptersResult.Success || (lChaptersResult.Result == null))
                return new ProxerResult<IMediaContent<T>>(lChaptersResult.Exceptions);

            return new ProxerResult<IMediaContent<T>>(lChaptersResult.Result.FirstOrDefault(
                chapter => chapter.ContentIndex == this.ContentIndex) as IMediaContent<T>);
        }

        #endregion
    }
}