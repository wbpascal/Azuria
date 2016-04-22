using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Main.Minor;
using Azuria.Main.Search;
using Azuria.Main.User.Comment;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Initialisation;
using JetBrains.Annotations;

namespace Azuria.Main
{
    /// <summary>
    ///     Eine Klasse, die einen <see cref="Anime" /> oder <see cref="Manga" /> darstellt.
    /// </summary>
    public interface IAnimeMangaObject : ISearchableObject
    {
        #region Properties

        /// <summary>
        ///     Gibt den Link zum Cover des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        Uri CoverUri { get; }

        /// <summary>
        ///     Gibt die Beschreibung des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<string> Description { get; }

        /// <summary>
        ///     Gibt den englische Titel des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<string> EnglishTitle { get; }

        /// <summary>
        ///     Gibt die Links zu allen FSK-Beschränkungen des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<Dictionary<Uri, string>> Fsk { get; }

        /// <summary>
        ///     Gitb die Genres des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<IEnumerable<GenreObject>> Genre { get; }

        /// <summary>
        ///     Gibt den deutschen Titel des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<string> GermanTitle { get; }

        /// <summary>
        ///     Gibt die Gruppen zurück, die den <see cref="Anime" /> oder <see cref="Manga" /> übersetzten oder übersetzt haben.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<IEnumerable<Group>> Groups { get; }

        /// <summary>
        ///     Gibt die ID des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        int Id { get; }

        /// <summary>
        ///     Gibt die Industrie des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<IEnumerable<Industry>> Industry { get; }

        /// <summary>
        ///     Gibt zurück, ob der <see cref="Anime" /> oder <see cref="Manga" /> lizensiert ist.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<bool> IsLicensed { get; }

        /// <summary>
        ///     Gibt den japanischen Titel des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<string> JapaneseTitle { get; }

        /// <summary>
        ///     Gibt den Namen des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<string> Name { get; }

        /// <summary>
        ///     Gibt zurück, ob es sich um einen <see cref="Anime" /> oder <see cref="Manga" /> handelt.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        AnimeMangaType ObjectType { get; }

        /// <summary>
        ///     Gibt die Season des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<IEnumerable<string>> Season { get; }

        /// <summary>
        ///     Gibt den Status des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        InitialisableProperty<AnimeMangaStatus> Status { get; }

        /// <summary>
        ///     Gibt das Synonym des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [NotNull]
        InitialisableProperty<string> Synonym { get; }

        #endregion

        #region

        /// <summary>
        ///     Gibt die Kommentare des <see cref="Anime" /> oder <see cref="Manga" /> chronologisch geordnet zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        /// <param name="startIndex">Der Start-Index der ausgegebenen Kommentare.</param>
        /// <param name="count">Die Anzahl der ausgegebenen Kommentare ab dem angegebenen <paramref name="startIndex" />.</param>
        /// <returns>Eine Aufzählung mit den Kommentaren.</returns>
        [ItemNotNull]
        Task<ProxerResult<IEnumerable<Comment>>> GetCommentsLatest(int startIndex, int count);

        /// <summary>
        ///     Gibt die Kommentare des <see cref="Anime" /> oder <see cref="Manga" />, nach ihrer Beliebtheit sortiert, zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        /// <param name="startIndex">Der Start-Index der ausgegebenen Kommentare.</param>
        /// <param name="count">Die Anzahl der ausgegebenen Kommentare ab dem angegebenen <paramref name="startIndex" />.</param>
        /// <returns>Eine Aufzählung mit den Kommentaren.</returns>
        [ItemNotNull]
        Task<ProxerResult<IEnumerable<Comment>>> GetCommentsRating(int startIndex, int count);

        /// <summary>
        ///     Initialisiert das Objekt.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        [ItemNotNull]
        [Obsolete("Bitte benutze die Methoden der jeweiligen Eigenschaften, um sie zu initalisieren!")]
        Task<ProxerResult> Init();

        #endregion
    }
}