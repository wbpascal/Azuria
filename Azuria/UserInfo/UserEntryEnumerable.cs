using System.Collections;
using System.Collections.Generic;
using Azuria.Media;

namespace Azuria.UserInfo
{
    /// <summary>
    /// </summary>
    public class UserEntryEnumerable<T> : IEnumerable<UserProfileEntry<T>> where T : class, IMediaObject
    {
        private readonly User _user;

        internal UserEntryEnumerable(User user)
        {
            this._user = user;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public Senpai Senpai { get; set; }

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
        public IEnumerator<UserProfileEntry<T>> GetEnumerator()
        {
            return new UserEntryEnumerator<T>(this._user, this.Senpai);
        }

        #endregion
    }
}