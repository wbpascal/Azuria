using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.Enums;
using Azuria.Exceptions;
using Azuria.User.Comment;
using Azuria.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Properties;
using JetBrains.Annotations;

// ReSharper disable LoopCanBeConvertedToQuery

namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents a manga.
    /// </summary>
    [DebuggerDisplay("Manga: {Name} [{Id}]")]
    public class Manga : IAnimeMangaObject
    {
        [UsedImplicitly]
        internal Manga()
        {
            this.AvailableLanguages =
                new InitialisableProperty<IEnumerable<Language>>(() => this.InitAvailableLangApi(this.Senpai));
            this.Clicks = new InitialisableProperty<int>(() => this.InitMainInfoApi(this.Senpai));
            this.ContentCount = new InitialisableProperty<int>(() => this.InitMainInfoApi(this.Senpai));
            this.Description = new InitialisableProperty<string>(() => this.InitMainInfoApi(this.Senpai));
            this.EnglishTitle = new InitialisableProperty<string>(() => this.InitNamesApi(this.Senpai), string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Fsk = new InitialisableProperty<IEnumerable<FskType>>(() => this.InitMainInfoApi(this.Senpai));
            this.Genre = new InitialisableProperty<IEnumerable<GenreType>>(() => this.InitMainInfoApi(this.Senpai));
            this.GermanTitle = new InitialisableProperty<string>(() => this.InitNamesApi(this.Senpai), string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Groups = new InitialisableProperty<IEnumerable<Group>>(() => this.InitGroupsApi(this.Senpai));
            this.Industry = new InitialisableProperty<IEnumerable<Industry>>(() => this.InitIndustryApi(this.Senpai));
            this.IsLicensed = new InitialisableProperty<bool>(() => this.InitMainInfoApi(this.Senpai));
            this.IsHContent = new InitialisableProperty<bool>(() => this.InitIsHContentApi(this.Senpai));
            this.JapaneseTitle = new InitialisableProperty<string>(() => this.InitNamesApi(this.Senpai), string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.MangaMedium = new InitialisableProperty<MangaMedium>(() => this.InitMainInfoApi(this.Senpai));
            this.Name = new InitialisableProperty<string>(() => this.InitMainInfoApi(this.Senpai), string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Rating = new InitialisableProperty<AnimeMangaRating>(() => this.InitMainInfoApi(this.Senpai));
            this.Relations =
                new InitialisableProperty<IEnumerable<IAnimeMangaObject>>(() => this.InitRelationsApi(this.Senpai));
            this.Season = new InitialisableProperty<AnimeMangaSeasonInfo>(() => this.InitSeasonsApi(this.Senpai));
            this.Status = new InitialisableProperty<AnimeMangaStatus>(() => this.InitMainInfoApi(this.Senpai));
            this.Synonym = new InitialisableProperty<string>(() => this.InitNamesApi(this.Senpai), string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Tags = new InitialisableProperty<IEnumerable<Tag>>(() => this.InitEntryTagsApi(this.Senpai));
        }

        internal Manga(int id, [NotNull] Senpai senpai) : this()
        {
            this.Id = id;
            this.Senpai = senpai;
        }

        internal Manga([NotNull] string name, int id, [NotNull] Senpai senpai) : this(id, senpai)
        {
            this.Name.SetInitialisedObject(name);
        }

        internal Manga(BookmarkDataModel dataModel, Senpai senpai) : this()
        {
            this.Senpai = senpai;
            this.Id = dataModel.EntryId;
            this.MangaMedium.SetInitialisedObject((MangaMedium) dataModel.Medium);
            this.Name.SetInitialisedObject(dataModel.Name);
            this.Status.SetInitialisedObject(dataModel.Status);
        }

        internal Manga([NotNull] string name, int id, [NotNull] Senpai senpai,
            [NotNull] IEnumerable<GenreType> genreList, AnimeMangaStatus status,
            MangaMedium medium) : this(name, id, senpai)
        {
            this.Genre.SetInitialisedObject(genreList);
            this.MangaMedium.SetInitialisedObject(medium);
            this.Status.SetInitialisedObject(status);
        }

        internal Manga(EntryDataModel entryDataModel, Senpai senpai) : this(entryDataModel.EntryId, senpai)
        {
            if (entryDataModel.EntryType != AnimeMangaEntryType.Manga)
                throw new ArgumentException(nameof(entryDataModel.EntryType));

            this.Clicks.SetInitialisedObject(entryDataModel.Clicks);
            this.ContentCount.SetInitialisedObject(entryDataModel.ContentCount);
            this.Description.SetInitialisedObject(entryDataModel.Description);
            this.Fsk.SetInitialisedObject(entryDataModel.Fsk);
            this.Genre.SetInitialisedObject(entryDataModel.Genre);
            this.IsLicensed.SetInitialisedObject(entryDataModel.IsLicensed);
            this.MangaMedium.SetInitialisedObject((MangaMedium) entryDataModel.Medium);
            this.Name.SetInitialisedObject(entryDataModel.Name);
            this.Rating.SetInitialisedObject(entryDataModel.Rating);
            this.Status.SetInitialisedObject(entryDataModel.Status);
        }

        internal Manga(RelationDataModel dataModel, Senpai senpai) : this((EntryDataModel) dataModel, senpai)
        {
            this.AvailableLanguages.SetInitialisedObject(dataModel.AvailableLanguages.Cast<Language>());
        }

        internal Manga(HistoryDataModel dataModel, Senpai senpai) : this()
        {
            this.Senpai = senpai;
            this.Id = dataModel.EntryId;
            this.MangaMedium.SetInitialisedObject((MangaMedium) dataModel.Medium);
            this.Name.SetInitialisedObject(dataModel.Name);
        }

        #region Properties

        /// <summary>
        ///     Gets the languages the <see cref="Manga" /> is available in.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<Language>> AvailableLanguages { get; }

        /// <summary>
        ///     Gets the total amount of clicks the <see cref="Manga" /> recieved. Is reset every 3 months.
        /// </summary>
        public InitialisableProperty<int> Clicks { get; }

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> in a chronological order.
        /// </summary>
        public IEnumerable<Comment<Manga>> CommentsLatest => new CommentEnumerable<Manga>(this, "latest", this.Senpai);

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> ordered by rating.
        /// </summary>
        public IEnumerable<Comment<Manga>> CommentsRating => new CommentEnumerable<Manga>(this, "rating", this.Senpai);

        /// <summary>
        ///     Gets the count of the <see cref="Chapter">Chapters</see> the <see cref="Manga" /> contains.
        /// </summary>
        public InitialisableProperty<int> ContentCount { get; }

        /// <summary>
        ///     Gets the link to the cover of the <see cref="Manga" />.
        /// </summary>
        public Uri CoverUri => new Uri($"https://cdn.proxer.me/cover/{this.Id}.jpg");

        /// <summary>
        ///     Gets the description of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> Description { get; }

        /// <summary>
        ///     Gets the english title of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> EnglishTitle { get; }

        /// <summary>
        ///     Gets an enumeration of the age restrictions of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<FskType>> Fsk { get; }

        /// <summary>
        ///     Gets an enumeration of all the genre of the <see cref="Manga" /> contains.
        /// </summary>
        public InitialisableProperty<IEnumerable<GenreType>> Genre { get; }

        /// <summary>
        ///     Gets the german title of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> GermanTitle { get; }

        /// <summary>
        ///     Gets an enumeration of all the groups that translated the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<Group>> Groups { get; }

        /// <summary>
        ///     Gets the Id of the <see cref="Manga" />.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets an enumeration of all the companies that were involved in making the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<Industry>> Industry { get; }

        /// <summary>
        ///     Gets whether the <see cref="Manga" /> contains H-Content (Adult).
        /// </summary>
        public InitialisableProperty<bool> IsHContent { get; }

        /// <summary>
        ///     Gets if the <see cref="Manga" /> is licensed by a german company.
        /// </summary>
        public InitialisableProperty<bool> IsLicensed { get; }

        /// <summary>
        ///     Gets the japanese title of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> JapaneseTitle { get; }

        /// <summary>
        ///     Gets the medium of the <see cref="Manga" />.
        /// </summary>
        [NotNull]
        public InitialisableProperty<MangaMedium> MangaMedium { get; }

        /// <summary>
        ///     Gets the original title of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> Name { get; }

        /// <summary>
        ///     Gets the rating of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<AnimeMangaRating> Rating { get; }

        /// <summary>
        ///     Gets the relations of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<IAnimeMangaObject>> Relations { get; }

        /// <summary>
        ///     Gets the seasons the <see cref="Manga" /> aired in. If the enumerable only contains one
        ///     value the value is always the start season of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<AnimeMangaSeasonInfo> Season { get; }

        /// <summary>
        /// </summary>
        public Senpai Senpai { get; set; }

        /// <summary>
        ///     Gets the status of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<AnimeMangaStatus> Status { get; }

        /// <summary>
        ///     Gets the synonym the <see cref="Manga" /> is also known as.
        /// </summary>
        public InitialisableProperty<string> Synonym { get; }

        /// <summary>
        ///     Gets the tags the <see cref="Manga" /> was tagged with.
        /// </summary>
        public InitialisableProperty<IEnumerable<Tag>> Tags { get; }

        #endregion

        #region Inherited

        /// <summary>
        ///     Adds the <see cref="Manga" /> to the planned list. If <paramref name="userControlPanel" />
        ///     is specified the object is also added to the corresponding <see cref="Manga" />-enumeration.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, the object is added to.</param>
        /// <returns>If the action was successful.</returns>
        async Task<ProxerResult> IAnimeMangaObject.AddToPlanned(UserControlPanel userControlPanel)
        {
            return await this.AddToPlanned(userControlPanel);
        }

        #endregion

        #region

        /// <summary>
        ///     Adds the <see cref="Manga" /> to the planned list. If <paramref name="userControlPanel" />
        ///     is specified the object is also added to the corresponding <see cref="Manga" />-enumeration.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, the object is added to.</param>
        /// <returns>If the action was successful and if was, the new entry.</returns>
        public Task<ProxerResult> AddToPlanned(UserControlPanel userControlPanel = null)
        {
            //TODO: Implement Manga.AddToPlanned
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Returns all <see cref="Chapter">epsiodes</see> of the <see cref="Manga" /> in a specified language.
        /// </summary>
        /// <param name="language">The language of the episodes.</param>
        /// <seealso cref="Chapter" />
        /// <returns>
        ///     An enumeration of all available <see cref="Chapter">chapters</see> in the specified
        ///     <paramref name="language">language</paramref> with a max count of <see cref="ContentCount" />.
        /// </returns>
        [NotNull]
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<Chapter>>> GetChapters(Language language)
        {
            if (!(await this.AvailableLanguages.GetObject(new Language[0])).Contains(language))
                return new ProxerResult<IEnumerable<Chapter>>(new Exception[] {new LanguageNotAvailableException()});

            ProxerResult<AnimeMangaContentDataModel[]> lContentObjectsResult =
                await this.GetContentObjects(this.Senpai);
            if (!lContentObjectsResult.Success || lContentObjectsResult.Result == null)
                return new ProxerResult<IEnumerable<Chapter>>(lContentObjectsResult.Exceptions);

            return new ProxerResult<IEnumerable<Chapter>>(from contentDataModel in lContentObjectsResult.Result
                where (Language) contentDataModel.Language == language
                select new Chapter(this, contentDataModel, this.Senpai));
        }

        #endregion

        /// <summary>
        ///     Represents a chapter of a <see cref="Manga" />.
        /// </summary>
        public class Chapter : IAnimeMangaContent<Manga>, IAnimeMangaContent<IAnimeMangaObject>
        {
            internal Chapter(Manga manga, AnimeMangaContentDataModel dataModel, Senpai senpai)
            {
                this.Senpai = senpai;
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (Language) dataModel.Language;
                this.ParentObject = manga;
                this.Title = dataModel.Title;
            }

            internal Chapter(BookmarkDataModel dataModel, Senpai senpai)
            {
                this.Senpai = senpai;
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (Language) dataModel.Language;
                this.ParentObject = new Manga(dataModel, senpai);
                //TODO: Chapter: Find a way to get the title if the chapter was fetched from the bookmarks/chronic
                this.Title = string.Empty;
            }

            internal Chapter(HistoryDataModel dataModel, Senpai senpai)
            {
                this.Senpai = senpai;
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (Language) dataModel.Language;
                this.ParentObject = new Manga(dataModel, senpai);
                this.Title = string.Empty;
            }

            #region Properties

            /// <summary>
            ///     Gets the <see cref="Chapter" />-number.
            /// </summary>
            public int ContentIndex { get; }

            /// <summary>
            ///     Gets the general language (english/german) of the <see cref="Chapter" />.
            /// </summary>
            public Language GeneralLanguage => this.Language;

            /// <summary>
            ///     Gets the language of the <see cref="Chapter" />.
            /// </summary>
            public Language Language { get; }

            /// <summary>
            ///     Gets the <see cref="Anime" /> or <see cref="Manga" /> this <see cref="Anime.Episode" /> or
            ///     <see cref="Manga.Chapter" /> belongs to.
            /// </summary>
            IAnimeMangaObject IAnimeMangaContent<IAnimeMangaObject>.ParentObject => this.ParentObject;

            /// <summary>
            ///     Gets the <see cref="Manga" /> this <see cref="Chapter" /> belongs to.
            /// </summary>
            public Manga ParentObject { get; }

            /// <summary>
            /// </summary>
            public Senpai Senpai { get; set; }

            /// <summary>
            ///     Gets the title of the <see cref="Chapter" />.
            /// </summary>
            [NotNull]
            public string Title { get; }

            #endregion

            #region Inherited

            /// <summary>
            ///     Adds the <see cref="Anime.Episode" /> or <see cref="Manga.Chapter" /> to the bookmarks. If
            ///     <paramref name="userControlPanel" /> is specified the object is also added to the corresponding
            ///     <see cref="UserControlPanel.AnimeBookmarks" />- or <see cref="UserControlPanel.MangaBookmarks" />-enumeration.
            /// </summary>
            /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
            /// <returns>If the action was successful.</returns>
            Task<ProxerResult<AnimeMangaBookmarkObject<IAnimeMangaObject>>> IAnimeMangaContent<IAnimeMangaObject>.
                AddToBookmarks(UserControlPanel userControlPanel)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///     Adds the <see cref="Chapter" /> to the bookmarks. If <paramref name="userControlPanel" /> is specified
            ///     the object is also added to the corresponding <see cref="UserControlPanel.MangaBookmarks" />-enumeration.
            /// </summary>
            /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
            /// <returns>If the action was successful.</returns>
            public Task<ProxerResult<AnimeMangaBookmarkObject<Manga>>> AddToBookmarks(
                UserControlPanel userControlPanel = null)
            {
                //TODO: Implement Episode.AddToBookmarks
                throw new NotImplementedException();
            }

            #endregion

            #region

            /// <summary>
            ///     Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            ///     A string that represents the current object.
            /// </returns>
            public override string ToString()
            {
                return "Chapter " + this.ContentIndex;
            }

            #endregion
        }
    }
}