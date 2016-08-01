using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga.Properties;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Api.v1.Enums;
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
    ///     Represents a manga.
    /// </summary>
    [DebuggerDisplay("Manga: {Name} [{Id}]")]
    public class Manga : IAnimeMangaObject
    {
        private readonly Senpai _senpai;

        [UsedImplicitly]
        internal Manga()
        {
            this.AvailableLanguages =
                new InitialisableProperty<IEnumerable<Language>>(() => this.InitAvailableLangApi(this._senpai));
            this.Clicks = new InitialisableProperty<int>(() => this.InitMainInfoApi(this._senpai));
            this.ContentCount = new InitialisableProperty<int>(() => this.InitMainInfoApi(this._senpai));
            this.Description = new InitialisableProperty<string>(() => this.InitMainInfoApi(this._senpai));
            this.EnglishTitle = new InitialisableProperty<string>(() => this.InitNamesApi(this._senpai), string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Fsk = new InitialisableProperty<IEnumerable<FskType>>(() => this.InitMainInfoApi(this._senpai));
            this.Genre = new InitialisableProperty<IEnumerable<GenreType>>(() => this.InitMainInfoApi(this._senpai));
            this.GermanTitle = new InitialisableProperty<string>(() => this.InitNamesApi(this._senpai), string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Groups = new InitialisableProperty<IEnumerable<Group>>(() => this.InitGroupsApi(this._senpai));
            this.Industry = new InitialisableProperty<IEnumerable<Industry>>(() => this.InitIndustryApi(this._senpai));
            this.IsLicensed = new InitialisableProperty<bool>(() => this.InitMainInfoApi(this._senpai));
            this.IsHContent = new InitialisableProperty<bool>(() => this.InitIsHContentApi(this._senpai));
            this.JapaneseTitle = new InitialisableProperty<string>(() => this.InitNamesApi(this._senpai), string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.MangaType = new InitialisableProperty<MangaType>(() => this.InitMainInfoApi(this._senpai));
            this.Name = new InitialisableProperty<string>(() => this.InitMainInfoApi(this._senpai), string.Empty)
            {
                IsInitialisedOnce = false
            };
            this.Rating = new InitialisableProperty<AnimeMangaRating>(() => this.InitMainInfoApi(this._senpai));
            this.Relations =
                new InitialisableProperty<IEnumerable<IAnimeMangaObject>>(() => this.InitRelationsApi(this._senpai));
            this.Season = new InitialisableProperty<AnimeMangaSeasonInfo>(() => this.InitSeasonsApi(this._senpai));
            this.Status = new InitialisableProperty<AnimeMangaStatus>(() => this.InitMainInfoApi(this._senpai));
            this.Synonym = new InitialisableProperty<string>(() => this.InitNamesApi(this._senpai), string.Empty)
            {
                IsInitialisedOnce = false
            };
        }

        internal Manga([NotNull] string name, int id, [NotNull] Senpai senpai) : this()
        {
            this.Id = id;
            this._senpai = senpai;

            this.Name.SetInitialisedObject(name);
        }

        internal Manga([NotNull] string name, int id, [NotNull] Senpai senpai,
            [NotNull] IEnumerable<GenreType> genreList, AnimeMangaStatus status,
            MangaType type) : this(name, id, senpai)
        {
            this.Genre.SetInitialisedObject(genreList);
            this.MangaType.SetInitialisedObject(type);
            this.Status.SetInitialisedObject(status);
        }

        internal Manga(EntryDataModel entryDataModel, Senpai senpai) : this()
        {
            if (entryDataModel.EntryType != AnimeMangaEntryType.Manga)
                throw new ArgumentException(nameof(entryDataModel.EntryType));

            this._senpai = senpai;
            this.Id = entryDataModel.EntryId;

            this.Clicks.SetInitialisedObject(entryDataModel.Clicks);
            this.ContentCount.SetInitialisedObject(entryDataModel.ContentCount);
            this.Description.SetInitialisedObject(entryDataModel.Description);
            this.Fsk.SetInitialisedObject(entryDataModel.Fsk);
            this.Genre.SetInitialisedObject(entryDataModel.Genre);
            this.IsLicensed.SetInitialisedObject(entryDataModel.IsLicensed);
            this.MangaType.SetInitialisedObject((MangaType) entryDataModel.Medium);
            this.Name.SetInitialisedObject(entryDataModel.Name);
            this.Rating.SetInitialisedObject(entryDataModel.Rating);
            this.Status.SetInitialisedObject(entryDataModel.State);
        }

        internal Manga(RelationDataModel dataModel, Senpai senpai) : this((EntryDataModel) dataModel, senpai)
        {
            this.AvailableLanguages.SetInitialisedObject(dataModel.AvailableLanguages.Cast<Language>());
        }

        #region Properties

        /// <summary>
        ///     Gets the languages the <see cref="Manga" /> is available in.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<Language>> AvailableLanguages { get; }

        /// <summary>
        ///     Gets the total amount of clicks the <see cref="Manga" /> recieved. Is reset every 3 months.
        /// </summary>
        public InitialisableProperty<int> Clicks { get; }

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> in a chronological order.
        /// </summary>
        public IEnumerable<Comment<Manga>> CommentsLatest => new CommentEnumerable<Manga>(this, "latest", this._senpai);

        /// <summary>
        ///     Gets the comments of the <see cref="Anime" /> ordered by rating.
        /// </summary>
        public IEnumerable<Comment<Manga>> CommentsRating => new CommentEnumerable<Manga>(this, "rating", this._senpai);

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
        public InitialisableProperty<IEnumerable<FskType>> Fsk { get; }

        /// <summary>
        ///     Gets an enumeration of all the genre of the <see cref="Manga" /> contains.
        /// </summary>
        public InitialisableProperty<IEnumerable<GenreType>> Genre { get; }

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
        ///     Gets whether the <see cref="Manga" /> contains H-Content (Adult).
        /// </summary>
        public InitialisableProperty<bool> IsHContent { get; }

        /// <summary>
        ///     Gets if the <see cref="Manga" /> is licensed by a german company.
        /// </summary>
        public InitialisableProperty<bool> IsLicensed { get; }

        /// <summary>
        ///     Gets the japanese title of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> JapaneseTitle { get; }

        /// <summary>
        ///     Gets the type of the <see cref="Manga" />.
        /// </summary>
        [NotNull]
        public InitialisableProperty<MangaType> MangaType { get; }

        /// <summary>
        ///     Gets the original title of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<string> Name { get; }

        /// <summary>
        ///     Gets the rating of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<AnimeMangaRating> Rating { get; }

        /// <summary>
        ///     Gets the relations of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<IEnumerable<IAnimeMangaObject>> Relations { get; }

        /// <summary>
        ///     Gets the seasons the <see cref="Manga" /> aired in. If the enumerable only contains one
        ///     value the value is always the start season of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<AnimeMangaSeasonInfo> Season { get; }

        /// <summary>
        ///     Gets the status of the <see cref="Manga" />.
        /// </summary>
        public InitialisableProperty<AnimeMangaStatus> Status { get; }

        /// <summary>
        ///     Gets the synonym the <see cref="Manga" /> is also known as.
        /// </summary>
        public InitialisableProperty<string> Synonym { get; }

        #endregion

        #region Inherited

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
        /// <returns>
        ///     An enumeration of all available <see cref="Chapter">chapters</see> in the specified
        ///     <paramref name="language">language</paramref> with a max count of <see cref="ContentCount" />.
        /// </returns>
        [NotNull]
        [ItemNotNull]
        public async Task<ProxerResult<IEnumerable<Chapter>>> GetChapters(Language language)
        {
            if (!(await this.AvailableLanguages.GetObject(new Language[0])).Contains(language))
                return new ProxerResult<IEnumerable<Chapter>>(new Exception[] {new LanguageNotAvailableException()});

            ProxerResult<AnimeMangaContentDataModel[]> lContentObjectsResult =
                await this.GetContentObjects(this._senpai);
            if (!lContentObjectsResult.Success || lContentObjectsResult.Result == null)
                return new ProxerResult<IEnumerable<Chapter>>(lContentObjectsResult.Exceptions);

            return new ProxerResult<IEnumerable<Chapter>>(from contentDataModel in lContentObjectsResult.Result
                where (Language) contentDataModel.Language == language
                select new Chapter(this, contentDataModel, this._senpai));
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

        #endregion

        /// <summary>
        ///     Represents a chapter of a <see cref="Manga" />.
        /// </summary>
        public class Chapter : IAnimeMangaContent<Manga>
        {
            private readonly Senpai _senpai;

            internal Chapter(Manga manga, AnimeMangaContentDataModel dataModel, Senpai senpai)
            {
                this._senpai = senpai;
                this.ContentIndex = dataModel.ContentIndex;
                this.Language = (Language) dataModel.Language;
                this.ParentObject = manga;
                this.Title = dataModel.Title;

                this.IsAvailable = new InitialisableProperty<bool>(this.InitInfo);
                this.Date = new InitialisableProperty<DateTime>(this.InitInfo);
                this.Pages = new InitialisableProperty<IEnumerable<Uri>>(this.InitPages);
                this.ScanlatorGroup = new InitialisableProperty<Group>(this.InitInfo);
                this.UploaderName = new InitialisableProperty<string>(this.InitInfo);
            }

            #region Properties

            /// <summary>
            ///     Gets the <see cref="Chapter" />-number.
            /// </summary>
            public int ContentIndex { get; }

            /// <summary>
            ///     Returns the release date of the <see cref="Chapter" />.
            /// </summary>
            [NotNull]
            public InitialisableProperty<DateTime> Date { get; }

            /// <summary>
            ///     Gets the general language (english/german) of the <see cref="Chapter" />.
            /// </summary>
            public Language GeneralLanguage => this.Language;

            /// <summary>
            ///     Gets if the <see cref="Chapter" /> is available.
            /// </summary>
            public InitialisableProperty<bool> IsAvailable { get; }

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
            ///     Gets the <see cref="Manga" /> this <see cref="Chapter" /> belongs to.
            /// </summary>
            public Manga ParentObject { get; }

            /// <summary>
            ///     Gets the scanlator-group of the <see cref="Chapter" />.
            /// </summary>
            [NotNull]
            public InitialisableProperty<Group> ScanlatorGroup { get; }

            /// <summary>
            ///     Gets the title of the <see cref="Chapter" />.
            /// </summary>
            [NotNull]
            public string Title { get; }

            /// <summary>
            ///     Gets the name of the uploader of the <see cref="Chapter" />.
            /// </summary>
            [NotNull]
            public InitialisableProperty<string> UploaderName { get; }

            #endregion

            #region Inherited

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
                            case "Uploader":
                                this.UploaderName.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
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
                    return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse, false).Exceptions);
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
                        (from s in
                            lDocument.DocumentNode.ChildNodes.FindFirst("script").InnerText.Split(';')[0].GetTagContents
                                ("[", "]")
                            where !s.StartsWith("[")
                            select new Uri("http://upload.proxer.me/manga/" + this.ParentObject.Id + "_" +
                                           this.Language.ToString().ToLower().Substring(0, 2) + "/" + this.ContentIndex +
                                           "/" +
                                           s.GetTagContents("\"", "\"")[0])).ToArray());

                    return new ProxerResult();
                }
                catch
                {
                    return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse, false).Exceptions);
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