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
    public class Anime : IAnimeMangaObject
    {
        /// <summary>
        /// </summary>
        public enum Language
        {
            /// <summary>
            /// </summary>
            GerSub,

            /// <summary>
            /// </summary>
            EngSub,

            /// <summary>
            /// </summary>
            EngDub,

            /// <summary>
            /// </summary>
            GerDub
        }

        private readonly Senpai _senpai;

        internal Anime(string name, int id, Senpai senpai)
        {
            this._senpai = senpai;
            this.ObjectType = AnimeMangaType.Anime;
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
        public int EpisodenZahl { get; private set; }

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
        public void Init()
        {
            this.InitMain();
            this.InitEpisodeCount();
            this.InitAvailableLang();
        }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        /// <exception cref="InitializeNeededException">Wenn der Anime noch nicht initialisiert ist.</exception>
        public Episode[] GetEpisodes(Language language)
        {
            if (this.Sprachen == null) throw new InitializeNeededException();

            if (!this.Sprachen.Contains(language)) return null;

            List<Episode> lEpisodes = new List<Episode>();
            for (int i = 1; i <= this.EpisodenZahl; i++)
            {
                lEpisodes.Add(new Episode(this, i, language, this._senpai));
            }

            return lEpisodes.ToArray();
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
            if (!this._senpai.LoggedIn) return;

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
            }
            catch (NullReferenceException)
            {
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        private void InitEpisodeCount()
        {
            if (!this._senpai.LoggedIn) return;

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
                    this.EpisodenZahl =
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
        public class Episode
        {
            private readonly Language _lang;
            private readonly Senpai _senpai;

            internal Episode(Anime anime, int nr, Language lang, Senpai senpai)
            {
                this.ParentAnime = anime;
                this.EpisodeNr = nr;
                this._lang = lang;
                this._senpai = senpai;
            }

            #region Properties

            /// <summary>
            /// 
            /// </summary>
            public Anime ParentAnime { get; set; }

            /// <summary>
            /// </summary>
            public int EpisodeNr { get; set; }

            /// <summary>
            ///     Wenn nach Init() immer noch null, dann sind keine Streams für diese Episode verfügbar.
            /// </summary>
            public Dictionary<Stream.StreamPartner, Stream> Streams { get; private set; }

            #endregion

            #region

            /// <summary>
            /// </summary>
            /// <exception cref="NotLoggedInException"></exception>
            /// <exception cref="CaptchaException"></exception>
            public void Init()
            {
                if (!this._senpai.LoggedIn) throw new NotLoggedInException();

                HtmlDocument lDocument = new HtmlDocument();
                string lResponse =
                    HttpUtility.GetWebRequestResponse(
                        "https://proxer.me/watch/" + this.ParentAnime.Id + "/" + this.EpisodeNr + "/" +
                        this._lang.ToString().ToLower(),
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

                    this.Streams = new Dictionary<Stream.StreamPartner, Stream>();

                    foreach (
                        HtmlNode childNode in
                            lDocument.DocumentNode.ChildNodes[1].ChildNodes[2].FirstChild.ChildNodes[1].ChildNodes[1]
                                .ChildNodes.Where(childNode => !childNode.FirstChild.Name.Equals("#text")))
                    {
                        switch (childNode.InnerText)
                        {
                            case "Dailymotion":
                                this.Streams.Add(Stream.StreamPartner.Dailymotion,
                                    new Stream(
                                        new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                        Stream.StreamPartner.Dailymotion));
                                break;
                            case "MP4Upload":
                                this.Streams.Add(Stream.StreamPartner.Mp4Upload,
                                    new Stream(
                                        new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                        Stream.StreamPartner.Mp4Upload));
                                break;
                            case "Streamcloud":
                                this.Streams.Add(Stream.StreamPartner.Streamcloud,
                                    new Stream(
                                        new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                        Stream.StreamPartner.Streamcloud));
                                break;
                            case "Videobam":
                                this.Streams.Add(Stream.StreamPartner.Videobam,
                                    new Stream(
                                        new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                        Stream.StreamPartner.Videobam));
                                break;
                            case "YourUpload":
                                this.Streams.Add(Stream.StreamPartner.YourUpload,
                                    new Stream(
                                        new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                        Stream.StreamPartner.YourUpload));
                                break;
                            case "Proxer-Stream":
                                this.Streams.Add(Stream.StreamPartner.ProxerStream,
                                    new Stream(
                                        new Uri(childNode.FirstChild.GetAttributeValue("href", "http://proxer.me/")),
                                        Stream.StreamPartner.ProxerStream));
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

            #endregion

            /// <summary>
            /// </summary>
            public class Stream
            {
                /// <summary>
                /// </summary>
                public enum StreamPartner
                {
                    /// <summary>
                    /// </summary>
                    Streamcloud,

                    /// <summary>
                    /// </summary>
                    Mp4Upload,

                    /// <summary>
                    /// </summary>
                    Dailymotion,

                    /// <summary>
                    /// </summary>
                    Videobam,

                    /// <summary>
                    /// </summary>
                    YourUpload,

                    /// <summary>
                    /// </summary>
                    ProxerStream
                }

                private readonly StreamPartner _streamPartner;

                internal Stream(Uri link, StreamPartner streamPartner)
                {
                    this.Link = link;
                    this._streamPartner = streamPartner;
                }

                #region Properties

                /// <summary>
                /// </summary>
                public Uri Link { get; private set; }

                #endregion
            }
        }
    }
}