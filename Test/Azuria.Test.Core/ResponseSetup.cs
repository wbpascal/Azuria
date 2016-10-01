using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Azuria.Test.Core
{
    public class ResponseSetup
    {
        private static readonly Dictionary<string, string> JsonResponses = GetJsonResponses();

        #region Methods

        public static ServerResponse CreateForMessageAutoCheck()
        {
            return ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "0")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(JsonResponses["messenger_getmessagesCheck.json"]);
        }

        private static Dictionary<string, string> GetJsonResponses()
        {
            Dictionary<string, string> lReturn = new Dictionary<string, string>();
            foreach (
                string filePath in
                Directory.GetFiles((AppDomain.CurrentDomain.BaseDirectory + @"\Response").Replace(@"/\", "/"), "*.json")
            )
                using (StreamReader lReader = new StreamReader(filePath))
                {
                    lReturn.Add(filePath.Split('\\').Last().Split('/').Last(), lReader.ReadToEnd());
                }
            return lReturn;
        }

        public static void InitRequests()
        {
            #region User

            ServerResponse.Create("https://proxer.me/api/v1", response =>
            {
                response.Post("/user/login")
                    .WithPostArgument("username", "InfiniteSoul")
                    .WithPostArgument("password", "correct")
                    .WithLoggedInSenpai(false);
            }).Respond(JsonResponses["user_login.json"]);
            ServerResponse.Create("https://proxer.me/api/v1", response =>
            {
                response.Post("/user/login")
                    .WithPostArgument("username", "InfiniteSoul")
                    .WithPostArgument("password", "wrong")
                    .WithLoggedInSenpai(false);
            }).Respond(JsonResponses["user_login_3001.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Post("/user/logout").WithLoggedInSenpai(true))
                .Respond(JsonResponses["user_logout.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/userinfo")
                            .WithHeader("proxer-api-token",
                                "bknnY3QLb0X7CHdfHzimhOLo1Tz4cpumHSJGTzp3Gx9i2lvJSO4aCOwBH4ERr0d92UMStcU5w3kylUfdHilg7SXL" +
                                "VuCfDQtCIfsapmiXmGsFyHSeZcv45kOXOoipcL2VYt6oNni02KOApFOmRhpvCbOox7OKPPDOhIa58sc5aYCxDrRs" +
                                "Ggjgp9FWetE3gfOxXYAYoK2wID4k3UKH95XvcCgo43qkhePdanby6a5OO67OXQv4Uty74Yt6YTpf7cs")
                            .WithLoggedInSenpai(false);
                        response.Post("/user/userinfo").WithQueryParameter("uid", "177103");
                        response.Post("/user/userinfo").WithQueryParameter("username", "InfiniteSoul");
                    })
                .Respond(JsonResponses["user_userinfo177103.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/userinfo").WithQueryParameter("uid", "163825");
                        response.Post("/user/userinfo").WithQueryParameter("username", "KutoSan");
                    })
                .Respond(JsonResponses["user_userinfo163825.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/userinfo").WithQueryParameter("uid", int.MaxValue.ToString());
                        response.Post("/user/userinfo").WithQueryParameter("username", "asd");
                    })
                .Respond(JsonResponses["user_userinfo_3003.json"]);

            #endregion

            #region Messenger

            ServerResponse.Create("https://proxer.me/api/v1", response => response.Post("/messenger/constants"))
                .Respond(JsonResponses["messenger_getconstants.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/conferences")
                            .WithQueryParameter("type", "default")
                            .WithQueryParameter("p", "0")
                            .WithLoggedInSenpai(false))
                .Respond(JsonResponses["messenger_getconferences_3023.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/conferences")
                            .WithQueryParameter("type", "default")
                            .WithQueryParameter("p", "0")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_getconferences.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/messenger/newconference")
                            .WithPostArgument("text", "hello")
                            .WithPostArgument("username", "KutoSa")
                            .WithLoggedInSenpai(true);
                        response.Post("/messenger/newconference")
                            .WithPostArgument("text", "hello")
                            .WithPostArgument("username", "InfiniteSoul")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(JsonResponses["messenger_newconference_3027.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/newconference")
                            .WithPostArgument("text", "hello")
                            .WithPostArgument("username", "KutoSan")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_newconference.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/newconferencegroup")
                            .WithPostArgument("topic", "hello")
                            .WithPostArgument("users[]", "KutoSan")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_newconferencegroup.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/newconferencegroup")
                            .WithPostArgument("topic", "hello")
                            .WithPostArgument("users[]", "InfiniteSoul")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_newconferencegroup_3030.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "0")
                            .WithLoggedInSenpai(true);
                        response.Post("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "5018808")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(JsonResponses["messenger_getmessages1.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "4993930")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_getmessages2.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/conferenceinfo")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_getconferenceinfo.json"]);

            #endregion
        }

        #endregion
    }
}