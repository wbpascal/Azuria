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

        private readonly Senpai senpai;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userID"></param>
        /// <param name="senpai"></param>
        internal User(string name, int userID, Senpai senpai)
        {
            this.senpai = senpai;

            this.UserName = name;
            this.ID = userID;
            this.Avatar = new Uri("https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userID"></param>
        /// <param name="avatar"></param>
        /// <param name="senpai"></param>
        internal User(string name, int userID, Uri avatar, Senpai senpai)
        {
            this.senpai = senpai;

            this.UserName = name;
            this.ID = userID;
            if (avatar != null) this.Avatar = avatar;
            else this.Avatar = new Uri("https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="senpai"></param>
        public User(int userID, Senpai senpai)
        {
            this.senpai = senpai;

            this.UserName = User.getUNameFromID(userID, senpai);
            this.ID = userID;
            this.Avatar = new Uri("https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
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
        public string Status { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Online { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Rang { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Punkte { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public List<User> Freunde { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Uri Avatar { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Info { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void initUser()
        {
            this.getMainInfo();
            this.getFriends();
            this.getInfos();
        }
        

        private void getMainInfo()
        {
            if (this.ID == -1)
            {
                this.Status = "";
                this.Online = false;
                this.Rang = "";
                this.Punkte = -1;
                this.Avatar = new Uri("https://proxer.me/components/com_comprofiler/plugin/templates/default/images/avatar/nophoto_n.png");
            }
            else if (senpai.LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (HttpUtility.GetWebRequestResponse("https://proxer.me/user/" + this.ID + "/overview?format=raw", this.senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "");

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
                                this.Avatar = new Uri("https://proxer.me" + lProfileNodes[0].ParentNode.ParentNode.ChildNodes[1].ChildNodes[0].Attributes["src"].Value);
                                this.Punkte = Convert.ToInt32(Utilities.Utility.GetTagContents(lProfileNodes[0].FirstChild.InnerText, "Summe: ", " - ")[0]);
                                this.Rang = Utilities.Utility.GetTagContents(lProfileNodes[0].FirstChild.InnerText, this.Punkte.ToString() + " - ", "[?]")[0];
                                this.Online = lProfileNodes[0].ChildNodes[1].InnerText.Equals("Status Online");
                                if (lProfileNodes[0].ChildNodes.Count() == 7) this.Status = lProfileNodes[0].ChildNodes[6].InnerText;
                                else this.Status = "";

                                if (this.UserName.Equals(""))
                                {
                                    this.UserName = lDocument.DocumentNode.SelectNodes("//div[@id='pageMetaAjax']")[0].InnerText.Split(' ')[1];
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
        }
        private void getFriends()
        {
            if (this.ID == -1)
            {
                this.Freunde = new List<User>();
            }
            else if (senpai.LoggedIn)
            {
                int lSeite = 1;
                string lResponse;
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();

                this.Freunde = new List<User>();

                while (!(lResponse = (HttpUtility.GetWebRequestResponse("https://proxer.me/user/" + this.ID + "/connections/" + lSeite + "?format=raw", this.senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "").Replace("\t", "")).Contains("Dieser Benutzer hat bisher keine Freunde"))
                {
                    if (Utilities.Utility.checkForCorrectResponse(lResponse, senpai.ErrHandler))
                    {
                        if (lResponse.Equals("<div class=\"inner\"><h3>Du hast keine Berechtigung um diese Seite zu betreten.</h3></div>")) break;

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
                                        this.Freunde.Add(new User(lUsername, lID, this.senpai));
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
            }
        }
        private void getInfos()
        {
            if (this.ID == -1)
            {
                this.Info = "";
            }
            else if (senpai.LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (HttpUtility.GetWebRequestResponse("https://proxer.me/user/" + this.ID + "/about?format=raw", this.senpai.LoginCookies)).Replace("</link>", "").Replace("\n", "");

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="senpai"></param>
        /// <returns></returns>
        public static string getUNameFromID(int id, Senpai senpai)
        {
            HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
            string lResponse = HttpUtility.GetWebRequestResponse("https://proxer.me/user/" + id + "/overview?format=raw", senpai.LoginCookies);

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
                    senpai.ErrHandler.add(lResponse);
                }
            }

            return "";
        }
    }
}
