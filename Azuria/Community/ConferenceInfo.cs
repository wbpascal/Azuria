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
            this.Conference = new Conference(dataModel, senpai);
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
        public IArgumentInitialisableProperty<bool, IEnumerable<Message>> UnreadMessages => this._unreadMessages;

        #endregion

        #region Methods

        private async Task<IProxerResult> GetUnreadMessages(ConferenceDataModel dataModel, bool markAsRead,
            Senpai senpai)
        {
            if (dataModel.UnreadMessagesCount == 0) this._unreadMessages.SetInitialisedObject(new Message[0]);
            else
            {
                IEnumerable<Message> lUnreadMessages = await Task.Run(() =>
                    new MessageEnumerable(dataModel.ConferenceId, senpai, markAsRead)
                        .Take(dataModel.UnreadMessagesCount));
                this._unreadMessages.SetInitialisedObject(lUnreadMessages);
            }
            return new ProxerResult();
        }

        #endregion
    }
}