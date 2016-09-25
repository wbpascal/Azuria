using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Azuria.Test
{
    [TestFixture]
    public class SenpaiTests
    {
        [Test]
        public void ConstructorTest()
        {
            Assert.Catch(() => new Senpai(""));
        }

        [Test]
        public void IsProbablyLoggedInTest()
        {
            Assert.IsTrue(SenpaiSetup.Senpai.IsProbablyLoggedIn);
            Assert.IsFalse(new Senpai("ad").IsProbablyLoggedIn);
        }

        [Test]
        public void LoginCookiesTest()
        {
            Assert.IsTrue(SenpaiSetup.Senpai.LoginCookies.Count > 0);
            Assert.IsTrue(SenpaiSetup.Senpai.LoginCookies.GetCookies(new Uri("https://proxer.me")).Count > 0);
        }

        [Test]
        public void LoginTokenTest()
        {
            string lLoginToken = new string(SenpaiSetup.Senpai.LoginToken.ReadValue());
            Assert.IsNotEmpty(lLoginToken);
            Assert.IsNotNull(new Senpai("ad").LoginToken.ReadValue());
        }

        [Test]
        public async Task MeTest()
        {
            var lMe = SenpaiSetup.Senpai;
            Assert.NotNull(lMe);
        }
    }
}
