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
                            .WithQueryParameter("read", "true")
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
            #region Anime

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/anime/streams")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("episode", "1")
                            .WithQueryParameter("language", "engsub"))
                .Respond(JsonResponses["anime_getstreams.json"]);

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/anime/link")
                            .WithQueryParameter("id", "401217"))
                .Respond(JsonResponses["anime_getlink.json"]);

            #endregion

            #region Info

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/entry")
                            .WithQueryParameter("id", "9200"))
                .Respond(JsonResponses["info_getentry.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/entry")
                            .WithQueryParameter("id", "7834"))
                .Respond(JsonResponses["info_getentry7834.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/entry")
                            .WithQueryParameter("id", "666"))
                .Respond(JsonResponses["info_getentry_3007.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/lang")
                            .WithQueryParameter("id", "9200"))
                .Respond(JsonResponses["info_getlang.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/lang")
                            .WithQueryParameter("id", "7834"))
                .Respond(JsonResponses["info_getlang7834.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/season")
                            .WithQueryParameter("id", "9200"))
                .Respond(JsonResponses["info_getseason.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/groups")
                            .WithQueryParameter("id", "9200"))
                .Respond(JsonResponses["info_getgroups.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/publisher")
                            .WithQueryParameter("id", "9200"))
                .Respond(JsonResponses["info_getpublisher.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/names")
                            .WithQueryParameter("id", "9200"))
                .Respond(JsonResponses["info_getnames.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/gate")
                            .WithQueryParameter("id", "9200"))
                .Respond(JsonResponses["info_getgate.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/relations")
                            .WithQueryParameter("id", "9200"))
                .Respond(JsonResponses["info_getrelations.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/entrytags")
                            .WithQueryParameter("id", "9200"))
                .Respond(JsonResponses["info_getentrytags.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/comments")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("sort", "latest"))
                .Respond(JsonResponses["info_getcommentslatest.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/comments")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("sort", "rating"))
                .Respond(JsonResponses["info_getcommentsrating1.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/comments")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "1")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("sort", "rating"))
                .Respond(JsonResponses["info_getcommentsrating2.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/listinfo")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "22"))
                .Respond(JsonResponses["info_getlistinfo9200.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/listinfo")
                            .WithQueryParameter("id", "7834")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "200"))
                .Respond(JsonResponses["info_getlistinfo7834.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/setuserinfo")
                            .WithPostArgument("id", "1")
                            .WithPostArgument("type", "favor")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["info_setuserinfofavor.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/setuserinfo")
                            .WithPostArgument("id", "1")
                            .WithPostArgument("type", "finish")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["info_setuserinfofinish.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/setuserinfo")
                            .WithPostArgument("id", "1")
                            .WithPostArgument("type", "note")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["info_setuserinfonote.json"]);

            #endregion

            #region List

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/list/entrylist")
                            .WithQueryParameter("limit", "100")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("kat", "manga")
                            .WithPostArgument("isH", "False")
                            .WithPostArgument("start", "nonAlpha")
                            .WithPostArgument("medium", "mangaseries"))
                .Respond(JsonResponses["list_getentrylist.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/list/entrysearch")
                            .WithQueryParameter("limit", "100")
                            .WithQueryParameter("p", "0")
                            .WithPostArgument("type", "all")
                            .WithPostArgument("sort", "name")
                            .WithPostArgument("length-limit", "down")
                            .WithPostArgument("tagratefilter", "rate_1")
                            .WithPostArgument("tagspoilerfilter", "spoiler_0")
                            .WithPostArgument("language", "en")
                            .WithPostArgument("genre", "Action")
                            .WithPostArgument("nogenre", "Ecchi")
                            .WithPostArgument("fsk", "fsk12")
                            .WithPostArgument("length", "50")
                            .WithPostArgument("tags", "243")
                            .WithPostArgument("notags", "157")
                            .WithPostArgument("name", "a"))
                .Respond(JsonResponses["list_getentrysearch.json"]);

            #endregion

            #region Manga

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/manga/chapter")
                            .WithQueryParameter("id", "7834")
                            .WithQueryParameter("episode", "162")
                            .WithQueryParameter("language", "en"))
                .Respond(JsonResponses["manga_getchapter.json"]);

            #endregion

            #region Media

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/media/randomheader")
                            .WithQueryParameter("style", "gray"))
                .Respond(JsonResponses["media_getrandomheadergray.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/media/randomheader")
                            .WithQueryParameter("style", "black"))
                .Respond(JsonResponses["media_getrandomheaderblack.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/media/randomheader")
                            .WithQueryParameter("style", "pantsu"))
                .Respond(JsonResponses["media_getrandomheaderpantsu.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/media/randomheader")
                            .WithQueryParameter("style", "old_blue"))
                .Respond(JsonResponses["media_getrandomheaderoldblue.json"]);
            ServerResponse.Create("https://proxer.me/api/v1", response => response.Post("/media/headerlist"))
                .Respond(JsonResponses["media_getheaderlist.json"]);

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
                            .WithQueryParameter("read", "true")
                            .WithLoggedInSenpai(true);
                        response.Post("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "5018808")
                            .WithQueryParameter("read", "true")
                            .WithLoggedInSenpai(true);
                        response.Post("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "0")
                            .WithQueryParameter("read", "false")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(JsonResponses["messenger_getmessages1.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "4993930")
                            .WithQueryParameter("read", "true")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_getmessages2.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/conferenceinfo")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_getconferenceinfo.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/report")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "a")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_setreport_3025.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/report")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "Report Reason")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_setreport.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setmessage")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "message")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_setmessage.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setmessage")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "/help")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_setmessageHelp.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setblock")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_setblock.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setunblock")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_setunblock.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setunread")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_setunread.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setfavour")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_setfavour.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setunfavour")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["messenger_setunfavour.json"]);

            #endregion

            #region Ucp

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                            response.Post("/ucp/listsum").WithQueryParameter("kat", "anime").WithLoggedInSenpai(true))
                .Respond(JsonResponses["ucp_getlistsumanime.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                            response.Post("/ucp/listsum").WithQueryParameter("kat", "manga").WithLoggedInSenpai(true))
                .Respond(JsonResponses["ucp_getlistsummanga.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/reminder")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["ucp_getreminderanime.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/reminder")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["ucp_getremindermanga.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/history")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "50")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["ucp_gethistory.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Post("/ucp/votes").WithLoggedInSenpai(true))
                .Respond(JsonResponses["ucp_getvotes.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Post("/ucp/topten").WithLoggedInSenpai(true))
                .Respond(JsonResponses["ucp_gettopten.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Post("/ucp/deletevote").WithPostArgument("id", "1").WithLoggedInSenpai(true))
                .Respond(JsonResponses["ucp_deletevote.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/deletefavorite")
                            .WithPostArgument("id", "1")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["ucp_deletefavourite.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/deletereminder")
                            .WithPostArgument("id", "1")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["ucp_deletereminder.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/setreminder")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("episode", "1")
                            .WithQueryParameter("language", "engsub")
                            .WithQueryParameter("kat", "anime")
                            .WithLoggedInSenpai(true))
                .Respond(JsonResponses["ucp_setreminder9200.json"]);

            #endregion

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
                .Respond(JsonResponses["user_getuserinfo177103.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/userinfo").WithQueryParameter("uid", "163825");
                        response.Post("/user/userinfo").WithQueryParameter("username", "KutoSan");
                    })
                .Respond(JsonResponses["user_getuserinfo163825.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/userinfo").WithQueryParameter("uid", "1");
                        response.Post("/user/userinfo").WithQueryParameter("username", "Username");
                    })
                .Respond(JsonResponses["user_getuserinfo.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/userinfo").WithQueryParameter("uid", int.MaxValue.ToString());
                        response.Post("/user/userinfo").WithQueryParameter("username", "asd");
                    })
                .Respond(JsonResponses["user_getuserinfo_3003.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/topten").WithQueryParameter("uid", "1").WithQueryParameter("kat", "anime");
                        response.Post("/user/topten")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "anime");
                    })
                .Respond(JsonResponses["user_gettoptenanime.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/topten").WithQueryParameter("uid", "1").WithQueryParameter("kat", "manga");
                        response.Post("/user/topten")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "manga");
                    })
                .Respond(JsonResponses["user_gettoptenmanga.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/comments")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0");
                        response.Post("/user/comments")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0");
                    })
                .Respond(JsonResponses["user_getlatestcommentsanime.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/comments")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0");
                        response.Post("/user/comments")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0");
                    })
                .Respond(JsonResponses["user_getlatestcommentsmanga.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/list")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100");
                        response.Post("/user/list")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100");
                    })
                .Respond(JsonResponses["user_getlistanime.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/list")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100");
                        response.Post("/user/list")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100");
                    })
                .Respond(JsonResponses["user_getlistmanga.json"]);

            #endregion
        }

        #endregion
    }
}