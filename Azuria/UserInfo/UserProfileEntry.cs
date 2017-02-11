using System;
using Azuria.Api.v1.DataModels.User;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.UserInfo.Comment;
using Azuria.Utilities.Properties;

namespace Azuria.UserInfo
{
    /// <summary>
    /// </summary>
    public class UserProfileEntry<T> : IUserProfileEntry where T : class, IMediaObject
    {
        internal UserProfileEntry(ListDataModel dataModel, User user)
        {
            this.User = user;
            this.MediaObject = this.InitMediaObject(dataModel);
            this.Comment = new Comment<T>(dataModel, user, this.MediaObject);
        }

        #region Properties

        /// <summary>
        /// </summary>
        public Comment<T> Comment { get; }

        IComment IUserProfileEntry.Comment => this.Comment;

        /// <summary>
        /// </summary>
        public T MediaObject { get; }

        IMediaObject IUserProfileEntry.MediaObject => this.MediaObject;

        /// <inheritdoc />
        public User User { get; set; }

        #endregion

        #region Methods

        private T InitMediaObject(ListDataModel dataModel)
        {
            T lReturnObject;

            if (typeof(T) == typeof(Anime))
            {
                Anime lAnime = new Anime(dataModel.EntryName, dataModel.EntryId);
                (lAnime.AnimeMedium as InitialisableProperty<AnimeMedium>)?.Set(
                    (AnimeMedium) dataModel.EntryMedium);
                lReturnObject = lAnime as T;
            }
            else if (typeof(T) == typeof(Manga))
            {
                Manga lManga = new Manga(dataModel.EntryName, dataModel.EntryId);
                (lManga.MangaMedium as InitialisableProperty<MangaMedium>)?.Set(
                    (MangaMedium) dataModel.EntryMedium);
                lReturnObject = lManga as T;
            }
            else
            {
                throw new ArgumentException(nameof(T));
            }

            (lReturnObject?.ContentCount as InitialisableProperty<int>)?.Set(dataModel.ContentCount);
            (lReturnObject?.Status as InitialisableProperty<MediaStatus>)?.Set(
                dataModel.EntryStatus);

            return lReturnObject;
        }

        #endregion
    }
}