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
            Assert.Catch<ArgumentException>(() => SearchHelper.EntryList<IMediaObject>(new EntryListInput()));

            Manga[] lResult = SearchHelper.EntryList<Manga>(input =>
            {
                input.Medium = MediaMedium.Mangaseries;
                input.StartWithNonAlphabeticalChar = true;
                input.StartWith = "a";
            }).ToArray();
            Assert.IsNotNull(lResult);
            Assert.AreEqual(5, lResult.Length);
            Assert.IsTrue(
                lResult.All(anime => new Regex(@"^[^a-zA-Z].*").IsMatch(anime.Name.GetIfInitialised("ERROR"))));
            Assert.IsTrue(
                lResult.All(anime => anime.MangaMedium.GetIfInitialised(MangaMedium.Unknown) == MangaMedium.Series));
        }

        [Test]
        public void EntrySearchTest()
        {
            Assert.Catch<ArgumentException>(() => SearchHelper.Search<IMediaObject>((SearchInput) null));

            IMediaObject[] lResult = SearchHelper.Search<IMediaObject>(input =>
            {
                input.Length = 50;
                input.Fsk = new[] {Fsk.Fsk12};
                input.GenreExclude = new[] {Genre.Ecchi};
                input.GenreInclude = new[] {Genre.Action};
                input.IsFilteringSpoilerTags = true;
                input.IsFilteringUnratedTags = true;
                input.Language = SearchLanguage.English;
                input.LengthLimit = LengthLimit.Down;
                input.SearchTerm = "a";
                input.Sort = SearchResultSort.Name;
                input.TagsExclude = new[] {TagType.Alcohol};
                input.TagsInclude = new[] {TagType.FemaleProtagonist};
                input.Type = MediaSearchType.All;
            }).ToArray();
            Assert.IsNotNull(lResult);
            Assert.AreEqual(3, lResult.Length);
            Assert.IsTrue(lResult.All(o => o.ContentCount.GetIfInitialised(int.MaxValue) <= 50));
            Assert.IsTrue(lResult.All(o => o.Name.GetIfInitialised("ERROR").Contains("a")));
            Assert.IsTrue(lResult.All(o => o.Genre.GetIfInitialised(new Genre[0]).Contains(Genre.Action)));

            Anime[] lAnimeResult = SearchHelper.Search<Anime>(input =>
            {
                input.Length = 50;
                input.Fsk = new[] {Fsk.Fsk12};
                input.GenreExclude = new[] {Genre.Ecchi};
                input.GenreInclude = new[] {Genre.Action};
                input.IsFilteringSpoilerTags = true;
                input.IsFilteringUnratedTags = true;
                input.Language = SearchLanguage.English;
                input.LengthLimit = LengthLimit.Down;
                input.SearchTerm = "a";
                input.Sort = SearchResultSort.Name;
                input.TagsExclude = new[] {TagType.Alcohol};
                input.TagsInclude = new[] {TagType.FemaleProtagonist};
                input.Type = MediaSearchType.All;
            }).ToArray();
            Assert.IsNotNull(lAnimeResult);
            Assert.AreEqual(3, lAnimeResult.Length);

            Manga[] lMangaResult = SearchHelper.Search<Manga>(input =>
            {
                input.Length = 50;
                input.Fsk = new[] {Fsk.Fsk12};
                input.GenreExclude = new[] {Genre.Ecchi};
                input.GenreInclude = new[] {Genre.Action};
                input.IsFilteringSpoilerTags = true;
                input.IsFilteringUnratedTags = true;
                input.Language = SearchLanguage.English;
                input.LengthLimit = LengthLimit.Down;
                input.SearchTerm = "a";
                input.Sort = SearchResultSort.Name;
                input.TagsExclude = new[] {TagType.Alcohol};
                input.TagsInclude = new[] {TagType.FemaleProtagonist};
                input.Type = MediaSearchType.All;
            }).ToArray();
            Assert.IsNotNull(lMangaResult);
            Assert.IsEmpty(lMangaResult);
        }
    }
}