using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Community;
using Azuria.ErrorHandling;
using Azuria.Utilities.Properties;

namespace Azuria.Notifications.Message
{
    /// <summary>
    /// Represents a private message notification.
    /// </summary>
    public class MessageNotification : INotification
    {
        private readonly int _conferenceId;
        private readonly InitialisableProperty<ConferenceInfo> _conferenceInfo;

        internal MessageNotification(int conferenceId, DateTime date, Senpai senpai)
        {
            this._conferenceInfo = new InitialisableProperty<ConferenceInfo>(this.InitConference);
            this._conferenceId = conferenceId;
            this.NotificationId = $"{conferenceId}_{date.ToFileTime().ToString().Replace("00", "")}";
            this.Senpai = senpai;
            this.TimeStamp = date;
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

        private async Task<IProxerResult> InitConference()
        {
            IProxerResult<IEnumerable<ConferenceInfo>> lConferencesResult =
                await Conference.GetConferences(this.Senpai);
            if (!lConferencesResult.Success || (lConferencesResult.Result == null))
                return new ProxerResult(lConferencesResult.Exceptions);

            this._conferenceInfo.Set(
                lConferencesResult.Result.FirstOrDefault(info => info.Conference.Id == this._conferenceId));
            return new ProxerResult();
        }

        #endregion
    }
}