using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Main;
using Azuria.Main.Minor;
using Azuria.Main.User;
using Azuria.Main.User.Comment;
using Azuria.Test.Attributes;
using Azuria.Test.Utility;
using Azuria.Utilities.ErrorHandling;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture, LoginRequired]
    public class AnimeTest
    {
        private readonly Senpai _senpai = SenpaiTest.Senpai;
        private Anime _anime;

        [Test, Order(2)]
        public async Task AnimeTypeTest()
        {
            Assert.IsNotNull(this._anime);
            Anime.AnimeType lAnimeType = await this._anime.AnimeTyp.GetObject(Anime.AnimeType.Unbekannt);
            Assert.AreNotEqual(lAnimeType, Anime.AnimeType.Unbekannt);
        }

        [Test, Order(2)]
        public async Task AvailableLanguagesTest()
        {
            Assert.IsNotNull(this._anime);
            IEnumerable<Anime.Language> lLanguages =
                await this._anime.AvailableLanguages.GetObject(new Anime.Language[0]);
            Assert.IsNotEmpty(lLanguages);
        }

        [Test, Order(2)]
        public void CoverTest()
        {
            Assert.IsNotNull(this._anime);
            Assert.IsTrue(this._anime.CoverUri.OriginalString.EndsWith(".jpg"));
        }

        [Test, Order(2)]
        public async Task DescriptionTest()
        {
            Assert.IsNotNull(this._anime);

            string lHexString = RandomUtility.GetRandomHexString();
            string lDescription = await this._anime.Description.GetObject(lHexString);
            Assert.AreNotEqual(lDescription, lHexString);
        }

        [Test, Order(2)]
        public async Task EnglishTitleTest()
        {
            Assert.IsNotNull(this._anime);

            string lHexString = RandomUtility.GetRandomHexString();
            string lEnglishTitle = await this._anime.EnglishTitle.GetObject(lHexString);
            Assert.AreNotEqual(lEnglishTitle, lHexString);
        }

        [Test, Order(2)]
        public async Task EpisodeCountTest()
        {
            Assert.IsNotNull(this._anime);
            int lEpisodeCount = await this._anime.EpisodeCount.GetObject(int.MinValue);
            Assert.AreNotEqual(lEpisodeCount, int.MinValue);
        }

        [Test, Order(2)]
        public async Task FskTest()
        {
            Assert.IsNotNull(this._anime);

            ProxerResult<IEnumerable<FskObject>> lFskResult = await this._anime.Fsk.GetObject();
            Assert.IsTrue(lFskResult.Success);
            Assert.IsNotNull(lFskResult.Result);
            if (lFskResult.Result.Any())
            {
                Assert.IsTrue(lFskResult.Result.All(o => o.FskPictureUri.OriginalString.EndsWith(".png")));
            }
            else
            {
                Assert.Pass("Keine Fsk gefunden!");
            }
        }

        [Test, Order(2)]
        public async Task GenreTest()
        {
            Assert.IsNotNull(this._anime);
            IEnumerable<GenreObject> lGenre = await this._anime.Genre.GetObject(new GenreObject[0]);
            Assert.IsNotEmpty(lGenre);
        }

        [Test, Order(2)]
        public async Task GermanTitleTest()
        {
            Assert.IsNotNull(this._anime);

            string lHexString = RandomUtility.GetRandomHexString();
            string lGermanTitle = await this._anime.GermanTitle.GetObject(lHexString);
            Assert.AreNotEqual(lGermanTitle, lHexString);
        }

        [Test, Order(1)]
        public async Task GetAnimeTest()
        {
            Assert.IsNotNull(this._senpai.Me);

            IEnumerable<KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<Anime>>> lAnimeList =
                await
                    this._senpai.Me.Anime.GetObject(
                        new KeyValuePair<AnimeMangaProgressState, AnimeMangaProgressObject<Anime>>[0]);
            IEnumerable<Anime> lFavouriteAnime = await this._senpai.Me.FavouriteAnime.GetObject(new Anime[0]);

            Assert.IsNotEmpty(lAnimeList);
            Assert.IsNotEmpty(lFavouriteAnime);

            this._anime = lAnimeList.First().Value.AnimeMangaObject;
            await Task.Delay(2000);
        }

        [Test, Order(3)]
        public async Task GetEpisodesTest()
        {
            Assert.IsNotNull(this._anime);

            IEnumerable<Anime.Language> lAvailableLanguages =
                await this._anime.AvailableLanguages.GetObject(new Anime.Language[0]);
            Assert.IsNotEmpty(lAvailableLanguages);

            ProxerResult<IEnumerable<Anime.Episode>> lEpisodes =
                await this._anime.GetEpisodes(lAvailableLanguages.First());
            Assert.IsTrue(lEpisodes.Success);
            Assert.IsNotNull(lEpisodes.Result);
            Assert.IsTrue(lEpisodes.Result.Count() == await this._anime.EpisodeCount.GetObject(int.MinValue));
            Assert.IsTrue(lEpisodes.Result.All(episode => episode.Language == lAvailableLanguages.First()));
            Assert.IsTrue(lEpisodes.Result.All(episode => episode.ParentAnime == this._anime));
        }

        [Test, Order(2)]
        public async Task GetLatestCommentsTest()
        {
            Assert.IsNotNull(this._anime);

            ProxerResult<IEnumerable<Comment>> lLatestComments1 = await this._anime.GetCommentsLatest(0, 2);
            Assert.IsTrue(lLatestComments1.Success);
            Assert.IsNotNull(lLatestComments1.Result);
            Assert.IsNotEmpty(lLatestComments1.Result);
            Assert.IsTrue(lLatestComments1.Result.First().AnimeMangaId == this._anime.Id);

            await Task.Delay(2000);

            ProxerResult<IEnumerable<Comment>> lLatestComments2 = await this._anime.GetCommentsLatest(1, 2);
            Assert.IsTrue(lLatestComments2.Success);
            Assert.IsNotNull(lLatestComments2.Result);
            Assert.IsNotEmpty(lLatestComments2.Result);
            Assert.IsTrue(lLatestComments2.Result.First().AnimeMangaId == this._anime.Id);

            Assert.AreEqual(lLatestComments1.Result.Last().Author.Id, lLatestComments2.Result.First().Author.Id);
        }

        [Test, Order(2)]
        public async Task GetPopularAnimeTest()
        {
            ProxerResult<IEnumerable<Anime>> lPopularAnime = await Anime.GetPopularAnime(this._senpai);
            Assert.IsTrue(lPopularAnime.Success);
            Assert.IsNotNull(lPopularAnime.Result);
            Assert.IsNotEmpty(lPopularAnime.Result);
        }

        [Test, Order(2)]
        public async Task GetRatingCommentsTest()
        {
            Assert.IsNotNull(this._anime);

            ProxerResult<IEnumerable<Comment>> lRatingComments1 = await this._anime.GetCommentsRating(0, 2);
            Assert.IsTrue(lRatingComments1.Success);
            Assert.IsNotNull(lRatingComments1.Result);
            Assert.IsNotEmpty(lRatingComments1.Result);
            Assert.IsTrue(lRatingComments1.Result.First().AnimeMangaId == this._anime.Id);

            await Task.Delay(2000);

            ProxerResult<IEnumerable<Comment>> lRatingComments2 = await this._anime.GetCommentsRating(1, 2);
            Assert.IsTrue(lRatingComments2.Success);
            Assert.IsNotNull(lRatingComments2.Result);
            Assert.IsNotEmpty(lRatingComments2.Result);
            Assert.IsTrue(lRatingComments2.Result.First().AnimeMangaId == this._anime.Id);

            Assert.AreEqual(lRatingComments1.Result.Last().Author.Id, lRatingComments2.Result.First().Author.Id);
        }

        [Test, Order(2)]
        public async Task GroupsTest()
        {
            Assert.IsNotNull(this._anime);
            IEnumerable<Group> lGroups = await this._anime.Groups.GetObject(new[] {Group.Error});
            Assert.IsFalse(lGroups.Count() == 1 && lGroups.Contains(Group.Error));
        }

        [Test, Order(2)]
        public async Task IndustryTest()
        {
            Assert.IsNotNull(this._anime);
            IEnumerable<Industry> lIndustry = await this._anime.Industry.GetObject(new[] {Industry.Error});
            Assert.IsFalse(lIndustry.Count() == 1 && lIndustry.Contains(Industry.Error));
        }

        [Test, Order(2)]
        public async Task IsLicensedTest()
        {
            Assert.IsNotNull(this._anime);
            ProxerResult<bool> lIsLicensed = await this._anime.IsLicensed.GetObject();
            Assert.IsTrue(lIsLicensed.Success);
        }

        [Test, Order(2)]
        public async Task JapaneseTitleTest()
        {
            Assert.IsNotNull(this._anime);

            string lHexString = RandomUtility.GetRandomHexString();
            string lJapaneseTitle = await this._anime.JapaneseTitle.GetObject(lHexString);
            Assert.AreNotEqual(lJapaneseTitle, lHexString);
        }

        [Test, Order(2)]
        public async Task NameTest()
        {
            Assert.IsNotNull(this._anime);

            string lHexString = RandomUtility.GetRandomHexString();
            string lName = await this._anime.Name.GetObject(lHexString);
            Assert.AreNotEqual(lName, lHexString);
        }

        [Test, Order(2)]
        public void ObjectTypeTest()
        {
            Assert.IsNotNull(this._anime);
            Assert.AreEqual(this._anime.ObjectType, AnimeMangaType.Anime);
        }

        [Test, Order(2)]
        public async Task SeasonTest()
        {
            Assert.IsNotNull(this._anime);

            string lHexString = RandomUtility.GetRandomHexString();
            IEnumerable<string> lSeason = await this._anime.Season.GetObject(new[] {lHexString});
            Assert.IsFalse(lSeason.Count() == 1 && lSeason.Contains(lHexString));
        }

        [Test, Order(2)]
        public async Task StatusTest()
        {
            Assert.IsNotNull(this._anime);
            AnimeMangaStatus lStatus = await this._anime.Status.GetObject(AnimeMangaStatus.Unknown);
            Assert.AreNotEqual(lStatus, AnimeMangaStatus.Unknown);
        }

        [Test, Order(2)]
        public async Task SynonymTest()
        {
            Assert.IsNotNull(this._anime);

            string lHexString = RandomUtility.GetRandomHexString();
            string lSynonym = await this._anime.Synonym.GetObject(lHexString);
            Assert.AreNotEqual(lSynonym, lHexString);
        }
    }
}