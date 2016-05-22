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
        public async Task AnimeUcpObjectTest()
        {
            this._controlPanel = new UserControlPanel(SenpaiTest.Senpai);

            ProxerResult<IEnumerable<AnimeMangaUcpObject<Anime>>> lFetchAnimeResult =
                await this._controlPanel.Anime.GetObject();
            Assert.IsTrue(lFetchAnimeResult.Success);
            Assert.IsNotNull(lFetchAnimeResult.Result);
            Assert.IsNotEmpty(lFetchAnimeResult.Result);
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
    }
}