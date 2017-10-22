using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Api.v1.Input.Messenger;
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
            ConferenceInfoInput lInput = new ConferenceInfoInput
            {
                ConferenceId = 42
            };
            IRequestBuilderWithResult<ConferenceInfoDataModel> lRequest = this.RequestBuilder.GetConferenceInfo(lInput);
            this.CheckUrl(lRequest, "messenger", "conferenceinfo");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void GetConferencesTest([Values] ConferenceList list)
        {
            ConferenceListInput lInput = new ConferenceListInput
            {
                List = list,
                Page = 51
            };
            IRequestBuilderWithResult<ConferenceDataModel[]> lRequest = this.RequestBuilder.GetConferences(lInput);
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
            MessageListInput lInput = new MessageListInput
            {
                ConferenceId = conferenceId,
                MessageId = messageId,
                MarkRead = markAsRead
            };
            IRequestBuilderWithResult<MessageDataModel[]> lRequest = this.RequestBuilder.GetMessages(lInput);
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
            NewConferenceInput lInput = new NewConferenceInput
            {
                Text = RandomHelper.GetRandomString(40),
                Username = RandomHelper.GetRandomString(10)
            };

            IRequestBuilderWithResult<int> lRequest = this.RequestBuilder.NewConference(lInput);
            this.CheckUrl(lRequest, "messenger", "newconference");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("username"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.AreEqual(lInput.Username, lRequest.PostParameter.GetValue("username").First());
            Assert.AreEqual(lInput.Text, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void NewConferenceGroupTest()
        {
            NewConferenceGroupInput lInput = new NewConferenceGroupInput
            {
                Text = RandomHelper.GetRandomString(40),
                Topic = RandomHelper.GetRandomString(25),
                Usernames = new string[5].Select(s => RandomHelper.GetRandomString(20)).ToArray()
            };

            IRequestBuilderWithResult<int> lRequest = this.RequestBuilder.NewConferenceGroup(lInput);
            this.CheckUrl(lRequest, "messenger", "newconferencegroup");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("users[]"));
            Assert.True(lRequest.PostParameter.ContainsKey("topic"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.AreEqual(lInput.Usernames, lRequest.PostParameter.GetValue("users[]").ToArray());
            Assert.AreEqual(lInput.Topic, lRequest.PostParameter.GetValue("topic").First());
            Assert.AreEqual(lInput.Text, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetBlockTest()
        {
            ConferenceIdInput lInput = new ConferenceIdInput
            {
                ConferenceId = 42
            };
            IRequestBuilder lRequest = this.RequestBuilder.SetBlock(lInput);
            this.CheckUrl(lRequest, "messenger", "setblock");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetFavourTest()
        {
            ConferenceIdInput lInput = new ConferenceIdInput
            {
                ConferenceId = 42
            };
            IRequestBuilder lRequest = this.RequestBuilder.SetFavour(lInput);
            this.CheckUrl(lRequest, "messenger", "setfavour");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetMessageTest()
        {
            SendMessageInput lInput = new SendMessageInput
            {
                ConferenceId = 42,
                Message = RandomHelper.GetRandomString(40)
            };

            IRequestBuilderWithResult<string> lRequest = this.RequestBuilder.SetMessage(lInput);
            this.CheckUrl(lRequest, "messenger", "setmessage");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("conference_id"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.AreEqual("42", lRequest.PostParameter.GetValue("conference_id").First());
            Assert.AreEqual(lInput.Message, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetReadTest()
        {
            ConferenceIdInput lInput = new ConferenceIdInput
            {
                ConferenceId = 42
            };
            IRequestBuilder lRequest = this.RequestBuilder.SetRead(lInput);
            this.CheckUrl(lRequest, "messenger", "setread");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetReportTest()
        {
            SendReportInput lInput = new SendReportInput
            {
                ConferenceId = 42,
                Reason = RandomHelper.GetRandomString(40)
            };

            IRequestBuilder lRequest = this.RequestBuilder.SetReport(lInput);
            this.CheckUrl(lRequest, "messenger", "report");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("conference_id"));
            Assert.True(lRequest.PostParameter.ContainsKey("text"));
            Assert.AreEqual("42", lRequest.PostParameter.GetValue("conference_id").First());
            Assert.AreEqual(lInput.Reason, lRequest.PostParameter.GetValue("text").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetUnblockTest()
        {
            ConferenceIdInput lInput = new ConferenceIdInput
            {
                ConferenceId = 42
            };
            IRequestBuilder lRequest = this.RequestBuilder.SetUnblock(lInput);
            this.CheckUrl(lRequest, "messenger", "setunblock");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetUnfavourTest()
        {
            ConferenceIdInput lInput = new ConferenceIdInput
            {
                ConferenceId = 42
            };
            IRequestBuilder lRequest = this.RequestBuilder.SetUnfavour(lInput);
            this.CheckUrl(lRequest, "messenger", "setunfavour");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("conference_id"));
            Assert.AreEqual("42", lRequest.GetParameters["conference_id"]);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void SetUnreadTest()
        {
            ConferenceIdInput lInput = new ConferenceIdInput
            {
                ConferenceId = 42
            };
            IRequestBuilder lRequest = this.RequestBuilder.SetUnread(lInput);
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