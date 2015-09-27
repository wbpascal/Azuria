using System.Threading.Tasks;

namespace Proxer.API.Notifications
{
    /// <summary>
    /// </summary>
    public interface INotificationCollection
    {
        #region Properties

        /// <summary>
        /// </summary>
        NotificationObjectType Type { get; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Task<INotificationObject[]> GetNotifications(int count);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task<INotificationObject[]> GetAllNotifications();

        #endregion
    }
}