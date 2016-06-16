using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Search;
using Azuria.Test.Attributes;
using Azuria.Utilities.ErrorHandling;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture]
    public class SearchTest
    {
        [Test, LoginRequired]
        public async Task AnimeMangaSearch()
        {
            ProxerResult<SearchResult<Anime>> lSearchResult = SearchHelper.SearchAnimeManga<Anime>("a",
                SenpaiTest.Senpai,
                SearchHelper.AnimeMangaType.Animeseries,
                new[] {GenreObject.GenreType.Action, GenreObject.GenreType.Mecha},
                new[]
                {GenreObject.GenreType.Fantasy, GenreObject.GenreType.Romance},
                new[] {Fsk.Fsk16, Fsk.BadWords}, Language.German);

            Assert.IsTrue(lSearchResult.Success);
            Assert.IsNotNull(lSearchResult.Result);
            Assert.IsTrue(lSearchResult.Result.Take(10).Any());
            Assert.IsTrue(
                lSearchResult.Result.Take(10).All(
                    anime => anime.Name.GetObjectIfInitialised("ERROR").ToLower().Contains("a")));

            foreach (Anime searchResult in lSearchResult.Result.Take(10))
            {
                Assert.IsTrue((await searchResult.Name.GetObject("ERROR")).Contains("a"));
                Assert.IsTrue(
                    (await searchResult.Genre.GetObject(new GenreObject[0])).Count(
                        o => o.Genre == GenreObject.GenreType.Action || o.Genre == GenreObject.GenreType.Mecha) == 2);
                Assert.IsFalse(
                    (await searchResult.Genre.GetObject(new GenreObject[0])).Any(
                        o => o.Genre == GenreObject.GenreType.Fantasy || o.Genre == GenreObject.GenreType.Romance));
                Assert.IsTrue(
                    (await searchResult.Fsk.GetObject(new FskObject[0])).Count(
                        o => o.Fsk == Fsk.Fsk16 || o.Fsk == Fsk.BadWords) == 2);
                Assert.IsTrue(
                    (await searchResult.AvailableLanguages.GetObject(new AnimeLanguage[0])).Any(
                        language => language == AnimeLanguage.GerDub || language == AnimeLanguage.GerSub));
            }

            await Task.Delay(2000);
        }

        [Test]
        public async Task UserSearch()
        {
            ProxerResult<SearchResult<User.User>> lSearchResult = SearchHelper.Search<User.User>(Credentials.Username,
                new Senpai());

            Assert.IsTrue(lSearchResult.Success);
            Assert.IsNotNull(lSearchResult.Result);
            Assert.IsTrue(lSearchResult.Result.Take(10).Any());
            Assert.IsTrue(
                (await lSearchResult.Result.Take(1).First().UserName.GetObject("ERROR")).Equals(
                    Credentials.Username));

            await Task.Delay(2000);
        }
    }
}