using System.Collections.Generic;

namespace Azuria.Notifications
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INotificationEnumerator<out T> : IEnumerator<T> where T : INotification
    {
    }
}