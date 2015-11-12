using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Proxer.API.Exceptions;
using Proxer.API.Main.Minor;
using Proxer.API.Utilities;
using Proxer.API.Utilities.Net;

namespace Proxer.API.Main
{
    /// <summary>
    ///     Eine Klasse, die einen <see cref="Manga" /> darstellt.
    /// </summary>
    public class Manga : IAnimeMangaObject
    {
        /// <summary>
        ///     Eine Enumeration, die die Sprache eines <see cref="Manga" /> darstellt.
        /// </summary>
        public enum Language
        {
            /// <summary>
            ///     Die Sprache ist Deutsch.
            /// </summary>
            Deutsch,

            /// <summary>
            ///     Die Sprache ist Englisch.
            /// </summary>
            English
        }

        private readonly Func<Task<ProxerResult>>[] _initFuncs;

        private readonly Senpai _senpai;
        private string _beschreibung;
        private string _englischTitel;
        private Dictionary<string, Uri> _fsk;
        private string[] _genre;
        private Group[] _gruppen;
        private Industry[] _industrie;
        private string _japanTitel;
        private string[] _season;
        private Language[] _sprachen;
        private string _synonym;

        internal Manga(string name, int id, Senpai senpai)
        {
            this._senpai = senpai;
            this._initFuncs = new Func<Task<ProxerResult>>[]
            {this.InitMain, this.InitAvailableLang, this.InitChapterCount};
            this.ObjectType = AnimeMangaType.Manga;
            this.Name = name;
            this.Id = id;
            this.CoverUri = new Uri("http://cdn.proxer.me/cover/" + this.Id + ".jpg");

            this.IstInitialisiert = false;
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
        public Dictionary<string, Uri> Fsk
        {
            get { return this._fsk ?? new Dictionary<string, Uri>(); }
            private set { this._fsk = value; }
        }


        /// <summary>
        ///     Gitb die Genres des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public string[] Genre
        {
            get { return this._genre ?? new string[0]; }
            private set { this._genre = value; }
        }


        /// <summary>
        ///     Gibt die Gruppen zurück, die den <see cref="Anime" /> oder <see cref="Manga" /> übersetzten oder übersetzt haben.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Minor.Group" />
        /// <seealso cref="Init" />
        public Group[] Gruppen
        {
            get { return this._gruppen ?? new Group[0]; }
            private set { this._gruppen = value; }
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
        public Industry[] Industrie
        {
            get { return this._industrie ?? new Industry[0]; }
            private set { this._industrie = value; }
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
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
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
        public string[] Season
        {
            get { return this._season ?? new string[0]; }
            private set { this._season = value; }
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

        #endregion

        #region Properties

        /// <summary>
        ///     Gibt die Anzahl der Kapitel zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public int KapitelZahl { get; private set; }

        /// <summary>
        ///     Gibt die verfügbaren Sprachen zurück.
        ///     <para />
        ///     <para>Diese Eigenschaft muss durch <see cref="Init" /> initialisiert werden.</para>
        /// </summary>
        /// <seealso cref="Init" />
        public Language[] Sprachen
        {
            get { return this._sprachen ?? new Language[0]; }
            private set { this._sprachen = value; }
        }

        #endregion

        #region

        /// <summary>
        ///     Gibt alle <see cref="Chapter">Kapitel</see> des <see cref="Manga" /> in der ausgewählten Sprache zurück.
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
        /// <param name="lang">Die Sprache der <see cref="Chapter">Kapitel</see>.</param>
        /// <seealso cref="Sprachen" />
        /// <returns>Ein Array mit length = <see cref="KapitelZahl" /></returns>
        public ProxerResult<Chapter[]> GetChapters(Language lang)
        {
            if (this.Sprachen == null)
                return new ProxerResult<Chapter[]>(new Exception[] {new InitializeNeededException()});
            if (!this.Sprachen.Contains(lang))
                return new ProxerResult<Chapter[]>(new Exception[] {new LanguageNotAvailableException()});

            List<Chapter> lChapters = new List<Chapter>();
            for (int i = 1; i <= this.KapitelZahl; i++)
            {
                lChapters.Add(new Chapter(i, lang, this, this._senpai));
            }

            return new ProxerResult<Chapter[]>(lChapters.ToArray());
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
                        case "Jap. Titel":
                            this.JapanTitel = childNode.ChildNodes[1].InnerText;
                            break;
                        case "Synonym":
                            this.Synonym = childNode.ChildNodes[1].InnerText;
                            break;
                        case "Genre":
                            List<string> lGenreList = new List<string>();
                            childNode.ChildNodes[1].ChildNodes.ToList()
                                                   .ForEach(
                                                       delegate(HtmlNode htmlNode)
                                                       {
                                                           if (htmlNode.Name.Equals("a"))
                                                               lGenreList.Add(htmlNode.InnerText);
                                                       });
                            this.Genre = lGenreList.ToArray();
                            break;
                        case "FSK":
                            this.Fsk = new Dictionary<string, Uri>();
                            childNode.ChildNodes[1].ChildNodes.ToList()
                                                   .ForEach(
                                                       delegate(HtmlNode htmlNode)
                                                       {
                                                           if (htmlNode.Name.Equals("span") &&
                                                               !this.Fsk.ContainsKey(htmlNode.GetAttributeValue(
                                                                   "title", "ERROR")))
                                                               this.Fsk.Add(
                                                                   htmlNode.GetAttributeValue("title", "ERROR"),
                                                                   new Uri("https://proxer.me" +
                                                                           htmlNode.FirstChild.GetAttributeValue("src",
                                                                               "/")));
                                                       });
                            break;
                        case "Season":
                            List<string> lSeasonList = new List<string>();
                            childNode.ChildNodes[1].ChildNodes.ToList()
                                                   .ForEach(
                                                       delegate(HtmlNode htmlNode)
                                                       {
                                                           if (htmlNode.Name.Equals("a"))
                                                               lSeasonList.Add(htmlNode.InnerText);
                                                       });
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

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse).Exceptions);
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
                        "http://proxer.me/edit/entry/" + this.Id + "/languages?format=raw",
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
                        case "Englisch":
                            languageList.Add(Language.English);
                            break;
                        case "Deutsch":
                            languageList.Add(Language.Deutsch);
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

        private async Task<ProxerResult> InitChapterCount()
        {
            HtmlDocument lDocument = new HtmlDocument();
            Func<string, ProxerResult> lCheckFunc = s =>
            {
                if (!string.IsNullOrEmpty(s) &&
                    s.Equals("Bitte logge dich ein."))
                    return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitChapterCount))});

                return new ProxerResult();
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "http://proxer.me/edit/entry/" + this.Id + "/count?format=raw",
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

                this.KapitelZahl =
                    Convert.ToInt32(
                        lDocument.DocumentNode.ChildNodes[4]
                            .ChildNodes[5].FirstChild.ChildNodes[1].InnerText);

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        #endregion

        /// <summary>
        ///     Eine Klasse, die ein <see cref="Chapter">Kapitel</see> eines <see cref="Manga">Mangas</see> darstellt.
        /// </summary>
        public class Chapter
        {
            private readonly Func<Task<ProxerResult>>[] _initFuncs;
            private readonly Senpai _senpai;
            private Group _scanlatorGruppe;
            private Uri[] _seiten;
            private string _titel;
            private string _uploaderName;

            internal Chapter(int kapitelNr, Language lang, Manga parentManga, Senpai senpai)
            {
                this._senpai = senpai;
                this._initFuncs = new Func<Task<ProxerResult>>[]
                {this.InitInfo, this.InitChapters};
                this.KapitelNr = kapitelNr;
                this.Sprache = lang;
                this.ParentManga = parentManga;

                this.IstInitialisiert = false;
            }

            #region Properties

            /// <summary>
            ///     Gibt das Erscheinungsdatum des <see cref="Chapter">Kapitels</see> zurück.
            /// </summary>
            /// <seealso cref="Init" />
            public DateTime Datum { get; private set; }

            /// <summary>
            ///     Gibt zurück, ob das Objekt bereits initialisiert ist.
            /// </summary>
            public bool IstInitialisiert { get; private set; }

            /// <summary>
            ///     Gibt die Nummer des <see cref="Chapter">Kapitels</see> zurück.
            /// </summary>
            public int KapitelNr { get; }

            /// <summary>
            ///     Gibt den <see cref="Manga" /> zurück, zu dem das <see cref="Chapter">Kapitel</see> gehört.
            /// </summary>
            public Manga ParentManga { get; set; }

            /// <summary>
            ///     Gibt die <see cref="Group">Gruppe</see> zurück, die das Kapitel übersetzt hat.
            /// </summary>
            /// <seealso cref="Init" />
            public Group ScanlatorGruppe
            {
                get { return this._scanlatorGruppe ?? new Group(-1, ""); }
                private set { this._scanlatorGruppe = value; }
            }

            /// <summary>
            ///     Gibt die Links zu den einzelnen Seiten des <see cref="Chapter">Kapitels</see> zurück.
            /// </summary>
            /// <seealso cref="Init" />
            public Uri[] Seiten
            {
                get { return this._seiten ?? new Uri[0]; }
                private set { this._seiten = value; }
            }

            /// <summary>
            ///     Gibt die <see cref="Language">Sprache</see> des <see cref="Chapter">Kapitels</see> zurück.
            /// </summary>
            public Language Sprache { get; }

            /// <summary>
            ///     Gibt den Titel des <see cref="Chapter">Kapitels</see> zurück.
            /// </summary>
            /// <seealso cref="Init" />
            public string Titel
            {
                get { return this._titel ?? ""; }
                private set { this._titel = value; }
            }

            /// <summary>
            ///     Gibt den Namen des Uploaders des <see cref="Chapter">Kapitels</see> zurück.
            /// </summary>
            /// <seealso cref="Init" />
            public string UploaderName
            {
                get { return this._uploaderName ?? ""; }
                private set { this._uploaderName = value; }
            }

            /// <summary>
            ///     Gibt zurück, ob das <see cref="Chapter">Kapitel</see> verfügbar ist.
            /// </summary>
            /// <seealso cref="Init" />
            public bool Verfuegbar { get; private set; }

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

            private async Task<ProxerResult> InitInfo()
            {
                HtmlDocument lDocument = new HtmlDocument();
                Func<string, ProxerResult> lCheckFunc = s =>
                {
                    if (!string.IsNullOrEmpty(s) &&
                        s.Equals("Du hast keine Berechtigung um diese Seite zu betreten."))
                        return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitInfo))});

