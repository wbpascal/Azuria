using System;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    public class ListInfoDataModelTest : DataModelsTestBase<ListInfoDataModel>
    {
        [Fact]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getlistinfo.json"];
            ProxerApiResponse<ListInfoDataModel> lResponse = this.Convert(lJson);
            this.CheckSuccessResponse(lResponse);
            CheckDataModel(lResponse.Result);
        }

        public static void CheckDataModel(ListInfoDataModel dataModel)
        {
            void CheckContentObject(
                MediaContentDataModel contentModel, int contentIndex, MediaLanguage language, string[] hosters,
                string[] hosterImages, string title)
            {
                Assert.Equal(contentIndex, contentModel.ContentIndex);
                Assert.Equal(language, contentModel.Language);
                Assert.Equal(hosters, contentModel.StreamHosters);
                Assert.Equal(title, contentModel.Title);
            }

            Assert.Equal(1, dataModel.StartIndex);
            Assert.Equal(22, dataModel.EndIndex);
            Assert.Equal(MediaEntryType.Anime, dataModel.Category);
            CheckContentObject(
                dataModel.ContentObjects[0], 1, MediaLanguage.EngSub,
                new[] {"mp4upload", "yourupload", "viewster", "proxer-stream", "streamcloud2"},
                new[] {"mp4upload.png", "yourupload.png", "viewster.png", "proxer-stream.png", "streamcloud.png"},
                null);
            CheckContentObject(
                dataModel.ContentObjects[1], 4, MediaLanguage.GerSub,
                new[] {"viewster", "akibapass"},
                new[] {"viewster.png", "akibapass.png"},
                null);
        }
    }
}