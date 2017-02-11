using System;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Notifications;
using Azuria.Notifications.Message;
using Azuria.Notifications.News;
using Azuria.Notifications.OtherMedia;
using Azuria.UserInfo;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Azuria.Test.NotificationTests
{
    [TestFixture]
    public class NotificationManagerTest
    {
        private NotificationManager _manager;

        [OneTimeSetUp]
        public void Setup()
        {
            this._manager = new NotificationManager(GeneralSetup.SenpaiInstance);
        }

        [Test]
        public async Task CountTest()
        {
            IProxerResult<NotificationCount> lResult = await this._manager.GetCount();
            Assert.IsTrue(lResult.Success, JsonConvert.SerializeObject(lResult.Exceptions));

            NotificationCount lCounts = lResult.Result;
            Assert.AreEqual(1, lCounts.FriendRequests);
            Assert.AreEqual(2, lCounts.Messages);
            Assert.AreEqual(5, lCounts.News);
            Assert.AreEqual(3, lCounts.OtherMedia);
        }

        [Test]
        public void MessageNotificationTest()
        {
            MessageNotification[] lNotifications = this._manager.MessageNotifications.ToArray();
            Assert.AreEqual(2, lNotifications.Length);
            Assert.IsTrue(lNotifications.All(notification => notification.Senpai == GeneralSetup.SenpaiInstance));
            Assert.IsTrue(lNotifications.All(notification => notification.TimeStamp != default(DateTime)));
            Assert.IsTrue(lNotifications.All(notification => !string.IsNullOrEmpty(notification.NotificationId.Trim())));
        }

        [Test]
        public void NewsNotficationTest()
        {
            NewsNotification[] lNotifications = this._manager.NewsNotifications.ToArray();
            Assert.AreEqual(3, lNotifications.Length);
            Assert.IsTrue(lNotifications.All(notification => notification.Senpai == GeneralSetup.SenpaiInstance));
            Assert.IsTrue(
                lNotifications.All(notification => notification.Author != null && notification.Author != User.System));
            Assert.IsTrue(lNotifications.All(notification => notification.CategoryId != default(int)));
            Assert.IsTrue(lNotifications.All(notification => !string.IsNullOrEmpty(notification.CategoryName)));
            Assert.IsTrue(lNotifications.All(notification => !string.IsNullOrEmpty(notification.Description)));
            Assert.IsTrue(lNotifications.All(notification => notification.Hits != default(int)));
            Assert.IsTrue(lNotifications.All(notification => notification.Image != null));
            Assert.IsTrue(lNotifications.All(notification => notification.ImageThumbnail != null));
            Assert.IsTrue(lNotifications.All(notification => notification.NewsId != default(int)));
            Assert.IsTrue(lNotifications.All(notification => notification.Posts != default(int)));
            Assert.IsTrue(lNotifications.All(notification => !string.IsNullOrEmpty(notification.NotificationId)));
            Assert.IsTrue(lNotifications.All(notification => !string.IsNullOrEmpty(notification.Subject)));
            Assert.IsTrue(lNotifications.All(notification => notification.TimeStamp != default(DateTime)));
        }

        [Test]
        public void OtherMediaNotificationTest()
        {
            OtherMediaNotification[] lNotifications = this._manager.OtherMediaNotifications.ToArray();
            Assert.AreEqual(2, lNotifications.Length);
            Assert.AreEqual(1, lNotifications.Count(notification =>
                notification.NotificationType == OtherMediaType.Other
                && !string.IsNullOrEmpty(notification.Message)));
            Assert.AreEqual(1, lNotifications.Count(notification =>
                notification.NotificationType == OtherMediaType.Media
                && notification.MediaNotification != null));
            Assert.IsTrue(lNotifications.All(notification => notification.Senpai == GeneralSetup.SenpaiInstance));
            Assert.IsTrue(lNotifications.All(notification => notification.NotificationId != default(int)));
        }
    }
}