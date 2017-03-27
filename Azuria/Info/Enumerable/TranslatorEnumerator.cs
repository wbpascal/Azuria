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
using Azuria.Media.Properties;
using Azuria.Utilities;

namespace Azuria.Info.Enumerable
{
    internal class TranslatorEnumerator : PagedEnumerator<Translator>
    {
        private readonly string _startsWith;
        private readonly string _contains;
        private readonly Country? _country;
        private const int ResultsPerPage = 100;

        internal TranslatorEnumerator(string startsWith, string contains, Country? country, int retryCount) 
            : base(ResultsPerPage, retryCount)
        {
            this._startsWith = startsWith;
            this._contains = contains;
            this._country = country;
        }

        /// <inheritdoc />
        protected override async Task<IProxerResult<IEnumerable<Translator>>> GetNextPage(int nextPage)
        {
            ProxerApiResponse<TranslatorDataModel[]> lResult = await RequestHandler.ApiRequest(
                    ListRequestBuilder.GetTranslatorgroups(
                        this._startsWith, this._contains, this._country.ToShortString(), ResultsPerPage, nextPage
                    )
                )
                .ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult<IEnumerable<Translator>>(lResult.Exceptions);

            return new ProxerResult<IEnumerable<Translator>>(lResult.Result
                .Select(model => new Translator(model)));
        }
    }
}
