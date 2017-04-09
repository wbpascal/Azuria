using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Azuria.Authentication;
using Azuria.Connection;
using Azuria.ErrorHandling;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace Azuria.Test.Core
{
    public class TestingHttpClient : IHttpClient
    {
        private readonly IProxerClient _client;

        public TestingHttpClient(IProxerClient client)
        {
            this._client = client;
        }

        #region Methods

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public async Task<IProxerResult<string>> GetRequestAsync(Uri url, Dictionary<string, string> headers = null)
        {
            return await Task.Factory.StartNew(
                           () =>
                               this.GetResponse(url, RequestMethod.Get, new Dictionary<string, string>(), headers)
                       )
                       .ConfigureAwait(false);
        }

        private IProxerResult<string> GetResponse(
            Uri url, RequestMethod method,
            IEnumerable<KeyValuePair<string, string>> postArgs, Dictionary<string, string> headers = null)
        {
            IEnumerable<KeyValuePair<string, string>> postArgsArray =
                postArgs as KeyValuePair<string, string>[] ?? postArgs.ToArray();
            if (!ServerResponse.ServerResponses.Any()) ResponseSetup.InitRequests();

            Dictionary<string, StringValues> lQueryParams = QueryHelpers.ParseNullableQuery(url.Query);
            foreach (
                ServerResponse response in
                ServerResponse.ServerResponses.ToArray()
                    .Reverse()
                    .Where(response => url.AbsoluteUri.StartsWith(response.BaseUrl)))
            {
                IEnumerable<ServerRequest> lMatchingRequests = response.Requests.Where(
                        request =>
                            (response.BaseUrl + request.Url).Equals($"{url.Scheme}://{url.Host}{url.AbsolutePath}"))
                    .Where(request => request.RequestMethod == method)
                    .Where(request => request.QueryParams.Count == lQueryParams.Count)
                    .Where(
                        request => lQueryParams.All(
                            pair =>
                                request.QueryParams.ContainsKey(pair.Key) &&
                                request.QueryParams[pair.Key].Equals(pair.Value)))
                    .Where(
                        request => postArgsArray.All(request.PostArguments.Contains) &&
                                   request.PostArguments.All(postArgsArray.Contains))
                    .Where(request => headers == null || request.Headers.All(headers.Contains))
                    .Where(
                        request => request.IsLoggedIn == null ||
                                   this._client.Container.Resolve<ILoginManager>().CheckIsLoginProbablyValid() ==
                                   request.IsLoggedIn.Value);

                if (lMatchingRequests.Any())
                    return new ProxerResult<string>(response.Response);
            }

            return new ProxerResult<string>(new Exception("Response not found!"));
        }

        /// <inheritdoc />
        public async Task<IProxerResult<string>> PostRequestAsync(
            Uri url,
            IEnumerable<KeyValuePair<string, string>> postArgs,
            Dictionary<string, string> headers = null)
        {
            return await Task.Factory.StartNew(
                           () =>
                               this.GetResponse(url, RequestMethod.Post, postArgs, headers)
                       )
                       .ConfigureAwait(false);
        }

        #endregion
    }
}