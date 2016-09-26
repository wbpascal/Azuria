using System;
using System.Collections.Generic;

namespace Azuria.Test.Core
{
    public class ServerResponse
    {
        private ServerResponse(string baseUrl)
        {
            this.BaseUrl = baseUrl;
        }

        #region Properties

        public string BaseUrl { get; }

        public HashSet<ServerRequest> PostRequests { get; set; } = new HashSet<ServerRequest>();
        public string Response { get; private set; }

        public static HashSet<ServerResponse> ServerResponses { get; set; } = new HashSet<ServerResponse>();

        #endregion

        #region Methods

        public static ServerResponse Create(string baseUrl, Action<ServerResponse> factory)
        {
            ServerResponse lResponse = new ServerResponse(baseUrl);
            factory.Invoke(lResponse);
            ServerResponses.Add(lResponse);
            return lResponse;
        }

        public ServerRequest Post(string url)
        {
            ServerRequest lRequest = new ServerRequest(url) {RequestType = RequestType.Post};
            this.PostRequests.Add(lRequest);
            return lRequest;
        }

        public ServerResponse Respond(string response)
        {
            this.Response = response;
            return this;
        }

        #endregion
    }
}