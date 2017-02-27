using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Api.v1.RequestBuilder;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.UserInfo;
using Azuria.Utilities.Properties;

namespace Azuria.Community
{
    /// <summary>
    /// Represents a Conference.
    /// </summary>
    [DebuggerDisplay("Conference: {Topic} [{Id}]")]
    public class Conference
    {
        private static int _conferencesPerPage;
        private readonly Timer _checkMessagesTimer;
        private readonly InitialisableProperty<User> _leader;
        private readonly InitialisableProperty<IEnumerable<User>> _participants;
        private readonly InitialisableProperty<string> _topic;
        private Message _autoLastMessageRecieved;

        internal Conference(int conferenceId, bool isGroup, Senpai senpai)
        {
            this.Id = conferenceId;
            this.Senpai = senpai;

            this._checkMessagesTimer = new Timer {Interval = TimeSpan.FromSeconds(15).TotalMilliseconds};
            this._checkMessagesTimer.Elapsed += this.OnCheckMessagesTimerElapsed;

            this.IsGroupConference = isGroup;
            this._leader = new InitialisableProperty<User>(this.InitInfo);
            this._participants = new InitialisableProperty<IEnumerable<User>>(this.InitInfo);
            this._topic = new InitialisableProperty<string>(this.InitInfo);
        }

        internal Conference(ConferenceDataModel dataModel, Senpai senpai)
            : this(dataModel.ConferenceId, dataModel.IsConferenceGroup, senpai)
        {
            this._topic.Set(dataModel.ConferenceTitle);
        }

        #region Properties

