using System.Threading.Tasks;
using Azuria.Main;
using Azuria.Test.Attributes;
using Azuria.Utilities.ErrorHandling;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Azuria.Test
{
    [TestFixture, LoginRequired]
    public class ProxerClassTest
    {
        [Test]
        public async Task GetAnimeMangaByIdTest()
        {
            ProxerResult<IAnimeMangaObject> lValidAnimeResult =
                await ProxerClass.GetAnimeMangaById(8455, SenpaiTest.Senpai);
            await Task.Delay(2000);
            ProxerResult<IAnimeMangaObject> lValidMangaResult =
                await ProxerClass.GetAnimeMangaById(7834, SenpaiTest.Senpai);
            await Task.Delay(2000);
            ProxerResult<IAnimeMangaObject> lInvalidResult = await ProxerClass.GetAnimeMangaById(-1, SenpaiTest.Senpai);

            Assert.IsTrue(lValidAnimeResult.Success && lValidAnimeResult.Result != null);
            Assert.IsInstanceOfType(lValidAnimeResult.Result, typeof(Anime));

            Assert.IsTrue(lValidMangaResult.Success && lValidMangaResult.Result != null);
            Assert.IsInstanceOfType(lValidMangaResult.Result, typeof(Manga));

            Assert.IsFalse(lInvalidResult.Success);
        }
    }
}