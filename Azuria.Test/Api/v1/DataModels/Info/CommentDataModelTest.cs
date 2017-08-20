using System;
using Azuria.Api.v1.DataModels;
using Azuria.Api.v1.DataModels.Info;
using Azuria.Enums.User;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using Xunit;

namespace Azuria.Test.Api.v1.DataModels.Info
{
    public class CommentDataModelTest : DataModelsTestBase<CommentDataModel>
    {
        [Fact]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["info_getcommentsrating.json"];
            ProxerApiResponse<CommentDataModel[]> lResponse = this.ConvertArray(lJson);
            this.CheckSuccessResponse(lResponse);
            CheckDataModels(lResponse.Result);
        }
        
        public static void CheckDataModels(CommentDataModel[] dataModels)
        {
            Assert.Equal(3, dataModels.Length);
            
            Assert.Equal(11660523, dataModels[0].CommentId);
            Assert.Equal(9223553, dataModels[1].CommentId);
            Assert.Equal(9091832, dataModels[2].CommentId);
            
            Assert.Equal(9200, dataModels[0].EntryId);
            Assert.Equal(9200, dataModels[1].EntryId);
            Assert.Equal(9200, dataModels[2].EntryId);
            
            Assert.Equal(82932, dataModels[0].UserId);
            Assert.Equal(192734, dataModels[1].UserId);
            Assert.Equal(628395, dataModels[2].UserId);
            
            Assert.Equal(MediaProgressState.Finished, dataModels[0].State);
            Assert.Equal(MediaProgressState.InProgress, dataModels[1].State);
            Assert.Equal(MediaProgressState.Planned, dataModels[2].State);
            
            Assert.NotNull(dataModels[0].SubRatings);
            Assert.Equal(5, dataModels[0].SubRatings.Count);
            Assert.True(dataModels[0].SubRatings.ContainsKey(RatingCategory.Genre));
            Assert.Equal(4, dataModels[0].SubRatings[RatingCategory.Genre]);
            Assert.True(dataModels[0].SubRatings.ContainsKey(RatingCategory.Story));
            Assert.Equal(4, dataModels[0].SubRatings[RatingCategory.Story]);
            Assert.True(dataModels[0].SubRatings.ContainsKey(RatingCategory.Animation));
            Assert.Equal(5, dataModels[0].SubRatings[RatingCategory.Animation]);
            Assert.True(dataModels[0].SubRatings.ContainsKey(RatingCategory.Characters));
            Assert.Equal(5, dataModels[0].SubRatings[RatingCategory.Characters]);
            Assert.True(dataModels[0].SubRatings.ContainsKey(RatingCategory.Music));
            Assert.Equal(5, dataModels[0].SubRatings[RatingCategory.Music]);
            
            Assert.NotNull(dataModels[1].SubRatings);
            Assert.Equal(4, dataModels[1].SubRatings.Count);
            Assert.True(dataModels[1].SubRatings.ContainsKey(RatingCategory.Genre));
            Assert.Equal(5, dataModels[1].SubRatings[RatingCategory.Genre]);
            Assert.True(dataModels[1].SubRatings.ContainsKey(RatingCategory.Story));
            Assert.Equal(5, dataModels[1].SubRatings[RatingCategory.Story]);
            Assert.True(dataModels[1].SubRatings.ContainsKey(RatingCategory.Animation));
            Assert.Equal(5, dataModels[1].SubRatings[RatingCategory.Animation]);
            Assert.True(dataModels[1].SubRatings.ContainsKey(RatingCategory.Characters));
            Assert.Equal(4, dataModels[1].SubRatings[RatingCategory.Characters]);
            
            Assert.NotNull(dataModels[2].SubRatings);
            Assert.Equal(0, dataModels[2].SubRatings.Count);
            
            Assert.Equal("comment text 1", dataModels[0].CommentText);
            Assert.Equal("comment text 2", dataModels[1].CommentText);
            Assert.Equal("comment text 3", dataModels[2].CommentText);
            
            Assert.Equal(10, dataModels[0].OverallRating);
            Assert.Equal(9, dataModels[1].OverallRating);
            Assert.Equal(7, dataModels[2].OverallRating);
            
            Assert.Equal(22, dataModels[0].ContentIndex);
            Assert.Equal(4, dataModels[1].ContentIndex);
            Assert.Equal(21, dataModels[2].ContentIndex);
            
            Assert.Equal(7, dataModels[0].Upvotes);
            Assert.Equal(1, dataModels[1].Upvotes);
            Assert.Equal(0, dataModels[2].Upvotes);
            
            Assert.Equal(DateTimeHelpers.UnixTimeStampToDateTime(1440565134), dataModels[0].TimeStamp);
            Assert.Equal(DateTimeHelpers.UnixTimeStampToDateTime(1424444000), dataModels[1].TimeStamp);
            Assert.Equal(DateTimeHelpers.UnixTimeStampToDateTime(1444404588), dataModels[2].TimeStamp);
            
            Assert.Equal("Username 1", dataModels[0].Username);
            Assert.Equal("Username 2", dataModels[1].Username);
            Assert.Equal("Username 3", dataModels[2].Username);
            
            Assert.Equal("104302_2jXP5i.jpg", dataModels[0].Avatar);
            Assert.Equal("81017_55958x946de72.jpg", dataModels[1].Avatar);
            Assert.Equal("360478_5KfIf7.jpg", dataModels[2].Avatar);
        }
    }
}