using System;
using Azuria.Api.v1.DataModels.Anime;
using Azuria.Api.v1.Input.Anime;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums.Info;
using Azuria.Requests.Builder;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    [TestFixture]
    public class AnimeRequestBuilderTest : RequestBuilderTestBase<AnimeRequestBuilder>
    {
        [Test]
        public void GetLinkTest()
        {
            GetLinkInput lInput = new GetLinkInput {Id = this.GetRandomNumber(4200)};
            IRequestBuilderWithResult<string> lRequest =
                this.RequestBuilder.GetLink(lInput);
            this.CheckUrl(lRequest, "anime", "link");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lInput.Id.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetProxerStreamTest([Values] AnimeLanguage language)
        {
            StreamListInput lInput = new StreamListInput
            {
                Id = this.GetRandomNumber(4200),
                Episode = this.GetRandomNumber(42),
                Language = language
            };

            if (language == AnimeLanguage.Unknown)
            {
                Assert.Throws<InvalidOperationException>(
                    () => this.RequestBuilder.GetProxerStreams(lInput)
                );
                return;
            }

            IRequestBuilderWithResult<StreamDataModel[]> lRequest =
                this.RequestBuilder.GetProxerStreams(lInput);
            this.CheckUrl(lRequest, "anime", "proxerstreams");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.AreEqual(lInput.Id.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual(lInput.Episode.ToString(), lRequest.GetParameters["episode"]);
            Assert.AreEqual(language.ToString().ToLowerInvariant(), lRequest.GetParameters["language"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetStreamTest([Values] AnimeLanguage language)
        {
            StreamListInput lInput = new StreamListInput
            {
                Id = this.GetRandomNumber(4200),
                Episode = this.GetRandomNumber(42),
                Language = language
            };
            
            if (language == AnimeLanguage.Unknown)
            {
                Assert.Throws<InvalidOperationException>(
                    () => this.RequestBuilder.GetStreams(lInput)
                );
                return;
            }

            IRequestBuilderWithResult<StreamDataModel[]> lRequest =
                this.RequestBuilder.GetStreams(lInput);
            this.CheckUrl(lRequest, "anime", "streams");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.AreEqual(lInput.Id.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual(lInput.Episode.ToString(), lRequest.GetParameters["episode"]);
            Assert.AreEqual(language.ToString().ToLowerInvariant(), lRequest.GetParameters["language"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}