using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Proxer.API.Exceptions;
using Proxer.API.Utilities;

namespace Proxer.API.Community
{
    /// <summary>
    ///     Repräsentiert eine Proxer-Konferenz
    /// </summary>
    public class Conference
    {
        /// <summary>
        /// </summary>
        /// <param name="sender">Die Konferenz, die das Event aufgerufen hat</param>
        /// <param name="e">Die neuen Nachrichten. Beim ersten mal werden hier alle Nachrichten aufgeführt</param>
        public delegate void NeuePmEventHandler(Conference sender, List<Message> e);

        private readonly Timer _getMessagesTimer;

        private readonly Senpai _senpai;

        /// <summary>
        ///     Standard-Konstruktor der Klasse
        /// </summary>
        /// <param name="id"></param>
        /// <param name="senpai"></param>
        public Conference(int id, Senpai senpai)
        {
            this.Id = id;
            this._senpai = senpai;

            this._getMessagesTimer = new Timer {Interval = (new TimeSpan(0, 0, 15)).TotalMilliseconds};
            this._getMessagesTimer.Elapsed += async (s, eArgs) =>
            {
                this._getMessagesTimer.Interval = (new TimeSpan(0, 0, 15)).TotalMilliseconds;
                Timer timer = s as Timer;
                if (timer != null) timer.Stop();
                if (this.IstInitialisiert)
                {
                    if (this.Nachrichten != null && this.Nachrichten.Any())
                        await this.GetMessages(this.Nachrichten.Last().NachrichtId);
                    else await this.GetAllMessages();
                }
                Timer timer1 = s as Timer;
                if (timer1 != null) timer1.Start();
            };

            this.Aktiv = false;
        }

        internal Conference(string title, int id, Senpai senpai)
        {
            this.Titel = title;
            this.Id = id;
            this._senpai = senpai;

            this._getMessagesTimer = new Timer {Interval = (new TimeSpan(0, 0, 15)).TotalMilliseconds};
            this._getMessagesTimer.Elapsed += async (s, eArgs) =>
            {
                this._getMessagesTimer.Interval = (new TimeSpan(0, 0, 15)).TotalMilliseconds;
                Timer timer = s as Timer;
                if (timer != null) timer.Stop();
                if (this.IstInitialisiert)
                {
                    if (this.Nachrichten != null && this.Nachrichten.Any())
                        await this.GetMessages(this.Nachrichten.Last().NachrichtId);
                    else await this.GetAllMessages();
                }
                Timer timer1 = s as Timer;
                if (timer1 != null) timer1.Start();
            };

            this.Aktiv = false;
        }

        #region Properties

