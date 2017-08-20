using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    public class EntryDataModelTest : DataModelsTestBase<EntryDataModel>
    {
        [Fact]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getentry.json"];
            ProxerApiResponse<EntryDataModel> lResponse = this.Convert(lJson);
            this.CheckSuccessResponse(lResponse);
            CheckDataModel(lResponse.Result);
        }

        public static void CheckDataModel(EntryDataModel dataModel)
        {
            Assert.Equal(9200, dataModel.EntryId);
            Assert.Equal("Shigatsu wa Kimi no Uso", dataModel.EntryName);
            Assert.Equal(new[] {Genre.Comedy, Genre.Shounen, Genre.SliceOfLife}, dataModel.Genre);
            Assert.Equal(new[] {Fsk.Fsk12, Fsk.Fsk18}, dataModel.Fsk);
            Assert.Equal("Description Text", dataModel.Description);
            Assert.Equal(MediaMedium.Animeseries, dataModel.EntryMedium);
            Assert.Equal(22, dataModel.ContentCount);
            Assert.Equal(MediaStatus.Completed, dataModel.Status);
            Assert.Equal(40527, dataModel.RatingsSum);
            Assert.Equal(4371, dataModel.RatingsCount);
            Assert.Equal(23604, dataModel.Clicks);
            Assert.Equal(MediaEntryType.Anime, dataModel.EntryType);
            Assert.True(dataModel.IsLicensed);
        }
    }
}