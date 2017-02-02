using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Community;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Test.Core;
using Azuria.UserInfo;
using Azuria.Utilities.Extensions;
using NUnit.Framework;

namespace Azuria.Test.ConferenceTests
{
    [TestFixture]
    public class ConferenceTest
    {
        [OneTimeSetUp]
        public async Task Setup()
        {
            this._conference = (await Conference.GetConferences(GeneralSetup.SenpaiInstance)
                .ThrowFirstForNonSuccess()).First().Conference;
            Assert.AreEqual(124536, this._conference.Id);
        }

        private Conference _conference;

        [Test]
        [MaxTime(2000)]
        public async Task CheckMessagesTest()
        {
            Assert.IsFalse(this._conference.AutoCheck);
            this._conference.AutoCheck = true;
            this._conference.AutoCheckInterval = TimeSpan.FromMilliseconds(100);
            ServerResponse lResponse = ResponseSetup.CreateForMessageAutoCheck();

            SemaphoreSlim lSemaphoreSlim = new SemaphoreSlim(0, 1);
            this._conference.NewMessageRecieved += (sender, messages) =>
            {
                Assert.IsNotNull(sender);
                Assert.IsNotNull(messages);
                Message[] lMessages = messages.ToArray();
                Assert.IsNotEmpty(lMessages);
                Assert.AreEqual(1, lMessages.Length);
                lSemaphoreSlim.Release();
            };
            await lSemaphoreSlim.WaitAsync(3000);

            this._conference.AutoCheck = false;
            ServerResponse.ServerResponses.Remove(lResponse);
        }

        [Test]
        public void ConstantsTest()
        {
            Assert.IsTrue(Conference.IsInitialised);
            Assert.AreEqual(65000, Conference.MaxCharactersPerMessage);
            Assert.AreEqual(32, Conference.MaxCharactersTopic);
            Assert.AreEqual(100, Conference.MaxUsersPerConference);
        }

