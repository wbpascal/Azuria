using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enums.Messenger;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    [TestFixture]
    public class MessengerRequestBuilderTest : RequestBuilderTestBase<MessengerRequestBuilder>
    {
        [Test]
        public void GetConferenceInfoTest()
        {
            IRequestBuilderWithResult<ConferenceInfoDataModel> lRequest = this.RequestBuilder.GetConferenceInfo(42);
            this.CheckUrl(lRequest, "messenger", "conferenceinfo");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void GetConferencesTest([Values] ConferenceList list)
        {
            IRequestBuilderWithResult<ConferenceDataModel[]> lRequest = this.RequestBuilder.GetConferences(list, 51);
            this.CheckUrl(lRequest, "messenger", "conferences");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("type"));
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.AreEqual(list.ToString().ToLowerInvariant(), lRequest.GetParameters["type"]);
            Assert.AreEqual("51", lRequest.GetParameters["p"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void GetConstantsTest()
        {
            IRequestBuilderWithResult<ConstantsDataModel> lRequest = this.RequestBuilder.GetConstants();
            this.CheckUrl(lRequest, "messenger", "constants");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        [TestCase(42, 122351, false)]
        [TestCase(7326, 526632, true)]
        public void GetMessagesTest(int conferenceId, int messageId, bool markAsRead)
        {
            IRequestBuilderWithResult<MessageDataModel[]> lRequest =
                this.RequestBuilder.GetMessages(conferenceId, messageId, markAsRead);
            this.CheckUrl(lRequest, "messenger", "messages");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.True(lRequest.GetParameters.ContainsKey("message_id"));
            Assert.True(lRequest.GetParameters.ContainsKey("read"));
            Assert.AreEqual(conferenceId.ToString(), lRequest.GetParameters["conference_id"]);
            Assert.AreEqual(messageId.ToString(), lRequest.GetParameters["message_id"]);
            Assert.AreEqual(markAsRead.ToString().ToLowerInvariant(), lRequest.GetParameters["read"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void NewConferenceTest()
        {
            string lUsername = RandomHelper.GetRandomString(20);
            string lText = RandomHelper.GetRandomString(40);

            IRequestBuilderWithResult<int> lRequest = this.RequestBuilder.NewConference(lUsername, lText);
            this.CheckUrl(lRequest, "messenger", "newconference");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("username"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.AreEqual(lUsername, lRequest.PostParameter.GetValue("username").First());
            Assert.AreEqual(lText, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void NewConferenceGroupTest()
        {
            IEnumerable<string> lParticipants = new string[5].Select(s => RandomHelper.GetRandomString(20)).ToArray();
            string lTopic = RandomHelper.GetRandomString(40);
            string lText = RandomHelper.GetRandomString(40);

            IRequestBuilderWithResult<int> lRequest =
                this.RequestBuilder.NewConferenceGroup(lParticipants, lTopic, lText);
            this.CheckUrl(lRequest, "messenger", "newconferencegroup");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("users[]"));
            Assert.True(lRequest.PostParameter.ContainsKey("topic"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.AreEqual(lParticipants, lRequest.PostParameter.GetValue("users[]").ToArray());
            Assert.AreEqual(lTopic, lRequest.PostParameter.GetValue("topic").First());
            Assert.AreEqual(lText, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetBlockTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetBlock(42);
            this.CheckUrl(lRequest, "messenger", "setblock");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetFavourTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetFavour(42);
            this.CheckUrl(lRequest, "messenger", "setfavour");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetMessageTest()
        {
            string lMessage = RandomHelper.GetRandomString(40);

            IRequestBuilderWithResult<string> lRequest = this.RequestBuilder.SetMessage(42, lMessage);
            this.CheckUrl(lRequest, "messenger", "setmessage");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.AreEqual(lMessage, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetReadTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetRead(42);
            this.CheckUrl(lRequest, "messenger", "setread");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetReportTest()
        {
            string lReason = RandomHelper.GetRandomString(40);

            IRequestBuilder lRequest = this.RequestBuilder.SetReport(42, lReason);
            this.CheckUrl(lRequest, "messenger", "report");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.AreEqual(lReason, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetUnblockTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetUnblock(42);
            this.CheckUrl(lRequest, "messenger", "setunblock");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetUnfavourTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetUnfavour(42);
            this.CheckUrl(lRequest, "messenger", "setunfavour");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetUnreadTest()
        {
            IRequestBuilder lRequest = this.RequestBuilder.SetUnread(42);
            this.CheckUrl(lRequest, "messenger", "setunread");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}