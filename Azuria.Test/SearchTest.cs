using System.Linq;
using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.AnimeManga.Properties;
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
                    (await searchResult.Genre.GetObject(new GenreType[0])).Count(
                        o => o == GenreType.Action || o == GenreType.Mecha) == 2);
                Assert.IsFalse(
                    (await searchResult.Genre.GetObject(new GenreType[0])).Any(
                        o => o == GenreType.Fantasy || o == GenreType.Romance));
                Assert.IsTrue(
                    (await searchResult.Fsk.GetObject(new FskType[0])).Count(
                        o => o == FskType.Fsk16 || o == FskType.BadWords) == 2);
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
                new Senpai(Credentials.Username));

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