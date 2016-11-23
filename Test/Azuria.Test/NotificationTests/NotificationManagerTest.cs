using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Notifications;
using Azuria.Notifications.OtherMedia;
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
        public void OtherMediaNotificationTest()
        {
            var lNotifications = this._manager.OtherMediaNotifications.ToArray();
            Assert.AreEqual(2, lNotifications.Length);
            Assert.AreEqual(1, lNotifications.Count(notification =>
                    notification.NotificationType == OtherMediaType.Other));
            Assert.AreEqual(1, lNotifications.Count(notification =>
                    notification.NotificationType == OtherMediaType.Media));
        }

        public void NewsNotficationTest()
        {
            
        }
    }
}
