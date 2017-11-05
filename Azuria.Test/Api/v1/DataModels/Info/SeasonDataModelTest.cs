using System.Linq;
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
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        public static SeasonDataModel BuildDataModel()
        {
            return new SeasonDataModel
            {
                EntryId = 9200,
                Id = 6283,
                Season = Season.Autumn,
                Year = 2014
            };
        }
    }
}