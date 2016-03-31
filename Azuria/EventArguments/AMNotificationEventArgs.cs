﻿using Azuria.Notifications;
using JetBrains.Annotations;

namespace Azuria.EventArguments
{
    /// <summary>
    ///     Stellt die Eventdaten für die Benachrichtigungen bereit.
    ///     <para>(Vererbt von <see cref="INotificationEventArgs" />)</para>
    /// </summary>
    /// <seealso cref="AmNotificationEventArgs" />
    /// <seealso cref="FriendNotificationEventArgs" />
    /// <seealso cref="INotificationEventArgs" />
    /// <seealso cref="NewsNotificationEventArgs" />
    /// <seealso cref="NotificationEventArgsType" />
    /// <seealso cref="PmNotificationEventArgs" />
    public class AmNotificationEventArgs : INotificationEventArgs
    {
        private readonly Senpai _senpai;

        internal AmNotificationEventArgs(int count, [NotNull] Senpai senpai)
        {
            this._senpai = senpai;
            this.Type = NotificationEventArgsType.AnimeManga;
            this.NotificationCount = count;
        }

        #region Geerbt

        /// <summary>
        ///     Gibt die Anzahl der Benachrichtigungen zurück.
        ///     <para>(Vererbt von <see cref="INotificationEventArgs" />)</para>
        /// </summary>
        public int NotificationCount { get; }

        /// <summary>
        ///     Gibt den Typ der Benachrichtigung zurück.
        ///     <para>(Vererbt von <see cref="INotificationEventArgs" />)</para>
        /// </summary>
        public NotificationEventArgsType Type { get; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gibt ein Objekt zurück, mithilfe dessen die Benachrichtigungen abgerufen werden können.
        /// </summary>
        [NotNull]
        public AnimeMangaUpdateCollection Notifications => this._senpai.AnimeMangaUpdates;

        #endregion
    }
}