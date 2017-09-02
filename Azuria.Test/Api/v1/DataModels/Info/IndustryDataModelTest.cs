using System;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    public class IndustryDataModelTest : DataModelsTestBase<IndustryDataModel>
    {
        [Fact]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getindustry.json"];
            ProxerApiResponse<IndustryDataModel> lResponse = this.Convert(lJson);
            this.CheckSuccessResponse(lResponse);
            IndustryDataModel lDataModel = lResponse.Result;

            Assert.Equal(1292, lDataModel.Id);
            Assert.Equal(IndustryType.Streaming, lDataModel.Type);
            Assert.Equal("Animax UK", lDataModel.Name);
            Assert.Equal(Country.UnitedStates, lDataModel.Country);
            Assert.Equal(new Uri("https://www.animaxtv.co.uk/"), lDataModel.Link);
            Assert.Equal("Animax UK Description Test Text", lDataModel.Description);
            Assert.Equal(new Uri("https://cdn.proxer.me/industry/1292.jpg"), lDataModel.CoverImage);
        }
    }
}