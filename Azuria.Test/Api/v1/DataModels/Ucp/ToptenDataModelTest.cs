using System.Linq;
using Azuria.Api.v1.DataModels.Ucp;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Ucp
{
    public class ToptenDataModelTest : DataModelsTestBase<ToptenDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["ucp_gettopten.json"];
            ProxerApiResponse<ToptenDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(2, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static ToptenDataModel BuildDataModel()
        {
            return new ToptenDataModel
            {
                EntryId = 4385,
                EntryMedium = MediaMedium.Mangaseries,
                EntryName = "Ao Haru Ride",
                EntryType = MediaEntryType.Manga,
                ToptenId = 1076532
            };
        }
    }
}