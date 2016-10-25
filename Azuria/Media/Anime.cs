using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Anime;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.Search;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Media.Properties;
using Azuria.UserInfo;
using Azuria.UserInfo.Comment;
using Azuria.UserInfo.ControlPanel;
using Azuria.Utilities.Properties;

// ReSharper disable LoopCanBeConvertedToQuery

namespace Azuria.Media
{
    /// <summary>
    /// Represents an anime.
    /// </summary>
    [DebuggerDisplay("Anime: {Name} [{Id}]")]
    public class Anime : MediaObject
    {
        private readonly InitialisableProperty<AnimeMedium> _animeMedium;

        private readonly InitialisableProperty<IEnumerable<AnimeLanguage>> _availableLanguages;

        internal Anime(int id) : base(id)
        {
            this._animeMedium = new InitialisableProperty<AnimeMedium>(this.InitMainInfo);
            this._availableLanguages =
                new InitialisableProperty<IEnumerable<AnimeLanguage>>(this.InitAvailableLanguages);
            this.CommentsLatest = new CommentEnumerable<Anime>(this, "latest");
            this.CommentsRating = new CommentEnumerable<Anime>(this, "rating");
        }

        internal Anime(string name, int id) : this(id)
        {
            this._name.SetInitialisedObject(name);
        }

        internal Anime(IEntryInfoDataModel dataModel) : this(dataModel.EntryName, dataModel.EntryId)
        {
            if (dataModel.EntryType != MediaEntryType.Anime)
                throw new ArgumentException(nameof(dataModel.EntryType));
            this._animeMedium.SetInitialisedObject((AnimeMedium) dataModel.EntryMedium);
        }

        private Anime(BookmarkDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._animeMedium.SetInitialisedObject((AnimeMedium) dataModel.EntryMedium);
            this._status.SetInitialisedObject(dataModel.Status);
        }

        internal Anime(EntryDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._clicks.SetInitialisedObject(dataModel.Clicks);
            this._contentCount.SetInitialisedObject(dataModel.ContentCount);
            this._description.SetInitialisedObject(dataModel.Description);
            this._fsk.SetInitialisedObject(dataModel.Fsk);
            this._genre.SetInitialisedObject(dataModel.Genre);
            this._isLicensed.SetInitialisedObject(dataModel.IsLicensed);
            this._rating.SetInitialisedObject(dataModel.Rating);
            this._status.SetInitialisedObject(dataModel.Status);
        }

        internal Anime(RelationDataModel dataModel) : this((EntryDataModel) dataModel)
        {
            this._availableLanguages.SetInitialisedObject(dataModel.AvailableLanguages.Cast<AnimeLanguage>());
        }

        internal Anime(SearchDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._availableLanguages.SetInitialisedObject(dataModel.AvailableLanguages.Cast<AnimeLanguage>());
            this._contentCount.SetInitialisedObject(dataModel.ContentCount);
            this._genre.SetInitialisedObject(dataModel.Genre);
            this._rating.SetInitialisedObject(dataModel.Rating);
            this._status.SetInitialisedObject(dataModel.Status);
        }

        #region Properties

        /// <summary>
        /// Gets the medium of the Anime.
        /// </summary>
        public IInitialisableProperty<AnimeMedium> AnimeMedium => this._animeMedium;

        /// <summary>
        /// Gets the languages the Anime is available in.
        /// </summary>
        public IInitialisableProperty<IEnumerable<AnimeLanguage>> AvailableLanguages => this._availableLanguages;

        /// <summary>
        /// Gets the comments of the anime in a chronological order.
        /// </summary>
        public IEnumerable<Comment<Anime>> CommentsLatest { get; }

        /// <summary>
        /// Gets the comments of the anime ordered by rating.
        /// </summary>
        public IEnumerable<Comment<Anime>> CommentsRating { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns all epsiode of the anime in a specified language.
        /// </summary>
        /// <param name="language">The language of the episodes.</param>
        /// <returns>
        /// An enumeration of all available episodes in the specified
        /// <paramref name="language">language</paramref> with a max count of <see cref="MediaObject.ContentCount" />.
        /// </returns>
        public async Task<ProxerResult<IEnumerable<Episode>>> GetEpisodes(AnimeLanguage language)
        {
            if (!(await this.AvailableLanguages.GetObject(new AnimeLanguage[0])).Contains(language))
                return new ProxerResult<IEnumerable<Episode>>(new LanguageNotAvailableException());

            ProxerResult<MediaContentDataModel[]> lContentObjectsResult =
                await this.GetContentObjects();
            if (!lContentObjectsResult.Success || (lContentObjectsResult.Result == null))
                return new ProxerResult<IEnumerable<Episode>>(lContentObjectsResult.Exceptions);

            return new ProxerResult<IEnumerable<Episode>>(from contentDataModel in lContentObjectsResult.Result
                where (AnimeLanguage) contentDataModel.Language == language
                select new Episode(this, contentDataModel));
        }

        private async Task<ProxerResult> InitAvailableLanguages()
        {
            ProxerResult<ProxerInfoLanguageResponse> lResult =
                await
                    RequestHandler.ApiCustomRequest<ProxerInfoLanguageResponse>(
                        ApiRequestBuilder.InfoGetLanguage(this.Id));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);
            this._availableLanguages.SetInitialisedObject(lResult.Result.Data.Cast<AnimeLanguage>());
            return new ProxerResult();
        }

        internal void InitMainInfoAnime(EntryDataModel dataModel)
        {
            this._animeMedium.SetInitialisedObject((AnimeMedium) dataModel.EntryMedium);
        }

