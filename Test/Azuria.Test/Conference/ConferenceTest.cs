using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Community;
using Azuria.Exceptions;
using Azuria.Test.Core;
using Azuria.UserInfo;
using Azuria.Utilities.Extensions;
using NUnit.Framework;

namespace Azuria.Test.Conference
{
    [TestFixture]
    public class ConferenceTest
    {
        [SetUp]
        public async Task Setup()
        {
            this._conference =
                (await Community.Conference.GetConferences(GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess()).First
                    ().Conference;
            Assert.AreEqual(this._conference.Id, 124536);
        }

        private Community.Conference _conference;

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
            await lSemaphoreSlim.WaitAsync();

            this._conference.AutoCheck = false;
            ServerResponse.ServerResponses.Remove(lResponse);
        }

        [Test]
        public void ConstantsTest()
        {
            Assert.IsTrue(Community.Conference.IsInitialised);
            Assert.AreEqual(Community.Conference.MaxCharactersPerMessage, 65000);
            Assert.AreEqual(Community.Conference.MaxCharactersTopic, 32);
            Assert.AreEqual(Community.Conference.MaxUsersPerConference, 100);
        }

        [Test]
        public async Task CreateGroupTestString()
        {
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new string[0], "topic", GeneralSetup.SenpaiInstance)
                            .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new[] {""}, "topic", GeneralSetup.SenpaiInstance)
                            .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new[] {"KutoSan"}, "", GeneralSetup.SenpaiInstance)
                            .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new[] {"KutoSan"},
                            new string(new char[Community.Conference.MaxCharactersTopic + 1]),
                            GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new[] {"KutoSan"}, "topic", null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new[] {"KutoSan"}, "topic", new Senpai("username"))
                            .ThrowFirstForNonSuccess());

            Community.Conference lConference =
                await Community.Conference.CreateGroup(new[] {"KutoSan"}, "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsTrue(lConference.IsGroupConference);

            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    async () =>
                        await
                            Community.Conference.CreateGroup(new[] {GeneralSetup.SenpaiInstance.Username}, "hello",
                                GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess()).ErrorCode,
                ErrorCode.MessengerNotEnoughUsers);
        }

        [Test]
        public async Task CreateGroupTestUser()
        {
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new User[0], "topic", GeneralSetup.SenpaiInstance)
                            .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new[] {User.System}, "topic", GeneralSetup.SenpaiInstance)
                            .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new[] {new User(163825)}, "", GeneralSetup.SenpaiInstance)
                            .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new[] {new User(163825)},
                            new string(new char[Community.Conference.MaxCharactersTopic + 1]),
                            GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new[] {new User(163825)}, "topic", null)
                            .ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                async () =>
                    await
                        Community.Conference.CreateGroup(new[] {new User(163825)}, "topic", new Senpai("username"))
                            .ThrowFirstForNonSuccess());

            Community.Conference lConference =
                await Community.Conference.CreateGroup(new[] {new User(163825)}, "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsTrue(lConference.IsGroupConference);

            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    async () =>
                        await
                            Community.Conference.CreateGroup(new[] {new User(int.MaxValue)}, "hello",
                                GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess()).ErrorCode,
                ErrorCode.UserinfoUserNotFound);
            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    async () =>
                        await
                            Community.Conference.CreateGroup(new[] {new User(GeneralSetup.SenpaiInstance.Me.Id)},
                                "hello",
                                GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess()).ErrorCode,
                ErrorCode.MessengerNotEnoughUsers);
        }

        [Test]
        public async Task CreateTestString()
        {
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.Create("", "hello", GeneralSetup.SenpaiInstance)
                            .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.Create("KutoSan", "", GeneralSetup.SenpaiInstance)
                            .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () => await Community.Conference.Create("KutoSan", "hello", null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                async () =>
                    await
                        Community.Conference.Create("KutoSan", "hello", new Senpai("ad"))
                            .ThrowFirstForNonSuccess());

            Community.Conference lConference =
                await Community.Conference.Create("KutoSan", "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsFalse(lConference.IsGroupConference);

            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    async () =>
                        await
                            Community.Conference.Create("KutoSa", "hello", GeneralSetup.SenpaiInstance)
                                .ThrowFirstForNonSuccess()).ErrorCode, ErrorCode.MessengerUserInvalid);
        }

        [Test]
        public async Task CreateTestUser()
        {
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.Create((User) null, "hello", GeneralSetup.SenpaiInstance)
                            .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () =>
                    await
                        Community.Conference.Create(new User(177103), "", GeneralSetup.SenpaiInstance)
                            .ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () => await Community.Conference.Create(new User(177103), "hello", null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                async () =>
                    await
                        Community.Conference.Create(new User(177103), "hello", new Senpai("ad"))
                            .ThrowFirstForNonSuccess());

            Community.Conference lConference =
                await Community.Conference.Create(new User(163825), "hello", GeneralSetup.SenpaiInstance)
                    .ThrowFirstForNonSuccess();
            Assert.IsNotNull(lConference);
            Assert.IsFalse(lConference.AutoCheck);
            Assert.AreNotEqual(lConference.Id, default(int));
            Assert.IsFalse(lConference.IsGroupConference);

            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    async () =>
                        await
                            Community.Conference.Create(new User(int.MaxValue), "hello", GeneralSetup.SenpaiInstance)
                                .ThrowFirstForNonSuccess()).ErrorCode, ErrorCode.UserinfoUserNotFound);
            Assert.AreEqual(
                Assert.CatchAsync<ProxerApiException>(
                    async () =>
                        await
                            Community.Conference.Create(new User(177103), "hello", GeneralSetup.SenpaiInstance)
                                .ThrowFirstForNonSuccess()).ErrorCode, ErrorCode.MessengerUserInvalid);
        }

        [Test]
        public async Task GetConferencesTest()
        {
            Assert.CatchAsync<ArgumentException>(
                async () => await Community.Conference.GetConferences(null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<NotLoggedInException>(
                async () => await Community.Conference.GetConferences(new Senpai("ad")).ThrowFirstForNonSuccess());

            ConferenceInfo[] lConferences =
                (await Community.Conference.GetConferences(GeneralSetup.SenpaiInstance).ThrowFirstForNonSuccess())
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
        public async Task TopicTest()
        {
            string lTopic = await this._conference.Topic.ThrowFirstOnNonSuccess();
            Assert.IsNotNull(lTopic);
            Assert.IsNotEmpty(lTopic);
            Assert.AreEqual(lTopic, "Proxer API - Diskussion");
        }
    }
}