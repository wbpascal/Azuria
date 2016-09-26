using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Azuria.Test.Core
{
    public class ResponseSetup
    {
        #region Methods

        private static Dictionary<string, string> GetJsonResponses()
        {
            Dictionary<string, string> lReturn = new Dictionary<string, string>();
            foreach (
                string filePath in
                Directory.GetFiles((AppDomain.CurrentDomain.BaseDirectory + @"\Response").Replace(@"/\", "/"), "*.json")
            )
                using (StreamReader lReader = new StreamReader(filePath))
                {
                    lReturn.Add(filePath.Split('\\').Last(), lReader.ReadToEnd());
                }
            return lReturn;
        }

        public static void InitRequests()
        {
            Dictionary<string, string> lJson = GetJsonResponses();

            ServerResponse.Create("https://proxer.me/api/v1", response =>
            {
                response.Post("/user/login")
                    .WithPostArgument("username", "username")
                    .WithPostArgument("password", "correct")
                    .WithSenpai(true);
            }).Respond(lJson["user_login.json"]);
            ServerResponse.Create("https://proxer.me/api/v1", response =>
            {
                response.Post("/user/login")
                    .WithPostArgument("username", "username")
                    .WithPostArgument("password", "wrong")
                    .WithSenpai(true);
            }).Respond(lJson["user_login_3001.json"]);
            ServerResponse.Create("https://proxer.me/api/v1", response => response.Post("/user/logout").WithSenpai(true))
                .Respond(lJson["user_logout.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/user/userinfo")
                            .WithHeader("proxer-api-token",
                                "bknnY3QLb0X7CHdfHzimhOLo1Tz4cpumHSJGTzp3Gx9i2lvJSO4aCOwBH4ERr0d92UMStcU5w3kylUfdHilg7SXL" +
                                "VuCfDQtCIfsapmiXmGsFyHSeZcv45kOXOoipcL2VYt6oNni02KOApFOmRhpvCbOox7OKPPDOhIa58sc5aYCxDrRs" +
                                "Ggjgp9FWetE3gfOxXYAYoK2wID4k3UKH95XvcCgo43qkhePdanby6a5OO67OXQv4Uty74Yt6YTpf7cs"))
                .Respond(lJson["user_userinfo177103.json"]);
        }

        #endregion
    }
}