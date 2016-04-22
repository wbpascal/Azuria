using JetBrains.Annotations;

namespace Azuria.Notifications
{
    /// <summary>
    ///     Eine Klasse, die die Informationen einer Benachrichtigung darstellt.
    /// </summary>
    public interface INotificationObject
    {
        #region Properties

        /// <summary>
        ///     Gibt die Nachricht der Benachrichtigung als Text zurück.
        ///     <para>(Vererbt von <see cref="INotificationObject" />)</para>
        /// </summary>
        [NotNull]
        string Message { get; }

        /// <summary>
        ///     Gibt den Typ der Benachrichtigung zurück.
        ///     <para>(Vererbt von <see cref="INotificationObject" />)</para>
        /// </summary>
        NotificationObjectType Type { get; }

        #endregion
    }
}