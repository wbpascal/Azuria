using System;
using System.Collections.Generic;
using System.Linq;
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
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        public static CommentDataModel BuildDataModel()
        {
            return new CommentDataModel
            {
                AvatarId = "104302_2jXP5i.jpg",
                CommentId = 11660523,
                CommentText = "comment text 1",
                Progress = 22,
                EntryId = 9200,
                Rating = 10,
                State = MediaProgressState.Finished,
                SubRatings = new Dictionary<RatingCategory, int>
                {
                    {RatingCategory.Genre, 4},
                    {RatingCategory.Story, 4},
                    {RatingCategory.Animation, 5},
                    {RatingCategory.Characters, 5},
                    {RatingCategory.Music, 5}
                },
                TimeStamp = DateTimeHelpers.UnixTimeStampToDateTime(1440565134),
                Type = "entry",
                Upvotes = 7,
                UserId = 82932,
                Username = "Username 1"
            };
        }
    }
}