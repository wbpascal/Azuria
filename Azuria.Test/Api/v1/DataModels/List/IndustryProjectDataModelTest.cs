using System.Linq;
using Azuria.Api.v1.DataModels.List;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.List
{
    public class IndustryProjectDataModelTest : DataModelsTestBase<IndustryProjectDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["list_getindustryprojects.json"];
            ProxerApiResponse<IndustryProjectDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(2, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static IndustryProjectDataModel BuildDataModel()
        {
            return new IndustryProjectDataModel
            {
                EntryFsk = new[] {Fsk.Fsk16, Fsk.BadLanguage, Fsk.Violence},
                EntryGenre = new[] {Genre.Action, Genre.Ecchi, Genre.Harem},
                EntryId = 7537,
                EntryMedium = MediaMedium.Mangaseries,
                EntryName = "Absolute Duo",
                EntryRatingsCount = 31,
                EntryRatingsSum = 235,
                EntryStatus = MediaStatus.Airing,
                IndustryRole = IndustryRole.Publisher
            };
        }
    }
}