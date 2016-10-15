using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.UserInfo.ControlPanel;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.UserInfoTests.UcpTests
{
    [TestFixture]
    public class ToptenTest
    {
        private ToptenObject<Anime> _toptenAnimeObject;
        private ToptenObject<Manga> _toptenMangaObject;

        [OneTimeSetUp]
        public async Task Setup()
        {
            this._toptenAnimeObject = (await UcpSetup.ControlPanel.ToptenAnime.ThrowFirstOnNonSuccess()).First();
            this._toptenMangaObject = (await UcpSetup.ControlPanel.ToptenManga.ThrowFirstOnNonSuccess()).First();
        }

        [Test]
        public void AnimeMangaObjectTest()
        {
            Assert.AreEqual(41, this._toptenAnimeObject.AnimeMangaObject.Id);
            Assert.AreEqual(4385, this._toptenMangaObject.AnimeMangaObject.Id);
            Assert.AreEqual("Code Geass: Hangyaku no Lelouch R2",
                this._toptenAnimeObject.AnimeMangaObject.Name.GetObjectIfInitialised(string.Empty));
            Assert.AreEqual("Ao Haru Ride",
                this._toptenMangaObject.AnimeMangaObject.Name.GetObjectIfInitialised(string.Empty));
            Assert.AreEqual(AnimeMedium.Series,
                this._toptenAnimeObject.AnimeMangaObject.AnimeMedium.GetObjectIfInitialised(AnimeMedium.Unknown));
            Assert.AreEqual(MangaMedium.Series,
                this._toptenMangaObject.AnimeMangaObject.MangaMedium.GetObjectIfInitialised(MangaMedium.Unknown));
        }

        [Test]
        public async Task DeleteToptenTest()
        {
            ProxerResult lResult = await this._toptenAnimeObject.DeleteTopten();
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
        }

        [Test]
        public void ToptenIdTest()
        {
            Assert.AreEqual(1127035, this._toptenAnimeObject.ToptenId);
            Assert.AreEqual(1076532, this._toptenMangaObject.ToptenId);
        }

        [Test]
        public void UserControlPanelTest()
        {
            Assert.AreSame(UcpSetup.ControlPanel, this._toptenAnimeObject.UserControlPanel);
            Assert.AreSame(UcpSetup.ControlPanel, this._toptenMangaObject.UserControlPanel);
        }
    }
}