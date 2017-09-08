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
            this.CheckSuccessResponse(lResponse);
            CheckDataModels(lResponse.Result);
        }

        public static void CheckDataModels(TranslatorBasicDataModel[] dataModels)
        {
            void CheckDataModel(TranslatorBasicDataModel dataModel, string name, int id, Country country)
            {
                Assert.Multiple(
                    () =>
                    {
                        Assert.AreEqual(country, dataModel.Country);
                        Assert.AreEqual(id, dataModel.Id);
                        Assert.AreEqual(name, dataModel.Name);
                    });
            }

            Assert.AreEqual(2, dataModels.Length);
            CheckDataModel(dataModels[0], "English Studio", 455, Country.England);
            CheckDataModel(dataModels[1], "Gruppe Kampfkuchen", 11, Country.Germany);
        }
    }
}