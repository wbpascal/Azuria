using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Proxer.API.Exceptions;
using Proxer.API.Main.Minor;
using Proxer.API.Utilities;

namespace Proxer.API.Main
{
    /// <summary>
    /// </summary>
    public class Manga : IAnimeMangaObject
    {
        /// <summary>
        /// </summary>
        public enum Language
        {
            /// <summary>
            /// </summary>
            Deutsch,

            /// <summary>
            /// </summary>
            English
        }

        private readonly Senpai _senpai;

        internal Manga(string name, int id, Senpai senpai)
        {
            this._senpai = senpai;
            this.ObjectType = AnimeMangaType.Manga;
            this.Name = name;
            this.Id = id;
            this.CoverUri = new Uri("http://cdn.proxer.me/cover/" + this.Id + ".jpg");
        }

        #region Properties

        /// <summary>
        /// </summary>
        public string Beschreibung { get; private set; }

        /// <summary>
        /// </summary>
        public Uri CoverUri { get; private set; }

        /// <summary>
        /// </summary>
        public string EnglischTitel { get; private set; }

        /// <summary>
        /// </summary>
        public Dictionary<string, Uri> Fsk { get; private set; }

        /// <summary>
        /// </summary>
        public string[] Genre { get; private set; }

        /// <summary>
        /// </summary>
        public Group[] Gruppen { get; private set; }

        /// <summary>
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// </summary>
        public Industry[] Industrie { get; private set; }

        /// <summary>
        /// </summary>
        public string JapanTitel { get; private set; }

        /// <summary>
        /// </summary>
        public int KapitelZahl { get; private set; }

        /// <summary>
        /// </summary>
        public bool Lizensiert { get; private set; }

        /// <summary>
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// </summary>
        public AnimeMangaType ObjectType { get; private set; }

        /// <summary>
        /// </summary>
        public string[] Season { get; private set; }

        /// <summary>
        /// </summary>
        public Language[] Sprachen { get; private set; }

        /// <summary>
        /// </summary>
        public AnimeMangaStatus Status { get; private set; }

        /// <summary>
        /// </summary>
        public string Synonym { get; private set; }

        #endregion

        #region Geerbt

        /// <summary>
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        public void Init()
        {
            try
            {
                this.InitMain();
                this.InitAvailableLang();
                this.InitChapterCount();
            }
            catch (NotLoggedInException)
            {
                throw new NotLoggedInException();
            }
        }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <param name="lang"></param>
        /// <exception cref="LanguageNotAvailableException"></exception>
        /// <returns></returns>
        public Chapter[] GetChapters(Language lang)
        {
            if (!this.Sprachen.Contains(lang)) throw new LanguageNotAvailableException();

            List<Chapter> lChapters = new List<Chapter>();
            for (int i = 1; i <= this.KapitelZahl; i++)
            {
                lChapters.Add(new Chapter(i, lang, this, this._senpai));
            }

            return lChapters.ToArray();
        }

