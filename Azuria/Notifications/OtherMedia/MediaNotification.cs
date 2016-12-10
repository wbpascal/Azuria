using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Media.Properties;

namespace Azuria.Notifications.OtherMedia
{
    /// <summary>
    /// Represents an <see cref="Anime" />- or <see cref="Manga" />-notification.
    /// </summary>
    public class MediaNotification : INotification
    {
        internal MediaNotification(int notificationId, IMediaObject mediaObject, int contentIndex,
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
        public IMediaObject MediaObject { get; set; }

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
            return new NotificationManager(this.Senpai).DeleteOtherMediaNotification(this.NotificationId);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<IProxerResult<IMediaContent>> GetContentObject()
        {
            Anime lAnime = this.MediaObject as Anime;
            if (lAnime != null)
            {
                IProxerResult<IEnumerable<Anime.Episode>> lEpisodesResult = await lAnime.GetEpisodes
                    ((AnimeLanguage) this.Language).ConfigureAwait(false);
                if (!lEpisodesResult.Success || (lEpisodesResult.Result == null))
                    return new ProxerResult<IMediaContent>(lEpisodesResult.Exceptions);

                return new ProxerResult<IMediaContent>(lEpisodesResult.Result.FirstOrDefault(
                    episode => episode.ContentIndex == this.ContentIndex));
            }
            if (!(this.MediaObject is Manga)) return new ProxerResult<IMediaContent>(new Exception[0]);

            IProxerResult<IEnumerable<Manga.Chapter>> lChaptersResult = await ((Manga) this.MediaObject)
                .GetChapters((Language) this.Language).ConfigureAwait(false);
            if (!lChaptersResult.Success || (lChaptersResult.Result == null))
                return new ProxerResult<IMediaContent>(lChaptersResult.Exceptions);

            return new ProxerResult<IMediaContent>(lChaptersResult.Result.FirstOrDefault(
                chapter => chapter.ContentIndex == this.ContentIndex));
        }

        #endregion
    }
}