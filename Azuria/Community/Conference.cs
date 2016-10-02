using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.UserInfo;
using Azuria.Utilities.Properties;

namespace Azuria.Community
{
    /// <summary>
    /// Represents a messaging Conference.
    /// </summary>
    [DebuggerDisplay("Conference: {Topic} [{Id}]")]
    public class Conference
    {
        private static int _conferencesPerPage;
        private readonly Timer _checkMessagesTimer;
        private readonly InitialisableProperty<User> _leader;
        private readonly InitialisableProperty<IEnumerable<User>> _participants;
        private readonly Senpai _senpai;
        private readonly InitialisableProperty<string> _topic;
        private Message _autoLastMessageRecieved;

        internal Conference(int conferenceId, bool isGroup, Senpai senpai)
        {
            this.Id = conferenceId;
            this._senpai = senpai;

            this._checkMessagesTimer = new Timer {Interval = AutoCheckInterval.TotalMilliseconds};
            this._checkMessagesTimer.Elapsed += this.OnCheckMessagesTimerElapsed;

            this.IsGroupConference = isGroup;
            this._leader = new InitialisableProperty<User>(this.InitInfo);
            this._participants = new InitialisableProperty<IEnumerable<User>>(this.InitInfo);
            this._topic = new InitialisableProperty<string>(this.InitInfo);
        }

        internal Conference(ConferenceDataModel dataModel, Senpai senpai)
            : this(dataModel.ConferenceId, dataModel.IsConferenceGroup, senpai)
        {
            this._topic.SetInitialisedObject(dataModel.ConferenceTitle);
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the conference is currently fetching new messages.
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
        /// </summary>
        public static TimeSpan AutoCheckInterval { get; set; } = TimeSpan.FromSeconds(15);

        /// <summary>
        /// Gets the Id of the conference.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// </summary>
        public bool IsGroupConference { get; }

        /// <summary>
        /// </summary>
        public static bool IsInitialised { get; private set; }

        /// <summary>
        /// Gets a <see cref="User" /> that is the current leader of the conference.
        /// </summary>
        public IInitialisableProperty<User> Leader => this._leader;

        /// <summary>
        /// </summary>
        /// <seealso cref="Init" />
        public static int MaxCharactersPerMessage { get; private set; }

        /// <summary>
        /// </summary>
        /// <seealso cref="Init" />
        public static int MaxCharactersTopic { get; private set; }

        /// <summary>
        /// </summary>
        /// <seealso cref="Init" />
        public static int MaxUsersPerConference { get; private set; }

        /// <summary>
        /// Gets all messages of the current conference ordered by newest first.
        /// </summary>
        public IEnumerable<Message> Messages => new MessageCollection(this.Id, this._senpai);

        internal static int MessagesPerPage { get; private set; }

        /// <summary>
        /// Gets all participants of the current conference.
        /// </summary>
        public IInitialisableProperty<IEnumerable<User>> Participants => this._participants;

        /// <summary>
        /// Gets the current title of the current conference.
        /// </summary>
        public IInitialisableProperty<string> Topic => this._topic;

        #endregion

        #region Events

        /// <summary>
        /// Represent a method, which is raised when an exception is thrown during the message fetching.
        /// </summary>
        /// <param name="sender">The conference that raised the event.</param>
        /// <param name="exception">The exception thrown.</param>
        public delegate void ErrorThrownAutoMessageFetchEventHandler(Conference sender, Exception exception);

        /// <summary>
        /// Represents a method, which is raised when new messages were recieved or once everytime Active is set to true.
        /// </summary>
        /// <param name="sender">The conference that raised the event.</param>
        /// <param name="e">
        /// Contains the new messages.
        /// </param>
        public delegate void NewMessageRecievedEventHandler(Conference sender, IEnumerable<Message> e);

        /// <summary>
        /// </summary>
        public event ErrorThrownAutoMessageFetchEventHandler ErrorThrownAutoMessageFetch;

        /// <summary>
        /// Occurs when new messages were recieved or once everytime Active is set to true.
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
            if (lNewMessages.Any(message => message.Action != MessageAction.NoAction)) await this.InitInfo();
            this._autoLastMessageRecieved = lNewMessages[0];
            this.NewMessageRecieved?.Invoke(this, lNewMessages);
        }

