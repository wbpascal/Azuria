using System.Linq;
using Azuria.Api.v1.DataModels.List;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.List
{
    public class TranslatorProjectDataModelTest : DataModelsTestBase<TranslatorProjectDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["list_gettranslatorgroupprojects.json"];
            ProxerApiResponse<TranslatorProjectDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(1, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        public static TranslatorProjectDataModel BuildDataModel()
        {
            return new TranslatorProjectDataModel
            {
                EntryFsk = new []{Fsk.Fsk12},
                EntryGenre = new []{Genre.Comedy, Genre.MartialArt},
                EntryId = 3247,
                EntryMedium = MediaMedium.Animeseries,
                EntryName = "Accel World",
                EntryRatingsCount = 6164,
                EntryRatingsSum = 49674,
                EntryStatus = MediaStatus.Completed,
                Status = TranslationStatus.Cancelled
            };
        }
    }
}