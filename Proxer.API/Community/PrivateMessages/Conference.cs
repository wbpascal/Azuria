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

        }

        private Senpai senpai;
        private Timer getMessagesTimer;

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
            this.getMessagesTimer.Interval = (new TimeSpan(0, 0, 10)).TotalMilliseconds;
            this.getMessagesTimer.AutoReset = true;
            this.getMessagesTimer.Elapsed += (s, eArgs) =>
            {

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
        /// 
        /// </summary>
        public List<User> Teilnehmer { get; private set; }

        /// <summary>
        /// 
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
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mid">Die ID der letzten Nachricht</param>
        private async void getMessages(int mid)
        {

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