        private void InitMain()
        {
            HtmlDocument lDocument = new HtmlDocument();
            string lResponse =
                HttpUtility.GetWebRequestResponse("https://proxer.me/info/" + this.Id, this._senpai.LoginCookies)
                    .Replace("</link>", "")
                    .Replace("\n", "");

            if (!Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler)) return;
            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lTableNode =
                    lDocument.DocumentNode.ChildNodes[1].ChildNodes[2].ChildNodes[2].ChildNodes[2].ChildNodes[5]
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
                                        if (htmlNode.Name.Equals("a")) lGenreList.Add(htmlNode.InnerText);
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
                                            !this.Fsk.ContainsKey(htmlNode.GetAttributeValue("title", "ERROR")))
                                            this.Fsk.Add(htmlNode.GetAttributeValue("title", "ERROR"),
                                                new Uri("https://proxer.me" +
                                                        htmlNode.FirstChild.GetAttributeValue("src", "/")));
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
                                                htmlNode.GetAttributeValue("href", "/translatorgroups?id=-1#top"),
                                                "/translatorgroups?id=", "#top")[0]), htmlNode.InnerText)).ToArray();
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
            catch (NullReferenceException)
            {
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        private void InitAvailableLang()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();

            HtmlDocument lDocument = new HtmlDocument();
            string lResponse =
                HttpUtility.GetWebRequestResponse("http://proxer.me/edit/entry/" + this.Id + "/languages",
                    this._senpai.LoginCookies)
                    .Replace("</link>", "")
                    .Replace("\n", "");

            if (!Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler)) return;

            try
            {
                lDocument.LoadHtml(lResponse);

                List<Language> languageList = new List<Language>();
                foreach (
                    HtmlNode childNode in
                        lDocument.DocumentNode.ChildNodes[1].ChildNodes[2].ChildNodes[2].ChildNodes[2].ChildNodes[4]
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
            }
            catch (NullReferenceException)
            {
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        private void InitChapterCount()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();

            HtmlDocument lDocument = new HtmlDocument();
            string lResponse =
                HttpUtility.GetWebRequestResponse("http://proxer.me/edit/entry/" + this.Id + "/count",
                    this._senpai.LoginCookies)
                    .Replace("</link>", "")
                    .Replace("\n", "");

            if (!Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler)) return;

            try
            {
                lDocument.LoadHtml(lResponse);

                if (
                    lDocument.DocumentNode.ChildNodes[1].ChildNodes[2].ChildNodes[2].ChildNodes[2].ChildNodes[4]
                        .ChildNodes[5].FirstChild.FirstChild.InnerText.Equals("Aktuelle Anzahl:"))
                    this.KapitelZahl =
                        Convert.ToInt32(
                            lDocument.DocumentNode.ChildNodes[1].ChildNodes[2].ChildNodes[2].ChildNodes[2].ChildNodes[4]
                                .ChildNodes[5].FirstChild.ChildNodes[1].InnerText);
            }
            catch (NullReferenceException)
            {
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        public class Chapter
        {
            private readonly Senpai _senpai;

            internal Chapter(int kapitelNr, Language lang, Manga parentManga, Senpai senpai)
            {
                this._senpai = senpai;
                this.KapitelNr = kapitelNr;
                this.Sprache = lang;
                this.ParentManga = parentManga;
            }

            #region Properties

            /// <summary>
            /// </summary>
            public DateTime Datum { get; private set; }

            /// <summary>
            /// </summary>
            public int KapitelNr { get; private set; }

            /// <summary>
            /// </summary>
            public Manga ParentManga { get; set; }

            /// <summary>
            /// </summary>
            public Group ScanlatorGruppe { get; private set; }

            /// <summary>
            /// </summary>
            public Uri[] Seiten { get; private set; }

            /// <summary>
            /// </summary>
            public Language Sprache { get; private set; }

            /// <summary>
            /// </summary>
            public string Titel { get; private set; }

            /// <summary>
            /// </summary>
            public string UploaderName { get; private set; }

            /// <summary>
            /// </summary>
            public bool Verfuegbar { get; private set; }

            #endregion

            #region

            /// <summary>
            /// </summary>
            /// <exception cref="NotLoggedInException"></exception>
            public void Init()
            {
                try
                {
                    this.InitInfo();
                    this.InitChapters();
                }
                catch (NotLoggedInException)
                {
                    throw new NotLoggedInException();
                }
            }

            private void InitInfo()
            {
                HtmlDocument lDocument = new HtmlDocument();
                string lResponse =
                    HttpUtility.GetWebRequestResponse(
                        "https://proxer.me/chapter/" + this.ParentManga.Id + "/" + this.KapitelNr + "/" +
                        this.Sprache.ToString().ToLower().Substring(0, 2),
                        this._senpai.LoginCookies)
                        .Replace("</link>", "")
                        .Replace("\n", "");

                if (!Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler)) return;

                if (lResponse.Contains("Dieses Kapitel ist leider noch nicht verfügbar :/"))
                {
                    this.Verfuegbar = false;
                    return;
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
                        throw new CaptchaException();

                    if (
                        lAllHtmlNodes.Any(
                            x =>
                                x.Name.Equals("img") && x.HasAttributes &&
                                x.GetAttributeValue("src", "").Equals("/images/misc/404.png")))
                        return;

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
                }
                catch (NullReferenceException)
                {
                }
                catch (IndexOutOfRangeException)
                {
                }
            }

            private void InitChapters()
            {
                if(!this._senpai.LoggedIn) throw new NotLoggedInException();

                HtmlDocument lDocument = new HtmlDocument();
                string lResponse =
                    HttpUtility.GetWebRequestResponse(
                        "https://proxer.me/read/" + this.ParentManga.Id + "/" + this.KapitelNr + "/" +
                        this.Sprache.ToString().ToLower().Substring(0, 2) + "?format=json",
                        this._senpai.MobileLoginCookies)
                        .Replace("</link>", "")
                        .Replace("\n", "");

                if (!Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler)) return;

                try
                {
                    lDocument.LoadHtml(lResponse);

                    HtmlNode[] lAllHtmlNodes = Utility.GetAllHtmlNodes(lDocument.DocumentNode.ChildNodes).ToArray();

                    if (
                        lAllHtmlNodes.Any(
                            x =>
                                x.Name.Equals("img") && x.HasAttributes &&
                                x.GetAttributeValue("src", "").Equals("/images/misc/stopyui.jpg")))
                        throw new CaptchaException();

                    if (
                        lAllHtmlNodes.Any(
                            x =>
                                x.Name.Equals("img") && x.HasAttributes &&
                                x.GetAttributeValue("src", "").Equals("/images/misc/404.png")))
                        return;

                    this.Seiten =
                        (from s in
                            Utility.GetTagContents(lDocument.DocumentNode.ChildNodes[1].InnerText.Split(';')[0], "[",
                                "]")
                            where !s.StartsWith("[")
                            select
                                new Uri("http://upload.proxer.me/manga/" + this.ParentManga.Id + "_" +
                                        this.Sprache.ToString().ToLower().Substring(0, 2) + "/" + this.KapitelNr + "/" +
                                        Utility.GetTagContents(s, "\"", "\"")[0])).ToArray();
                }
                catch (NullReferenceException)
                {
                }
                catch (IndexOutOfRangeException)
                {
                }
            }

            #endregion
        }
    }
}