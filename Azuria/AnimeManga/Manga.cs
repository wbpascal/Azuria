using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.Search;
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
                new InitialisableProperty<IEnumerable<Language>>(this.InitAvailableLangApi);
            this.Clicks = new InitialisableProperty<int>(this.InitMainInfoApi);
            this.CommentsLatest = new CommentEnumerable<Manga>(this, "latest");
            this.CommentsRating = new CommentEnumerable<Manga>(this, "rating");
            this.ContentCount = new InitialisableProperty<int>(this.InitMainInfoApi);
            this.Description = new InitialisableProperty<string>(this.InitMainInfoApi);
            this.EnglishTitle = new InitialisableProperty<string>(this.InitNamesApi, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Fsk = new InitialisableProperty<IEnumerable<FskType>>(this.InitMainInfoApi);
            this.Genre = new InitialisableProperty<IEnumerable<GenreType>>(this.InitMainInfoApi);
            this.GermanTitle = new InitialisableProperty<string>(this.InitNamesApi, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Groups = new InitialisableProperty<IEnumerable<Group>>(this.InitGroupsApi);
            this.Industry = new InitialisableProperty<IEnumerable<Industry>>(this.InitIndustryApi);
            this.IsLicensed = new InitialisableProperty<bool>(this.InitMainInfoApi);
            this.IsHContent = new InitialisableProperty<bool>(this.InitIsHContentApi);
            this.JapaneseTitle = new InitialisableProperty<string>(this.InitNamesApi, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.MangaMedium = new InitialisableProperty<MangaMedium>(this.InitMainInfoApi);
            this.Name = new InitialisableProperty<string>(this.InitMainInfoApi, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Rating = new InitialisableProperty<AnimeMangaRating>(this.InitMainInfoApi);
            this.Relations =
                new InitialisableProperty<IEnumerable<IAnimeMangaObject>>(this.InitRelationsApi);
            this.Season = new InitialisableProperty<AnimeMangaSeasonInfo>(this.InitSeasonsApi);
            this.Status = new InitialisableProperty<AnimeMangaStatus>(this.InitMainInfoApi);
            this.Synonym = new InitialisableProperty<string>(this.InitNamesApi, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Tags = new InitialisableProperty<IEnumerable<Tag>>(this.InitEntryTagsApi);
        }

        internal Manga(int id) : this()
        {
            this.Id = id;
        }

        internal Manga([NotNull] string name, int id) : this(id)
        {
            this.Name.SetInitialisedObject(name);
        }

        internal Manga(IEntryInfoDataModel dataModel) : this(dataModel.EntryName, dataModel.EntryId)
        {
            if (dataModel.EntryType != AnimeMangaEntryType.Manga)
                throw new ArgumentException(nameof(dataModel.EntryType));
            this.MangaMedium.SetInitialisedObject((MangaMedium) dataModel.EntryMedium);
        }

        internal Manga(BookmarkDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this.MangaMedium.SetInitialisedObject((MangaMedium) dataModel.EntryMedium);
            this.Status.SetInitialisedObject(dataModel.Status);
        }

        internal Manga(EntryDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this.Clicks.SetInitialisedObject(dataModel.Clicks);
            this.ContentCount.SetInitialisedObject(dataModel.ContentCount);
            this.Description.SetInitialisedObject(dataModel.Description);
            this.Fsk.SetInitialisedObject(dataModel.Fsk);
            this.Genre.SetInitialisedObject(dataModel.Genre);
            this.IsLicensed.SetInitialisedObject(dataModel.IsLicensed);
            this.Rating.SetInitialisedObject(dataModel.Rating);
            this.Status.SetInitialisedObject(dataModel.Status);
        }

        internal Manga(RelationDataModel dataModel) : this((EntryDataModel) dataModel)
        {
            this.AvailableLanguages.SetInitialisedObject(dataModel.AvailableLanguages.Cast<Language>());
        }

        internal Manga(SearchDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this.AvailableLanguages.SetInitialisedObject(dataModel.AvailableLanguages.Cast<Language>());
            this.ContentCount.SetInitialisedObject(dataModel.ContentCount);
            this.Genre.SetInitialisedObject(dataModel.Genre);
            this.Rating.SetInitialisedObject(dataModel.Rating);
            this.Status.SetInitialisedObject(dataModel.Status);
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
        public IEnumerable<Comment<Manga>> CommentsLatest { get; }

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> ordered by rating.
        /// </summary>
        public IEnumerable<Comment<Manga>> CommentsRating { get; }

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
                await this.GetContentObjects();
            if (!lContentObjectsResult.Success || lContentObjectsResult.Result == null)
                return new ProxerResult<IEnumerable<Chapter>>(lContentObjectsResult.Exceptions);

            return new ProxerResult<IEnumerable<Chapter>>(from contentDataModel in lContentObjectsResult.Result
                where (Language) contentDataModel.Language == language
                select new Chapter(this, contentDataModel));
        }

        #endregion

        /// <summary>
        ///     Represents a chapter of a <see cref="Manga" />.
        /// </summary>
        public class Chapter : IAnimeMangaContent<Manga>, IAnimeMangaContent<IAnimeMangaObject>
        {
            internal Chapter(Manga manga, AnimeMangaContentDataModel dataModel)
            {
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (Language) dataModel.Language;
                this.ParentObject = manga;
                this.Title = dataModel.Title;
            }

            internal Chapter(BookmarkDataModel dataModel)
            {
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (Language) dataModel.Language;
                this.ParentObject = new Manga(dataModel);
                //TODO: Chapter: Find a way to get the title if the chapter was fetched from the bookmarks/chronic
                this.Title = string.Empty;
            }

            internal Chapter(HistoryDataModel dataModel)
            {
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (Language) dataModel.Language;
                this.ParentObject = new Manga(dataModel);
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

            IAnimeMangaObject IAnimeMangaContent<IAnimeMangaObject>.ParentObject => this.ParentObject;

            /// <summary>
            ///     Gets the <see cref="Manga" /> this <see cref="Chapter" /> belongs to.
            /// </summary>
            public Manga ParentObject { get; }

            /// <summary>
            ///     Gets the title of the <see cref="Chapter" />.
            /// </summary>
            [NotNull]
            public string Title { get; }

            #endregion

            #region Inherited

            /// <summary>
            ///     Adds the <see cref="Chapter" /> to the bookmarks.
            /// </summary>
            /// <param name="senpai"></param>
            /// <returns>If the action was successful.</returns>
            public Task<ProxerResult> AddToBookmarks(Senpai senpai)
            {
                return new UserControlPanel(senpai).AddToBookmarks((IAnimeMangaContent<Manga>) this);
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