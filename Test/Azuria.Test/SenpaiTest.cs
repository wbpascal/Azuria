using System;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Security;
using Azuria.UserInfo;
using Azuria.Utilities.Extensions;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture]
    public class SenpaiTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            this._senpai = GeneralSetup.SenpaiInstance;
        }

        private Senpai _senpai;

        [Test]
        [Order(1)]
        public async Task FromTokenTest()
        {
            Senpai lNewSenpai = await Senpai.FromToken(
                ("bknnY3QLb0X7CHdfHzimhOLo1Tz4cpumHSJGTzp3Gx9i2lvJSO4aCOwBH4ERr0d92UMStcU5w3kylUfdHilg7SXL" +
                    "VuCfDQtCIfsapmiXmGsFyHSeZcv45kOXOoipcL2VYt6oNni02KOApFOmRhpvCbOox7OKPPDOhIa58sc5aYCxDrRs" +
                    "Ggjgp9FWetE3gfOxXYAYoK2wID4k3UKH95XvcCgo43qkhePdanby6a5OO67OXQv4Uty74Yt6YTpf7cs").ToCharArray())
                .ThrowFirstForNonSuccess();
            Assert.IsTrue(lNewSenpai.IsProbablyLoggedIn);
            Assert.AreEqual(this._senpai.Me.UserName.GetIfInitialised(), lNewSenpai.Me.UserName.GetIfInitialised());

            Assert.CatchAsync<ArgumentException>(async () => await Senpai.FromToken(null).ThrowFirstForNonSuccess());
            Assert.CatchAsync<ArgumentException>(
                async () => await Senpai.FromToken(new char[0]).ThrowFirstForNonSuccess());
        }

        [Test]
        public void IsProbablyLoggedInTest()
        {
            Assert.IsTrue(this._senpai.IsProbablyLoggedIn);
        }

        [Test]
        [Order(1)]
        public async Task LoginTest()
        {
            Assert.CatchAsync<ArgumentException>(
                async () => await Senpai.FromCredentials(null).ThrowFirstForNonSuccess());

            IProxerResult<Senpai> lLoginResult = await Senpai.FromCredentials(
                new ProxerCredentials("InfiniteSoul", "wrong".ToCharArray()));
            Assert.IsFalse(lLoginResult.Success);

            lLoginResult = await Senpai.FromCredentials(
                new ProxerCredentials("InfiniteSoul", "correct".ToCharArray()));
            Assert.IsTrue(lLoginResult.Success);
            Assert.IsNotNull(lLoginResult.Result);

            Senpai lNewSenpai = lLoginResult.Result;
            Assert.IsTrue(lNewSenpai.IsProbablyLoggedIn);
            Assert.IsNotNull(lNewSenpai.Me);
            Assert.AreEqual("InfiniteSoul", lNewSenpai.Me.UserName.GetIfInitialised());
            Assert.IsNotEmpty(lNewSenpai.LoginToken.ReadValue());
        }

        [Test]
        public void LoginTokenTest()
        {
            string lLoginToken = new string(this._senpai.LoginToken.ReadValue());
            Assert.IsNotEmpty(lLoginToken);
            Assert.AreEqual(255, lLoginToken.Length);
        }

        [Test]
        [Order(2)]
        public async Task LogoutTest()
        {
            IProxerResult<Senpai> lLoginResult = await Senpai.FromCredentials(
                new ProxerCredentials("InfiniteSoul", "correct".ToCharArray()));
            Assert.IsTrue(lLoginResult.Success);
            Assert.IsNotNull(lLoginResult.Result);
            Senpai lSenpai = lLoginResult.Result;

            await lSenpai.Logout().ThrowFirstForNonSuccess();
            Assert.IsFalse(lSenpai.IsProbablyLoggedIn);
        }

        [Test]
        public void MeTest()
        {
            User lMe = this._senpai.Me;
            Assert.NotNull(lMe);
            Assert.AreEqual("InfiniteSoul", lMe.UserName.GetIfInitialised());
        }
    }
}