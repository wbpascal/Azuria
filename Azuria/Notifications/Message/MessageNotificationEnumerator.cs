using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.ErrorHandling;

namespace Azuria.Notifications.Message
{
    /// <summary>
    /// </summary>
    internal sealed class MessageNotificationEnumerator : IEnumerator<MessageNotification>
    {
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
            ProxerApiResponse<MessageDataModel[]> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.MessengerGetMessages(this._senpai, markAsRead: false));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<MessageNotification>>(lResult.Exceptions);

            return
                new ProxerResult<IEnumerable<MessageNotification>>(
                (from notificationDataModel in lResult.Result
                    select new MessageNotification(notificationDataModel, this._senpai)).Reverse());
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