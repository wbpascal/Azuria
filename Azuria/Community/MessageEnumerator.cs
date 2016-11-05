using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.ErrorHandling;
using Azuria.Utilities;

namespace Azuria.Community
{
    /// <summary>
    /// </summary>
    public sealed class MessageEnumerator : PageEnumerator<Message>
    {
        private readonly int _conferenceId;
        private readonly Senpai _senpai;

        internal MessageEnumerator(int conferenceId, Senpai senpai)
            : base(Conference.MessagesPerPage)
        {
            this._conferenceId = conferenceId;
            this._senpai = senpai;
        }

        #region Methods

        internal override async Task<IProxerResult<IEnumerable<Message>>> GetNextPage(int nextPage)
        {
            Message lLastMessage = this.GetCurrentPage().LastOrDefault();
            ProxerApiResponse<MessageDataModel[]> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.MessengerGetMessages(this._senpai, this._conferenceId,
                    lLastMessage?.MessageId ?? 0));
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<Message>>(lResult.Exceptions);

            return new ProxerResult<IEnumerable<Message>>((from messageDataModel in lResult.Result
                select new Message(messageDataModel, this._conferenceId != 0
                    ? this._conferenceId
                    : messageDataModel.ConferenceId)).Reverse());
        }

        #endregion
    }
}