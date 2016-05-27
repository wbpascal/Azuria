using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Main;
using Azuria.Main.User;
using Azuria.Main.User.Comment;
using Azuria.Main.User.ControlPanel;
using Azuria.Test.Attributes;
using Azuria.Test.Utility;
using Azuria.Utilities.ErrorHandling;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture, LoginRequired]
    public class UcpTest
    {
        private UserControlPanel _controlPanel;

        [Test, Order(1)]
        public async Task AnimeBookmarkTest()
        {
            this._controlPanel = new UserControlPanel(SenpaiTest.Senpai);

            ProxerResult<IEnumerable<AnimeMangaBookmarkObject<Anime>>> lFetchAnimeResult =
                await this._controlPanel.AnimeBookmarks.GetObject();
            Assert.IsTrue(lFetchAnimeResult.Success);
            Assert.IsNotNull(lFetchAnimeResult.Result);
            Assert.IsNotEmpty(lFetchAnimeResult.Result);
            Assert.IsTrue(
                lFetchAnimeResult.Result.All(
                    o =>
                        o.EntryId != -1 && o.AnimeMangaContentObject.ContentIndex != -1 &&
                        o.AnimeMangaContentObject.ParentObject.Id != -1));

            await Task.Delay(2000);
        }

        [Test, Order(1)]
        public async Task AnimeChronicTest()
        {
            this._controlPanel = new UserControlPanel(SenpaiTest.Senpai);

            ProxerResult<IEnumerable<AnimeMangaChronicObject<Anime>>> lFetchAnimeResult =
                await this._controlPanel.AnimeChronic.GetObject();
            Assert.IsTrue(lFetchAnimeResult.Success);
            Assert.IsNotNull(lFetchAnimeResult.Result);
            Assert.IsNotEmpty(lFetchAnimeResult.Result);
            Assert.IsTrue(
                lFetchAnimeResult.Result.All(
                    o =>
                        o.AnimeMangaContentObject.ContentIndex != -1 && o.AnimeMangaContentObject.ParentObject.Id != -1 &&
                        o.DateTime != DateTime.MinValue));

            await Task.Delay(2000);
        }

        [Test, Order(1)]
        public async Task AnimeFavouriteTest()
        {
            this._controlPanel = new UserControlPanel(SenpaiTest.Senpai);

            ProxerResult<IEnumerable<AnimeMangaFavouriteObject<Anime>>> lFetchAnimeResult =
                await this._controlPanel.AnimeFavourites.GetObject();
            Assert.IsTrue(lFetchAnimeResult.Success);
            Assert.IsNotNull(lFetchAnimeResult.Result);
            Assert.IsNotEmpty(lFetchAnimeResult.Result);
            Assert.IsTrue(lFetchAnimeResult.Result.All(o => o.EntryId != -1 && o.AnimeMangaObject.Id != -1));

            await Task.Delay(2000);
        }

        [Test, Order(1)]
        public async Task AnimeUcpObjectTest()
        {
            this._controlPanel = new UserControlPanel(SenpaiTest.Senpai);

            ProxerResult<IEnumerable<AnimeMangaUcpObject<Anime>>> lFetchAnimeResult =
                await this._controlPanel.Anime.GetObject();
            Assert.IsTrue(lFetchAnimeResult.Success);
            Assert.IsNotNull(lFetchAnimeResult.Result);
            Assert.IsNotEmpty(lFetchAnimeResult.Result);
            Assert.IsTrue(lFetchAnimeResult.Result.All(o => o.Progress.MaxProgress != -1));

            await Task.Delay(2000);
        }

        [Test, Order(2)]
        public async Task CommentTest()
        {
            ProxerResult<IEnumerable<AnimeMangaUcpObject<Anime>>> lFetchAnimeResult =
                await this._controlPanel.Anime.GetObject();
            Assert.IsNotNull(lFetchAnimeResult.Result);

            ProxerResult<EditableComment<Anime>> lFetchCommentResult =
                await lFetchAnimeResult.Result.First().Comment.GetObject();
            Assert.IsTrue(lFetchCommentResult.Success);
            Assert.IsNotNull(lFetchCommentResult.Result);
            EditableComment<Anime> lEditableComment = lFetchCommentResult.Result;

            string lCommentContent = lEditableComment.Content;
            AnimeMangaProgressState lCommentProgressState = lEditableComment.ProgressState;
            int lCommentRating = lEditableComment.Rating;
            Dictionary<RatingCategory, int> lCommentSubRatings = lEditableComment.SubRatings;

            lEditableComment.Content = RandomUtility.GetRandomHexString();
            lEditableComment.ProgressState = AnimeMangaProgressState.Aborted;
            lEditableComment.Rating = lEditableComment.Rating == 0 ? 1 : 0;
            lEditableComment.SubRatings = new Dictionary<RatingCategory, int> {{RatingCategory.Genre, 0}};

            ProxerResult lSaveResult = await lEditableComment.Save();
            Assert.IsTrue(lSaveResult.Success);

            await Task.Delay(2000);

            lFetchCommentResult = await lFetchAnimeResult.Result.First().Comment.GetNewObject();
            Assert.IsTrue(lFetchCommentResult.Success);
            Assert.IsNotNull(lFetchCommentResult.Result);
            lEditableComment = lFetchCommentResult.Result;

            Assert.AreNotEqual(lEditableComment.Content, lCommentContent);
            Assert.AreNotEqual(lEditableComment.ProgressState, lCommentProgressState);
            Assert.AreNotEqual(lEditableComment.Rating, lCommentRating);
            Assert.AreEqual(lEditableComment.SubRatings.Count, 1);
            Assert.IsTrue(lEditableComment.SubRatings.ContainsKey(RatingCategory.Genre));
            Assert.AreEqual(lEditableComment.SubRatings[RatingCategory.Genre], 0);

            lEditableComment.Content = lCommentContent;
            lEditableComment.ProgressState = lCommentProgressState;
            lEditableComment.Rating = lCommentRating;
            lEditableComment.SubRatings = lCommentSubRatings;

            lSaveResult = await lEditableComment.Save();
            Assert.IsTrue(lSaveResult.Success);
        }

        [Test, Order(2)]
        public async Task EditableProgressTest()
        {
            ProxerResult<IEnumerable<AnimeMangaUcpObject<Anime>>> lFetchAnimeResult =
                await this._controlPanel.Anime.GetObject();
            Assert.IsNotNull(lFetchAnimeResult.Result);
            Assert.IsNotEmpty(lFetchAnimeResult.Result);

            EditableAnimeMangaProgress lEditableProgress = lFetchAnimeResult.Result.First().Progress;
        }

        [Test, Order(1)]
        public async Task MangaBookmarkTest()
        {
            this._controlPanel = new UserControlPanel(SenpaiTest.Senpai);

            ProxerResult<IEnumerable<AnimeMangaBookmarkObject<Manga>>> lFetchMangaResult =
                await this._controlPanel.MangaBookmarks.GetObject();
            Assert.IsTrue(lFetchMangaResult.Success);
            Assert.IsNotNull(lFetchMangaResult.Result);
            Assert.IsNotEmpty(lFetchMangaResult.Result);
            Assert.IsTrue(
                lFetchMangaResult.Result.All(
                    o =>
                        o.EntryId != -1 && o.AnimeMangaContentObject.ContentIndex != -1 &&
                        o.AnimeMangaContentObject.ParentObject.Id != -1));

            await Task.Delay(2000);
        }

        [Test, Order(1)]
        public async Task MangaChronicTest()
        {
            this._controlPanel = new UserControlPanel(SenpaiTest.Senpai);

            ProxerResult<IEnumerable<AnimeMangaChronicObject<Manga>>> lFetchMangaResult =
                await this._controlPanel.MangaChronic.GetObject();
            Assert.IsTrue(lFetchMangaResult.Success);
            Assert.IsNotNull(lFetchMangaResult.Result);
            Assert.IsNotEmpty(lFetchMangaResult.Result);
            Assert.IsTrue(
                lFetchMangaResult.Result.All(
                    o =>
                        o.AnimeMangaContentObject.ContentIndex != -1 && o.AnimeMangaContentObject.ParentObject.Id != -1 &&
                        o.DateTime != DateTime.MinValue));

            await Task.Delay(2000);
        }

        [Test, Order(1)]
        public async Task MangaFavouriteTest()
        {
            this._controlPanel = new UserControlPanel(SenpaiTest.Senpai);

            ProxerResult<IEnumerable<AnimeMangaFavouriteObject<Manga>>> lFetchMangaResult =
                await this._controlPanel.MangaFavourites.GetObject();
            Assert.IsTrue(lFetchMangaResult.Success);
            Assert.IsNotNull(lFetchMangaResult.Result);
            Assert.IsNotEmpty(lFetchMangaResult.Result);
            Assert.IsTrue(lFetchMangaResult.Result.All(o => o.EntryId != -1 && o.AnimeMangaObject.Id != -1));

            await Task.Delay(2000);
        }

        [Test, Order(1)]
        public async Task MangaUcpObjectTest()
        {
            this._controlPanel = new UserControlPanel(SenpaiTest.Senpai);

            ProxerResult<IEnumerable<AnimeMangaUcpObject<Manga>>> lFetchMangaResult =
                await this._controlPanel.Manga.GetObject();
            Assert.IsTrue(lFetchMangaResult.Success);
            Assert.IsNotNull(lFetchMangaResult.Result);
            Assert.IsNotEmpty(lFetchMangaResult.Result);
            Assert.IsTrue(lFetchMangaResult.Result.All(o => o.Progress.MaxProgress != -1));

            await Task.Delay(2000);
        }
    }
}