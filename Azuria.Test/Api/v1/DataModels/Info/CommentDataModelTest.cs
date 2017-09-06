using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.User;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    [TestFixture]
    public class CommentDataModelTest : DataModelsTestBase<CommentDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getcommentsrating.json"];
            ProxerApiResponse<CommentDataModel[]> lResponse = this.ConvertArray(lJson);
            this.CheckSuccessResponse(lResponse);
            CheckDataModels(lResponse.Result);
        }

        private static void CheckDataModels(CommentDataModel[] dataModels)
        {
            Assert.AreEqual(3, dataModels.Length);

            Assert.AreEqual(11660523, dataModels[0].CommentId);
            Assert.AreEqual(9223553, dataModels[1].CommentId);
            Assert.AreEqual(9091832, dataModels[2].CommentId);

            Assert.AreEqual(9200, dataModels[0].EntryId);
            Assert.AreEqual(9200, dataModels[1].EntryId);
            Assert.AreEqual(9200, dataModels[2].EntryId);

            Assert.AreEqual(82932, dataModels[0].UserId);
            Assert.AreEqual(192734, dataModels[1].UserId);
            Assert.AreEqual(628395, dataModels[2].UserId);

            Assert.AreEqual(MediaProgressState.Finished, dataModels[0].State);
            Assert.AreEqual(MediaProgressState.InProgress, dataModels[1].State);
            Assert.AreEqual(MediaProgressState.Planned, dataModels[2].State);

            Assert.NotNull(dataModels[0].SubRatings);
            Assert.AreEqual(5, dataModels[0].SubRatings.Count);
            Assert.True(dataModels[0].SubRatings.ContainsKey(RatingCategory.Genre));
            Assert.AreEqual(4, dataModels[0].SubRatings[RatingCategory.Genre]);
            Assert.True(dataModels[0].SubRatings.ContainsKey(RatingCategory.Story));
            Assert.AreEqual(4, dataModels[0].SubRatings[RatingCategory.Story]);
            Assert.True(dataModels[0].SubRatings.ContainsKey(RatingCategory.Animation));
            Assert.AreEqual(5, dataModels[0].SubRatings[RatingCategory.Animation]);
            Assert.True(dataModels[0].SubRatings.ContainsKey(RatingCategory.Characters));
            Assert.AreEqual(5, dataModels[0].SubRatings[RatingCategory.Characters]);
            Assert.True(dataModels[0].SubRatings.ContainsKey(RatingCategory.Music));
            Assert.AreEqual(5, dataModels[0].SubRatings[RatingCategory.Music]);

            Assert.NotNull(dataModels[1].SubRatings);
            Assert.AreEqual(4, dataModels[1].SubRatings.Count);
            Assert.True(dataModels[1].SubRatings.ContainsKey(RatingCategory.Genre));
            Assert.AreEqual(5, dataModels[1].SubRatings[RatingCategory.Genre]);
            Assert.True(dataModels[1].SubRatings.ContainsKey(RatingCategory.Story));
            Assert.AreEqual(5, dataModels[1].SubRatings[RatingCategory.Story]);
            Assert.True(dataModels[1].SubRatings.ContainsKey(RatingCategory.Animation));
            Assert.AreEqual(5, dataModels[1].SubRatings[RatingCategory.Animation]);
            Assert.True(dataModels[1].SubRatings.ContainsKey(RatingCategory.Characters));
            Assert.AreEqual(4, dataModels[1].SubRatings[RatingCategory.Characters]);

            Assert.NotNull(dataModels[2].SubRatings);
            Assert.AreEqual(0, dataModels[2].SubRatings.Count);

            Assert.AreEqual("comment text 1", dataModels[0].CommentText);
            Assert.AreEqual("comment text 2", dataModels[1].CommentText);
            Assert.AreEqual("comment text 3", dataModels[2].CommentText);

            Assert.AreEqual(10, dataModels[0].OverallRating);
            Assert.AreEqual(9, dataModels[1].OverallRating);
            Assert.AreEqual(7, dataModels[2].OverallRating);

            Assert.AreEqual(22, dataModels[0].ContentIndex);
            Assert.AreEqual(4, dataModels[1].ContentIndex);
            Assert.AreEqual(21, dataModels[2].ContentIndex);

            Assert.AreEqual(7, dataModels[0].Upvotes);
            Assert.AreEqual(1, dataModels[1].Upvotes);
            Assert.AreEqual(0, dataModels[2].Upvotes);

            Assert.AreEqual(DateTimeHelpers.UnixTimeStampToDateTime(1440565134), dataModels[0].TimeStamp);
            Assert.AreEqual(DateTimeHelpers.UnixTimeStampToDateTime(1424444000), dataModels[1].TimeStamp);
            Assert.AreEqual(DateTimeHelpers.UnixTimeStampToDateTime(1444404588), dataModels[2].TimeStamp);

            Assert.AreEqual("Username 1", dataModels[0].Username);
            Assert.AreEqual("Username 2", dataModels[1].Username);
            Assert.AreEqual("Username 3", dataModels[2].Username);

            Assert.AreEqual("104302_2jXP5i.jpg", dataModels[0].Avatar);
            Assert.AreEqual("81017_55958x946de72.jpg", dataModels[1].Avatar);
            Assert.AreEqual("360478_5KfIf7.jpg", dataModels[2].Avatar);
        }
    }
}