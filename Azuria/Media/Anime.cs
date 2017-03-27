using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.List;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.Enums;
using Azuria.Api.v1.RequestBuilder;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Media.Properties;
using Azuria.UserInfo.Comment;
using Azuria.Utilities.Properties;

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
            this._name.Set(name);
        }

        internal Anime(IEntryInfoDataModel dataModel) : this(dataModel.EntryName, dataModel.EntryId)
        {
            if (dataModel.EntryType != MediaEntryType.Anime)
                throw new ArgumentException(nameof(dataModel.EntryType));
            this._animeMedium.Set((AnimeMedium) dataModel.EntryMedium);
        }

        internal Anime(BookmarkDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._animeMedium.Set((AnimeMedium) dataModel.EntryMedium);
            this._status.Set(dataModel.Status);
        }

        internal Anime(EntryDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._clicks.Set(dataModel.Clicks);
            this._contentCount.Set(dataModel.ContentCount);
            this._description.Set(dataModel.Description);
            this._fsk.Set(dataModel.Fsk);
            this._genre.Set(dataModel.Genre);
            this._isLicensed.Set(dataModel.IsLicensed);
            this._rating.Set(dataModel.Rating);
            this._status.Set(dataModel.Status);
        }

        internal Anime(FullEntryDataModel dataModel) : this((EntryDataModel) dataModel)
        {
            this.InitGroups(dataModel.Translator);
            this.InitIndustry(dataModel.Publisher);
            this.InitNames(dataModel.Names);
            this.InitSeasons(dataModel.Seasons);
            this.InitTags(dataModel.Tags);
            this._availableLanguages.Set(dataModel.AvailableLanguages.Cast<AnimeLanguage>());
        }

        internal Anime(IndustryProjectDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._fsk.Set(dataModel.EntryFsk);
            this._genre.Set(dataModel.EntryGenre);
            this._rating.Set(dataModel.EntryRating);
            this._status.Set(dataModel.EntryStatus);
        }

        internal Anime(RelationDataModel dataModel) : this((EntryDataModel) dataModel)
        {
            this._availableLanguages.Set(dataModel.AvailableLanguages.Cast<AnimeLanguage>());
        }

        internal Anime(SearchDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._availableLanguages.Set(dataModel.AvailableLanguages.Cast<AnimeLanguage>());
            this._contentCount.Set(dataModel.ContentCount);
            this._genre.Set(dataModel.Genre);
            this._rating.Set(dataModel.Rating);
            this._status.Set(dataModel.Status);
        }

        internal Anime(TranslatorProjectDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._fsk.Set(dataModel.EntryFsk);
            this._genre.Set(dataModel.EntryGenre);
            this._rating.Set(dataModel.EntryRating);
            this._status.Set(dataModel.EntryStatus);
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
        public new CommentEnumerable<Anime> CommentsLatest { get; }

        /// <summary>
        /// Gets the comments of the anime ordered by rating.
        /// </summary>
        public new CommentEnumerable<Anime> CommentsRating { get; }

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
        public async Task<IProxerResult<IEnumerable<Episode>>> GetEpisodes(AnimeLanguage language)
        {
            if (!(await this.AvailableLanguages.Get(new AnimeLanguage[0]).ConfigureAwait(false)).Contains(language))
                return new ProxerResult<IEnumerable<Episode>>(new LanguageNotAvailableException());

            IProxerResult<MediaContentDataModel[]> lContentObjectsResult =
                await this.GetContentObjects().ConfigureAwait(false);
            if (!lContentObjectsResult.Success || lContentObjectsResult.Result == null)
                return new ProxerResult<IEnumerable<Episode>>(lContentObjectsResult.Exceptions);

            return new ProxerResult<IEnumerable<Episode>>(from contentDataModel in lContentObjectsResult.Result
                where (AnimeLanguage) contentDataModel.Language == language
                select new Episode(this, contentDataModel));
        }

        private async Task<IProxerResult> InitAvailableLanguages()
        {
            ProxerApiResponse<MediaLanguage[]> lResult = await RequestHandler.ApiRequest(
                InfoRequestBuilder.GetLanguage(this.Id)).ConfigureAwait(false);
            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);
            this._availableLanguages.Set(lResult.Result.Cast<AnimeLanguage>());
            return new ProxerResult();
        }

        internal void InitMainInfoAnime(EntryDataModel dataModel)
        {
            this._animeMedium.Set((AnimeMedium) dataModel.EntryMedium);
        }

        #endregion
    }
}