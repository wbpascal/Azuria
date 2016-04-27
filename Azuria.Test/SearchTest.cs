using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Main;
using Azuria.Main.Search;
using Azuria.Utilities.ErrorHandling;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture]
    public class SearchTest
    {
        [Test]
        public async Task AnimeMangaSearch()
        {
            string lTest = Utility.RandomUtility.GetRandomHexString();
            ProxerResult<SearchResult<Anime>> lSearchResult =
                await SearchHelper.SearchAnimeManga<Anime>("Code Geass", new Senpai(),
                    SearchHelper.AnimeMangaType.AllAnime);

            Assert.IsTrue(lSearchResult.Success);
            Assert.IsNotNull(lSearchResult.Result);
            Assert.IsTrue(lSearchResult.Result.SearchResults.Any());
            Assert.IsTrue(
                lSearchResult.Result.SearchResults.All(
                    anime => anime.Name.GetObjectIfInitialised("ERROR").ToLower().Contains("code geass")));

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