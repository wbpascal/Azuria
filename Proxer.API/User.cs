using Nito.AsyncEx;
using Proxer.API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Proxer.API
{
    /// <summary>
    /// 
    /// </summary>
    public class User
    {
        private Senpai senpai;

        private string status;
        private bool online;
        private string rang;
        private int punkte;

        private bool checkMain;
        //private bool checkInfos;
        //private bool checkFriendList;
        //private bool checkFriend;

        private Timer checkTimer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userID"></param>
        /// <param name="senpai"></param>
        public User(string name, int userID, Senpai senpai)
        {
            this.senpai = senpai;

            this.checkMain = true;

            this.UserName = name;
            this.ID = userID;

            this.checkTimer = new Timer();
            this.checkTimer.AutoReset = true;
            this.checkTimer.Interval = (new TimeSpan(0, 30, 0)).TotalMilliseconds;
            this.checkTimer.Elapsed += (s, eArgs) =>
            {
                this.checkMain = true;
                //checkInfos = true;
                //checkFriendList = true;
                //checkFriend = true;
            };

            AsyncContext.Run(() => this.getMainInfo());
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            get
            {
                if (checkMain) AsyncContext.Run(() => getMainInfo());
                return this.status;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Online
        {
            get
            {
                if (checkMain) AsyncContext.Run(() => getMainInfo());
                return this.online;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Rang
        {
            get
            {
                if (checkMain) AsyncContext.Run(() => getMainInfo());
                return this.rang;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Punkte
        {
            get
            {
                if (checkMain) AsyncContext.Run(() => getMainInfo());
                return this.punkte;
            }
        }


        /// <summary>
        /// (vorläufig, nicht ausführlich getestet)
        /// </summary>
        public async Task getMainInfo()
        {
            HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
            string lResponse = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/user/" + this.ID + "/overview?format=raw", this.senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "");

            lDocument.LoadHtml(lResponse);

            if (lDocument.ParseErrors.Count() == 0)
            {
                HtmlAgilityPack.HtmlNodeCollection lProfileNodes = lDocument.DocumentNode.SelectNodes("//table[@class='profile']");

                if (lProfileNodes != null)
                {
                    this.punkte = Convert.ToInt32(Utility.Utility.GetTagContents(lProfileNodes[0].FirstChild.InnerText, "Summe: ", " - ")[0]);
                    this.rang = Utility.Utility.GetTagContents(lProfileNodes[0].FirstChild.InnerText, this.punkte.ToString() + " - ", "[?]")[0];
                    this.online = lProfileNodes[0].ChildNodes[1].InnerText.Equals("Status Online");
                    this.status = lProfileNodes[0].ChildNodes[6].InnerText;

                    if (this.UserName.Equals(""))
                    {
                        this.UserName = lDocument.DocumentNode.SelectNodes("//div[@id='pageMetaAjax']")[0].InnerText.Split(' ')[1];
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginCookies"></param>
        /// <returns></returns>
        public static async Task<string> getUNameFromID(int id, System.Net.CookieContainer loginCookies)
        {
            HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
            string lResponse = await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/user/" + id + "/overview?format=raw", loginCookies);

            lDocument.LoadHtml(lResponse);

            if (lDocument.ParseErrors.Count() == 0)
            {
                try
                {
                    return lDocument.DocumentNode.SelectNodes("//div[@id='pageMetaAjax']")[0].InnerText.Split(' ')[1];
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
    }
}
