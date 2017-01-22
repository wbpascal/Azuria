using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Info;
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
            Manga lManga = await MediaObject.CreateFromId(7834).ThrowFirstForNonSuccess() as Manga;
            Assert.IsNotNull(lManga);
            this._chapter = (await lManga.GetChapters(Language.English).ThrowFirstForNonSuccess()).LastOrDefault();
            Assert.IsNotNull(this._chapter);
        }

        [Test]
        public async Task ChapterIdTest()
        {
            IProxerResult<int> lResult = await this._chapter.ChapterId;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreEqual(241036, lResult.Result);
        }

        [Test]
        public void ContentIndexTest()
        {
            Assert.AreEqual(162, this._chapter.ContentIndex);
        }

        [Test]
        public void LanguageTest()
        {
            Assert.AreEqual(Language.English, this._chapter.Language);
        }

        [Test]
        public async Task PagesTest()
        {
            IProxerResult<IEnumerable<Manga.Chapter.Page>> lResult = await this._chapter.Pages;
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
            Assert.AreEqual(7834, this._chapter.ParentObject.Id);
        }

        [Test]
        public async Task TitleTest()
        {
            IProxerResult<string> lResult = await this._chapter.Title;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.IsNotEmpty(lResult.Result);
            Assert.AreEqual("Chapter 159", lResult.Result);
        }

        [Test]
        public async Task TranslatorTest()
        {
            IProxerResult<Translator> lResult = await this._chapter.Translator;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(795, lResult.Result.Id);
            Assert.AreEqual("Dicescans", lResult.Result.Name);
            Assert.AreEqual(Language.English, lResult.Result.Language);
        }

        [Test]
        public async Task UploadDateTest()
        {
            IProxerResult<DateTime> lResult = await this._chapter.UploadDate;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.AreNotEqual(lResult.Result, DateTime.MinValue);
            Assert.AreNotEqual(lResult.Result, DateTime.MaxValue);
        }

        [Test]
        public async Task UploaderTest()
        {
            IProxerResult<User> lResult = await this._chapter.Uploader;
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));
            Assert.IsNotNull(lResult.Result);
            Assert.AreEqual(177103, lResult.Result.Id);
            Assert.AreEqual("InfiniteSoul", lResult.Result.UserName.GetIfInitialised(string.Empty));
        }
    }
}