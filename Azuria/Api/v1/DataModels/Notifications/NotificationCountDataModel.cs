namespace Azuria.Api.v1.DataModels.Notifications
{
    internal class NotificationCountDataModel : IDataModel
    {
        #region Properties

        internal int FriendRequests { get; set; }

        internal int News { get; set; }

        internal int OtherAnimeManga { get; set; }
        internal int PrivateMessages { get; set; }

        #endregion
    }
}