using System.Linq;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    public class NameDataModelTest : DataModelsTestBase<NameDataModel>
    {
        [Fact]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getnames.json"];
            ProxerApiResponse<NameDataModel[]> lResponse = this.ConvertArray(lJson);
            this.CheckSuccessResponse(lResponse);
            CheckDataModels(lResponse.Result);
        }

        public static void CheckDataModels(NameDataModel[] dataModels)
        {
            Assert.Equal(4, dataModels.Length);
            
            Assert.True(dataModels.All(model => model.EntryId == 9200));
            
            Assert.Equal("Shigatsu wa Kimi no Uso", dataModels[0].Name);
            Assert.Equal("Your Lie in April", dataModels[1].Name);
            Assert.Equal("Shigatsu wa Kimi no Uso: Sekunden in Moll", dataModels[2].Name);
            Assert.Equal("四月は君の嘘", dataModels[3].Name);
            
            Assert.Equal(19232, dataModels[0].Id);
            Assert.Equal(22019, dataModels[1].Id);
            Assert.Equal(40161, dataModels[2].Id);
            Assert.Equal(19233, dataModels[3].Id);
            
            Assert.Equal(MediaNameType.Original, dataModels[0].Type);
            Assert.Equal(MediaNameType.English, dataModels[1].Type);
            Assert.Equal(MediaNameType.German, dataModels[2].Type);
            Assert.Equal(MediaNameType.Japanese, dataModels[3].Type);
        }
    }
}