using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class EntryDataModelTest : DataModelsTestBase<EntryDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getentry.json"];
            ProxerApiResponse<EntryDataModel> lResponse = this.Convert(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        public static EntryDataModel BuildDataModel()
        {
            return new EntryDataModel
            {
                AdaptionData = new AdaptionDataModel
                {
                    EntryId = 8899,
                    Medium = MediaMedium.Mangaseries,
                    Name = "Shigatsu wa Kimi no Uso"
                },
                AdaptionType = AdaptionType.Entry,
                AdaptionValue = "8899",
                Clicks = 23604,
                ContentCount = 22,
                Description = "Description Text",
                EntryId = 9200,
                EntryMedium = MediaMedium.Animeseries,
                EntryName = "Shigatsu wa Kimi no Uso",
                EntryType = MediaEntryType.Anime,
                Fsk = new[] {Fsk.Fsk12, Fsk.Fsk18},
                Genre = new[] {Genre.Comedy, Genre.Shounen, Genre.SliceOfLife},
                IsLicensed = true,
                RatingsCount = 4371,
                RatingsSum = 40527,
                Status = MediaStatus.Completed
            };
        }
    }
}