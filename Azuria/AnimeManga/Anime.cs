using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.User.Comment;
using Azuria.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Properties;
using Azuria.Utilities.Web;
using HtmlAgilityPack;
using JetBrains.Annotations;

// ReSharper disable LoopCanBeConvertedToQuery

namespace Azuria.AnimeManga
{
    /// <summary>
    ///     Represents an anime.
    /// </summary>
    [DebuggerDisplay("Anime: {Name} [{Id}]")]
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
            this.Rating = new InitialisableProperty<AnimeMangaRating>(this.InitMain);
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

        #region Inherited

        /// <summary>
        ///     Gets the count of the <see cref="Episode">Episodes</see> the <see cref="Anime" /> contains.
        /// </summary>
        public InitialisableProperty<int> ContentCount { get; }

        /// <summary>
        ///     Gets the link to the cover of the <see cref="Anime" />.
        /// </summary>
        public Uri CoverUri => new Uri($"https://cdn.proxer.me/cover/{this.Id}.jpg");

        /// <summary>
        ///     Gets the description of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<string> Description { get; }

        /// <summary>
        ///     Gets the english title of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<string> EnglishTitle { get; }

        /// <summary>
        ///     Gets an enumeration of the age restrictions of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<FskObject>> Fsk { get; }

        /// <summary>
        ///     Gets an enumeration of all the genre of the <see cref="Anime" /> contains.
        /// </summary>
        public InitialisableProperty<IEnumerable<GenreObject>> Genre { get; }

        /// <summary>
        ///     Gets the german title of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<string> GermanTitle { get; }

        /// <summary>
        ///     Gets an enumeration of all the groups that translated the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<Group>> Groups { get; }

        /// <summary>
        ///     Gets the Id of the <see cref="Anime" />.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets an enumeration of all the companies that were involved in making the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<Industry>> Industry { get; }

        /// <summary>
        ///     Gets if the <see cref="Anime" /> is licensed by a german company.
        /// </summary>
        public InitialisableProperty<bool> IsLicensed { get; }

        /// <summary>
        ///     Gets the japanese title of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<string> JapaneseTitle { get; }

        /// <summary>
        ///     Gets the original title of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<string> Name { get; }

        /// <summary>
        ///     Gets the rating of the <see cref="Anime" /> or <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<AnimeMangaRating> Rating { get; }

        /// <summary>
        ///     Gets the seasons the <see cref="Anime" /> aired in. If the enumerable only contains one value the value is always
        ///     the start season of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<string>> Season { get; }

        /// <summary>
        ///     Gets the status of the <see cref="Anime" />.
        /// </summary>
        public InitialisableProperty<AnimeMangaStatus> Status { get; }

        /// <summary>
        ///     Gets the synonym the <see cref="Anime" /> is also known as.
        /// </summary>
        public InitialisableProperty<string> Synonym { get; }

        /// <summary>
        ///     Adds the <see cref="Anime" /> to the planned list. If <paramref name="userControlPanel" /> is specified the object
        ///     is also added to the corresponding <see cref="UserControlPanel.Anime" />-enumeration.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
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
        ///     Gets the type of the <see cref="Anime" />.
        /// </summary>
        [NotNull]
        public InitialisableProperty<AnimeType> AnimeTyp { get; }

        /// <summary>
        ///     Gets the languages the <see cref="Anime" /> is available in.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<AnimeLanguage>> AvailableLanguages { get; }

        #endregion

        #region

        /// <summary>
        ///     Adds the <see cref="Anime" /> to the planned list. If <paramref name="userControlPanel" /> is specified the object
        ///     is also added to the corresponding <see cref="UserControlPanel.Anime" />-enumeration.
        /// </summary>
        /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
        /// <returns>If the action was successful.</returns>
        public async Task<ProxerResult<AnimeMangaUcpObject<Anime>>> AddToPlanned(
            UserControlPanel userControlPanel = null)
        {
            userControlPanel = userControlPanel ?? new UserControlPanel(this._senpai);
            return await userControlPanel.AddToPlanned(this);
        }

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> in a chronological order.
        /// </summary>
        /// <param name="startIndex">The offset of the comments parsed.</param>
        /// <param name="count">The count of the returned comments starting at <paramref name="startIndex" />.</param>
        /// <returns>If the action was successful and if it was, an enumeration of the comments.</returns>
        public async Task<ProxerResult<IEnumerable<Comment<Anime>>>> GetCommentsLatest(int startIndex, int count)
        {
            return
                await
                    Comment<Anime>.GetCommentsFromUrl(startIndex, count,
                        "https://proxer.me/info/" + this.Id + "/comments/",
                        "latest", this._senpai, this);
        }

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> ordered by rating.
        /// </summary>
        /// <param name="startIndex">The offset of the comments parsed.</param>
        /// <param name="count">The count of the returned comments starting at <paramref name="startIndex" />.</param>
        /// <returns>If the action was successful and if it was, an enumeration of the comments.</returns>
        public async Task<ProxerResult<IEnumerable<Comment<Anime>>>> GetCommentsRating(int startIndex, int count)
        {
            return
                await
                    Comment<Anime>.GetCommentsFromUrl(startIndex, count,
                        "https://proxer.me/info/" + this.Id + "/comments/",
                        "rating", this._senpai, this);
        }

