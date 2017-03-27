using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.List;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enumerable;
using Azuria.ErrorHandling;
using Azuria.Media.Properties;
using Azuria.Utilities;

namespace Azuria.Info.Enumerable
{
    internal class IndustryEnumerator : PagedEnumerator<Industry>
    {
        private const int ResultsPerPage = 100;
        private readonly string _contains;
        private readonly Country? _country;
        private readonly string _startWith;
        private readonly IndustryType? _type;

        internal IndustryEnumerator(string startWith = "", string contains = "", Country? country = null,
            IndustryType? type = null, int retryCount = 2) : base(ResultsPerPage, retryCount)
        {
            this._startWith = startWith;
            this._contains = contains;
            this._country = country;
            this._type = type;
        }

        #region Methods

        /// <inheritdoc />
        protected override async Task<IProxerResult<IEnumerable<Industry>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<IndustryDataModel[]> lResult = await RequestHandler.ApiRequest(
                    ListRequestBuilder.GetIndustries(
                        this._startWith, this._contains, this._country.ToShortString(),
                        this._type.ToTypeString(), nextPage, ResultsPerPage
                    )
                )
                .ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<IEnumerable<Industry>>(lResult.Exceptions);

            return new ProxerResult<IEnumerable<Industry>>(lResult.Result.Select(model => new Industry(model)));
        }

        #endregion
    }
}