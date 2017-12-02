using Azuria.Api.v1.DataModels.Manga;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Manga
{
    public class ChapterDataModelTest : DataModelsTestBase<ChapterDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["manga_getchapter.json"];
            ProxerApiResponse<ChapterDataModel> lResponse = this.Convert(lJson);
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        private static ChapterDataModel BuildDataModel()
        {
            return new ChapterDataModel
            {
                ChapterId = 241036,
                ChapterTitle = "Chapter 159",
                EntryId = 7834,
                Pages = new[]
                {
                    new PageDataModel
                    {
                        PageHeight = 1600,
                        PageWidth = 690,
                        ServerFileName = "1.jpg"
                    },
                    new PageDataModel
                    {
                        PageHeight = 1200,
                        PageWidth = 570,
                        ServerFileName = "2.jpg"
                    },
                },
                ServerId = 5,
                TranslatorId = 795,
                TranslatorName = "Dicescans",
                UploaderId = 177103,
                UploaderName = "InfiniteSoul",
                UploadTimestamp = DateTimeHelpers.UnixTimeStampToDateTime(1475095092)
            };
        }
    }
}