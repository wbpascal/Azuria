using System;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Community;
using Azuria.Exceptions;
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
        }

        private Community.Conference _conference;

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
    }
}