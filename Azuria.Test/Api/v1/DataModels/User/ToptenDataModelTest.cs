using System.Linq;
using Azuria.Api.v1.DataModels.User;
using Azuria.Enums;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.User
{
    public class ToptenDataModelTest : DataModelsTestBase<ToptenDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["user_gettoptenanime.json"];
            ProxerApiResponse<ToptenDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(2, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static ToptenDataModel BuildDataModel()
        {
            return new ToptenDataModel
            {
                EntryId = 2085,
                EntryMedium = MediaMedium.Animeseries,
                EntryName = "Guilty Crown",
                EntryType = MediaEntryType.Anime
            };
        }
    }
}