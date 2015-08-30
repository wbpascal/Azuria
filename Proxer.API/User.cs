using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Proxer.API.Utilities;

namespace Proxer.API
{
    /// <summary>
    ///     Repräsentiert einen Proxer-Benutzer
    /// </summary>
    public class User
    {
        /// <summary>
        ///     Representiert das System
        /// </summary>
        public static User System = new User("System", -1, new Senpai());

        private readonly Senpai _senpai;

        internal User(string name, int userId, Senpai senpai)
        {
            this._senpai = senpai;

            this.UserName = name;
            this.Id = userId;
            this.Avatar =
                new Uri(
                    "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
        }

        internal User(string name, int userId, Uri avatar, Senpai senpai)
        {
            this._senpai = senpai;

            this.UserName = name;
            this.Id = userId;
            if (avatar != null) this.Avatar = avatar;
            else
                this.Avatar =
                    new Uri(
                        "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
        }

        /// <summary>
        ///     Initialisiert die Klasse mit allen Standardeinstellungen.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="senpai"></param>
        public User(int userId, Senpai senpai)
        {
            this._senpai = senpai;

            this.UserName = GetUNameFromId(userId, senpai);
            this.Id = userId;
            this.Avatar =
                new Uri(
                    "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
        }


        /// <summary>
        ///     Gibt den Link zu dem Avatar des Benutzers zurück
        /// </summary>
        public Uri Avatar { get; private set; }

        /// <summary>
        ///     Gibt die Freunde des Benutzers in einer Liste zurück
        /// </summary>
        public List<User> Freunde { get; private set; }

        /// <summary>
        ///     Gibt die Freunde des Benutzers in einer Liste zurück
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        ///     Gibt die Info des Benutzers als Html-Dokument zurück
        /// </summary>
        public string Info { get; private set; }

        /// <summary>
        ///     Gibt zurück, ob der Benutzter zur Zeit online ist
        /// </summary>
        public bool Online { get; private set; }

        /// <summary>
        ///     Gibt zurück, wie viele Punkte der Benutzter momentan hat
        /// </summary>
        public int Punkte { get; private set; }

        /// <summary>
        ///     Gibt den Rang-namen des Benutzers zurück
        /// </summary>
        public string Rang { get; private set; }

        /// <summary>
        ///     Gibt den Status des Benutzers zurück
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        ///     Gibt den Benutzernamen des Benutzers zurück
        /// </summary>
        public string UserName { get; private set; }


        /// <summary>
        ///     Initialisiert die Eigenschaften der Klasse
        /// </summary>
        public void InitUser()
        {
            this.GetMainInfo();
            this.GetFriends();
            this.GetInfos();
        }


        private void GetMainInfo()
        {
            if (this.Id == -1)
            {
                this.Status = "";
                this.Online = false;
                this.Rang = "";
                this.Punkte = -1;
                this.Avatar =
                    new Uri(
                        "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
            }
            else if (this._senpai.LoggedIn)
            {
                HtmlDocument lDocument = new HtmlDocument();
                string lResponse =
                    (HttpUtility.GetWebRequestResponse("https://proxer.me/user/" + this.Id + "/overview?format=raw", this._senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "");

                if (Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                {
                    try
                    {
                        lDocument.LoadHtml(lResponse);

                        if (lDocument.ParseErrors.Count() == 0)
                        {
                            HtmlNodeCollection lProfileNodes = lDocument.DocumentNode.SelectNodes("//table[@class='profile']");

                            if (lProfileNodes != null)
                            {
                                this.Avatar =
                                    new Uri("https://proxer.me" +
                                            lProfileNodes[0].ParentNode.ParentNode.ChildNodes[1].ChildNodes[0]
                                                .Attributes["src"].Value);
                                this.Punkte =
                                    Convert.ToInt32(
                                        Utility.GetTagContents(lProfileNodes[0].FirstChild.InnerText, "Summe: ", " - ")[
                                            0]);
                                this.Rang =
                                    Utility.GetTagContents(lProfileNodes[0].FirstChild.InnerText, this.Punkte + " - ", "[?]")
                                        [0];
                                this.Online = lProfileNodes[0].ChildNodes[1].InnerText.Equals("Status Online");
                                if (lProfileNodes[0].ChildNodes.Count() == 7)
                                    this.Status = lProfileNodes[0].ChildNodes[6].InnerText;
                                else this.Status = "";

                                if (this.UserName.Equals(""))
                                {
                                    this.UserName =
                                        lDocument.DocumentNode.SelectNodes("//div[@id='pageMetaAjax']")[0].InnerText
                                            .Split(' ')[1];
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        this._senpai.ErrHandler.Add(lResponse);
                    }
                }
            }
        }

        private void GetFriends()
        {
            if (this.Id == -1)
            {
                this.Freunde = new List<User>();
            }
            else if (this._senpai.LoggedIn)
            {
                int lSeite = 1;
                string lResponse;
                HtmlDocument lDocument = new HtmlDocument();

                this.Freunde = new List<User>();

                while (
                    !(lResponse =
                        (HttpUtility.GetWebRequestResponse(
                            "https://proxer.me/user/" + this.Id + "/connections/" + lSeite + "?format=raw", this._senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "").Replace("\t", "")).Contains(
                                "Dieser Benutzer hat bisher keine Freunde"))
                {
                    if (Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                    {
                        if (
                            lResponse.Equals(
                                "<div class=\"inner\"><h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3></div>"))
                            break;

                        try
                        {
                            lDocument.LoadHtml(lResponse);

                            if (lDocument.ParseErrors.Count() > 0)
                                lDocument.LoadHtml(Utility.TryFixParseErrors(lResponse, lDocument.ParseErrors));

                            if (lDocument.ParseErrors.Count() == 0)
                            {
                                HtmlNodeCollection lProfileNodes = lDocument.DocumentNode.SelectNodes("//table[@id='box-table-a']");

                                if (lProfileNodes != null)
                                {
                                    lProfileNodes[0].ChildNodes.Remove(0);
                                    foreach (HtmlNode curFriendNode in lProfileNodes[0].ChildNodes)
                                    {
                                        string lUsername = curFriendNode.ChildNodes[2].InnerText;
                                        int lId =
                                            Convert.ToInt32(
                                                curFriendNode.Attributes["id"].Value.Substring("entry".Length));
                                        this.Freunde.Add(new User(lUsername, lId, this._senpai));
                                    }
                                }
                            }

                            lSeite++;
                        }
                        catch (Exception)
                        {
                            this._senpai.ErrHandler.Add(lResponse);
                        }
                    }
                }
            }
        }

        private void GetInfos()
        {
            if (this.Id == -1)
            {
                this.Info = "";
            }
            else if (this._senpai.LoggedIn)
            {
                HtmlDocument lDocument = new HtmlDocument();
                string lResponse =
                    (HttpUtility.GetWebRequestResponse("https://proxer.me/user/" + this.Id + "/about?format=raw", this._senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "");

                if (Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                {
                    try
                    {
                        lDocument.LoadHtml(lResponse);

                        if (lDocument.ParseErrors.Count() == 0)
                        {
                            HtmlNodeCollection lProfileNodes = lDocument.DocumentNode.SelectNodes("//table[@class='profile']");

                            if (lProfileNodes != null)
                            {
                                this.Info = lProfileNodes[0].ChildNodes[10].InnerText;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        this._senpai.ErrHandler.Add(lResponse);
                    }
                }
            }
        }


        /*
         * -----------------------------------
         * --------Statische Methoden---------
         * -----------------------------------
        */

        /// <summary>
        ///     Überprüft, ob zwei Benutzter Freunde sind.
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        public static bool IsUserFriendOf(User user1, User user2)
        {
            return user1.Freunde.Any(item => item.Id == user2.Id);
        }

        /// <summary>
        ///     Gibt den Benutzernamen eines Benutzers mit der spezifizierten ID zurück
        /// </summary>
        /// <param name="id"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static string GetUNameFromId(int id, Senpai senpai)
        {
            HtmlDocument lDocument = new HtmlDocument();
            string lResponse = HttpUtility.GetWebRequestResponse("https://proxer.me/user/" + id + "/overview?format=raw",
                senpai.LoginCookies);

            if (Utility.CheckForCorrectResponse(lResponse, senpai.ErrHandler))
            {
                try
                {
                    lDocument.LoadHtml(lResponse);

                    if (lDocument.ParseErrors.Count() == 0)
                    {
                        HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//div[@id='pageMetaAjax']");
                        if (lNodes != null) return lNodes[0].InnerText.Split(' ')[1];
                        return "";
                    }
                    return "";
                }
                catch (Exception)
                {
                    senpai.ErrHandler.Add(lResponse);
                }
            }

            return "";
        }
    }
}