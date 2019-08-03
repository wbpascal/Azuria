using System.Linq;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.Api.v1.Input.Notifications;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using NUnit.Framework;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    [TestFixture]
    public class NotificationsRequestBuilderTest : RequestBuilderTestBase<NotificationsRequestBuilder>
    {
        [Test]
        public void DeleteTest()
        {
            DeleteNotificationInput lInput = new DeleteNotificationInput {NotificationId = this.GetRandomNumber(10000)};

            IRequestBuilder lRequest = this.RequestBuilder.Delete(lInput);
            this.CheckUrl(lRequest, "notifications", "delete");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("nid"));
            Assert.AreEqual(lInput.NotificationId.ToString(), lRequest.PostParameter.GetValue("nid").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        public void GetCountTest()
        {
            IRequestBuilderWithResult<NotificationCountDataModel> lRequest = this.RequestBuilder.GetCount();
            this.CheckUrl(lRequest, "notifications", "count");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.NotNull(lRequest.CustomDataConverter);
            Assert.True(lRequest.CheckLogin);
        }

        [Test]
        [TestCase(2, 40, true)]
        [TestCase(0, 93, false)]
        public void GetNewsTest(int page, int limit, bool markRead)
        {
            NewsListInput lInput = new NewsListInput
            {
                Limit = limit,
                MarkRead = markRead,
                Page = page
            };
            IRequestBuilderWithResult<NewsNotificationDataModel[]> lRequest = this.RequestBuilder.GetNews(lInput);
            this.CheckUrl(lRequest, "notifications", "news");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.AreEqual(page.ToString(), lRequest.GetParameters["p"]);
            Assert.AreEqual(limit.ToString(), lRequest.GetParameters["limit"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}