using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.UserInfo.ControlPanel;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.UcpTests
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
        public async Task DeleteToptenTest()
        {
            IProxerResult lResult = await this._toptenAnimeObject.Delete();
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
        }

        [Test]
        public void MediaObjectTest()
        {
            Assert.AreEqual(41, this._toptenAnimeObject.MediaObject.Id);
            Assert.AreEqual(4385, this._toptenMangaObject.MediaObject.Id);
            Assert.AreEqual("Code Geass: Hangyaku no Lelouch R2",
                this._toptenAnimeObject.MediaObject.Name.GetIfInitialised(string.Empty));
            Assert.AreEqual("Ao Haru Ride",
                this._toptenMangaObject.MediaObject.Name.GetIfInitialised(string.Empty));
            Assert.AreEqual(AnimeMedium.Series,
                this._toptenAnimeObject.MediaObject.AnimeMedium.GetIfInitialised(AnimeMedium.Unknown));
            Assert.AreEqual(MangaMedium.Series,
                this._toptenMangaObject.MediaObject.MangaMedium.GetIfInitialised(MangaMedium.Unknown));
        }

        [Test]
        public void ToptenIdTest()
        {
            Assert.AreEqual(1, this._toptenAnimeObject.ToptenId);
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