                    return new ProxerResult();
                };
                ProxerResult<string> lResult =
                    await
                        HttpUtility.GetResponseErrorHandling(
                            "https://proxer.me/chapter/" + this.ParentManga.Id + "/" + this.KapitelNr + "/" +
                            this.Sprache.ToString().ToLower().Substring(0, 2) + "?format=raw",
                            null,
                            this._senpai.ErrHandler,
                            this._senpai,
                            new[] {lCheckFunc});

                if (!lResult.Success)
                    return new ProxerResult(lResult.Exceptions);

                string lResponse = lResult.Result;

                if (lResponse.Contains("Dieses Kapitel ist leider noch nicht verfügbar :/"))
                {
                    this.Verfuegbar = false;
                    return new ProxerResult();
                }

                this.Verfuegbar = true;

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

                    foreach (
                        HtmlNode childNode in
                            lDocument.DocumentNode.SelectNodes("//table[@class='details']")[0].ChildNodes)
                    {
                        switch (childNode.FirstChild.InnerText)
                        {
                            case "Titel":
                                this.Titel = childNode.ChildNodes[1].InnerText;
                                break;
                            case "Uploader":
                                this.UploaderName = childNode.ChildNodes[1].InnerText;
                                break;
                            case "Scanlator-Gruppe":
                                if (childNode.ChildNodes[1].InnerText.Equals("siehe Kapitelcredits"))
                                    this.ScanlatorGruppe = new Group(-1, "siehe Kapitelcredits");
                                else
                                    this.ScanlatorGruppe = new Group(
                                        Convert.ToInt32(
                                            Utility.GetTagContents(
                                                childNode.ChildNodes[1].FirstChild.GetAttributeValue("href",
                                                    "/translatorgroups?id=-1#top"),
                                                "/translatorgroups?id=", "#top")[0]), childNode.ChildNodes[1].InnerText);
                                break;
                            case "Datum":
                                this.Datum = Utility.ToDateTime(childNode.ChildNodes[1].InnerText);
                                break;
                        }
                    }

