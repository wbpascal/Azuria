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
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        public static TranslatorDataModel BuildDataModel()
        {
            return new TranslatorDataModel
            {
                Count = 4855,
                Country = Country.Germany,
                CProjects = 11,
                Description = "TranslatorGroup Description Test Text",
                Id = 48,
                Image = new Uri("http://www.melon-subs.de/wp-content/uploads/2014/05/cropped-ML-Logo-1080p-v12.png"),
                Link = new Uri("http://melon-subs.de/index.php"),
                Name = "Melon-Subs"
            };
        }
    }
}