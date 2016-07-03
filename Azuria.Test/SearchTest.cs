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
                new[] {GenreType.Action, GenreType.Mecha},
                new[]
                {GenreType.Fantasy, GenreType.Romance},
                fskContains: new[] {FskType.Fsk16, FskType.BadWords}, language: Language.German);

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
                        o => o.Genre == GenreType.Action || o.Genre == GenreType.Mecha) == 2);
                Assert.IsFalse(
                    (await searchResult.Genre.GetObject(new GenreObject[0])).Any(
                        o => o.Genre == GenreType.Fantasy || o.Genre == GenreType.Romance));
                Assert.IsTrue(
                    (await searchResult.Fsk.GetObject(new FskObject[0])).Count(
                        o => o.FskType == FskType.Fsk16 || o.FskType == FskType.BadWords) == 2);
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