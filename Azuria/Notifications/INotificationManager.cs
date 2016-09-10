using Azuria.Api.v1.DataModels.Notifications;

namespace Azuria.Notifications
{
    internal interface INotificationManager
    {
        #region Properties

        Senpai Senpai { get; }

        #endregion

        #region Methods

        void OnNewNotificationsAvailable(NotificationCountDataModel notificationsCounts);

        #endregion
    }
}