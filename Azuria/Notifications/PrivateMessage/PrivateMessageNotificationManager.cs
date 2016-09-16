﻿using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.DataModels.Notifications;

namespace Azuria.Notifications.PrivateMessage
{
    /// <summary>
    /// </summary>
    public class PrivateMessageNotificationManager : INotificationManager
    {
        private readonly List<PrivateMessageNotificationEventHandler> _notificationEventHandlers =
            new List<PrivateMessageNotificationEventHandler>();

        private readonly Senpai _senpai;

        private PrivateMessageNotificationManager(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Properties

        Senpai INotificationManager.Senpai => this._senpai;

        #endregion

        #region Events

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ExceptionThrownNotificationFetchEventHandler(
            PrivateMessageNotificationManager sender, Exception e);

        /// <summary>
        ///     Represents a method that is executed when new private message notifications are available.
        /// </summary>
        /// <param name="sender">The user that recieved the notifications.</param>
        /// <param name="e">The notifications.</param>
        public delegate void PrivateMessageNotificationEventHandler(
            Senpai sender, IEnumerable<PrivateMessageNotification> e);

        /// <summary>
        /// </summary>
        public event PrivateMessageNotificationEventHandler NotificationRecieved
        {
            add
            {
                if (this._notificationEventHandlers.Contains(value)) return;
                this._notificationEventHandlers.Add(value);
                NotificationCountManager.CheckNotificationsForNewEvent().ConfigureAwait(false);
            }
            remove { this._notificationEventHandlers.Remove(value); }
        }

        /// <summary>
        /// </summary>
        public event ExceptionThrownNotificationFetchEventHandler ExceptionThrownNotificationFetch;

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static PrivateMessageNotificationManager Create(Senpai senpai)
        {
            return NotificationCountManager.GetOrAddManager(senpai, new PrivateMessageNotificationManager(senpai));
        }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnExceptionThrownNotificationFetch(Exception e)
        {
            this.ExceptionThrownNotificationFetch?.Invoke(this, e);
        }

        void INotificationManager.OnNewNotificationsAvailable(NotificationCountDataModel notificationsCounts)
        {
            if (notificationsCounts.PrivateMessages == 0) return;

            try
            {
                PrivateMessageNotification[] lPrivateMessageNotifications =
                    new PrivateMessageNotificationCollection(this._senpai).Take(notificationsCounts.PrivateMessages)
                        .ToArray();
                if (lPrivateMessageNotifications.Length > 0)
                    this.OnNotificationRecieved(this._senpai, lPrivateMessageNotifications);
            }
            catch (Exception ex)
            {
                this.OnExceptionThrownNotificationFetch(ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnNotificationRecieved(Senpai sender, IEnumerable<PrivateMessageNotification> e)
        {
            IEnumerable<PrivateMessageNotification> lNotifications = e as PrivateMessageNotification[] ?? e.ToArray();
            foreach (
                PrivateMessageNotificationEventHandler newsNotificationEventHandler in this._notificationEventHandlers)
                newsNotificationEventHandler?.Invoke(sender, lNotifications);
        }

        #endregion
    }
}