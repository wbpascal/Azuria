using System;
using System.Linq;
using System.Threading;
using Azuria.Api.v1.DataModels.Notifications;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Requests.Builder;
using Azuria.Test.Core.Helpers;
using Xunit;

namespace Azuria.Test.Api.v1.RequestBuilder
{
    public class NotificationsRequestBuilderTest : RequestBuilderTestBase<NotificationsRequestBuilder>
    {
        [Fact]
        public void DeleteTest()
        {
            int lRandomId = this.GetRandomNumber(10000);

            IRequestBuilder lRequest = this.RequestBuilder.Delete(lRandomId);
            this.CheckUrl(lRequest, "notifications", "delete");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.PostParameter.ContainsKey("nid"));
            Assert.Equal(lRandomId.ToString(), lRequest.PostParameter.GetValue("nid").First());
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void GetCountTest()
        {
            IRequestBuilderWithResult<NotificationCountDataModel> lRequest = this.RequestBuilder.GetCount();
            this.CheckUrl(lRequest, "notifications", "count");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.NotNull(lRequest.CustomDataConverter);
            Assert.True(lRequest.CheckLogin);
        }

        [Fact]
        public void GetNewsTest()
        {
            IRequestBuilderWithResult<NewsNotificationDataModel[]> lRequest = this.RequestBuilder.GetNews(2, 40);
            this.CheckUrl(lRequest, "notifications", "news");
            Assert.Same(this.ProxerClient, lRequest.Client);
            Assert.True(lRequest.GetParameters.ContainsKey("p"));
            Assert.True(lRequest.GetParameters.ContainsKey("limit"));
            Assert.Equal("2", lRequest.GetParameters["p"]);
            Assert.Equal("40", lRequest.GetParameters["limit"]);
            Assert.False(lRequest.CheckLogin);
        }
        
        [Fact]
        public override void ProxerClientTest()
        {
            base.ProxerClientTest();
        }
    }
}