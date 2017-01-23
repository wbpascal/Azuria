using System;
using System.Collections.Generic;
using Azuria.Info;
using Azuria.Media.Properties;
using Azuria.UserInfo.Comment;
using Azuria.Utilities.Properties;

namespace Azuria.Media
{
    /// <summary>
    /// Represents an <see cref="Anime" /> or <see cref="Manga" />.
    /// </summary>
    public interface IMediaObject
    {
        #region Properties

        /// <summary>
        /// Gets the total amount of clicks the <see cref="Anime" /> or <see cref="Manga" /> recieved. Is reset every 3 months.
        /// </summary>
        IInitialisableProperty<int> Clicks { get; }

        /// <summary>
        /// Gets the comments of the anime in a chronological order.
        /// </summary>
        CommentEnumerable<IMediaObject> CommentsLatest { get; }

        /// <summary>
        /// Gets the comments of the anime ordered by rating.
        /// </summary>
        CommentEnumerable<IMediaObject> CommentsRating { get; }

        /// <summary>
        /// Gets the count of the <see cref="Anime.Episode">Episodes</see> or <see cref="Manga.Chapter">Chapters</see> the
        /// <see cref="Anime" /> or <see cref="Manga" /> contains.
        /// </summary>
        IInitialisableProperty<int> ContentCount { get; }

        /// <summary>
        /// Gets the link to the cover of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        Uri CoverUri { get; }

        /// <summary>
        /// Gets the description of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        IInitialisableProperty<string> Description { get; }

        /// <summary>
        /// Gets the english title of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        IInitialisableProperty<string> EnglishTitle { get; }

        /// <summary>
        /// Gets an enumeration of the age restrictions of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        IInitialisableProperty<IEnumerable<FskType>> Fsk { get; }

        /// <summary>
        /// Gets an enumeration of all the genre of the <see cref="Anime" /> or <see cref="Manga" /> contains.
        /// </summary>
        IInitialisableProperty<IEnumerable<GenreType>> Genre { get; }

        /// <summary>
        /// Gets the german title of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        IInitialisableProperty<string> GermanTitle { get; }

        /// <summary>
        /// Gets the Id of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets an enumeration of all the companies that were involved in making the <see cref="Anime" /> or
        /// <see cref="Manga" />.
        /// </summary>
        IInitialisableProperty<IEnumerable<Industry>> Industry { get; }

        /// <summary>
        /// Gets whether the <see cref="Anime" /> or <see cref="Manga" /> contains H-Content (Adult).
        /// </summary>
        IInitialisableProperty<bool> IsHContent { get; }

        /// <summary>
        /// Gets if the <see cref="Anime" /> or <see cref="Manga" /> is licensed by a german company.
        /// </summary>
        IInitialisableProperty<bool?> IsLicensed { get; }

        /// <summary>
        /// Gets the japanese title of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        IInitialisableProperty<string> JapaneseTitle { get; }

        /// <summary>
        /// Gets the original title of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        IInitialisableProperty<string> Name { get; }

        /// <summary>
        /// Gets the rating of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        IInitialisableProperty<MediaRating> Rating { get; }

        /// <summary>
        /// Gets the relations of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        IInitialisableProperty<IEnumerable<IMediaObject>> Relations { get; }

        /// <summary>
        /// Gets the seasons the <see cref="Anime" /> or <see cref="Manga" /> aired in.
        /// </summary>
        IInitialisableProperty<MediaSeasonInfo> Season { get; }

        /// <summary>
        /// Gets the status of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        IInitialisableProperty<MediaStatus> Status { get; }

        /// <summary>
        /// Gets the synonym the <see cref="Anime" /> or <see cref="Manga" /> is also known as.
        /// </summary>
        IInitialisableProperty<string> Synonym { get; }

        /// <summary>
        /// Gets the tags the <see cref="Anime" /> or <see cref="Manga" /> was tagged with.
        /// </summary>
        IInitialisableProperty<IEnumerable<Tag>> Tags { get; }

        /// <summary>
        /// Gets an enumeration of all the groups that translated the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        IInitialisableProperty<IEnumerable<Translator>> Translator { get; }

        #endregion
    }
}