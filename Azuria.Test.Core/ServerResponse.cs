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

        public string BaseUrl { get; }

        public List<ServerRequest> Requests { get; set; } = new List<ServerRequest>();

        public string Response { get; private set; }

        public static List<ServerResponse> ServerResponses { get; set; } = new List<ServerResponse>();

        public static ServerResponse Create(string baseUrl, Action<ServerResponse> factory)
        {
            ServerResponse lResponse = new ServerResponse(baseUrl);
            factory.Invoke(lResponse);
            ServerResponses.Add(lResponse);
            return lResponse;
        }

        public ServerRequest Get(string url)
        {
            ServerRequest lRequest = new ServerRequest(this.BaseUrl + url) {RequestMethod = RequestMethod.Get};
            this.Requests.Add(lRequest);
            return lRequest;
        }

        public ServerRequest Post(string url)
        {
            ServerRequest lRequest = new ServerRequest(this.BaseUrl + url) {RequestMethod = RequestMethod.Post};
            this.Requests.Add(lRequest);
            return lRequest;
        }

        public ServerResponse Respond(string response)
        {
            this.Response = response;
            return this;
        }
    }
}