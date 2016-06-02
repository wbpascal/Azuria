using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Main;
using Azuria.Main.Search;
using Azuria.Main.User;
using Azuria.Main.User.Comment;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using Azuria.Utilities.Properties;
using HtmlAgilityPack;
using JetBrains.Annotations;
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
        [NotNull] public static User System = new User("System", -1, new Senpai());

        private readonly Senpai _senpai;

        /// <summary>
        ///     Initialisiert die Klasse mit allen Standardeinstellungen.
        /// </summary>
        /// <param name="userId">Die ID des Benutzers</param>
        /// <param name="senpai">Wird benötigt um einige Eigenschaften abzurufen</param>
        public User(int userId, [NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Id = userId;

            this.Anime =
                new InitialisableProperty
                    <IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<Anime>>>>
                    (this.InitAnime);
            this.Avatar = new InitialisableProperty<Uri>(this.InitMainInfo,
                new Uri("https://cdn.proxer.me/avatar/nophoto.png"))
            {
                IsInitialisedOnce = false
            };
            this.AnimeChronic = new InitialisableProperty<IEnumerable<AnimeMangaChronicObject<Anime>>>(this.InitChronic);
            this.FavouriteAnime = new InitialisableProperty<IEnumerable<Anime>>(this.InitAnime);
            this.FavouriteManga = new InitialisableProperty<IEnumerable<Manga>>(this.InitManga);
            this.Friends = new InitialisableProperty<IEnumerable<User>>(this.InitFriends);
            this.Info = new InitialisableProperty<string>(this.InitInfos);
            this.InfoHtml = new InitialisableProperty<string>(this.InitInfos);
            this.IsOnline = new InitialisableProperty<bool>(this.InitMainInfo);
            this.Manga =
                new InitialisableProperty
                    <IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<Manga>>>>
                    (this.InitManga);
            this.MangaChronic = new InitialisableProperty<IEnumerable<AnimeMangaChronicObject<Manga>>>(this.InitChronic);
            this.Points = new InitialisableProperty<int>(this.InitMainInfo);
            this.Ranking = new InitialisableProperty<string>(this.InitMainInfo);
            this.Status = new InitialisableProperty<string>(this.InitMainInfo);
            this.UserName = new InitialisableProperty<string>(this.InitMainInfo);
        }

        internal User([NotNull] string name, int userId, [NotNull] Senpai senpai) : this(userId, senpai)
        {
            this.UserName = new InitialisableProperty<string>(this.InitMainInfo, name);
        }

        internal User(int userId, [CanBeNull] Uri avatar, [NotNull] Senpai senpai)
            : this(userId, senpai)
        {
            this.Avatar = new InitialisableProperty<Uri>(this.InitMainInfo,
                avatar ?? new Uri("https://cdn.proxer.me/avatar/nophoto.png"));
        }

        internal User([NotNull] string name, int userId, [CanBeNull] Uri avatar, [NotNull] Senpai senpai)
            : this(name, userId, senpai)
        {
            this.Avatar = new InitialisableProperty<Uri>(this.InitMainInfo,
                avatar ?? new Uri("https://cdn.proxer.me/avatar/nophoto.png"));
        }

        internal User([NotNull] string name, int userId, [CanBeNull] Uri avatar, int points, [NotNull] Senpai senpai)
            : this(name, userId, avatar, senpai)
        {
            this.Points = new InitialisableProperty<int>(this.InitMainInfo, points);
        }

        internal User([NotNull] string name, int userId, [NotNull] Uri avatar, bool online, [NotNull] Senpai senpai)
            : this(name, userId, avatar, senpai)
        {
            this.IsOnline = new InitialisableProperty<bool>(this.InitMainInfo, online);
        }

        #region Properties

        /// <summary>
        ///     Gibt alle <see cref="Anime">Anime</see> zurück, die der <see cref="User">Benutzer</see>
        ///     in seinem Profil markiert hat.
        /// </summary>
        [NotNull]
        public
            InitialisableProperty<IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<Anime>>>>
            Anime { get; }

        /// <summary>
        ///     Gibt die Chronik des Benutzers zurück.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaChronicObject<Anime>>> AnimeChronic { get; }

        /// <summary>
        ///     Gibt den Link zu dem Avatar des Benutzers zurück.
        /// </summary>
        [NotNull]
        public InitialisableProperty<Uri> Avatar { get; }

        /// <summary>
        ///     Gibt die Anime-Favouriten des Benutzers zurück.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<Anime>> FavouriteAnime { get; }

        /// <summary>
        ///     Gibt die Manga-Favouriten des Benutzers zurück.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<Manga>> FavouriteManga { get; }

        /// <summary>
        ///     Gibt die Freunde des Benutzers in einer Liste zurück.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<User>> Friends { get; }

        /// <summary>
        ///     Gibt die ID des Benutzers zurück.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gibt die Info des Benutzers als Text-Dokument zurück.
        ///     Dabei werden sämtliche Html-Eigenschaften ignoriert.
        /// </summary>
        [NotNull]
        public InitialisableProperty<string> Info { get; }

        /// <summary>
        ///     Gibt die Info des Benutzers als Html-Dokument zurück.
        /// </summary>
        [NotNull]
        public InitialisableProperty<string> InfoHtml { get; }

        /// <summary>
        ///     Gibt an, ob das Objekt bereits Initialisiert ist.
        /// </summary>
        public bool IsInitialized => this.IsFullyInitialised();

        /// <summary>
        ///     Gibt zurück, ob der Benutzter zur Zeit online ist.
        /// </summary>
        [NotNull]
        public InitialisableProperty<bool> IsOnline { get; }

        /// <summary>
        ///     Gibt alle <see cref="Manga">Manga</see> zurück, die der <see cref="User">Benutzer</see>
        ///     in seinem Profil markiert hat.
        /// </summary>
        [NotNull]
        public
            InitialisableProperty<IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<Manga>>>>
            Manga { get; }

        /// <summary>
        ///     Gibt die Chronik des Benutzers zurück.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeMangaChronicObject<Manga>>> MangaChronic { get; }

        /// <summary>
        ///     Gibt zurück, wie viele Punkte der Benutzter momentan hat.
        /// </summary>
        [NotNull]
        public InitialisableProperty<int> Points { get; }

        /// <summary>
        ///     Gibt den Rangnamen des Benutzers zurück.
        /// </summary>
        [NotNull]
        public InitialisableProperty<string> Ranking { get; }

        /// <summary>
        ///     Gibt den Status des Benutzers zurück.
        /// </summary>
        [NotNull]
        public InitialisableProperty<string> Status { get; }

        /// <summary>
        ///     Gibt den Benutzernamen des Benutzers zurück.
        /// </summary>
        [NotNull]
        public InitialisableProperty<string> UserName { get; }

        #endregion

        #region

        /// <summary>
        ///     Überprüft, ob zwei Benutzter Freunde sind.
        /// </summary>
        /// <param name="user1">Benutzer 1</param>
        /// <param name="user2">Benutzer 2</param>
        /// <returns>Benutzer sind Freunde. True oder False.</returns>
        [NotNull]
        public static async Task<ProxerResult<bool>> AreUserFriends([NotNull] User user1, [NotNull] User user2)
        {
            return
                new ProxerResult<bool>(
                    (await user1.Friends.GetObject()).OnError(new User[0]).Any(item => item.Id == user2.Id));
        }

        [ItemNotNull]
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
                            new Uri("https://proxer.me/user/" + this.Id + "/connections/" + lSeite + "?format=raw"),
                            this._senpai.LoginCookies, this._senpai, new[] {lCheckFunc})).Success)
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
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<Comment<IAnimeMangaObject>>>> GetComments(int startIndex, int count)
        {
            return
                await
                    Comment<IAnimeMangaObject>.GetCommentsFromUrl(startIndex, count,
                        "https://proxer.me/user/" + this.Id + "/latestcomments/",
                        "", this._senpai, null, true, this);
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
        /// <returns></returns>
        [ItemNotNull]
        [Obsolete("Erzeuge ein neues Objekt mit der Id und rufe den Username des Objektes ab.", true)]
        public static async Task<ProxerResult<string>> GetUNameFromId(int id, [NotNull] Senpai senpai)
        {
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
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/user/" + id + "/overview?format=raw"),
                        senpai.LoginCookies,
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
        [ItemNotNull]
        [Obsolete("Bitte benutze die Methoden der jeweiligen Eigenschaften, um sie zu initalisieren!")]
        public async Task<ProxerResult> Init()
        {
            return await this.InitAllInitalisableProperties();
        }

        [ItemNotNull]
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
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/user/" + this.Id + "/anime?format=raw"),
                        this._senpai.LoginCookies,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                this.Anime.SetInitialisedObject(
                    new List<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<Anime>>>());

                lDocument.LoadHtml(lResponse);

                this.FavouriteAnime.SetInitialisedObject(lDocument.DocumentNode.ChildNodes[5].ChildNodes.Where(
                    x =>
                        x.HasAttributes && x.Attributes.Contains("href") &&
                        x.Attributes["href"].Value.StartsWith("/info/"))
                    .Select(
                        favouritenNode =>
                            new Anime(favouritenNode.Attributes["title"].Value,
                                Convert.ToInt32(
                                    favouritenNode.Attributes["href"].Value.GetTagContents("/info/", "#top").First()),
                                this._senpai))
                    .ToArray());

                ProxerResult<IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<Anime>>>>
                    lProcessResult =
                        this.ProcessAnimeMangaProgressNodes<Anime>(lDocument);
                if (!lProcessResult.Success) return new ProxerResult(lProcessResult.Exceptions);
                this.Anime.SetInitialisedObject(lProcessResult.Result);
                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        [ItemNotNull]
        private async Task<ProxerResult> InitChronic()
        {
            if (this.Id == -1) return new ProxerResult();

            HtmlDocument lDocument = new HtmlDocument();
            List<AnimeMangaChronicObject<Anime>> lAnimeChronicObjects = new List<AnimeMangaChronicObject<Anime>>();
            List<AnimeMangaChronicObject<Manga>> lMangaChronicObjects = new List<AnimeMangaChronicObject<Manga>>();

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
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/user/" + this.Id + "/chronik?format=raw"),
                        this._senpai.LoginCookies,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lTableNode = lDocument.GetElementbyId("box-table-a");
                lTableNode.ChildNodes.RemoveAt(0);

                int lFailedParses = 0;
                int lParses = 0;
                foreach (HtmlNode chronicNode in lTableNode.ChildNodes)
                {
                    lParses++;
                    ProxerResult<AnimeMangaChronicObject<Anime>> lAnimeParseResult =
                        AnimeMangaChronicObject<Anime>.GetChronicObjectFromNode(chronicNode, this._senpai);
                    ProxerResult<AnimeMangaChronicObject<Manga>> lMangaParseResult =
                        AnimeMangaChronicObject<Manga>.GetChronicObjectFromNode(chronicNode, this._senpai);

                    if (lAnimeParseResult.Success && lAnimeParseResult.Result != null)
                    {
                        lAnimeChronicObjects.Add(lAnimeParseResult.Result);
                    }
                    else if (lMangaParseResult.Success && lMangaParseResult.Result != null)
                    {
                        lMangaChronicObjects.Add(lMangaParseResult.Result);
                    }
                    else lFailedParses++;
                }

                if (lParses == lFailedParses) return new ProxerResult {Success = false};
                this.AnimeChronic.SetInitialisedObject(lAnimeChronicObjects);
                this.MangaChronic.SetInitialisedObject(lMangaChronicObjects);

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        [ItemNotNull]
        private async Task<ProxerResult> InitFriends()
        {
            if (this.Id == -1) return new ProxerResult();

            ProxerResult<HtmlNode[]> lResult = await this.GetAllFriendNodes();
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            try
            {
                List<User> lFriendList = new List<User>();
                foreach (HtmlNode curFriendNode in lResult.Result)
                {
                    string lUsername = curFriendNode.ChildNodes[2].InnerText;
                    int lId =
                        Convert.ToInt32(
                            curFriendNode.Attributes["id"].Value.Substring("entry".Length));
                    Uri lAvatar = curFriendNode.ChildNodes[2].GetAttributeValue("title", "Avatar:")
                        .Equals("Avatar:")
                        ? new Uri("https://cdn.proxer.me/avatar/nophoto.png")
                        : new Uri("https://proxer.me/avatar/" +
                                  curFriendNode.ChildNodes[2].GetAttributeValue("title", "Avatar:")
                                      .Split(':')[1]);
                    bool lOnline =
                        curFriendNode.ChildNodes[1].FirstChild.GetAttributeValue("src",
                            "/images/misc/offlineicon.png").Equals("/images/misc/onlineicon.png");

                    lFriendList.Add(new User(lUsername, lId, lAvatar, lOnline, this._senpai));
                }
                this.Friends.SetInitialisedObject(lFriendList);

                return new ProxerResult();
            }
            catch
            {
                return
                    new ProxerResult(new Exception[] {new WrongResponseException()});
            }
        }

        [ItemNotNull]
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
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/user/" + this.Id + "/about?format=raw"),
                        this._senpai.LoginCookies,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode[] lProfileNodes =
                    lDocument.DocumentNode.SelectNodesUtility("class", "profile").ToArray();

                this.Info.SetInitialisedObject(lProfileNodes[0].ChildNodes[10].InnerText);
                this.InfoHtml.SetInitialisedObject(lProfileNodes[0].ChildNodes[10].InnerHtml);

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        [ItemNotNull]
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
                    HttpUtility.GetResponseErrorHandling(new Uri("https://proxer.me/user/" + this.Id + "?format=raw"),
                        this._senpai.LoginCookies,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode[] lProfileNodes =
                    lDocument.DocumentNode.SelectNodesUtility("class", "profile").ToArray();

                this.Avatar.SetInitialisedObject(new Uri("https:" +
                                                         lProfileNodes[0].ParentNode.ParentNode.ChildNodes[1].ChildNodes
                                                             [0]
                                                             .Attributes["src"].Value));
                this.Points.SetInitialisedObject(Convert.ToInt32(
                    lProfileNodes[0].FirstChild.InnerText.GetTagContents("Summe: ", " - ")[
                        0]));
                this.Ranking.SetInitialisedObject(
                    lProfileNodes[0].FirstChild.InnerText.GetTagContents(
                        this.Points.GetObjectIfInitialised(-1) + " - ", "[?]")[0]);
                this.IsOnline.SetInitialisedObject(lProfileNodes[0].ChildNodes[1].InnerText.Equals("Status Online"));
                this.Status.SetInitialisedObject(lProfileNodes[0].ChildNodes.Count == 7
                    ? lProfileNodes[0].ChildNodes[6].InnerText
                    : "");

                this.UserName.SetInitialisedObject(lDocument.GetElementbyId("pageMetaAjax").InnerText.Split(' ')[1]);

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        [ItemNotNull]
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
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/user/" + this.Id + "/manga?format=raw"),
                        this._senpai.LoginCookies,
                        this._senpai, new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                this.FavouriteManga.SetInitialisedObject(lDocument.DocumentNode.ChildNodes[5].ChildNodes.Where(
                    x =>
                        x.HasAttributes && x.Attributes.Contains("href") &&
                        x.Attributes["href"].Value.StartsWith("/info/"))
                    .Select(
                        favouritenNode =>
                            new Manga(favouritenNode.Attributes["title"].Value,
                                Convert.ToInt32(
                                    favouritenNode.Attributes["href"].Value.GetTagContents("/info/", "#top").First()),
                                this._senpai))
                    .ToArray());

                ProxerResult<IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<Manga>>>>
                    lProcessResult =
                        this.ProcessAnimeMangaProgressNodes<Manga>(lDocument);
                if (!lProcessResult.Success) return new ProxerResult(lProcessResult.Exceptions);
                this.Manga.SetInitialisedObject(lProcessResult.Result);
                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        [NotNull]
        private ProxerResult<IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<T>>>>
            ProcessAnimeMangaProgressNodes<T>([NotNull] HtmlDocument htmlDocument)
            where T : IAnimeMangaObject
        {
            try
            {
                ConstructorInfo lConstructorToInvoke = null;
                foreach (
                    ConstructorInfo lDeclaredConstructor in
                        from lDeclaredConstructor in typeof(T).GetTypeInfo().DeclaredConstructors
                        let lParameters = lDeclaredConstructor.GetParameters()
                        where lParameters.Length == 3 && lParameters[0].ParameterType == typeof(string) &&
                              lParameters[1].ParameterType == typeof(int) &&
                              lParameters[2].ParameterType == typeof(Senpai)
                        select lDeclaredConstructor)
                {
                    lConstructorToInvoke = lDeclaredConstructor;
                }

                if (lConstructorToInvoke == null)
                    return
                        new ProxerResult
                            <IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<T>>>>(
                            new Exception[0]);

                #region Process Nodes

                List<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<T>>> lReturnList =
                    new List<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<T>>>();
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

                    T lAnimeManga = (T) lConstructorToInvoke.Invoke(lActivatorParameters);

                    if (lAnimeManga != null)
                        lReturnList.Add(
                            new KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<T>>(
                                AnimeMangaProgressState.Finished,
                                AnimeMangaProgressObject<T>.ParseFromHtmlNode(animeMangaNode, this, lAnimeManga,
                                    AnimeMangaProgressState.Finished, this._senpai)));
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

                    T lAnimeManga = (T) lConstructorToInvoke.Invoke(lActivatorParameters);

                    if (lAnimeManga != null)
                        lReturnList.Add(
                            new KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<T>>(
                                AnimeMangaProgressState.InProgress,
                                AnimeMangaProgressObject<T>.ParseFromHtmlNode(animeMangaNode, this, lAnimeManga,
                                    AnimeMangaProgressState.InProgress, this._senpai)));
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

                    T lAnimeManga = (T) lConstructorToInvoke.Invoke(lActivatorParameters);

                    if (lAnimeManga != null)
                        lReturnList.Add(
                            new KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<T>>(
                                AnimeMangaProgressState.Planned,
                                AnimeMangaProgressObject<T>.ParseFromHtmlNode(animeMangaNode, this, lAnimeManga,
                                    AnimeMangaProgressState.Planned, this._senpai)));
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

                    T lAnimeManga = (T) lConstructorToInvoke.Invoke(lActivatorParameters);

                    if (lAnimeManga != null)
                        lReturnList.Add(
                            new KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<T>>(
                                AnimeMangaProgressState.Aborted,
                                AnimeMangaProgressObject<T>.ParseFromHtmlNode(animeMangaNode, this, lAnimeManga,
                                    AnimeMangaProgressState.Aborted, this._senpai)));
                }

                #endregion

                return
                    new ProxerResult<IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<T>>>>(
                        lReturnList);
            }
            catch
            {
                return
                    new ProxerResult<IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<T>>>>(
                        new Exception[] {new WrongResponseException()});
            }
        }

        /// <summary>
        ///     Sendet den <see cref="User">Benutzer</see> eine Freundschaftsanfrage.
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <exception cref="NotLoggedInException">
        ///     Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see>, der die Anfrage
        ///     schickt, nicht eingeloggt ist.
        /// </exception>
        /// <exception cref="InvalidUserException">
        ///     Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see>, an den die Anfrage
        ///     geschickt wird, nicht gültig ist.
        /// </exception>
        /// <returns>Einen boolischen Wert, der angibt, ob die Aktion erfolgreich war.</returns>
        [ItemNotNull]
        public async Task<ProxerResult> SendFriendRequest()
        {
            if (this.Id == -1) return new ProxerResult(new[] {new InvalidUserException()});

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"type", "addFriend"}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(new Uri("https://proxer.me/user/" + this.Id + "?format=json"),
                        lPostArgs,
                        this._senpai.LoginCookies,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            try
            {
                Dictionary<string, string> lResultDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResult.Result);

                return new ProxerResult
                {
                    Success = lResultDictionary.ContainsKey("error") && lResultDictionary["error"].Equals("0")
                };
            }
            catch
            {
                return
                    new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResult.Result, false)).Exceptions);
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
            return this.UserName.GetObjectIfInitialised("ERROR") + " [" + this.Id + "]";
        }

        #endregion
    }
}