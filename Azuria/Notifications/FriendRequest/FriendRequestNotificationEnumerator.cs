﻿using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;

namespace Azuria.Notifications.FriendRequest
{
    /// <summary>
    /// </summary>
    public sealed class FriendRequestNotificationEnumerator : INotificationEnumerator<FriendRequestNotification>
    {
        private readonly Senpai _senpai;
        private int _itemIndex = -1;
        private FriendRequestNotification[] _notifications = new FriendRequestNotification[0];

        internal FriendRequestNotificationEnumerator(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Properties

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public FriendRequestNotification Current => this._notifications[this._itemIndex];

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => this.Current;

        #endregion

        #region Methods

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._notifications = new FriendRequestNotification[0];
        }

        private Task<ProxerResult> GetNotifications()
        {
            throw new NotImplementedException();
        }

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>
        ///     true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
        ///     end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            this._itemIndex++;
            if (this._notifications.Any()) return this._itemIndex < this._notifications.Length;

            ProxerResult lGetNotificationsResult = Task.Run(this.GetNotifications).Result;
            if (!lGetNotificationsResult.Success)
                throw lGetNotificationsResult.Exceptions.FirstOrDefault() ?? new WrongResponseException();
            return this._notifications.Any();
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public void Reset()
        {
            this._itemIndex = -1;
            this._notifications = new FriendRequestNotification[0];
        }

        #endregion
    }
}