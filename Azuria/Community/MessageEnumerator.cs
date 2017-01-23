using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Api.v1.RequestBuilder;
using Azuria.Enumerable;
using Azuria.ErrorHandling;

namespace Azuria.Community
{
    /// <summary>
    /// </summary>
    public sealed class MessageEnumerator : PagedEnumerator<Message>
    {
        private readonly Conference _conference;
        private readonly bool _markAsRead;
        private readonly Senpai _senpai;

        internal MessageEnumerator(Conference conference, bool markAsRead, Senpai senpai)
            : base(Conference.MessagesPerPage)
        {
            this._conference = conference;
            this._senpai = senpai;
            this._markAsRead = markAsRead;
        }

        #region Methods

        internal override async Task<IProxerResult<IEnumerable<Message>>> GetNextPage(int nextPage)
        {
            Message lLastMessage = this.GetCurrentPage().LastOrDefault();
            ProxerApiResponse<MessageDataModel[]> lResult = await RequestHandler.ApiRequest(
                    MessengerRequestBuilder.GetMessages(this._senpai, this._conference.Id,
                        lLastMessage?.MessageId ?? 0, this._markAsRead))
                .ConfigureAwait(false);
            if (!lResult.Success || (lResult.Result == null))
                return new ProxerResult<IEnumerable<Message>>(lResult.Exceptions);

            return new ProxerResult<IEnumerable<Message>>((from messageDataModel in lResult.Result
                select new Message(messageDataModel, this._conference)).Reverse());
        }

        #endregion
    }
}