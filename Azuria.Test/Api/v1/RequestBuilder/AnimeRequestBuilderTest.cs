using System;
using Azuria.Api.v1.DataModels.Anime;
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
            int lRandomId = this.GetRandomNumber(4200);
            IRequestBuilderWithResult<string> lRequest = this.RequestBuilder.GetLink(lRandomId);
            this.CheckUrl(lRequest, "anime", "link");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetProxerStreamTest([Values] AnimeLanguage language)
        {
            if(language == AnimeLanguage.Unknown)
            {
                Assert.Throws<ArgumentException>(
                    () => this.RequestBuilder.GetProxerStreams(42, 42, AnimeLanguage.Unknown)
                );
                return;
            }
            
            int lRandomId = this.GetRandomNumber(4200);
            int lRandomEpisode = this.GetRandomNumber(42);
            IRequestBuilderWithResult<StreamDataModel[]> lRequest =
                this.RequestBuilder.GetProxerStreams(lRandomId, lRandomEpisode, language);
            this.CheckUrl(lRequest, "anime", "proxerstreams");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual(lRandomEpisode.ToString(), lRequest.GetParameters["episode"]);
            Assert.AreEqual(language.ToString().ToLowerInvariant(), lRequest.GetParameters["language"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public void GetStreamTest([Values] AnimeLanguage language)
        {
            if(language == AnimeLanguage.Unknown)
            {
                Assert.Throws<ArgumentException>(
                    () => this.RequestBuilder.GetStreams(42, 42, AnimeLanguage.Unknown)
                );
                return;
            }
            
            int lRandomId = this.GetRandomNumber(4200);
            int lRandomEpisode = this.GetRandomNumber(42);
            IRequestBuilderWithResult<StreamDataModel[]> lRequest =
                this.RequestBuilder.GetStreams(lRandomId, lRandomEpisode, language);
            this.CheckUrl(lRequest, "anime", "streams");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("id"));
            Assert.True(lRequest.GetParameters.ContainsKey("episode"));
            Assert.True(lRequest.GetParameters.ContainsKey("language"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.GetParameters["id"]);
            Assert.AreEqual(lRandomEpisode.ToString(), lRequest.GetParameters["episode"]);
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