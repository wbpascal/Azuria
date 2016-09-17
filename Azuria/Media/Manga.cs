using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.Search;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.Enums;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Media.Properties;
using Azuria.UserInfo.Comment;
using Azuria.UserInfo.ControlPanel;
using Azuria.Utilities.Properties;

// ReSharper disable LoopCanBeConvertedToQuery

namespace Azuria.Media
{
    /// <summary>
    ///     Represents a manga.
    /// </summary>
    [DebuggerDisplay("Manga: {Name} [{Id}]")]
    public class Manga : AnimeMangaObject
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
            this._name.SetInitialisedObject(name);
        }

        internal Manga(IEntryInfoDataModel dataModel) : this(dataModel.EntryName, dataModel.EntryId)
        {
            if (dataModel.EntryType != AnimeMangaEntryType.Manga)
                throw new ArgumentException(nameof(dataModel.EntryType));
            this._mangaMedium.SetInitialisedObject((MangaMedium) dataModel.EntryMedium);
        }

        internal Manga(BookmarkDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._mangaMedium.SetInitialisedObject((MangaMedium) dataModel.EntryMedium);
            this._status.SetInitialisedObject(dataModel.Status);
        }

        internal Manga(EntryDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
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

        internal Manga(RelationDataModel dataModel) : this((EntryDataModel) dataModel)
        {
            this._availableLanguages.SetInitialisedObject(dataModel.AvailableLanguages.Cast<Language>());
        }

        internal Manga(SearchDataModel dataModel) : this((IEntryInfoDataModel) dataModel)
        {
            this._availableLanguages.SetInitialisedObject(dataModel.AvailableLanguages.Cast<Language>());
            this._contentCount.SetInitialisedObject(dataModel.ContentCount);
            this._genre.SetInitialisedObject(dataModel.Genre);
            this._rating.SetInitialisedObject(dataModel.Rating);
            this._status.SetInitialisedObject(dataModel.Status);
        }

        #region Properties

        /// <summary>
        ///     Gets the languages the <see cref="Manga" /> is available in.
        /// </summary>
        public IInitialisableProperty<IEnumerable<Language>> AvailableLanguages => this._availableLanguages;

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> in a chronological order.
        /// </summary>
        public IEnumerable<Comment<Manga>> CommentsLatest { get; }

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> ordered by rating.
        /// </summary>
        public IEnumerable<Comment<Manga>> CommentsRating { get; }

        /// <summary>
        ///     Gets the medium of the <see cref="Manga" />.
        /// </summary>
        public IInitialisableProperty<MangaMedium> MangaMedium => this._mangaMedium;

        #endregion

        #region Methods

        /// <summary>
        ///     Returns all <see cref="Chapter">epsiodes</see> of the <see cref="Manga" /> in a specified language.
        /// </summary>
        /// <param name="language">The language of the episodes.</param>
        /// <seealso cref="Chapter" />
        /// <returns>
        ///     An enumeration of all available <see cref="Chapter">chapters</see> in the specified
        ///     <paramref name="language">language</paramref> with a max count of <see cref="AnimeMangaObject.ContentCount" />.
        /// </returns>
        public async Task<ProxerResult<IEnumerable<Chapter>>> GetChapters(Language language)
        {
            if (!(await this.AvailableLanguages.GetObject(new Language[0])).Contains(language))
                return new ProxerResult<IEnumerable<Chapter>>(new Exception[] {new LanguageNotAvailableException()});

            ProxerResult<AnimeMangaContentDataModel[]> lContentObjectsResult =
                await this.GetContentObjects();
            if (!lContentObjectsResult.Success || (lContentObjectsResult.Result == null))
                return new ProxerResult<IEnumerable<Chapter>>(lContentObjectsResult.Exceptions);

            return new ProxerResult<IEnumerable<Chapter>>(from contentDataModel in lContentObjectsResult.Result
                where (Language) contentDataModel.Language == language
                select new Chapter(this, contentDataModel));
        }

        internal async Task<ProxerResult> InitAvailableLanguages()
        {
            ProxerResult<ProxerInfoLanguageResponse> lResult =
                await
                    RequestHandler.ApiCustomRequest<ProxerInfoLanguageResponse>(
                        ApiRequestBuilder.InfoGetLanguage(this.Id));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);
            this._availableLanguages.SetInitialisedObject(lResult.Result.Data.Cast<Language>());
            return new ProxerResult();
        }

        internal void InitMainInfoManga(EntryDataModel dataModel)
        {
            this._mangaMedium.SetInitialisedObject((MangaMedium) dataModel.EntryMedium);
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
            public string Title { get; }

            #endregion

            #region Methods

            /// <summary>
            ///     Adds the <see cref="Chapter" /> to the bookmarks.
            /// </summary>
            /// <param name="senpai"></param>
            /// <returns>If the action was successful.</returns>
            public Task<ProxerResult> AddToBookmarks(Senpai senpai)
            {
                return new UserControlPanel(senpai).AddToBookmarks((IAnimeMangaContent<Manga>) this);
            }

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