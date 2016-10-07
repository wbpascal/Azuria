using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.UserInfo;
using Azuria.Utilities.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.MediaTests
{
    [TestFixture]
    public class ChapterTest
    {
        private Manga.Chapter _chapter;

        [OneTimeSetUp]
        public async Task Setup()
        {
            Manga lManga = await AnimeMangaObject.CreateFromId(7834).ThrowFirstForNonSuccess() as Manga;
            Assert.IsNotNull(lManga);
            this._chapter = (await lManga.GetChapters(Language.English).ThrowFirstForNonSuccess()).LastOrDefault();
            Assert.IsNotNull(this._chapter);
        }

        [Test]
        public async Task ChapterIdTest()
        {
            ProxerResult<int> lResult = await this._chapter.ChapterId;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(lResult.Result, 241036);
        }

        [Test]
        public void ContentIndexTest()
        {
            Assert.AreEqual(this._chapter.ContentIndex, 162);
        }

        [Test]
        public void LanguageTest()
        {
            Assert.AreEqual(this._chapter.Language, Language.English);
            Assert.AreEqual(this._chapter.Language, Language.English);
        }

        [Test]
        public async Task PagesTest()
        {
            ProxerResult<IEnumerable<Manga.Chapter.Page>> lResult = await this._chapter.Pages;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotEmpty(lResult.Result);
            Assert.IsTrue(
                lResult.Result.All(
                    page =>
                        new Regex("https:\\/\\/manga[0-9]+\\.proxer\\.me\\/f\\/[0-9]+\\/[0-9]+\\/[\\S]+?\\.jpg")
                            .IsMatch(page.Image.AbsoluteUri)));
            Assert.IsTrue(lResult.Result.All(page => (page.Height != default(int)) && (page.Width != default(int))));
        }

        [Test]
        public void ParentObjectTest()
        {
            Assert.IsNotNull(this._chapter.ParentObject);
            Assert.AreEqual(this._chapter.ParentObject.Id, 7834);
        }

        [Test]
        public async Task TitleTest()
        {
            ProxerResult<string> lResult = await this._chapter.Title;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotEmpty(lResult.Result);
            Assert.AreEqual(lResult.Result, "Chapter 159");
        }

        [Test]
        public async Task TranslatorTest()
        {
            ProxerResult<Translator> lResult = await this._chapter.Translator;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(lResult.Result.Id, 795);
            Assert.AreEqual(lResult.Result.Name, "Dicescans");
            Assert.AreEqual(lResult.Result.Language, Language.English);
        }

        [Test]
        public async Task UploadDateTest()
        {
            ProxerResult<DateTime> lResult = await this._chapter.UploadDate;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreNotEqual(lResult.Result, DateTime.MinValue);
            Assert.AreNotEqual(lResult.Result, DateTime.MaxValue);
        }

        [Test]
        public async Task UploaderTest()
        {
            ProxerResult<User> lResult = await this._chapter.Uploader;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(lResult.Result.Id, 177103);
            Assert.AreEqual(lResult.Result.UserName.GetObjectIfInitialised(string.Empty), "InfiniteSoul");
        }
    }
}