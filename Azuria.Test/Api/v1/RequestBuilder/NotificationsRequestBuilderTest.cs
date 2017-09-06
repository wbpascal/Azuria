using System.Linq;
using Azuria.Api.v1.DataModels.Notifications;
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
            int lRandomId = this.GetRandomNumber(10000);

            IRequestBuilder lRequest = this.RequestBuilder.Delete(lRandomId);
            this.CheckUrl(lRequest, "notifications", "delete");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("nid"));
            Assert.AreEqual(lRandomId.ToString(), lRequest.PostParameter.GetValue("nid").First());
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
        public void GetNewsTest()
        {
            IRequestBuilderWithResult<NewsNotificationDataModel[]> lRequest = this.RequestBuilder.GetNews(2, 40);
            this.CheckUrl(lRequest, "notifications", "news");
            Assert.AreSame(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.AreEqual("2", lRequest.GetParameters["p"]);
            Assert.AreEqual("40", lRequest.GetParameters["limit"]);
            Assert.False(lRequest.CheckLogin);
        }

        [Test]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}