        /// <summary>
        /// Gets or sets whether the conference is currently searching for new messages in the background and invoking the
        /// <see cref="NewMessageRecieved" /> event when new messages were found. In order for the backround search to work you
        /// need to have the permissions to access <see cref="Messages" />.
        /// </summary>
        public bool AutoCheck
        {
            get { return this._checkMessagesTimer.Enabled; }
            set
            {
                if (value) this._autoLastMessageRecieved = this.Messages.FirstOrDefault();
                this._checkMessagesTimer.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the interval in which new messages are searched for in the background if <see cref="AutoCheck" /> is set
        /// to true.
        /// </summary>
        public TimeSpan AutoCheckInterval
        {
            get { return TimeSpan.FromMilliseconds(this._checkMessagesTimer.Interval); }
            set { this._checkMessagesTimer.Interval = value.TotalMilliseconds; }
        }

        /// <summary>
        /// Gets the Id of the conference.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets whether the current instance is a group conference. Commands can only be used in group conferences.
        /// </summary>
        public bool IsGroupConference { get; }

        /// <summary>
        /// Gets whether all static variables of the class have already been initialised. Static variables can be initialised by
        /// calling <see cref="Init" />.
        /// </summary>
        public static bool IsInitialised { get; private set; }

        /// <summary>
        /// Gets a <see cref="User" /> that is the current leader of the conference.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        public IInitialisableProperty<User> Leader => this._leader;

        /// <summary>
        /// Gets the max amount of characters that can be send per message. Needs to be initialised by <see cref="Init" />.
        /// </summary>
        /// <seealso cref="Init" />
        public static int MaxCharactersPerMessage { get; private set; }

        /// <summary>
        /// Gets the max length of the topic in characters. Needs to be initialised by <see cref="Init" />.
        /// </summary>
        /// <seealso cref="Init" />
        public static int MaxCharactersTopic { get; private set; }

        /// <summary>
        /// Gets the max amount of users a group conference can hold. Needs to be initialised by <see cref="Init" />.
        /// </summary>
        /// <seealso cref="Init" />
        public static int MaxUsersPerConference { get; private set; }

        /// <summary>
        /// Gets all messages of the current conference ordered by newest first.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        public MessageEnumerable Messages => new MessageEnumerable(this, this.Senpai);

        internal static int MessagesPerPage { get; private set; }

        /// <summary>
        /// Gets an enumeration of all participants of the conference.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        public IInitialisableProperty<IEnumerable<User>> Participants => this._participants;

        /// <summary>
        /// Gets the user that created this instance and is passed when calling <see cref="SendMessage" />,
        /// <see cref="SendReport" />, <see cref="SetBlock" />, <see cref="SetFavourite" /> and <see cref="SetUnread" />.
        /// </summary>
        public Senpai Senpai { get; }

        /// <summary>
        /// Gets the current title of the conference.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        public IInitialisableProperty<string> Topic => this._topic;

        #endregion

        #region Events

        /// <summary>
        /// Represent a method which is invoked when an exception is thrown during the background search of new messages.
        /// </summary>
        /// <param name="sender">The conference that raised the event.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public delegate void ErrorThrownAutoMessageFetchEventHandler(Conference sender, Exception exception);

        /// <summary>
        /// Represents a method which is invoked when new messages were found or once everytime <see cref="AutoCheck" /> is set to
        /// true.
        /// </summary>
        /// <param name="sender">The conference that raised the event.</param>
        /// <param name="e">An enumeration of the new messages.</param>
        public delegate void NewMessageRecievedEventHandler(Conference sender, IEnumerable<Message> e);

        /// <summary>
        /// Raised when an exception is thrown during the background search of new messages.
        /// </summary>
        public event ErrorThrownAutoMessageFetchEventHandler ErrorThrownAutoMessageFetch;

        /// <summary>
        /// Raised when new messages were recieved during the background search or once every time <see cref="AutoCheck" /> is set
        /// to true.
        /// </summary>
        public event NewMessageRecievedEventHandler NewMessageRecieved;

        #endregion

        #region Methods

        private async void CheckForNewMessages()
        {
            if (this._autoLastMessageRecieved == null) return;

            Message[] lNewMessages = {};
            try
            {
                lNewMessages =
                    this.Messages.TakeWhile(message => message.MessageId != this._autoLastMessageRecieved.MessageId)
                        .ToArray();
            }
            catch (Exception ex)
            {
                this.ErrorThrownAutoMessageFetch?.Invoke(this, ex);
            }
            if (lNewMessages.Length == 0) return;
            if (lNewMessages.Any(message => message.Action != MessageAction.NoAction))
                await this.InitInfo().ConfigureAwait(false);
            this._autoLastMessageRecieved = lNewMessages[0];
            this.NewMessageRecieved?.Invoke(this, lNewMessages);
        }

        /// <summary>
        /// Creates or returns an existing conference with a specified user and sends a message to the conference.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// * User - Level 0
        /// </summary>
        /// <param name="user">The user that will recieve the message.</param>
        /// <param name="message">The message that will be send to the conference.</param>
        /// <param name="senpai">The user that sends the message. Must be logged in.</param>
        /// <returns>An asynchronous <see cref="Task" /> that returns an <see cref="IProxerResult" /> containing the conference.</returns>
        public static async Task<IProxerResult<Conference>> Create(User user, string message, Senpai senpai)
        {
            if (user == null) return new ProxerResult<Conference>(new ArgumentException(nameof(user)));

            IProxerResult<string> lUsernameResult = await user.UserName.Get().ConfigureAwait(false);
            if (!lUsernameResult.Success || string.IsNullOrEmpty(lUsernameResult.Result))
                return new ProxerResult<Conference>(lUsernameResult.Exceptions);

            return await Create(lUsernameResult.Result, message, senpai).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates or returns an existing conference with a specified user and sends a message to the conference.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="username">The username of the user that recieves the message.</param>
        /// <param name="message">The message that will be send to the conference.</param>
        /// <param name="senpai">The user that sends the message. Must be logged in.</param>
        /// <returns>An asynchronous <see cref="Task" /> that returns an <see cref="IProxerResult" /> containing the conference.</returns>
        public static async Task<IProxerResult<Conference>> Create(string username, string message, Senpai senpai)
        {
            if (!IsInitialised)
                return new ProxerResult<Conference>(new NotInitialisedException("Please call " + nameof(Init)));
            if (string.IsNullOrEmpty(username))
                return new ProxerResult<Conference>(new[] {new ArgumentException(nameof(username))});
            if (string.IsNullOrEmpty(message) || message.Length > MaxCharactersPerMessage)
                return new ProxerResult<Conference>(new ArgumentException(message));

            ProxerApiResponse<int> lResult = await RequestHandler.ApiRequest(
                MessengerRequestBuilder.NewConference(username, message, senpai)).ConfigureAwait(false);
            return !lResult.Success
                ? new ProxerResult<Conference>(lResult.Exceptions)
                : new ProxerResult<Conference>(new Conference(lResult.Result, false, senpai));
        }

        /// <summary>
        /// Creates a new group conference with 2 or more participants with a specified topic and an optional message that is send.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// * User - Level 0
        /// </summary>
        /// <param name="participants">
        /// An enumertaion of the participants. The user who creates the conference does not need to be
        /// included.
        /// </param>
        /// <param name="topic">The topic of the created conference.</param>
        /// <param name="senpai">
        /// The user that creates the conference and who will be the initial leader of the conference. Needs
        /// to be logged in.
        /// </param>
        /// <param name="message">Optional. A message that is send to the created conference. Default: null</param>
        /// <returns>An asynchronous <see cref="Task" /> that returns an <see cref="IProxerResult" /> containing the conference.</returns>
        public static async Task<IProxerResult<Conference>> CreateGroup(IEnumerable<User> participants,
            string topic, Senpai senpai, string message = null)
        {
            if (!IsInitialised)
                return new ProxerResult<Conference>(new NotInitialisedException("Please call " + nameof(Init)));
            if (participants == null)
                return new ProxerResult<Conference>(new ArgumentException(nameof(participants)));
            if (string.IsNullOrEmpty(topic) || topic.Length > MaxCharactersTopic)
                return new ProxerResult<Conference>(new ArgumentException(nameof(topic)));

            IEnumerable<User> lParticipants = participants as User[] ?? participants.ToArray();
            if (!lParticipants.Any() || lParticipants.Any(user => user == null || user == User.System))
                return new ProxerResult<Conference>(new[] {new ArgumentException(nameof(participants))});

            List<string> lParticipantNames = new List<string>();
            foreach (User participant in lParticipants)
            {
                IProxerResult<string> lUsernameResult = await participant.UserName.Get().ConfigureAwait(false);
                if (!lUsernameResult.Success || string.IsNullOrEmpty(lUsernameResult.Result))
                    return new ProxerResult<Conference>(lUsernameResult.Exceptions);

                lParticipantNames.Add(lUsernameResult.Result);
            }
            return await CreateGroup(lParticipantNames, topic, senpai, message).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new group conference with 2 or more participants with a specified topic and an optional message that is send.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="participants">
        /// An enumertaion of the usernames of the participants. The user who creates the conference
        /// does not need to be included.
        /// </param>
        /// <param name="topic">The topic of the created conference.</param>
        /// <param name="senpai">
        /// The user that creates the conference and who will be the initial leader of the conference. Needs
        /// to be logged in.
        /// </param>
        /// <param name="message">Optional. A message that is send to the created conference. Default: null</param>
        /// <returns>An asynchronous <see cref="Task" /> that returns an <see cref="IProxerResult" /> containing the conference.</returns>
        public static async Task<IProxerResult<Conference>> CreateGroup(IEnumerable<string> participants,
            string topic, Senpai senpai, string message = null)
        {
            if (!IsInitialised)
                return new ProxerResult<Conference>(new NotInitialisedException("Please call " + nameof(Init)));
            if (participants == null)
                return new ProxerResult<Conference>(new ArgumentException(nameof(participants)));
            if (string.IsNullOrEmpty(topic) || topic.Length > MaxCharactersTopic)
                return new ProxerResult<Conference>(new ArgumentException(nameof(topic)));

            IEnumerable<string> lParticipantNames = participants as string[] ?? participants.ToArray();
            if (!lParticipantNames.Any() || lParticipantNames.Any(username => string.IsNullOrEmpty(username.Trim())))
                return new ProxerResult<Conference>(new[] {new ArgumentException(nameof(participants))});

            ProxerApiResponse<int> lResult = await RequestHandler.ApiRequest(
                    MessengerRequestBuilder.NewConferenceGroup(lParticipantNames, topic, senpai, message))
                .ConfigureAwait(false);
            return !lResult.Success
                ? new ProxerResult<Conference>(lResult.Exceptions)
                : new ProxerResult<Conference>(new Conference(lResult.Result, true, senpai));
        }

        /// <summary>
        /// Gets all conferences a user is participant in.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="senpai">The user of which the conferences are returned. Must be logged in.</param>
        /// <param name="type">
        /// Optional. The type of conferences that will be returned. Default:
        /// <see cref="ConferenceListType.Default" />
        /// </param>
        /// <returns>
        /// An asynchronous <see cref="Task" /> that returns an <see cref="IProxerResult" /> containing an enumeration of
        /// conferences and all unread messages.
        /// </returns>
        public static async Task<IProxerResult<IEnumerable<ConferenceInfo>>> GetConferences(Senpai senpai,
            ConferenceListType type = ConferenceListType.Default)
        {
            if (!IsInitialised)
                return new ProxerResult<IEnumerable<ConferenceInfo>>(
                    new[] {new NotInitialisedException("Please call " + nameof(Init))});

            List<ConferenceInfo> lConferences = new List<ConferenceInfo>();
            for (int page = 0; page == 0 || lConferences.Count % _conferencesPerPage == 0; page++)
            {
                ProxerApiResponse<ConferenceDataModel[]> lResult = await RequestHandler.ApiRequest(
                        MessengerRequestBuilder.GetConferences(senpai, type, page))
                    .ConfigureAwait(false);
                if (!lResult.Success || lResult.Result == null)
                    return new ProxerResult<IEnumerable<ConferenceInfo>>(lResult.Exceptions);
                lConferences.AddRange(from conferenceDataModel in lResult.Result
                    select new ConferenceInfo(conferenceDataModel, senpai));
            }
            return new ProxerResult<IEnumerable<ConferenceInfo>>(lConferences);
        }

        /// <summary>
        /// Initialises all static variables of the class. Needs to be executed successfully before any other method is called.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <returns>An asynchronous <see cref="Task" /> that returns an <see cref="IProxerResult" />.</returns>
        public static async Task<IProxerResult> Init()
        {
            ProxerApiResponse<ConstantsDataModel> lResult = await RequestHandler.ApiRequest(
                MessengerRequestBuilder.GetConstants()).ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            ConstantsDataModel lData = lResult.Result;

            MaxCharactersPerMessage = lData.MaxCharactersPerMessage;
            MaxUsersPerConference = lData.MaxUsersPerConference;
            MaxCharactersTopic = lData.MaxCharactersTopic;
            MessagesPerPage = lData.MessagesPerPage;
            _conferencesPerPage = lData.ConferencesPerPage;
            IsInitialised = true;
            return new ProxerResult();
        }

        private async Task<IProxerResult> InitInfo()
        {
            ProxerApiResponse<ConferenceInfoDataModel> lResult = await RequestHandler.ApiRequest(
                    MessengerRequestBuilder.GetConferenceInfo(this.Id, this.Senpai))
                .ConfigureAwait(false);
            if (!lResult.Success || lResult.Result == null) return new ProxerResult(lResult.Exceptions);
            ConferenceInfoDataModel lData = lResult.Result;

            this._leader.Set(new User(lData.MainInfo.LeaderUserId));
            this._participants.Set(from conferenceInfoParticipantDataModel in lData.ParticipantsInfo
                select new User(conferenceInfoParticipantDataModel));
            this._topic.Set(lData.MainInfo.Title);

            return new ProxerResult();
        }

        private void OnCheckMessagesTimerElapsed(object s, EventArgs eArgs)
        {
            Timer timer = s as Timer;
            timer?.Stop();
            this.CheckForNewMessages();
            timer?.Start();
        }

        /// <summary>
        /// Sends a message to the conference.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="message">The message that is send to the conference.</param>
        /// <returns>An asynchronous <see cref="Task" /> that returns an <see cref="IProxerResult" />.</returns>
        public async Task<IProxerResult<string>> SendMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException(nameof(message));

            ProxerApiResponse<string> lResult = await RequestHandler.ApiRequest(
                    MessengerRequestBuilder.SetMessage(this.Id, message, this.Senpai))
                .ConfigureAwait(false);
            return !lResult.Success
                ? new ProxerResult<string>(lResult.Exceptions)
                : new ProxerResult<string>(lResult.Result ?? string.Empty);
        }

        /// <summary>
        /// Reports the conference to the admins with a specified reason.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="reason">The reason why the conference is being reported.</param>
        /// <returns>An asynchronous <see cref="Task" /> that returns an <see cref="IProxerResult" />.</returns>
        public async Task<IProxerResult> SendReport(string reason)
        {
            if (string.IsNullOrEmpty(reason)) return new ProxerResult(new ArgumentException(nameof(reason)));

            ProxerApiResponse lResult = await RequestHandler.ApiRequest(
                    MessengerRequestBuilder.SetReport(this.Id, reason, this.Senpai))
                .ConfigureAwait(false);
            return !lResult.Success
                ? new ProxerResult(lResult.Exceptions)
                : new ProxerResult();
        }

        /// <summary>
        /// Blocks or unblocks the conference.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 0
        /// </summary>
        /// <param name="isBlocked">Whether the conference should be blocked or unblocked.</param>
        /// <returns>An asynchronous <see cref="Task" /> that returns an <see cref="IProxerResult" />.</returns>
        public async Task<IProxerResult> SetBlock(bool isBlocked)
        {
            ProxerApiResponse lResult =
                await RequestHandler.ApiRequest(isBlocked
                        ? MessengerRequestBuilder.SetBlock(this.Id, this.Senpai)
                        : MessengerRequestBuilder.SetUnblock(this.Id, this.Senpai))
                    .ConfigureAwait(false);
            return !lResult.Success
                ? new ProxerResult(lResult.Exceptions)
                : new ProxerResult();
        }

        /// <summary>
        /// Favours or unfavours the conference.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <param name="isFavourite">Whether the conference should be blocked or unblocked.</param>
        /// <returns>An asynchronous <see cref="Task" /> that returns an <see cref="IProxerResult" />.</returns>
        public async Task<IProxerResult> SetFavourite(bool isFavourite)
        {
            ProxerApiResponse lResult =
                await RequestHandler.ApiRequest(isFavourite
                        ? MessengerRequestBuilder.SetFavour(this.Id, this.Senpai)
                        : MessengerRequestBuilder.SetUnfavour(this.Id, this.Senpai))
                    .ConfigureAwait(false);
            return !lResult.Success
                ? new ProxerResult(lResult.Exceptions)
                : new ProxerResult();
        }

        /// <summary>
        /// Marks the conference as unread.
        /// 
        /// Api permissions required:
        /// * Messenger - Level 1
        /// </summary>
        /// <returns>An asynchronous <see cref="Task" /> that returns an <see cref="IProxerResult" />.</returns>
        public async Task<IProxerResult> SetUnread()
        {
            ProxerApiResponse lResult = await RequestHandler.ApiRequest(
                    MessengerRequestBuilder.SetUnread(this.Id, this.Senpai))
                .ConfigureAwait(false);
            return !lResult.Success
                ? new ProxerResult(lResult.Exceptions)
                : new ProxerResult();
        }

        #endregion
    }
}