        [Test]
        public async Task CreateGroupTestString()
        {
            Assert.CatchAsync<ArgumentException>(
                () => Conference.CreateGroup(new string[0], "topic", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                () => Conference.CreateGroup(new[] {""}, "topic", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                () => Conference.CreateGroup(new[] {"KutoSan"}, "", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(() => Conference.CreateGroup(new[] {"KutoSan"},
                new string(new char[Conference.MaxCharactersTopic + 1]),
                GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                () => Conference.CreateGroup(new[] {"KutoSan"}, "topic", null).ThrowFirstForNonSuccess());

            Conference lConference =
                await Conference.CreateGroup(new[] {"KutoSan"}, "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsTrue(lConference.IsGroupConference);

            Assert.AreEqual(ErrorCode.MessengerNotEnoughUsers,
                Assert.CatchAsync<ProxerApiException>(() =>
                    Conference.CreateGroup(new[] {GeneralSetup.SenpaiInstance.Me.UserName.GetIfInitialised()},
                        "hello", GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess()).ErrorCode);
        }

        [Test]
        public async Task CreateGroupTestUser()
        {
            User lTestUser = await User.FromId(163825).ThrowFirstForNonSuccess();

            Assert.CatchAsync<ArgumentException>(
                () => Conference.CreateGroup(new User[0], "topic", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                () => Conference.CreateGroup(new[] {User.System}, "topic", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                () => Conference.CreateGroup(new[] {lTestUser}, "", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(() => Conference.CreateGroup(new[] {lTestUser},
                new string(new char[Conference.MaxCharactersTopic + 1]),
                GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                () => Conference.CreateGroup(new[] {lTestUser}, "topic", null)
                    .ThrowFirstForNonSuccess());

            Conference lConference = await Conference.CreateGroup(
                    new[] {lTestUser}, "hello", GeneralSetup.SenpaiInstance)
                .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsTrue(lConference.IsGroupConference);
        }

        [Test]
        public async Task CreateTestString()
        {
            Assert.CatchAsync<ArgumentException>(() => Conference.Create("", "hello", GeneralSetup.SenpaiInstance)
                .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(() => Conference.Create("KutoSan", "", GeneralSetup.SenpaiInstance)
                .ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                () => Conference.Create("KutoSan", "hello", null).ThrowFirstForNonSuccess());

            Conference lConference =
                await Conference.Create("KutoSan", "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsFalse(lConference.IsGroupConference);

            Assert.AreEqual(ErrorCode.MessengerUserInvalid,
                Assert.CatchAsync<ProxerApiException>(
                    () => Conference.Create("KutoSa", "hello", GeneralSetup.SenpaiInstance)
                        .ThrowFirstForNonSuccess()).ErrorCode);
        }

        [Test]
        public async Task CreateTestUser()
        {
            User lTestUser = await User.FromId(163825).ThrowFirstForNonSuccess();

            Assert.CatchAsync<ArgumentException>(
                () => Conference.Create((User) null, "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                () => Conference.Create(lTestUser, "", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                () => Conference.Create(lTestUser, "hello", null).ThrowFirstForNonSuccess());

            Conference lConference =
                await Conference.Create(lTestUser, "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsFalse(lConference.IsGroupConference);
        }

        [Test]
        public async Task GetConferencesTest()
        {
            Assert.CatchAsync<NotLoggedInException>(() => Conference.GetConferences(null).ThrowFirstForNonSuccess());

            ConferenceInfo[] lConferences = (await Conference.GetConferences(GeneralSetup.SenpaiInstance)
                .ThrowFirstForNonSuccess()).ToArray();
            Assert.IsNotNull(lConferences);
            Assert.AreEqual(3, lConferences.Length);
            Assert.AreEqual(1, lConferences.Count(info => info.UnreadMessagesCount != 0));
        }

        [Test]
        public async Task LeaderTest()
        {
            User lLeader = await this._conference.Leader.ThrowFirstOnNonSuccess();
            Assert.IsNotNull(lLeader);
            Assert.AreNotEqual(lLeader, User.System);
            Assert.AreEqual(121658, lLeader.Id);
        }

        [Test]
        public void MessagesTest()
        {
            Message[] lMessages = this._conference.Messages.ToArray();
            Assert.IsNotEmpty(lMessages);
            Assert.AreEqual(56, lMessages.Length);
            Assert.IsTrue(lMessages.Any(message => message.Action == MessageAction.AddUser));
            Assert.IsTrue(lMessages.Any(message => message.Action == MessageAction.RemoveUser));
            Assert.IsTrue(lMessages.Any(message => message.Action == MessageAction.SetTopic));
            Assert.IsTrue(lMessages.Any(message => message.Action == MessageAction.SetLeader));
            Assert.IsTrue(lMessages.Any(message => message.Action == MessageAction.NoAction));
            Assert.IsTrue(lMessages.All(message => message.Conference == this._conference));
            Assert.IsTrue(lMessages.All(message => !string.IsNullOrEmpty(message.Content)));
            Assert.IsTrue(lMessages.All(message => message.Sender != null));
            Assert.IsTrue(lMessages.All(message => message.TimeStamp != default(DateTime)));
        }

        [Test]
        public async Task ParticipantsTest()
        {
            User[] lParticipants = (await this._conference.Participants.ThrowFirstOnNonSuccess()).ToArray();
            Assert.IsNotNull(lParticipants);
            Assert.IsNotEmpty(lParticipants);
            Assert.AreEqual(10, lParticipants.Length);
            Assert.IsTrue(lParticipants.All(user => user != null && user != User.System));
        }

        [Test]
        public async Task SendMessageTest()
        {
            Assert.CatchAsync<ArgumentException>(() => this._conference.SendMessage(null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(() => this._conference.SendMessage("").ThrowFirstForNonSuccess());

            string lValidMessage = await this._conference.SendMessage("message").ThrowFirstForNonSuccess();
            Assert.IsEmpty(lValidMessage);

            string lValidCommand = await this._conference.SendMessage("/help").ThrowFirstForNonSuccess();
            Assert.IsNotEmpty(lValidCommand);
        }

        [Test]
        public async Task SendReportTest()
        {
            Assert.CatchAsync<ArgumentException>(() => this._conference.SendReport(null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(() => this._conference.SendReport("").ThrowFirstForNonSuccess());

            IProxerResult lResult = await this._conference.SendReport("Report Reason");
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");

            Assert.AreEqual(ErrorCode.MessengerReportTooShort,
                Assert.CatchAsync<ProxerApiException>(() => this._conference.SendReport("a").ThrowFirstForNonSuccess())
                    .ErrorCode);
        }

        [Test]
        public async Task SetBlock()
        {
            IProxerResult lResult = await this._conference.SetBlock(true);
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");

            lResult = await this._conference.SetBlock(false);
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");
        }

        [Test]
        public async Task SetFavour()
        {
            IProxerResult lResult = await this._conference.SetFavourite(true);
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");

            lResult = await this._conference.SetFavourite(false);
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");
        }

        [Test]
        public async Task SetUnread()
        {
            IProxerResult lResult = await this._conference.SetUnread();
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");
        }

        [Test]
        public async Task TopicTest()
        {
            string lTopic = await this._conference.Topic.ThrowFirstOnNonSuccess();
            Assert.IsNotNull(lTopic);
            Assert.IsNotEmpty(lTopic);
            Assert.AreEqual("Proxer API - Diskussion", lTopic);
        }
    }
}