        /// <summary>
        ///     Gibt einen Wert an, ob die Privatnachrichten in einem bestimmten Intervall abgerufen werden, oder legt diesen fest
        /// </summary>
        public bool Aktiv
        {
            get { return this._getMessagesTimer.Enabled; }
            set
            {
                if (value)
                {
                    this._getMessagesTimer.Interval = 1;
                    this._getMessagesTimer.Start();
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
        public int Id { get; private set; }


        private bool IsConference { get; set; }

        /// <summary>
        ///     Gibt zurück, ob die Konferenz bereits initialisiert ist
        /// </summary>
        public bool IstInitialisiert { get; private set; }

        /// <summary>
        ///     Gibt den Leiter der Konferenz zurück (initConference() muss dafür zunächst einmal aufgerufen werden)
        /// </summary>
        public User Leiter { get; private set; }

        /// <summary>
        ///     Gibt alle Nachrichten aus der Konferenz in chronoligischer Ordnung zurück.
        /// </summary>
        public List<Message> Nachrichten { get; private set; }

        /// <summary>
        ///     Gibt alle Teilnehmer der Konferenz zurück (initConference() muss dafür zunächst einmal aufgerufen werden)
        /// </summary>
        public List<User> Teilnehmer { get; private set; }

        /// <summary>
        ///     Gibt den Titel der Konferenz zurück (initConference() muss dafür zunächst einmal aufgerufen werden)
        /// </summary>
        public string Titel { get; private set; }

        #endregion

        #region

        /// <summary>
        ///     Wird immer aufgerufen, wenn neue Nachrichten in der Konferenz vorhanden sind
        /// </summary>
        public event NeuePmEventHandler NeuePmRaised;


        /// <summary>
        ///     Initialisiert die Konferenz
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        public async Task InitConference()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();

            if(!await IstTeilnehmner(this.Id, this._senpai)) return;
            this.IsConference = await this.CheckIsConference();
            try
            {
                await this.GetAllParticipants();
                await this.GetLeader();
                await this.GetTitle();
            }
            catch (NotLoggedInException)
            {
                throw new NotLoggedInException();
            }

            this.IstInitialisiert = true;
        }

        /// <summary>
        ///     Sendet eine Nachricht an die Konferenz
        /// </summary>
        /// <param name="nachricht">Die Nachricht, die gesendet werden soll</param>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<bool> SendeNachricht(string nachricht)
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();

            this._getMessagesTimer.Stop();

            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", nachricht}
            };
            string lResponse =
                await HttpUtility.PostWebRequestResponse(
                    "https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer",
                    this._senpai.LoginCookies,
                    lPostArgs);

            if (Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
            {
                try
                {
                    Dictionary<string, string> lResponseJson =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                    if (lResponseJson.Keys.Contains("message"))
                    {
                        if (this.NeuePmRaised != null)
                            this.NeuePmRaised(this,
                                new List<Message>
                                {
                                    new Message(User.System, -1, lResponseJson["message"], DateTime.Now,
                                        Message.Action.GetAction)
                                });
                        return true;
                    }
                    if (lResponseJson["msg"].Equals("Erfolgreich!"))
                    {
                        await this.GetMessages(this.Nachrichten.Last().NachrichtId);
                        this._getMessagesTimer.Start();
                        return true;
                    }
                }
                catch (Exception)
                {
                    this._senpai.ErrHandler.Add(lResponse);
                }
            }

            this._getMessagesTimer.Start();

            return false;
        }

        /// <summary>
        ///     Markiert die Konferenz als ungelesen
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<bool> AlsUngelesenMarkieren()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();
            string lResponse =
                await HttpUtility.GetWebRequestResponse(
                    "http://proxer.me/messages?format=json&json=setUnread&id=" + this.Id, this._senpai.LoginCookies);

            return lResponse.StartsWith("{\"error\":0");
        }

        /// <summary>
        ///     Markiert die Konferenz als Favorit
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<bool> FavoritHinzufuegen()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();
            string lResponse =
                await HttpUtility.GetWebRequestResponse(
                    "http://proxer.me/messages?format=json&json=favour&id=" + this.Id, this._senpai.LoginCookies);
            return lResponse.StartsWith("{\"error\":0");
        }

