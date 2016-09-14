using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.ErrorHandling;

namespace Azuria.Notifications.PrivateMessage
{
    /// <summary>
    /// </summary>
    public sealed class PrivateMessageNotificationEnumerator : IEnumerator<PrivateMessageNotification>
    {
        private readonly Senpai _senpai;
        private PrivateMessageNotification[] _content;
        private int _currentContentIndex = -1;

        internal PrivateMessageNotificationEnumerator(Senpai senpai)
        {
            this._senpai = senpai;
        }

        #region Properties

        /// <inheritdoc />
        public PrivateMessageNotification Current => this._content[this._currentContentIndex];

        /// <inheritdoc />
        object IEnumerator.Current => this.Current;

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Dispose()
        {
            this._content = new PrivateMessageNotification[0];
        }


        /// <inheritdoc />
        internal async Task<ProxerResult<IEnumerable<PrivateMessageNotification>>> GetNextPage()
        {
            ProxerResult<ProxerApiResponse<MessageDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.MessengerGetMessages(this._senpai));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<PrivateMessageNotification>>(lResult.Exceptions);

            return
                new ProxerResult<IEnumerable<PrivateMessageNotification>>(
                (from notificationDataModel in lResult.Result.Data
                    select new PrivateMessageNotification(notificationDataModel, this._senpai)).Reverse());
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (this._content == null)
            {
                ProxerResult<IEnumerable<PrivateMessageNotification>> lGetSearchResult =
                    Task.Run(this.GetNextPage).Result;
                if (!lGetSearchResult.Success || (lGetSearchResult.Result == null))
                    throw lGetSearchResult.Exceptions.FirstOrDefault() ?? new Exception("Unkown error");
                this._content = lGetSearchResult.Result as PrivateMessageNotification[] ??
                                lGetSearchResult.Result.ToArray();
            }
            this._currentContentIndex++;
            return this._content.Length > this._currentContentIndex;
        }

        /// <inheritdoc />
        public void Reset()
        {
            this._content = new PrivateMessageNotification[0];
            this._currentContentIndex = -1;
        }

        #endregion
    }
}