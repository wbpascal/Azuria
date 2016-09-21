using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Community;
using Azuria.ErrorHandling;
using Azuria.Utilities.Properties;

namespace Azuria.Notifications.PrivateMessage
{
    /// <summary>
    /// Represents a private message notification.
    /// </summary>
    public class PrivateMessageNotification : INotification
    {
        private readonly int _conferenceId;
        private readonly InitialisableProperty<ConferenceInfo> _conferenceInfo;

        internal PrivateMessageNotification(MessageDataModel dataModel, Senpai senpai)
        {
            this._conferenceInfo = new InitialisableProperty<ConferenceInfo>(this.InitConference);
            this._conferenceId = dataModel.ConferenceId;
            this.NotificationId = $"{dataModel.ConferenceId}_{dataModel.MessageId}";
            this.Senpai = senpai;
            this.TimeStamp = dataModel.MessageTimeStamp;
        }

        #region Properties

        /// <summary>
        /// Gets the conference the private message was recieved from.
        /// </summary>
        public IInitialisableProperty<ConferenceInfo> ConferenceInfo => this._conferenceInfo;

        /// <summary>
        /// Gets the id of the notification.
        /// </summary>
        public string NotificationId { get; }

        /// <inheritdoc />
        public Senpai Senpai { get; }

        /// <summary>
        /// Gets the date of the private message.
        /// </summary>
        public DateTime TimeStamp { get; }

        #endregion

        #region Methods

        private async Task<ProxerResult> InitConference()
        {
            ProxerResult<IEnumerable<ConferenceInfo>> lConferencesResult =
                await Conference.GetConferences(this.Senpai);
            if (!lConferencesResult.Success || (lConferencesResult.Result == null))
                return new ProxerResult(lConferencesResult.Exceptions);

            this._conferenceInfo.SetInitialisedObject(
                lConferencesResult.Result.FirstOrDefault(info => info.Conference.Id == this._conferenceId));
            return new ProxerResult();
        }

        #endregion
    }
}