using System;
using System.Linq;
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
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static StreamDataModel BuildDataModel()
        {
            return new StreamDataModel
            {
                Count = 25,
                EntryName = "Entry Name Stream 1",
                HosterFullName = "MP4Upload",
                HosterImageFileName = "mp4upload.png",
                HostingType = "iframe",
                PlaceholderLink = "http://www.mp4upload.com/embed-#.html",
                StreamHoster = "mp4upload",
                StreamId = 401217,
                TranslatorId = 1158,
                TranslatorName = "THORAnime",
                UploaderId = 205400,
                UploaderName = "Tadakuni",
                UploadTimestamp = DateTimeHelpers.UnixTimeStampToDateTime(1412882290)
            };
        }
    }
}