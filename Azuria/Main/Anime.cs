using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Main.Minor;
using Azuria.Utilities;
using Azuria.Utilities.Net;
using HtmlAgilityPack;

// ReSharper disable LoopCanBeConvertedToQuery

namespace Azuria.Main
{
    /// <summary>
    ///     Eine Klasse, die einen <see cref="Anime" /> darstellt.
    /// </summary>
    public class Anime : IAnimeMangaObject
    {
        /// <summary>
        ///     Eine Enumeration, die die Verschiedenen Typen eines Anime darstellt.
        /// </summary>
        public enum AnimeType
        {
            /// <summary>
            ///     Stellt eine Anime-Serie dar.
            /// </summary>
            Series,

            /// <summary>
            ///     Stellt einen Film dar.
            /// </summary>
            Movie,

            /// <summary>
            ///     Stellt eine Original Video Animation dar.
            /// </summary>
            Ova
        }

        /// <summary>
        ///     Eine Enumeration, die die Sprache eines <see cref="Anime" /> darstellt.
        /// </summary>
        public enum Language
        {
            /// <summary>
            ///     Der Anime besitzt Deutsche Untertitel.
            /// </summary>
            GerSub,

            /// <summary>
            ///     Der Anime besitzt Englische Untertitel.
            /// </summary>
            EngSub,

            /// <summary>
            ///     Der Anime ist Englisch vertont.
            /// </summary>
            EngDub,

            /// <summary>
            ///     Der Anime ist Deutsch vertont.
            /// </summary>
            GerDub
        }

        private readonly Func<Task<ProxerResult>>[] _initFuncs;

        private readonly Senpai _senpai;
        private string _beschreibung;
        private string _deutschTitel;
        private string _englischTitel;
        private Dictionary<Uri, string> _fsk;
        private GenreObject[] _genre;
        private Group[] _gruppen;
        private Industry[] _industrie;
        private string _japanTitel;
        private string[] _season;
        private string _synonym;

        internal Anime() { }

        /// <exception cref="ArgumentNullException"><paramref name="senpai" /> is <see langword="null" />.</exception>
        internal Anime(string name, int id, Senpai senpai)
        {
            if (senpai == null) throw new ArgumentNullException(nameof(senpai));
            this._senpai = senpai;
            this._initFuncs = new Func<Task<ProxerResult>>[]
            {this.InitMain, this.InitAvailableLang, this.InitEpisodeCount, this.InitType};
            this.ObjectType = AnimeMangaType.Anime;
            this.Name = name;
            this.Id = id;
            this.CoverUri = new Uri("http://cdn.proxer.me/cover/" + this.Id + ".jpg");

            this.IstInitialisiert = false;
        }

        internal Anime(string name, int id, Senpai senpai, IEnumerable<GenreObject> genreList, AnimeMangaStatus status) : this(name, id, senpai) 
        {
            this.Genre = genreList;
            this.Status = status;
        }

        #region Geerbt

        /// <summary>
        ///     Gibt die Beschreibung des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public string Beschreibung
        {
            get { return this._beschreibung ?? ""; }
            private set { this._beschreibung = value; }
        }


        /// <summary>
        ///     Gibt den Link zum Cover des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        public Uri CoverUri { get; }

        /// <summary>
        ///     Gibt den deutschen Titel des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public string DeutschTitel
        {
            get { return this._deutschTitel ?? ""; }
            private set { this._deutschTitel = value; }
        }

        /// <summary>
        ///     Gibt den Englische Titel des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public string EnglischTitel
        {
            get { return this._englischTitel ?? ""; }
            private set { this._englischTitel = value; }
        }


        /// <summary>
        ///     Gibt die Links zu allen FSK-Beschränkungen des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public Dictionary<Uri, string> Fsk
        {
            get { return this._fsk ?? new Dictionary<Uri, string>(); }
            private set { this._fsk = value; }
        }


        /// <summary>
        ///     Gitb die Genres des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public IEnumerable<GenreObject> Genre
        {
            get { return this._genre ?? new GenreObject[0]; }
            private set { this._genre = value.ToArray(); }
        }


