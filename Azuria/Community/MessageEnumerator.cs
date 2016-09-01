using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Community
{
    /// <summary>
    /// </summary>
    public sealed class MessageEnumerator : PageEnumerator<Message>
    {
        private readonly int _conferenceId;
        [NotNull] private readonly Senpai _senpai;

        internal MessageEnumerator(int conferenceId, [NotNull] Senpai senpai)
            : base(Conference.MessagesPerPage)
        {
            this._conferenceId = conferenceId;
            this._senpai = senpai;
        }

        #region

        internal override async Task<ProxerResult<IEnumerable<Message>>> GetNextPage(int nextPage)
        {
            Message lLastMessage = this.GetCurrentPage().LastOrDefault();
            ProxerResult<ProxerApiResponse<MessageDataModel[]>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.MessengerGetMessages(this._senpai, this._conferenceId,
                        lLastMessage?.MessageId ?? 0));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<Message>>(lResult.Exceptions);

            return new ProxerResult<IEnumerable<Message>>((from messageDataModel in lResult.Result.Data
                select
                new Message(messageDataModel,
                    this._conferenceId != 0 ? this._conferenceId : messageDataModel.ConferenceId)).Reverse());
        }

        #endregion
    }
}