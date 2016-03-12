using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Main;
using Azuria.Main.Minor;
using Azuria.Main.Search;
using Azuria.Main.User;
using Azuria.Utilities;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Azuria
{
    /// <summary>
    ///     Repräsentiert einen Proxer-Benutzer
    /// </summary>
    public class User : ISearchableObject
    {
        /// <summary>
        ///     Representiert das System.
        /// </summary>
        public static User System = new User("System", -1, new Senpai());

        private readonly Func<Task<ProxerResult>>[] _initFuncs;

        private readonly Senpai _senpai;
        private List<KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>> _animeList;
        private Uri _avatar;
        private IEnumerable<Anime> _favouritenAnime;
        private IEnumerable<Manga> _favouritenManga;
        private List<User> _freunde;
        private string _info;
        private string _infoHtml;
        private List<KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>> _mangaList;
        private string _rang;
        private string _status;
        private string _userName;

        /// <summary>
        ///     Initialisiert die Klasse mit allen Standardeinstellungen.
        /// </summary>
        /// <param name="userId">Die ID des Benutzers</param>
        /// <param name="senpai">Wird benötigt um einige Eigenschaften abzurufen</param>
        public User(int userId, Senpai senpai) : this("", userId, senpai)
        {
        }

        internal User(string name, int userId, Senpai senpai) : this(name, userId, null, senpai)
        {
        }

        internal User(string name, int userId, Uri avatar, int points, Senpai senpai)
            : this(name, userId, avatar, senpai)
        {
            this.Points = points;
        }

        internal User(string name, int userId, Uri avatar, Senpai senpai) : this(name, userId, avatar, false, senpai)
        {
        }

        internal User(string name, int userId, Uri avatar, bool online, Senpai senpai)
        {
            this._senpai = senpai;
            this._initFuncs = new Func<Task<ProxerResult>>[]
            {this.InitMainInfo, this.InitInfos, this.InitFriends, this.InitAnime, this.InitManga, this.InitChronic};
            this.IsInitialized = false;

            this.UserName = name;
            this.Id = userId;
            this.IsOnline = online;
            if (avatar != null) this.Avatar = avatar;
            else
                this.Avatar =
                    new Uri(
                        "https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
        }

        #region Properties

        /// <summary>
        ///     Gibt alle <see cref="Anime">Anime</see> zurück, die der <see cref="User">Benutzer</see>
        ///     in seinem Profil markiert hat.
        /// </summary>
        /// <seealso cref="Init" />
        public IEnumerable<KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>> Anime
            => this._animeList ??
               new List<KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>>();

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
        ///     Gibt die Chronik des Benutzers zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public IEnumerable<AnimeMangaChronicObject> Chronic { get; private set; }

        /// <summary>
        ///     Gibt die Anime-Favouriten des Benutzers zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public IEnumerable<Anime> FavouriteAnime
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
        public IEnumerable<Manga> FavouriteManga
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
        public List<User> Friends
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
        public bool IsInitialized { get; private set; }

        /// <summary>
        ///     Gibt zurück, ob der Benutzter zur Zeit online ist.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public bool IsOnline { get; private set; }

        /// <summary>
        ///     Gibt alle <see cref="Manga">Manga</see> zurück, die der <see cref="User">Benutzer</see>
        ///     in seinem Profil markiert hat.
        /// </summary>
        /// <seealso cref="Init" />
        public IEnumerable<KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>> Manga
            => this._mangaList ??
               new List<KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>>();

        /// <summary>
        ///     Gibt zurück, wie viele Punkte der Benutzter momentan hat.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public int Points { get; private set; }

        /// <summary>
        ///     Gibt den Rangnamen des Benutzers zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public string Ranking
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
        ///     Überprüft, ob zwei Benutzter Freunde sind.
        /// </summary>
        /// <exception cref="InitializeNeededException">
        ///     Wird ausgelöst, wenn die Eigenschaften der Parameter noch nicht
        ///     initialisiert sind.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Wird ausgelöst, wenn <paramref name="user1" /> null (oder Nothing in Visual
        ///     Basic) ist.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Wird ausgelöst, wenn <paramref name="user2" /> null (oder Nothing in Visual
        ///     Basic) ist.
        /// </exception>
        /// <param name="user1">Benutzer 1</param>
        /// <param name="user2">Benutzer 2</param>
        /// <returns>Benutzer sind Freunde. True oder False.</returns>
        public static ProxerResult<bool> AreUserFriends(User user1, User user2)
        {
            if (user1 == null)
                return new ProxerResult<bool>(new Exception[] {new ArgumentNullException(nameof(user1))});

            return user2 == null
                ? new ProxerResult<bool>(new Exception[] {new ArgumentNullException(nameof(user2))})
                : new ProxerResult<bool>(user1.Friends.Any(item => item.Id == user2.Id));
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
                        lDocument.DocumentNode.SelectNodesUtility("class", "inner").First()?
                            .InnerText.Equals("Dieser Benutzer hat bisher keine Freunde :/") ?? false)
                    {
                        break;
                    }

                    HtmlNode lProfileNodes = lDocument.GetElementbyId("box-table-a");

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

        /// <summary>
        ///     Gibt die Kommentare des <see cref="User" /> chronologisch geordnet zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <param name="startIndex">Der Start-Index der ausgegebenen Kommentare.</param>
        /// <param name="count">Die Anzahl der ausgegebenen Kommentare ab dem angegebenen <paramref name="startIndex" />.</param>
        /// <returns>Eine Aufzählung mit den Kommentaren.</returns>
        public async Task<ProxerResult<IEnumerable<Comment>>> GetComments(int startIndex, int count)
        {
            return
                await
                    Comment.GetCommentsFromUrl(startIndex, count,
                        "https://proxer.me/user/" + this.Id + "/latestcomments/",
                        "", this._senpai, true, this);
        }

        /// <summary>
        ///     Gibt den Benutzernamen eines Benutzers mit der spezifizierten ID zurück.
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <exception cref="ArgumentNullException">
        ///     Wird ausgelöst, wenn <paramref name="senpai" /> null (oder Nothing in Visual
        ///     Basic) ist.
        /// </exception>
        /// <exception cref="NotLoggedInException">
        ///     Wird ausgelöst, wenn <paramref name="senpai">Benutzer</paramref> nicht
        ///     eingeloggt ist.
        /// </exception>
        /// <exception cref="NoAccessException">
        ///     Wird ausgelöst, wenn der <paramref name="senpai">Benutzer</paramref> nicht die
        ///     nötigen Rechte für die Aktion hat.
        /// </exception>
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
                return new ProxerResult<string>(lDocument.GetElementbyId("pageMetaAjax").InnerText.Split(' ')[1]);
            }
            catch
            {
                return new ProxerResult<string>((await ErrorHandler.HandleError(senpai, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        ///     Initialisiert die Eigenschaften der Klasse.
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</exception>
        /// <exception cref="NoAccessException">
        ///     Wird ausgelöst, wenn Teile der Initialisierung nicht durchgeführt werden können, da
        ///     der <see cref="Senpai">Benutzer</see> nicht die nötigen Rechte dafür hat.
        /// </exception>
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

            this.IsInitialized = true;

            if (lFailedInits < this._initFuncs.Length)
                lReturn.Success = true;

            return lReturn;
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
                this._animeList =
                    new List<KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>>();

                lDocument.LoadHtml(lResponse);

                this.FavouriteAnime =
                    lDocument.DocumentNode.ChildNodes[5].ChildNodes.Where(
                        x =>
                            x.HasAttributes && x.Attributes.Contains("href") &&
                            x.Attributes["href"].Value.StartsWith("/info/"))
                        .Select(
                            favouritenNode =>
                                new Anime(favouritenNode.Attributes["title"].Value,
                                    Convert.ToInt32(
                                        favouritenNode.Attributes["href"].Value.GetTagContents("/info/", "#top").First()),
                                    this._senpai))
                        .ToArray();

                ProxerResult lProcessResult;
                return
                    !(lProcessResult = this.ProcessAnimeMangaProgressNodes<Anime>(lDocument, ref this._animeList))
                        .Success
                        ? new ProxerResult(lProcessResult.Exceptions)
                        : new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        private async Task<ProxerResult> InitChronic()
        {
            if (this.Id == -1) return new ProxerResult();

            HtmlDocument lDocument = new HtmlDocument();

            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\">\n<h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3>\n</div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitChronic))});

                return new ProxerResult();
            };

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling("https://proxer.me/user/" + this.Id + "/chronik?format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lTableNode = lDocument.GetElementbyId("box-table-a");
                lTableNode.ChildNodes.RemoveAt(0);
                List<AnimeMangaChronicObject> lChronicObjects = new List<AnimeMangaChronicObject>();
                foreach (HtmlNode chronicNode in lTableNode.ChildNodes)
                {
                    ProxerResult<AnimeMangaChronicObject> lParseResult =
                        AnimeMangaChronicObject.GetChronicObjectFromNode(chronicNode, this._senpai);
                    if (lParseResult.Success) lChronicObjects.Add(lParseResult.Result);
                }
                this.Chronic = lChronicObjects;

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

            this.Friends = new List<User>();

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

                    this.Friends.Add(new User(lUsername, lId, lAvatar, lOnline, this._senpai));
                }

                return new ProxerResult();
            }
            catch
            {
                return
                    new ProxerResult(new Exception[] {new WrongResponseException()});
            }
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

                HtmlNode[] lProfileNodes =
                    lDocument.DocumentNode.SelectNodesUtility("class", "profile").ToArray();

                this.Info = lProfileNodes[0].ChildNodes[10].InnerText;
                this.InfoHtml = lProfileNodes[0].ChildNodes[10].InnerHtml;

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
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
                    HttpUtility.GetResponseErrorHandling("https://proxer.me/user/" + this.Id + "?format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode[] lProfileNodes =
                    lDocument.DocumentNode.SelectNodesUtility("class", "profile").ToArray();

                this.Avatar =
                    new Uri("https:" +
                            lProfileNodes[0].ParentNode.ParentNode.ChildNodes[1].ChildNodes[0]
                                .Attributes["src"].Value);
                this.Points =
                    Convert.ToInt32(
                        lProfileNodes[0].FirstChild.InnerText.GetTagContents("Summe: ", " - ")[
                            0]);
                this.Ranking =
                    lProfileNodes[0].FirstChild.InnerText.GetTagContents(this.Points + " - ",
                        "[?]")
                        [0];
                this.IsOnline = lProfileNodes[0].ChildNodes[1].InnerText.Equals("Status Online");
                this.Status = lProfileNodes[0].ChildNodes.Count == 7
                    ? lProfileNodes[0].ChildNodes[6].InnerText
                    : "";

                this.UserName =
                    lDocument.GetElementbyId("pageMetaAjax").InnerText.Split(' ')[1];

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
                this._mangaList =
                    new List<KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>>();

                lDocument.LoadHtml(lResponse);

                this.FavouriteManga =
                    lDocument.DocumentNode.ChildNodes[5].ChildNodes.Where(
                        x =>
                            x.HasAttributes && x.Attributes.Contains("href") &&
                            x.Attributes["href"].Value.StartsWith("/info/"))
                        .Select(
                            favouritenNode =>
                                new Manga(favouritenNode.Attributes["title"].Value,
                                    Convert.ToInt32(
                                        favouritenNode.Attributes["href"].Value.GetTagContents("/info/", "#top").First()),
                                    this._senpai))
                        .ToArray();

                ProxerResult lProcessResult;
                return
                    !(lProcessResult = this.ProcessAnimeMangaProgressNodes<Manga>(lDocument, ref this._mangaList))
                        .Success
                        ? new ProxerResult(lProcessResult.Exceptions)
                        : new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        private ProxerResult ProcessAnimeMangaProgressNodes<T>(HtmlDocument htmlDocument,
            ref List<KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>> saveList)
            where T : IAnimeMangaObject
        {
            try
            {
                ConstructorInfo lConstructorToInvoke = null;
                foreach (
                    ConstructorInfo lDeclaredConstructor in
                        from lDeclaredConstructor in typeof (T).GetTypeInfo().DeclaredConstructors
                        let lParameters = lDeclaredConstructor.GetParameters()
                        where lParameters.Length == 3 && lParameters[0].ParameterType == typeof (string) &&
                              lParameters[1].ParameterType == typeof (int) &&
                              lParameters[2].ParameterType == typeof (Senpai)
                        select lDeclaredConstructor)
                {
                    lConstructorToInvoke = lDeclaredConstructor;
                }

                if (lConstructorToInvoke == null) return new ProxerResult(new Exception[0]);

                #region Process Nodes

                foreach (
                    HtmlNode animeMangaNode in
                        htmlDocument.DocumentNode.ChildNodes[7].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    object[] lActivatorParameters =
                    {
                        animeMangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeMangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1")
                                .Split(':')[1]),
                        this._senpai
                    };

                    IAnimeMangaObject lAnimeManga =
                        lConstructorToInvoke.Invoke(lActivatorParameters) as IAnimeMangaObject;

                    saveList.Add(
                        new KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>(AnimeMangaProgress.Finished,
                            new AnimeMangaProgressObject(this, lAnimeManga,
                                Convert.ToInt32(animeMangaNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(animeMangaNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgress.Finished)));
                }

                foreach (
                    HtmlNode animeMangaNode in
                        htmlDocument.DocumentNode.ChildNodes[10].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    object[] lActivatorParameters =
                    {
                        animeMangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeMangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1")
                                .Split(':')[1]),
                        this._senpai
                    };

                    IAnimeMangaObject lAnimeManga =
                        lConstructorToInvoke.Invoke(lActivatorParameters) as IAnimeMangaObject;

                    saveList.Add(
                        new KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>(AnimeMangaProgress.InProgress,
                            new AnimeMangaProgressObject(this, lAnimeManga,
                                Convert.ToInt32(animeMangaNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(animeMangaNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgress.Finished)));
                }

                foreach (
                    HtmlNode animeMangaNode in
                        htmlDocument.DocumentNode.ChildNodes[13].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    object[] lActivatorParameters =
                    {
                        animeMangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeMangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1")
                                .Split(':')[1]),
                        this._senpai
                    };

                    IAnimeMangaObject lAnimeManga =
                        lConstructorToInvoke.Invoke(lActivatorParameters) as IAnimeMangaObject;

                    saveList.Add(
                        new KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>(AnimeMangaProgress.Planned,
                            new AnimeMangaProgressObject(this, lAnimeManga,
                                Convert.ToInt32(animeMangaNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(animeMangaNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgress.Finished)));
                }

                foreach (
                    HtmlNode animeMangaNode in
                        htmlDocument.DocumentNode.ChildNodes[16].ChildNodes.Where(
                            x =>
                                x.HasAttributes && x.Attributes.Contains("id") &&
                                x.Attributes["id"].Value.StartsWith("entry")))
                {
                    object[] lActivatorParameters =
                    {
                        animeMangaNode.ChildNodes[1].InnerText,
                        Convert.ToInt32(
                            animeMangaNode.ChildNodes[1].FirstChild.GetAttributeValue("title", "Cover:-1")
                                .Split(':')[1]),
                        this._senpai
                    };

                    IAnimeMangaObject lAnimeManga =
                        lConstructorToInvoke.Invoke(lActivatorParameters) as IAnimeMangaObject;

                    saveList.Add(
                        new KeyValuePair<AnimeMangaProgress, AnimeMangaProgressObject>(AnimeMangaProgress.Aborted,
                            new AnimeMangaProgressObject(this, lAnimeManga,
                                Convert.ToInt32(animeMangaNode.ChildNodes[4].InnerText.Split('/')[0].Trim()),
                                Convert.ToInt32(animeMangaNode.ChildNodes[4].InnerText.Split('/')[1].Trim()),
                                AnimeMangaProgress.Finished)));
                }

                #endregion
            }
            catch
            {
                return new ProxerResult(new Exception[] {new WrongResponseException()});
            }

            return new ProxerResult();
        }

        /// <summary>
        ///     Sendet den <see cref="User">Benutzer</see> eine Freundschaftsanfrage.
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <exception cref="NotLoggedInException">Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</exception>
        /// <returns>Einen boolischen Wert, der angibt, ob die Aktion erfolgreich war.</returns>
        public async Task<ProxerResult<bool>> SendFriendRequest()
        {
            if (this.Id == -1) return new ProxerResult<bool>(new Exception[0]) {Success = false};

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

        /// <summary>
        ///     Gibt einen string zurück, der das aktuelle Objekt repräsentiert.
        /// </summary>
        /// <returns>
        ///     Einen string, der das aktuelle Objekt repräsentiert.
        /// </returns>
        public override string ToString()
        {
            return this.UserName + " [" + this.Id + "]";
        }

        #endregion
    }
}