using InfiniteSoul.Utilities;
using Nito.AsyncEx;
using Proxer.API.Utilities;
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
        /// <summary>
        /// Representiert das System
        /// </summary>
        public static User System = new User("System", -1, new Senpai());

        private Senpai senpai;

        private string status;
        private bool online;
        private string rang;
        private int punkte;
        private List<User> freunde;
        private Uri avatar;
        private string info;

        private bool checkMain;
        private bool checkFriends;
        private bool checkInfo;

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
            this.checkFriends = true;
            this.checkInfo = true;

            this.UserName = name;
            this.ID = userID;
            this.Avatar = new Uri("https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");

            this.checkTimer = new Timer();
            this.checkTimer.AutoReset = true;
            this.checkTimer.Interval = (new TimeSpan(0, 5, 0)).TotalMilliseconds;
            this.checkTimer.Elapsed += (s, eArgs) =>
            {
                this.checkMain = true;
                this.checkFriends = true;
                this.checkInfo = true;
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userID"></param>
        /// <param name="avatar"></param>
        /// <param name="senpai"></param>
        public User(string name, int userID, Uri avatar, Senpai senpai)
        {
            this.senpai = senpai;

            this.checkMain = true;
            this.checkFriends = true;
            this.checkInfo = true;

            this.UserName = name;
            this.ID = userID;
            if (avatar != null) this.Avatar = avatar;
            else this.Avatar = new Uri("https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");

            this.checkTimer = new Timer();
            this.checkTimer.AutoReset = true;
            this.checkTimer.Interval = (new TimeSpan(0, 5, 0)).TotalMilliseconds;
            this.checkTimer.Elapsed += (s, eArgs) =>
            {
                this.checkMain = true;
                this.checkFriends = true;
                this.checkInfo = true;
            };
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
        /// 
        /// </summary>
        public List<User> Freunde
        {
            get
            {
                if (this.checkFriends) AsyncContext.Run(() => getFriends());
                return this.freunde;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Uri Avatar
        {
            get
            {
                if (this.checkFriends) AsyncContext.Run(() => getMainInfo());
                return this.avatar;
            }
            private set
            {
                if (value != null) this.avatar = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Info
        {
            get
            {
                if (this.checkInfo) AsyncContext.Run(() => this.getInfos());
                return this.info;
            }
            private set
            {
                if (value != null) this.info = value;
            }
        }


        /// <summary>
        /// (vorläufig, nicht ausführlich getestet)
        /// </summary>
        private async Task getMainInfo()
        {
            if (this.ID == -1)
            {
                this.status = "";
                this.online = false;
                this.rang = "";
                this.punkte = -1;
                this.Avatar = new Uri("https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");

                this.checkMain = false;
            }
            else if (senpai.LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/user/" + this.ID + "/overview?format=raw", this.senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "");

                if (Utilities.Utility.checkForCorrectResponse(lResponse, senpai.ErrHandler))
                {
                    try
                    {
                        lDocument.LoadHtml(lResponse);

                        if (lDocument.ParseErrors.Count() == 0)
                        {
                            HtmlAgilityPack.HtmlNodeCollection lProfileNodes = lDocument.DocumentNode.SelectNodes("//table[@class='profile']");

                            if (lProfileNodes != null)
                            {
                                this.Avatar = new Uri("https://proxer.me" + lProfileNodes[0].ParentNode.ChildNodes[1].ChildNodes[0].Attributes["src"].Value);
                                this.punkte = Convert.ToInt32(Utilities.Utility.GetTagContents(lProfileNodes[0].FirstChild.InnerText, "Summe: ", " - ")[0]);
                                this.rang = Utilities.Utility.GetTagContents(lProfileNodes[0].FirstChild.InnerText, this.punkte.ToString() + " - ", "[?]")[0];
                                this.online = lProfileNodes[0].ChildNodes[1].InnerText.Equals("Status Online");
                                if (lProfileNodes[0].ChildNodes.Count() == 7) this.status = lProfileNodes[0].ChildNodes[6].InnerText;
                                else this.status = "";

                                if (this.UserName.Equals(""))
                                {
                                    this.UserName = lDocument.DocumentNode.SelectNodes("//div[@id='pageMetaAjax']")[0].InnerText.Split(' ')[1];
                                }

                                this.checkMain = false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        this.senpai.ErrHandler.add(lResponse);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task getFriends()
        {
            if (this.ID == -1)
            {
                this.freunde = new List<User>();

                this.checkFriends = false;
            }
            else if (senpai.LoggedIn)
            {
                int lSeite = 1;
                string lResponse;
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();

                this.freunde = new List<User>();

                while (!(lResponse = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/user/" + this.ID + "/connections/" + lSeite + "?format=raw", this.senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "").Replace("\t", "")).Contains("Dieser Benutzer hat bisher keine Freunde"))
                {
                    if (Utilities.Utility.checkForCorrectResponse(lResponse, senpai.ErrHandler))
                    {
                        try
                        {
                            lDocument.LoadHtml(lResponse);

                            if (lDocument.ParseErrors.Count() > 0) lDocument.LoadHtml(Utilities.Utility.tryFixParseErrors(lResponse, lDocument.ParseErrors));

                            if (lDocument.ParseErrors.Count() == 0)
                            {
                                HtmlAgilityPack.HtmlNodeCollection lProfileNodes = lDocument.DocumentNode.SelectNodes("//table[@id='box-table-a']");

                                if (lProfileNodes != null)
                                {
                                    lProfileNodes[0].ChildNodes.Remove(0);
                                    foreach (HtmlAgilityPack.HtmlNode curFriendNode in lProfileNodes[0].ChildNodes)
                                    {
                                        string lUsername = curFriendNode.ChildNodes[2].InnerText;
                                        int lID = Convert.ToInt32(curFriendNode.Attributes["id"].Value.Substring("entry".Length));
                                        this.freunde.Add(new User(lUsername, lID, this.senpai));
                                    }
                                }
                            }

                            lSeite++;
                        }
                        catch (Exception)
                        {
                            this.senpai.ErrHandler.add(lResponse);
                        }
                    }
                }

                this.checkFriends = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private async Task getInfos()
        {
            if (this.ID == -1)
            {
                this.Info = "";

                this.checkInfo = false;
            }
            else if (senpai.LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/user/" + this.ID + "/about?format=raw", this.senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "");

                if (Utilities.Utility.checkForCorrectResponse(lResponse, senpai.ErrHandler))
                {
                    try
                    {
                        lDocument.LoadHtml(lResponse);

                        if (lDocument.ParseErrors.Count() == 0)
                        {
                            HtmlAgilityPack.HtmlNodeCollection lProfileNodes = lDocument.DocumentNode.SelectNodes("//table[@class='profile']");

                            if (lProfileNodes != null)
                            {
                                this.Info = lProfileNodes[0].ChildNodes[10].InnerText;

                                this.checkInfo = false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        this.senpai.ErrHandler.add(lResponse);
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
        public async Task<string> getUNameFromID(int id, System.Net.CookieContainer loginCookies)
        {
            HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
            string lResponse = await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/user/" + id + "/overview?format=raw", loginCookies);

            if (Utilities.Utility.checkForCorrectResponse(lResponse, senpai.ErrHandler))
            {
                try
                {
                    lDocument.LoadHtml(lResponse);

                    if (lDocument.ParseErrors.Count() == 0)
                    {
                        HtmlAgilityPack.HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//div[@id='pageMetaAjax']");
                        if (lNodes != null) return lNodes[0].InnerText.Split(' ')[1];
                        else return "";
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception)
                {
                    this.senpai.ErrHandler.add(lResponse);
                }
            }

            return "";
        }


        /*
         * -----------------------------------
         * --------Statische Methoden---------
         * -----------------------------------
        */

        /// <summary>
        /// Überprüft, ob zwei Benutzter Freunde sind.
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        public static bool isUserFriendOf(User user1, User user2)
        {
            return user1.Freunde.Any(item => item.ID == user2.ID);
        }
    }
}
