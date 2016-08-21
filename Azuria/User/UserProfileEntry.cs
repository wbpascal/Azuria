using System;
using Azuria.AnimeManga;
using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.DataModels.User;
using Azuria.User.Comment;

namespace Azuria.User
{
    /// <summary>
    /// </summary>
    public class UserProfileEntry<T> where T : class, IAnimeMangaObject
    {
        internal UserProfileEntry(ListDataModel dataModel, User user)
        {
            this.User = user;
            this.AnimeMangaObject = this.InitAnimeMangaObject(dataModel);
            this.Comment = new Comment<T>(dataModel, user, this.AnimeMangaObject);
        }

        #region Properties

        /// <summary>
        /// </summary>
        public T AnimeMangaObject { get; }

        /// <summary>
        /// </summary>
        public Comment<T> Comment { get; }

        /// <summary>
        /// </summary>
        public User User { get; set; }

        #endregion

        #region

        private T InitAnimeMangaObject(ListDataModel dataModel)
        {
            T lReturnObject;

            if (typeof(T) == typeof(Anime))
            {
                Anime lAnime = new Anime(dataModel.EntryName, dataModel.EntryId);
                lAnime.AnimeMedium.SetInitialisedObject((AnimeMedium) dataModel.EntryMedium);
                lReturnObject = lAnime as T;
            }
            else if (typeof(T) == typeof(Manga))
            {
                Manga lManga = new Manga(dataModel.EntryName, dataModel.EntryId);
                lManga.MangaMedium.SetInitialisedObject((MangaMedium) dataModel.EntryMedium);
                lReturnObject = lManga as T;
            }
            else throw new ArgumentException(nameof(T));

            lReturnObject?.ContentCount.SetInitialisedObject(dataModel.ContentCount);
            lReturnObject?.Status.SetInitialisedObject(dataModel.EntryStatus);

            return lReturnObject;
        }

        #endregion
    }
}