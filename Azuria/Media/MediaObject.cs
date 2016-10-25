using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.Enums;
using Azuria.ErrorHandling;
using Azuria.Media.Properties;
using Azuria.UserInfo;
using Azuria.UserInfo.ControlPanel;
using Azuria.Utilities.Properties;

#pragma warning disable 1591

namespace Azuria.Media
{
    /// <summary>
    /// </summary>
    public abstract class MediaObject : IMediaObject
    {
        protected readonly InitialisableProperty<int> _clicks;
        protected readonly InitialisableProperty<int> _contentCount;
        protected readonly InitialisableProperty<string> _description;
        protected readonly InitialisableProperty<string> _englishTitle;
        protected readonly InitialisableProperty<IEnumerable<FskType>> _fsk;
        protected readonly InitialisableProperty<IEnumerable<GenreType>> _genre;
        protected readonly InitialisableProperty<string> _germanTitle;
        protected readonly InitialisableProperty<IEnumerable<Translator>> _groups;
        protected readonly InitialisableProperty<IEnumerable<Industry>> _industry;
        protected readonly InitialisableProperty<bool> _isHContent;
        protected readonly InitialisableProperty<bool> _isLicensed;
        protected readonly InitialisableProperty<string> _japaneseTitle;
        protected readonly InitialisableProperty<string> _name;
        protected readonly InitialisableProperty<MediaRating> _rating;
        protected readonly InitialisableProperty<IEnumerable<IMediaObject>> _relations;
        protected readonly InitialisableProperty<MediaSeasonInfo> _season;
        protected readonly InitialisableProperty<MediaStatus> _status;
        protected readonly InitialisableProperty<string> _synonym;
        protected readonly InitialisableProperty<IEnumerable<Tag>> _tags;

