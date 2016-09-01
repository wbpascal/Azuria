using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.DataModels.Messenger;

namespace Azuria.Community
{
    /// <summary>
    /// </summary>
    public class ConferenceInfo
    {
        internal ConferenceInfo(ConferenceDataModel dataModel, Senpai senpai)
        {
            this.Conference = new Conference(dataModel, senpai);
            this.UnreadMessages = dataModel.UnreadMessagesCount > 0
                ? this.Conference.Messages.Take(dataModel.UnreadMessagesCount)
                : new Message[0];
        }

        #region Properties

        /// <summary>
        /// </summary>
        public Conference Conference { get; }

        /// <summary>
        /// </summary>
        public IEnumerable<Message> UnreadMessages { get; }

        #endregion
    }
}