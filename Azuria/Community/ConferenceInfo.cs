using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.ErrorHandling;
using Azuria.Utilities.Properties;

namespace Azuria.Community
{
    /// <summary>
    /// </summary>
    public class ConferenceInfo
    {
        private readonly ArgumentInitialisableProperty<bool, IEnumerable<Message>> _unreadMessages;

        internal ConferenceInfo(ConferenceDataModel dataModel, Senpai senpai)
        {
            this.Senpai = senpai;
            this.Conference = new Conference(dataModel, senpai);
            this.UnreadMessagesCount = dataModel.UnreadMessagesCount;
            this._unreadMessages =
                new ArgumentInitialisableProperty<bool, IEnumerable<Message>>(
                    markAsRead => this.GetUnreadMessages(dataModel, markAsRead, senpai));
        }

        #region Properties

        /// <summary>
        /// </summary>
        public Conference Conference { get; }

        /// <summary>
        /// </summary>
        public Senpai Senpai { get; set; }

        /// <summary>
        /// </summary>
        public IArgumentInitialisableProperty<bool, IEnumerable<Message>> UnreadMessages => this._unreadMessages;

        /// <summary>
        /// 
        /// </summary>
        public int UnreadMessagesCount { get; }

        #endregion

        #region Methods

        private async Task<IProxerResult> GetUnreadMessages(ConferenceDataModel dataModel, bool markAsRead,
            Senpai senpai)
        {
            if (dataModel.UnreadMessagesCount == 0)
            {
                this._unreadMessages.Set(new Message[0]);
            }
            else
            {
                IEnumerable<Message> lUnreadMessages = await Task.Run(() =>
                    new MessageEnumerable(this.Conference, senpai, markAsRead)
                        .Take(dataModel.UnreadMessagesCount)).ConfigureAwait(false);
                this._unreadMessages.Set(lUnreadMessages);
            }
            return new ProxerResult();
        }

        #endregion
    }
}