using System.Collections.Generic;
using System.Threading.Tasks;
using Azuria.Utilities;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Eine Klasse, die eine Sammlung von Benachrichtigungen darstellt.
    /// </summary>
    public interface INotificationCollection
    {
        #region Properties

        /// <summary>
        ///     Gibt den Typ der Benachrichtigung zurück.
        ///     <para>(Vererbt von <see cref="INotificationCollection" />)</para>
        /// </summary>
        NotificationObjectType Type { get; }

        #endregion

        #region

        /// <summary>
        ///     Gibt eine bestimmte Anzahl der aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        ///     <para>(Vererbt von <see cref="INotificationCollection" />)</para>
        /// </summary>
        /// <param name="count">Die Anzahl der Benachrichtigungen</param>
        /// <seealso cref="GetAllNotifications">GetAllNotifications Funktion</seealso>
        /// <returns>
        ///     Ein Array mit der Anzahl an Elementen in <paramref name="count" /> spezifiziert.
        ///     Wenn <paramref name="count" /> > Array.length, dann wird der gesamte Array zurückgegeben.
        /// </returns>
        Task<ProxerResult<IEnumerable<INotificationObject>>> GetNotifications(int count);

        /// <summary>
        ///     Gibt alle aktuellen Benachrichtigungen, die diese Klasse repräsentiert, zurück.
        ///     <para>(Vererbt von <see cref="INotificationCollection" />)</para>
        /// </summary>
        /// <seealso cref="GetNotifications" />
        /// <returns>Ein Array mit allen aktuellen Benachrichtigungen.</returns>
        Task<ProxerResult<IEnumerable<INotificationObject>>> GetAllNotifications();

        #endregion
    }
}