using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Azuria.Test.Core
{
    public class ResponseSetup
    {
        private static readonly Dictionary<string, string> FileResponses = GetFileResponses();

        #region Methods

        public static ServerResponse CreateForMessageAutoCheck()
        {
            return ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "0")
                            .WithQueryParameter("read", "true")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["messenger_getmessagesCheck.json"]);
        }

        private static Dictionary<string, string> GetFileResponses()
        {
            Dictionary<string, string> lReturn = new Dictionary<string, string>();
            foreach (string filePath in Directory.GetFiles(
                (AppDomain.CurrentDomain.BaseDirectory + @"\Response").Replace(@"/\", "/"), "*.*"))
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
                        response.Get("/anime/streams")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("episode", "1")
                            .WithQueryParameter("language", "engsub"))
                .Respond(FileResponses["anime_getstreams.json"]);

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/anime/link")
                            .WithQueryParameter("id", "401217"))
                .Respond(FileResponses["anime_getlink.json"]);

            #endregion

            #region Info

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/entry")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getentry.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/fullentry")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getfullentry.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/entry")
                            .WithQueryParameter("id", "7834"))
                .Respond(FileResponses["info_getentry7834.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/entry")
                            .WithQueryParameter("id", "666"))
                .Respond(FileResponses["info_getentry_3007.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/fullentry")
                            .WithQueryParameter("id", "666"))
                .Respond(FileResponses["info_getfullentry_3007.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/lang")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getlang.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/lang")
                            .WithQueryParameter("id", "7834"))
                .Respond(FileResponses["info_getlang7834.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/season")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getseason.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/groups")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getgroups.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/publisher")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getpublisher.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/names")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getnames.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/gate")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getgate.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/relations")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getrelations.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/entrytags")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getentrytags.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/comments")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("sort", "latest"))
                .Respond(FileResponses["info_getcommentslatest.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/comments")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("sort", "rating"))
                .Respond(FileResponses["info_getcommentsrating1.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/comments")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "1")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("sort", "rating"))
                .Respond(FileResponses["info_getcommentsrating2.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/listinfo")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "22"))
                .Respond(FileResponses["info_getlistinfo9200.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/info/listinfo")
                            .WithQueryParameter("id", "7834")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "200"))
                .Respond(FileResponses["info_getlistinfo7834.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/setuserinfo")
                            .WithPostArgument("id", "1")
                            .WithPostArgument("type", "favor")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["info_setuserinfofavor.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/setuserinfo")
                            .WithPostArgument("id", "1")
                            .WithPostArgument("type", "finish")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["info_setuserinfofinish.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/setuserinfo")
                            .WithPostArgument("id", "1")
                            .WithPostArgument("type", "note")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["info_setuserinfonote.json"]);

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
                .Respond(FileResponses["list_getentrylist.json"]);
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
                .Respond(FileResponses["list_getentrysearch.json"]);

            #endregion

            #region Manga

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/manga/chapter")
                            .WithQueryParameter("id", "7834")
                            .WithQueryParameter("episode", "162")
                            .WithQueryParameter("language", "en"))
                .Respond(FileResponses["manga_getchapter.json"]);

            #endregion

            #region Media

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/media/randomheader")
                            .WithQueryParameter("style", "gray"))
                .Respond(FileResponses["media_getrandomheadergray.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/media/randomheader")
                            .WithQueryParameter("style", "black"))
                .Respond(FileResponses["media_getrandomheaderblack.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/media/randomheader")
                            .WithQueryParameter("style", "pantsu"))
                .Respond(FileResponses["media_getrandomheaderpantsu.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/media/randomheader")
                            .WithQueryParameter("style", "old_blue"))
                .Respond(FileResponses["media_getrandomheaderoldblue.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/media/headerlist"))
                .Respond(FileResponses["media_getheaderlist.json"]);

            #endregion

            #region Messenger

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/messenger/constants"))
                .Respond(FileResponses["messenger_getconstants.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/messenger/conferences")
                            .WithQueryParameter("type", "default")
                            .WithQueryParameter("p", "0")
                            .WithLoggedInSenpai(false))
                .Respond(FileResponses["messenger_getconferences_3023.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/messenger/conferences")
                            .WithQueryParameter("type", "default")
                            .WithQueryParameter("p", "0")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_getconferences.json"]);
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
                .Respond(FileResponses["messenger_newconference_3027.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/newconference")
                            .WithPostArgument("text", "hello")
                            .WithPostArgument("username", "KutoSan")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_newconference.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/newconferencegroup")
                            .WithPostArgument("topic", "hello")
                            .WithPostArgument("users[]", "KutoSan")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_newconferencegroup.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/newconferencegroup")
                            .WithPostArgument("topic", "hello")
                            .WithPostArgument("users[]", "InfiniteSoul")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_newconferencegroup_3030.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "0")
                            .WithQueryParameter("read", "true")
                            .WithLoggedInSenpai(true);
                        response.Get("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "5018808")
                            .WithQueryParameter("read", "true")
                            .WithLoggedInSenpai(true);
                        response.Get("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "0")
                            .WithQueryParameter("read", "false")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["messenger_getmessages1.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "4993930")
                            .WithQueryParameter("read", "true")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_getmessages2.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/messenger/conferenceinfo")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_getconferenceinfo.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/report")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "a")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setreport_3025.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/report")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "Report Reason")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setreport.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setmessage")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "message")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setmessage.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setmessage")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "/help")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setmessageHelp.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/messenger/setblock")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setblock.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/messenger/setunblock")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setunblock.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/messenger/setunread")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setunread.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/messenger/setfavour")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setfavour.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/messenger/setunfavour")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setunfavour.json"]);
            ServerResponse.Create("https://proxer.me",
                    response =>
                        response.Get("/messages")
                            .WithQueryParameter("format", "raw")
                            .WithQueryParameter("s", "notification")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messages_notifications.html"]);

            #endregion

            #region Notifications

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/notifications/count")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["notifications_getcount.json"]);
            ServerResponse.Create("https://proxer.me/api/v1", response =>
                    response.Get("/notifications/news")
                        .WithQueryParameter("p", "0")
                        .WithQueryParameter("limit", "15"))
                .Respond(FileResponses["notifications_getnews.json"]);
            ServerResponse.Create("https://proxer.me/components/com_proxer/misc",
                    response =>
                        response.Get("/notifications_misc.php")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["notifications_misc.php"]);

            #endregion

            #region Ucp

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/ucp/listsum")
                            .WithQueryParameter("kat", "anime")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_getlistsumanime.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/ucp/listsum")
                            .WithQueryParameter("kat", "manga")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_getlistsummanga.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/ucp/reminder")
                            .WithQueryParameter("kat", "")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_getreminder.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/ucp/reminder")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_getreminderanime.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/ucp/reminder")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_getremindermanga.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/ucp/history")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "50")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_gethistory.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Get("/ucp/votes")
                        .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_getvotes.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Get("/ucp/topten")
                        .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_gettopten.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Post("/ucp/deletevote")
                        .WithPostArgument("id", "1")
                        .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_deletevote.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/deletefavorite")
                            .WithPostArgument("id", "1")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_deletefavourite.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/deletereminder")
                            .WithPostArgument("id", "1")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_deletereminder.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Get("/ucp/setreminder")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("episode", "1")
                            .WithQueryParameter("language", "engsub")
                            .WithQueryParameter("kat", "anime")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_setreminder9200.json"]);

            #endregion

            #region User

            ServerResponse.Create("https://proxer.me/api/v1", response =>
            {
                response.Post("/user/login")
                    .WithPostArgument("username", "InfiniteSoul")
                    .WithPostArgument("password", "correct")
                    .WithLoggedInSenpai(false);
            }).Respond(FileResponses["user_login.json"]);
            ServerResponse.Create("https://proxer.me/api/v1", response =>
            {
                response.Post("/user/login")
                    .WithPostArgument("username", "InfiniteSoul")
                    .WithPostArgument("password", "wrong")
                    .WithLoggedInSenpai(false);
            }).Respond(FileResponses["user_login_3001.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Get("/user/logout")
                        .WithLoggedInSenpai(true))
                .Respond(FileResponses["user_logout.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/user/userinfo")
                            .WithHeader("proxer-api-token",
                                "bknnY3QLb0X7CHdfHzimhOLo1Tz4cpumHSJGTzp3Gx9i2lvJSO4aCOwBH4ERr0d92UMStcU5w3kylUfdHilg7SXL" +
                                "VuCfDQtCIfsapmiXmGsFyHSeZcv45kOXOoipcL2VYt6oNni02KOApFOmRhpvCbOox7OKPPDOhIa58sc5aYCxDrRs" +
                                "Ggjgp9FWetE3gfOxXYAYoK2wID4k3UKH95XvcCgo43qkhePdanby6a5OO67OXQv4Uty74Yt6YTpf7cs")
                            .WithLoggedInSenpai(false);
                        response.Get("/user/userinfo")
                            .WithQueryParameter("uid", "177103");
                        response.Get("/user/userinfo")
                            .WithQueryParameter("username", "InfiniteSoul");
                    })
                .Respond(FileResponses["user_getuserinfo177103.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/user/userinfo")
                            .WithQueryParameter("uid", "163825");
                        response.Get("/user/userinfo")
                            .WithQueryParameter("username", "KutoSan");
                    })
                .Respond(FileResponses["user_getuserinfo163825.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/user/userinfo")
                            .WithQueryParameter("uid", "1");
                        response.Get("/user/userinfo")
                            .WithQueryParameter("username", "Username");
                    })
                .Respond(FileResponses["user_getuserinfo.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/user/userinfo")
                            .WithQueryParameter("uid", int.MaxValue.ToString());
                        response.Get("/user/userinfo")
                            .WithQueryParameter("username", "asd");
                    })
                .Respond(FileResponses["user_getuserinfo_3003.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/user/topten")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "anime")
                            .WithLoggedInSenpai(true);
                        response.Get("/user/topten")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "anime")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_gettoptenanime.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/user/topten")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "manga")
                            .WithLoggedInSenpai(true);
                        response.Get("/user/topten")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "manga")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_gettoptenmanga.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/user/comments")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0")
                            .WithLoggedInSenpai(true);
                        response.Get("/user/comments")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_getlatestcommentsanime.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/user/comments")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0")
                            .WithLoggedInSenpai(true);
                        response.Get("/user/comments")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_getlatestcommentsmanga.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/user/list")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true);
                        response.Get("/user/list")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_getlistanime.json"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Get("/user/list")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true);
                        response.Get("/user/list")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_getlistmanga.json"]);

            #endregion
        }

        #endregion
    }
}