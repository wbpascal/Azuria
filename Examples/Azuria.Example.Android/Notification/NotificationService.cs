using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Azuria.AnimeManga;
using Azuria.Example.Android.Notification;
using Azuria.Example.Android.Utility.Extensions;
using Azuria.Notifications;
using Azuria.Notifications.AnimeManga;
using Azuria.Notifications.FriendRequest;
using Azuria.Notifications.News;
using Azuria.Notifications.PrivateMessage;
using InboxStyle = Android.Support.V4.App.NotificationCompat.InboxStyle;

namespace Azuria.Example.Android
{
    [Service]
    public class NotificationService : Service
    {
        private readonly List<INotification> _notifications = new List<INotification>();

        #region

        private void AnimeNotificationEventHandler(Senpai sender,
            IEnumerable<AnimeMangaNotification<Anime>> animeMangaNotifications)
        {
            this._notifications.AddIf(animeMangaNotifications,
                (list, notification) =>
                    list.All(
                        notification1 =>
                            !notification1.NotificationId.Equals((notification as INotification).NotificationId)));
            this.UpdateNotifications(sender);
        }

        private void FriendRequestNotificationEventHandler(Senpai sender,
            IEnumerable<FriendRequestNotification> friendRequestNotifications)
        {
            this._notifications.AddIf(friendRequestNotifications,
                (list, notification) =>
                    list.All(
                        notification1 => !notification1.NotificationId.Equals(notification.NotificationId)));
            this.UpdateNotifications(sender);
        }

        private void MangaNotificationEventHandler(Senpai sender,
            IEnumerable<AnimeMangaNotification<Manga>> animeMangaNotifications)
        {
            this._notifications.AddIf(animeMangaNotifications,
                (list, notification) =>
                    list.All(
                        notification1 =>
                            !notification1.NotificationId.Equals((notification as INotification).NotificationId)));
            this.UpdateNotifications(sender);
        }

        private void NewsNotificationEventHandler(Senpai sender, IEnumerable<NewsNotification> newsNotifications)
        {
            this._notifications.AddIf(newsNotifications,
                (list, notification) =>
                    list.All(
                        notification1 => !notification1.NotificationId.Equals(notification.NotificationId)));
            this.UpdateNotifications(sender);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            SenpaiParcelable lSenpaiParcelable = intent.GetParcelableExtra("SenpaiParcelable") as SenpaiParcelable;
            if (lSenpaiParcelable == null) throw new InvalidDataException();

            this.RegisterCallbacks(lSenpaiParcelable.Senpai);
            return StartCommandResult.RedeliverIntent;
        }

        private void PrivateMessageNotificationEventHandler(Senpai sender,
            IEnumerable<PrivateMessageNotification> privateMessageNotifications)
        {
            this._notifications.AddIf(privateMessageNotifications,
                (list, notification) =>
                    list.All(
                        notification1 => !notification1.NotificationId.Equals(notification.NotificationId)));
            this.UpdateNotifications(sender);
        }

        private void RegisterCallbacks(Senpai senpai)
        {
            AnimeMangaNotificationManager.RegisterAnimeNotificationCallback(senpai, this.AnimeNotificationEventHandler);
            AnimeMangaNotificationManager.RegisterMangaNotificationCallback(senpai, this.MangaNotificationEventHandler);
            FriendRequestNotificationManager.RegisterNotificationCallback(senpai,
                this.FriendRequestNotificationEventHandler);
            NewsNotificationManager.RegisterNotificationCallback(senpai, this.NewsNotificationEventHandler);
            PrivateMessageNotificationManager.RegisterNotificationCallback(senpai,
                this.PrivateMessageNotificationEventHandler);
        }

        public async void UpdateNotifications(Senpai senpai, IEnumerable<INotification> notifications = null)
        {
            notifications = notifications ?? this._notifications;
            InboxStyle lExtendedContent = new InboxStyle();
            foreach (INotification notification in notifications.Take(3))
            {
                if (notification is AnimeMangaNotification<Anime>)
                {
                    AnimeMangaNotification<Anime> lAnimeNotification = notification as AnimeMangaNotification<Anime>;
                    lExtendedContent.AddLine(
                        $"{await lAnimeNotification.ContentObject.ParentObject.Name.GetObject("ERROR")} #{lAnimeNotification.ContentObject.ContentIndex} now online!");
                }
                else if (notification is AnimeMangaNotification<Manga>)
                {
                    AnimeMangaNotification<Manga> lMangaNotification = notification as AnimeMangaNotification<Manga>;
                    lExtendedContent.AddLine(
                        $"{await lMangaNotification.ContentObject.ParentObject.Name.GetObject("ERROR")} #{lMangaNotification.ContentObject.ContentIndex} now online!");
                }
                else if (notification is FriendRequestNotification)
                {
                    FriendRequestNotification lFriendRequestNotification = notification as FriendRequestNotification;
                    lExtendedContent.AddLine(
                        $"{await lFriendRequestNotification.User.UserName.GetObject("ERROR")} (uid: {lFriendRequestNotification.User.Id}) wants to be friends!");
                }
                else if (notification is NewsNotification)
                {
                    lExtendedContent.AddLine((notification as NewsNotification).Subject);
                }
                else if (notification is PrivateMessageNotification)
                {
                    PrivateMessageNotification lFriendRequestNotification = notification as PrivateMessageNotification;
                    lExtendedContent.AddLine(
                        $"New messages in \"{await lFriendRequestNotification.Conference.Title.GetObject("ERROR")}\"");
                }
            }
            if (this._notifications.Count - 3 > 0)
                lExtendedContent.SetSummaryText($"+{this._notifications.Count - 3} more");

            NotificationCompat.Builder lNotificationBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetContentTitle(this._notifications.Count + " new notifications")
                .SetContentText(await senpai.Me.UserName.GetObject("ERROR"))
                .SetStyle(lExtendedContent);

            Intent lDismissIntent = new Intent(this, typeof(NotificationDismissBroadcastReciever));
            PendingIntent lPendingIntent = PendingIntent.GetBroadcast(this, 0, lDismissIntent, 0);
            lNotificationBuilder.SetDeleteIntent(lPendingIntent);

            NotificationManager notificationManager = (NotificationManager) this.GetSystemService(NotificationService);
            notificationManager.Notify(1000, lNotificationBuilder.Build());
        }

        #endregion
    }
}