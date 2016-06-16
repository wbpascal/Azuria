using System.Collections.Generic;
using System.Linq;
using Azuria.Notifications.News;
using Azuria.Test.Attributes;
using NUnit.Framework;

namespace Azuria.Test
{
    [TestFixture, LoginRequired]
    public class NotificationTest
    {
        private readonly Senpai _senpai = SenpaiTest.Senpai;

        [Test]
        public void NewsNotificationTest()
        {
            List<NewsNotification> lNewsList = NewsNotificationManager.GetNotifications(this._senpai).Take(150).ToList();
            Assert.IsTrue(lNewsList.Count == 150);
            Assert.IsTrue(
                lNewsList.All(
                    notification =>
                        notification?.Author != null && notification.Author != User.User.System &&
                        notification.NewsId != default(int) && notification.ImageId != ""));
        }
    }
}