using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Community.ConferenceHelper;
using Azuria.Exceptions;
using Azuria.Utilities;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Net;
using Azuria.Utilities.Properties;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.Community
{
    /// <summary>
    ///     Represents a messaging Conference.
    /// </summary>
    public class Conference
    {
        /// <summary>
        ///     Represent a method, which is raised when an exception is thrown during the message fetching.
        /// </summary>
        /// <param name="sender">The conference that raised the event.</param>
        /// <param name="exceptions">The exceptions thrown.</param>
        public delegate void ErrorDuringPmFetchEventHandler(Conference sender, IEnumerable<Exception> exceptions);

        /// <summary>
        ///     Represents a method, which is raised when new messages were recieved or once everytime Active is set to true.
        /// </summary>
        /// <param name="sender">The conference that raised the event.</param>
        /// <param name="e">Contains the new messages. If <paramref name="completeFetch"/> is true it contains the last 15 messages of the conference.</param>
        /// <param name="completeFetch">Returns if every message of the conference(15 most recent) was fetched or only unread ones.</param>
        public delegate void NeuePmEventHandler(Conference sender, IEnumerable<Message> e, bool completeFetch);

        private readonly Timer _getMessagesTimer;
        private readonly Senpai _senpai;
        private IEnumerable<Message> _nachrichten;

        /// <summary>
        ///     Initialises a new instance of the Conference class.
        /// </summary>
        /// <param name="id">The if of the conference.</param>
        /// <param name="senpai">The user which is logged in and part of the conference.</param>
        public Conference(int id, [NotNull] Senpai senpai)
        {
            this.Id = id;
            this._senpai = senpai;

            this.IsBlocked = new SetableInitialisableProperty<bool>(this.GetConferenceOptions, this.SetBlock);
            this.IsFavourite = new SetableInitialisableProperty<bool>(this.GetConferenceOptions, this.SetFavourite);
            this.Leader = new InitialisableProperty<User>(this.GetLeader);
            this.Participants = new InitialisableProperty<IEnumerable<User>>(this.GetMainInfo);
            this.Title = new InitialisableProperty<string>(this.GetTitle);
            this.IsConference = new InitialisableProperty<bool>(this.CheckIsConference);

            this._getMessagesTimer = new Timer {Interval = new TimeSpan(0, 0, 15).TotalMilliseconds};
            this._getMessagesTimer.Elapsed += this.OnGetMessagesTimerElapsed;
        }

        internal Conference(string title, int id, Senpai senpai) : this(id, senpai)
        {
            this.Title = new InitialisableProperty<string>(this.GetTitle, title);
        }

        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the conference is currently fetching new messages.
        /// </summary>
        public bool Active
        {
            get { return this._getMessagesTimer.Enabled; }
            set
            {
                if (value)
                {
                    this.GetMessagesTimer(this._getMessagesTimer);
                }
                else
                {
                    this._getMessagesTimer.Stop();
                }
            }
        }

        /// <summary>
        ///     Gets the Id of the conference.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether the <see cref="Senpai.Me">User</see> blocks the current conference or not.
        /// </summary>
        [NotNull]
        public SetableInitialisableProperty<bool> IsBlocked { get; }

        [NotNull]
        private InitialisableProperty<bool> IsConference { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether the current conference is currently a favourite of the <see cref="Senpai.Me">User</see>.
        /// </summary>
        [NotNull]
        public SetableInitialisableProperty<bool> IsFavourite { get; }

        /// <summary>
        ///     Gets a value indicating whether the current object is fully initialised.
        /// </summary>
        public bool IsInitialized => this.IsFullyInitialised();

        /// <summary>
        ///     Gets a <see cref="User"/> that is the current leader of the conference.
        /// </summary>
        [NotNull]
        public InitialisableProperty<User> Leader { get; }

        /// <summary>
        ///     Gets all messages of the current conference in chronological order.
        /// </summary>
        [NotNull]
        public IEnumerable<Message> Messages
        {
            get { return this._nachrichten ?? new List<Message>(); }
            private set { this._nachrichten = value; }
        }

        /// <summary>
        ///     Gets all participants of the current conference.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<User>> Participants { get; }

        /// <summary>
        ///     Gets the current title of the current conference.
        /// </summary>
        [NotNull]
        public InitialisableProperty<string> Title { get; }

        #endregion

        #region

        [ItemNotNull]
        private async Task<ProxerResult> Block()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("http://proxer.me/messages?format=json&json=block&id=" + this.Id),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            return lResponse?.StartsWith("{\"error\":0") ?? false
                ? new ProxerResult()
                : new ProxerResult {Success = false};
        }

        [ItemNotNull]
        private async Task<ProxerResult> CheckIsConference()
        {
            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", "/ping"}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(
                        new Uri("https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer"),
                        lPostArgs,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;
            try
            {
                Dictionary<string, string> lDict =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                this.IsConference.SetInitialisedObject(lDict["msg"].Equals("Erfolgreich!") &&
                                                       !lDict["message"].Equals(
                                                           "Befehle sind nur in Konferenzen verfügbar."));
                return new ProxerResult();
            }
            catch
            {
                return
                    new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        ///     Occurs when one or multiple exceptions are thrown during message fetching.
        /// </summary>
        public event ErrorDuringPmFetchEventHandler ErrorDuringPmFetchRaised;

        [ItemNotNull]
        private async Task<ProxerResult> Favour()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("http://proxer.me/messages?format=json&json=favour&id=" + this.Id),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            return lResponse?.StartsWith("{\"error\":0") ?? false
                ? new ProxerResult()
                : new ProxerResult {Success = false};
        }

        [ItemNotNull]
        private async Task<ProxerResult> GetAllMessages()
        {
            if (this.Messages.ToList().Count > 0)
                return await this.GetMessages(this.Messages.Last().MessageId);

            if (this.Messages.ToList().Count != 0)
                return new ProxerResult
                {
                    Success = false
                };

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("http://proxer.me/messages?format=json&json=messages&id=" + this.Id),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
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
                ProxerResult<List<Message>> lResultMessages = await this.ProcessMessages(lResponse);
                if (!lResultMessages.Success)
                    return new ProxerResult(new Exception[] {new WrongResponseException(lResponse)});

                if (lResultMessages.Result != null) this.Messages = lResultMessages.Result;

                if (this.Messages.Any(
                    x => x.MessageAction == Message.Action.AddUser || x.MessageAction == Message.Action.RemoveUser))
                    await this.GetMainInfo();
                if (this.Messages.Any(x => x.MessageAction == Message.Action.SetLeader) &&
                    this.IsInitialized)
                    await this.GetLeader();
                if (this.Messages.Any(x => x.MessageAction == Message.Action.SetTopic) &&
                    this.IsInitialized)
                    await this.GetTitle();

                this.NeuePmRaised?.Invoke(this, this.Messages, true);
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            return new ProxerResult();
        }

        private async Task<ProxerResult> GetConferenceOptions()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("http://proxer.me/messages?format=json&json=messages&id=" + this.Id),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
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
                MessagesModel lConferenceJson = JsonConvert.DeserializeObject<MessagesModel>(lResponse);
                this.IsBlocked.SetInitialisedObject(lConferenceJson.Blocked == 1);
                this.IsFavourite.SetInitialisedObject(lConferenceJson.Favourite == 1);

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        [ItemNotNull]
        private async Task<ProxerResult> GetLeader()
        {
            if (!(await this.IsConference.GetObject()).OnError(false))
            {
                this.Leader.SetInitialisedObject(User.System);
                return new ProxerResult();
            }

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", "/leader"}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(
                        new Uri("https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer"),
                        lPostArgs,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                Dictionary<string, string> lDict =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                if (!lDict["msg"].Equals("Erfolgreich!"))
                    return new ProxerResult
                    {
                        Success = false
                    };

                this.Leader.SetInitialisedObject
                    ((await this.Participants.GetObject())
                        .OnError(new User[0])
                        .FirstOrDefault(
                            x =>
                                x.UserName.GetObjectIfInitialised("")
                                    .Equals(lDict["message"].Remove(0, "Konferenzleiter: ".Length))) ?? User.System);

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        [ItemNotNull]
        private async Task<ProxerResult> GetMainInfo()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("https://proxer.me/messages?id=" + this.Id + "&format=raw"),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            lDocument.LoadHtml(lResponse);
            try
            {
                List<User> lTeilnehmer = new List<User>();

                lTeilnehmer.AddRange(
                    from curTeilnehmer in lDocument.GetElementbyId("conferenceUsers").ChildNodes[1].ChildNodes
                    let lUserName = curTeilnehmer.ChildNodes[1].FirstChild.InnerText
                    let lUserId =
                        Convert.ToInt32(
                            curTeilnehmer.ChildNodes[1].FirstChild.Attributes["href"].Value.GetTagContents("/user/",
                                "#top")[0])
                    select new User(lUserName, lUserId, this._senpai));

                this.Participants.SetInitialisedObject(lTeilnehmer);

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        [ItemNotNull]
        private async Task<ProxerResult> GetMessages(int startMessageId)
        {
            if (this.Messages.Count(x => x.MessageId == startMessageId) == 0)
                return await this.GetAllMessages();

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("http://proxer.me/messages?format=json&json=newmessages&id=" + this.Id + "&mid=" +
                                startMessageId),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
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
                ProxerResult<List<Message>> lResultMessages = await this.ProcessMessages(lResponse);
                if (!lResultMessages.Success || lResultMessages.Result == null)
                    return new ProxerResult(new Exception[] {new WrongResponseException(lResponse)});

                List<Message> lNewMessages = lResultMessages.Result;

                if (
                    lNewMessages.Count(
                        x => x.MessageAction == Message.Action.AddUser || x.MessageAction == Message.Action.RemoveUser) >
                    0)
                    await this.GetMainInfo();
                if (lNewMessages.Count(x => x.MessageAction == Message.Action.SetLeader) > 0 &&
                    this.IsInitialized)
                    await this.GetLeader();
                if (lNewMessages.Count(x => x.MessageAction == Message.Action.SetTopic) > 0 &&
                    this.IsInitialized)
                    await this.GetTitle();

                this.Messages = this.Messages.Concat(lNewMessages).ToList();

                this.NeuePmRaised?.Invoke(this, lNewMessages, false);
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            return new ProxerResult();
        }

        private async void GetMessagesTimer([NotNull] Timer timer)
        {
            ProxerResult lResult;
            if (this.Messages.Any())
                lResult = await this.GetMessages(this.Messages.Last().MessageId);
            else lResult = await this.GetAllMessages();

            try
            {
                if (!lResult.Success)
                    this.ErrorDuringPmFetchRaised?.Invoke(this, lResult.Exceptions);
            }
            catch
            {
                //ignored
            }

            timer.Start();
        }

        [ItemNotNull]
        private async Task<ProxerResult> GetTitle()
        {
            if ((await this.IsConference.GetObject()).OnError(false))
            {
                Dictionary<string, string> lPostArgs = new Dictionary<string, string>
                {
                    {"message", "/topic"}
                };

                ProxerResult<string> lResult =
                    await
                        HttpUtility.PostResponseErrorHandling(
                            new Uri("https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer"),
                            lPostArgs,
                            this._senpai.LoginCookies,
                            this._senpai.ErrHandler,
                            this._senpai);

                if (!lResult.Success)
                    return new ProxerResult(lResult.Exceptions);

                string lResponse = lResult.Result;

                try
                {
                    Dictionary<string, string> lDict =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                    if (lDict["msg"].Equals("Erfolgreich!")) this.Title.SetInitialisedObject(lDict["message"]);

                    return new ProxerResult();
                }
                catch
                {
                    return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
                }
            }

            if ((await this.Participants.GetObject()).OnError(new User[0]).Count() > 1 && this._senpai.Me != null)
                this.Title.SetInitialisedObject(await (await this.Participants.GetObject()).OnError(new User[0])
                    .Where(x => x.Id != this._senpai.Me.Id)
                    .ToArray()[0].UserName.GetObject("ERROR"));

            return new ProxerResult();
        }


        /// <summary>
        ///     Initialises every property of the current class.
        /// </summary>
        /// <exception cref="NotLoggedInException">Thrown when the <see cref="Senpai">User</see> is not logged in.</exception>
        /// <exception cref="WrongResponseException">Thrown when the server response is not expected.</exception>
        /// <seealso cref="Senpai.Login" />
        [ItemNotNull]
        [Obsolete("Bitte benutze die Methoden der jeweiligen Eigenschaften, um sie zu initalisieren!")]
        public async Task<ProxerResult> Init()
        {
            return await this.InitAllInitalisableProperties();
        }

        /// <summary>
        ///     Occurs when new messages were recieved or once everytime Active is set to true.
        /// </summary>
        public event NeuePmEventHandler NeuePmRaised;

        private void OnGetMessagesTimerElapsed(object s, EventArgs eArgs)
        {
            Timer timer = s as Timer;
            timer?.Stop();
            if (timer != null) this.GetMessagesTimer(timer);
        }

        /// <summary>
        ///     Returns if the <paramref name="senpai">User</paramref> is part of the conference.
        /// </summary>
        /// <exception cref="NotLoggedInException">Thrown when the <see cref="Senpai">User</see> is not logged in.</exception>
        /// <exception cref="WrongResponseException">Thrown when the server response is not expected.</exception>
        /// <param name="id">The Id of the <see cref="Conference"/></param>
        /// <param name="senpai">The user which is tested.</param>
        /// <returns>Whether the <paramref name="senpai">User</paramref> is part of the conference.</returns>
        [ItemNotNull]
        public static async Task<ProxerResult<bool>> Participates(int id, [NotNull] Senpai senpai)
        {
            if (!senpai.IsLoggedIn) return new ProxerResult<bool>(new Exception[] {new NotLoggedInException(senpai)});

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", "/ping"}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(
                        new Uri("https://proxer.me/messages?id=" + id + "&format=json&json=answer"),
                        lPostArgs,
                        senpai.LoginCookies,
                        senpai.ErrHandler,
                        senpai);

            if (!lResult.Success)
                return new ProxerResult<bool>(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                Dictionary<string, string> lDict =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                return new ProxerResult<bool>(!lDict["msg"].Equals("Ein Fehler ist passiert."));
            }
            catch
            {
                return new ProxerResult<bool>((await ErrorHandler.HandleError(senpai, lResponse, false)).Exceptions);
            }
        }

        [ItemNotNull]
        private async Task<ProxerResult<List<Message>>> ProcessMessages([NotNull] string messages)
        {
            List<Message> lReturn = new List<Message>();

            try
            {
                MessagesModel lMessages = JsonConvert.DeserializeObject<MessagesModel>(messages);
                this.IsBlocked.SetInitialisedObject(lMessages.Blocked == 1);
                this.IsFavourite.SetInitialisedObject(lMessages.Favourite == 1);

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

                    User[] lSender =
                        (await this.Participants.GetObject()).OnError(new User[0])
                            .Where(x => x.Id == Convert.ToInt32(curMessage.Fromid))
                            .ToArray();

                    if (lSender.Any())
                        lReturn.Insert(0,
                            new Message(lSender[0], Convert.ToInt32(curMessage.Id), curMessage.Message,
                                Convert.ToInt32(curMessage.Timestamp), lMessageAction));
                    else
                        lReturn.Insert(0,
                            new Message(
                                new User(curMessage.Username, Convert.ToInt32(curMessage.Fromid),
                                    this._senpai), Convert.ToInt32(curMessage.Id), curMessage.Message,
                                Convert.ToInt32(curMessage.Timestamp), lMessageAction));
                }
            }
            catch
            {
                return
                    new ProxerResult<List<Message>>(
                        (await ErrorHandler.HandleError(this._senpai, messages, false)).Exceptions);
            }

            return new ProxerResult<List<Message>>(lReturn);
        }

        /// <summary>
        ///     Sends a message to the current conference.
        /// </summary>
        /// <exception cref="NotLoggedInException">Thrown when the <see cref="Senpai">User</see> is not logged in.</exception>
        /// <exception cref="WrongResponseException">Thrown when the server response is not expected.</exception>
        /// <param name="nachricht">The content of the message that is being send.</param>
        /// <returns>Whether the action was successfull.</returns>
        [ItemNotNull]
        public async Task<ProxerResult> SendMessage([NotNull] string nachricht)
        {
            if (string.IsNullOrEmpty(nachricht))
                return
                    new ProxerResult(new[]
                    {new ArgumentException("Argument is null or empty", nameof(nachricht))});

            this._getMessagesTimer.Stop();

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", nachricht}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(
                        new Uri("https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer"),
                        lPostArgs,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                Dictionary<string, string> lResponseJson =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (lResponseJson.Keys.Contains("message"))
                {
                    this.NeuePmRaised?.Invoke(this,
                        new List<Message>
                        {
                            new Message(User.System, -1, lResponseJson["message"], DateTime.Now,
                                Message.Action.GetAction)
                        }, false);
                    return new ProxerResult();
                }
                if (!lResponseJson.Keys.Contains("msg")) return new ProxerResult {Success = false};

                await this.GetMessages(this.Messages.Last().MessageId);
                this._getMessagesTimer.Start();
                return new ProxerResult();
            }
            catch
            {
                this._getMessagesTimer.Start();
                return
                    new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        private async Task<ProxerResult> SetBlock(bool isBlocked)
        {
            return isBlocked ? await this.Block() : await this.Unblock();
        }

        private async Task<ProxerResult> SetFavourite(bool isFavourite)
        {
            return isFavourite ? await this.Favour() : await this.Unfavour();
        }

        /// <summary>
        ///     Marks the current conference as unread.
        /// </summary>
        /// <exception cref="NotLoggedInException">Thrown when the <see cref="Senpai">User</see> is not logged in.</exception>
        /// <exception cref="WrongResponseException">Thrown when the server response is not expected.</exception>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Whether the action was successfull.</returns>
        [ItemNotNull]
        public async Task<ProxerResult> SetUnread()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("http://proxer.me/messages?format=json&json=setUnread&id=" + this.Id),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            return lResponse?.StartsWith("{\"error\":0") ?? false
                ? new ProxerResult()
                : new ProxerResult {Success = false};
        }

        [ItemNotNull]
        private async Task<ProxerResult> Unblock()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("http://proxer.me/messages?format=json&json=unblock&id=" + this.Id),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            return lResponse?.StartsWith("{\"error\":0") ?? false
                ? new ProxerResult()
                : new ProxerResult {Success = false};
        }

        [ItemNotNull]
        private async Task<ProxerResult> Unfavour()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("http://proxer.me/messages?format=json&json=unfavour&id=" + this.Id),
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            return lResponse?.StartsWith("{\"error\":0") ?? false
                ? new ProxerResult()
                : new ProxerResult {Success = false};
        }

        #endregion

        /// <summary>
        ///     Represents a single message of a <see cref="Conference"/>
        /// </summary>
        public class Message
        {
            /// <summary>
            ///     The action of the message.
            /// </summary>
            public enum Action
            {
                /// <summary>
                ///     Normal message, only text content.
                /// </summary>
                NoAction,

                /// <summary>
                ///     A user was added to the conference.
                /// </summary>
                AddUser,

                /// <summary>
                ///     A user was removed from the conference.
                /// </summary>
                RemoveUser,

                /// <summary>
                ///     The leader of the conference was changed.
                /// </summary>
                SetLeader,

                /// <summary>
                ///     The topic of the conference was changed.
                /// </summary>
                SetTopic,

                /// <summary>
                ///     A message was returned directly by the system. Happens most of the times if the user issued a command.
                /// </summary>
                GetAction
            }

            internal Message([NotNull] User sender, int mid, [NotNull] string nachricht, int unix, Action action)
                : this(sender, mid, nachricht, Utility.UnixTimeStampToDateTime(unix), action)
            {
            }

            internal Message([NotNull] User sender, int mid, [NotNull] string nachricht, DateTime date, Action action)
            {
                this.Sender = sender;
                this.MessageId = mid;
                this.Content = nachricht;
                this.TimeStamp = date;
                this.MessageAction = action;
            }

            #region Properties

            /// <summary>
            ///     Gets the message content.
            /// </summary>
            [NotNull]
            public string Content { get; private set; }

            /// <summary>
            ///     Gets the action of the message.
            /// </summary>
            public Action MessageAction { get; }

            /// <summary>
            ///     Gets the Id of the current message.
            /// </summary>
            public int MessageId { get; }

            /// <summary>
            ///     Gets the sender of the current message.
            /// </summary>
            [NotNull]
            public User Sender { get; private set; }

            /// <summary>
            ///     Gets the timestamp of the current message.
            /// </summary>
            public DateTime TimeStamp { get; private set; }

            #endregion
        }
    }
}