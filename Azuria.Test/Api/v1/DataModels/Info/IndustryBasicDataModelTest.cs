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
            this.CheckSuccessResponse(lResponse);
            CheckDataModels(lResponse.Result);
        }

        public static void CheckDataModels(IndustryBasicDataModel[] dataModels)
        {
            void CheckDataModel(
                IndustryBasicDataModel dataModel, string name, int id, IndustryType type, Country country)
            {
                Assert.Multiple(
                    () =>
                    {
                        Assert.AreEqual(name, dataModel.Name);
                        Assert.AreEqual(id, dataModel.Id);
                        Assert.AreEqual(type, dataModel.Type);
                        Assert.AreEqual(country, dataModel.Country);
                    });
            }

            Assert.AreEqual(5, dataModels.Length);

            CheckDataModel(dataModels[0], "Viewster", 216, IndustryType.Streaming, Country.Germany);
            CheckDataModel(dataModels[1], "Peppermint Anime", 9, IndustryType.Publisher, Country.Germany);
            CheckDataModel(
                dataModels[2], "Kyoraku Industrial Holdings Co.,Ltd.", 430, IndustryType.Producer, Country.Japan
            );
            CheckDataModel(dataModels[3], "Aniplex of America", 431, IndustryType.Publisher, Country.UnitedStates);
            CheckDataModel(dataModels[4], "A-1 Pictures", 7, IndustryType.Studio, Country.Japan);
        }
    }
}