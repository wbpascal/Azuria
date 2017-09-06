using System;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class TranslatorDataModelTest : DataModelsTestBase<TranslatorDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_gettranslatorgroup.json"];
            ProxerApiResponse<TranslatorDataModel> lResponse = this.Convert(lJson);
            this.CheckSuccessResponse(lResponse);

            TranslatorDataModel lDataModel = lResponse.Result;
            Assert.AreEqual(4855, lDataModel.Count);
            Assert.AreEqual(Country.Germany, lDataModel.Country);
            Assert.AreEqual(11, lDataModel.CProjects);
            Assert.AreEqual("TranslatorGroup Description Test Text", lDataModel.Description);
            Assert.AreEqual(48, lDataModel.Id);
            Assert.AreEqual(
                new Uri("http://www.melon-subs.de/wp-content/uploads/2014/05/cropped-ML-Logo-1080p-v12.png"),
                lDataModel.Image
            );
            Assert.AreEqual(new Uri("http://melon-subs.de/index.php"), lDataModel.Link);
            Assert.AreEqual("Melon-Subs", lDataModel.Name);
        }
    }
}