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
    ///     Represents an anime.
    /// </summary>
    [DebuggerDisplay("Anime: {Name} [{Id}]")]
    public class Anime : IAnimeMangaObject
    {
        [UsedImplicitly]
        internal Anime()
        {
            this.AnimeMedium = new InitialisableProperty<AnimeMedium>(this.InitMainInfoApi);
            this.AvailableLanguages =
                new InitialisableProperty<IEnumerable<AnimeLanguage>>(this.InitAvailableLangApi);
            this.Clicks = new InitialisableProperty<int>(this.InitMainInfoApi);
            this.CommentsLatest = new CommentEnumerable<Anime>(this, "latest");
            this.CommentsRating = new CommentEnumerable<Anime>(this, "rating");
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

        internal Anime(int id) : this()
        {
            this.Id = id;
        }

        internal Anime([NotNull] string name, int id) : this(id)
        {
            this.Name.SetInitialisedObject(name);
        }

        internal Anime(IEntryInfoDataModel dataModel) : this(dataModel.EntryName, dataModel.EntryId)
        {
            if (dataModel.EntryType != AnimeMangaEntryType.Anime)
                throw new ArgumentException(nameof(dataModel.EntryType));
            this.AnimeMedium.SetInitialisedObject((AnimeMedium) dataModel.EntryMedium);
        }

        internal Anime(BookmarkDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this.AnimeMedium.SetInitialisedObject((AnimeMedium) dataModel.EntryMedium);
            this.Status.SetInitialisedObject(dataModel.Status);
        }

        internal Anime(EntryDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
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

        internal Anime(RelationDataModel dataModel) : this((EntryDataModel) dataModel)
        {
            this.AvailableLanguages.SetInitialisedObject(dataModel.AvailableLanguages.Cast<AnimeLanguage>());
        }

        internal Anime(SearchDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this.AvailableLanguages.SetInitialisedObject(dataModel.AvailableLanguages.Cast<AnimeLanguage>());
            this.ContentCount.SetInitialisedObject(dataModel.ContentCount);
            this.Genre.SetInitialisedObject(dataModel.Genre);
            this.Rating.SetInitialisedObject(dataModel.Rating);
            this.Status.SetInitialisedObject(dataModel.Status);
        }

        #region Properties

        /// <summary>
        ///     Gets the medium of the Anime.
        /// </summary>
        [NotNull]
        public InitialisableProperty<AnimeMedium> AnimeMedium { get; }

        /// <summary>
        ///     Gets the languages the Anime is available in.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeLanguage>> AvailableLanguages { get; }

        /// <summary>
        ///     Gets the total amount of clicks the anime recieved. Is reset every 3 months.
        /// </summary>
        public InitialisableProperty<int> Clicks { get; }

        /// <summary>
        ///     Gets the comments of the anime in a chronological order.
        /// </summary>
        public IEnumerable<Comment<Anime>> CommentsLatest { get; }

        /// <summary>
        ///     Gets the comments of the anime ordered by rating.
        /// </summary>
        public IEnumerable<Comment<Anime>> CommentsRating { get; }

        /// <summary>
        ///     Gets the count of the episodes the anime contains.
        /// </summary>
        public InitialisableProperty<int> ContentCount { get; }

        /// <summary>
        ///     Gets the link to the cover of the anime.
        /// </summary>
        public Uri CoverUri => new Uri($"https://cdn.proxer.me/cover/{this.Id}.jpg");

        /// <summary>
        ///     Gets the description of the anime.
        /// </summary>
        public InitialisableProperty<string> Description { get; }

        /// <summary>
        ///     Gets the english title of the anime.
        /// </summary>
        public InitialisableProperty<string> EnglishTitle { get; }

        /// <summary>
        ///     Gets an enumeration of the age restrictions of the anime.
        /// </summary>
        public InitialisableProperty<IEnumerable<FskType>> Fsk { get; }

        /// <summary>
        ///     Gets an enumeration of all the genre of the anime contains.
        /// </summary>
        public InitialisableProperty<IEnumerable<GenreType>> Genre { get; }

        /// <summary>
        ///     Gets the german title of the anime.
        /// </summary>
        public InitialisableProperty<string> GermanTitle { get; }

        /// <summary>
        ///     Gets an enumeration of all the groups that translated the anime.
        /// </summary>
        public InitialisableProperty<IEnumerable<Group>> Groups { get; }

        /// <summary>
        ///     Gets the Id of the anime.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets an enumeration of all the companies that were involved in making the anime.
        /// </summary>
        public InitialisableProperty<IEnumerable<Industry>> Industry { get; }

        /// <summary>
        ///     Gets whether the anime contains H-Content (Adult-Content).
        /// </summary>
        public InitialisableProperty<bool> IsHContent { get; }

        /// <summary>
        ///     Gets if the anime is licensed by a german company.
        /// </summary>
        public InitialisableProperty<bool> IsLicensed { get; }

        /// <summary>
        ///     Gets the japanese title of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<string> JapaneseTitle { get; }

        /// <summary>
        ///     Gets the original title of the anime.
        /// </summary>
        public InitialisableProperty<string> Name { get; }

        /// <summary>
        ///     Gets the rating of the anime.
        /// </summary>
        public InitialisableProperty<AnimeMangaRating> Rating { get; }

        /// <summary>
        ///     Gets the relations of the anime.
        /// </summary>
        public InitialisableProperty<IEnumerable<IAnimeMangaObject>> Relations { get; }

        /// <summary>
        ///     Gets the seasons the anime aired in.
        /// </summary>
        public InitialisableProperty<AnimeMangaSeasonInfo> Season { get; }

        /// <summary>
        ///     Gets the status of the anime.
        /// </summary>
        public InitialisableProperty<AnimeMangaStatus> Status { get; }

        /// <summary>
        ///     Gets the synonym the anime is also known as.
        /// </summary>
        public InitialisableProperty<string> Synonym { get; }

        /// <summary>
        ///     Gets the tags the anime was tagged with.
        /// </summary>
        public InitialisableProperty<IEnumerable<Tag>> Tags { get; }

        #endregion

        #region Inherited

        /// <summary>
        ///     Adds the anime to the planned list.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
        /// <returns>If the action was successful.</returns>
        async Task<ProxerResult> IAnimeMangaObject.AddToPlanned(UserControlPanel userControlPanel)
        {
            return await this.AddToPlanned(userControlPanel);
        }

        #endregion

        #region

        /// <summary>
        ///     Adds the anime to the planned list.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
        /// <returns>If the action was successful.</returns>
        public Task<ProxerResult> AddToPlanned(UserControlPanel userControlPanel)
        {
            //TODO: Implement Anime.AddToPlanned
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Returns all epsiode of the anime in a specified language.
        /// </summary>
        /// <param name="language">The language of the episodes.</param>
        /// <returns>
        ///     An enumeration of all available episodes in the specified
        ///     <paramref name="language">language</paramref> with a max count of <see cref="ContentCount" />.
        /// </returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<Episode>>> GetEpisodes(AnimeLanguage language)
        {
            if (!(await this.AvailableLanguages.GetObject(new AnimeLanguage[0])).Contains(language))
                return new ProxerResult<IEnumerable<Episode>>(new Exception[] {new LanguageNotAvailableException()});

            ProxerResult<AnimeMangaContentDataModel[]> lContentObjectsResult =
                await this.GetContentObjects();
            if (!lContentObjectsResult.Success || lContentObjectsResult.Result == null)
                return new ProxerResult<IEnumerable<Episode>>(lContentObjectsResult.Exceptions);

            return new ProxerResult<IEnumerable<Episode>>(from contentDataModel in lContentObjectsResult.Result
                where (AnimeLanguage) contentDataModel.Language == language
                select new Episode(this, contentDataModel));
        }

        #endregion

        /// <summary>
        ///     Represents an episode of an anime.
        /// </summary>
        public class Episode : IAnimeMangaContent<Anime>, IAnimeMangaContent<IAnimeMangaObject>
        {
            internal Episode([NotNull] Anime anime, AnimeMangaContentDataModel dataModel)
            {
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (AnimeLanguage) dataModel.Language;
                this.ParentObject = anime;
            }

            internal Episode([NotNull] BookmarkDataModel dataModel)
            {
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (AnimeLanguage) dataModel.Language;
                this.ParentObject = new Anime(dataModel);
            }

            internal Episode([NotNull] HistoryDataModel dataModel)
            {
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (AnimeLanguage) dataModel.Language;
                this.ParentObject = new Anime(dataModel);
            }

            #region Properties

            /// <summary>
            ///     Gets the episode number.
            /// </summary>
            public int ContentIndex { get; }

            /// <summary>
            ///     Gets the general language (english/german) of the episode.
            /// </summary>
            public Language GeneralLanguage
                =>
                    this.Language == AnimeLanguage.GerSub || this.Language == AnimeLanguage.GerDub
                        ? Properties.Language.German
                        : this.Language == AnimeLanguage.EngSub || this.Language == AnimeLanguage.EngDub
                            ? Properties.Language.English
                            : Properties.Language.Unkown;

            /// <summary>
            ///     Gets the language of the episode
            /// </summary>
            public AnimeLanguage Language { get; }

            /// <summary>
            ///     Gets the anime or manga this object belongs to.
            /// </summary>
            IAnimeMangaObject IAnimeMangaContent<IAnimeMangaObject>.ParentObject => this.ParentObject;

            /// <summary>
            ///     Gets the anime this episode> belongs to.
            /// </summary>
            public Anime ParentObject { get; }

            /// <summary>
            ///     Gets the available streams of the episode.
            ///     TODO: Implement Streams
            /// </summary>
            [NotNull]
            public IEnumerable<Stream> Streams { get; }

            #endregion

            #region Inherited

            /// <summary>
            ///     Adds the episode to the bookmarks.
            /// </summary>
            /// <param name="senpai"></param>
            /// <returns>If the action was successful.</returns>
            public Task<ProxerResult> AddToBookmarks(Senpai senpai)
            {
                return new UserControlPanel(senpai).AddToBookmarks((IAnimeMangaContent<Anime>) this);
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
                return "Episode " + this.ContentIndex;
            }

            #endregion

            /// <summary>
            ///     Represents a stream of an episode.
            /// </summary>
            public class Stream
            {
                internal Stream(StreamPartner streamPartner)
                {
                    this.Partner = streamPartner;
                }

                #region Properties

                /// <summary>
                ///     Gets the link of the stream. Currently not implemented.
                /// </summary>
                [NotNull]
                public Uri Link { get; }

                /// <summary>
                ///     Gets the streampartner of the stream.
                /// </summary>
                public StreamPartner Partner { get; }

                #endregion
            }
        }
    }
}