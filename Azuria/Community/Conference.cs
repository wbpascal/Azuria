﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Azuria.Community.ConferenceHelper;
using Azuria.ErrorHandling;
using Azuria.Exceptions;
using Azuria.Utilities;
using Azuria.Utilities.Net;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Azuria.Community
{
    /// <summary>
    ///     Repräsentiert eine Proxer-Konferenz.
    /// </summary>
    public class Conference
    {
        /// <summary>
        ///     Stellt eine Methode da, die ausgelöst wird, wenn während des Abrufen der Nachrichten ein Fehler auftritt.
        /// </summary>
        /// <param name="sender">Die Konferenz, die das Event aufgerufen hat.</param>
        /// <param name="exceptions">Die Ausnahmen, die ausgelöst wurden.</param>
        public delegate void ErrorDuringPmFetchEventHandler(Conference sender, IEnumerable<Exception> exceptions);

        /// <summary>
        ///     Stellt eine Methode da, die ausgelöst wird, wenn neue Nachrichten in der Konferenz vorhanden sind.
        /// </summary>
        /// <param name="sender">Die Konferenz, die das Event aufgerufen hat.</param>
        /// <param name="e">Die neuen Nachrichten. Beim ersten mal werden hier alle Nachrichten aufgeführt.</param>
        /// <param name="alleNachrichten">Gibt an, ob alle Nachrichten geholt wurden oder nur die neuesten.</param>
        public delegate void NeuePmEventHandler(Conference sender, IEnumerable<Message> e, bool alleNachrichten);

        private readonly Timer _getMessagesTimer;
        private readonly Func<Task<ProxerResult>>[] _initFuncs;
        private readonly Senpai _senpai;
        private User _leiter;
        private IEnumerable<Message> _nachrichten;
        private IEnumerable<User> _teilnehmer;

        /// <summary>
        ///     Standard-Konstruktor der Klasse.
        /// </summary>
        /// <param name="id">Die ID der Konferenz</param>
        /// <param name="senpai">Muss Teilnehmer der Konferenz sein. Darf nicht null sein.</param>
        public Conference(int id, Senpai senpai)
        {
            this._initFuncs = new Func<Task<ProxerResult>>[] {this.GetAllParticipants, this.GetLeader, this.GetTitle};

            this.Id = id;
            this._senpai = senpai;

            this._getMessagesTimer = new Timer {Interval = new TimeSpan(0, 0, 15).TotalMilliseconds};
            this._getMessagesTimer.Elapsed += this.OnGetMessagesTimerElapsed;
        }

        internal Conference(string title, int id, Senpai senpai)
        {
            this._initFuncs = new Func<Task<ProxerResult>>[] {this.GetAllParticipants, this.GetLeader, this.GetTitle};

            this.Title = title;
            this.Id = id;
            this._senpai = senpai;

            this._getMessagesTimer = new Timer {Interval = new TimeSpan(0, 0, 15).TotalMilliseconds};
            this._getMessagesTimer.Elapsed += this.OnGetMessagesTimerElapsed;
        }

        #region Properties

        /// <summary>
        ///     Gibt einen Wert an, ob die Privatnachrichten in einem bestimmten Intervall abgerufen werden, oder legt diesen fest.
        /// </summary>
        public bool Activ
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
        ///     Gibt die ID der Konferenz zurück
        /// </summary>
        public int Id { get; }


        private bool IsConference { get; set; }

        /// <summary>
        ///     Gibt zurück, ob die Konferenz bereits initialisiert ist.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        ///     Gibt den Leiter der Konferenz zurück. (<see cref="Init" /> muss dafür zunächst einmal aufgerufen werden)
        /// </summary>
        public User Leader
        {
            get { return this._leiter ?? User.System; }
            private set { this._leiter = value; }
        }

        /// <summary>
        ///     Gibt alle Nachrichten aus der Konferenz in chronoligischer Ordnung zurück.
        /// </summary>
        public IEnumerable<Message> Messages
        {
            get { return this._nachrichten ?? new List<Message>(); }
            private set { this._nachrichten = value; }
        }

        /// <summary>
        ///     Gibt alle Teilnehmer der Konferenz zurück (<see cref="Init" /> muss dafür zunächst einmal aufgerufen
        ///     werden)
        /// </summary>
        public IEnumerable<User> Participants
        {
            get { return this._teilnehmer ?? new List<User>(); }
            private set { this._teilnehmer = value; }
        }

        /// <summary>
        ///     Gibt den Titel der Konferenz zurück (<see cref="Init" /> muss dafür zunächst einmal aufgerufen werden)
        /// </summary>
        public string Title { get; private set; }

        #endregion

        #region

        /// <summary>
        ///     Blockiert die Konferenz.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<ProxerResult<bool>> Block()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "http://proxer.me/messages?format=json&json=block&id=" + this.Id,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult<bool>(lResult.Exceptions);

            string lResponse = lResult.Result;

            return new ProxerResult<bool>(lResponse.StartsWith("{\"error\":0"));
        }

        private async Task<ProxerResult<bool>> CheckIsConference()
        {
            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", "/ping"}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(
                        "https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer",
                        lPostArgs,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult<bool>(lResult.Exceptions);

            string lResponse = lResult.Result;
            try
            {
                Dictionary<string, string> lDict =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                return new ProxerResult<bool>(lDict["msg"].Equals("Erfolgreich!") &&
                                              !lDict["message"].Equals("Befehle sind nur in Konferenzen verfügbar."));
            }
            catch
            {
                return
                    new ProxerResult<bool>((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        /// <summary>
        ///     Wird ausgelöst, wenn während des Abrufen der Nachrichten ein Fehler auftritt.
        /// </summary>
        public event ErrorDuringPmFetchEventHandler ErrorDuringPmFetchRaised;

        /// <summary>
        ///     Markiert die Konferenz als Favorit.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<ProxerResult<bool>> Favour()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "http://proxer.me/messages?format=json&json=favour&id=" + this.Id,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult<bool>(lResult.Exceptions);

            string lResponse = lResult.Result;

            return new ProxerResult<bool>(lResponse.StartsWith("{\"error\":0"));
        }

        private async Task<ProxerResult> GetAllMessages()
        {
            if (this.Messages != null && this.Messages.ToList().Count > 0)
                return await this.GetMessages(this.Messages.Last().MessageId);

            if (this.Messages != null && this.Messages.ToList().Count != 0)
                return new ProxerResult
                {
                    Success = false
                };

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "http://proxer.me/messages?format=json&json=messages&id=" + this.Id,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            if (lResponse.Equals("{\"uid\":\"" + this._senpai.Me.Id +
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

                this.Messages = lResultMessages.Result;

                if (this.Messages.Any(
                    x => x.MessageAction == Message.Action.AddUser || x.MessageAction == Message.Action.RemoveUser))
                    await this.GetAllParticipants();
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

        private async Task<ProxerResult> GetAllParticipants()
        {
            HtmlDocument lDocument = new HtmlDocument();
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "https://proxer.me/messages?id=" + this.Id + "&format=raw",
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            try
            {
                lDocument.LoadHtml(lResponse);

                List<User> lTeilnehmer = this.Participants.ToList();

                lTeilnehmer.AddRange(
                    from curTeilnehmer in lDocument.GetElementbyId("conferenceUsers").ChildNodes[1].ChildNodes
                    let lUserName = curTeilnehmer.ChildNodes[1].FirstChild.InnerText
                    let lUserId =
                        Convert.ToInt32(
                            curTeilnehmer.ChildNodes[1].FirstChild.Attributes["href"].Value.GetTagContents("/user/",
                                "#top")[0])
                    select new User(lUserName, lUserId, this._senpai));

                this.Participants = lTeilnehmer;

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        private async Task<ProxerResult> GetLeader()
        {
            if (!this.IsConference) return new ProxerResult();

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", "/leader"}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(
                        "https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer",
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

                this.Leader =
                    this.Participants?.FirstOrDefault(
                        x => x.UserName.Equals(lDict["message"].Remove(0, "Konferenzleiter: ".Length))) ?? User.System;

                return new ProxerResult();
            }
            catch
            {
                return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }
        }

        private async Task<ProxerResult> GetMessages(int mid)
        {
            if (this.Messages == null || this.Messages.Count(x => x.MessageId == mid) == 0)
                return await this.GetAllMessages();

            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "http://proxer.me/messages?format=json&json=newmessages&id=" + this.Id + "&mid=" + mid,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult(lResult.Exceptions);

            string lResponse = lResult.Result;

            if (lResponse.Equals("{\"uid\":\"" + this._senpai.Me.Id +
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

                List<Message> lNewMessages = lResultMessages.Result;

                if (
                    lNewMessages.Count(
                        x => x.MessageAction == Message.Action.AddUser || x.MessageAction == Message.Action.RemoveUser) >
                    0)
                    await this.GetAllParticipants();
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

        private async void GetMessagesTimer(Timer timer)
        {
            if (this.IsInitialized)
            {
                ProxerResult lResult;
                if (this.Messages != null && this.Messages.Any())
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
            }

            timer?.Start();
        }

        private async Task<ProxerResult> GetTitle()
        {
            if (this.IsConference)
            {
                Dictionary<string, string> lPostArgs = new Dictionary<string, string>
                {
                    {"message", "/topic"}
                };

                ProxerResult<string> lResult =
                    await
                        HttpUtility.PostResponseErrorHandling(
                            "https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer",
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
                    if (lDict["msg"].Equals("Erfolgreich!")) this.Title = lDict["message"];

                    return new ProxerResult();
                }
                catch
                {
                    return new ProxerResult((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
                }
            }

            if (this.Participants.ToList().Count > 1)
                this.Title = this.Participants.Where(x => x.Id != this._senpai.Me.Id).ToArray()[0].UserName;

            return new ProxerResult();
        }


        /// <summary>
        ///     Initialisiert die Konferenz.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        public async Task<ProxerResult> Init()
        {
            if (!this._senpai.IsLoggedIn)
                return new ProxerResult(new Exception[] {new NotLoggedInException(this._senpai)});

            ProxerResult<bool> lIstTeilnehmer = await Participates(this.Id, this._senpai);
            if (lIstTeilnehmer.Success && !lIstTeilnehmer.Result)
                return new ProxerResult
                {
                    Success = false
                };

            ProxerResult<bool> lIsConference = await this.CheckIsConference();
            if (lIsConference.Success) this.IsConference = lIsConference.Result;

            int lFailedInits = 0;
            ProxerResult lReturn = new ProxerResult();
            foreach (Func<Task<ProxerResult>> initFunc in this._initFuncs)
            {
                try
                {
                    ProxerResult lResult;
                    if ((lResult = await initFunc.Invoke()).Success) continue;

                    lReturn.AddExceptions(lResult.Exceptions);
                    lFailedInits++;
                }
                catch
                {
                    return new ProxerResult
                    {
                        Success = false
                    };
                }
            }

            this.IsInitialized = true;
            if (lFailedInits < this._initFuncs.Length)
                lReturn.Success = true;

            return lReturn;
        }

        /// <summary>
        ///     Wird immer aufgerufen, wenn neue Nachrichten in der Konferenz vorhanden sind.
        /// </summary>
        public event NeuePmEventHandler NeuePmRaised;

        private void OnGetMessagesTimerElapsed(object s, EventArgs eArgs)
        {
            Timer timer = s as Timer;
            timer?.Stop();
            this.GetMessagesTimer(timer);
        }

        /// <summary>
        ///     Gibt zurück, ob Senpai ein Teilnehmer einer Konferenz mit der bestimmten ID ist.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="ArgumentNullException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn <paramref name="senpai" /> null (oder Nothing in Visual Basic) ist.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="id">ID der Konferenz</param>
        /// <param name="senpai">Muss eingeloggt sein</param>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Benutzer ist Teilnehmer der Konferenz. True oder False.</returns>
        public static async Task<ProxerResult<bool>> Participates(int id, Senpai senpai)
        {
            if (senpai == null)
                return new ProxerResult<bool>(new Exception[] {new ArgumentNullException(nameof(senpai))});

            if (!senpai.IsLoggedIn) return new ProxerResult<bool>(new Exception[] {new NotLoggedInException(senpai)});

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", "/ping"}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(
                        "https://proxer.me/messages?id=" + id + "&format=json&json=answer",
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

        private async Task<ProxerResult<List<Message>>> ProcessMessages(string messages)
        {
            List<Message> lReturn = new List<Message>();

            try
            {
                MessagesModel lMessages = JsonConvert.DeserializeObject<MessagesModel>(messages);

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
                        this.Participants.Where(x => x.Id == Convert.ToInt32(curMessage.Fromid)).ToArray();

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
        ///     Sendet eine Nachricht an die Konferenz.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="nachricht">Die Nachricht, die gesendet werden soll</param>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<ProxerResult<bool>> SendMessage(string nachricht)
        {
            this._getMessagesTimer.Stop();

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", nachricht}
            };
            ProxerResult<string> lResult =
                await
                    HttpUtility.PostResponseErrorHandling(
                        "https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer",
                        lPostArgs,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult<bool>(lResult.Exceptions);

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
                    return new ProxerResult<bool>(true);
                }
                if (lResponseJson["msg"].Equals("Erfolgreich!"))
                {
                    await this.GetMessages(this.Messages.Last().MessageId);
                    this._getMessagesTimer.Start();
                    return new ProxerResult<bool>(true);
                }
            }
            catch
            {
                return
                    new ProxerResult<bool>((await ErrorHandler.HandleError(this._senpai, lResponse, false)).Exceptions);
            }

            this._getMessagesTimer.Start();

            return new ProxerResult<bool>(new Exception[] {new WrongResponseException {Response = lResponse}});
        }

        /// <summary>
        ///     Markiert die Konferenz als ungelesen.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<ProxerResult<bool>> SetUnread()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "http://proxer.me/messages?format=json&json=setUnread&id=" + this.Id,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult<bool>(lResult.Exceptions);

            string lResponse = lResult.Result;

            return new ProxerResult<bool>(lResponse.StartsWith("{\"error\":0"));
        }

        /// <summary>
        ///     Entblockt die Konferenz.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<ProxerResult<bool>> Unblock()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "http://proxer.me/messages?format=json&json=unblock&id=" + this.Id,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult<bool>(lResult.Exceptions);

            string lResponse = lResult.Result;

            return new ProxerResult<bool>(lResponse.StartsWith("{\"error\":0"));
        }

        /// <summary>
        ///     Entfernt die Konferenz aus den Favoriten.
        ///     <para>Mögliche Fehler, die <see cref="ProxerResult" /> enthalten kann:</para>
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Ausnahme</term>
        ///             <description>Beschreibung</description>
        ///         </listheader>
        ///         <item>
        ///             <term>
        ///                 <see cref="NotLoggedInException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn der <see cref="Senpai">Benutzer</see> nicht eingeloggt ist.</description>
        ///         </item>
        ///         <item>
        ///             <term>
        ///                 <see cref="WrongResponseException" />
        ///             </term>
        ///             <description>Wird ausgelöst, wenn die Antwort des Servers nicht der Erwarteten entspricht.</description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <seealso cref="Senpai.Login" />
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<ProxerResult<bool>> Unfavour()
        {
            ProxerResult<string> lResult =
                await
                    HttpUtility.GetResponseErrorHandling(
                        "http://proxer.me/messages?format=json&json=unfavour&id=" + this.Id,
                        this._senpai.LoginCookies,
                        this._senpai.ErrHandler,
                        this._senpai);

            if (!lResult.Success)
                return new ProxerResult<bool>(lResult.Exceptions);

            string lResponse = lResult.Result;

            return new ProxerResult<bool>(lResponse.StartsWith("{\"error\":0"));
        }

        #endregion

        /// <summary>
        ///     Repräsentiert die jeweilige einzelne Nachricht in der Konferenz.
        /// </summary>
        public class Message
        {
            /// <summary>
            ///     Die Aktion der Nachricht.
            /// </summary>
            public enum Action
            {
                /// <summary>
                ///     Normale Nachricht, nur Text.
                /// </summary>
                NoAction,

                /// <summary>
                ///     Ein Benutzer wurde hinzugefügt.
                /// </summary>
                AddUser,

                /// <summary>
                ///     Ein Benutzer wurde entfernt.
                /// </summary>
                RemoveUser,

                /// <summary>
                ///     Der Leiter der Konferenz wurde geändert.
                /// </summary>
                SetLeader,

                /// <summary>
                ///     Das Thema der Konferenz wurde geändert.
                /// </summary>
                SetTopic,

                /// <summary>
                ///     Immer wenn von der JSON direkt etwas zurückgegeben wird z.B. bei /leader
                /// </summary>
                GetAction
            }

            internal Message(User sender, int mid, string nachricht, int unix, Action action)
                : this(sender, mid, nachricht, Utility.UnixTimeStampToDateTime(unix), action)
            {
            }

            internal Message(User sender, int mid, string nachricht, DateTime date, Action action)
            {
                this.Sender = sender;
                this.MessageId = mid;
                this.Content = nachricht;
                this.TimeStamp = date;
                this.MessageAction = action;
            }

            #region Properties

            /// <summary>
            ///     Gibt den Text der Nachricht zurück.
            /// </summary>
            public string Content { get; private set; }

            /// <summary>
            ///     Gibt die Aktion der Nachricht zurück.
            /// </summary>
            public Action MessageAction { get; }

            /// <summary>
            ///     Gibt die ID der Nachricht zurück.
            /// </summary>
            public int MessageId { get; }

            /// <summary>
            ///     Gibt den Sender der Nachricht zurück.
            /// </summary>
            public User Sender { get; private set; }

            /// <summary>
            ///     Gibt das Datum der Nachricht zurück.
            /// </summary>
            public DateTime TimeStamp { get; private set; }

            #endregion
        }
    }
}