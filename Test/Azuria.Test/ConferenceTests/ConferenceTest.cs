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
        [SetUp]
        public async Task Setup()
        {
            this._conference =
                (await Conference.GetConferences(GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess()).First
                    ().Conference;
            Assert.AreEqual(this._conference.Id, 124536);
        }

        private Conference _conference;

        [Test]
        [MaxTime(2000)]
        public async Task CheckMessagesTest()
        {
            Assert.IsFalse(this._conference.AutoCheck);
            this._conference.AutoCheck = true;
            ServerResponse lResponse = ResponseSetup.CreateForMessageAutoCheck();

            SemaphoreSlim lSemaphoreSlim = new SemaphoreSlim(0, 1);
            this._conference.NewMessageRecieved += (sender, messages) =>
            {
                Assert.IsNotNull(sender);
                Assert.IsNotNull(messages);
                Message[] lMessages = messages.ToArray();
                Assert.IsNotEmpty(lMessages);
                Assert.AreEqual(lMessages.Length, 1);
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
            Assert.AreEqual(Conference.MaxCharactersPerMessage, 65000);
            Assert.AreEqual(Conference.MaxCharactersTopic, 32);
            Assert.AreEqual(Conference.MaxUsersPerConference, 100);
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
            Assert.CatchAsync<NotLoggedInException>(
                () => Conference.CreateGroup(new[] {"KutoSan"}, "topic", new Senpai("username"))
                    .ThrowFirstForNonSuccess());

            Conference lConference =
                await Conference.CreateGroup(new[] {"KutoSan"}, "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsTrue(lConference.IsGroupConference);

            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    () => Conference.CreateGroup(new[] {GeneralSetup.SenpaiInstance.Username}, "hello",
                        GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess()).ErrorCode,
                ErrorCode.MessengerNotEnoughUsers);
        }

        [Test]
        public async Task CreateGroupTestUser()
        {
            Assert.CatchAsync<ArgumentException>(
                () => Conference.CreateGroup(new User[0], "topic", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                () => Conference.CreateGroup(new[] {User.System}, "topic", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                () => Conference.CreateGroup(new[] {new User(163825)}, "", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(() => Conference.CreateGroup(new[] {new User(163825)},
                new string(new char[Conference.MaxCharactersTopic + 1]),
                GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                () => Conference.CreateGroup(new[] {new User(163825)}, "topic", null)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                () => Conference.CreateGroup(new[] {new User(163825)}, "topic", new Senpai("username"))
                    .ThrowFirstForNonSuccess());

            Conference lConference =
                await Conference.CreateGroup(new[] {new User(163825)}, "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsTrue(lConference.IsGroupConference);

            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    () => Conference.CreateGroup(new[] {new User(int.MaxValue)}, "hello",
                        GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess()).ErrorCode,
                ErrorCode.UserinfoUserNotFound);
            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    () => Conference.CreateGroup(new[] {new User(GeneralSetup.SenpaiInstance.Me.Id)},
                        "hello",
                        GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess()).ErrorCode,
                ErrorCode.MessengerNotEnoughUsers);
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
            Assert.CatchAsync<NotLoggedInException>(() => Conference.Create("KutoSan", "hello", new Senpai("ad"))
                .ThrowFirstForNonSuccess());

            Conference lConference =
                await Conference.Create("KutoSan", "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsFalse(lConference.IsGroupConference);

            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    () => Conference.Create("KutoSa", "hello", GeneralSetup.SenpaiInstance)
                        .ThrowFirstForNonSuccess()).ErrorCode, ErrorCode.MessengerUserInvalid);
        }

        [Test]
        public async Task CreateTestUser()
        {
            Assert.CatchAsync<ArgumentException>(
                () => Conference.Create((User) null, "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                () => Conference.Create(new User(177103), "", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                () => Conference.Create(new User(177103), "hello", null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(() => Conference.Create(new User(177103), "hello", new Senpai("ad"))
                .ThrowFirstForNonSuccess());

            Conference lConference =
                await Conference.Create(new User(163825), "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsFalse(lConference.IsGroupConference);

            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    () => Conference.Create(new User(int.MaxValue), "hello", GeneralSetup.SenpaiInstance)
                        .ThrowFirstForNonSuccess()).ErrorCode, ErrorCode.UserinfoUserNotFound);
            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    () => Conference.Create(new User(177103), "hello", GeneralSetup.SenpaiInstance)
                        .ThrowFirstForNonSuccess()).ErrorCode, ErrorCode.MessengerUserInvalid);
        }

        [Test]
        public async Task GetConferencesTest()
        {
            Assert.CatchAsync<NotLoggedInException>(() => Conference.GetConferences(null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                () => Conference.GetConferences(new Senpai("ad")).ThrowFirstForNonSuccess());

            ConferenceInfo[] lConferences =
                (await Conference.GetConferences(GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess())
                    .ToArray();
            Assert.IsNotNull(lConferences);
            Assert.AreEqual(lConferences.Length, 3);
            Assert.True(lConferences.Skip(1).All(info => !info.UnreadMessages.Any()));
        }

        [Test]
        public async Task LeaderTest()
        {
            User lLeader = await this._conference.Leader.ThrowFirstOnNonSuccess();
            Assert.IsNotNull(lLeader);
            Assert.AreNotEqual(lLeader, User.System);
            Assert.AreEqual(lLeader.Id, 121658);
        }

        [Test]
        public void MessagesTest()
        {
            Message[] lMessages = this._conference.Messages.ToArray();
            Assert.IsNotEmpty(lMessages);
            Assert.AreEqual(lMessages.Length, 56);
        }

        [Test]
        public async Task ParticipantsTest()
        {
            User[] lParticipants = (await this._conference.Participants.ThrowFirstOnNonSuccess()).ToArray();
            Assert.IsNotNull(lParticipants);
            Assert.IsNotEmpty(lParticipants);
            Assert.AreEqual(lParticipants.Length, 10);
            Assert.IsTrue(lParticipants.All(user => (user != null) && (user != User.System)));
        }

        [Test]
        public async Task SendMessageTest()
        {
            Assert.CatchAsync<ArgumentException>(() => this._conference.SendMessage(null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(() => this._conference.SendMessage("").ThrowFirstForNonSuccess());

            string lValidMessage = await this._conference.SendMessage("message").ThrowFirstForNonSuccess();
            Assert.AreEqual(lValidMessage, string.Empty);

            string lValidCommand = await this._conference.SendMessage("/help").ThrowFirstForNonSuccess();
            Assert.IsFalse(string.IsNullOrEmpty(lValidCommand));
        }

        [Test]
        public async Task SendReportTest()
        {
            Assert.CatchAsync<ArgumentException>(() => this._conference.SendReport(null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(() => this._conference.SendReport("").ThrowFirstForNonSuccess());

            ProxerResult lResult = await this._conference.SendReport("Report Reason");
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");

            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(() => this._conference.SendReport("a").ThrowFirstForNonSuccess())
                    .ErrorCode, ErrorCode.MessengerReportTooShort);
        }

        [Test]
        public async Task SetBlock()
        {
            ProxerResult lResult = await this._conference.SetBlock(true);
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");

            lResult = await this._conference.SetBlock(false);
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");
        }

        [Test]
        public async Task SetFavour()
        {
            ProxerResult lResult = await this._conference.SetFavourite(true);
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");

            lResult = await this._conference.SetFavourite(false);
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");
        }

        [Test]
        public async Task SetUnread()
        {
            ProxerResult lResult = await this._conference.SetUnread();
            Assert.IsTrue(lResult.Success,
                $"{lResult.Exceptions.FirstOrDefault()?.GetType().FullName}: {lResult.Exceptions.FirstOrDefault()?.Message}");
        }

        [Test]
        public async Task TopicTest()
        {
            string lTopic = await this._conference.Topic.ThrowFirstOnNonSuccess();
            Assert.IsNotNull(lTopic);
            Assert.IsNotEmpty(lTopic);
            Assert.AreEqual(lTopic, "Proxer API - Diskussion");
        }
    }
}