using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class SeasonDataModelTest : DataModelsTestBase<SeasonDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getseason.json"];
            ProxerApiResponse<SeasonDataModel[]> lResponse = this.ConvertArray(lJson);
            this.CheckSuccessResponse(lResponse);
            CheckDataModels(lResponse.Result);
        }

        public static void CheckDataModels(SeasonDataModel[] dataModels, bool ignoreEntryId = false)
        {
            void CheckDataModel(SeasonDataModel dataModel, int id, int year, Season season, int entryId)
            {
                Assert.AreEqual(id, dataModel.Id);
                Assert.AreEqual(year, dataModel.Year);
                Assert.AreEqual(season, dataModel.Season);
                if (!ignoreEntryId) Assert.AreEqual(entryId, dataModel.EntryId);
            }

            Assert.AreEqual(2, dataModels.Length);
            CheckDataModel(dataModels[0], 6283, 2014, Season.Autumn, 9200);
            CheckDataModel(dataModels[1], 8526, 2015, Season.Winter, 9200);
        }
    }
}