using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Anime;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.RequestBuilder;
using Azuria.ErrorHandling;
using Azuria.Media.Properties;
using Azuria.UserInfo.ControlPanel;
using Azuria.Utilities.Properties;

namespace Azuria.Media
{
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
        public Language GeneralLanguage => this.GetGeneralLanguage();
                

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
        public Task<IProxerResult> AddToBookmarks(Senpai senpai)
        {
            return new UserControlPanel(senpai).AddToBookmarks(this);
        }

        private Language GetGeneralLanguage()
        {
            switch (this.Language)
            {
                case AnimeLanguage.GerSub:
                case AnimeLanguage.GerDub:
                    return Properties.Language.German;
                case AnimeLanguage.EngSub:
                case AnimeLanguage.EngDub:
                    return Properties.Language.English;
                default:
                    return Properties.Language.Unkown;
            }
        }

        private async Task<IProxerResult> InitStreams()
        {
            ProxerApiResponse<StreamDataModel[]> lResult = await RequestHandler.ApiRequest(
                    AnimeRequestBuilder.GetStreams(this.ParentObject.Id, this.ContentIndex,
                        this.Language.ToString().ToLowerInvariant(), this.Senpai))
                .ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);

            this._streams.Set(from streamDataModel in lResult.Result
                select new Stream(streamDataModel, this));

            return new ProxerResult();
        }

        #endregion
    }
}