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
                        response.Post("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "0")
                            .WithQueryParameter("read", "true")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["messenger_getmessagesCheck.response"]);
        }

        private static Dictionary<string, string> GetFileResponses()
        {
            Dictionary<string, string> lReturn = new Dictionary<string, string>();
            foreach (
                string filePath in
                Directory.GetFiles((AppDomain.CurrentDomain.BaseDirectory + @"\Response").Replace(@"/\", "/"), "*.response")
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
                .Respond(FileResponses["anime_getstreams.response"]);

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/anime/link")
                            .WithQueryParameter("id", "401217"))
                .Respond(FileResponses["anime_getlink.response"]);

            #endregion

            #region Info

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/entry")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getentry.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/fullentry")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getfullentry.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/entry")
                            .WithQueryParameter("id", "7834"))
                .Respond(FileResponses["info_getentry7834.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/entry")
                            .WithQueryParameter("id", "666"))
                .Respond(FileResponses["info_getentry_3007.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/fullentry")
                            .WithQueryParameter("id", "666"))
                .Respond(FileResponses["info_getfullentry_3007.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/lang")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getlang.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/lang")
                            .WithQueryParameter("id", "7834"))
                .Respond(FileResponses["info_getlang7834.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/season")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getseason.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/groups")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getgroups.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/publisher")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getpublisher.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/names")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getnames.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/gate")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getgate.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/relations")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getrelations.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/entrytags")
                            .WithQueryParameter("id", "9200"))
                .Respond(FileResponses["info_getentrytags.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/comments")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("sort", "latest"))
                .Respond(FileResponses["info_getcommentslatest.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/comments")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("sort", "rating"))
                .Respond(FileResponses["info_getcommentsrating1.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/comments")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "1")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("sort", "rating"))
                .Respond(FileResponses["info_getcommentsrating2.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/listinfo")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "22"))
                .Respond(FileResponses["info_getlistinfo9200.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/listinfo")
                            .WithQueryParameter("id", "7834")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "200"))
                .Respond(FileResponses["info_getlistinfo7834.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/setuserinfo")
                            .WithPostArgument("id", "1")
                            .WithPostArgument("type", "favor")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["info_setuserinfofavor.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/setuserinfo")
                            .WithPostArgument("id", "1")
                            .WithPostArgument("type", "finish")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["info_setuserinfofinish.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/info/setuserinfo")
                            .WithPostArgument("id", "1")
                            .WithPostArgument("type", "note")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["info_setuserinfonote.response"]);

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
                .Respond(FileResponses["list_getentrylist.response"]);
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
                .Respond(FileResponses["list_getentrysearch.response"]);

            #endregion

            #region Manga

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/manga/chapter")
                            .WithQueryParameter("id", "7834")
                            .WithQueryParameter("episode", "162")
                            .WithQueryParameter("language", "en"))
                .Respond(FileResponses["manga_getchapter.response"]);

            #endregion

            #region Media

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/media/randomheader")
                            .WithQueryParameter("style", "gray"))
                .Respond(FileResponses["media_getrandomheadergray.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/media/randomheader")
                            .WithQueryParameter("style", "black"))
                .Respond(FileResponses["media_getrandomheaderblack.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/media/randomheader")
                            .WithQueryParameter("style", "pantsu"))
                .Respond(FileResponses["media_getrandomheaderpantsu.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/media/randomheader")
                            .WithQueryParameter("style", "old_blue"))
                .Respond(FileResponses["media_getrandomheaderoldblue.response"]);
            ServerResponse.Create("https://proxer.me/api/v1", response => response.Post("/media/headerlist"))
                .Respond(FileResponses["media_getheaderlist.response"]);

            #endregion

            #region Messenger

            ServerResponse.Create("https://proxer.me/api/v1", response => response.Post("/messenger/constants"))
                .Respond(FileResponses["messenger_getconstants.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/conferences")
                            .WithQueryParameter("type", "default")
                            .WithQueryParameter("p", "0")
                            .WithLoggedInSenpai(false))
                .Respond(FileResponses["messenger_getconferences_3023.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/conferences")
                            .WithQueryParameter("type", "default")
                            .WithQueryParameter("p", "0")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_getconferences.response"]);
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
                .Respond(FileResponses["messenger_newconference_3027.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/newconference")
                            .WithPostArgument("text", "hello")
                            .WithPostArgument("username", "KutoSan")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_newconference.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/newconferencegroup")
                            .WithPostArgument("topic", "hello")
                            .WithPostArgument("users[]", "KutoSan")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_newconferencegroup.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/newconferencegroup")
                            .WithPostArgument("topic", "hello")
                            .WithPostArgument("users[]", "InfiniteSoul")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_newconferencegroup_3030.response"]);
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
                .Respond(FileResponses["messenger_getmessages1.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/messages")
                            .WithQueryParameter("conference_id", "124536")
                            .WithQueryParameter("message_id", "4993930")
                            .WithQueryParameter("read", "true")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_getmessages2.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/conferenceinfo")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_getconferenceinfo.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/report")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "a")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setreport_3025.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/report")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "Report Reason")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setreport.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setmessage")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "message")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setmessage.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setmessage")
                            .WithQueryParameter("conference_id", "124536")
                            .WithPostArgument("text", "/help")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setmessageHelp.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setblock")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setblock.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setunblock")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setunblock.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setunread")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setunread.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setfavour")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setfavour.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/messenger/setunfavour")
                            .WithQueryParameter("conference_id", "124536")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["messenger_setunfavour.response"]);

            #endregion

            #region Notifications

            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Post("/notifications/count").WithLoggedInSenpai(true))
                .Respond(FileResponses["notifications_getcount.response"]);
            ServerResponse.Create("https://proxer.me/components/com_proxer/misc",
                    response => response.Get("/notifications_misc.php").WithLoggedInSenpai(true))
                .Respond(FileResponses["notifications_misc.php.response"]);

            #endregion

            #region Ucp

            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                            response.Post("/ucp/listsum").WithQueryParameter("kat", "anime").WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_getlistsumanime.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                            response.Post("/ucp/listsum").WithQueryParameter("kat", "manga").WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_getlistsummanga.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/reminder")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_getreminderanime.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/reminder")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_getremindermanga.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/history")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "50")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_gethistory.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Post("/ucp/votes").WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_getvotes.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Post("/ucp/topten").WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_gettopten.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Post("/ucp/deletevote").WithPostArgument("id", "1").WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_deletevote.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/deletefavorite")
                            .WithPostArgument("id", "1")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_deletefavourite.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/deletereminder")
                            .WithPostArgument("id", "1")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_deletereminder.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                        response.Post("/ucp/setreminder")
                            .WithQueryParameter("id", "9200")
                            .WithQueryParameter("episode", "1")
                            .WithQueryParameter("language", "engsub")
                            .WithQueryParameter("kat", "anime")
                            .WithLoggedInSenpai(true))
                .Respond(FileResponses["ucp_setreminder9200.response"]);

            #endregion

            #region User

            ServerResponse.Create("https://proxer.me/api/v1", response =>
            {
                response.Post("/user/login")
                    .WithPostArgument("username", "InfiniteSoul")
                    .WithPostArgument("password", "correct")
                    .WithLoggedInSenpai(false);
            }).Respond(FileResponses["user_login.response"]);
            ServerResponse.Create("https://proxer.me/api/v1", response =>
            {
                response.Post("/user/login")
                    .WithPostArgument("username", "InfiniteSoul")
                    .WithPostArgument("password", "wrong")
                    .WithLoggedInSenpai(false);
            }).Respond(FileResponses["user_login_3001.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response => response.Post("/user/logout").WithLoggedInSenpai(true))
                .Respond(FileResponses["user_logout.response"]);
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
                .Respond(FileResponses["user_getuserinfo177103.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/userinfo").WithQueryParameter("uid", "163825");
                        response.Post("/user/userinfo").WithQueryParameter("username", "KutoSan");
                    })
                .Respond(FileResponses["user_getuserinfo163825.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/userinfo").WithQueryParameter("uid", "1");
                        response.Post("/user/userinfo").WithQueryParameter("username", "Username");
                    })
                .Respond(FileResponses["user_getuserinfo.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/userinfo").WithQueryParameter("uid", int.MaxValue.ToString());
                        response.Post("/user/userinfo").WithQueryParameter("username", "asd");
                    })
                .Respond(FileResponses["user_getuserinfo_3003.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/topten")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "anime")
                            .WithLoggedInSenpai(true);
                        response.Post("/user/topten")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "anime")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_gettoptenanime.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/topten")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "manga")
                            .WithLoggedInSenpai(true);
                        response.Post("/user/topten")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "manga")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_gettoptenmanga.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/comments")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0")
                            .WithLoggedInSenpai(true);
                        response.Post("/user/comments")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_getlatestcommentsanime.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/comments")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0")
                            .WithLoggedInSenpai(true);
                        response.Post("/user/comments")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "25")
                            .WithQueryParameter("length", "0")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_getlatestcommentsmanga.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/list")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true);
                        response.Post("/user/list")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "anime")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_getlistanime.response"]);
            ServerResponse.Create("https://proxer.me/api/v1",
                    response =>
                    {
                        response.Post("/user/list")
                            .WithQueryParameter("uid", "1")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true);
                        response.Post("/user/list")
                            .WithQueryParameter("username", "Username")
                            .WithQueryParameter("kat", "manga")
                            .WithQueryParameter("p", "0")
                            .WithQueryParameter("limit", "100")
                            .WithLoggedInSenpai(true);
                    })
                .Respond(FileResponses["user_getlistmanga.response"]);

            #endregion
        }

        #endregion
    }
}