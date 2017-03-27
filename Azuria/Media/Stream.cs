using System;
using System.Threading.Tasks;
using Azuria.Api;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Anime;
using Azuria.Api.v1.DataModels.Manga;
using Azuria.Api.v1.RequestBuilder;
using Azuria.ErrorHandling;
using Azuria.Info;
using Azuria.Media.Properties;
using Azuria.UserInfo;
using Azuria.Utilities;
using Azuria.Utilities.Properties;

namespace Azuria.Media
{
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
            this.Translator = GetTranslator();
            this.UploadDate = dataModel.UploadTimestamp;
            this.Uploader = new User(dataModel.UploaderName, dataModel.UploaderId);

            Translator GetTranslator()
            {
                if (dataModel.TranslatorId == null) return null;
                return new Translator(dataModel.TranslatorId.Value, dataModel.TranslatorName, 
                    this.Episode.GeneralLanguage.GetCountry());
            }
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

        private async Task<IProxerResult> InitStreamLink()
        {
            ProxerApiResponse<string> lResult = await RequestHandler.ApiRequest(
                AnimeRequestBuilder.GetLink(this.Id)).ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            string lData = lResult.Result;

            this._link.Set(new Uri(lData.StartsWith("//") ? $"https:{lData}" : lData));

            return new ProxerResult();
        }

        #endregion
    }
}