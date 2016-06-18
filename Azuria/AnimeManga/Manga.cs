using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.User.Comment;
using Azuria.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using Azuria.Utilities.Properties;
using HtmlAgilityPack;
using JetBrains.Annotations;

// ReSharper disable LoopCanBeConvertedToQuery

namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents a manga.
    /// </summary>
    [DebuggerDisplay("Manga: {Name} [{Id}]")]
    public class Manga : IAnimeMangaObject
    {
        /// <summary>
        ///     Represents the type of a manga.
        /// </summary>
        public enum MangaType
        {
            /// <summary>
            ///     Represents a series
            /// </summary>
            Series,

            /// <summary>
            ///     Represents an one-shot.
            /// </summary>
            OneShot,

            /// <summary>
            ///     Represents an unkwon type.
            /// </summary>
            Unknown
        }

        private readonly Senpai _senpai;

        [UsedImplicitly]
        internal Manga()
        {
            this.AvailableLanguages = new InitialisableProperty<IEnumerable<Language>>(this.InitAvailableLang);
            this.ContentCount = new InitialisableProperty<int>(this.InitChapterCount);
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
            this.MangaTyp = new InitialisableProperty<MangaType>(this.InitType);
            this.Name = new InitialisableProperty<string>(this.InitMain, string.Empty) {IsInitialisedOnce = false};
            this.Season = new InitialisableProperty<IEnumerable<string>>(this.InitMain);
            this.Status = new InitialisableProperty<AnimeMangaStatus>(this.InitMain);
            this.Synonym = new InitialisableProperty<string>(this.InitMain, string.Empty) {IsInitialisedOnce = false};
        }

        internal Manga([NotNull] string name, int id, [NotNull] Senpai senpai) : this()
        {
            this.Id = id;
            this._senpai = senpai;

            this.Name = new InitialisableProperty<string>(this.InitMain, name);
        }

        internal Manga([NotNull] string name, int id, [NotNull] Senpai senpai,
            [NotNull] IEnumerable<GenreObject> genreList, AnimeMangaStatus status,
            MangaType type) : this(name, id, senpai)
        {
            this.Genre = new InitialisableProperty<IEnumerable<GenreObject>>(this.InitMain, genreList);
            this.Status = new InitialisableProperty<AnimeMangaStatus>(this.InitMain, status);
            this.MangaTyp = new InitialisableProperty<MangaType>(this.InitType, type);
        }

        #region Geerbt

        /// <summary>
        ///     Gets the count of the <see cref="Chapter">Chapters</see> the <see cref="Manga" /> contains.
        /// </summary>
        public InitialisableProperty<int> ContentCount { get; }

        /// <summary>
        ///     Gets the link to the cover of the <see cref="Manga" />.
        /// </summary>
        public Uri CoverUri => new Uri($"https://cdn.proxer.me/cover/{this.Id}.jpg");

        /// <summary>
        ///     Gets the description of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> Description { get; }

        /// <summary>
        ///     Gets the english title of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> EnglishTitle { get; }

        /// <summary>
        ///     Gets an enumeration of the age restrictions of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<FskObject>> Fsk { get; }

        /// <summary>
        ///     Gets an enumeration of all the genre of the <see cref="Manga" /> contains.
        /// </summary>
        public InitialisableProperty<IEnumerable<GenreObject>> Genre { get; }

        /// <summary>
        ///     Gets the german title of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> GermanTitle { get; }

        /// <summary>
        ///     Gets an enumeration of all the groups that translated the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<Group>> Groups { get; }

        /// <summary>
        ///     Gets the Id of the <see cref="Manga" />.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets an enumeration of all the companies that were involved in making the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<Industry>> Industry { get; }

        /// <summary>
        ///     Gets if the <see cref="Manga" /> is licensed by a german company.
        /// </summary>
        public InitialisableProperty<bool> IsLicensed { get; }

        /// <summary>
        ///     Gets the japanese title of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> JapaneseTitle { get; }

        /// <summary>
        ///     Gets the original title of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> Name { get; }

        /// <summary>
        ///     Gets the seasons the <see cref="Manga" /> aired in. If the enumerable only contains one
        ///     value the value is always the start season of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<string>> Season { get; }

        /// <summary>
        ///     Gets the status of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<AnimeMangaStatus> Status { get; }

        /// <summary>
        ///     Gets the synonym the <see cref="Manga" /> is also known as.
        /// </summary>
        public InitialisableProperty<string> Synonym { get; }

        /// <summary>
        ///     Adds the <see cref="Manga" /> to the planned list. If <paramref name="userControlPanel" />
        ///     is specified the object is also added to the corresponding <see cref="UserControlPanel.Manga" />-enumeration.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, the object is added to.</param>
        /// <returns>If the action was successful.</returns>
        async Task<ProxerResult> IAnimeMangaObject.AddToPlanned(UserControlPanel userControlPanel)
        {
            return await this.AddToPlanned(userControlPanel);
        }

        /// <summary>
        ///     Initialises the object.
        /// </summary>
        public async Task<ProxerResult> Init()
        {
            return await this.InitAllInitalisableProperties();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the languages the <see cref="Manga" /> is available in.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<Language>> AvailableLanguages { get; }

        /// <summary>
        ///     Gets the type of the <see cref="Manga" />.
        /// </summary>
        [NotNull]
        public InitialisableProperty<MangaType> MangaTyp { get; }

        #endregion

        #region

        /// <summary>
        ///     Adds the <see cref="Manga" /> to the planned list. If <paramref name="userControlPanel" />
        ///     is specified the object is also added to the corresponding <see cref="UserControlPanel.Manga" />-enumeration.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, the object is added to.</param>
        /// <returns>If the action was successful and if was, the new entry.</returns>
        public async Task<ProxerResult<AnimeMangaUcpObject<Manga>>> AddToPlanned(
            UserControlPanel userControlPanel = null)
        {
            userControlPanel = userControlPanel ?? new UserControlPanel(this._senpai);
            return await userControlPanel.AddToPlanned(this);
        }

        /// <summary>
        ///     Returns all <see cref="Chapter">epsiodes</see> of the <see cref="Manga" /> in a specified language.
        /// </summary>
        /// <param name="language">The language of the episodes.</param>
        /// <seealso cref="Chapter" />
        /// <returns>An enumeration of <see cref="Chapter">episodes</see> with a count of <see cref="ContentCount" />.</returns>
        [NotNull]
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<Chapter>>> GetChapters(Language language)
        {
            if (!(await this.AvailableLanguages.GetObject(new Language[0])).Contains(language))
                return new ProxerResult<IEnumerable<Chapter>>(new Exception[] {new LanguageNotAvailableException()});

            List<Chapter> lChapters = new List<Chapter>();
            for (int i = 1; i <= await this.ContentCount.GetObject(-1); i++)
            {
                lChapters.Add(new Chapter(this, i, language, this._senpai));
            }

            return new ProxerResult<IEnumerable<Chapter>>(lChapters.ToArray());
        }

        /// <summary>
        ///     Gets the comments of the <see cref="Manga" /> in a chronological order.
        /// </summary>
        /// <param name="startIndex">The offset of the comments parsed.</param>
        /// <param name="count">The count of the returned comments starting at <paramref name="startIndex" />.</param>
        /// <returns>If the action was successful and if it was, an enumeration of the comments.</returns>
        public async Task<ProxerResult<IEnumerable<Comment<Manga>>>> GetCommentsLatest(int startIndex, int count)
        {
            return
                await
                    Comment<Manga>.GetCommentsFromUrl(startIndex, count,
                        "https://proxer.me/info/" + this.Id + "/comments/",
                        "latest", this._senpai, this);
        }

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> ordered by rating.
        /// </summary>
        /// <param name="startIndex">The offset of the comments parsed.</param>
        /// <param name="count">The count of the returned comments starting at <paramref name="startIndex" />.</param>
        /// <returns>If the action was successful and if it was, an enumeration of the comments.</returns>
        public async Task<ProxerResult<IEnumerable<Comment<Manga>>>> GetCommentsRating(int startIndex, int count)
        {
            return
                await
                    Comment<Manga>.GetCommentsFromUrl(startIndex, count,
                        "https://proxer.me/info/" + this.Id + "/comments/",
                        "rating", this._senpai, this);
        }

        /// <summary>
        ///     Returns the currently most popular <see cref="Manga" />.
        /// </summary>
        /// <param name="senpai">The user that makes the request.</param>
        /// <returns>An enumeration of the currently most popular <see cref="Manga" />.</returns>
        [ItemNotNull]
        public static async Task<ProxerResult<IEnumerable<Manga>>> GetPopularManga([NotNull] Senpai senpai)
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/manga?format=raw"),
                        null,
                        senpai);

            if (!lResult.Success)
                return new ProxerResult<IEnumerable<Manga>>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                return
                    new ProxerResult<IEnumerable<Manga>>(
                        (from childNode in lDocument.DocumentNode.ChildNodes[5].FirstChild.FirstChild.ChildNodes
                            let lId =
                                Convert.ToInt32(
                                    childNode.FirstChild.GetAttributeValue("href", "/info/-1#top").Split('/')[2].Split(
                                        '#')
                                        [0])
                            select new Manga(childNode.FirstChild.GetAttributeValue("title", "ERROR"), lId, senpai))
                            .ToArray());
            }
            catch
            {
                return new ProxerResult<IEnumerable<Manga>>(ErrorHandler.HandleError(senpai, lResponse).Exceptions);
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
                        new Uri("http://proxer.me/edit/entry/" + this.Id + "/languages?format=raw"),
                        this._senpai.LoginCookies,
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
                            languageList.Add(Language.German);
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
                        new Uri("http://proxer.me/edit/entry/" + this.Id + "/count?format=raw"),
                        this._senpai.LoginCookies,
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
                        lDocument.DocumentNode.ChildNodes[4]
                            .ChildNodes[5].FirstChild.ChildNodes[1].InnerText));

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        [ItemNotNull]
        private async Task<ProxerResult> InitMain()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<Tuple<string, CookieContainer>> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/info/" + this.Id + "?format=raw"),
                        null,
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
                                                                   o.FskType !=
                                                                   FskHelper.StringToFskDictionary[
                                                                       htmlNode.FirstChild.GetAttributeValue("src",
                                                                           "/images/fsk/unknown.png")
                                                                           .GetTagContents("/", ".png")
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
                                    this.Status.SetInitialisedObject(AnimeMangaStatus.Cancelled);
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
                                    lIndustryType = AnimeManga.Industry.IndustryType.Studio;
                                else if (htmlNode.NextSibling.InnerText.Contains("Publisher"))
                                    lIndustryType = AnimeManga.Industry.IndustryType.Publisher;
                                else if (htmlNode.NextSibling.InnerText.Contains("Producer"))
                                    lIndustryType = AnimeManga.Industry.IndustryType.Producer;
                                else lIndustryType = AnimeManga.Industry.IndustryType.Unknown;

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
                        new Uri("http://proxer.me/edit/entry/" + this.Id + "/medium?format=raw"),
                        this._senpai.LoginCookies,
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
                    case "mangaseries":
                        this.MangaTyp.SetInitialisedObject(MangaType.Series);
                        break;
                    case "oneshot":
                        this.MangaTyp.SetInitialisedObject(MangaType.OneShot);
                        break;
                    default:
                        this.MangaTyp.SetInitialisedObject(MangaType.Series);
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
        ///     Represents a chapter of a <see cref="Manga" />.
        /// </summary>
        public class Chapter : IAnimeMangaContent<Manga>
        {
            private readonly Senpai _senpai;

            internal Chapter([NotNull] Manga parentManga, int chapterNumber, Language lang, [NotNull] Senpai senpai)
            {
                this._senpai = senpai;
                this.ContentIndex = chapterNumber;
                this.Language = lang;
                this.ParentObject = parentManga;

                this.IsAvailable = new InitialisableProperty<bool>(this.InitInfo);
                this.Date = new InitialisableProperty<DateTime>(this.InitInfo);
                this.Pages = new InitialisableProperty<IEnumerable<Uri>>(this.InitPages);
                this.ScanlatorGroup = new InitialisableProperty<Group>(this.InitInfo);
                this.Titel = new InitialisableProperty<string>(this.InitInfo);
                this.UploaderName = new InitialisableProperty<string>(this.InitInfo);
            }

            internal Chapter([NotNull] Manga parentManga, int chapterNumber, Language lang, bool isOnline,
                [NotNull] Senpai senpai) : this(parentManga, chapterNumber, lang, senpai)
            {
                this.IsAvailable = new InitialisableProperty<bool>(this.InitInfo, isOnline);
            }

            #region Geerbt

            /// <summary>
            ///     Gets the <see cref="Chapter" />-number.
            /// </summary>
            public int ContentIndex { get; }

            /// <summary>
            ///     Gets if the <see cref="Chapter" /> is available.
            /// </summary>
            public InitialisableProperty<bool> IsAvailable { get; }

            /// <summary>
            ///     Gets whether the language of the <see cref="Chapter" /> is
            ///     <see cref="Language.English">english</see> or <see cref="Language.German">german</see>.
            /// </summary>
            public Language GeneralLanguage => this.Language;

            /// <summary>
            ///     Gets the <see cref="Manga" /> this <see cref="Chapter" /> belongs to.
            /// </summary>
            public Manga ParentObject { get; }

            /// <summary>
            ///     Adds the <see cref="Chapter" /> to the bookmarks. If <paramref name="userControlPanel" /> is specified
            ///     the object is also added to the corresponding <see cref="UserControlPanel.MangaBookmarks" />-enumeration.
            /// </summary>
            /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
            /// <returns>If the action was successful.</returns>
            public async Task<ProxerResult<AnimeMangaBookmarkObject<Manga>>> AddToBookmarks(
                UserControlPanel userControlPanel = null)
            {
                userControlPanel = userControlPanel ?? new UserControlPanel(this._senpai);
                return await userControlPanel.AddToBookmarks(this);
            }

            #endregion

            #region Properties

            /// <summary>
            ///     Returns the release date of the <see cref="Chapter" />.
            /// </summary>
            [NotNull]
            public InitialisableProperty<DateTime> Date { get; }

            /// <summary>
            ///     Gets the language of the <see cref="Chapter" />.
            /// </summary>
            public Language Language { get; }

            /// <summary>
            ///     Gets an enumeration containing all pages of the <see cref="Chapter" />.
            /// </summary>
            [NotNull]
            public InitialisableProperty<IEnumerable<Uri>> Pages { get; }

            /// <summary>
            ///     Gets the scanlator-group of the <see cref="Chapter" />.
            /// </summary>
            [NotNull]
            public InitialisableProperty<Group> ScanlatorGroup { get; }

            /// <summary>
            ///     Gets the title of the <see cref="Chapter" />.
            /// </summary>
            [NotNull]
            public InitialisableProperty<string> Titel { get; }

            /// <summary>
            ///     Gets the name of the uploader of the <see cref="Chapter" />.
            /// </summary>
            [NotNull]
            public InitialisableProperty<string> UploaderName { get; }

            #endregion

            #region

            /// <summary>
            ///     Initialises the object.
            /// </summary>
            [ItemNotNull]
            [Obsolete("Bitte benutze die Methoden der jeweiligen Eigenschaften, um sie zu initalisieren!")]
            public async Task<ProxerResult> Init()
            {
                return await this.InitAllInitalisableProperties();
            }

            [ItemNotNull]
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
                            new Uri("https://proxer.me/chapter/" + this.ParentObject.Id + "/" + this.ContentIndex + "/" +
                                    this.Language.ToString().ToLower().Substring(0, 2) + "?format=raw"),
                            null,
                            this._senpai,
                            new[] {lCheckFunc});

                if (!lResult.Success)
                    return new ProxerResult(lResult.Exceptions);

                string lResponse = lResult.Result;

                if (lResponse == null || lResponse.Contains("Dieses Kapitel ist leider noch nicht verfügbar :/"))
                {
                    this.IsAvailable.SetInitialisedObject(false);
                    return new ProxerResult();
                }

                this.IsAvailable.SetInitialisedObject(true);

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

                    foreach (
                        HtmlNode childNode in
                            lAllHtmlNodes.First(
                                x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "details")
                                .ChildNodes)
                    {
                        switch (childNode.FirstChild.InnerText)
                        {
                            case "Titel":
                                this.Titel.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                                break;
                            case "Uploader":
                                this.UploaderName.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                                break;
                            case "Scanlator-Gruppe":
                                if (childNode.ChildNodes[1].InnerText.Equals("siehe Kapitelcredits"))
                                    this.ScanlatorGroup.SetInitialisedObject(new Group(-1, "siehe Kapitelcredits"));
                                else
                                    this.ScanlatorGroup.SetInitialisedObject(new Group(
                                        Convert.ToInt32(
                                            childNode.ChildNodes[1].FirstChild.GetAttributeValue("href",
                                                "/translatorgroups?id=-1#top")
                                                .GetTagContents("/translatorgroups?id=", "#top")[0]),
                                        childNode.ChildNodes[1].InnerText));
                                break;
                            case "Datum":
                                this.Date.SetInitialisedObject(childNode.ChildNodes[1].InnerText.ToDateTime());
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

            [ItemNotNull]
            private async Task<ProxerResult> InitPages()
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
                            new Uri("https://proxer.me/read/" + this.ParentObject.Id + "/" + this.ContentIndex + "/" +
                                    this.Language.ToString().ToLower().Substring(0, 2) + "?format=json"),
                            this._senpai.MobileLoginCookies,
                            this._senpai,
                            new[] {lCheckFunc});

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

                    this.Pages.SetInitialisedObject(
                        (from s in lDocument.DocumentNode.ChildNodes[1].InnerText.Split(';')[0].GetTagContents("[", "]")
                            where !s.StartsWith("[")
                            select new Uri("http://upload.proxer.me/manga/" + this.ParentObject.Id + "_" +
                                           this.Language.ToString().ToLower().Substring(0, 2) + "/" + this.ContentIndex +
                                           "/" +
                                           s.GetTagContents("\"", "\"")[0])).ToArray());

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
                return "Chapter " + this.ContentIndex;
            }

            #endregion
        }
    }
}