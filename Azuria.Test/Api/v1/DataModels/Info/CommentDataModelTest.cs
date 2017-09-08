using System;
using System.Collections.Generic;
using Azuria.Api;
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
            void CheckDataModel(
                CommentDataModel dataModel, int commentId, int entryId, int userId, MediaProgressState state,
                Dictionary<RatingCategory, int> subratings, string commentText, int rating, int contentIndex,
                int upvotes, long timestamp, string username, string avatar)
            {
                Assert.Multiple(
                    () =>
                    {
                        Assert.AreEqual(commentId, dataModel.CommentId);
                        Assert.AreEqual(entryId, dataModel.EntryId);
                        Assert.AreEqual(userId, dataModel.UserId);
                        Assert.AreEqual(state, dataModel.State);
                        Assert.AreEqual(commentText, dataModel.CommentText);
                        Assert.AreEqual(rating, dataModel.Rating);
                        Assert.AreEqual(contentIndex, dataModel.ContentIndex);
                        Assert.AreEqual(upvotes, dataModel.Upvotes);
                        Assert.AreEqual(DateTimeHelpers.UnixTimeStampToDateTime(timestamp), dataModel.TimeStamp);
                        Assert.AreEqual(username, dataModel.Username);
                        Assert.AreEqual(new Uri(ApiConstants.ProxerAvatarShortCdnUrl + avatar), dataModel.Avatar);

                        Assert.AreEqual(subratings.Count, dataModel.SubRatings.Count);
                        foreach (RatingCategory key in subratings.Keys)
                            Assert.AreEqual(subratings[key], dataModel.SubRatings[key]);
                    });
            }

            Assert.AreEqual(3, dataModels.Length);

            Dictionary<RatingCategory, int> lSubRatings = new Dictionary<RatingCategory, int>
            {
                {RatingCategory.Genre, 4},
                {RatingCategory.Story, 4},
                {RatingCategory.Animation, 5},
                {RatingCategory.Characters, 5},
                {RatingCategory.Music, 5}
            };
            CheckDataModel(
                dataModels[0], 11660523, 9200, 82932, MediaProgressState.Finished, lSubRatings, "comment text 1", 10,
                22, 7, 1440565134, "Username 1", "104302_2jXP5i.jpg");

            lSubRatings = new Dictionary<RatingCategory, int>
            {
                {RatingCategory.Genre, 5},
                {RatingCategory.Story, 5},
                {RatingCategory.Animation, 5},
                {RatingCategory.Characters, 4}
            };
            CheckDataModel(
                dataModels[1], 9223553, 9200, 192734, MediaProgressState.InProgress, lSubRatings, "comment text 2", 9,
                4, 1, 1424444000, "Username 2", "81017_55958x946de72.jpg");

            CheckDataModel(
                dataModels[2], 9091832, 9200, 628395, MediaProgressState.Planned,
                new Dictionary<RatingCategory, int>(), "comment text 3", 7, 21, 0, 1444404588, "Username 3",
                "360478_5KfIf7.jpg");
        }
    }
}