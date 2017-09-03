using System;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    public class TranslatorDataModelTest : DataModelsTestBase<TranslatorDataModel>
    {
        [Fact]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_gettranslatorgroup.json"];
            ProxerApiResponse<TranslatorDataModel> lResponse = this.Convert(lJson);
            this.CheckSuccessResponse(lResponse);

            TranslatorDataModel lDataModel = lResponse.Result;
            Assert.Equal(4855, lDataModel.Count);
            Assert.Equal(Country.Germany, lDataModel.Country);
            Assert.Equal(11, lDataModel.CProjects);
            Assert.Equal("TranslatorGroup Description Test Text", lDataModel.Description);
            Assert.Equal(48, lDataModel.Id);
            Assert.Equal(
                new Uri("http://www.melon-subs.de/wp-content/uploads/2014/05/cropped-ML-Logo-1080p-v12.png"),
                lDataModel.Image
            );
            Assert.Equal(new Uri("http://melon-subs.de/index.php"), lDataModel.Link);
            Assert.Equal("Melon-Subs", lDataModel.Name);
        }
    }
}