        #endregion

        /// <summary>
        /// Represents an episode of an anime.
        /// </summary>
        public class Episode : IMediaContent<Anime>, IMediaContent<IMediaObject>
        {
            private readonly InitialisableProperty<IEnumerable<Stream>> _streams;

            private Episode()
            {
                this._streams = new InitialisableProperty<IEnumerable<Stream>>(this.InitStreams);
            }

            internal Episode(Anime anime, MediaContentDataModel dataModel) : this()
            {
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (AnimeLanguage) dataModel.Language;
                this.ParentObject = anime;
            }

            internal Episode(BookmarkDataModel dataModel) : this()
            {
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (AnimeLanguage) dataModel.Language;
                this.ParentObject = new Anime(dataModel);
            }

            internal Episode(HistoryDataModel dataModel) : this()
            {
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (AnimeLanguage) dataModel.Language;
                this.ParentObject = new Anime(dataModel);
            }

            #region Properties

            /// <summary>
            /// Gets the episode number.
            /// </summary>
            public int ContentIndex { get; }

            /// <summary>
            /// Gets the general language (english/german) of the episode.
            /// </summary>
            public Language GeneralLanguage
                =>
                (this.Language == AnimeLanguage.GerSub) || (this.Language == AnimeLanguage.GerDub)
                    ? Properties.Language.German
                    : (this.Language == AnimeLanguage.EngSub) || (this.Language == AnimeLanguage.EngDub)
                        ? Properties.Language.English
                        : Properties.Language.Unkown;

            /// <summary>
            /// Gets the language of the episode
            /// </summary>
            public AnimeLanguage Language { get; }

            /// <inheritdoc />
            IMediaObject IMediaContent<IMediaObject>.ParentObject => this.ParentObject;

            /// <inheritdoc />
            IMediaObject IMediaContent.ParentObject => this.ParentObject;

            /// <summary>
            /// Gets the anime this episode> belongs to.
            /// </summary>
            public Anime ParentObject { get; }

            /// <summary>
            /// </summary>
            public Senpai Senpai { get; set; }

            /// <summary>
            /// Gets the available streams of the episode.
            /// </summary>
            public IInitialisableProperty<IEnumerable<Stream>> Streams => this._streams;

            #endregion

            #region Methods

            /// <summary>
            /// Adds the episode to the bookmarks.
            /// </summary>
            /// <param name="senpai"></param>
            /// <returns>If the action was successful.</returns>
            public Task<ProxerResult> AddToBookmarks(Senpai senpai)
            {
                return new UserControlPanel(senpai).AddToBookmarks(this);
            }

            private async Task<ProxerResult> InitStreams()
            {
                ProxerResult<ProxerApiResponse<StreamDataModel[]>> lResult =
                    await
                        RequestHandler.ApiRequest(ApiRequestBuilder.AnimeGetStreams(this.ParentObject.Id,
                            this.ContentIndex, this.Language.ToString().ToLowerInvariant(), this.Senpai));
                if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

                this._streams.SetInitialisedObject(from streamDataModel in lResult.Result.Data
                    select new Stream(streamDataModel, this));

                return new ProxerResult();
            }

            #endregion

            /// <summary>
            /// Represents a stream of an episode.
            /// </summary>
            public class Stream
            {
                private readonly InitialisableProperty<Uri> _link;

                internal Stream(StreamDataModel dataModel, Episode episode)
                {
                    this._link = new InitialisableProperty<Uri>(this.InitStreamLink);

                    this.Episode = episode;
                    this.Hoster = dataModel.StreamHoster;
                    this.HosterFullName = dataModel.HosterFullName;
                    this.HosterImage = new Uri(ApiConstants.ProxerHosterImageUrl + dataModel.HosterImageFileName);
                    this.HostingType = dataModel.HostingType;
                    this.Id = dataModel.StreamId;
                    this.Translator = dataModel.TranslatorId == null
                        ? null
                        : new Translator(dataModel.TranslatorId.Value, dataModel.TranslatorName,
                            this.Episode.GeneralLanguage);
                    this.UploadDate = dataModel.UploadTimestamp;
                    this.Uploader = new User(dataModel.UploaderName, dataModel.UploaderId);
                }

                #region Properties

                /// <summary>
                /// </summary>
                public Episode Episode { get; }

                /// <summary>
                /// Gets the streampartner of the stream.
                /// </summary>
                public StreamHoster Hoster { get; }

                /// <summary>
                /// </summary>
                public string HosterFullName { get; }

                /// <summary>
                /// </summary>
                public Uri HosterImage { get; }

                /// <summary>
                /// </summary>
                public string HostingType { get; }

                /// <summary>
                /// </summary>
                public int Id { get; }

                /// <summary>
                /// Gets the link of the stream.
                /// </summary>
                public IInitialisableProperty<Uri> Link => this._link;

                /// <summary>
                /// </summary>
                public Translator Translator { get; }

                /// <summary>
                /// </summary>
                public DateTime UploadDate { get; }

                /// <summary>
                /// </summary>
                public User Uploader { get; }

                #endregion

                #region Methods

                private async Task<ProxerResult> InitStreamLink()
                {
                    ProxerResult<ProxerApiResponse<string>> lResult =
                        await RequestHandler.ApiRequest(ApiRequestBuilder.AnimeGetLink(this.Id));
                    if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);
                    string lData = lResult.Result.Data;

                    this._link.SetInitialisedObject(new Uri(lData.StartsWith("//") ? $"https:{lData}" : lData));

                    return new ProxerResult();
                }

                #endregion
            }
        }
    }
}