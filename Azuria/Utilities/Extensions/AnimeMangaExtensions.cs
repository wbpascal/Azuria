using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Web;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Azuria.Utilities.Extensions
{
    internal static class AnimeMangaExtensions
    {
        #region

        [ItemNotNull]
        internal static async Task<ProxerResult> InitMainInfo(this IAnimeMangaObject animeMangaObject, Senpai senpai)
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/info/" + animeMangaObject.Id + "?format=raw"),
                        senpai, new Func<string, ProxerResult>[0], checkLogin: false);

            if (!lResult.Success || lResult.Result == null)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                HtmlNode lTableNode =
                    lDocument.DocumentNode.ChildNodes[5]
                        .ChildNodes[2].FirstChild.ChildNodes[1].FirstChild;

                #region Info Table

                foreach (HtmlNode childNode in lTableNode.ChildNodes.Where(childNode => childNode.Name.Equals("tr")))
                {
                    switch (childNode.FirstChild.FirstChild.InnerText)
                    {
                        case "Original Titel":
                            animeMangaObject.Name.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                            break;
                        case "Eng. Titel":
                            animeMangaObject.EnglishTitle.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                            break;
                        case "Ger. Titel":
                            animeMangaObject.GermanTitle.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                            break;
                        case "Jap. Titel":
                            animeMangaObject.JapaneseTitle.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                            break;
                        case "Synonym":
                            animeMangaObject.Synonym.SetInitialisedObject(childNode.ChildNodes[1].InnerText);
                            break;
                        case "Genre":
                            List<GenreObject> lGenreList = new List<GenreObject>();
                            foreach (HtmlNode htmlNode in childNode.ChildNodes[1].ChildNodes.ToList())
                            {
                                if (htmlNode.Name.Equals("a"))
                                    lGenreList.Add(new GenreObject(htmlNode.InnerText));
                            }
                            animeMangaObject.Genre.SetInitialisedObject(lGenreList.ToArray());
                            break;
                        case "PSK":
                            List<FskObject> lTempList = new List<FskObject>();
                            foreach (
                                HtmlNode htmlNode in
                                    childNode.ChildNodes[1].ChildNodes.ToList()
                                        .Where(htmlNode => htmlNode.Name.Equals("span")))
                            {
                                string lFskString = htmlNode.FirstChild.GetAttributeValue("src",
                                    "/images/fsk/unknown.png")
                                    .GetTagContents("fsk/", ".png")
                                    .First();
                                lTempList.AddIf(new FskObject(FskHelper.StringToFskDictionary[lFskString],
                                    new Uri($"https://proxer.me/images/fsk/{lFskString}.png")),
                                    fsk => lTempList.All(o => o.FskType != fsk.FskType));
                            }
                            animeMangaObject.Fsk.SetInitialisedObject(lTempList);
                            break;
                        case "Season":
                            List<string> lSeasonList = new List<string>();
                            foreach (HtmlNode htmlNode in childNode.ChildNodes[1].ChildNodes.ToList())
                            {
                                if (htmlNode.Name.Equals("a"))
                                    lSeasonList.Add(htmlNode.InnerText);
                            }
                            animeMangaObject.Season.SetInitialisedObject(lSeasonList);
                            break;
                        case "Status":
                            switch (childNode.ChildNodes[1].InnerText)
                            {
                                case "Airing":
                                    animeMangaObject.Status.SetInitialisedObject(AnimeMangaStatus.Airing);
                                    break;
                                case "Abgeschlossen":
                                    animeMangaObject.Status.SetInitialisedObject(AnimeMangaStatus.Completed);
                                    break;
                                case "Nicht erschienen (Pre-Airing)":
                                    animeMangaObject.Status.SetInitialisedObject(AnimeMangaStatus.PreAiring);
                                    break;
                                default:
                                    animeMangaObject.Status.SetInitialisedObject(AnimeMangaStatus.Cancelled);
                                    break;
                            }
                            break;
                        case "Gruppen":
                            if (childNode.ChildNodes[1].InnerText.Contains("Keine Gruppen eingetragen.")) break;
                            animeMangaObject.Groups.SetInitialisedObject(
                                from htmlNode in childNode.ChildNodes[1].ChildNodes
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
                                    lIndustryType = Industry.IndustryType.Studio;
                                else if (htmlNode.NextSibling.InnerText.Contains("Publisher"))
                                    lIndustryType = Industry.IndustryType.Publisher;
                                else if (htmlNode.NextSibling.InnerText.Contains("Producer"))
                                    lIndustryType = Industry.IndustryType.Producer;
                                else lIndustryType = Industry.IndustryType.Unknown;

                                lIndustries.Add(new Industry(Convert.ToInt32(
                                    htmlNode.GetAttributeValue("href", "/industry?id=-1#top")
                                        .GetTagContents("/industry?id=", "#top")[0]), htmlNode.InnerText, lIndustryType));
                            }
                            animeMangaObject.Industry.SetInitialisedObject(lIndustries.ToArray());
                            break;
                        case "Lizenz":
                            animeMangaObject.IsLicensed.SetInitialisedObject(
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
                            animeMangaObject.Description.SetInitialisedObject(lTempString);
                            break;
                    }
                }

                #endregion

                double lRating =
                    Convert.ToDouble(
                        lDocument.DocumentNode.SelectNodesUtility("class", "average")
                            .First()
                            .InnerText.Replace('.', ','));
                int lVoters =
                    Convert.ToInt32(lDocument.DocumentNode.SelectNodesUtility("class", "votes").First().InnerText);
                animeMangaObject.Rating.SetInitialisedObject(new AnimeMangaRating(lRating, lVoters));
            }
            catch
            {
                return new ProxerResult(ErrorHandler.HandleError(senpai, lResponse).Exceptions);
            }

            return new ProxerResult();
        }

        #endregion
    }
}