using System.Linq;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class RelationDataModelTest : DataModelsTestBase<RelationDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getrelations.json"];
            ProxerApiResponse<RelationDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        public static RelationDataModel BuildDataModel()
        {
            EntryDataModel lEntryDataModel = EntryDataModelTest.BuildDataModel();
            return new RelationDataModel
            {
                AvailableLanguages = new[] {MediaLanguage.EngSub},
                Clicks = lEntryDataModel.Clicks,
                ContentCount = lEntryDataModel.ContentCount,
                Description = lEntryDataModel.Description,
                EntryId = lEntryDataModel.EntryId,
                EntryMedium = lEntryDataModel.EntryMedium,
                EntryName = lEntryDataModel.EntryName,
                EntryType = lEntryDataModel.EntryType,
                Fsk = lEntryDataModel.Fsk,
                GenreRaw = lEntryDataModel.GenreRaw,
                IsLicensed = lEntryDataModel.IsLicensed,
                RatingsCount = lEntryDataModel.RatingsCount,
                RatingsSum = lEntryDataModel.RatingsSum,
                Season = Season.Spring,
                Status = lEntryDataModel.Status,
                Year = 2011
            };
        }
    }
}