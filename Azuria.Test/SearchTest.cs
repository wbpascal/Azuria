using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Main;
using Azuria.Main.Minor;
using Azuria.Main.Search;
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
            ProxerResult<SearchResult<Anime>> lSearchResult =
                await SearchHelper.SearchAnimeManga<Anime>("a", SenpaiTest.Senpai,
                    SearchHelper.AnimeMangaType.Animeseries,
                    new[] {GenreObject.GenreType.Action, GenreObject.GenreType.Mecha},
                    new[]
                    {GenreObject.GenreType.Fantasy, GenreObject.GenreType.Romance},
                    new[] {Fsk.Fsk16, Fsk.BadWords}, Language.German);

            Assert.IsTrue(lSearchResult.Success);
            Assert.IsNotNull(lSearchResult.Result);
            Assert.IsTrue(lSearchResult.Result.SearchResults.Any());
            Assert.IsTrue(
                lSearchResult.Result.SearchResults.All(
                    anime => anime.Name.GetObjectIfInitialised("ERROR").ToLower().Contains("a")));

            foreach (Anime searchResult in lSearchResult.Result.SearchResults)
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

            int lOldSearchResultsCount = lSearchResult.Result.SearchResults.Count();
            while (!lSearchResult.Result.SearchFinished)
            {
                ProxerResult<IEnumerable<Anime>> lNewSearchResults = await lSearchResult.Result.GetNextSearchResults();

                Assert.IsTrue(lNewSearchResults.Success);
                Assert.IsNotNull(lNewSearchResults.Result);

                int lNewSearchResultsCount = lSearchResult.Result.SearchResults.Count();
                if (lNewSearchResultsCount == lOldSearchResultsCount || !lNewSearchResults.Result.Any())
                    Assert.IsTrue(lSearchResult.Result.SearchFinished);

                lOldSearchResultsCount = lNewSearchResultsCount;

                await Task.Delay(1000);
            }

            await Task.Delay(2000);
        }

        [Test]
        public async Task UserSearch()
        {
            ProxerResult<SearchResult<User>> lSearchResult =
                await SearchHelper.Search<User>(Credentials.Username, new Senpai());

            Assert.IsTrue(lSearchResult.Success);
            Assert.IsNotNull(lSearchResult.Result);
            Assert.IsTrue(lSearchResult.Result.SearchResults.Any());
            Assert.IsTrue(
                (await lSearchResult.Result.SearchResults.First().UserName.GetObject("ERROR")).Equals(
                    Credentials.Username));

            int lOldSearchResultsCount = lSearchResult.Result.SearchResults.Count();
            while (!lSearchResult.Result.SearchFinished)
            {
                ProxerResult<IEnumerable<User>> lNewSearchResults = await lSearchResult.Result.GetNextSearchResults();

                Assert.IsTrue(lNewSearchResults.Success);
                Assert.IsNotNull(lNewSearchResults.Result);

                int lNewSearchResultsCount = lSearchResult.Result.SearchResults.Count();
                if (lNewSearchResultsCount == lOldSearchResultsCount || !lNewSearchResults.Result.Any())
                    Assert.IsTrue(lSearchResult.Result.SearchFinished);

                lOldSearchResultsCount = lNewSearchResultsCount;

                await Task.Delay(1000);
            }

            await Task.Delay(2000);
        }
    }
}