        /// <summary>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static async Task<ProxerResult<Conference>> Create(User user, string message, Senpai senpai)
        {
            if (!IsInitialised)
                return new ProxerResult<Conference>(new NotInitialisedException("Please call " + nameof(Init)));
            if (user == null) return new ProxerResult<Conference>(new ArgumentException(nameof(user)));
            if (string.IsNullOrEmpty(message) || (message.Length > MaxCharactersPerMessage))
                return new ProxerResult<Conference>(new ArgumentException(message));

            ProxerResult<string> lUsernameResult = await user.UserName.GetObject();
            if (!lUsernameResult.Success || string.IsNullOrEmpty(lUsernameResult.Result))
                return new ProxerResult<Conference>(lUsernameResult.Exceptions);

            return await Create(lUsernameResult.Result, message, senpai);
        }

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static async Task<ProxerResult<Conference>> Create(string username, string message, Senpai senpai)
        {
            if (!IsInitialised)
                return new ProxerResult<Conference>(new NotInitialisedException("Please call " + nameof(Init)));
            if (string.IsNullOrEmpty(username))
                return new ProxerResult<Conference>(new[] {new ArgumentException(nameof(username))});
            if (string.IsNullOrEmpty(message) || (message.Length > MaxCharactersPerMessage))
                return new ProxerResult<Conference>(new ArgumentException(message));

            ProxerResult<ProxerApiResponse<int>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.MessengerNewConference(username, message, senpai));
            return !lResult.Success || (lResult.Result == null)
                ? new ProxerResult<Conference>(lResult.Exceptions)
                : new ProxerResult<Conference>(new Conference(lResult.Result.Data, false, senpai));
        }