                    return new ProxerResult();
                }
                catch
                {
                    return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
                }
            }

            private async Task<ProxerResult> InitChapters()
            {
                HtmlDocument lDocument = new HtmlDocument();
                Func<string, ProxerResult> lCheckFunc = s =>
                {
                    if (!string.IsNullOrEmpty(s) &&
                        s.Equals("Du hast keine Berechtigung um diese Seite zu betreten."))
                        return new ProxerResult(new Exception[] {new NoAccessException(nameof(this.InitInfo))});

                    return new ProxerResult();
                };
                ProxerResult<string> lResult =
                    await
                        HttpUtility.GetResponseErrorHandling(
                            "https://proxer.me/read/" + this.ParentManga.Id + "/" + this.KapitelNr + "/" +
                            this.Sprache.ToString().ToLower().Substring(0, 2) + "?format=json",
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

                    this.Seiten =
                        (from s in
                            Utility.GetTagContents(lDocument.DocumentNode.ChildNodes[1].InnerText.Split(';')[0], "[",
                                "]")
                         where !s.StartsWith("[")
                         select
                             new Uri("http://upload.proxer.me/manga/" + this.ParentManga.Id + "_" +
                                     this.Sprache.ToString().ToLower().Substring(0, 2) + "/" + this.KapitelNr + "/" +
                                     Utility.GetTagContents(s, "\"", "\"")[0])).ToArray();

                    return new ProxerResult();
                }
                catch
                {
                    return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
                }
            }

            #endregion
        }
    }
}