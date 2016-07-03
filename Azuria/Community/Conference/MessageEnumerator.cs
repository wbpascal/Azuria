using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azuria.Exceptions;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Web;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.Community.Conference
{
    /// <summary>
    /// </summary>
    public class MessageEnumerator : IEnumerator<Message>
    {
        private readonly Conference _conference;
        private Message[] _currentPageContent = new Message[0];
        private int _currentPageIndex = -1;
        private int _nextPage;
        private Senpai _senpai;

        internal MessageEnumerator(Conference conference, Senpai senpai)
        {
            this._conference = conference;
            this._senpai = senpai;
        }

        #region Inherited

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._currentPageContent = null;
            this._senpai = null;
        }

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>
        ///     true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
        ///     end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            this._currentPageIndex++;
            if (this._currentPageIndex < this._currentPageContent.Length) return true;

            Task<ProxerResult> lGetNextPageTask = this.GetNextPage();
            lGetNextPageTask.Wait();
            if (!lGetNextPageTask.Result.Success)
                throw lGetNextPageTask.Result.Exceptions.FirstOrDefault() ?? new WrongResponseException();
            this._currentPageIndex = 0;
            return this._currentPageContent.Any();
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public void Reset()
        {
            this._currentPageIndex = -1;
            this._currentPageContent = new Message[0];
            this._nextPage = 0;
        }

        /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        public Message Current => this._currentPageContent[this._currentPageIndex];

        /// <summary>Gets the current element in the collection.</summary>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current => this.Current;

        #endregion

        #region

        [ItemNotNull]
        private async Task<ProxerResult> GetNextPage()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri(
                            $"http://proxer.me/messages?format=json&json=messages&id={this._conference.Id}&p={this._nextPage}"),
                        this._senpai.LoginCookies,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            if (lResponse == null || this._senpai.Me == null || lResponse.Equals("{\"uid\":\"" + this._senpai.Me.Id +
                                                                                 "\",\"error\":1,\"msg\":\"Ein Fehler ist passiert.\"}"))
                return new ProxerResult
                {
                    Success = false
                };
            try
            {
                ProxerResult<Message[]> lResultMessages = await this.ProcessMessages(lResponse);
                if (!lResultMessages.Success)
                    return new ProxerResult(new Exception[] {new WrongResponseException(lResponse)});

                if (lResultMessages.Result != null) this._currentPageContent = lResultMessages.Result;
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            this._nextPage++;
            return new ProxerResult();
        }

        [ItemNotNull]
        private async Task<ProxerResult<Message[]>> ProcessMessages([NotNull] string messages)
        {
            List<Message> lReturn = new List<Message>();

            try
            {
                MessagesModel lMessages = JsonConvert.DeserializeObject<MessagesModel>(messages);
                this._conference.IsBlocked.SetInitialisedObject(lMessages.Blocked == 1);
                this._conference.IsFavourite.SetInitialisedObject(lMessages.Favourite == 1);

                foreach (MessageModel curMessage in lMessages.MessageModels)
                {
                    Message.Action lMessageAction;

                    switch (curMessage.Action)
                    {
                        case "addUser":
                            lMessageAction = Message.Action.AddUser;
                            break;
                        case "removeUser":
                            lMessageAction = Message.Action.RemoveUser;
                            break;
                        case "setTopic":
                            lMessageAction = Message.Action.SetTopic;
                            break;
                        case "setLeader":
                            lMessageAction = Message.Action.SetLeader;
                            break;
                        default:
                            lMessageAction = Message.Action.NoAction;
                            break;
                    }

                    User.User lSender =
                        (await this._conference.Participants.GetObject()).OnError(new User.User[0])?
                            .FirstOrDefault(x => x.Id == curMessage.Fromid);

                    if (lSender != null)
                        lReturn.Add(
                            new Message(lSender, curMessage.Id, curMessage.Message,
                                Convert.ToInt32(curMessage.Timestamp), lMessageAction));
                    else
                        lReturn.Add(
                            new Message(
                                new User.User(curMessage.Username, curMessage.Fromid,
                                    this._senpai), curMessage.Id, curMessage.Message,
                                Convert.ToInt32(curMessage.Timestamp), lMessageAction));
                }
            }
            catch
            {
                return
                    new ProxerResult<Message[]>(
                        (await ErrorHandler.HandleError(this._senpai, messages, false)).Exceptions);
            }

            return new ProxerResult<Message[]>(lReturn.ToArray());
        }

        #endregion
    }
}