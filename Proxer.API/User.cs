using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Proxer.API.Exceptions;
using Proxer.API.Main;
using Proxer.API.Main.User;
using Proxer.API.Utilities;
using Proxer.API.Utilities.Net;

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

        private readonly Func<Task<ProxerResult>>[] _initFuncs;

        private readonly Senpai _senpai;
        private List<KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>> _animeList;
        private Uri _avatar;
        private IEnumerable<Anime> _favouritenAnime;
        private IEnumerable<Manga> _favouritenManga;
        private List<User> _freunde;
        private string _info;
        private string _infoHtml;
        private List<KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>> _mangaList;
        private string _rang;
        private string _status;
        private string _userName;

        internal User(string name, int userId, Senpai senpai)
        {
            this._senpai = senpai;
            this._initFuncs = new Func<Task<ProxerResult>>[]
            {this.InitMainInfo, this.InitInfos, this.InitFriends, this.InitAnime, this.InitManga};
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
            this._initFuncs = new Func<Task<ProxerResult>>[]
            {this.InitMainInfo, this.InitInfos, this.InitFriends, this.InitAnime, this.InitManga};
            this.IstInitialisiert = false;

            this.UserName = name;
            this.Id = userId;
            if (avatar != null) this.Avatar = avatar;
            else
                this.Avatar =
                    new Uri(
                        "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
        }

        internal User(string name, int userId, Uri avatar, bool online, Senpai senpai)
        {
            this._senpai = senpai;
            this._initFuncs = new Func<Task<ProxerResult>>[]
            {this.InitMainInfo, this.InitInfos, this.InitFriends, this.InitAnime, this.InitManga};
            this.IstInitialisiert = false;

            this.UserName = name;
            this.Id = userId;
            this.Online = online;
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
            this._initFuncs = new Func<Task<ProxerResult>>[]
            {this.InitMainInfo, this.InitInfos, this.InitFriends, this.InitAnime, this.InitManga};
            this.IstInitialisiert = false;

            this.Id = userId;
            this.Avatar =
                new Uri(
                    "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
        }

        #region Properties

        /// <summary>
        ///     Gibt alle <see cref="Anime">Anime</see> zurück, die der <see cref="User">Benutzer</see>
        ///     in seinem Profil markiert hat.
        /// </summary>
        public IEnumerable<KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>> Anime
            => this._animeList ??
               new List<KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>>();

        /// <summary>
        ///     Gibt den Link zu dem Avatar des Benutzers zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public Uri Avatar
        {
            get
            {
                return this._avatar ??
                       new Uri(
                           "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
            }
            private set { this._avatar = value; }
        }

        /// <summary>
        ///     Gibt die Anime-Favouriten des Benutzers zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public IEnumerable<Anime> FavouritenAnime
        {
            get { return this._favouritenAnime ?? new Anime[0]; }
            private set { this._favouritenAnime = value; }
        }

        /// <summary>
        ///     Gibt die Manga-Favouriten des Benutzers zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public IEnumerable<Manga> FavouritenManga
        {
            get { return this._favouritenManga ?? new Manga[0]; }
            private set { this._favouritenManga = value; }
        }

        /// <summary>
        ///     Gibt die Freunde des Benutzers in einer Liste zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public List<User> Freunde
        {
            get { return this._freunde ?? new List<User>(); }
            private set { this._freunde = value; }
        }

        /// <summary>
        ///     Gibt die ID des Benutzers zurück.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gibt die Info des Benutzers als Text-Dokument zurück.
        ///     Dabei werden sämtliche Html-Eigenschaften ignoriert.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public string Info
        {
            get { return this._info ?? ""; }
            private set { this._info = value; }
        }

        /// <summary>
        ///     Gibt die Info des Benutzers als Html-Dokument zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public string InfoHtml
        {
            get { return this._infoHtml ?? ""; }
            private set { this._infoHtml = value; }
        }

        /// <summary>
        ///     Gibt an, ob das Objekt bereits Initialisiert ist.
        /// </summary>
        public bool IstInitialisiert { get; private set; }

        /// <summary>
        ///     Gibt alle <see cref="Manga">Manga</see> zurück, die der <see cref="User">Benutzer</see>
        ///     in seinem Profil markiert hat.
        /// </summary>
        public IEnumerable<KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>> Manga
            => this._mangaList ??
               new List<KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>>();

        /// <summary>
        ///     Gibt zurück, ob der Benutzter zur Zeit online ist.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public bool Online { get; private set; }

        /// <summary>
        ///     Gibt zurück, wie viele Punkte der Benutzter momentan hat.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public int Punkte { get; private set; }

        /// <summary>
        ///     Gibt den Rangnamen des Benutzers zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public string Rang
        {
            get { return this._rang ?? ""; }
            private set { this._rang = value; }
        }

        /// <summary>
        ///     Gibt den Status des Benutzers zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public string Status
        {
            get { return this._status ?? ""; }
            private set { this._status = value; }
        }

        /// <summary>
        ///     Gibt den Benutzernamen des Benutzers zurück.
        /// </summary>
        public string UserName
        {
            get { return this._userName ?? ""; }
            private set { this._userName = value; }
        }

        #endregion

        #region

        /// <summary>
        ///     Initialisiert die Eigenschaften der Klasse
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="WrongResponseException" /> wird ausgelöst, wenn die Antwort des Servers nicht der
        ///                 Erwarteten entspricht.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="NoAccessException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="NoAccessException" /> wird ausgelöst, wenn Teile der Initialisierung nicht durchgeführt
        ///                 werden können,
        ///                 da der <see cref="Senpai">Benutzer</see> nicht die nötigen Rechte dafür hat.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        public async Task<ProxerResult> Init()
        {
            int lFailedInits = 0;
            ProxerResult lReturn = new ProxerResult();

            foreach (Func<Task<ProxerResult>> initFunc in this._initFuncs)
            {
                try
                {
                    ProxerResult lResult;
                    if ((lResult = await initFunc.Invoke()).Success) continue;

                    lReturn.AddExceptions(lResult.Exceptions);
                    lFailedInits++;
                }
                catch
                {
                    return new ProxerResult
                    {
                        Success = false
                    };
                }
            }

            this.IstInitialisiert = true;

            if (lFailedInits < this._initFuncs.Length)
                lReturn.Success = true;

            return lReturn;
        }

        /// <summary>
        ///     Sendet dem <see cref="User">Benutzer</see> eine Freundschaftsanfrage.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="WrongResponseException" /> wird ausgelöst, wenn die Antwort des Servers nicht der
        ///                 Erwarteten entspricht.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <returns>Einen boolischen Wert, der angibt, ob die Aktion erfolgreich war.</returns>
        public async Task<ProxerResult<bool>> SendFriendRequest()
        {
            if (this.Id == -1) return new ProxerResult<bool> {Success = false};

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"type", "addFriend"}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling("https://proxer.me/user/" + this.Id + "?format=json",
                        lPostArgs,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult<bool>(lResult.Exceptions);

            try
            {
                Dictionary<string, string> lResultDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResult.Result);

                return lResultDictionary.ContainsKey("error")
                    ? new ProxerResult<bool>(lResultDictionary["error"].Equals("0"))
                    : new ProxerResult<bool>(new Exception[] {new WrongResponseException {Response = lResult.Result}});
            }
            catch
            {
                return
                    new ProxerResult<bool>(
                        (await ErrorHandler.HandleError(this._senpai, lResult.Result, false)).Exceptions);
            }
        }


        private async Task<ProxerResult> InitMainInfo()
        {
            if (this.Id == -1) return new ProxerResult();

            HtmlDocument lDocument = new HtmlDocument();

            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\">\n<h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3>\n</div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitMainInfo))});

                return new ProxerResult();
            };

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling("https://proxer.me/user/" + this.Id, this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNodeCollection lProfileNodes =
                    lDocument.DocumentNode.SelectNodes("//table[@class='profile']");

                this.Avatar =
                    new Uri("https:" +
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

        private async Task<ProxerResult> InitFriends()
        {
            if (this.Id == -1) return new ProxerResult();

            this.Freunde = new List<User>();

            ProxerResult<HtmlNode[]> lResult = await this.GetAllFriendNodes();
            if (!lResult.Success) return new ProxerResult(lResult.Exceptions);
            try
            {
                foreach (HtmlNode curFriendNode in lResult.Result)
                {
                    string lUsername = curFriendNode.ChildNodes[2].InnerText;
                    int lId =
                        Convert.ToInt32(
                            curFriendNode.Attributes["id"].Value.Substring("entry".Length));
                    Uri lAvatar = curFriendNode.ChildNodes[2].GetAttributeValue("title", "Avatar:")
                                                             .Equals("Avatar:")
                        ? new Uri(
                            "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png")
                        : new Uri("https://proxer.me/images/comprofiler/" +
                                  curFriendNode.ChildNodes[2].GetAttributeValue("title", "Avatar:")
                                                             .Split(':')[1]);
                    bool lOnline =
                        curFriendNode.ChildNodes[1].FirstChild.GetAttributeValue("src",
                            "/images/misc/offlineicon.png").Equals("/images/misc/onlineicon.png");

                    this.Freunde.Add(new User(lUsername, lId, lAvatar, lOnline, this._senpai));
                }

                return new ProxerResult();
            }
            catch
            {
                return
                    new ProxerResult(new Exception[] {new WrongResponseException()});
            }
        }

        private async Task<ProxerResult<HtmlNode[]>> GetAllFriendNodes()
        {
            ProxerResult<string> lResult;
            int lSeite = 1;

            List<HtmlNode> lReturn = new List<HtmlNode>();

            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\"><h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3></div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitFriends))});

                return new ProxerResult();
            };

            while (
                (lResult =
                    await
                        HttpUtility.GetResponseErrorHandling(
                            "https://proxer.me/user/" + this.Id + "/connections/" + lSeite + "?format=raw",
                            this._senpai.LoginCookies
                            , this._senpai.ErrHandler, this._senpai, new[] {lCheckFunc})).Success)
            {
                HtmlDocument lDocument = new HtmlDocument();
                lDocument.LoadHtml(lResult.Result);

                try
                {
                    if (
                        lDocument.DocumentNode.SelectSingleNode("//div[@class='inner']")?
                                 .InnerText.Equals("Dieser Benutzer hat bisher keine Freunde :/") ?? false)
                    {
                        break;
                    }

                    HtmlNode lProfileNodes =
                        lDocument.DocumentNode.SelectSingleNode("//table[@id='box-table-a']");

                    if (lProfileNodes == null)
                        return
                            new ProxerResult<HtmlNode[]>(new Exception[]
                            {new WrongResponseException {Response = lResult.Result}});

                    lProfileNodes.ChildNodes.Remove(0);
                    lReturn.AddRange(lProfileNodes.ChildNodes);

                    lSeite++;
                }
                catch
                {
                    return
                        new ProxerResult<HtmlNode[]>(
                            (await ErrorHandler.HandleError(this._senpai, lResult.Result, false)).Exceptions);
                }
            }

            return new ProxerResult<HtmlNode[]>(lReturn.ToArray());
        }

        private async Task<ProxerResult> InitInfos()
        {
            if (this.Id == -1) return new ProxerResult();

            HtmlDocument lDocument = new HtmlDocument();

            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\"><h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3></div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitInfos))});

                return new ProxerResult();
            };

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling("https://proxer.me/user/" + this.Id + "/about?format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNodeCollection lProfileNodes =
                    lDocument.DocumentNode.SelectNodes("//table[@class='profile']");

                this.Info = lProfileNodes[0].ChildNodes[10].InnerText;
                this.InfoHtml = lProfileNodes[0].ChildNodes[10].InnerHtml;

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        private async Task<ProxerResult> InitAnime()
        {
            if (this.Id == -1) return new ProxerResult();

            HtmlDocument lDocument = new HtmlDocument();

            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\"><h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3></div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitAnime))});

                return new ProxerResult();
            };

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling("https://proxer.me/user/" + this.Id + "/anime?format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                #region Process Nodes

                this._animeList =
                    new List<KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>>();

                lDocument.LoadHtml(lResponse);

                this.FavouritenAnime =
                    lDocument.DocumentNode.ChildNodes[5].ChildNodes.Where(
                        x =>
                            x.HasAttributes && x.Attributes.Contains("href") &&
                            x.Attributes["href"].Value.StartsWith("/info/"))
                                                        .Select(
                                                            favouritenNode =>
                                                                new Anime(favouritenNode.Attributes["title"].Value,
                                                                    Convert.ToInt32(
                                                                        Utility.GetTagContents(
                                                                            favouritenNode.Attributes["href"].Value,
                                                                            "/info/", "#top").First()), this._senpai))
                                                        .ToArray();

                foreach (
                    HtmlNode animeNode in
                        lDocument.DocumentNode.ChildNodes[7].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    Anime lAnime = new Anime(animeNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1").Split(':')[1]),
                        this._senpai);

                    this._animeList.Add(
                        new KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>(
                            AnimeMangaProgressObject.AnimeMangaProgress.Finished,
                            new AnimeMangaProgressObject(this, lAnime,
                                Convert.ToInt32(animeNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(animeNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgressObject.AnimeMangaProgress.Finished)));
                }

                foreach (
                    HtmlNode animeNode in
                        lDocument.DocumentNode.ChildNodes[10].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    Anime lAnime = new Anime(animeNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1").Split(':')[1]),
                        this._senpai);

                    this._animeList.Add(
                        new KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>(
                            AnimeMangaProgressObject.AnimeMangaProgress.InProgress,
                            new AnimeMangaProgressObject(this, lAnime,
                                Convert.ToInt32(animeNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(animeNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgressObject.AnimeMangaProgress.InProgress)));
                }

                foreach (
                    HtmlNode animeNode in
                        lDocument.DocumentNode.ChildNodes[13].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    Anime lAnime = new Anime(animeNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1").Split(':')[1]),
                        this._senpai);

                    this._animeList.Add(
                        new KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>(
                            AnimeMangaProgressObject.AnimeMangaProgress.Planned,
                            new AnimeMangaProgressObject(this, lAnime,
                                Convert.ToInt32(animeNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(animeNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgressObject.AnimeMangaProgress.Planned)));
                }

                foreach (
                    HtmlNode animeNode in
                        lDocument.DocumentNode.ChildNodes[16].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    Anime lAnime = new Anime(animeNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1").Split(':')[1]),
                        this._senpai);

                    this._animeList.Add(
                        new KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>(
                            AnimeMangaProgressObject.AnimeMangaProgress.Aborted,
                            new AnimeMangaProgressObject(this, lAnime,
                                Convert.ToInt32(animeNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(animeNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgressObject.AnimeMangaProgress.Aborted)));
                }

                #endregion

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        private async Task<ProxerResult> InitManga()
        {
            if (this.Id == -1) return new ProxerResult();

            HtmlDocument lDocument = new HtmlDocument();

            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\"><h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3></div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitManga))});

                return new ProxerResult();
            };

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling("https://proxer.me/user/" + this.Id + "/manga?format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                #region Process Nodes

                this._mangaList =
                    new List<KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>>();

                lDocument.LoadHtml(lResponse);

                this.FavouritenManga =
                    lDocument.DocumentNode.ChildNodes[5].ChildNodes.Where(
                        x =>
                            x.HasAttributes && x.Attributes.Contains("href") &&
                            x.Attributes["href"].Value.StartsWith("/info/"))
                                                        .Select(
                                                            favouritenNode =>
                                                                new Manga(favouritenNode.Attributes["title"].Value,
                                                                    Convert.ToInt32(
                                                                        Utility.GetTagContents(
                                                                            favouritenNode.Attributes["href"].Value,
                                                                            "/info/", "#top").First()), this._senpai))
                                                        .ToArray();

                foreach (
                    HtmlNode mangaNode in
                        lDocument.DocumentNode.ChildNodes[7].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    Manga lManga = new Manga(mangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            mangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1").Split(':')[1]),
                        this._senpai);

                    this._mangaList.Add(
                        new KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>(
                            AnimeMangaProgressObject.AnimeMangaProgress.Finished,
                            new AnimeMangaProgressObject(this, lManga,
                                Convert.ToInt32(mangaNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(mangaNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgressObject.AnimeMangaProgress.Finished)));
                }

                foreach (
                    HtmlNode mangaNode in
                        lDocument.DocumentNode.ChildNodes[10].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    Manga lManga = new Manga(mangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            mangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1").Split(':')[1]),
                        this._senpai);

                    this._mangaList.Add(
                        new KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>(
                            AnimeMangaProgressObject.AnimeMangaProgress.InProgress,
                            new AnimeMangaProgressObject(this, lManga,
                                Convert.ToInt32(mangaNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(mangaNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgressObject.AnimeMangaProgress.InProgress)));
                }

                foreach (
                    HtmlNode mangaNode in
                        lDocument.DocumentNode.ChildNodes[13].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    Manga lManga = new Manga(mangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            mangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1").Split(':')[1]),
                        this._senpai);

                    this._mangaList.Add(
                        new KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>(
                            AnimeMangaProgressObject.AnimeMangaProgress.Planned,
                            new AnimeMangaProgressObject(this, lManga,
                                Convert.ToInt32(mangaNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(mangaNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgressObject.AnimeMangaProgress.Planned)));
                }

                foreach (
                    HtmlNode mangaNode in
                        lDocument.DocumentNode.ChildNodes[16].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    Manga lManga = new Manga(mangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            mangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1").Split(':')[1]),
                        this._senpai);

                    this._mangaList.Add(
                        new KeyValuePair<AnimeMangaProgressObject.AnimeMangaProgress, AnimeMangaProgressObject>(
                            AnimeMangaProgressObject.AnimeMangaProgress.Aborted,
                            new AnimeMangaProgressObject(this, lManga,
                                Convert.ToInt32(mangaNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(mangaNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgressObject.AnimeMangaProgress.Aborted)));
                }

                #endregion

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        ///     Gibt einen string zurück, der das aktuelle Objekt repräsentiert.
        /// </summary>
        /// <returns>
        ///     Ein string, der das aktuelle Objekt repräsentiert.
        /// </returns>
        public override string ToString()
        {
            return this.UserName + " [" + this.Id + "]";
        }


        /*
         * -----------------------------------
         * --------Statische Methoden---------
         * -----------------------------------
        */

        /// <summary>
        ///     Überprüft, ob zwei Benutzter Freunde sind.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="InitializeNeededException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="InitializeNeededException" /> wird ausgelöst, wenn die Eigenschaften des Objektes
        ///                 noch nicht initialisiert sind.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="ArgumentNullException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="ArgumentNullException" /> wird ausgelöst, wenn <paramref name="user1" /> oder
        ///                 <paramref name="user2" /> null (oder
        ///                 Nothing in Visual Basic) sind.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="user1">Benutzer 1</param>
        /// <param name="user2">Benutzer 2</param>
        /// <returns>Benutzer sind Freunde. True oder False.</returns>
        public static ProxerResult<bool> IsUserFriendOf(User user1, User user2)
        {
            if (user1 == null)
                return new ProxerResult<bool>(new Exception[] {new ArgumentNullException(nameof(user1))});

            return user2 == null
                ? new ProxerResult<bool>(new Exception[] {new ArgumentNullException(nameof(user2))})
                : new ProxerResult<bool>(user1.Freunde.Any(item => item.Id == user2.Id));
        }

        /// <summary>
        ///     Gibt den Benutzernamen eines Benutzers mit der spezifizierten ID zurück.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="NotLoggedInException" /> wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see>
        ///                 nicht eingeloggt ist.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="WrongResponseException" /> wird ausgelöst, wenn die Antwort des Servers nicht der
        ///                 Erwarteten entspricht.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="ArgumentNullException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="ArgumentNullException" /> wird ausgelöst, wenn <paramref name="senpai" /> null
        ///                 (oder Nothing in Visual Basic) ist.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="NoAccessException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="NoAccessException" /> wird ausgelöst, wenn Teile der Initialisierung nicht durchgeführt
        ///                 werden können,
        ///                 da der <see cref="Senpai">Benutzer</see> nicht die nötigen Rechte dafür hat.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="id">Die ID des Benutzers</param>
        /// <param name="senpai">Login-Cookies werden benötigt</param>
        /// <seealso cref="Senpai.Login" />
        /// <returns></returns>
        public static async Task<ProxerResult<string>> GetUNameFromId(int id, Senpai senpai)
        {
            if (senpai == null)
                return new ProxerResult<string>(new Exception[] {new ArgumentNullException(nameof(senpai))});

            HtmlDocument lDocument = new HtmlDocument();
            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\">\n<h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3>\n</div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(GetUNameFromId))});

                return new ProxerResult();
            };

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling("https://proxer.me/user/" + id + "/overview?format=raw",
                        senpai.LoginCookies,
                        senpai.ErrHandler,
                        senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult<string>(lResult.Exceptions);

            string lResponse = lResult.Result;

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