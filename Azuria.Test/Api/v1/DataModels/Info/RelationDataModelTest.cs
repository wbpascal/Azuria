using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    public class RelationDataModelTest : DataModelsTestBase<RelationDataModel>
    {
        [Fact]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getrelations.json"];
            ProxerApiResponse<RelationDataModel[]> lResponse = this.ConvertArray(lJson);
            this.CheckSuccessResponse(lResponse);

            Assert.Equal(1, lResponse.Result.Length);
            EntryDataModelTest.CheckDataModel(lResponse.Result[0]);
            Assert.Equal(new[] {MediaLanguage.EngSub}, lResponse.Result[0].AvailableLanguages);
            Assert.Equal(Season.Spring, lResponse.Result[0].StartSeason.Season);
            Assert.Equal(2011, lResponse.Result[0].StartSeason.Year);
        }
    }
}