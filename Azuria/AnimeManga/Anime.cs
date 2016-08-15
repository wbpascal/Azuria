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
    ///     Represents an anime.
    /// </summary>
    [DebuggerDisplay("Anime: {Name} [{Id}]")]
    public class Anime : IAnimeMangaObject
    {
        [UsedImplicitly]
        internal Anime()
        {
            this.AnimeTyp = new InitialisableProperty<AnimeType>(() => this.InitMainInfoApi(this.Senpai));
            this.AvailableLanguages =
                new InitialisableProperty<IEnumerable<AnimeLanguage>>(() => this.InitAvailableLangApi(this.Senpai));
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

        internal Anime(int id, [NotNull] Senpai senpai) : this()
        {
            this.Id = id;
            this.Senpai = senpai;
        }

        internal Anime([NotNull] string name, int id, [NotNull] Senpai senpai) : this(id, senpai)
        {
            this.Name.SetInitialisedObject(name);
        }

        internal Anime([NotNull] string name, int id, [NotNull] Senpai senpai,
            [NotNull] IEnumerable<GenreType> genreList, AnimeMangaStatus status,
            AnimeType type) : this(name, id, senpai)
        {
            this.Genre.SetInitialisedObject(genreList);
            this.Status.SetInitialisedObject(status);
            this.AnimeTyp.SetInitialisedObject(type);
        }

        internal Anime(BookmarkDataModel dataModel, Senpai senpai) : this()
        {
            this.Senpai = senpai;
            this.Id = dataModel.EntryId;
            this.AnimeTyp.SetInitialisedObject((AnimeType) dataModel.Medium);
            this.Name.SetInitialisedObject(dataModel.Name);
            this.Status.SetInitialisedObject(dataModel.Status);
        }

        internal Anime(EntryDataModel entryDataModel, Senpai senpai) : this(entryDataModel.EntryId, senpai)
        {
            if (entryDataModel.EntryType != AnimeMangaEntryType.Anime)
                throw new ArgumentException(nameof(entryDataModel.EntryType));

            this.AnimeTyp.SetInitialisedObject((AnimeType) entryDataModel.Medium);
            this.Clicks.SetInitialisedObject(entryDataModel.Clicks);
            this.ContentCount.SetInitialisedObject(entryDataModel.ContentCount);
            this.Description.SetInitialisedObject(entryDataModel.Description);
            this.Fsk.SetInitialisedObject(entryDataModel.Fsk);
            this.Genre.SetInitialisedObject(entryDataModel.Genre);
            this.IsLicensed.SetInitialisedObject(entryDataModel.IsLicensed);
            this.Name.SetInitialisedObject(entryDataModel.Name);
            this.Rating.SetInitialisedObject(entryDataModel.Rating);
            this.Status.SetInitialisedObject(entryDataModel.Status);
        }

        internal Anime(RelationDataModel dataModel, Senpai senpai) : this((EntryDataModel) dataModel, senpai)
        {
            this.AvailableLanguages.SetInitialisedObject(dataModel.AvailableLanguages.Cast<AnimeLanguage>());
        }

        #region Properties

        /// <summary>
        ///     Gets the type of the <see cref="Anime" />.
        /// </summary>
        [NotNull]
        public InitialisableProperty<AnimeType> AnimeTyp { get; }

        /// <summary>
        ///     Gets the languages the <see cref="Anime" /> is available in.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeLanguage>> AvailableLanguages { get; }

        /// <summary>
        ///     Gets the total amount of clicks the <see cref="Anime" /> recieved. Is reset every 3 months.
        /// </summary>
        public InitialisableProperty<int> Clicks { get; }

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> in a chronological order.
        /// </summary>
        public IEnumerable<Comment<Anime>> CommentsLatest => new CommentEnumerable<Anime>(this, "latest", this.Senpai);

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> ordered by rating.
        /// </summary>
        public IEnumerable<Comment<Anime>> CommentsRating => new CommentEnumerable<Anime>(this, "rating", this.Senpai);

        /// <summary>
        ///     Gets the count of the <see cref="Episode">Episodes</see> the <see cref="Anime" /> contains.
        /// </summary>
        public InitialisableProperty<int> ContentCount { get; }

        /// <summary>
        ///     Gets the link to the cover of the <see cref="Anime" />.
        /// </summary>
        public Uri CoverUri => new Uri($"https://cdn.proxer.me/cover/{this.Id}.jpg");

        /// <summary>
        ///     Gets the description of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<string> Description { get; }

        /// <summary>
        ///     Gets the english title of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<string> EnglishTitle { get; }

        /// <summary>
        ///     Gets an enumeration of the age restrictions of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<FskType>> Fsk { get; }

        /// <summary>
        ///     Gets an enumeration of all the genre of the <see cref="Anime" /> contains.
        /// </summary>
        public InitialisableProperty<IEnumerable<GenreType>> Genre { get; }

        /// <summary>
        ///     Gets the german title of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<string> GermanTitle { get; }

        /// <summary>
        ///     Gets an enumeration of all the groups that translated the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<Group>> Groups { get; }

        /// <summary>
        ///     Gets the Id of the <see cref="Anime" />.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets an enumeration of all the companies that were involved in making the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<Industry>> Industry { get; }

        /// <summary>
        ///     Gets whether the <see cref="Anime" /> contains H-Content (Adult).
        /// </summary>
        public InitialisableProperty<bool> IsHContent { get; }

        /// <summary>
        ///     Gets if the <see cref="Anime" /> is licensed by a german company.
        /// </summary>
        public InitialisableProperty<bool> IsLicensed { get; }

        /// <summary>
        ///     Gets the japanese title of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<string> JapaneseTitle { get; }

        /// <summary>
        ///     Gets the original title of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<string> Name { get; }

        /// <summary>
        ///     Gets the rating of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<AnimeMangaRating> Rating { get; }

        /// <summary>
        ///     Gets the relations of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<IAnimeMangaObject>> Relations { get; }

        /// <summary>
        ///     Gets the seasons the <see cref="Anime" /> aired in.
        /// </summary>
        public InitialisableProperty<AnimeMangaSeasonInfo> Season { get; }

        /// <summary>
        /// </summary>
        public Senpai Senpai { get; set; }

        /// <summary>
        ///     Gets the status of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<AnimeMangaStatus> Status { get; }

        /// <summary>
        ///     Gets the synonym the <see cref="Anime" /> is also known as.
        /// </summary>
        public InitialisableProperty<string> Synonym { get; }

        /// <summary>
        ///     Gets the tags the <see cref="Anime" /> was tagged with.
        /// </summary>
        public InitialisableProperty<IEnumerable<Tag>> Tags { get; }

        #endregion

        #region Inherited

        /// <summary>
        ///     Adds the <see cref="Anime" /> to the planned list.
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
        ///     Adds the <see cref="Anime" /> to the planned list.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
        /// <returns>If the action was successful.</returns>
        public async Task<ProxerResult> AddToPlanned(
            UserControlPanel userControlPanel = null)
        {
            //TODO: Implement Anime.AddToPlanned
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Returns all <see cref="Episode">epsiodes</see> of the <see cref="Anime" /> in a specified language.
        /// </summary>
        /// <param name="language">The language of the episodes.</param>
        /// <seealso cref="Episode" />
        /// <returns>
        ///     An enumeration of all available <see cref="Episode">episodes</see> in the specified
        ///     <paramref name="language">language</paramref> with a max count of <see cref="ContentCount" />.
        /// </returns>
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<Episode>>> GetEpisodes(AnimeLanguage language)
        {
            if (!(await this.AvailableLanguages.GetObject(new AnimeLanguage[0])).Contains(language))
                return new ProxerResult<IEnumerable<Episode>>(new Exception[] {new LanguageNotAvailableException()});

            ProxerResult<AnimeMangaContentDataModel[]> lContentObjectsResult =
                await this.GetContentObjects(this.Senpai);
            if (!lContentObjectsResult.Success || lContentObjectsResult.Result == null)
                return new ProxerResult<IEnumerable<Episode>>(lContentObjectsResult.Exceptions);

            return new ProxerResult<IEnumerable<Episode>>(from contentDataModel in lContentObjectsResult.Result
                where (AnimeLanguage) contentDataModel.Language == language
                select new Episode(this, contentDataModel, this.Senpai));
        }

        #endregion

        /// <summary>
        ///     Represents an episode of an <see cref="Anime" />.
        /// </summary>
        public class Episode : IAnimeMangaContent<Anime>
        {
            internal Episode([NotNull] Anime anime, AnimeMangaContentDataModel dataModel, Senpai senpai)
            {
                this.Senpai = senpai;
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (AnimeLanguage) dataModel.Language;
                this.ParentObject = anime;
            }

            internal Episode([NotNull] BookmarkDataModel dataModel, Senpai senpai)
            {
                this.Senpai = senpai;
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (AnimeLanguage) dataModel.Language;
                this.ParentObject = new Anime(dataModel, senpai);
            }

            #region Properties

            /// <summary>
            ///     Gets the <see cref="Episode" />-number.
            /// </summary>
            public int ContentIndex { get; }

            /// <summary>
            ///     Gets the general language (english/german) of the <see cref="Episode" />.
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
            ///     Gets the <see cref="Anime" /> this <see cref="Episode" /> belongs to.
            /// </summary>
            public Anime ParentObject { get; }

            /// <summary>
            /// </summary>
            public Senpai Senpai { get; set; }

            /// <summary>
            ///     Gets the available streams of the episode.
            ///     TODO: Implement Streams
            /// </summary>
            [NotNull]
            public IEnumerable<Stream> Streams { get; }

            #endregion

            #region Inherited

            /// <summary>
            ///     Adds the <see cref="Episode" /> to the bookmarks.
            /// </summary>
            /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
            /// <returns>If the action was successful.</returns>
            public async Task<ProxerResult<AnimeMangaBookmarkObject<Anime>>> AddToBookmarks(
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
                return "Episode " + this.ContentIndex;
            }

            #endregion

            /// <summary>
            ///     Represents a stream of an <see cref="Episode" /> of an <see cref="Anime" />.
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