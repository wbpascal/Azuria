using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Search;
using Azuria.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Properties;
using JetBrains.Annotations;

namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents an <see cref="Anime" /> or <see cref="Manga" />.
    /// </summary>
    public interface IAnimeMangaObject : ISearchableObject
    {
        #region Properties

        /// <summary>
        ///     Gets the count of the <see cref="Anime.Episode">Episodes</see> or <see cref="Manga.Chapter">Chapters</see> the
        ///     <see cref="Anime" /> or <see cref="Manga" /> contains.
        /// </summary>
        [NotNull]
        InitialisableProperty<int> ContentCount { get; }

        /// <summary>
        ///     Gets the link to the cover of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        [NotNull]
        Uri CoverUri { get; }

        /// <summary>
        ///     Gets the description of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        [NotNull]
        InitialisableProperty<string> Description { get; }

        /// <summary>
        ///     Gets the english title of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        [NotNull]
        InitialisableProperty<string> EnglishTitle { get; }

        /// <summary>
        ///     Gets an enumeration of the age restrictions of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        [NotNull]
        InitialisableProperty<IEnumerable<FskObject>> Fsk { get; }

        /// <summary>
        ///     Gets an enumeration of all the genre of the <see cref="Anime" /> or <see cref="Manga" /> contains.
        /// </summary>
        [NotNull]
        InitialisableProperty<IEnumerable<GenreObject>> Genre { get; }

        /// <summary>
        ///     Gets the german title of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        [NotNull]
        InitialisableProperty<string> GermanTitle { get; }

        /// <summary>
        ///     Gets an enumeration of all the groups that translated the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        [NotNull]
        InitialisableProperty<IEnumerable<Group>> Groups { get; }

        /// <summary>
        ///     Gets the Id of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        int Id { get; }

        /// <summary>
        ///     Gets an enumeration of all the companies that were involved in making the <see cref="Anime" /> or
        ///     <see cref="Manga" />.
        /// </summary>
        [NotNull]
        InitialisableProperty<IEnumerable<Industry>> Industry { get; }

        /// <summary>
        ///     Gets if the <see cref="Anime" /> or <see cref="Manga" /> is licensed by a german company.
        /// </summary>
        [NotNull]
        InitialisableProperty<bool> IsLicensed { get; }

        /// <summary>
        ///     Gets the japanese title of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        [NotNull]
        InitialisableProperty<string> JapaneseTitle { get; }

        /// <summary>
        ///     Gets the original title of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        [NotNull]
        InitialisableProperty<string> Name { get; }

        /// <summary>
        ///     Gets the seasons the <see cref="Anime" /> or <see cref="Manga" /> aired in. If the enumerable only contains one
        ///     value the value is always the start season of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        [NotNull]
        InitialisableProperty<IEnumerable<string>> Season { get; }

        /// <summary>
        ///     Gets the status of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        InitialisableProperty<AnimeMangaStatus> Status { get; }

        /// <summary>
        ///     Gets the synonym the <see cref="Anime" /> or <see cref="Manga" /> is also known as.
        /// </summary>
        [NotNull]
        InitialisableProperty<string> Synonym { get; }

        #endregion

        #region

        /// <summary>
        ///     Adds the <see cref="Anime" /> or <see cref="Manga" /> to the planned list. If <paramref name="userControlPanel" />
        ///     is specified the object is also added to the corresponding <see cref="UserControlPanel.Anime" />- or
        ///     <see cref="UserControlPanel.Manga" />-enumeration.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, the object is added to.</param>
        /// <returns>If the action was successful.</returns>
        Task<ProxerResult> AddToPlanned(UserControlPanel userControlPanel = null);

        /// <summary>
        ///     Initialises the object.
        /// </summary>
        [ItemNotNull]
        [Obsolete("Please use the methods provided by the properties!")]
        Task<ProxerResult> Init();

        #endregion
    }
}