        /// <summary>
        ///     Gibt die Gruppen zurück, die den <see cref="Anime" /> oder <see cref="Manga" /> übersetzten oder übersetzt haben.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Group" />
        /// <seealso cref="Init" />
        public IEnumerable<Group> Gruppen
        {
            get { return this._gruppen ?? new Group[0]; }
            private set { this._gruppen = value.ToArray(); }
        }


        /// <summary>
        ///     Gibt die ID des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public int Id { get; }


        /// <summary>
        ///     Gibt die Industrie des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Minor.Industry" />
        /// <seealso cref="Init" />
        public IEnumerable<Industry> Industrie
        {
            get { return this._industrie ?? new Industry[0]; }
            private set { this._industrie = value.ToArray(); }
        }


        /// <summary>
        ///     Gibt zurück, ob das Objekt bereits Initialisiert ist.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public bool IstInitialisiert { get; private set; }


        /// <summary>
        ///     Gibt den japanischen Titel des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public string JapanTitel
        {
            get { return this._japanTitel ?? ""; }
            private set { this._japanTitel = value; }
        }


        /// <summary>
        ///     Gibt zurück, ob der <see cref="Anime" /> oder <see cref="Manga" /> lizensiert ist.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public bool Lizensiert { get; private set; }


        /// <summary>
        ///     Gibt den Namen des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        ///     Gibt zurück, ob es sich um einen <see cref="Anime" /> oder <see cref="Manga" /> handelt.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        /// <seealso cref="AnimeMangaType" />
        public AnimeMangaType ObjectType { get; }


        /// <summary>
        ///     Gibt die Season des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public IEnumerable<string> Season
        {
            get { return this._season ?? new string[0]; }
            private set { this._season = value.ToArray(); }
        }


        /// <summary>
        ///     Gibt den Status des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="AnimeMangaStatus" />
        /// <seealso cref="Init" />
        public AnimeMangaStatus Status { get; private set; }


        /// <summary>
        ///     Gibt das Synonym des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public string Synonym
        {
            get { return this._synonym ?? ""; }
            private set { this._synonym = value; }
        }


        /// <summary>
        ///     Gibt die Kommentare des <see cref="Anime" />, nach ihrer Beliebtheit sortiert, zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
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
        ///     </list>
        /// </summary>
        /// <param name="startIndex">Der Start-Index der ausgegebenen Kommentare.</param>
        /// <param name="count">Die Anzahl der ausgegebenen Kommentare ab dem angegebenen <paramref name="startIndex" />.</param>
        /// <returns>Eine Aufzählung mit den Kommentaren.</returns>
        public async Task<ProxerResult<IEnumerable<Comment>>> GetCommentsRating(int startIndex, int count)
        {
            return
                await
                    Comment.GetCommentsFromUrl(startIndex, count, "https://proxer.me/info/" + this.Id + "/comments/",
                        "rating", this._senpai);
        }

