using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    public class IndustryBasicDataModelTest : DataModelsTestBase<IndustryBasicDataModel>
    {
        [Fact]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getpublisher.json"];
            ProxerApiResponse<IndustryBasicDataModel[]> lResponse = this.ConvertArray(lJson);
            this.CheckSuccessResponse(lResponse);
            CheckDataModels(lResponse.Result);
        }

        public static void CheckDataModels(IndustryBasicDataModel[] dataModels)
        {
            Assert.Equal(5, dataModels.Length);

            Assert.Equal("Viewster", dataModels[0].Name);
            Assert.Equal("Peppermint Anime", dataModels[1].Name);
            Assert.Equal("Kyoraku Industrial Holdings Co.,Ltd.", dataModels[2].Name);
            Assert.Equal("Aniplex of America", dataModels[3].Name);
            Assert.Equal("A-1 Pictures", dataModels[4].Name);

            Assert.Equal(216, dataModels[0].Id);
            Assert.Equal(9, dataModels[1].Id);
            Assert.Equal(430, dataModels[2].Id);
            Assert.Equal(431, dataModels[3].Id);
            Assert.Equal(7, dataModels[4].Id);

            Assert.Equal(IndustryType.Streaming, dataModels[0].Type);
            Assert.Equal(IndustryType.Publisher, dataModels[1].Type);
            Assert.Equal(IndustryType.Producer, dataModels[2].Type);
            Assert.Equal(IndustryType.Publisher, dataModels[3].Type);
            Assert.Equal(IndustryType.Studio, dataModels[4].Type);

            Assert.Equal(Country.Germany, dataModels[0].Country);
            Assert.Equal(Country.Germany, dataModels[1].Country);
            Assert.Equal(Country.Japan, dataModels[2].Country);
            Assert.Equal(Country.UnitedStates, dataModels[3].Country);
            Assert.Equal(Country.Japan, dataModels[4].Country);
        }
    }
}