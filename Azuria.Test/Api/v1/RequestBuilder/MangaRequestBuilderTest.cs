using Azuria.Api.v1.DataModels.Manga;
using Azuria.Api.v1.Input.Manga;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums.Info;
using Azuria.Helpers.Extensions;
using Azuria.Requests.Builder;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    [TestFixture]
    public class MangaRequestBuilderTest : RequestBuilderTestBase<MangaRequestBuilder>
    {
        public MangaRequestBuilderTest() : base(client => new MangaRequestBuilder(client))
        {
        }

        [Test]
        [TestCase(5112, 51, Language.English)]
        [TestCase(14923, 12, Language.German)]
        public void GetChapterTest(int id, int episode, Language language)
        {
            ChapterInfoInput lInput = new ChapterInfoInput
            {
                Chapter = episode,
                Id = id,
                Language = language
            };

            IRequestBuilderWithResult<ChapterDataModel> lRequest = this.RequestBuilder.GetChapter(lInput);
            this.CheckUrl(lRequest, "manga", "chapter");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.AreEqual(id.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual(episode.ToString(), lRequest.GetParameters["episode"]);
            Assert.AreEqual(language.ToShortString(), lRequest.GetParameters["language"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}