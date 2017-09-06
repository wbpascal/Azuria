using Azuria.Api.v1.DataModels.Anime;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Anime
{
    [TestFixture]
    public class StreamDataModelTest : DataModelsTestBase<StreamDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["anime_getstreams.json"];
            ProxerApiResponse<StreamDataModel[]> lResponse = this.ConvertArray(lJson);
            this.CheckSuccessResponse(lResponse);
            Assert.IsNotEmpty(lResponse.Result);

            StreamDataModel[] lDataModels = lResponse.Result;
            Assert.AreEqual(401217, lDataModels[0].StreamId);
            Assert.AreEqual("mp4upload", lDataModels[0].StreamHoster);
            Assert.AreEqual("iframe", lDataModels[0].HostingType);
            Assert.AreEqual("MP4Upload", lDataModels[0].HosterFullName);
            Assert.AreEqual("mp4upload.png", lDataModels[0].HosterImageFileName);
            Assert.AreEqual("http://www.mp4upload.com/embed-#.html", lDataModels[0].PlaceholderLink);
            Assert.AreEqual(205400, lDataModels[0].UploaderId);
            Assert.AreEqual("Tadakuni", lDataModels[0].UploaderName);
            Assert.AreEqual(DateTimeHelpers.UnixTimeStampToDateTime(1412882290), lDataModels[0].UploadTimestamp);
            Assert.AreEqual(1158, lDataModels[0].TranslatorId);
            Assert.AreEqual("THORAnime", lDataModels[0].TranslatorName);

            Assert.AreEqual(515544, lDataModels[1].StreamId);
            Assert.Null(lDataModels[1].TranslatorId);
            Assert.Null(lDataModels[1].TranslatorName);
        }
    }
}