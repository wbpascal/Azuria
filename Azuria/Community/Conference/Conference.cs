using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Exceptions;
using Azuria.User;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Extensions;
using Azuria.Utilities.Properties;
using Azuria.Utilities.Web;
using HtmlAgilityPack;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Azuria.Community.Conference
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
        /// <param name="e">
        ///     Contains the new messages.
        /// </param>
        public delegate void NewPmEventHandler(Conference sender, IEnumerable<Message> e);

        private readonly Timer _checkMessagesTimer;
        private readonly Senpai _senpai;
        private Message _autoLastMessageRecieved;

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
            this.Leader = new InitialisableProperty<User.User>(this.GetLeader);
            this.Participants = new InitialisableProperty<IEnumerable<User.User>>(this.GetMainInfo);
            this.Title = new InitialisableProperty<string>(this.GetTitle);
            this.CanPerformCommands = new InitialisableProperty<bool>(this.CheckCanPerformCommands);

            this._checkMessagesTimer = new Timer {Interval = new TimeSpan(0, 0, 15).TotalMilliseconds};
            this._checkMessagesTimer.Elapsed += this.OnCheckMessagesTimerElapsed;
        }

        internal Conference(string title, int id, Senpai senpai) : this(id, senpai)
        {
            this.Title = new InitialisableProperty<string>(this.GetTitle, title);
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
        public InitialisableProperty<string> Title { get; }

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
                    HttpUtility.PostResponseErrorHandling(
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
                    new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse, false).Exceptions);
            }
        }

        private void CheckForNewMessages()
        {
            if (this._autoLastMessageRecieved == null && this.Messages.Any())
                this.NeuePmRaised?.Invoke(this, this.Messages.ToArray());
            else
            {
                Message[] lNewMessages =
                    this.Messages.TakeWhile(message => message.MessageId != this._autoLastMessageRecieved.MessageId)
                        .ToArray();
                if (!lNewMessages.Any()) return;

#pragma warning disable CS4014
                if (
                    lNewMessages.Any(
                        x =>
                            x.MessageAction == Message.Action.AddUser ||
                            x.MessageAction == Message.Action.RemoveUser) && this.Participants.IsInitialisedOnce)
                    this.GetMainInfo();
                if (lNewMessages.Any(x => x.MessageAction == Message.Action.SetLeader) &&
                    this.Leader.IsInitialisedOnce) this.GetLeader();
                if (lNewMessages.Any(x => x.MessageAction == Message.Action.SetTopic) &&
                    this.Title.IsInitialisedOnce) this.GetTitle();
#pragma warning restore CS4014
                this.NeuePmRaised?.Invoke(this, lNewMessages);
            }
        }

        private async Task<ProxerResult> GetConferenceOptions()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        new Uri("http://proxer.me/messages?format=json&json=messages&id=" + this.Id),
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
                return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse, false).Exceptions);
            }
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
                    HttpUtility.PostResponseErrorHandling(
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
                return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse, false).Exceptions);
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
                    select new User.User(lUserName, lUserId, this._senpai));

                this.Participants.SetInitialisedObject(lTeilnehmer);

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse, false).Exceptions);
            }
        }

        [ItemNotNull]
        private async Task<ProxerResult> GetTitle()
        {
            if ((await this.CanPerformCommands.GetObject()).OnError(false))
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
                    return new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse, false).Exceptions);
                }
            }

            if ((await this.Participants.GetObject()).OnError(new User.User[0])?.Count() > 1 && this._senpai.Me != null)
                this.Title.SetInitialisedObject(await (await this.Participants.GetObject()).OnError(new User.User[0])?
                    .Where(x => x.Id != this._senpai.Me.Id)
                    .ToArray()[0].UserName.GetObject("ERROR"));

            return new ProxerResult();
        }


        /// <summary>
        ///     Initialises every property of the current class.
        /// </summary>
        [ItemNotNull]
        [Obsolete("Bitte benutze die Methoden der jeweiligen Eigenschaften, um sie zu initalisieren!")]
        public async Task<ProxerResult> Init()
        {
            return await this.InitAllInitalisableProperties();
        }

        /// <summary>
        ///     Occurs when new messages were recieved or once everytime Active is set to true.
        /// </summary>
        public event NewPmEventHandler NeuePmRaised;

        private void OnCheckMessagesTimerElapsed(object s, EventArgs eArgs)
        {
            Timer timer = s as Timer;
            timer?.Stop();
            this.CheckForNewMessages();
            timer?.Start();
        }

        /// <summary>
        ///     Returns if the <paramref name="senpai">User</paramref> is part of the conference.
        /// </summary>
        /// <param name="id">The Id of the <see cref="Conference" /></param>
        /// <param name="senpai">The user which is tested.</param>
        /// <returns>Whether the <paramref name="senpai">User</paramref> is part of the conference.</returns>
        [ItemNotNull]
        public static async Task<ProxerResult<bool>> Participates(int id, [NotNull] Senpai senpai)
        {
            if (!senpai.IsProbablyLoggedIn)
                return new ProxerResult<bool>(new Exception[] {new NotLoggedInException(senpai)});

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", "/ping"}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(
                        new Uri("https://proxer.me/messages?id=" + id + "&format=json&json=answer"),
                        lPostArgs,
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
                return new ProxerResult<bool>(ErrorHandler.HandleError(senpai, lResponse, false).Exceptions);
            }
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
                    HttpUtility.PostResponseErrorHandling(
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
                {
                    return new ProxerResult();
                }
                if (!lResponseJson.Keys.Contains("msg")) return new ProxerResult {Success = false};

                this._checkMessagesTimer.Start();
                return new ProxerResult();
            }
            catch
            {
                this._checkMessagesTimer.Start();
                return
                    new ProxerResult(ErrorHandler.HandleError(this._senpai, lResponse, false).Exceptions);
            }
        }

        private async Task<ProxerResult> SetBlock(bool isBlocked)
        {
            string lAction = isBlocked ? "block" : "unblock";
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
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
                    HttpUtility.GetResponseErrorHandling(
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
                    HttpUtility.GetResponseErrorHandling(
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