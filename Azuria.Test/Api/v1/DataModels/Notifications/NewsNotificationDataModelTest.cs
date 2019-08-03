using System.Linq;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.ErrorHandling;
using Azuria.Helpers;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Notifications
{
    public class NewsNotificationDataModelTest : DataModelsTestBase<NewsNotificationDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["notifications_getnews.json"];
            ProxerApiResponse<NewsNotificationDataModel[]> lResponse = this.ConvertArray(lJson);
            Assert.AreEqual(3, lResponse.Result.Length);
            Assert.AreEqual(BuildDataModel(), lResponse.Result.First());
        }

        private static NewsNotificationDataModel BuildDataModel()
        {
            return new NewsNotificationDataModel
            {
                AuthorId = 155334,
                AuthorName = "Minato.",
                CategoryId = 56,
                CategoryName = "Anime- und Manga News",
                Description =
                    "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut " +
                    "labore et dolore magna aliquyam erat, sed diam voluptua.",
                Hits = 15,
                ImageId = "142259208567",
                ImageStyle = "background-position: 50% 34%;",
                NewsId = 6974,
                Posts = 1,
                Subject = "Hinmin, Seihitsu, Daifugou – neuer Manga des Jormungand-Autors",
                ThreadId = 380237,
                TimeStamp = DateTimeHelpers.UnixTimeStampToDateTime(1479931200)
            };
        }
    }
}