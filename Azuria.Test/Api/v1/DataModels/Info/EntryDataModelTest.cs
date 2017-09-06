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
            this.CheckSuccessResponse(lResponse);
            CheckDataModel(lResponse.Result);
        }

        public static void CheckDataModel(EntryDataModel dataModel)
        {
            Assert.AreEqual(9200, dataModel.EntryId);
            Assert.AreEqual("Shigatsu wa Kimi no Uso", dataModel.EntryName);
            Assert.AreEqual(new[] {Genre.Comedy, Genre.Shounen, Genre.SliceOfLife}, dataModel.Genre);
            Assert.AreEqual(new[] {Fsk.Fsk12, Fsk.Fsk18}, dataModel.Fsk);
            Assert.AreEqual("Description Text", dataModel.Description);
            Assert.AreEqual(MediaMedium.Animeseries, dataModel.EntryMedium);
            Assert.AreEqual(22, dataModel.ContentCount);
            Assert.AreEqual(MediaStatus.Completed, dataModel.Status);
            Assert.AreEqual(40527, dataModel.RatingsSum);
            Assert.AreEqual(4371, dataModel.RatingsCount);
            Assert.AreEqual(23604, dataModel.Clicks);
            Assert.AreEqual(MediaEntryType.Anime, dataModel.EntryType);
            Assert.True(dataModel.IsLicensed);
        }
    }
}