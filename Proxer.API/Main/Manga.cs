using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Proxer.API.Main.Minor;
using Proxer.API.Utilities;

namespace Proxer.API.Main
{
    /// <summary>
    /// </summary>
    public class Manga : IAnimeMangaObject
    {
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
        }

        #endregion

        #region

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

                if (lDocument.ParseErrors.Any())
                    lDocument.LoadHtml(Utility.TryFixParseErrors(lDocument.DocumentNode.InnerHtml, lDocument.ParseErrors));

                if (lDocument.ParseErrors.Any()) return;
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
                            break;
                    }
                }
            }
            catch (NullReferenceException)
            {
                this._senpai.ErrHandler.Add(lResponse);
            }
        }

        #endregion
    }
}