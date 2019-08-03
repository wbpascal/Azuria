using System.Linq;
using Azuria.Api.v1.DataModels.User;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.User
{
    public class HistoryDataModelTest : DataModelsTestBase<HistoryDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["user_gethistory.json"];
            ProxerApiResponse<HistoryDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(3, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static HistoryDataModel BuildDataModel()
        {
            return new HistoryDataModel
            {
                ContentIndex = 213,
                EntryId = 7834,
                EntryMedium = MediaMedium.Mangaseries,
                EntryName = "DICE",
                EntryType = MediaEntryType.Manga,
                Id = 411816788,
                Language = MediaLanguage.English
            };
        }
    }
}