using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class ListInfoDataModelTest : DataModelsTestBase<ListInfoDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getlistinfo.json"];
            ProxerApiResponse<ListInfoDataModel> lResponse = this.Convert(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        public static ListInfoDataModel BuildDataModel()
        {
            return new ListInfoDataModel
            {
                Category = MediaEntryType.Anime,
                ContentObjects = new[]
                {
                    new MediaContentDataModel
                    {
                        ContentIndex = 1,
                        Language = MediaLanguage.EngSub,
                        StreamHosterImages = new[]
                            {"mp4upload.png", "yourupload.png", "viewster.png", "proxer-stream.png", "streamcloud.png"},
                        StreamHosters = new[] {"mp4upload", "yourupload", "viewster", "proxer-stream", "streamcloud2"},
                        Title = null
                    },
                    new MediaContentDataModel
                    {
                        ContentIndex = 4,
                        Language = MediaLanguage.GerSub,
                        StreamHosterImages = new[] {"viewster.png", "akibapass.png"},
                        StreamHosters = new[] {"viewster", "akibapass"},
                        Title = null
                    }
                },
                EndIndex = 22,
                Languages = new [] {MediaLanguage.GerSub, MediaLanguage.EngSub},
                StartIndex = 1
            };
        }
    }
}