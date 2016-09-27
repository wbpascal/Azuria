using System;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.UserInfo;
using Azuria.Utilities.Extensions;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture]
    public class SenpaiTest
    {
        [SetUp]
        public void Setup()
        {
            this._senpai = GeneralSetup.SenpaiInstance;
        }

        private Senpai _senpai;

        [Test]
        public void ConstructorTest()
        {
            Assert.Catch(() => new Senpai(""));
        }

        [Test]
        [Order(1)]
        public async Task FromTokenTest()
        {
            Senpai lNewSenpai =
                await
                    Senpai.FromToken(
                        ("bknnY3QLb0X7CHdfHzimhOLo1Tz4cpumHSJGTzp3Gx9i2lvJSO4aCOwBH4ERr0d92UMStcU5w3kylUfdHilg7SXL" +
                         "VuCfDQtCIfsapmiXmGsFyHSeZcv45kOXOoipcL2VYt6oNni02KOApFOmRhpvCbOox7OKPPDOhIa58sc5aYCxDrRs" +
                         "Ggjgp9FWetE3gfOxXYAYoK2wID4k3UKH95XvcCgo43qkhePdanby6a5OO67OXQv4Uty74Yt6YTpf7cs").ToCharArray())
                        .ThrowFirstForNonSuccess();
            Assert.IsTrue(lNewSenpai.IsProbablyLoggedIn);
            Assert.AreEqual(lNewSenpai.Username, this._senpai.Username, "Token differs from username!");

            Assert.CatchAsync<ArgumentException>(async () => await Senpai.FromToken(null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () => await Senpai.FromToken(new char[0]).ThrowFirstForNonSuccess());
        }

        [Test]
        public void IsProbablyLoggedInTest()
        {
            Assert.IsTrue(this._senpai.IsProbablyLoggedIn);
            Assert.IsFalse(new Senpai("ad").IsProbablyLoggedIn);
        }

        [Test]
        [Order(1)]
        public async Task LoginTest()
        {
            Assert.CatchAsync<ArgumentException>(
                async () => await new Senpai("ad").Login(null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(async () => await new Senpai("ad").Login("").ThrowFirstForNonSuccess());
            Assert.CatchAsync<AlreadyLoggedInException>(
                async () => await this._senpai.Login("password").ThrowFirstForNonSuccess());

            Senpai lNewSenpai = new Senpai("username");
            ProxerResult lLoginResult = await lNewSenpai.Login("wrong");
            Assert.IsFalse(lLoginResult.Success);

            lLoginResult = await lNewSenpai.Login("correct");
            Assert.IsTrue(lLoginResult.Success);

            Assert.IsTrue(lNewSenpai.IsProbablyLoggedIn);
            Assert.IsNotNull(lNewSenpai.Me);
            Assert.AreEqual(lNewSenpai.Username, await lNewSenpai.Me.UserName.ThrowFirstOnNonSuccess());
            Assert.IsNotEmpty(lNewSenpai.LoginToken.ReadValue());
        }

        [Test]
        public void LoginTokenTest()
        {
            string lLoginToken = new string(this._senpai.LoginToken.ReadValue());
            Assert.IsNotEmpty(lLoginToken);
            Assert.IsNotNull(new Senpai("ad").LoginToken.ReadValue());
        }

        [Test]
        [Order(2)]
        public async Task LogoutTest()
        {
            Assert.CatchAsync<NotLoggedInException>(
                async () => await new Senpai("ad").Logout().ThrowFirstForNonSuccess());

            Senpai lNewSenpai = new Senpai("username");
            ProxerResult lLoginResult = await lNewSenpai.Login("correct");
            Assert.IsTrue(lLoginResult.Success);

            await lNewSenpai.Logout().ThrowFirstForNonSuccess();
            Assert.IsFalse(lNewSenpai.IsProbablyLoggedIn);
        }

        [Test]
        public async Task MeTest()
        {
            User lMe = this._senpai.Me;
            Assert.NotNull(lMe);
            Assert.AreEqual(await lMe.UserName.ThrowFirstOnNonSuccess(), this._senpai.Username);
            Assert.IsNull(new Senpai("ad").Me);
        }

        [Test]
        public void UsernameTest()
        {
            Assert.IsNotEmpty(this._senpai.Username);
            Assert.AreEqual(this._senpai.Username, "username");
        }
    }
}