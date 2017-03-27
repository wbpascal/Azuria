using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.List;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enumerable;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.Info.Enumerable
{
    internal class TranslatorProjectEnumerator : PagedEnumerator<TranslatorProject>
    {
        private readonly int _id;
        private readonly TranslationStatus? _status;
        private readonly bool? _includeH;
        private const int ResultsPerPage = 100;

        internal TranslatorProjectEnumerator(int id, TranslationStatus? status = null, bool? includeH = null,
            int retryCount = 2) : base(ResultsPerPage, retryCount)
        {
            this._id = id;
            this._status = status;
            this._includeH = includeH;
        }

        /// <inheritdoc />
        protected override async Task<IProxerResult<IEnumerable<TranslatorProject>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<TranslatorProjectDataModel[]> lResult = await RequestHandler.ApiRequest(
                    ListRequestBuilder.GetTranslatorProjects(
                        this._id, this._status, this._includeH, nextPage, ResultsPerPage
                    )
                )
                .ConfigureAwait(false);
            if(!lResult.Success || lResult.Result == null)
                return new ProxerResult<IEnumerable<TranslatorProject>>(lResult.Exceptions);

            return new ProxerResult<IEnumerable<TranslatorProject>>(lResult.Result
                .Select(model => new TranslatorProject(model)));
        }
    }
}
