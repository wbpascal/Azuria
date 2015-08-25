using Newtonsoft.Json;
using Proxer.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Proxer.API.Community.PrivateMessages
{
    /// <summary>
    /// 
    /// </summary>
    public class Conference
    {
        /// <summary>
        /// 
        /// </summary>
        public class Message
        {
            internal Message(User sender, int mid, string nachricht, int unix, Action aktion)
            {
                this.Sender = sender;
                this.NachrichtID = mid;
                this.Nachricht = nachricht;
                this.TimeStamp = Utility.UnixTimeStampToDateTime((long)unix);
                this.Aktion = aktion;
            }
            internal Message(User sender, int mid, string nachricht, DateTime date, Action aktion)
            {
                this.Sender = sender;
                this.NachrichtID = mid;
                this.Nachricht = nachricht;
                this.TimeStamp = date;
                this.Aktion = aktion;
            }

            /// <summary>
            /// 
            /// </summary>
            public User Sender { get; private set; }
            /// <summary>
            /// 
            /// </summary>
            public int NachrichtID { get; private set; }
            /// <summary>
            /// 
            /// </summary>
            public string Nachricht { get; private set; }
            /// <summary>
            /// 
            /// </summary>
            public DateTime TimeStamp { get; private set; }
            /// <summary>
            /// 
            /// </summary>
            public Action Aktion { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public enum Action
            {
                /// <summary>
                /// 
                /// </summary>
                NoAction,
                /// <summary>
                /// 
                /// </summary>
                AddUser,
                /// <summary>
                /// 
                /// </summary>
                RemoveUser,
                /// <summary>
                /// 
                /// </summary>
                SetLeader,
                /// <summary>
                /// 
                /// </summary>
                SetTopic,
                /// <summary>
                /// Immer wenn von der JSON direkt etwas zurückgegeben wird z.B. bei /leader 
                /// </summary>
                GetAction
            }
        }

        private Senpai senpai;
        private Timer getMessagesTimer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Die Konferenz, die das Event aufgerufen hat</param>
        /// <param name="e">Die neuen Nachrichten. Beim ersten mal werden hier alle Nachrichten aufgeführt</param>
        public delegate void NeuePMEventHandler(Conference sender, List<Message> e);
        /// <summary>
        /// Wird immer aufgerufen, wenn neue Nachrichten in der Konferenz vorhanden sind
        /// </summary>
        public event NeuePMEventHandler NeuePM_Raised;

        /// <summary>
        /// Standard-Konstruktor der Klasse
        /// </summary>
        /// <param name="id"></param>
        /// <param name="senpai"></param>
        public Conference(int id, Senpai senpai)
        {
            this.ID = id;
            this.senpai = senpai;

            this.getMessagesTimer = new Timer();
            this.getMessagesTimer.Interval = (new TimeSpan(0, 0, 15)).TotalMilliseconds;
            this.getMessagesTimer.Elapsed += (s, eArgs) =>
            {
                this.getMessagesTimer.Interval = (new TimeSpan(0, 0, 15)).TotalMilliseconds;
                (s as Timer).Stop();
                if (this.IstInitialisiert)
                {
                    if (this.Nachrichten != null && this.Nachrichten.Count() > 0) this.getMessages(this.Nachrichten.Last().NachrichtID);
                    else this.getAllMessages();
                }
                (s as Timer).Start();
            };

            this.Aktiv = false;

            this.initConference();
        }

        internal Conference(string title, int id, Senpai senpai)
        {
            this.Titel = title;
            this.ID = id;
            this.senpai = senpai;

            this.getMessagesTimer = new Timer();
            this.getMessagesTimer.Interval = (new TimeSpan(0, 0, 15)).TotalMilliseconds;
            this.getMessagesTimer.Elapsed += (s, eArgs) =>
            {
                this.getMessagesTimer.Interval = (new TimeSpan(0, 0, 15)).TotalMilliseconds;
                (s as Timer).Stop();
                if (this.IstInitialisiert)
                {
                    if (this.Nachrichten != null && this.Nachrichten.Count() > 0) this.getMessages(this.Nachrichten.Last().NachrichtID);
                    else this.getAllMessages();
                }
                (s as Timer).Start();
            };

            this.Aktiv = false;
        }

        /// <summary>
        /// Gibt die ID der Konferenz zurück
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// Gibt den Titel der Konferenz zurück (initConference() muss dafür zunächst einmal aufgerufen werden)
        /// </summary>
        public string Titel { get; private set; }
        /// <summary>
        /// Gibt einen Wert an, ob die Privatnachrichten in einem bestimmten Intervall abgerufen werden, oder legt diesen fest 
        /// </summary>
        public bool Aktiv
        {
            get
            {
                return this.getMessagesTimer.Enabled;
            }
            set
            {
                if (value)
                {
                    this.getMessagesTimer.Interval = 1;
                    this.getMessagesTimer.Start();
                }
                else
                {
                    this.getMessagesTimer.Stop();
                }
            }
        }
        /// <summary>
        /// Gibt alle Teilnehmer der Konferenz zurück (initConference() muss dafür zunächst einmal aufgerufen werden)
        /// </summary>
        public List<User> Teilnehmer { get; private set; }
        /// <summary>
        /// Gibt alle Nachrichten aus der Konferenz in chronoligischer Ordnung zurück.
        /// </summary>
        public List<Message> Nachrichten { get; private set; }
        /// <summary>
        /// Gibt den Leiter der Konferenz zurück (initConference() muss dafür zunächst einmal aufgerufen werden)
        /// </summary>
        public User Leiter { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IstInitialisiert { get; private set; }


        private bool IsConference { get; set; }


        /// <summary>
        /// Initialisiert die Konferenz
        /// </summary>
        public void initConference()
        {
            if (this.senpai.LoggedIn && istTeilnehmner(this.ID, this.senpai))
            {
                this.IsConference = this.isConference();
                this.getAllParticipants();
                this.getLeader();
                this.getTitle();

                this.IstInitialisiert = true;
            }
        }
        /// <summary>
        /// Sendet eine Nachricht an die Konferenz
        /// </summary>
        /// <param name="nachricht">Die Nachricht, die gesendet werden soll</param>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public bool sendeNachricht(string nachricht)
        {
            if (senpai.LoggedIn)
            {
                this.getMessagesTimer.Stop();

                Dictionary<string, string> lPostArgs = new Dictionary<string, string>
                {
                    {"message", nachricht}
                };
                string lResponse = HttpUtility.PostWebRequestResponse("https://proxer.me/messages?id=" + this.ID + "&format=json&json=answer", senpai.LoginCookies, lPostArgs);

                if (Utilities.Utility.checkForCorrectResponse(lResponse, senpai.ErrHandler))
                {
                    try
                    {
                        Dictionary<string, string> lResponseJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);

                        if (lResponseJson.Keys.Contains("message"))
                        {
                            if (this.NeuePM_Raised != null) this.NeuePM_Raised(this, new List<Message> { new Message(User.System, -1, lResponseJson["message"], DateTime.Now, Message.Action.GetAction) });
                            return true;
                        }
                        else if (lResponseJson["msg"].Equals("Erfolgreich!"))
                        {
                            this.getMessages(this.Nachrichten.Last().NachrichtID);
                            this.getMessagesTimer.Start();
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        this.senpai.ErrHandler.add(lResponse);
                    }
                }

                this.getMessagesTimer.Start();
            }

            return false;
        }
        /// <summary>
        /// Markiert die Konferenz als ungelesen 
        /// </summary>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public bool alsUngelesenMarkieren()
        {
            if (senpai.LoggedIn)
            {
                string lResponse = HttpUtility.GetWebRequestResponse("http://proxer.me/messages?format=json&json=setUnread&id=" + this.ID, senpai.LoginCookies);
                if (lResponse.StartsWith("{\"error\":0"))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Markiert die Konferenz als Favorit 
        /// </summary>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public bool favoritHinzufuegen()
        {
            if (senpai.LoggedIn)
            {
                string lResponse = HttpUtility.GetWebRequestResponse("http://proxer.me/messages?format=json&json=favour&id=" + this.ID, senpai.LoginCookies);
                if (lResponse.StartsWith("{\"error\":0"))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Entfernt die Konferenz aus den Favoriten
        /// </summary>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public bool favoritEntfernen()
        {
            if (senpai.LoggedIn)
            {
                string lResponse = HttpUtility.GetWebRequestResponse("http://proxer.me/messages?format=json&json=unfavour&id=" + this.ID, senpai.LoginCookies);
                if (lResponse.StartsWith("{\"error\":0"))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Blockiert die Konferenz
        /// </summary>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public bool blockHinzufuegen()
        {
            if (senpai.LoggedIn)
            {
                string lResponse = HttpUtility.GetWebRequestResponse("http://proxer.me/messages?format=json&json=block&id=" + this.ID, senpai.LoginCookies);
                if (lResponse.StartsWith("{\"error\":0"))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Entblockt die Konferenz
        /// </summary>
        /// <returns>Gibt zurück, ob die Aktion erfolgreich war</returns>
        public bool blockEntfernen()
        {
            if (senpai.LoggedIn)
            {
                string lResponse = HttpUtility.GetWebRequestResponse("http://proxer.me/messages?format=json&json=unblock&id=" + this.ID, senpai.LoginCookies);
                if (lResponse.StartsWith("{\"error\":0"))
                {
                    return true;
                }
            }

            return false;
        }


        private void getLeader()
        {
            if (this.IsConference)
            {
                Dictionary<string, string> lPostArgs = new Dictionary<string, string>()
                {
                    {"message", "/leader"},
                };
                string lResponse = HttpUtility.PostWebRequestResponse("https://proxer.me/messages?id=" + this.ID + "&format=json&json=answer", senpai.LoginCookies, lPostArgs);

                if (Utilities.Utility.checkForCorrectResponse(lResponse, senpai.ErrHandler))
                {
                    try
                    {
                        Dictionary<string, string> lDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                        if (lDict["msg"].Equals("Erfolgreich!"))
                        {
                            User[] lLeiterArray = this.Teilnehmer.Where(x => x.UserName.Equals(lDict["message"].Remove(0, "Konferenzleiter: ".Count()))).ToArray();
                            if (lLeiterArray.Count() > 0) this.Leiter = lLeiterArray[0];
                        }
                    }
                    catch (Exception)
                    {
                        this.senpai.ErrHandler.add(lResponse);
                    }
                }
            }
            else
            {
                this.Leiter = User.System;
            }
        }
        private void getAllParticipants()
        {
            HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
            string lResponse = HttpUtility.GetWebRequestResponse("https://proxer.me/messages?id=" + this.ID + "&format=raw", senpai.LoginCookies).Replace("</link>", "").Replace("\n", "");

            if (Utilities.Utility.checkForCorrectResponse(lResponse, senpai.ErrHandler))
            {
                try
                {
                    lDocument.LoadHtml(lResponse);

                    if (lDocument.ParseErrors.Count() == 0)
                    {
                        HtmlAgilityPack.HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//div[@id='conferenceUsers']");
                        if (lNodes != null)
                        {
                            this.Teilnehmer = new List<User>();

                            foreach (HtmlAgilityPack.HtmlNode curTeilnehmer in lNodes[0].ChildNodes[1].ChildNodes)
                            {
                                string lUserName = curTeilnehmer.ChildNodes[1].InnerText;
                                int lUserID = Convert.ToInt32(Utilities.Utility.GetTagContents(curTeilnehmer.ChildNodes[1].FirstChild.Attributes["href"].Value, "/user/", "#top")[0]);

                                this.Teilnehmer.Add(new User(lUserName, lUserID, this.senpai));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    this.senpai.ErrHandler.add(lResponse);
                }
            }

        }
        private void getTitle()
        {
            if (this.IsConference)
            {
                Dictionary<string, string> lPostArgs = new Dictionary<string, string>()
                {
                    {"message", "/topic"},
                };
                string lResponse = HttpUtility.PostWebRequestResponse("https://proxer.me/messages?id=" + this.ID + "&format=json&json=answer", senpai.LoginCookies, lPostArgs);

                if (Utilities.Utility.checkForCorrectResponse(lResponse, senpai.ErrHandler))
                {
                    try
                    {
                        Dictionary<string, string> lDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                        if (lDict["msg"].Equals("Erfolgreich!")) this.Titel = lDict["message"];
                    }
                    catch (Exception)
                    {
                        this.senpai.ErrHandler.add(lResponse);
                    }
                }
            }
            else
            {
                if(this.Teilnehmer.Count > 1) this.Titel = this.Teilnehmer.Where(x => x.ID != this.senpai.Me.ID).ToArray()[0].UserName;
            }
        }
        private void getAllMessages()
        {
            if (this.Nachrichten != null && this.Nachrichten.Count > 0) this.getMessages(this.Nachrichten.Last().NachrichtID);
            else if ((this.Nachrichten == null || this.Nachrichten.Count == 0) && senpai.LoggedIn)
            {
                string lResponse = HttpUtility.GetWebRequestResponse("http://proxer.me/messages?format=json&json=messages&id=" + this.ID, senpai.LoginCookies);
                if (!lResponse.Equals("{\"uid\":\"" + senpai.Me.ID + "\",\"error\":1,\"msg\":\"Ein Fehler ist passiert.\"}"))
                {
                    try
                    {
                        string lMessagesJson = Utilities.Utility.GetTagContents(lResponse, "\"messages\":[", "],\"favour")[0];
                        if (lMessagesJson.Equals("")) return;

                        List<Dictionary<string, string>> lMessages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>("[" + lMessagesJson + "]");

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

                            User[] lSender = this.Teilnehmer.Where(x => x.ID == Convert.ToInt32(curMessage["fromid"])).ToArray();

                            if(lSender != null && lSender.Count() > 0) this.Nachrichten.Insert(0, new Message(lSender[0], Convert.ToInt32(curMessage["id"]), curMessage["message"], Convert.ToInt32(curMessage["timestamp"]), lMessageAction));
                            else this.Nachrichten.Insert(0, new Message(new User(curMessage["username"], Convert.ToInt32(curMessage["fromid"]), senpai), Convert.ToInt32(curMessage["id"]), curMessage["message"], Convert.ToInt32(curMessage["timestamp"]), lMessageAction));
                        }
                        if (this.Nachrichten.Count(x => x.Aktion == Message.Action.AddUser || x.Aktion == Message.Action.RemoveUser) > 0) this.getAllParticipants();
                        if (this.Nachrichten.Count(x => x.Aktion == Message.Action.SetLeader) > 0 && this.IstInitialisiert) this.getLeader();
                        if (this.Nachrichten.Count(x => x.Aktion == Message.Action.SetTopic) > 0 && this.IstInitialisiert) this.getTitle();
                    }
                    catch (Exception)
                    {
                        this.senpai.ErrHandler.add(lResponse);
                    }

                    if (NeuePM_Raised != null) NeuePM_Raised.Invoke(this, this.Nachrichten);
                }
            }
        }
        private void getMessages(int mid)
        {
            if (this.Nachrichten == null || this.Nachrichten.Count(x => x.NachrichtID == mid) == 0) this.getAllMessages();
            else
            {
                if (senpai.LoggedIn)
                {
                    string lResponse = HttpUtility.GetWebRequestResponse("http://proxer.me/messages?format=json&json=newmessages&id=" + this.ID + "&mid=" + mid, senpai.LoginCookies);
                    if (!lResponse.Equals("{\"uid\":\"" + senpai.Me.ID + "\",\"error\":1,\"msg\":\"Ein Fehler ist passiert.\"}"))
                    {
                        List<Message> lNewMessages = new List<Message>();
                        try
                        {
                            string lMessagesJson = Utilities.Utility.GetTagContents(lResponse, "\"messages\":[", "]}")[0];
                            if (lMessagesJson.Equals("")) return;

                            List<Dictionary<string, string>> lMessages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>("[" + lMessagesJson + "]");

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

                                User[] lSender = this.Teilnehmer.Where(x => x.ID == Convert.ToInt32(curMessage["fromid"])).ToArray();

                                if (lSender != null && lSender.Count() > 0) lNewMessages.Insert(0, new Message(lSender[0], Convert.ToInt32(curMessage["id"]), curMessage["message"], Convert.ToInt32(curMessage["timestamp"]), lMessageAction));
                                else lNewMessages.Insert(0, new Message(new User(curMessage["username"], Convert.ToInt32(curMessage["fromid"]), senpai), Convert.ToInt32(curMessage["id"]), curMessage["message"], Convert.ToInt32(curMessage["timestamp"]), lMessageAction));
                            }

                            if (lNewMessages.Count(x => x.Aktion == Message.Action.AddUser || x.Aktion == Message.Action.RemoveUser) > 0) this.getAllParticipants();
                            if (lNewMessages.Count(x => x.Aktion == Message.Action.SetLeader) > 0 && this.IstInitialisiert) this.getLeader();
                            if (lNewMessages.Count(x => x.Aktion == Message.Action.SetTopic) > 0 && this.IstInitialisiert) this.getTitle();

                            this.Nachrichten = this.Nachrichten.Concat(lNewMessages).ToList();


                        }
                        catch (Exception)
                        {
                            this.senpai.ErrHandler.add(lResponse);
                        }

                        if (NeuePM_Raised != null) NeuePM_Raised.Invoke(this, lNewMessages);
                    }
                }
            }
        }
        private bool isConference()
        {
            Dictionary<string, string> lPostArgs = new Dictionary<string, string>()
            {
                {"message", "/ping"},
            };
            string lResponse = HttpUtility.PostWebRequestResponse("https://proxer.me/messages?id=" + this.ID + "&format=json&json=answer", this.senpai.LoginCookies, lPostArgs);

            if (Utilities.Utility.checkForCorrectResponse(lResponse, senpai.ErrHandler))
            {
                try
                {
                    Dictionary<string, string> lDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                    return lDict["msg"].Equals("Erfolgreich!") && !lDict["message"].Equals("Befehle sind nur in Konferenzen verfügbar.");
                }
                catch (Exception)
                {
                    this.senpai.ErrHandler.add(lResponse);
                }
            }

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool istTeilnehmner(int id, Senpai senpai)
        {
            Dictionary<string, string> lPostArgs = new Dictionary<string, string>()
            {
                {"message", "/ping"},
            };
            string lResponse = HttpUtility.PostWebRequestResponse("https://proxer.me/messages?id=" + id + "&format=json&json=answer", senpai.LoginCookies, lPostArgs);

            if (Utilities.Utility.checkForCorrectResponse(lResponse, senpai.ErrHandler))
            {
                try
                {
                    Dictionary<string, string> lDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(lResponse);
                    return !lDict["msg"].Equals("Ein Fehler ist passiert.");
                }
                catch (Exception)
                {
                    senpai.ErrHandler.add(lResponse);
                }
            }

            return false;
        }
    }
}
