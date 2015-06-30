using Proxer.API.Utility;
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
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="mid"></param>
            /// <param name="nachricht"></param>
            /// <param name="unix"></param>
            /// <param name="aktion"></param>
            public Message(User sender, int mid, string nachricht, int unix, Action aktion)
            {
                this.Sender = sender;
                this.NachrichtID = mid;
                this.Nachricht = nachricht;
                this.TimeStamp = Utility.Utility.UnixTimeStampToDateTime((long)unix);
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
                AddUser
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
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="id"></param>
        /// <param name="senpai"></param>
        public Conference(string title, int id, Senpai senpai)
        {
            this.Titel = title;
            this.ID = id;
            this.senpai = senpai;

            this.getMessagesTimer = new Timer();
            this.getMessagesTimer.AutoReset = true;
            this.getMessagesTimer.Interval = (new TimeSpan(0, 0, 15)).TotalMilliseconds;
            this.getMessagesTimer.Elapsed += (s, eArgs) =>
            {
                (s as Timer).Stop();
                this.getMessages(this.Nachrichten.Last().NachrichtID);
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Titel { get; private set; }
        /// <summary>
        /// Gibt einen Wert an, ob die Privatnachrichten in einem bestimmten Intervall abgerufen werden, oder legt diesen fest.
        /// </summary>
        public bool Aktiv
        {
            get
            {
                return getMessagesTimer.Enabled;
            }
            set
            {
                if (value)
                {
                    getAllMessages();
                }
                else
                {
                    getMessagesTimer.Stop();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<User> Teilnehmer { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Message> Nachrichten { get; private set; }

        /// <summary>
        /// Initialisiert die 
        /// </summary>
        public async Task initConference()
        {
            if (await istTeilnehmner(this.ID, this.senpai))
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/messages?id=" + this.ID + "&format=raw", senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "");

                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Count() == 0)
                {
                    HtmlAgilityPack.HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//div[@id='conferenceUsers']");
                    if (lNodes != null)
                    {
                        this.Teilnehmer = new List<User>();

                        foreach(HtmlAgilityPack.HtmlNode curTeilnehmer in lNodes[0].ChildNodes[1].ChildNodes)
                        {
                            string lUserName = curTeilnehmer.ChildNodes[1].InnerText;
                            int lUserID = Convert.ToInt32(Utility.Utility.GetTagContents(curTeilnehmer.ChildNodes[1].FirstChild.Attributes["href"].Value, "/user/", "#top")[0]);

                            this.Teilnehmer.Add(new User(lUserName, lUserID, this.senpai));
                        }
                        this.Aktiv = true;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task getAllMessages()
        {
            if (senpai.LoggedIn)
            {
                string lResponse = await HttpUtility.GetWebRequestResponseAsync("http://proxer.me/messages?format=json&json=messages&id=" + this.ID, senpai.LoginCookies);
                if (!lResponse.Equals("{\"uid\":\"" + senpai.Me.ID + "\",\"error\":1,\"msg\":\"Ein Fehler ist passiert.\"}"))
                {
                    string lMessagesJson = Utility.Utility.GetTagContents(lResponse, "\"messages\":[", "],\"favour")[0];
                    if (lMessagesJson.Equals("")) return;

                    List<Dictionary<string, string>> lMessages = await Task.Factory.StartNew(() => Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>("[" + lMessagesJson + "]"));

                    this.Nachrichten = new List<Message>();
                    foreach(Dictionary<string, string> curMessage in lMessages)
                    {
                        User lSender = new User(curMessage["username"], Convert.ToInt32(curMessage["fromid"]), senpai);
                        this.Nachrichten.Insert(0, new Message(lSender, Convert.ToInt32(curMessage["id"]), curMessage["message"], Convert.ToInt32(curMessage["timestamp"]), curMessage["action"].Equals("addUser") ? Message.Action.AddUser : Message.Action.NoAction));
                    }

                    if (NeuePM_Raised != null) NeuePM_Raised.Invoke(this, this.Nachrichten);
                    if (!getMessagesTimer.Enabled) getMessagesTimer.Start();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mid">Die ID der letzten Nachricht</param>
        private async void getMessages(int mid)
        {
            if (this.Nachrichten == null) await this.getAllMessages();
            else
            {
                if (senpai.LoggedIn)
                {
                    string lResponse = await HttpUtility.GetWebRequestResponseAsync("http://proxer.me/messages?format=json&json=newmessages&id=" + this.ID + "&mid=" + mid, senpai.LoginCookies);
                    if (!lResponse.Equals("{\"uid\":\"" + senpai.Me.ID + "\",\"error\":1,\"msg\":\"Ein Fehler ist passiert.\"}"))
                    {
                        string lMessagesJson = Utility.Utility.GetTagContents(lResponse, "\"messages\":[", "]}")[0];
                        if (!lMessagesJson.Equals(""))
                        {
                            List<Dictionary<string, string>> lMessages = await Task.Factory.StartNew(() => Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>("[" + lMessagesJson + "]"));

                            List<Message> lNewMessages = new List<Message>();
                            foreach (Dictionary<string, string> curMessage in lMessages)
                            {
                                User lSender = new User(curMessage["username"], Convert.ToInt32(curMessage["fromid"]), senpai);
                                Message lMessage = new Message(lSender, Convert.ToInt32(curMessage["id"]), curMessage["message"], Convert.ToInt32(curMessage["timestamp"]), curMessage["action"].Equals("addUser") ? Message.Action.AddUser : Message.Action.NoAction);
                                lNewMessages.Insert(0, lMessage);
                            }
                            this.Nachrichten = this.Nachrichten.Concat(lNewMessages).ToList();

                            if (NeuePM_Raised != null) NeuePM_Raised.Invoke(this, lNewMessages);
                        }
                    }
                }
            }

            this.getMessagesTimer.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nachricht"></param>
        public async void sendeNachricht(string nachricht)
        {
            if (senpai.LoggedIn)
            {
                Dictionary<string, string> lPostArgs = new Dictionary<string, string>
                {
                    {"message", nachricht}
                };
                string lResponse = await HttpUtility.PostWebRequestResponseAsync("https://proxer.me/messages?id=" + this.ID + "&format=json&json=answer", senpai.LoginCookies, lPostArgs);

                this.getMessages(this.Nachrichten.Last().NachrichtID);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="senpai"></param>
        /// <returns>Ob der Benutzter zu der Konferenz gehört</returns>
        public async Task<bool> istTeilnehmner(int id, Senpai senpai)
        {
            string lResponse = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/messages?id=" + id + "&format=raw", senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "");

            return !lResponse.Contains("Du hast keine Berechtigungen für diese Konferenz.");
        }
    }
}
