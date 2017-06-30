using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums.Messenger;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using Xunit;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    public class MessengerRequestBuilderTest : RequestBuilderTestBase<MessengerRequestBuilder>
    {
        [Fact]
        public void GetConferenceInfoTest()
        {
            IRequestBuilderWithResult<ConferenceInfoDataModel> lRequest = this.RequestBuilder.GetConferenceInfo(42);
            this.CheckUrl(lRequest, "messenger", "conferenceinfo");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.Equal("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(ConferenceList.Block)]
        [InlineData(ConferenceList.Default)]
        [InlineData(ConferenceList.Group)]
        [InlineData(ConferenceList.Favour)]
        public void GetConferencesTest(ConferenceList list)
        {
            IRequestBuilderWithResult<ConferenceDataModel[]> lRequest = this.RequestBuilder.GetConferences(list, 51);
            this.CheckUrl(lRequest, "messenger", "conferences");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.Equal(list.ToString().ToLowerInvariant(), lRequest.GetParameters["type"]);
            Assert.Equal("51", lRequest.GetParameters["p"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void GetConstantsTest()
        {
            IRequestBuilderWithResult<ConstantsDataModel> lRequest = this.RequestBuilder.GetConstants();
            this.CheckUrl(lRequest, "messenger", "constants");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.False(lRequest.CheckLogin);
        }

        [Theory]
        [InlineData(42, 122351, false)]
        [InlineData(7326, 526632, true)]
        public void GetMessagesTest(int conferenceId, int messageId, bool markAsRead)
        {
            IRequestBuilderWithResult<MessageDataModel[]> lRequest =
                this.RequestBuilder.GetMessages(conferenceId, messageId, markAsRead);
            this.CheckUrl(lRequest, "messenger", "messages");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.True(lRequest.GetParameters.ContainsKey("message_id"));
            Assert.True(lRequest.GetParameters.ContainsKey("read"));
            Assert.Equal(conferenceId.ToString(), lRequest.GetParameters["conference_id"]);
            Assert.Equal(messageId.ToString(), lRequest.GetParameters["message_id"]);
            Assert.Equal(markAsRead.ToString().ToLowerInvariant(), lRequest.GetParameters["read"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void NewConferenceTest()
        {
            string lUsername = RandomHelper.GetRandomString(20);
            string lText = RandomHelper.GetRandomString(40);

            IRequestBuilderWithResult<int> lRequest = this.RequestBuilder.NewConference(lUsername, lText);
            this.CheckUrl(lRequest, "messenger", "newconference");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("username"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.Equal(lUsername, lRequest.PostParameter.GetValue("username").First());
            Assert.Equal(lText, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void NewConferenceGroupTest()
        {
            IEnumerable<string> lParticipants = new string[5].Select(s => RandomHelper.GetRandomString(20)).ToArray();
            string lTopic = RandomHelper.GetRandomString(40);
            string lText = RandomHelper.GetRandomString(40);

            IRequestBuilderWithResult<int> lRequest =
                this.RequestBuilder.NewConferenceGroup(lParticipants, lTopic, lText);
            this.CheckUrl(lRequest, "messenger", "newconferencegroup");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("users[]"));
            Assert.True(lRequest.PostParameter.ContainsKey("topic"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.Equal(lParticipants, lRequest.PostParameter.GetValue("users[]").ToArray());
            Assert.Equal(lTopic, lRequest.PostParameter.GetValue("topic").First());
            Assert.Equal(lText, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void SetBlockTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetBlock(42);
            this.CheckUrl(lRequest, "messenger", "setblock");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.Equal("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void SetFavourTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetFavour(42);
            this.CheckUrl(lRequest, "messenger", "setfavour");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.Equal("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void SetMessageTest()
        {
            string lMessage = RandomHelper.GetRandomString(40);

            IRequestBuilderWithResult<string> lRequest = this.RequestBuilder.SetMessage(42, lMessage);
            this.CheckUrl(lRequest, "messenger", "setmessage");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.Equal("42", lRequest.GetParameters["conference_id"]);
            Assert.Equal(lMessage, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void SetReadTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetRead(42);
            this.CheckUrl(lRequest, "messenger", "setread");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.Equal("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void SetReportTest()
        {
            string lReason = RandomHelper.GetRandomString(40);

            IRequestBuilder lRequest = this.RequestBuilder.SetReport(42, lReason);
            this.CheckUrl(lRequest, "messenger", "report");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.Equal("42", lRequest.GetParameters["conference_id"]);
            Assert.Equal(lReason, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void SetUnblockTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetUnblock(42);
            this.CheckUrl(lRequest, "messenger", "setunblock");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.Equal("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void SetUnfavourTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetUnfavour(42);
            this.CheckUrl(lRequest, "messenger", "setunfavour");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.Equal("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void SetUnreadTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetUnread(42);
            this.CheckUrl(lRequest, "messenger", "setunread");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.Equal("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}