        /// <summary>
        ///     Returns all <see cref="Episode">epsiodes</see> of the <see cref="Anime" /> in a specified language.
        /// </summary>
        /// <param name="language">The language of the episodes.</param>
        /// <seealso cref="Episode" />
        /// <returns>An enumeration of <see cref="Episode">episodes</see> with a count of <see cref="ContentCount" />.</returns>
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
        ///     Returns the currently most popular <see cref="Anime" />.
        /// </summary>
        /// <param name="senpai">The user that makes the request.</param>
        /// <returns>An enumeration of the currently most popular <see cref="Anime" />.</returns>
        [ItemNotNull]
        public static async Task<ProxerResult<IEnumerable<Anime>>> GetPopularAnime([NotNull] Senpai senpai)
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/anime?format=raw"),
                        null,
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
            return await this.InitMainInfo(this._senpai);
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
        ///     Represents an episode of an <see cref="Anime" />.
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

            #region Inherited

            /// <summary>
            ///     Gets the <see cref="Episode" />-number.
            /// </summary>
            public int ContentIndex { get; }

            /// <summary>
            ///     Gets if the <see cref="Episode" /> is available.
            /// </summary>
            public InitialisableProperty<bool> IsAvailable { get; }

            /// <summary>
            ///     Gets whether the language of the <see cref="Episode" /> is <see cref="Language.English">english</see> or
            ///     <see cref="Language.German">german</see>.
            /// </summary>
            public Language GeneralLanguage
                =>
                    this.Language == AnimeLanguage.GerSub || this.Language == AnimeLanguage.GerDub
                        ? AnimeManga.Language.German
                        : this.Language == AnimeLanguage.EngSub || this.Language == AnimeLanguage.EngDub
                            ? AnimeManga.Language.English
                            : AnimeManga.Language.Unkown;

            /// <summary>
            ///     Gets the <see cref="Anime" /> this <see cref="Episode" /> belongs to.
            /// </summary>
            public Anime ParentObject { get; }

            /// <summary>
            ///     Adds the <see cref="Episode" /> to the bookmarks. If <paramref name="userControlPanel" /> is specified the object
            ///     is also added to the corresponding <see cref="UserControlPanel.AnimeBookmarks" />-enumeration.
            /// </summary>
            /// <param name="userControlPanel">The object which, if specified, this object is added to.</param>
            /// <returns>If the action was successful.</returns>
            public async Task<ProxerResult<AnimeMangaBookmarkObject<Anime>>> AddToBookmarks(
                UserControlPanel userControlPanel = null)
            {
                userControlPanel = userControlPanel ?? new UserControlPanel(this._senpai);
                return await userControlPanel.AddToBookmarks(this);
            }

            #endregion

            #region Properties

            /// <summary>
            ///     Gets the language of the episode
            /// </summary>
            public AnimeLanguage Language { get; }

            /// <summary>
            ///     Gets the available streams of the episode.
            /// </summary>
            [NotNull]
            public InitialisableProperty<IEnumerable<KeyValuePair<Stream.StreamPartner, Stream>>> Streams { get; }

            #endregion

            #region

            /// <summary>
            ///     Initialises the object.
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
                            new Uri("https://proxer.me/watch/" + this.ParentObject.Id + "/" + this.ContentIndex + "/" +
                                    this.Language.ToString().ToLower()),
                            this._senpai.MobileLoginCookies,
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
                return "Episode " + this.ContentIndex;
            }

            #endregion

            /// <summary>
            ///     Represents a stream of an <see cref="Episode" /> of an <see cref="Anime" />.
            /// </summary>
            public class Stream
            {
                /// <summary>
                ///     Represents a streampartner.
                /// </summary>
                public enum StreamPartner
                {
                    /// <summary>
                    ///     Represents the streampartner Crunchyroll.
                    /// </summary>
                    Crunchyroll,

                    /// <summary>
                    ///     Represents the streampartner Viewster.
                    /// </summary>
                    Viewster,

                    /// <summary>
                    ///     Represents the streampartner Streamcloud.
                    /// </summary>
                    Streamcloud,

                    /// <summary>
                    ///     Represents the streampartner MP4Upload.
                    /// </summary>
                    Mp4Upload,

                    /// <summary>
                    ///     Represents the streampartner Dailymotion.
                    /// </summary>
                    Dailymotion,

                    /// <summary>
                    ///     Represents the streampartner Videobam.
                    /// </summary>
                    Videobam,

                    /// <summary>
                    ///     Represents the streampartner YourUpload.
                    /// </summary>
                    YourUpload,

                    /// <summary>
                    ///     Represents the internal streampartner of Proxer.Me.
                    /// </summary>
                    ProxerStream,

                    /// <summary>
                    ///     Represents no streampartner.
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
                ///     Gets the link of the stream.
                /// </summary>
                [NotNull]
                public Uri Link { get; private set; }

                /// <summary>
                ///     Gets the streampartner of the stream.
                /// </summary>
                public StreamPartner Partner { get; private set; }

                #endregion
            }
        }
    }
}