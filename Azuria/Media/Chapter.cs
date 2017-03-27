using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.DataModels.Manga;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Api.v1.RequestBuilder;
using Azuria.ErrorHandling;
using Azuria.Info;
using Azuria.Media.Properties;
using Azuria.UserInfo;
using Azuria.UserInfo.ControlPanel;
using Azuria.Utilities;
using Azuria.Utilities.Properties;

namespace Azuria.Media
{
    /// <summary>
    /// Represents a chapter of a <see cref="Manga" />.
    /// </summary>
    public class Chapter : IMediaContent<Manga>, IMediaContent<IMediaObject>
    {
        private readonly InitialisableProperty<int> _chapterId;
        private readonly InitialisableProperty<IEnumerable<Page>> _pages;
        private readonly InitialisableProperty<string> _title;
        private readonly InitialisableProperty<Translator> _translator;
        private readonly InitialisableProperty<DateTime> _uploadDate;
        private readonly InitialisableProperty<User> _uploader;

        private Chapter()
        {
            this._chapterId = new InitialisableProperty<int>(this.InitInfo);
            this._pages = new InitialisableProperty<IEnumerable<Page>>(this.InitInfo);
            this._uploadDate = new InitialisableProperty<DateTime>(this.InitInfo);
            this._uploader = new InitialisableProperty<User>(this.InitInfo);
            this._title = new InitialisableProperty<string>(this.InitInfo);
            this._translator = new InitialisableProperty<Translator>(this.InitInfo);
        }

        internal Chapter(Manga manga, MediaContentDataModel dataModel) : this()
        {
            this.ContentIndex = dataModel.ContentIndex;
            this.Language = (Language) dataModel.Language;
            this.ParentObject = manga;

            this._title.Set(dataModel.Title);
        }

        internal Chapter(BookmarkDataModel dataModel) : this()
        {
            this.ContentIndex = dataModel.ContentIndex;
            this.Language = (Language) dataModel.Language;
            this.ParentObject = new Manga(dataModel);
        }

        internal Chapter(HistoryDataModel dataModel) : this()
        {
            this.ContentIndex = dataModel.ContentIndex;
            this.Language = (Language) dataModel.Language;
            this.ParentObject = new Manga(dataModel);
        }

        #region Properties

        /// <summary>
        /// </summary>
        public IInitialisableProperty<int> ChapterId => this._chapterId;

        /// <summary>
        /// Gets the <see cref="Chapter" />-number.
        /// </summary>
        public int ContentIndex { get; }

        /// <summary>
        /// Gets the general language (english/german) of the <see cref="Chapter" />.
        /// </summary>
        public Language GeneralLanguage => this.Language;

        /// <summary>
        /// Gets the language of the <see cref="Chapter" />.
        /// </summary>
        public Language Language { get; }

        /// <summary>
        /// </summary>
        public IInitialisableProperty<IEnumerable<Page>> Pages => this._pages;

        /// <inheritdoc />
        IMediaObject IMediaContent<IMediaObject>.ParentObject => this.ParentObject;

        /// <inheritdoc />
        IMediaObject IMediaContent.ParentObject => this.ParentObject;

        /// <summary>
        /// Gets the <see cref="Manga" /> this <see cref="Chapter" /> belongs to.
        /// </summary>
        public Manga ParentObject { get; }

        /// <summary>
        /// </summary>
        public Senpai Senpai { get; set; }

        /// <summary>
        /// Gets the title of the <see cref="Chapter" />.
        /// </summary>
        public IInitialisableProperty<string> Title => this._title;

        /// <summary>
        /// </summary>
        public IInitialisableProperty<Translator> Translator => this._translator;

        /// <summary>
        /// </summary>
        public IInitialisableProperty<DateTime> UploadDate => this._uploadDate;

        /// <summary>
        /// </summary>
        public IInitialisableProperty<User> Uploader => this._uploader;

        #endregion

        #region Methods

        /// <summary>
        /// Adds the <see cref="Chapter" /> to the bookmarks.
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns>If the action was successful.</returns>
        public Task<IProxerResult> AddToBookmarks(Senpai senpai)
        {
            return new UserControlPanel(senpai).AddToBookmarks(this);
        }

        private async Task<IProxerResult> InitInfo()
        {
            ProxerApiResponse<ChapterDataModel> lResult = await RequestHandler.ApiRequest(
                    MangaRequestBuilder.GetChapter(this.ParentObject.Id, this.ContentIndex,
                        this.Language == Language.German ? "de" : "en", this.Senpai))
                .ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            ChapterDataModel lData = lResult.Result;

            this._chapterId.Set(lData.ChapterId);
            this._pages.Set(from pageDataModel in lData.Pages
                select new Page(pageDataModel, lData.ServerId, lData.EntryId, lData.ChapterId));
            this._uploadDate.Set(lData.UploadTimestamp);
            this._uploader.Set(new User(lData.UploaderName, lData.UploaderId));
            this._title.Set(lData.ChapterTitle);
            this._translator.Set(GetTranslator(lData));

            return new ProxerResult();

            Translator GetTranslator(ChapterDataModel dataModel)
            {
                if (dataModel.TranslatorId == null) return null;
                return new Translator(dataModel.TranslatorId.Value, dataModel.TranslatorName,
                    this.GeneralLanguage.GetCountry());
            }
        }

        #endregion
    }
}