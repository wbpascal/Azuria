using System.Linq;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class NameDataModelTest : DataModelsTestBase<NameDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getnames.json"];
            ProxerApiResponse<NameDataModel[]> lResponse = this.ConvertArray(lJson);
            this.CheckSuccessResponse(lResponse);
            CheckDataModels(lResponse.Result);
        }

        public static void CheckDataModels(NameDataModel[] dataModels)
        {
            Assert.AreEqual(4, dataModels.Length);

            Assert.True(dataModels.All(model => model.EntryId == 9200));

            Assert.AreEqual("Shigatsu wa Kimi no Uso", dataModels[0].Name);
            Assert.AreEqual("Your Lie in April", dataModels[1].Name);
            Assert.AreEqual("Shigatsu wa Kimi no Uso: Sekunden in Moll", dataModels[2].Name);
            Assert.AreEqual("四月は君の嘘", dataModels[3].Name);

            Assert.AreEqual(19232, dataModels[0].Id);
            Assert.AreEqual(22019, dataModels[1].Id);
            Assert.AreEqual(40161, dataModels[2].Id);
            Assert.AreEqual(19233, dataModels[3].Id);

            Assert.AreEqual(MediaNameType.Original, dataModels[0].Type);
            Assert.AreEqual(MediaNameType.English, dataModels[1].Type);
            Assert.AreEqual(MediaNameType.German, dataModels[2].Type);
            Assert.AreEqual(MediaNameType.Japanese, dataModels[3].Type);
        }
    }
}