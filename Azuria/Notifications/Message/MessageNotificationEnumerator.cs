using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Exceptions;

namespace Azuria.Notifications.Message
{
    /// <summary>
    /// </summary>
    internal sealed class MessageNotificationEnumerator : IEnumerator<MessageNotification>
    {
        private static readonly Regex NotificationInfoRegex = new Regex(
            "<a class=\"conferenceList\".*?href=\"\\/messages\\?id=(?<cid>[0-9].*?)#top.*?<\\/div>.*?<div>(?<date>.*?)<\\/div>",
            RegexOptions.ExplicitCapture);

        private readonly Senpai _senpai;
        private MessageNotification[] _content;
        private int _currentContentIndex = -1;

        internal MessageNotificationEnumerator(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Properties

        /// <inheritdoc />
        public MessageNotification Current => this._content[this._currentContentIndex];

        /// <inheritdoc />
        object IEnumerator.Current => this.Current;

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Dispose()
        {
            this._content = null;
        }


        /// <inheritdoc />
        internal async Task<IProxerResult<IEnumerable<MessageNotification>>> GetNextPage()
        {
            if (!this._senpai.IsProbablyLoggedIn)
                return new ProxerResult<IEnumerable<MessageNotification>>(new NotLoggedInException(this._senpai));

            IProxerResult<string> lResponse = await this._senpai.HttpClient.GetRequest(
                new Uri("https://proxer.me/messages?format=raw&s=notification"));
            if (!lResponse.Success || string.IsNullOrEmpty(lResponse.Result))
                return new ProxerResult<IEnumerable<MessageNotification>>(lResponse.Exceptions);

            List<MessageNotification> lNotifications = new List<MessageNotification>();
            MatchCollection lMatches = NotificationInfoRegex.Matches(lResponse.Result.Replace("\n", ""));

            foreach (Match lNotification in lMatches)
            {
                int lNotificationId = Convert.ToInt32(lNotification.Groups["cid"].Value);
                DateTime lDate = DateTime.ParseExact(lNotification.Groups["date"].Value, "dd.MM.yyyy",
                    CultureInfo.InvariantCulture);
                lNotifications.Add(new MessageNotification(lNotificationId, lDate, this._senpai));
            }

            return new ProxerResult<IEnumerable<MessageNotification>>(lNotifications);
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (this._content == null)
            {
                IProxerResult<IEnumerable<MessageNotification>> lGetSearchResult =
                    Task.Run(this.GetNextPage).Result;
                if (!lGetSearchResult.Success || (lGetSearchResult.Result == null))
                    throw lGetSearchResult.Exceptions.FirstOrDefault() ?? new Exception("Unkown error");
                this._content = lGetSearchResult.Result as MessageNotification[] ??
                                lGetSearchResult.Result.ToArray();
            }
            this._currentContentIndex++;
            return this._content.Length > this._currentContentIndex;
        }

        /// <inheritdoc />
        public void Reset()
        {
            this._content = new MessageNotification[0];
            this._currentContentIndex = -1;
        }

        #endregion
    }
}