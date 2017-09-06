using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class RelationDataModelTest : DataModelsTestBase<RelationDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getrelations.json"];
            ProxerApiResponse<RelationDataModel[]> lResponse = this.ConvertArray(lJson);
            this.CheckSuccessResponse(lResponse);

            Assert.AreEqual(1, lResponse.Result.Length);
            EntryDataModelTest.CheckDataModel(lResponse.Result[0]);
            Assert.AreEqual(new[] {MediaLanguage.EngSub}, lResponse.Result[0].AvailableLanguages);
            Assert.AreEqual(Season.Spring, lResponse.Result[0].StartSeason.Season);
            Assert.AreEqual(2011, lResponse.Result[0].StartSeason.Year);
        }
    }
}