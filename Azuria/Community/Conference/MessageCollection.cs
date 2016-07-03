using System.Collections;
using System.Collections.Generic;

namespace Azuria.Community.Conference
{
    /// <summary>
    /// </summary>
    public class MessageCollection : IEnumerable<Message>
    {
        private readonly Conference _conference;
        private readonly Senpai _senpai;

        internal MessageCollection(Conference conference, Senpai senpai)
        {
            this._conference = conference;
            this._senpai = senpai;
        }

        #region Inherited

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Message> GetEnumerator()
        {
            return new MessageEnumerator(this._conference, this._senpai);
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}