using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Web;

namespace Azuria.Test.Core
{
    public class TestingHttpClient : IHttpClient
    {
        private readonly Senpai _senpai;

        public TestingHttpClient(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Methods

        /// <inheritdoc />
        public Task<ProxerResult<string>> GetRequest(Uri url, Dictionary<string, string> headers = null)
        {
            return Task.Factory.StartNew(() => new ProxerResult<string>(new NotImplementedException()));
        }

        /// <inheritdoc />
        public Task<ProxerResult<string>> PostRequest(Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            Dictionary<string, string> headers = null)
        {
            return Task.Factory.StartNew(() => this.PostRequestSync(url, postArgs, headers));
        }

        private ProxerResult<string> PostRequestSync(Uri url, IEnumerable<KeyValuePair<string, string>> postArgs,
            Dictionary<string, string> headers = null)
        {
            IEnumerable<KeyValuePair<string, string>> postArgsArray = postArgs as KeyValuePair<string, string>[] ??
                                                                      postArgs.ToArray();
            if (!ServerResponse.ServerResponses.Any()) ResponseSetup.InitRequests();

            foreach (
                ServerResponse response in
                ServerResponse.ServerResponses.Where(response => url.AbsoluteUri.StartsWith(response.BaseUrl)))
            {
                IEnumerable<ServerRequest> lMatchingRequests = response.PostRequests.Where(
                        request =>
                            (response.BaseUrl + request.Url).Equals(
                                $"{url.Scheme}://{url.Host}{url.AbsolutePath}"))
                    .Where(request => request.GetQuery().Equals(url.Query))
                    .Where(request => !request.ContainsSenpai || (this._senpai != null))
                    .Where(request => postArgsArray.All(request.PostArguments.Contains) &&
                                      request.PostArguments.All(postArgsArray.Contains))
                    .Where(request => (headers == null) || request.Headers.All(headers.Contains))
                    .Where(
                        request =>
                            (request.IsLoggedIn == null) ||
                            (this._senpai.IsProbablyLoggedIn == request.IsLoggedIn.Value));
                if (lMatchingRequests.Any())
                    return new ProxerResult<string>(response.Response);
            }

            return new ProxerResult<string>(new Exception("Response not found!"));
        }

        #endregion
    }
}