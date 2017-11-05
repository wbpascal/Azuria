using System.Linq;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class IndustryBasicDataModelTest : DataModelsTestBase<IndustryBasicDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getpublisher.json"];
            ProxerApiResponse<IndustryBasicDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        public static IndustryBasicDataModel BuildDataModel()
        {
            return new IndustryBasicDataModel
            {
                Country = Country.Germany,
                Id = 216,
                Name = "Viewster",
                Type = IndustryType.Streaming
            };
        }
    }
}