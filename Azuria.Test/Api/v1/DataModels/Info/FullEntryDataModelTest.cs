using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class FullEntryDataModelTest : DataModelsTestBase<FullEntryDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getfullentry.json"];
            ProxerApiResponse<FullEntryDataModel> lResponse = this.Convert(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        public static FullEntryDataModel BuildDataModel()
        {
            EntryDataModel lEntryDataModel = EntryDataModelTest.BuildDataModel();
            IndustryBasicDataModel lIndustryBasicDataModel = IndustryBasicDataModelTest.BuildDataModel();
            EntryNameDataModel lNameDataModel = EntryNameDataModelTest.BuildDataModel();
            SeasonDataModel lSeasonDataModel = SeasonDataModelTest.BuildDataModel();
            //Entry id is not included, set it back to default value
            lSeasonDataModel.EntryId = int.MinValue;

            TagDataModel lTagDataModel = TagDataModelTest.BuildDataModel();
            TranslatorBasicDataModel lTranslatorBasicDataModel = TranslatorBasicDataModelTest.BuildDataModel();
            return new FullEntryDataModel
            {
                AdaptionData = new AdaptionDataModel
                {
                    EntryId = 8899,
                    Medium = MediaMedium.Mangaseries,
                    Name = "Shigatsu wa Kimi no Uso"
                },
                AdaptionType = AdaptionType.Entry,
                AdaptionValue = "8899",
                Clicks = lEntryDataModel.Clicks,
                ContentCount = lEntryDataModel.ContentCount,
                Description = lEntryDataModel.Description,
                EntryId = lEntryDataModel.EntryId,
                EntryMedium = lEntryDataModel.EntryMedium,
                EntryName = lEntryDataModel.EntryName,
                EntryType = lEntryDataModel.EntryType,
                Fsk = lEntryDataModel.Fsk,
                Genre = lEntryDataModel.Genre,
                IsLicensed = lEntryDataModel.IsLicensed,
                RatingsCount = lEntryDataModel.RatingsCount,
                RatingsSum = lEntryDataModel.RatingsSum,
                Status = lEntryDataModel.Status,
                AvailableLanguages = new[] {MediaLanguage.GerSub, MediaLanguage.EngSub},
                IsHContent = false,
                Industry = new[] {lIndustryBasicDataModel},
                Names = new[] {lNameDataModel},
                Seasons = new[] {lSeasonDataModel},
                Tags = new[] {lTagDataModel},
                Translator = new[] {lTranslatorBasicDataModel}
            };
        }
    }
}