        internal MediaObject(int id)
        {
            this.Id = id;
            this._clicks = new InitialisableProperty<int>(this.InitMainInfo);
            this._contentCount = new InitialisableProperty<int>(this.InitMainInfo);
            this._description = new InitialisableProperty<string>(this.InitMainInfo);
            this._englishTitle = new InitialisableProperty<string>(this.InitNames, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this._fsk = new InitialisableProperty<IEnumerable<FskType>>(this.InitMainInfo);
            this._genre = new InitialisableProperty<IEnumerable<GenreType>>(this.InitMainInfo);
            this._germanTitle = new InitialisableProperty<string>(this.InitNames, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this._groups = new InitialisableProperty<IEnumerable<Translator>>(this.InitGroups);
            this._industry = new InitialisableProperty<IEnumerable<Industry>>(this.InitIndustry);
            this._isLicensed = new InitialisableProperty<bool>(this.InitMainInfo);
            this._isHContent = new InitialisableProperty<bool>(this.InitIsHContent);
            this._japaneseTitle = new InitialisableProperty<string>(this.InitNames, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this._name = new InitialisableProperty<string>(this.InitMainInfo, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this._rating = new InitialisableProperty<MediaRating>(this.InitMainInfo);
            this._relations =
                new InitialisableProperty<IEnumerable<IMediaObject>>(this.InitRelations);
            this._season = new InitialisableProperty<MediaSeasonInfo>(this.InitSeasons);
            this._status = new InitialisableProperty<MediaStatus>(this.InitMainInfo);
            this._synonym = new InitialisableProperty<string>(this.InitNames, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this._tags = new InitialisableProperty<IEnumerable<Tag>>(this.InitEntryTags);
        }

        #region Properties

        /// <inheritdoc />
        public IInitialisableProperty<int> Clicks => this._clicks;

        /// <inheritdoc />
        public IInitialisableProperty<int> ContentCount => this._contentCount;

        /// <inheritdoc />
        public Uri CoverUri => new Uri($"{ApiConstants.ProxerCoverCdnUrl}{this.Id}.jpg");

        /// <inheritdoc />
        public IInitialisableProperty<string> Description => this._description;

        /// <inheritdoc />
        public IInitialisableProperty<string> EnglishTitle => this._englishTitle;

        /// <inheritdoc />
        public IInitialisableProperty<IEnumerable<FskType>> Fsk => this._fsk;

        /// <inheritdoc />
        public IInitialisableProperty<IEnumerable<GenreType>> Genre => this._genre;

        /// <inheritdoc />
        public IInitialisableProperty<string> GermanTitle => this._germanTitle;

        /// <inheritdoc />
        public IInitialisableProperty<IEnumerable<Translator>> Groups => this._groups;

        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public IInitialisableProperty<IEnumerable<Industry>> Industry => this._industry;

        /// <inheritdoc />
        public IInitialisableProperty<bool> IsHContent => this._isHContent;

        /// <inheritdoc />
        public IInitialisableProperty<bool> IsLicensed => this._isLicensed;

        /// <inheritdoc />
        public IInitialisableProperty<string> JapaneseTitle => this._japaneseTitle;

        /// <inheritdoc />
        public IInitialisableProperty<string> Name => this._name;

        /// <inheritdoc />
        public IInitialisableProperty<MediaRating> Rating => this._rating;

        /// <inheritdoc />
        public IInitialisableProperty<IEnumerable<IMediaObject>> Relations => this._relations;

        /// <inheritdoc />
        public IInitialisableProperty<MediaSeasonInfo> Season => this._season;

        /// <inheritdoc />
        public IInitialisableProperty<MediaStatus> Status => this._status;

        /// <inheritdoc />
        public IInitialisableProperty<string> Synonym => this._synonym;

        /// <inheritdoc />
        public IInitialisableProperty<IEnumerable<Tag>> Tags => this._tags;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Task<ProxerResult> AddToProfileList(Senpai senpai, MediaProfileList list)
        {
            return new UserControlPanel(senpai).AddToProfileList(this, list);
        }

        /// <summary>
        /// Gets an <see cref="Anime" /> or <see cref="Manga" /> of a specified id.
        /// </summary>
        /// <param name="id">The id of the <see cref="Anime" /> or <see cref="Manga" />.</param>
        /// <returns>
        /// If the action was successful and if it was, an object representing either an <see cref="Anime" /> or
        /// <see cref="Manga" />.
        /// </returns>
        public static async Task<ProxerResult<IMediaObject>> CreateFromId(int id)
        {
            if (id <= 0) return new ProxerResult<IMediaObject>(new ArgumentException(nameof(id)));

            ProxerResult<ProxerApiResponse<EntryDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetEntry(id));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IMediaObject>(lResult.Exceptions);
            EntryDataModel lDataModel = lResult.Result.Data;

            return
                new ProxerResult<IMediaObject>(lDataModel.EntryType == MediaEntryType.Anime
                    ? new Anime(lDataModel)
                    : (IMediaObject) new Manga(lDataModel));
        }

        internal async Task<ProxerResult<MediaContentDataModel[]>> GetContentObjects()
        {
            ProxerResult<int> lContentCountResult = await this.ContentCount;
            if (!lContentCountResult.Success || (lContentCountResult.Result == default(int)))
                return new ProxerResult<MediaContentDataModel[]>(lContentCountResult.Exceptions);

            ProxerResult<ProxerApiResponse<ListInfoDataModel>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetListInfo(this.Id, 0, lContentCountResult.Result));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<MediaContentDataModel[]>(lResult.Exceptions);
            ListInfoDataModel lData = lResult.Result.Data;

            return new ProxerResult<MediaContentDataModel[]>(lData.ContentObjects);
        }

        protected async Task<ProxerResult> InitEntryTags()
        {
            ProxerResult<ProxerApiResponse<EntryTagDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetEntryTags(this.Id));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            this._tags.SetInitialisedObject(from entryTagDataModel in lResult.Result.Data
                select new Tag(entryTagDataModel));
            return new ProxerResult();
        }

        protected async Task<ProxerResult> InitGroups()
        {
            ProxerResult<ProxerApiResponse<TranslatorDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetGroups(this.Id));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            this._groups.SetInitialisedObject(from translatorDataModel in lResult.Result.Data
                select new Translator(translatorDataModel));

            return new ProxerResult();
        }

        protected async Task<ProxerResult> InitIndustry()
        {
            ProxerResult<ProxerApiResponse<PublisherDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetPublisher(this.Id));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            this._industry.SetInitialisedObject(from publisherDataModel in lResult.Result.Data
                select new Industry(publisherDataModel));

            return new ProxerResult();
        }