        /// <summary>
        ///     Entfernt die Konferenz aus den Favoriten
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<bool> FavoritEntfernen()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();
            string lResponse =
                await HttpUtility.GetWebRequestResponse(
                    "http://proxer.me/messages?format=json&json=unfavour&id=" + this.Id, this._senpai.LoginCookies);
            return lResponse.StartsWith("{\"error\":0");
        }

        /// <summary>
        ///     Blockiert die Konferenz
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<bool> BlockHinzufuegen()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();
            string lResponse =
                await HttpUtility.GetWebRequestResponse("http://proxer.me/messages?format=json&json=block&id=" + this.Id,
                    this._senpai.LoginCookies);
            return lResponse.StartsWith("{\"error\":0");
        }

        /// <summary>
        ///     Entblockt die Konferenz
        /// </summary>
        /// <exception cref="NotLoggedInException"></exception>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public async Task<bool> BlockEntfernen()
        {
            if (!this._senpai.LoggedIn) throw new NotLoggedInException();
            string lResponse =
                await HttpUtility.GetWebRequestResponse(
                    "http://proxer.me/messages?format=json&json=unblock&id=" + this.Id, this._senpai.LoginCookies);
            return lResponse.StartsWith("{\"error\":0");
        }


        private async Task GetLeader()
        {
            if (this.IsConference)
            {
                Dictionary<string, string> lPostArgs = new Dictionary<string, string>
                {
                    {"message", "/leader"}
                };
                string lResponse =
                    await HttpUtility.PostWebRequestResponse(
                        "https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer",
                        this._senpai.LoginCookies,
                        lPostArgs);

                if (Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler))
                {
                    try
                    {
                        Dictionary<string, string> lDict =
                            JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                        if (!lDict["msg"].Equals("Erfolgreich!")) return;
                        User[] lLeiterArray = this.Teilnehmer.Where(
                            x => x.UserName.Equals(lDict["message"].Remove(0, "Konferenzleiter: ".Length)))
                            .ToArray();
                        if (lLeiterArray.Any()) this.Leiter = lLeiterArray[0];
                    }
                    catch (Exception)
                    {
                        this._senpai.ErrHandler.Add(lResponse);
                    }
                }
            }
            else
            {
                this.Leiter = User.System;
            }
        }

        private async Task GetAllParticipants()
        {
            HtmlDocument lDocument = new HtmlDocument();
            string lResponse =
                (await HttpUtility.GetWebRequestResponse("https://proxer.me/messages?id=" + this.Id + "&format=raw",
                    this._senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "");

            if (!Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler)) return;
            try
            {
                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Any()) return;
                HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//div[@id='conferenceUsers']");
                if (lNodes == null) return;
                this.Teilnehmer = new List<User>();

                foreach (HtmlNode curTeilnehmer in lNodes[0].ChildNodes[1].ChildNodes)
                {
                    string lUserName = curTeilnehmer.ChildNodes[1].InnerText;
                    int lUserId =
                        Convert.ToInt32(
                            Utility.GetTagContents(
                                curTeilnehmer.ChildNodes[1].FirstChild.Attributes["href"].Value, "/user/",
                                "#top")[0]);

                    this.Teilnehmer.Add(new User(lUserName, lUserId, this._senpai));
                }
            }
            catch (Exception)
            {
                this._senpai.ErrHandler.Add(lResponse);
            }
        }

        private async Task GetTitle()
        {
            if (this.IsConference)
            {
                Dictionary<string, string> lPostArgs = new Dictionary<string, string>
                {
                    {"message", "/topic"}
                };
                string lResponse =
                    await HttpUtility.PostWebRequestResponse(
                        "https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer",
                        this._senpai.LoginCookies,
                        lPostArgs);

                if (!Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler)) return;
                try
                {
                    Dictionary<string, string> lDict =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                    if (lDict["msg"].Equals("Erfolgreich!")) this.Titel = lDict["message"];
                }
                catch (Exception)
                {
                    this._senpai.ErrHandler.Add(lResponse);
                }
            }
            else
            {
                if (this.Teilnehmer.Count > 1)
                    this.Titel = this.Teilnehmer.Where(x => x.Id != this._senpai.Me.Id).ToArray()[0].UserName;
            }
        }

        private async Task GetAllMessages()
        {
            if (this.Nachrichten != null && this.Nachrichten.Count > 0)
                await this.GetMessages(this.Nachrichten.Last().NachrichtId);
            else if ((this.Nachrichten == null || this.Nachrichten.Count == 0) && this._senpai.LoggedIn)
            {
                string lResponse =
                    await HttpUtility.GetWebRequestResponse(
                        "http://proxer.me/messages?format=json&json=messages&id=" + this.Id, this._senpai.LoginCookies);
                if (lResponse.Equals("{\"uid\":\"" + this._senpai.Me.Id +
                                     "\",\"error\":1,\"msg\":\"Ein Fehler ist passiert.\"}")) return;
                try
                {
                    string lMessagesJson = Utility.GetTagContents(lResponse, "\"messages\":[", "],\"favour")[0];
                    if (lMessagesJson.Equals("")) return;

                    List<Dictionary<string, string>> lMessages =
                        JsonConvert.DeserializeObject<List<Dictionary<string, string>>>("[" + lMessagesJson + "]");

                    this.Nachrichten = new List<Message>();
                    foreach (Dictionary<string, string> curMessage in lMessages)
                    {
                        Message.Action lMessageAction;

                        switch (curMessage["action"])
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
                            this.Teilnehmer.Where(x => x.Id == Convert.ToInt32(curMessage["fromid"])).ToArray();

                        if (lSender.Any())
                            this.Nachrichten.Insert(0,
                                new Message(lSender[0], Convert.ToInt32(curMessage["id"]), curMessage["message"],
                                    Convert.ToInt32(curMessage["timestamp"]), lMessageAction));
                        else
                            this.Nachrichten.Insert(0,
                                new Message(
                                    new User(curMessage["username"], Convert.ToInt32(curMessage["fromid"]),
                                        this._senpai),
                                    Convert.ToInt32(curMessage["id"]), curMessage["message"],
                                    Convert.ToInt32(curMessage["timestamp"]), lMessageAction));
                    }
                    try
                    {
                        if (this.Nachrichten.Count(
                            x => x.Aktion == Message.Action.AddUser || x.Aktion == Message.Action.RemoveUser) > 0)
                            await this.GetAllParticipants();
                        if (this.Nachrichten.Count(x => x.Aktion == Message.Action.SetLeader) > 0 &&
                            this.IstInitialisiert)
                            await this.GetLeader();
                        if (this.Nachrichten.Count(x => x.Aktion == Message.Action.SetTopic) > 0 &&
                            this.IstInitialisiert)
                            await this.GetTitle();
                    }
                    catch (NotLoggedInException)
                    {
                        
                    }
                }
                catch (Exception)
                {
                    this._senpai.ErrHandler.Add(lResponse);
                }

                if (this.NeuePmRaised != null) this.NeuePmRaised(this, this.Nachrichten);
            }
        }

        private async Task GetMessages(int mid)
        {
            if (this.Nachrichten == null || this.Nachrichten.Count(x => x.NachrichtId == mid) == 0)
                await this.GetAllMessages();
            else
            {
                if (!this._senpai.LoggedIn) return;

                string lResponse =
                    await HttpUtility.GetWebRequestResponse(
                        "http://proxer.me/messages?format=json&json=newmessages&id=" + this.Id + "&mid=" + mid,
                        this._senpai.LoginCookies);

                if (lResponse.Equals("{\"uid\":\"" + this._senpai.Me.Id +
                                     "\",\"error\":1,\"msg\":\"Ein Fehler ist passiert.\"}")) return;
                List<Message> lNewMessages = new List<Message>();
                try
                {
                    string lMessagesJson = Utility.GetTagContents(lResponse, "\"messages\":[", "]}")[0];
                    if (lMessagesJson.Equals("")) return;

                    List<Dictionary<string, string>> lMessages =
                        JsonConvert.DeserializeObject<List<Dictionary<string, string>>>("[" + lMessagesJson +
                                                                                        "]");

                    foreach (Dictionary<string, string> curMessage in lMessages)
                    {
                        Message.Action lMessageAction;

                        switch (curMessage["action"])
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
                            this.Teilnehmer.Where(x => x.Id == Convert.ToInt32(curMessage["fromid"])).ToArray();

                        if (lSender.Any())
                            lNewMessages.Insert(0,
                                new Message(lSender[0], Convert.ToInt32(curMessage["id"]), curMessage["message"],
                                    Convert.ToInt32(curMessage["timestamp"]), lMessageAction));
                        else
                            lNewMessages.Insert(0,
                                new Message(
                                    new User(curMessage["username"], Convert.ToInt32(curMessage["fromid"]),
                                        this._senpai), Convert.ToInt32(curMessage["id"]), curMessage["message"],
                                    Convert.ToInt32(curMessage["timestamp"]), lMessageAction));
                    }

                    try
                    {
                        if (
                            lNewMessages.Count(
                                x => x.Aktion == Message.Action.AddUser || x.Aktion == Message.Action.RemoveUser) >
                            0) await this.GetAllParticipants();
                        if (lNewMessages.Count(x => x.Aktion == Message.Action.SetLeader) > 0 &&
                            this.IstInitialisiert)
                            await this.GetLeader();
                        if (lNewMessages.Count(x => x.Aktion == Message.Action.SetTopic) > 0 &&
                            this.IstInitialisiert)
                            await this.GetTitle();
                    }
                    catch (NotLoggedInException)
                    {
                        
                    }

                    this.Nachrichten = this.Nachrichten.Concat(lNewMessages).ToList();
                }
                catch (Exception)
                {
                    this._senpai.ErrHandler.Add(lResponse);
                }

                if (this.NeuePmRaised != null) this.NeuePmRaised(this, lNewMessages);
            }
        }

        private async Task<bool> CheckIsConference()
        {
            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", "/ping"}
            };
            string lResponse =
                await HttpUtility.PostWebRequestResponse(
                    "https://proxer.me/messages?id=" + this.Id + "&format=json&json=answer", this._senpai.LoginCookies,
                    lPostArgs);

            if (!Utility.CheckForCorrectResponse(lResponse, this._senpai.ErrHandler)) return false;
            try
            {
                Dictionary<string, string> lDict =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                return lDict["msg"].Equals("Erfolgreich!") &&
                       !lDict["message"].Equals("Befehle sind nur in Konferenzen verfügbar.");
            }
            catch (Exception)
            {
                this._senpai.ErrHandler.Add(lResponse);
            }

            return false;
        }


        /// <summary>
        ///     Gibt zurück, ob Senpai ein Teilnehmer einer Konferenz mit der bestimmten ID ist
        /// </summary>
        /// <param name="id">ID der Konferenz</param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static async Task<bool> IstTeilnehmner(int id, Senpai senpai)
        {
            Dictionary<string, string> lPostArgs = new Dictionary<string, string>
            {
                {"message", "/ping"}
            };
            string lResponse =
                await HttpUtility.PostWebRequestResponse("https://proxer.me/messages?id=" + id + "&format=json&json=answer",
                    senpai.LoginCookies, lPostArgs);

            if (!Utility.CheckForCorrectResponse(lResponse, senpai.ErrHandler)) return false;
            try
            {
                Dictionary<string, string> lDict =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                return !lDict["msg"].Equals("Ein Fehler ist passiert.");
            }
            catch (Exception)
            {
                senpai.ErrHandler.Add(lResponse);
            }

            return false;
        }

        #endregion

        /// <summary>
        ///     Repräsentiert die jeweilige einzelne Nachricht in der Konferenz
        /// </summary>
        public class Message
        {
            /// <summary>
            /// </summary>
            public enum Action
            {
                /// <summary>
                /// </summary>
                NoAction,

                /// <summary>
                /// </summary>
                AddUser,

                /// <summary>
                /// </summary>
                RemoveUser,

                /// <summary>
                /// </summary>
                SetLeader,

                /// <summary>
                /// </summary>
                SetTopic,

                /// <summary>
                ///     Immer wenn von der JSON direkt etwas zurückgegeben wird z.B. bei /leader
                /// </summary>
                GetAction
            }

            internal Message(User sender, int mid, string nachricht, int unix, Action aktion)
            {
                this.Sender = sender;
                this.NachrichtId = mid;
                this.Nachricht = nachricht;
                this.TimeStamp = Utility.UnixTimeStampToDateTime(unix);
                this.Aktion = aktion;
            }

            internal Message(User sender, int mid, string nachricht, DateTime date, Action aktion)
            {
                this.Sender = sender;
                this.NachrichtId = mid;
                this.Nachricht = nachricht;
                this.TimeStamp = date;
                this.Aktion = aktion;
            }

            #region Properties

            /// <summary>
            /// </summary>
            public Action Aktion { get; private set; }

            /// <summary>
            /// </summary>
            public string Nachricht { get; private set; }

            /// <summary>
            /// </summary>
            public int NachrichtId { get; private set; }

            /// <summary>
            /// </summary>
            public User Sender { get; private set; }

            /// <summary>
            /// </summary>
            public DateTime TimeStamp { get; private set; }

            #endregion
        }
    }
}