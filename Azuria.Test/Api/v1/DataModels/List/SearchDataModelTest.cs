using System.Linq;
using Azuria.Api.v1.DataModels.List;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Helpers.Extensions;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.List
{
    public class SearchDataModelTest : DataModelsTestBase<SearchDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["list_getentrysearch.json"];
            ProxerApiResponse<SearchDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(3, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        public static SearchDataModel BuildDataModel()
        {
            return new SearchDataModel
            {
                AvailableLanguages = new[] {MediaLanguage.GerSub, MediaLanguage.EngSub, MediaLanguage.EngDub},
                ContentCount = 24,
                EntryId = 9651,
                EntryMedium = MediaMedium.Animeseries,
                EntryName = "Akatsuki no Yona",
                GenreRaw = string.Join(" ", Genre.Adventure.GetDescription(), Genre.Action.GetDescription()),
                RateCount = 2779,
                RateSum = 23457,
                Status = MediaStatus.Completed
            };
        }
    }
}