        protected async Task<ProxerResult> InitIsHContent()
        {
            ProxerResult<ProxerApiResponse<bool>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetGate(this.Id));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            this._isHContent.SetInitialisedObject(lResult.Result.Data);

            return new ProxerResult();
        }

        protected async Task<ProxerResult> InitMainInfo()
        {
            ProxerResult<ProxerApiResponse<EntryDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetEntry(this.Id));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);
            EntryDataModel lDataModel = lResult.Result.Data;

            (this as Anime)?.InitMainInfoAnime(lDataModel);
            this._clicks.SetInitialisedObject(lDataModel.Clicks);
            this._contentCount.SetInitialisedObject(lDataModel.ContentCount);
            this._description.SetInitialisedObject(lDataModel.Description);
            this._fsk.SetInitialisedObject(lDataModel.Fsk);
            this._genre.SetInitialisedObject(lDataModel.Genre);
            this._isLicensed.SetInitialisedObject(lDataModel.IsLicensed);
            (this as Manga)?.InitMainInfoManga(lDataModel);
            this._name.SetInitialisedObject(lDataModel.EntryName);
            this._rating.SetInitialisedObject(lDataModel.Rating);
            this._status.SetInitialisedObject(lDataModel.Status);

            return new ProxerResult();
        }

        protected async Task<ProxerResult> InitNames()
        {
            ProxerResult<ProxerApiResponse<NameDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetName(this.Id));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);
            foreach (NameDataModel nameDataModel in lResult.Result.Data)
                switch (nameDataModel.Type)
                {
                    case MediaNameType.Original:
                        this._name.SetInitialisedObject(nameDataModel.Name);
                        break;
                    case MediaNameType.English:
                        this._englishTitle.SetInitialisedObject(nameDataModel.Name);
                        break;
                    case MediaNameType.German:
                        this._germanTitle.SetInitialisedObject(nameDataModel.Name);
                        break;
                    case MediaNameType.Japanese:
                        this._japaneseTitle.SetInitialisedObject(nameDataModel.Name);
                        break;
                    case MediaNameType.Synonym:
                        this._synonym.SetInitialisedObject(nameDataModel.Name);
                        break;
                }

            return new ProxerResult();
        }

        protected async Task<ProxerResult> InitRelations()
        {
            ProxerResult<ProxerApiResponse<RelationDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetRelations(this.Id));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);

            this._relations.SetInitialisedObject(from dataModel in lResult.Result.Data
                select dataModel.EntryType == MediaEntryType.Anime
                    ? new Anime(dataModel)
                    : (IMediaObject) new Manga(dataModel));

            return new ProxerResult();
        }

        protected async Task<ProxerResult> InitSeasons()
        {
            ProxerResult<ProxerApiResponse<SeasonDataModel[]>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.InfoGetSeason(this.Id));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);
            SeasonDataModel[] lData = lResult.Result.Data;

            if ((lData.Length > 1) && !lData[0].Equals(lData[1]))
                this._season.SetInitialisedObject(new MediaSeasonInfo(lData[0], lData[1]));
            else if (lData.Length > 0) this._season.SetInitialisedObject(new MediaSeasonInfo(lData[0]));

            return new ProxerResult();
        }

        #endregion
    }
}