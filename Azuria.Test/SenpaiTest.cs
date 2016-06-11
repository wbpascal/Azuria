using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Notifications;
using Azuria.Test.Utility;
using Azuria.Utilities.ErrorHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Azuria.Test
{
    [TestFixture]
    public class SenpaiTest
    {
        public static Senpai Senpai = new Senpai();

        [Test]
        public async Task CheckLoginTest()
        {
            Senpai lNewSenpai = new Senpai();
            PrivateObject lPrivateSenpai = new PrivateObject(lNewSenpai);
            ProxerResult<bool> lNotLoggedInResult = await (Task<ProxerResult<bool>>) lPrivateSenpai.Invoke("CheckLogin");

            Assert.IsTrue(lNotLoggedInResult.Success);
            Assert.IsFalse(lNotLoggedInResult.Result);
            Assert.IsFalse(lNewSenpai.IsLoggedIn);

            await Task.Delay(2000);
        }

        [Test]
        public async Task CheckNotificationsTest()
        {
            PrivateObject lPrivateSenpai = new PrivateObject(Senpai);
            ProxerResult lNotLoggedInResult = await (Task<ProxerResult>) lPrivateSenpai.Invoke("CheckNotifications");

            Assert.IsTrue(lNotLoggedInResult.Success);

            await Task.Delay(2000);
        }

        [Test]
        public async Task ForcePropertyReloadTest()
        {
            //TODO: Test ForceCheckLogin
        }

        [Test]
        public void LoginCookiesTest()
        {
            CookieCollection lLoginCookies = Senpai.LoginCookies.GetCookies(new Uri("https://proxer.me"));
            Assert.IsFalse(lLoginCookies.ContainsCookie("device", "mobile"));
        }

        [Test, Order(1)]
        public async Task LoginTest()
        {
            ProxerResult<bool> lValid = await Senpai.Login(Credentials.Username, Credentials.Password);
            await Task.Delay(2000);
            ProxerResult<bool> lInvalidInput = await new Senpai().Login("", "");
            await Task.Delay(2000);
            ProxerResult<bool> lWrongCredentials = await new Senpai().Login("Test", "WrongPassword");

            Assert.IsTrue(lValid.Success);
            Assert.IsTrue(lValid.Result);
            Assert.IsTrue(lInvalidInput.Success);
            Assert.IsFalse(lInvalidInput.Result);
            Assert.IsTrue(lWrongCredentials.Success);
            Assert.IsFalse(lWrongCredentials.Result);

            await Task.Delay(2000);
        }

        [Test]
        public void MeTest()
        {
            Assert.IsNotNull(Senpai.Me);
        }

        [Test]
        public void MobileCookiesTest()
        {
            CookieCollection lMobileCookies = Senpai.MobileLoginCookies.GetCookies(new Uri("https://proxer.me"));
            Assert.IsTrue(lMobileCookies.ContainsCookie("device", "mobile"));
        }
    }
}