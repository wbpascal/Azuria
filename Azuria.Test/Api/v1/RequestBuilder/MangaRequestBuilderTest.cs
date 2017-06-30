using System;
using Azuria.Api.v1.DataModels.Manga;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using Xunit;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    public class MangaRequestBuilderTest : RequestBuilderTestBase<MangaRequestBuilder>
    {
        [Theory]
        [InlineData(5112, 51, Language.English)]
        [InlineData(14923, 12, Language.German)]
        public void GetChapterTest(int id, int episode, Language language)
        {
            IRequestBuilderWithResult<ChapterDataModel> lRequest =
                this.RequestBuilder.GetChapter(id, episode, language);
            this.CheckUrl(lRequest, "manga", "chapter");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.Equal(id.ToString(), lRequest.GetParameters["id"]);
            Assert.Equal(episode.ToString(), lRequest.GetParameters["episode"]);
            Assert.Equal(language.ToShortString(), lRequest.GetParameters["language"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Fact]
        public void GetChapterInvalidLanguageTest()
        {
            Assert.Throws<ArgumentException>(() => this.RequestBuilder.GetChapter(1924, 42, Language.Unkown));
        }

        [Fact]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}