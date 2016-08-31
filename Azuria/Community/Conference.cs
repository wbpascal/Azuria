using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Api;
using Azuria.Api.v1;
using Azuria.Api.v1.DataModels.Messenger;
using Azuria.Exceptions;
using Azuria.User;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Properties;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.Community
{
    /// <summary>
    ///     Represents a messaging Conference.
    /// </summary>
    [DebuggerDisplay("Conference: {Title} [{Id}]")]
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
        /// <param name="e">
        ///     Contains the new messages.
        /// </param>
        public delegate void NewPmEventHandler(Conference sender, IEnumerable<Message> e);

        private static int _conferencesPerPage;

        private readonly Timer _checkMessagesTimer;
        private readonly Senpai _senpai;
        private Message _autoLastMessageRecieved;

        internal Conference(string title, int id, Senpai senpai)
        {
            this.Id = id;
            this._senpai = senpai;

            this.IsBlocked = new SetableInitialisableProperty<bool>(this.GetConferenceOptions, this.SetBlock);
            this.IsFavourite = new SetableInitialisableProperty<bool>(this.GetConferenceOptions, this.SetFavourite);
            this.Leader = new InitialisableProperty<User.User>(this.GetLeader);
            this.Participants = new InitialisableProperty<IEnumerable<User.User>>(this.GetMainInfo);
            this.Title = title;
            this.CanPerformCommands = new InitialisableProperty<bool>(this.CheckCanPerformCommands);

            this._checkMessagesTimer = new Timer {Interval = new TimeSpan(0, 0, 15).TotalMilliseconds};
            this._checkMessagesTimer.Elapsed += this.OnCheckMessagesTimerElapsed;
        }

        internal Conference(ConferenceDataModel dataModel, Senpai senpai)
            : this(dataModel.ConferenceTitle, dataModel.ConferenceId, senpai)
        {
        }

        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the conference is currently fetching new messages.
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

        [NotNull]
        private InitialisableProperty<bool> CanPerformCommands { get; }

        /// <summary>
        ///     Gets the Id of the conference.
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether the <see cref="Senpai.Me">User</see> blocks the current conference or not.
        /// </summary>
        [NotNull]
        public SetableInitialisableProperty<bool> IsBlocked { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether the current conference is currently a favourite of the
        ///     <see cref="Senpai.Me">User</see>.
        /// </summary>
        [NotNull]
        public SetableInitialisableProperty<bool> IsFavourite { get; }

        /// <summary>
        ///     Gets a value indicating whether the current object is fully initialised.
        /// </summary>
        public bool IsInitialized => this.IsFullyInitialised();

        /// <summary>
        ///     Gets a <see cref="User" /> that is the current leader of the conference.
        /// </summary>
        [NotNull]
        public InitialisableProperty<User.User> Leader { get; }

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
        ///     Gets all messages of the current conference ordered by newest first.
        /// </summary>
        [NotNull]
        public IEnumerable<Message> Messages => new MessageCollection(this, this._senpai);

        /// <summary>
        ///     Gets all participants of the current conference.
        /// </summary>
        [NotNull]
        public InitialisableProperty<IEnumerable<User.User>> Participants { get; }

        /// <summary>
        ///     Gets the current title of the current conference.
        /// </summary>
        [NotNull]
        public string Title { get; }

        #endregion

        #region

        [ItemNotNull]
        private async Task<ProxerResult> CheckCanPerformCommands()
        {
            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", "/ping"}
            };
            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.PostRequest(
                        new Uri("https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer"),
                        lPostArgs,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;
            try
            {
                Dictionary<string, string> lDict =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                this.CanPerformCommands.SetInitialisedObject(lDict["msg"].Equals("Erfolgreich!") &&
                                                             !lDict["message"].Equals(
                                                                 "Befehle sind nur in Konferenzen verfügbar."));
                return new ProxerResult();
            }
            catch
            {
                return
                    new ProxerResult(ErrorHandler.HandleError(lResponse, false).Exceptions);
            }
        }

        private void CheckForNewMessages()
        {
            if (this._autoLastMessageRecieved == null) return;

            Message[] lNewMessages =
                this.Messages.TakeWhile(message => message.MessageId != this._autoLastMessageRecieved.MessageId)
                    .ToArray();
            if (lNewMessages.Length == 0) return;

            //TODO: Invoke event/Check for actions
        }

        private async Task<ProxerResult> GetConferenceOptions()
        {
            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.GetRequest(
                        new Uri("http://proxer.me/messages?format=json&json=messages&id=" + this.Id),
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            if ((lResponse == null) || (this._senpai.Me == null) ||
                lResponse.Equals("{\"uid\":\"" + this._senpai.Me.Id +
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
                return new ProxerResult(ErrorHandler.HandleError(lResponse, false).Exceptions);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="senpai"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static async Task<ProxerResult<IEnumerable<Conference>>> GetConferences(Senpai senpai,
            ConferenceListType type = ConferenceListType.Default)
        {
            if (_conferencesPerPage == default(int))
                return
                    new ProxerResult<IEnumerable<Conference>>(new[]
                        {new NotInitialisedException("Please call " + nameof(Init))});

            List<Conference> lConferences = new List<Conference>();
            for (int page = 0; (page == 0) || (lConferences.Count%_conferencesPerPage == 0); page++)
            {
                ProxerResult<ProxerApiResponse<ConferenceDataModel[]>> lResult =
                    await RequestHandler.ApiRequest(ApiRequestBuilder.MessengerGetConferences(type, page, senpai));
                if (!lResult.Success || (lResult.Result == null))
                    return new ProxerResult<IEnumerable<Conference>>(lResult.Exceptions);
                lConferences.AddRange(from conferenceDataModel in lResult.Result.Data
                    select new Conference(conferenceDataModel, senpai));
            }
            return new ProxerResult<IEnumerable<Conference>>(lConferences);
        }

        [ItemNotNull]
        private async Task<ProxerResult> GetLeader()
        {
            if (!(await this.CanPerformCommands.GetObject()).OnError(false))
            {
                this.Leader.SetInitialisedObject(User.User.System);
                return new ProxerResult();
            }

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", "/leader"}
            };
            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.PostRequest(
                        new Uri("https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer"),
                        lPostArgs,
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
                     .OnError(new User.User[0])?
                     .FirstOrDefault(
                         x =>
                             x.UserName.GetObjectIfInitialised("")
                                 .Equals(lDict["message"].Remove(0, "Konferenzleiter: ".Length))) ?? User.User.System);

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult(ErrorHandler.HandleError(lResponse, false).Exceptions);
            }
        }

        [ItemNotNull]
        private async Task<ProxerResult> GetMainInfo()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.GetRequest(
                        new Uri("https://proxer.me/messages?id=" + this.Id + "&format=raw"),
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            lDocument.LoadHtml(lResponse);
            try
            {
                List<User.User> lTeilnehmer = new List<User.User>();

                lTeilnehmer.AddRange(
                    from curTeilnehmer in lDocument.GetElementbyId("conferenceUsers").ChildNodes[1].ChildNodes
                    let lUserName = curTeilnehmer.ChildNodes[1].FirstChild.InnerText
                    let lUserId =
                    Convert.ToInt32(
                        curTeilnehmer.ChildNodes[1].FirstChild.Attributes["href"].Value.GetTagContents("/user/",
                            "#top")[0])
                    select new User.User(lUserName, lUserId));

                this.Participants.SetInitialisedObject(lTeilnehmer);

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult(ErrorHandler.HandleError(lResponse, false).Exceptions);
            }
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
            _conferencesPerPage = lData.ConferencesPerPage;
            return new ProxerResult();
        }

        /// <summary>
        ///     Occurs when new messages were recieved or once everytime Active is set to true.
        /// </summary>
        public event NewPmEventHandler NewMessageRecieved;

        private void OnCheckMessagesTimerElapsed(object s, EventArgs eArgs)
        {
            Timer timer = s as Timer;
            timer?.Stop();
            this.CheckForNewMessages();
            timer?.Start();
        }

        /// <summary>
        ///     Sends a message to the current conference.
        /// </summary>
        /// <param name="message">The content of the message that is being send.</param>
        /// <returns>Whether the action was successfull.</returns>
        [ItemNotNull]
        public async Task<ProxerResult> SendMessage([NotNull] string message)
        {
            if (string.IsNullOrEmpty(message))
                return
                    new ProxerResult(new[]
                        {new ArgumentException("Argument is null or empty", nameof(message))});

            this._checkMessagesTimer.Stop();

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", message}
            };
            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.PostRequest(
                        new Uri("https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer"),
                        lPostArgs,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                Dictionary<string, string> lResponseJson =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                if (this.AutoCheck) this.CheckForNewMessages();
                if (lResponseJson.Keys.Contains("message"))
                    return new ProxerResult();
                if (!lResponseJson.Keys.Contains("msg")) return new ProxerResult {Success = false};

                this._checkMessagesTimer.Start();
                return new ProxerResult();
            }
            catch
            {
                this._checkMessagesTimer.Start();
                return
                    new ProxerResult(ErrorHandler.HandleError(lResponse, false).Exceptions);
            }
        }

        private async Task<ProxerResult> SetBlock(bool isBlocked)
        {
            string lAction = isBlocked ? "block" : "unblock";
            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.GetRequest(
                        new Uri($"http://proxer.me/messages?format=json&json={lAction}&id={this.Id}"),
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            return lResponse?.StartsWith("{\"error\":0") ?? false
                ? new ProxerResult()
                : new ProxerResult {Success = false};
        }

        private async Task<ProxerResult> SetFavourite(bool isFavourite)
        {
            string lAction = isFavourite ? "favour" : "unfavour";
            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.GetRequest(
                        new Uri($"http://proxer.me/messages?format=json&json={lAction}&id={this.Id}"),
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            return lResponse?.StartsWith("{\"error\":0") ?? false
                ? new ProxerResult()
                : new ProxerResult {Success = false};
        }

        /// <summary>
        ///     Marks the current conference as unread.
        /// </summary>
        /// <returns>Whether the action was successfull.</returns>
        [ItemNotNull]
        public async Task<ProxerResult> SetUnread()
        {
            ProxerResult<string> lResult =
                await
                    ApiInfo.HttpClient.GetRequest(
                        new Uri("http://proxer.me/messages?format=json&json=setUnread&id=" + this.Id),
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            return lResponse?.StartsWith("{\"error\":0") ?? false
                ? new ProxerResult()
                : new ProxerResult {Success = false};
        }

        #endregion
    }
}