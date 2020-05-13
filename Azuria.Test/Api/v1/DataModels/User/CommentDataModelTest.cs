using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.DataModels.User;
using Azuria.Enums;
using Azuria.Enums.User;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.User
{
    public class CommentDataModelTest : DataModelsTestBase<CommentDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["user_getlatestcommentsmanga.json"];
            ProxerApiResponse<CommentDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(1, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static CommentDataModel BuildDataModel()
        {
            return new CommentDataModel
            {
                AvatarId = "163825_54623dbccb324.jpg",
                CommentId = 2255388,
                CommentText = "Comment Text 2255388",
                EntryId = 126,
                EntryMedium = MediaMedium.Mangaseries,
                EntryName = "Sora no Otoshimono",
                EntryType = MediaEntryType.Manga,
                Progress = 13,
                Rating = 10,
                State = MediaProgressState.Finished,
                SubRatings = new Dictionary<RatingCategory, int>
                {
                    {RatingCategory.Animation, 3},
                    {RatingCategory.Characters, 4},
                    {RatingCategory.Genre, 1},
                    {RatingCategory.Music, 5},
                    {RatingCategory.Story, 2}
                },
                TimeStamp = DateTimeHelpers.UnixTimeStampToDateTime(1488368929),
                Upvotes = 11,
                UserId = 163825,
                Username = "KutoSan"
            };
        }
    }
}