using Azuria.Api.v1.DataModels.Media;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Media
{
    public class HeaderDataModelTest : DataModelsTestBase<HeaderDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["media_getrandomheaderblack.json"];
            ProxerApiResponse<HeaderDataModel> lResponse = this.Convert(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        private static HeaderDataModel BuildDataModel()
        {
            return new HeaderDataModel
            {
                HeaderFileName = "59243",
                HeaderId = 59243,
                HeaderPath = "other_stuff__memes_17/63"
            };
        }
    }
}