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
    /// Represents a manga.
    /// </summary>
    [DebuggerDisplay("Manga: {Name} [{Id}]")]
    public class Manga : MediaObject
    {
        private readonly InitialisableProperty<IEnumerable<Language>> _availableLanguages;
        private readonly InitialisableProperty<MangaMedium> _mangaMedium;

        internal Manga(int id) : base(id)
        {
            this._availableLanguages =
                new InitialisableProperty<IEnumerable<Language>>(this.InitAvailableLanguages);
            this.CommentsLatest = new CommentEnumerable<Manga>(this, "latest");
            this.CommentsRating = new CommentEnumerable<Manga>(this, "rating");
            this._mangaMedium = new InitialisableProperty<MangaMedium>(this.InitMainInfo);
        }

        internal Manga(string name, int id) : this(id)
        {
            this._name.Set(name);
        }

        internal Manga(IEntryInfoDataModel dataModel) : this(dataModel.EntryName, dataModel.EntryId)
        {
            if (dataModel.EntryType != MediaEntryType.Manga)
                throw new ArgumentException(nameof(dataModel.EntryType));
            this._mangaMedium.Set((MangaMedium) dataModel.EntryMedium);
        }

        internal Manga(BookmarkDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._mangaMedium.Set((MangaMedium) dataModel.EntryMedium);
            this._status.Set(dataModel.Status);
        }

        internal Manga(EntryDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
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

        internal Manga(FullEntryDataModel dataModel) : this((EntryDataModel) dataModel)
        {
            this.InitGroups(dataModel.Translator);
            this.InitIndustry(dataModel.Publisher);
            this.InitNames(dataModel.Names);
            this.InitSeasons(dataModel.Seasons);
            this.InitTags(dataModel.Tags);
            this._availableLanguages.Set(dataModel.AvailableLanguages.Cast<Language>());
        }

        internal Manga(RelationDataModel dataModel) : this((EntryDataModel) dataModel)
        {
            this._availableLanguages.Set(dataModel.AvailableLanguages.Cast<Language>());
        }

        internal Manga(SearchDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._availableLanguages.Set(dataModel.AvailableLanguages.Cast<Language>());
            this._contentCount.Set(dataModel.ContentCount);
            this._genre.Set(dataModel.Genre);
            this._rating.Set(dataModel.Rating);
            this._status.Set(dataModel.Status);
        }

        #region Properties

        /// <summary>
        /// Gets the languages the <see cref="Manga" /> is available in.
        /// </summary>
        public IInitialisableProperty<IEnumerable<Language>> AvailableLanguages => this._availableLanguages;

        /// <summary>
        /// Gets the comments of the <see cref="Anime" /> in a chronological order.
        /// </summary>
        public new CommentEnumerable<Manga> CommentsLatest { get; }

        /// <summary>
        /// Gets the comments of the <see cref="Anime" /> ordered by rating.
        /// </summary>
        public new CommentEnumerable<Manga> CommentsRating { get; }

        /// <summary>
        /// Gets the medium of the <see cref="Manga" />.
        /// </summary>
        public IInitialisableProperty<MangaMedium> MangaMedium => this._mangaMedium;

        #endregion

        #region Methods

        /// <summary>
        /// Returns all <see cref="Chapter">epsiodes</see> of the <see cref="Manga" /> in a specified language.
        /// </summary>
        /// <param name="language">The language of the episodes.</param>
        /// <seealso cref="Chapter" />
        /// <returns>
        /// An enumeration of all available <see cref="Chapter">chapters</see> in the specified
        /// <paramref name="language">language</paramref> with a max count of <see cref="MediaObject.ContentCount" />.
        /// </returns>
        public async Task<IProxerResult<IEnumerable<Chapter>>> GetChapters(Language language)
        {
            if (!(await this.AvailableLanguages.Get(new Language[0]).ConfigureAwait(false)).Contains(language))
                return new ProxerResult<IEnumerable<Chapter>>(new Exception[] {new LanguageNotAvailableException()});

            IProxerResult<MediaContentDataModel[]> lContentObjectsResult =
                await this.GetContentObjects().ConfigureAwait(false);
            if (!lContentObjectsResult.Success || lContentObjectsResult.Result == null)
                return new ProxerResult<IEnumerable<Chapter>>(lContentObjectsResult.Exceptions);

            return new ProxerResult<IEnumerable<Chapter>>(from contentDataModel in lContentObjectsResult.Result
                where (Language) contentDataModel.Language == language
                select new Chapter(this, contentDataModel));
        }

        internal async Task<IProxerResult> InitAvailableLanguages()
        {
            ProxerApiResponse<MediaLanguage[]> lResult = await RequestHandler.ApiRequest(
                InfoRequestBuilder.GetLanguage(this.Id)).ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            this._availableLanguages.Set(lResult.Result.Cast<Language>());
            return new ProxerResult();
        }

        internal void InitMainInfoManga(EntryDataModel dataModel)
        {
            this._mangaMedium.Set((MangaMedium) dataModel.EntryMedium);
        }

        #endregion
    }
}