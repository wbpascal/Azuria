using Azuria.Enumerable;
using Azuria.Exceptions;
using Azuria.Media;

namespace Azuria.UserInfo.Comment
{
    /// <summary>
    /// </summary>
    public class CommentEnumerable<T> : PagedEnumerable<Comment<T>> where T : class, IMediaObject
    {
        private readonly T _mediaObject;
        private readonly string _sort;
        private readonly User _user;

        internal CommentEnumerable(T mediaObject, string sort)
        {
            this._mediaObject = mediaObject;
            this._sort = sort;
        }

        internal CommentEnumerable(User user)
        {
            if (user == User.System) throw new InvalidUserException();

            this._user = user;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public Senpai Senpai { get; set; }

        #endregion

        #region Methods

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public override PagedEnumerator<Comment<T>> GetEnumerator()
        {
            return this._user == null
                ? new CommentEnumerator<T>(this._mediaObject, this._sort)
                : new CommentEnumerator<T>(this._user, this.Senpai);
        }

        #endregion
    }
}