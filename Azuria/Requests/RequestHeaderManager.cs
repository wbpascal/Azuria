using System.Collections.Generic;
using Azuria.Authentication;

namespace Azuria.Requests
{
    /// <summary>
    /// </summary>
    public class RequestHeaderManager : IRequestHeaderManager
    {
        private const string ApiKeyHeaderName = "proxer-api-key";
        private const string LoginTokenHeaderName = "proxer-api-token";
        private readonly char[] _apiKey;
        private readonly ILoginManager _loginManager;

        /// <inheritdoc />
        public RequestHeaderManager(char[] apiKey, ILoginManager loginManager)
        {
            this._apiKey = apiKey;
            this._loginManager = loginManager;
        }

        /// <inheritdoc />
        public bool ContainsAuthenticationHeaders(Dictionary<string, string> header)
        {
            return header.ContainsKey(LoginTokenHeaderName);
        }

        /// <inheritdoc />
        public Dictionary<string, string> GetHeader()
        {
            Dictionary<string, string> lHeaders = new Dictionary<string, string>
            {
                {ApiKeyHeaderName, new string(this._apiKey)}
            };
            if (this._loginManager.SendTokenWithNextRequest())
                lHeaders.Add(LoginTokenHeaderName, new string(this._loginManager.LoginToken));

            return lHeaders;
        }
    }
}