        /// <summary>
        ///     Initialisiert das Objekt.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
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
        ///                 <see cref="CaptchaException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="CaptchaException" /> wird ausgelöst, wenn der Server das Ausfüllen eines Captchas
        ///                 erfordert.
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
        /// <seealso cref="Senpai.Login" />
        public async Task<ProxerResult> Init()
        {
            int lFailedInits = 0;
            ProxerResult lReturn = new ProxerResult();

            foreach (Func<Task<ProxerResult>> initFunc in this._initFuncs)
            {
                try
                {
                    ProxerResult lResult = await initFunc.Invoke();
                    if (lResult.Success) continue;

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
        ///     Gibt die Kommentare des <see cref="Anime" /> chronologisch geordnet zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
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
        ///     </list>
        /// </summary>
        /// <param name="startIndex">Der Start-Index der ausgegebenen Kommentare.</param>
        /// <param name="count">Die Anzahl der ausgegebenen Kommentare ab dem angegebenen <paramref name="startIndex" />.</param>
        /// <returns>Eine Aufzählung mit den Kommentaren.</returns>
        public async Task<ProxerResult<IEnumerable<Comment>>> GetCommentsLatest(int startIndex, int count)
        {
            return
                await
                    Comment.GetCommentsFromUrl(startIndex, count, "https://proxer.me/info/" + this.Id + "/comments/",
                        "latest", this._senpai);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gibt den Typ eines Anime zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public AnimeType AnimeTyp { get; private set; }

        /// <summary>
        ///     Gibt die Episodenanzahl eines <see cref="Anime" /> zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public int EpisodenZahl { get; private set; }

        /// <summary>
        ///     Gibt die verfügbaren Sprachen des <see cref="Anime" /> zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Language" />
        /// <seealso cref="Init" />
        public IEnumerable<Language> Sprachen { get; private set; }

        #endregion

        #region

        /// <summary>
        ///     Gibt die Episoden des <see cref="Anime" /> in einer bestimmten <see cref="Language">Sprache</see> zurück.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="LanguageNotAvailableException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="LanguageNotAvailableException" /> wird ausgelöst, wenn der Anime nicht in der
        ///                 angegebenen Sprache verfügbar ist.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="InitializeNeededException" />
        ///             </term>
        ///             <description>
        ///                 <see cref="InitializeNeededException" /> wird ausgelöst, wenn die Eigenschaften des Objektes
        ///                 noch nicht initialisiert sind.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="language">Die Sprache der Episoden.</param>
        /// <seealso cref="Anime.Episode" />
        /// <returns>Einen Array mit length = <see cref="EpisodenZahl" />.</returns>
        public ProxerResult<IEnumerable<Episode>> GetEpisodes(Language language)
        {
            if (this.Sprachen == null)
                return new ProxerResult<IEnumerable<Episode>>(new Exception[] {new InitializeNeededException()});
            if (!this.Sprachen.Contains(language))
                return new ProxerResult<IEnumerable<Episode>>(new Exception[] {new LanguageNotAvailableException()});

            List<Episode> lEpisodes = new List<Episode>();
            for (int i = 1; i <= this.EpisodenZahl; i++)
            {
                lEpisodes.Add(new Episode(this, i, language, this._senpai));
            }

            return new ProxerResult<IEnumerable<Episode>>(lEpisodes.ToArray());
        }

        /// <summary>
        ///     Gibt die aktuell am beliebtesten <see cref="Anime" /> zurück.
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
        ///     </list>
        /// </summary>
        /// <returns>Ein Array mit den aktuell beliebtesten <see cref="Anime" />.</returns>
        public static async Task<ProxerResult<IEnumerable<Anime>>> GetPopularAnime(Senpai senpai)
        {
            if (senpai == null)
                return new ProxerResult<IEnumerable<Anime>>(new Exception[] {new ArgumentNullException(nameof(senpai))});

            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "https://proxer.me/anime?format=raw",
                        null,
                        senpai.ErrHandler,
                        senpai);

            if (!lResult.Success)
                return new ProxerResult<IEnumerable<Anime>>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                return
                    new ProxerResult<IEnumerable<Anime>>(
                        (from childNode in lDocument.DocumentNode.ChildNodes[5].FirstChild.FirstChild.ChildNodes
                            let lId =
                                Convert.ToInt32(
                                    childNode.FirstChild.GetAttributeValue("href", "/info/-1#top").Split('/')[2].Split(
                                        '#')
                                        [0])
                            select new Anime(childNode.FirstChild.GetAttributeValue("title", "ERROR"), lId, senpai))
                            .ToArray());
            }
            catch
            {
                return new ProxerResult<IEnumerable<Anime>>(ErrorHandler.HandleError(senpai, lResponse).Exceptions);
            }
        }

        private async Task<ProxerResult> InitAvailableLang()
        {
            HtmlDocument lDocument = new HtmlDocument();
            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals("Bitte logge dich ein."))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitAvailableLang))});

                return new ProxerResult();
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "https://proxer.me/edit/entry/" + this.Id + "/languages?format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai,
                        new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                List<Language> languageList = new List<Language>();
                foreach (
                    HtmlNode childNode in
                        lDocument.DocumentNode.ChildNodes[4]
                            .ChildNodes[5].ChildNodes.Where(
                                childNode =>
                                    childNode.ChildNodes.Count > 3 &&
                                    childNode.ChildNodes[3].FirstChild.GetAttributeValue("value", "+").Equals("-")))
                {
                    switch (childNode.FirstChild.InnerText)
                    {
                        case "GerSub":
                            languageList.Add(Language.GerSub);
                            break;
                        case "EngSub":
                            languageList.Add(Language.EngSub);
                            break;
                        case "EngDub":
                            languageList.Add(Language.EngDub);
                            break;
                        case "GerDub":
                            languageList.Add(Language.GerDub);
                            break;
                    }
                }

                this.Sprachen = languageList.ToArray();

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        private async Task<ProxerResult> InitEpisodeCount()
        {
            HtmlDocument lDocument = new HtmlDocument();
            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals("Bitte logge dich ein."))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitEpisodeCount))});

                return new ProxerResult();
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "https://proxer.me/edit/entry/" + this.Id + "/count?format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai,
                        new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                this.EpisodenZahl =
                    Convert.ToInt32(
                        lDocument.DocumentNode.ChildNodes[4]
                            .ChildNodes[5].FirstChild.ChildNodes[1].InnerText);
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            return new ProxerResult();
        }

        private async Task<ProxerResult> InitMain()
        {
            HtmlDocument lDocument = new HtmlDocument();
            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals(
                        "<div class=\"inner\"><h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3></div>"))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitMain))});

                return new ProxerResult();
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "https://proxer.me/info/" + this.Id + "?format=raw",
                        null,
                        this._senpai.ErrHandler,
                        this._senpai,
                        new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lTableNode =
                    lDocument.DocumentNode.ChildNodes[5]
                        .ChildNodes[2].FirstChild.ChildNodes[1].FirstChild;
                foreach (HtmlNode childNode in lTableNode.ChildNodes.Where(childNode => childNode.Name.Equals("tr")))
                {
                    switch (childNode.FirstChild.FirstChild.InnerText)
                    {
                        case "Original Titel":
                            this.Name = childNode.ChildNodes[1].InnerText;
                            break;
                        case "Eng. Titel":
                            this.EnglischTitel = childNode.ChildNodes[1].InnerText;
                            break;
                        case "Ger. Titel":
                            this.DeutschTitel = childNode.ChildNodes[1].InnerText;
                            break;
                        case "Jap. Titel":
                            this.JapanTitel = childNode.ChildNodes[1].InnerText;
                            break;
                        case "Synonym":
                            this.Synonym = childNode.ChildNodes[1].InnerText;
                            break;
                        case "Genre":
                            List<GenreObject> lGenreList = new List<GenreObject>();
                            foreach (HtmlNode htmlNode in childNode.ChildNodes[1].ChildNodes.ToList())
                            {
                                if (htmlNode.Name.Equals("a"))
                                    lGenreList.Add(new GenreObject(htmlNode.InnerText));
                            }
                            this.Genre = lGenreList.ToArray();
                            break;
                        case "FSK":
                            this.Fsk = new Dictionary<Uri, string>();
                            foreach (
                                HtmlNode htmlNode in
                                    childNode.ChildNodes[1].ChildNodes.ToList()
                                        .Where(htmlNode => htmlNode.Name.Equals("span") &&
                                                           !this.Fsk.ContainsValue(htmlNode
                                                               .GetAttributeValue(
                                                                   "title", "ERROR"))))
                            {
                                this.Fsk.Add(
                                    new Uri("https://proxer.me" +
                                            htmlNode.FirstChild.GetAttributeValue("src",
                                                "/")),
                                    htmlNode.GetAttributeValue("title", "ERROR"));
                            }
                            break;
                        case "Season":
                            List<string> lSeasonList = new List<string>();
                            foreach (HtmlNode htmlNode in childNode.ChildNodes[1].ChildNodes.ToList())
                            {
                                if (htmlNode.Name.Equals("a"))
                                    lSeasonList.Add(htmlNode.InnerText);
                            }
                            this.Season = lSeasonList.ToArray();
                            break;
                        case "Status":
                            switch (childNode.ChildNodes[1].InnerText)
                            {
                                case "Airing":
                                    this.Status = AnimeMangaStatus.Airing;
                                    break;
                                case "Abgeschlossen":
                                    this.Status = AnimeMangaStatus.Abgeschlossen;
                                    break;
                                case "Nicht erschienen (Pre-Airing)":
                                    this.Status = AnimeMangaStatus.PreAiring;
                                    break;
                                default:
                                    this.Status = AnimeMangaStatus.Abgebrochen;
                                    break;
                            }
                            break;
                        case "Gruppen":
                            if (childNode.ChildNodes[1].InnerText.Contains("Keine Gruppen eingetragen.")) break;
                            this.Gruppen = (from htmlNode in childNode.ChildNodes[1].ChildNodes
                                where htmlNode.Name.Equals("a")
                                select
                                    new Group(
                                        Convert.ToInt32(
                                            Utility.GetTagContents(
                                                htmlNode.GetAttributeValue("href",
                                                    "/translatorgroups?id=-1#top"),
                                                "/translatorgroups?id=", "#top")[0]), htmlNode.InnerText))
                                .ToArray();
                            break;
                        case "Industrie":
                            if (childNode.ChildNodes[1].InnerText.Contains("Keine Unternehmen eingetragen.")) break;
                            List<Industry> lIndustries = new List<Industry>();
                            foreach (
                                HtmlNode htmlNode in
                                    childNode.ChildNodes[1].ChildNodes.Where(htmlNode => htmlNode.Name.Equals("a")))
                            {
                                Industry.IndustryType lIndustryType;
                                if (htmlNode.NextSibling.InnerText.Contains("Studio"))
                                    lIndustryType = Industry.IndustryType.Studio;
                                else if (htmlNode.NextSibling.InnerText.Contains("Publisher"))
                                    lIndustryType = Industry.IndustryType.Publisher;
                                else if (htmlNode.NextSibling.InnerText.Contains("Producer"))
                                    lIndustryType = Industry.IndustryType.Producer;
                                else lIndustryType = Industry.IndustryType.None;

                                lIndustries.Add(new Industry(Convert.ToInt32(
                                    Utility.GetTagContents(
                                        htmlNode.GetAttributeValue("href", "/industry?id=-1#top"),
                                        "/industry?id=", "#top")[0]), htmlNode.InnerText, lIndustryType));
                            }
                            this.Industrie = lIndustries.ToArray();
                            break;
                        case "Lizenz":
                            this.Lizensiert = childNode.ChildNodes[1].InnerText.StartsWith("Lizenziert!");
                            break;
                        case "Beschreibung:":
                            childNode.FirstChild.FirstChild.Remove();
                            this.Beschreibung = "";
                            foreach (HtmlNode htmlNode in childNode.FirstChild.ChildNodes)
                            {
                                if (htmlNode.Name.Equals("br")) this.Beschreibung += "\n";
                                else this.Beschreibung += htmlNode.InnerText;
                            }
                            if (this.Beschreibung.StartsWith("\n")) this.Beschreibung = this.Beschreibung.TrimStart();
                            break;
                    }
                }
            }
            catch
            {
                return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse).Exceptions);
            }

            return new ProxerResult();
        }

        private async Task<ProxerResult> InitType()
        {
            HtmlDocument lDocument = new HtmlDocument();
            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals("Bitte logge dich ein."))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitType))});

                return new ProxerResult();
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "https://proxer.me/edit/entry/" + this.Id + "/medium?format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai,
                        new[] {lCheckFunc});

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lNode =
                    lDocument.GetElementbyId("medium").ChildNodes.First(node => node.Attributes.Contains("selected"));
                switch (lNode.Attributes["value"].Value)
                {
                    case "animeseries":
                        this.AnimeTyp = AnimeType.Series;
                        break;
                    case "movie":
                        this.AnimeTyp = AnimeType.Movie;
                        break;
                    case "ova":
                        this.AnimeTyp = AnimeType.Ova;
                        break;
                    default:
                        this.AnimeTyp = AnimeType.Series;
                        break;
                }
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            return new ProxerResult();
        }

        #endregion

        /// <summary>
        ///     Eine Klasse, die die <see cref="Episode">Episode</see> eines <see cref="Anime" /> darstellt.
        /// </summary>
        public class Episode
        {
            private readonly Language _lang;
            private readonly Senpai _senpai;
            private List<KeyValuePair<Stream.StreamPartner, Stream>> _streams;

            internal Episode(Anime anime, int nr, Language lang, Senpai senpai)
            {
                this.ParentAnime = anime;
                this.EpisodeNr = nr;
                this._lang = lang;
                this._senpai = senpai;

                this.IstInitialisiert = false;
            }

            #region Properties

            /// <summary>
            ///     Gibt die Nummer der Episode zurück.
            /// </summary>
            public int EpisodeNr { get; set; }

            /// <summary>
            ///     Gibt zurück, ob das Objekt bereits initialisiert ist.
            /// </summary>
            public bool IstInitialisiert { get; private set; }

            /// <summary>
            ///     Gibt den Anime der Episode zurück.
            /// </summary>
            public Anime ParentAnime { get; set; }

            /// <summary>
            ///     Gibt die vorhandenen Streams der Episode zurück.
            ///     <para>Wenn nach Aufruf von Init() immer noch null, dann sind keine Streams für diese Episode verfügbar.</para>
            /// </summary>
            /// <seealso cref="Episode.Init" />
            public IEnumerable<KeyValuePair<Stream.StreamPartner, Stream>> Streams
            {
                get
                {
                    return this._streams ??
                           new List<KeyValuePair<Stream.StreamPartner, Stream>>();
                }
                private set { this._streams = value.ToList(); }
            }

            #endregion

            #region

            /// <summary>
            ///     Initialisiert das Objekt.
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
            ///                 <see cref="CaptchaException" />
            ///             </term>
            ///             <description>
            ///                 <see cref="CaptchaException" /> wird ausgelöst, wenn die Website das Ausfüllen eines Captcha
            ///                 erfordert.
            ///             </description>
            ///         </item>
            ///     </list>
            /// </summary>
            /// <seealso cref="Senpai.Login" />
            public async Task<ProxerResult> Init()
            {
                HtmlDocument lDocument = new HtmlDocument();
                ProxerResult<string> lResult =
                    await
                        HttpUtility.GetResponseErrorHandling(
                            "https://proxer.me/watch/" + this.ParentAnime.Id + "/" + this.EpisodeNr + "/" +
                            this._lang.ToString().ToLower(),
                            this._senpai.MobileLoginCookies,
                            this._senpai.ErrHandler,
                            this._senpai);

                if (!lResult.Success)
                    return new ProxerResult(lResult.Exceptions);

                string lResponse = lResult.Result;

                try
                {
                    lDocument.LoadHtml(lResponse);

                    HtmlNode[] lAllHtmlNodes = Utility.GetAllHtmlNodes(lDocument.DocumentNode.ChildNodes).ToArray();

                    if (
                        lAllHtmlNodes.Any(
                            x =>
                                x.Name.Equals("img") && x.HasAttributes &&
                                x.GetAttributeValue("src", "").Equals("/images/misc/stopyui.jpg")))
                        return new ProxerResult(new Exception[] {new CaptchaException()});

                    if (
                        lAllHtmlNodes.Any(
                            x =>
                                x.Name.Equals("img") && x.HasAttributes &&
                                x.GetAttributeValue("src", "").Equals("/images/misc/404.png")))
                        return new ProxerResult(new Exception[] {new WrongResponseException {Response = lResponse}});

                    List<KeyValuePair<Stream.StreamPartner, Stream>> lStreams =
                        new List<KeyValuePair<Stream.StreamPartner, Stream>>();

                    foreach (
                        HtmlNode childNode in
                            lDocument.DocumentNode.ChildNodes[1].ChildNodes[2].FirstChild.ChildNodes[1].ChildNodes[1]
                                .ChildNodes.Where(childNode => !childNode.FirstChild.Name.Equals("#text")))
                    {
                        switch (childNode.InnerText)
                        {
                            case "Viewster":
                                lStreams.Add(
                                    new KeyValuePair<Stream.StreamPartner, Stream>(Stream.StreamPartner.Viewster,
                                        new Stream(
                                            new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                            Stream.StreamPartner.Viewster)));
                                break;

                            case "Crunchyroll (EN)":
                                lStreams.Add(
                                    new KeyValuePair<Stream.StreamPartner, Stream>(Stream.StreamPartner.Crunchyroll,
                                        new Stream(
                                            new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                            Stream.StreamPartner.Crunchyroll)));
                                break;

                            case "Dailymotion":
                                lStreams.Add(
                                    new KeyValuePair<Stream.StreamPartner, Stream>(Stream.StreamPartner.Dailymotion,
                                        new Stream(
                                            new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                            Stream.StreamPartner.Dailymotion)));
                                break;
                            case "MP4Upload":
                                lStreams.Add(
                                    new KeyValuePair<Stream.StreamPartner, Stream>(Stream.StreamPartner.Mp4Upload,
                                        new Stream(
                                            new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                            Stream.StreamPartner.Mp4Upload)));
                                break;
                            case "Streamcloud":
                                lStreams.Add(
                                    new KeyValuePair<Stream.StreamPartner, Stream>(Stream.StreamPartner.Streamcloud,
                                        new Stream(
                                            new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                            Stream.StreamPartner.Streamcloud)));
                                break;
                            case "Videobam":
                                lStreams.Add(
                                    new KeyValuePair<Stream.StreamPartner, Stream>(Stream.StreamPartner.Videobam,
                                        new Stream(
                                            new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                            Stream.StreamPartner.Videobam)));
                                break;
                            case "YourUpload":
                                lStreams.Add(
                                    new KeyValuePair<Stream.StreamPartner, Stream>(Stream.StreamPartner.YourUpload,
                                        new Stream(
                                            new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                            Stream.StreamPartner.YourUpload)));
                                break;
                            case "Proxer-Stream":
                                lStreams.Add(
                                    new KeyValuePair<Stream.StreamPartner, Stream>(Stream.StreamPartner.ProxerStream,
                                        new Stream(
                                            new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                            Stream.StreamPartner.ProxerStream)));
                                break;
                        }
                        this.Streams = lStreams;
                    }

                    this.IstInitialisiert = true;

                    return new ProxerResult();
                }
                catch
                {
                    return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
                }
            }

            /// <summary>
            ///     Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            ///     A string that represents the current object.
            /// </returns>
            public override string ToString()
            {
                return "Episode " + this.EpisodeNr;
            }

            #endregion

            /// <summary>
            ///     Eine Klasse, die einen <see cref="Stream">Stream</see> einer <see cref="Episode">Episode</see> eines
            ///     <see cref="Anime" />
            ///     darstellt.
            /// </summary>
            public class Stream
            {
                /// <summary>
                ///     Eine Enumeration, die alle unterstützten Streampartner aufführt.
                /// </summary>
                public enum StreamPartner
                {
                    /// <summary>
                    ///     Stellt den Streampartner Crunchyroll dar.
                    /// </summary>
                    Crunchyroll,

                    /// <summary>
                    ///     Stellt den Streampartner Viewster dar.
                    /// </summary>
                    Viewster,

                    /// <summary>
                    ///     Stellt den Streampartner Streamcloud dar.
                    /// </summary>
                    Streamcloud,

                    /// <summary>
                    ///     Stellt den Streampartner MP4Upload dar.
                    /// </summary>
                    Mp4Upload,

                    /// <summary>
                    ///     Stellt den Streampartner Dailymotion dar.
                    /// </summary>
                    Dailymotion,

                    /// <summary>
                    ///     Stellt den Streampartner Videobam dar.
                    /// </summary>
                    Videobam,

                    /// <summary>
                    ///     Stellt den Streampartner YourUpload dar.
                    /// </summary>
                    YourUpload,

                    /// <summary>
                    ///     Stellt den hauseigenen Stream von Proxer dar.
                    /// </summary>
                    ProxerStream,

                    /// <summary>
                    ///     Stellt keinen Streampartner dar.
                    /// </summary>
                    None
                }

                internal Stream(Uri link, StreamPartner streamPartner)
                {
                    this.Link = link;
                    this.SPartner = streamPartner;
                }

                #region Properties

                /// <summary>
                ///     Gibt den Link des <see cref="Stream">Streams</see> zurück.
                /// </summary>
                public Uri Link { get; private set; }

                /// <summary>
                ///     Gibt den <see cref="StreamPartner">Streampartner</see> des <see cref="Stream">Streams</see> zurück.
                /// </summary>
                public StreamPartner SPartner { get; private set; }

                #endregion
            }
        }
    }
}