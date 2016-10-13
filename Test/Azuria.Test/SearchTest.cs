using System.Linq;
using System.Text.RegularExpressions;
using Azuria.Media;
using Azuria.Media.Properties;
using Azuria.Search;
using Azuria.Search.Input;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture]
    public class SearchTest
    {
        [Test]
        public void EntryListTest()
        {
            Manga[] lResult = SearchHelper.EntryList<Manga>(input =>
            {
                input.Medium = AnimeMangaMedium.Mangaseries;
                input.StartWithNonAlphabeticalChar = true;
                input.StartWith = "a";
            }).ToArray();
            Assert.IsNotNull(lResult);
            Assert.AreEqual(lResult.Length, 5);
            Assert.IsTrue(
                lResult.All(anime => new Regex(@"^[^a-zA-Z].*").IsMatch(anime.Name.GetObjectIfInitialised("ERROR"))));
            Assert.IsTrue(
                lResult.All(anime => anime.MangaMedium.GetObjectIfInitialised(MangaMedium.Unknown) == MangaMedium.Series));
        }

        [Test]
        public void EntrySearchTest()
        {
            IAnimeMangaObject[] lResult = SearchHelper.Search<IAnimeMangaObject>(input =>
            {
                input.Length = 50;
                input.Fsk = new[] {FskType.Fsk12};
                input.GenreExclude = new[] {GenreType.Ecchi};
                input.GenreInclude = new[] {GenreType.Action};
                input.IsFilteringSpoilerTags = true;
                input.IsFilteringUnratedTags = true;
                input.Language = SearchLanguage.English;
                input.LengthLimit = LengthLimit.Down;
                input.SearchTerm = "a";
                input.Sort = SearchResultSort.Name;
                input.TagsExclude = new[] {TagType.Alcohol};
                input.TagsInclude = new[] {TagType.FemaleProtagonist};
                input.Type = AnimeMangaSearchType.All;
            }).ToArray();
            Assert.IsNotNull(lResult);
            Assert.IsNotEmpty(lResult);
            Assert.AreEqual(lResult.Length, 3);
            Assert.IsTrue(lResult.All(o => o.ContentCount.GetObjectIfInitialised(int.MaxValue) <= 50));
            Assert.IsTrue(lResult.All(o => o.Name.GetObjectIfInitialised("ERROR").Contains("a")));
            Assert.IsTrue(lResult.All(o => o.Genre.GetObjectIfInitialised(new GenreType[0]).Contains(GenreType.Action)));
        }
    }
}