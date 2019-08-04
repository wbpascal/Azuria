using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.DataModels.User;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.Enums.User;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.User
{
    public class ListDataModelTest : DataModelsTestBase<ListDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["user_getlistmanga.json"];
            ProxerApiResponse<ListDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(4, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static ListDataModel BuildDataModel()
        {
            return new ListDataModel
            {
                CommentContent =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.",
                CommentContentIndex = 105,
                CommentId = 7215389,
                CommentLastChanged = DateTimeHelpers.UnixTimeStampToDateTime(1416410114),
                CommentState = MediaProgressState.Finished,
                CommentSubRatings = new Dictionary<RatingCategory, int>
                {
                    {RatingCategory.Animation, 1},
                    {RatingCategory.Characters, 3},
                    {RatingCategory.Genre, 4},
                    {RatingCategory.Music, 2},
                    {RatingCategory.Story, 5}
                },
                ContentCount = 105,
                EntryId = 7014,
                EntryMedium = MediaMedium.Mangaseries,
                EntryName = "Flow",
                EntryStatus = MediaStatus.Completed,
                Rating = 7
            };
        }
    }
}