using Azuria.Enumerable;

namespace Azuria.Community
{
    /// <summary>
    /// </summary>
    public class MessageEnumerable : PagedEnumerable<Message>
    {
        private readonly Conference _conference;
        private readonly Senpai _senpai;

        internal MessageEnumerable(Conference conference, Senpai senpai, bool markAsRead = true)
        {
            this._conference = conference;
            this._senpai = senpai;
            this.MarkAsRead = markAsRead;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public bool MarkAsRead { get; set; }

        #endregion

        #region Methods

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public override PagedEnumerator<Message> GetEnumerator()
        {
            return new MessageEnumerator(this._conference, this.MarkAsRead, this._senpai);
        }

        #endregion
    }
}