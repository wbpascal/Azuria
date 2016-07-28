using System;
using System.Net;
using System.Threading.Tasks;
using Azuria.Api;
using Azuria.Test.Utility;
using Azuria.Utilities.ErrorHandling;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture]
    public class SenpaiTest
    {
        public static Senpai Senpai = new Senpai(Credentials.Username);

        [Test]
        public void LoginCookiesTest()
        {
            CookieCollection lLoginCookies = Senpai.LoginCookies.GetCookies(new Uri("https://proxer.me"));
            Assert.IsFalse(lLoginCookies.ContainsCookie("device", "mobile"));
        }

        [Test, Order(1)]
        public async Task LoginTest()
        {
            ApiInfo.InitV1(Credentials.ApiKey);
            Senpai = new Senpai(Credentials.Username);

            ProxerResult<bool> lValid = await Senpai.Login(Credentials.Password);
            Assert.IsTrue(lValid.Success);
            Assert.IsTrue(lValid.Result);

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