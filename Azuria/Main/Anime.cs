using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Main.Minor;
using Azuria.Main.User.Comment;
using Azuria.Main.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using Azuria.Utilities.Properties;
using HtmlAgilityPack;
using JetBrains.Annotations;

// ReSharper disable LoopCanBeConvertedToQuery

namespace Azuria.Main
{
    /// <summary>
    ///     Eine Klasse, die einen <see cref="Anime" /> darstellt.
    /// </summary>
    public class Anime : IAnimeMangaObject
    {
        private readonly Senpai _senpai;

        internal Anime()
        {
            this.AnimeTyp = new InitialisableProperty<AnimeType>(this.InitType);
            this.AvailableLanguages = new InitialisableProperty<IEnumerable<AnimeLanguage>>(this.InitAvailableLang);
            this.ContentCount = new InitialisableProperty<int>(this.InitEpisodeCount);
            this.Description = new InitialisableProperty<string>(this.InitMain);
            this.EnglishTitle = new InitialisableProperty<string>(this.InitMain, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Fsk = new InitialisableProperty<IEnumerable<FskObject>>(this.InitMain);
            this.Genre = new InitialisableProperty<IEnumerable<GenreObject>>(this.InitMain);
            this.GermanTitle = new InitialisableProperty<string>(this.InitMain, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Groups = new InitialisableProperty<IEnumerable<Group>>(this.InitMain);
            this.Industry = new InitialisableProperty<IEnumerable<Industry>>(this.InitMain);
            this.IsLicensed = new InitialisableProperty<bool>(this.InitMain);
            this.JapaneseTitle = new InitialisableProperty<string>(this.InitMain, string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Name = new InitialisableProperty<string>(this.InitMain, string.Empty) {IsInitialisedOnce = false};
            this.Season = new InitialisableProperty<IEnumerable<string>>(this.InitMain);
            this.Status = new InitialisableProperty<AnimeMangaStatus>(this.InitMain);
            this.Synonym = new InitialisableProperty<string>(this.InitMain, string.Empty) {IsInitialisedOnce = false};
        }

        internal Anime([NotNull] string name, int id, [NotNull] Senpai senpai) : this()
        {
            this.Id = id;
            this._senpai = senpai;

            this.Name = new InitialisableProperty<string>(this.InitMain, name);
        }

        internal Anime([NotNull] string name, int id, [NotNull] Senpai senpai,
            [NotNull] IEnumerable<GenreObject> genreList, AnimeMangaStatus status,
            AnimeType type) : this(name, id, senpai)
        {
            this.Genre = new InitialisableProperty<IEnumerable<GenreObject>>(this.InitMain, genreList);
            this.Status = new InitialisableProperty<AnimeMangaStatus>(this.InitMain, status);
            this.AnimeTyp = new InitialisableProperty<AnimeType>(this.InitType, type);
        }

        #region Geerbt

        /// <summary>
        ///     Gibt den Link zum Cover des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public Uri CoverUri => new Uri("http://cdn.proxer.me/cover/" + this.Id + ".jpg");

        /// <summary>
        /// </summary>
        public InitialisableProperty<int> ContentCount { get; }

        /// <summary>
        ///     Gibt die Beschreibung des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<string> Description { get; }

        /// <summary>
        ///     Gibt den englische Titel des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<string> EnglishTitle { get; }

        /// <summary>
        ///     Gibt die Links zu allen FSK-Beschränkungen des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<IEnumerable<FskObject>> Fsk { get; }

        /// <summary>
        ///     Gitb die Genres des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<IEnumerable<GenreObject>> Genre { get; }

        /// <summary>
        ///     Gibt den deutschen Titel des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<string> GermanTitle { get; }

        /// <summary>
        ///     Gibt die Gruppen zurück, die den <see cref="Anime" /> oder <see cref="Manga" /> übersetzten oder übersetzt haben.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<IEnumerable<Group>> Groups { get; }

        /// <summary>
        ///     Gibt die ID des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gibt die Industrie des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<IEnumerable<Industry>> Industry { get; }

        /// <summary>
        ///     Gibt zurück, ob der <see cref="Anime" /> oder <see cref="Manga" /> lizensiert ist.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<bool> IsLicensed { get; }

        /// <summary>
        ///     Gibt den japanischen Titel des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<string> JapaneseTitle { get; }

        /// <summary>
        ///     Gibt den Namen des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<string> Name { get; }

        /// <summary>
        ///     Gibt zurück, ob es sich um einen <see cref="Anime" /> oder <see cref="Manga" /> handelt.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public AnimeMangaType ObjectType => AnimeMangaType.Anime;

        /// <summary>
        ///     Gibt die Season des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<IEnumerable<string>> Season { get; }

        /// <summary>
        ///     Gibt den Status des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<AnimeMangaStatus> Status { get; }

        /// <summary>
        ///     Gibt das Synonym des <see cref="Anime" /> oder <see cref="Manga" /> zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public InitialisableProperty<string> Synonym { get; }

        /// <summary>
        ///     Initialisiert das Objekt.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        public async Task<ProxerResult> Init()
        {
            return await this.InitAllInitalisableProperties();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        async Task<ProxerResult> IAnimeMangaObject.AddToPlanned(UserControlPanel userControlPanel)
        {
            return await this.AddToPlanned(userControlPanel);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gibt den Typ eines Anime zurück.
        /// </summary>
        [NotNull]
        public InitialisableProperty<AnimeType> AnimeTyp { get; }

        /// <summary>
        ///     Gibt die verfügbaren Sprachen des <see cref="Anime" /> zurück.
        /// </summary>
        /// <seealso cref="Language" />
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeLanguage>> AvailableLanguages { get; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <param name="userControlPanel"></param>
        /// <returns></returns>
        public async Task<ProxerResult<AnimeMangaUcpObject<Anime>>> AddToPlanned(
            UserControlPanel userControlPanel = null)
        {
            userControlPanel = userControlPanel ?? new UserControlPanel(this._senpai);
            return await userControlPanel.AddToPlanned(this);
        }

        /// <summary>
        ///     Gibt die Kommentare des <see cref="Anime" /> oder <see cref="Manga" /> chronologisch geordnet zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        /// <param name="startIndex">Der Start-Index der ausgegebenen Kommentare.</param>
        /// <param name="count">Die Anzahl der ausgegebenen Kommentare ab dem angegebenen <paramref name="startIndex" />.</param>
        /// <returns>Eine Aufzählung mit den Kommentaren.</returns>
        public async Task<ProxerResult<IEnumerable<Comment<Anime>>>> GetCommentsLatest(int startIndex, int count)
        {
            return
                await
                    Comment<Anime>.GetCommentsFromUrl(startIndex, count,
                        "https://proxer.me/info/" + this.Id + "/comments/",
                        "latest", this._senpai, this);
        }

        /// <summary>
        ///     Gibt die Kommentare des <see cref="Anime" /> oder <see cref="Manga" />, nach ihrer Beliebtheit sortiert, zurück.
        ///     <para>(Vererbt von <see cref="IAnimeMangaObject" />)</para>
        /// </summary>
        /// <param name="startIndex">Der Start-Index der ausgegebenen Kommentare.</param>
        /// <param name="count">Die Anzahl der ausgegebenen Kommentare ab dem angegebenen <paramref name="startIndex" />.</param>
        /// <returns>Eine Aufzählung mit den Kommentaren.</returns>
        public async Task<ProxerResult<IEnumerable<Comment<Anime>>>> GetCommentsRating(int startIndex, int count)
        {
            return
                await
                    Comment<Anime>.GetCommentsFromUrl(startIndex, count,
                        "https://proxer.me/info/" + this.Id + "/comments/",
                        "rating", this._senpai, this);
        }

        /// <summary>
        ///     Gibt die Episoden des <see cref="Anime" /> in einer bestimmten <see cref="Language">Sprache</see> zurück.
        /// </summary>
        /// <exception cref="LanguageNotAvailableException">
        ///     Wird ausgelöst, wenn der Anime nicht in der angegebenen Sprache
        ///     verfügbar ist.
        /// </exception>
        /// <param name="language">Die Sprache der Episoden.</param>
        /// <seealso cref="Episode" />
        /// <returns>Einen Array mit length = <see cref="ContentCount" />.</returns>
        [NotNull]
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<Episode>>> GetEpisodes(AnimeLanguage language)
        {
            if (!(await this.AvailableLanguages.GetObject(new AnimeLanguage[0])).Contains(language))
                return new ProxerResult<IEnumerable<Episode>>(new Exception[] {new LanguageNotAvailableException()});

            List<Episode> lEpisodes = new List<Episode>();
            for (int i = 1; i <= await this.ContentCount.GetObject(-1); i++)
            {
                lEpisodes.Add(new Episode(this, i, language, this._senpai));
            }

            return new ProxerResult<IEnumerable<Episode>>(lEpisodes.ToArray());
        }

        /// <summary>
        ///     Gibt die aktuell am beliebtesten <see cref="Anime" /> zurück.
        /// </summary>
        /// <exception cref="WrongResponseException">Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</exception>
        /// <param name="senpai">Der aktuelle Benutzer.</param>
        /// <returns>Ein Array mit den aktuell beliebtesten <see cref="Anime" />.</returns>
        [ItemNotNull]
        public static async Task<ProxerResult<IEnumerable<Anime>>> GetPopularAnime([NotNull] Senpai senpai)
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/anime?format=raw"),
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

        [ItemNotNull]
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
                        new Uri("https://proxer.me/edit/entry/" + this.Id + "/languages?format=raw"),
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

                List<AnimeLanguage> languageList = new List<AnimeLanguage>();
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
                            languageList.Add(AnimeLanguage.GerSub);
                            break;
                        case "EngSub":
                            languageList.Add(AnimeLanguage.EngSub);
                            break;
                        case "EngDub":
                            languageList.Add(AnimeLanguage.EngDub);
                            break;
                        case "GerDub":
                            languageList.Add(AnimeLanguage.GerDub);
                            break;
                    }
                }

                this.AvailableLanguages.SetInitialisedObject(languageList.ToArray());

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        [ItemNotNull]
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
                        new Uri("https://proxer.me/edit/entry/" + this.Id + "/count?format=raw"),
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

                this.ContentCount.SetInitialisedObject(
                    Convert.ToInt32(
                        lDocument.DocumentNode.ChildNodes[4].ChildNodes[5].FirstChild.ChildNodes[1].InnerText));
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            return new ProxerResult();
        }

        [ItemNotNull]
        private async Task<ProxerResult> InitMain()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<Tuple<string, CookieContainer>> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/info/" + this.Id + "?format=raw"),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai,
                        new Func<string, ProxerResult>[0], false);

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result.Item1;

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
                            this.Name.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                            break;
                        case "Eng. Titel":
                            this.EnglishTitle.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                            break;
                        case "Ger. Titel":
                            this.GermanTitle.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                            break;
                        case "Jap. Titel":
                            this.JapaneseTitle.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                            break;
                        case "Synonym":
                            this.Synonym.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                            break;
                        case "Genre":
                            List<GenreObject> lGenreList = new List<GenreObject>();
                            foreach (HtmlNode htmlNode in childNode.ChildNodes[1].ChildNodes.ToList())
                            {
                                if (htmlNode.Name.Equals("a"))
                                    lGenreList.Add(new GenreObject(htmlNode.InnerText));
                            }
                            this.Genre.SetInitialisedObject(lGenreList.ToArray());
                            break;
                        case "FSK":
                            List<FskObject> lTempList = new List<FskObject>();
                            foreach (
                                HtmlNode htmlNode in
                                    childNode.ChildNodes[1].ChildNodes.ToList()
                                        .Where(htmlNode => htmlNode.Name.Equals("span") &&
                                                           lTempList.All(
                                                               o =>
                                                                   o.Fsk !=
                                                                   FskHelper.StringToFskDictionary[
                                                                       htmlNode.FirstChild.GetAttributeValue("src",
                                                                           "/images/fsk/unknown.png")
                                                                           .GetTagContents("fsk/", ".png")
                                                                           .First()])))
                            {
                                string lFskString = htmlNode.FirstChild.GetAttributeValue("src",
                                    "/images/fsk/unknown.png")
                                    .GetTagContents("fsk/", ".png")
                                    .First();
                                lTempList.Add(new FskObject(FskHelper.StringToFskDictionary[lFskString],
                                    new Uri($"https://proxer.me/images/fsk/{lFskString}.png")));
                            }
                            this.Fsk.SetInitialisedObject(lTempList);
                            break;
                        case "Season":
                            List<string> lSeasonList = new List<string>();
                            foreach (HtmlNode htmlNode in childNode.ChildNodes[1].ChildNodes.ToList())
                            {
                                if (htmlNode.Name.Equals("a"))
                                    lSeasonList.Add(htmlNode.InnerText);
                            }
                            this.Season.SetInitialisedObject(lSeasonList);
                            break;
                        case "Status":
                            switch (childNode.ChildNodes[1].InnerText)
                            {
                                case "Airing":
                                    this.Status.SetInitialisedObject(AnimeMangaStatus.Airing);
                                    break;
                                case "Abgeschlossen":
                                    this.Status.SetInitialisedObject(AnimeMangaStatus.Completed);
                                    break;
                                case "Nicht erschienen (Pre-Airing)":
                                    this.Status.SetInitialisedObject(AnimeMangaStatus.PreAiring);
                                    break;
                                default:
                                    this.Status.SetInitialisedObject(AnimeMangaStatus.Canceled);
                                    break;
                            }
                            break;
                        case "Gruppen":
                            if (childNode.ChildNodes[1].InnerText.Contains("Keine Gruppen eingetragen.")) break;
                            this.Groups.SetInitialisedObject(from htmlNode in childNode.ChildNodes[1].ChildNodes
                                where htmlNode.Name.Equals("a")
                                select
                                    new Group(
                                        Convert.ToInt32(
                                            htmlNode.GetAttributeValue("href",
                                                "/translatorgroups?id=-1#top")
                                                .GetTagContents("/translatorgroups?id=", "#top")[0]), htmlNode.InnerText));
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
                                    lIndustryType = Minor.Industry.IndustryType.Studio;
                                else if (htmlNode.NextSibling.InnerText.Contains("Publisher"))
                                    lIndustryType = Minor.Industry.IndustryType.Publisher;
                                else if (htmlNode.NextSibling.InnerText.Contains("Producer"))
                                    lIndustryType = Minor.Industry.IndustryType.Producer;
                                else lIndustryType = Minor.Industry.IndustryType.Unknown;

                                lIndustries.Add(new Industry(Convert.ToInt32(
                                    htmlNode.GetAttributeValue("href", "/industry?id=-1#top")
                                        .GetTagContents("/industry?id=", "#top")[0]), htmlNode.InnerText, lIndustryType));
                            }
                            this.Industry.SetInitialisedObject(lIndustries.ToArray());
                            break;
                        case "Lizenz":
                            this.IsLicensed.SetInitialisedObject(
                                childNode.ChildNodes[1].InnerText.StartsWith("Lizenziert!"));
                            break;
                        case "Beschreibung:":
                            childNode.FirstChild.FirstChild.Remove();
                            string lTempString = "";
                            foreach (HtmlNode htmlNode in childNode.FirstChild.ChildNodes)
                            {
                                if (htmlNode.Name.Equals("br")) lTempString += "\n";
                                else lTempString += htmlNode.InnerText;
                            }
                            if (lTempString.StartsWith("\n")) lTempString = lTempString.TrimStart();
                            this.Description.SetInitialisedObject(lTempString);
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

        [ItemNotNull]
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
                        new Uri("https://proxer.me/edit/entry/" + this.Id + "/medium?format=raw"),
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
                        this.AnimeTyp.SetInitialisedObject(AnimeType.Series);
                        break;
                    case "movie":
                        this.AnimeTyp.SetInitialisedObject(AnimeType.Movie);
                        break;
                    case "ova":
                        this.AnimeTyp.SetInitialisedObject(AnimeType.Ova);
                        break;
                    default:
                        this.AnimeTyp.SetInitialisedObject(AnimeType.Series);
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
        public class Episode : IAnimeMangaContent<Anime>
        {
            private readonly Senpai _senpai;

            internal Episode([NotNull] Anime anime, int index, AnimeLanguage lang, [NotNull] Senpai senpai)
            {
                this.ParentObject = anime;
                this.ContentIndex = index;
                this.Language = lang;
                this._senpai = senpai;

                this.IsAvailable = new InitialisableProperty<bool>(this.InitInfo);
                this.Streams =
                    new InitialisableProperty<IEnumerable<KeyValuePair<Stream.StreamPartner, Stream>>>(
                        this.InitInfo);
            }

            internal Episode([NotNull] Anime anime, int index, AnimeLanguage lang, bool isAvailable,
                [NotNull] Senpai senpai)
                : this(anime, index, lang, senpai)
            {
                this.IsAvailable = new InitialisableProperty<bool>(this.InitInfo, isAvailable);
            }

            #region Geerbt

            /// <summary>
            /// </summary>
            /// <returns></returns>
            public async Task<ProxerResult<AnimeMangaBookmarkObject<Anime>>> AddToBookmarks(
                UserControlPanel userControlPanel = null)
            {
                userControlPanel = userControlPanel ?? new UserControlPanel(this._senpai);
                return await userControlPanel.AddToBookmarks(this);
            }

            /// <summary>
            /// </summary>
            public Anime ParentObject { get; }

            /// <summary>
            /// </summary>
            public InitialisableProperty<bool> IsAvailable { get; }

            /// <summary>
            /// </summary>
            public int ContentIndex { get; }

            Language IAnimeMangaContent<Anime>.GeneralLanguage
                =>
                    this.Language == AnimeLanguage.GerSub || this.Language == AnimeLanguage.GerDub
                        ? Minor.Language.German
                        : this.Language == AnimeLanguage.EngSub || this.Language == AnimeLanguage.EngDub
                            ? Minor.Language.English
                            : Minor.Language.Unkown;

            #endregion

            #region Properties

            /// <summary>
            ///     Gibt die Nummer der Episode zurück.
            /// </summary>
            public int EpisodeNumber { get; set; }

            /// <summary>
            ///     Gibt die Sprache der Episode zurück.
            /// </summary>
            public AnimeLanguage Language { get; }

            /// <summary>
            ///     Gibt die vorhandenen Streams der Episode zurück.
            /// </summary>
            [NotNull]
            public InitialisableProperty<IEnumerable<KeyValuePair<Stream.StreamPartner, Stream>>> Streams { get; }

            #endregion

            #region

            /// <summary>
            ///     Initialisiert das Objekt.
            /// </summary>
            [ItemNotNull, Obsolete("Bitte benutze die Methoden der jeweiligen Eigenschaften, um sie zu initalisieren!")]
            public async Task<ProxerResult> Init()
            {
                return await this.InitAllInitalisableProperties();
            }

            [ItemNotNull]
            private async Task<ProxerResult> InitInfo()
            {
                HtmlDocument lDocument = new HtmlDocument();
                ProxerResult<string> lResult =
                    await
                        HttpUtility.GetResponseErrorHandling(
                            new Uri("https://proxer.me/watch/" + this.ParentObject.Id + "/" + this.EpisodeNumber + "/" +
                                    this.Language.ToString().ToLower()),
                            this._senpai.MobileLoginCookies,
                            this._senpai.ErrHandler,
                            this._senpai);

                if (!lResult.Success)
                    return new ProxerResult(lResult.Exceptions);

                string lResponse = lResult.Result;

                try
                {
                    lDocument.LoadHtml(lResponse);

                    HtmlNode[] lAllHtmlNodes = lDocument.DocumentNode.DescendantsAndSelf().ToArray();

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
                                            new Uri("https:" +
                                                    childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                            Stream.StreamPartner.Viewster)));
                                break;

                            case "Crunchyroll (EN)":
                                lStreams.Add(
                                    new KeyValuePair<Stream.StreamPartner, Stream>(Stream.StreamPartner.Crunchyroll,
                                        new Stream(
                                            new Uri("https:" +
                                                    childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
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
                        this.Streams.SetInitialisedObject(lStreams);
                        this.IsAvailable.SetInitialisedObject(lStreams.Any());
                    }

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
                return "Episode " + this.EpisodeNumber;
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

                internal Stream([NotNull] Uri link, StreamPartner streamPartner)
                {
                    this.Link = link;
                    this.Partner = streamPartner;
                }

                #region Properties

                /// <summary>
                ///     Gibt den Link des <see cref="Stream">Streams</see> zurück.
                /// </summary>
                [NotNull]
                public Uri Link { get; private set; }

                /// <summary>
                ///     Gibt den <see cref="StreamPartner">Streampartner</see> des <see cref="Stream">Streams</see> zurück.
                /// </summary>
                public StreamPartner Partner { get; private set; }

                #endregion
            }
        }
    }
}