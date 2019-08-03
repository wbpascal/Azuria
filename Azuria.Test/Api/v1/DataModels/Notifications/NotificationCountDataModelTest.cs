using Azuria.Api.v1.Converters.Notifications;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.ErrorHandling;
using Azuria.Test.Core;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.DataModels.Notifications
{
    public class NotificationCountDataModelTest : DataModelsTestBase<NotificationCountDataModel>
    {
        [Test]
        public void ConvertTest()
        {
            string lJson = ResponseSetup.FileResponses["notifications_getcount.json"];
            ProxerApiResponse<NotificationCountDataModel> lResponse = this.Convert(
                lJson, new NotificationCountConverter());
            Assert.AreEqual(BuildDataModel(), lResponse.Result);
        }

        private static NotificationCountDataModel BuildDataModel()
        {
            return new NotificationCountDataModel
            {
                FriendRequests = 1,
                News = 5,
                OtherMedia = 3,
                PrivateMessages = 2
            };
        }
    }
}