        /// <summary>
        /// </summary>
        /// <param name="participants"></param>
        /// <param name="topic"></param>
        /// <param name="senpai"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<ProxerResult<Conference>> CreateGroup(IEnumerable<User> participants,
            string topic, Senpai senpai, string message = null)
        {
            if (!IsInitialised)
                return new ProxerResult<Conference>(new NotInitialisedException("Please call " + nameof(Init)));
            if (string.IsNullOrEmpty(topic) || (topic.Length > MaxCharactersTopic))
                return new ProxerResult<Conference>(new ArgumentException(nameof(topic)));

            IEnumerable<User> lParticipants = participants as User[] ?? participants.ToArray();
            if (!lParticipants.Any() || lParticipants.Any(user => (user == null) || (user == User.System)))
                return new ProxerResult<Conference>(new[] {new ArgumentException(nameof(participants))});

            List<string> lParticipantNames = new List<string>();
            foreach (User participant in lParticipants)
            {
                ProxerResult<string> lUsernameResult = await participant.UserName;
                if (!lUsernameResult.Success || string.IsNullOrEmpty(lUsernameResult.Result))
                    return new ProxerResult<Conference>(lUsernameResult.Exceptions);

                lParticipantNames.Add(lUsernameResult.Result);
            }
            return await CreateGroup(lParticipantNames, topic, senpai, message);
        }

        /// <summary>
        /// </summary>
        /// <param name="participants"></param>
        /// <param name="topic"></param>
        /// <param name="senpai"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<ProxerResult<Conference>> CreateGroup(IEnumerable<string> participants,
            string topic, Senpai senpai, string message = null)
        {
            if (!IsInitialised)
                return new ProxerResult<Conference>(new NotInitialisedException("Please call " + nameof(Init)));
            if (string.IsNullOrEmpty(topic) || (topic.Length > MaxCharactersTopic))
                return new ProxerResult<Conference>(new ArgumentException(nameof(topic)));

            IEnumerable<string> lParticipantNames = participants as string[] ?? participants.ToArray();
            if (!lParticipantNames.Any() || lParticipantNames.Any(username => string.IsNullOrEmpty(username.Trim())))
                return new ProxerResult<Conference>(new[] {new ArgumentException(nameof(participants))});

            ProxerResult<ProxerApiResponse<int>> lResult =
                await
                    RequestHandler.ApiRequest(ApiRequestBuilder.MessengerNewConferenceGroup(lParticipantNames, topic,
                        senpai, message));
            return !lResult.Success || (lResult.Result == null)
                ? new ProxerResult<Conference>(lResult.Exceptions)
                : new ProxerResult<Conference>(new Conference(lResult.Result.Data, true, senpai));
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static async Task<ProxerResult<IEnumerable<ConferenceInfo>>> GetConferences(Senpai senpai,
            ConferenceListType type = ConferenceListType.Default)
        {
            if (!IsInitialised)
                return
                    new ProxerResult<IEnumerable<ConferenceInfo>>(new[]
                        {new NotInitialisedException("Please call " + nameof(Init))});

            List<ConferenceInfo> lConferences = new List<ConferenceInfo>();
            for (int page = 0; (page == 0) || (lConferences.Count%_conferencesPerPage == 0); page++)
            {
                ProxerResult<ProxerApiResponse<ConferenceDataModel[]>> lResult =
                    await RequestHandler.ApiRequest(ApiRequestBuilder.MessengerGetConferences(type, page, senpai));
                if (!lResult.Success || (lResult.Result == null))
                    return new ProxerResult<IEnumerable<ConferenceInfo>>(lResult.Exceptions);
                lConferences.AddRange(from conferenceDataModel in lResult.Result.Data
                    select new ConferenceInfo(conferenceDataModel, senpai));
            }
            return new ProxerResult<IEnumerable<ConferenceInfo>>(lConferences);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static async Task<ProxerResult> Init()
        {
            ProxerResult<ProxerApiResponse<ConstantsDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.MessengerGetConstants());
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);
            ConstantsDataModel lData = lResult.Result.Data;

            MaxCharactersPerMessage = lData.MaxCharactersPerMessage;
            MaxUsersPerConference = lData.MaxUsersPerConference;
            MaxCharactersTopic = lData.MaxCharactersTopic;
            MessagesPerPage = lData.MessagesPerPage;
            _conferencesPerPage = lData.ConferencesPerPage;
            IsInitialised = true;
            return new ProxerResult();
        }

        private async Task<ProxerResult> InitInfo()
        {
            ProxerResult<ProxerApiResponse<ConferenceInfoDataModel>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.MessengerGetConferenceInfo(this.Id, this._senpai));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult(lResult.Exceptions);
            ConferenceInfoDataModel lData = lResult.Result.Data;

            this._leader.SetInitialisedObject(new User(lData.MainInfo.LeaderUserId));
            this._participants.SetInitialisedObject(from conferenceInfoParticipantDataModel in lData.ParticipantsInfo
                select new User(conferenceInfoParticipantDataModel));
            this._topic.SetInitialisedObject(lData.MainInfo.Title);

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
        /// Sends a message to the current conference.
        /// </summary>
        /// <param name="message">The content of the message that is being send.</param>
        /// <returns>Whether the action was successfull.</returns>
        public async Task<ProxerResult<string>> SendMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException(nameof(message));

            ProxerResult<ProxerApiResponse<string>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.MessengerSetMessage(this.Id, message, this._senpai));
            if (!lResult.Success || (lResult.Result == null)) return new ProxerResult<string>(lResult.Exceptions);
            return new ProxerResult<string>(lResult.Result.Data ?? string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task<ProxerResult> SendReport(string reason)
        {
            if (string.IsNullOrEmpty(reason)) return new ProxerResult(new ArgumentException(nameof(reason)));

            ProxerResult<ProxerApiResponse<int>> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.MessengerSetReport(this.Id, reason, this._senpai));
            return !lResult.Success || (lResult.Result == null)
                ? new ProxerResult(lResult.Exceptions)
                : new ProxerResult();
        }

        /// <summary>
        /// </summary>
        /// <param name="isBlocked"></param>
        /// <returns></returns>
        public async Task<ProxerResult> SetBlock(bool isBlocked)
        {
            ProxerResult<ProxerApiResponse<int>> lResult =
                await
                    RequestHandler.ApiRequest(isBlocked
                        ? ApiRequestBuilder.MessengerSetBlock(this.Id, this._senpai)
                        : ApiRequestBuilder.MessengerSetUnblock(this.Id, this._senpai));
            return !lResult.Success || (lResult.Result == null)
                ? new ProxerResult(lResult.Exceptions)
                : new ProxerResult();
        }

        /// <summary>
        /// </summary>
        /// <param name="isFavourite"></param>
        /// <returns></returns>
        public async Task<ProxerResult> SetFavourite(bool isFavourite)
        {
            ProxerResult<ProxerApiResponse<int>> lResult =
                await
                    RequestHandler.ApiRequest(isFavourite
                        ? ApiRequestBuilder.MessengerSetFavour(this.Id, this._senpai)
                        : ApiRequestBuilder.MessengerSetUnfavour(this.Id, this._senpai));
            return !lResult.Success || (lResult.Result == null)
                ? new ProxerResult(lResult.Exceptions)
                : new ProxerResult();
        }

        /// <summary>
        /// Marks the current conference as unread.
        /// </summary>
        /// <returns>Whether the action was successfull.</returns>
        public async Task<ProxerResult> SetUnread()
        {
            ProxerResult<ProxerApiResponse> lResult =
                await RequestHandler.ApiRequest(ApiRequestBuilder.MessengerSetUnread(this.Id, this._senpai));
            return !lResult.Success || (lResult.Result == null)
                ? new ProxerResult(lResult.Exceptions)
                : new ProxerResult();
        }

        #endregion
    }
}