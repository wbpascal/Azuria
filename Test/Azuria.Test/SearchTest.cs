using System;
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
            Assert.Catch<ArgumentException>(() => SearchHelper.EntryList<Anime>((EntryListInput) null));
            Assert.Catch<ArgumentException>(() => SearchHelper.EntryList<IAnimeMangaObject>(new EntryListInput()));

            Manga[] lResult = SearchHelper.EntryList<Manga>(input =>
            {
                input.Medium = AnimeMangaMedium.Mangaseries;
                input.StartWithNonAlphabeticalChar = true;
                input.StartWith = "a";
            }).ToArray();
            Assert.IsNotNull(lResult);
            Assert.AreEqual(5, lResult.Length);
            Assert.IsTrue(
                lResult.All(anime => new Regex(@"^[^a-zA-Z].*").IsMatch(anime.Name.GetObjectIfInitialised("ERROR"))));
            Assert.IsTrue(
                lResult.All(anime => anime.MangaMedium.GetObjectIfInitialised(MangaMedium.Unknown) == MangaMedium.Series));
        }

        [Test]
        public void EntrySearchTest()
        {
            Assert.Catch<ArgumentException>(() => SearchHelper.Search<IAnimeMangaObject>((SearchInput) null));

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
            Assert.AreEqual(3, lResult.Length);
            Assert.IsTrue(lResult.All(o => o.ContentCount.GetObjectIfInitialised(int.MaxValue) <= 50));
            Assert.IsTrue(lResult.All(o => o.Name.GetObjectIfInitialised("ERROR").Contains("a")));
            Assert.IsTrue(lResult.All(o => o.Genre.GetObjectIfInitialised(new GenreType[0]).Contains(GenreType.Action)));

            Anime[] lAnimeResult = SearchHelper.Search<Anime>(input =>
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
            Assert.IsNotNull(lAnimeResult);
            Assert.AreEqual(3, lAnimeResult.Length);

            Manga[] lMangaResult = SearchHelper.Search<Manga>(input =>
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
            Assert.IsNotNull(lMangaResult);
            Assert.IsEmpty(lMangaResult);
        }
    }
}