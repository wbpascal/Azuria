using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    public class SeasonDataModelTest : DataModelsTestBase<SeasonDataModel>
    {
        [Fact]
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
                Assert.Equal(id, dataModel.Id);
                Assert.Equal(year, dataModel.Year);
                Assert.Equal(season, dataModel.Season);
                if (!ignoreEntryId) Assert.Equal(entryId, dataModel.EntryId);
            }

            Assert.Equal(2, dataModels.Length);
            CheckDataModel(dataModels[0], 6283, 2014, Season.Autumn, 9200);
            CheckDataModel(dataModels[1], 8526, 2015, Season.Winter, 9200);
        }
    }
}