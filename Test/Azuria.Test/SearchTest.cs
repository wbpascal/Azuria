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
            Anime[] lResult = SearchHelper.EntryList<Anime>(input =>
            {
                input.Medium = AnimeMangaMedium.Animeseries;
                input.StartWithNonAlphabeticalChar = true;
                input.StartWith = "a";
            }).ToArray();
            Assert.IsNotNull(lResult);
            Assert.AreEqual(lResult.Length, 5);
            Assert.IsTrue(
                lResult.All(anime => new Regex(@"^[^a-zA-Z].*").IsMatch(anime.Name.GetObjectIfInitialised("ERROR"))));
            Assert.IsTrue(
                lResult.All(anime => anime.AnimeMedium.GetObjectIfInitialised(AnimeMedium.Unknown) == AnimeMedium.Series));
        }
    }
}