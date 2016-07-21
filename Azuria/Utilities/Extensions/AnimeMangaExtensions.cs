using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Exceptions;
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
                        case "Season":
                            List<string> lSeasonList = new List<string>();
                            foreach (HtmlNode htmlNode in childNode.ChildNodes[1].ChildNodes.ToList())
                            {
                                if (htmlNode.Name.Equals("a"))
                                    lSeasonList.Add(htmlNode.InnerText);
                            }
                            animeMangaObject.Season.SetInitialisedObject(lSeasonList);
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

        internal static async Task<ProxerResult> InitMainInfoApi(this IAnimeMangaObject animeMangaObject, Senpai senpai)
        {
            ProxerResult<ProxerApiResponse<GetEntryDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.BuildForGetEntry(animeMangaObject.Id, senpai));
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            if (lResult.Result.Error || lResult.Result.Data == null)
                return new ProxerResult(new[] {new ProxerApiException(lResult.Result.ErrorCode)});
            GetEntryDataModel lDataModel = lResult.Result.Data;

            animeMangaObject.Name.SetInitialisedObject(lDataModel.Name);
            animeMangaObject.Description.SetInitialisedObject(lDataModel.Description);
            animeMangaObject.ContentCount.SetInitialisedObject(lDataModel.ContentCount);
            animeMangaObject.Status.SetInitialisedObject(lDataModel.State);
            animeMangaObject.Rating.SetInitialisedObject(lDataModel.Rating);
            animeMangaObject.Fsk.SetInitialisedObject(lDataModel.Fsk);
            animeMangaObject.Genre.SetInitialisedObject(lDataModel.Genre);
            (animeMangaObject as Anime)?.AnimeTyp.SetInitialisedObject((AnimeType) lDataModel.Medium);
            (animeMangaObject as Manga)?.MangaTyp.SetInitialisedObject((MangaType) lDataModel.Medium);

            return new ProxerResult();
        }

        #endregion
    }
}