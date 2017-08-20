using System.IO;
using Azuria.Api.v1.DataModels.Anime;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Anime
{
    public class StreamDataModelTest : DataModelsTestBase<StreamDataModel>
    {
        [Fact]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["anime_getstreams.json"];
            ProxerApiResponse<StreamDataModel[]> lResponse = this.ConvertArray(lJson);
            this.CheckSuccessResponse(lResponse);
            Assert.NotEmpty(lResponse.Result);

            StreamDataModel[] lDataModels = lResponse.Result;
            Assert.Equal(401217, lDataModels[0].StreamId);
            Assert.Equal("mp4upload", lDataModels[0].StreamHoster);
            Assert.Equal("iframe", lDataModels[0].HostingType);
            Assert.Equal("MP4Upload", lDataModels[0].HosterFullName);
            Assert.Equal("mp4upload.png", lDataModels[0].HosterImageFileName);
            Assert.Equal("http://www.mp4upload.com/embed-#.html", lDataModels[0].PlaceholderLink);
            Assert.Equal(205400, lDataModels[0].UploaderId);
            Assert.Equal("Tadakuni", lDataModels[0].UploaderName);
            Assert.Equal(DateTimeHelpers.UnixTimeStampToDateTime(1412882290), lDataModels[0].UploadTimestamp);
            Assert.Equal(1158, lDataModels[0].TranslatorId);
            Assert.Equal("THORAnime", lDataModels[0].TranslatorName);
            
            Assert.Equal(515544, lDataModels[1].StreamId);
            Assert.Null(lDataModels[1].TranslatorId);
            Assert.Null(lDataModels[1].TranslatorName);
        }
    }
}