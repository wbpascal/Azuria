using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;
using Proxer.API.Utilities.Net;
using RestSharp;
using Web = System.Web;

namespace Proxer.API
{
    /// <summary>
    ///     Repräsentiert einen Proxer-Benutzer
    /// </summary>
    public class User
    {
        /// <summary>
        ///     Representiert das System.
        /// </summary>
        public static User System = new User("System", -1, new Senpai());

        private readonly Senpai _senpai;
        private Uri _avatar;
        private List<User> _freunde;
        private string _info;
        private bool _online;
        private int _punkte;
        private string _rang;
        private string _status;

        internal User(string name, int userId, Senpai senpai)
        {
            this._senpai = senpai;
            this.IstInitialisiert = false;

            this.UserName = name;
            this.Id = userId;
            this.Avatar =
                new Uri(
                    "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
        }

        internal User(string name, int userId, Uri avatar, Senpai senpai)
        {
            this._senpai = senpai;
            this.IstInitialisiert = false;

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
        /// <param name="userId">Die ID des Benutzers</param>
        /// <param name="senpai">Wird benötigt um einige Eigenschaften abzurufen</param>
        public User(int userId, Senpai senpai)
        {
            this._senpai = senpai;
            this.IstInitialisiert = false;

            this.Id = userId;
            this.Avatar =
                new Uri(
                    "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
        }

        #region Properties

        /// <summary>
        ///     Gibt den Link zu dem Avatar des Benutzers zurück.
        /// </summary>
        /// <exception cref="InitializeNeededException">Wird ausgelöst, wenn das Objekt noch nicht initialisiert ist.</exception>
        /// <seealso cref="InitUser" />
        public Uri Avatar
        {
            get
            {
                if (!this.IstInitialisiert) throw new InitializeNeededException();
                return this._avatar;
            }
            private set { this._avatar = value; }
        }

        /// <summary>
        ///     Gibt die Freunde des Benutzers in einer Liste zurück.
        /// </summary>
        /// <exception cref="InitializeNeededException">Wird ausgelöst, wenn das Objekt noch nicht initialisiert ist.</exception>
        /// <seealso cref="InitUser" />
        public List<User> Freunde
        {
            get
            {
                if (!this.IstInitialisiert) throw new InitializeNeededException();
                return this._freunde;
            }
            private set { this._freunde = value; }
        }

        /// <summary>
        ///     Gibt die ID des Benutzers zurück.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gibt die Info des Benutzers als Html-Dokument zurück.
        /// </summary>
        /// <exception cref="InitializeNeededException">Wird ausgelöst, wenn das Objekt noch nicht initialisiert ist.</exception>
        /// <seealso cref="InitUser" />
        public string Info
        {
            get
            {
                if (!this.IstInitialisiert) throw new InitializeNeededException();
                return this._info;
            }
            private set { this._info = value; }
        }

        /// <summary>
        ///     Gibt an, ob das Objekt bereits Initialisiert ist.
        /// </summary>
        public bool IstInitialisiert { get; private set; }

        /// <summary>
        ///     Gibt zurück, ob der Benutzter zur Zeit online ist.
        /// </summary>
        /// <exception cref="InitializeNeededException">Wird ausgelöst, wenn das Objekt noch nicht initialisiert ist.</exception>
        /// <seealso cref="InitUser" />
        public bool Online
        {
            get
            {
                if (!this.IstInitialisiert) throw new InitializeNeededException();
                return this._online;
            }
            private set { this._online = value; }
        }

        /// <summary>
        ///     Gibt zurück, wie viele Punkte der Benutzter momentan hat.
        /// </summary>
        /// <exception cref="InitializeNeededException">Wird ausgelöst, wenn das Objekt noch nicht initialisiert ist.</exception>
        /// <seealso cref="InitUser" />
        public int Punkte
        {
            get
            {
                if (!this.IstInitialisiert) throw new InitializeNeededException();
                return this._punkte;
            }
            private set { this._punkte = value; }
        }

        /// <summary>
        ///     Gibt den Rangnamen des Benutzers zurück.
        /// </summary>
        /// <exception cref="InitializeNeededException">Wird ausgelöst, wenn das Objekt noch nicht initialisiert ist.</exception>
        /// <seealso cref="InitUser" />
        public string Rang
        {
            get
            {
                if (!this.IstInitialisiert) throw new InitializeNeededException();
                return this._rang;
            }
            private set { this._rang = value; }
        }

        /// <summary>
        ///     Gibt den Status des Benutzers zurück.
        /// </summary>
        /// <exception cref="InitializeNeededException">Wird ausgelöst, wenn das Objekt noch nicht initialisiert ist.</exception>
        /// <seealso cref="InitUser" />
        public string Status
        {
            get
            {
                if (!this.IstInitialisiert) throw new InitializeNeededException();
                return this._status;
            }
            private set { this._status = value; }
        }

        /// <summary>
        ///     Gibt den Benutzernamen des Benutzers zurück.
        /// </summary>
        public string UserName { get; private set; }

        #endregion

        #region

        /// <summary>
        ///     Initialisiert die Eigenschaften der Klasse
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        public async Task<ProxerResult> InitUser()
        {
            ProxerResult lResult;
            if (!(lResult = await this.GetMainInfo()).Success || !(lResult = await this.GetFriends()).Success ||
                !(lResult = await this.GetInfos()).Success)
            {
                return lResult;
            }

            this.IstInitialisiert = true;

            return new ProxerResult();
        }


        private async Task<ProxerResult> GetMainInfo()
        {
            if (this.Id != -1)
            {
                if (!this._senpai.LoggedIn) return new ProxerResult(new Exception[] { new NotLoggedInException(this._senpai) });
                HtmlDocument lDocument = new HtmlDocument();
                string lResponse;

                IRestResponse lResponseObject =
                    await HttpUtility.GetWebRequestResponse("https://proxer.me/user/" + this.Id + "/overview?format=raw", this._senpai.LoginCookies);
                if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                    lResponse = Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
                else return new ProxerResult(new[] { new WrongResponseException(), lResponseObject.ErrorException });

                if (string.IsNullOrEmpty(lResponse) || !Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                    return new ProxerResult(new Exception[] { new WrongResponseException() });

                try
                {
                    lDocument.LoadHtml(lResponse);

                    HtmlNodeCollection lProfileNodes =
                        lDocument.DocumentNode.SelectNodes("//table[@class='profile']");

                    this.Avatar =
                        new Uri("https://proxer.me" +
                                lProfileNodes[0].ParentNode.ParentNode.ChildNodes[1].ChildNodes[0]
                                    .Attributes["src"].Value);
                    this.Punkte =
                        Convert.ToInt32(
                            Utility.GetTagContents(lProfileNodes[0].FirstChild.InnerText, "Summe: ", " - ")[
                                0]);
                    this.Rang =
                        Utility.GetTagContents(lProfileNodes[0].FirstChild.InnerText, this.Punkte + " - ",
                            "[?]")
                            [0];
                    this.Online = lProfileNodes[0].ChildNodes[1].InnerText.Equals("Status Online");
                    this.Status = lProfileNodes[0].ChildNodes.Count == 7
                        ? lProfileNodes[0].ChildNodes[6].InnerText
                        : "";

                    this.UserName =
                        lDocument.DocumentNode.SelectNodes("//div[@id='pageMetaAjax']")[0].InnerText
                                                                                          .Split(' ')[1];

                    return new ProxerResult();
                }
                catch
                {
                    return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
                }
            }
            else
            {
                this.Status = "";
                this.Online = false;
                this.Rang = "";
                this.Punkte = -1;
                this.Avatar =
                    new Uri(
                        "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");

                return new ProxerResult();
            }
        }

        private async Task<ProxerResult> GetFriends()
        {
            if (this.Id != -1)
            {
                if (!this._senpai.LoggedIn) return new ProxerResult(new Exception[] { new NotLoggedInException(this._senpai) });

                int lSeite = 1;
                IRestResponse lResponseObject;
                string lResponse;
                HtmlDocument lDocument = new HtmlDocument();

                this.Freunde = new List<User>();

                while (
                    (lResponseObject =
                        (await HttpUtility.GetWebRequestResponse(
                            "https://proxer.me/user/" + this.Id + "/connections/" + lSeite + "?format=raw",
                            this._senpai.LoginCookies))).StatusCode == HttpStatusCode.OK && (lResponse = Web.HttpUtility.HtmlDecode(lResponseObject.Content)).Replace("\n", "")
                        .Contains(
                            "Dieser Benutzer hat bisher keine Freunde"))
                {
                    if (string.IsNullOrEmpty(lResponse) ||
                        !Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                        return new ProxerResult(new Exception[] { new WrongResponseException() });
                    if (
                        lResponse.Equals(
                            "<div class=\"inner\"><h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3></div>"))
                        break;

                    try
                    {
                        lDocument.LoadHtml(lResponse);

                        if (lDocument.ParseErrors.Any())
                            lDocument.LoadHtml(Utility.TryFixParseErrors(lResponse, lDocument.ParseErrors));

                        if (!lDocument.ParseErrors.Any())
                        {
                            HtmlNodeCollection lProfileNodes =
                                lDocument.DocumentNode.SelectNodes("//table[@id='box-table-a']");

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
                    catch
                    {
                        return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
                    }
                }

                return lResponseObject.StatusCode != HttpStatusCode.OK
                    ? new ProxerResult(new Exception[] { new WebException() })
                    : new ProxerResult();
            }

            this.Freunde = new List<User>();
            return new ProxerResult();
        }

        private async Task<ProxerResult> GetInfos()
        {
            if (this.Id != -1)
            {
                if (!this._senpai.LoggedIn)
                    return new ProxerResult(new Exception[] { new NotLoggedInException(this._senpai) });
                HtmlDocument lDocument = new HtmlDocument();
                string lResponse;

                IRestResponse lResponseObject =
                    await HttpUtility.GetWebRequestResponse("https://proxer.me/user/" + this.Id + "/about?format=raw", this._senpai.LoginCookies);
                if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                    lResponse = Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
                else return new ProxerResult(new[] { new WrongResponseException(), lResponseObject.ErrorException });

                if (string.IsNullOrEmpty(lResponse) || !Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                    return new ProxerResult(new Exception[] { new WrongResponseException() });

                try
                {
                    lDocument.LoadHtml(lResponse);

                    HtmlNodeCollection lProfileNodes =
                        lDocument.DocumentNode.SelectNodes("//table[@class='profile']");

                    if (lProfileNodes != null)
                    {
                        this.Info = lProfileNodes[0].ChildNodes[10].InnerText;
                    }
                }
                catch
                {
                    return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
                }
            }
            else
            {
                this.Info = "";
            }

            return new ProxerResult();
        }


        /*
         * -----------------------------------
         * --------Statische Methoden---------
         * -----------------------------------
        */

        /// <summary>
        ///     Überprüft, ob zwei Benutzter Freunde sind.
        /// </summary>
        /// <param name="user1">Benutzer 1</param>
        /// <param name="user2">Benutzer 2</param>
        /// <returns>Benutzer sind Freunde. True oder False.</returns>
        /// <exception cref="InitializeNeededException">Wird ausgelöst, wenn das Objekt noch nicht initialisiert ist.</exception>
        public static bool IsUserFriendOf(User user1, User user2)
        {
            return user1.Freunde.Any(item => item.Id == user2.Id);
        }

        /// <summary>
        ///     Gibt den Benutzernamen eines Benutzers mit der spezifizierten ID zurück.
        /// </summary>
        /// <param name="id">Die ID des Benutzers</param>
        /// <param name="senpai">Login-Cookies werden benötigt</param>
        /// <seealso cref="Senpai.Login" />
        /// <returns></returns>
        public static async Task<ProxerResult<string>> GetUNameFromId(int id, Senpai senpai)
        {
            if (!senpai.LoggedIn) return new ProxerResult<string>(new Exception[] { new NotLoggedInException() });

            HtmlDocument lDocument = new HtmlDocument();
            string lResponse;

            IRestResponse lResponseObject =
                await HttpUtility.GetWebRequestResponse("https://proxer.me/user/" + id + "/overview?format=raw", senpai.LoginCookies);
            if (lResponseObject.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(lResponseObject.Content))
                lResponse = Web.HttpUtility.HtmlDecode(lResponseObject.Content).Replace("\n", "");
            else return new ProxerResult<string>(new[] { new WrongResponseException(), lResponseObject.ErrorException });

            if (string.IsNullOrEmpty(lResponse) || !Utility.CheckForCorrectResponse(lResponse, senpai.ErrHandler))
                return new ProxerResult<string>(new Exception[] { new WrongResponseException() });

            try
            {
                lDocument.LoadHtml(lResponse);
                HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//div[@id='pageMetaAjax']");
                return new ProxerResult<string>(lNodes[0].InnerText.Split(' ')[1]);
            }
            catch
            {
                return new ProxerResult<string>((await ErrorHandler.HandleError(senpai, lResponse, false)).Exceptions);
            }
        }

        #endregion
    }
}