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
            Assert.AreEqual(5, dataModels.Length);

            Assert.AreEqual("Viewster", dataModels[0].Name);
            Assert.AreEqual("Peppermint Anime", dataModels[1].Name);
            Assert.AreEqual("Kyoraku Industrial Holdings Co.,Ltd.", dataModels[2].Name);
            Assert.AreEqual("Aniplex of America", dataModels[3].Name);
            Assert.AreEqual("A-1 Pictures", dataModels[4].Name);

            Assert.AreEqual(216, dataModels[0].Id);
            Assert.AreEqual(9, dataModels[1].Id);
            Assert.AreEqual(430, dataModels[2].Id);
            Assert.AreEqual(431, dataModels[3].Id);
            Assert.AreEqual(7, dataModels[4].Id);

            Assert.AreEqual(IndustryType.Streaming, dataModels[0].Type);
            Assert.AreEqual(IndustryType.Publisher, dataModels[1].Type);
            Assert.AreEqual(IndustryType.Producer, dataModels[2].Type);
            Assert.AreEqual(IndustryType.Publisher, dataModels[3].Type);
            Assert.AreEqual(IndustryType.Studio, dataModels[4].Type);

            Assert.AreEqual(Country.Germany, dataModels[0].Country);
            Assert.AreEqual(Country.Germany, dataModels[1].Country);
            Assert.AreEqual(Country.Japan, dataModels[2].Country);
            Assert.AreEqual(Country.UnitedStates, dataModels[3].Country);
            Assert.AreEqual(Country.Japan, dataModels[4].Country);
        }
    }
}