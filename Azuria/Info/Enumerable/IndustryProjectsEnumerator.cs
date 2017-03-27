using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.List;
using Azuria.Api.v1.Enums;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enumerable;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.Info.Enumerable
{
    internal class IndustryProjectsEnumerator : PagedEnumerator<IMediaObject>
    {
        private const int ResultsPerPage = 100;
        private readonly int _id;
        private readonly bool? _includeH;
        private readonly IndustryType? _type;

        internal IndustryProjectsEnumerator(int id, IndustryType? type, bool? includeH, int retryCount = 2) 
            : base(ResultsPerPage, retryCount)
        {
            this._id = id;
            this._type = type;
            this._includeH = includeH;
        }

        #region Methods

        /// <inheritdoc />
        protected override async Task<IProxerResult<IEnumerable<IMediaObject>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<IndustryProjectDataModel[]> lResult = await RequestHandler.ApiRequest(
                ListRequestBuilder.GetIndustryProjects(
                    this._id, this._type, this._includeH, nextPage, ResultsPerPage
                )
            ).ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<IEnumerable<IMediaObject>>(lResult.Exceptions);

            return new ProxerResult<IEnumerable<IMediaObject>>(lResult.Result
                .Select(model => GetMediaObject(model))
                .Where(mediaObject => mediaObject != null));

            IMediaObject GetMediaObject(IndustryProjectDataModel dataModel)
            {
                switch (dataModel.EntryType)
                {
                    case MediaEntryType.Anime:
                        return new Anime(dataModel);
                    case MediaEntryType.Manga:
                        return new Manga(dataModel);
                    default:
                        return null;
                }
            }
        }

        #endregion
    }
}