using System.Linq;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class TranslatorBasicDataModelTest : DataModelsTestBase<TranslatorBasicDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getgroups.json"];
            ProxerApiResponse<TranslatorBasicDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        public static TranslatorBasicDataModel BuildDataModel()
        {
            return new TranslatorBasicDataModel
            {
                Country = Country.England,
                Id = 455,
                Name = "English Studio"
            };
        }
    }
}