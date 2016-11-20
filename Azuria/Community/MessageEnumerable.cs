﻿using System.Collections;
using System.Collections.Generic;

namespace Azuria.Community
{
    /// <summary>
    /// </summary>
    public class MessageEnumerable : IEnumerable<Message>
    {
        private readonly int _conferenceId;
        private readonly Senpai _senpai;

        internal MessageEnumerable(int conferenceId, Senpai senpai, bool markAsRead = true)
        {
            this._conferenceId = conferenceId;
            this._senpai = senpai;
            this.MarkAsRead = markAsRead;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public bool MarkAsRead { get; set; }

        #endregion

        #region Methods

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Message> GetEnumerator()
        {
            return new MessageEnumerator(this._conferenceId, this.MarkAsRead, this._senpai);
        }

        #endregion
    }
}