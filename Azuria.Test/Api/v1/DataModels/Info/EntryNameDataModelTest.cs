using System.Linq;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class EntryNameDataModelTest : DataModelsTestBase<EntryNameDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getnames.json"];
            ProxerApiResponse<EntryNameDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        public static EntryNameDataModel BuildDataModel()
        {
            return new EntryNameDataModel
            {
                EntryId = 9200,
                Id = 19232,
                Name = "Shigatsu wa Kimi no Uso",
                Type = MediaNameType.Original
            };
        }
    }
}