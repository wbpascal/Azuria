using Azuria.Enumerable;
using Azuria.Media;

namespace Azuria.UserInfo
{
    /// <summary>
    /// </summary>
    public class UserEntryEnumerable<T> : PagedEnumerable<UserProfileEntry<T>> where T : class, IMediaObject
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

        /// <inheritdoc />
        public override PagedEnumerator<UserProfileEntry<T>> GetEnumerator()
        {
            return new UserEntryEnumerator<T>(this._user, this.Senpai);
        }

        #endregion
    }
}