using Proxer.API.Notifications.NotificationObjects;
using Proxer.API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Notifications
{
    /// <summary>
    /// 
    /// </summary>
    public class PMNotification : INotification
    {
        private readonly Senpai senpai;
        private PMObject[] updateObjects;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCount"></param>
        /// <param name="senpai"></param>
        public PMNotification(int updateCount, Senpai senpai)
        {
            this.Typ = NotificationType.PrivateMessage;
            this.Count = updateCount;
            this.senpai = senpai;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public NotificationType Typ { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<INotificationObject[]> getUpdates()
        {
            List<PMObject> lReturn = new List<PMObject>();

            if (updateObjects == null && senpai.LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = (await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/messages?format=raw&s=notification", senpai.LoginCookies)).Replace("</link>","");

                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Count() == 0)
                {
                    HtmlAgilityPack.HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//a[@class='conferenceList']");

                    foreach (HtmlAgilityPack.HtmlNode curNode in lNodes)
                    {
                        string lTitel;
                        string[] lDatum;
                        if (curNode.ChildNodes[1].Name.ToLower().Equals("img"))
                        {
                            lTitel = curNode.ChildNodes[4].InnerText;
                            lDatum = curNode.ChildNodes[6].InnerText.Split('.');

                            DateTime lTimeStamp = new DateTime(Convert.ToInt32(lDatum[2]), Convert.ToInt32(lDatum[1]), Convert.ToInt32(lDatum[0]));
                            int lID = Convert.ToInt32(curNode.Attributes["href"].Value.Substring(13, curNode.Attributes["href"].Value.Length - 17));

                            lReturn.Add(new PMObject(lID, lTitel, lTimeStamp));
                        }
                        else
                        {
                            lTitel = curNode.ChildNodes[1].InnerText;
                            lDatum = curNode.ChildNodes[3].InnerText.Split('.');

                            DateTime lTimeStamp = new DateTime(Convert.ToInt32(lDatum[2]), Convert.ToInt32(lDatum[1]), Convert.ToInt32(lDatum[0]));
                            int lID = Convert.ToInt32(curNode.Attributes["href"].Value.Substring(13, curNode.Attributes["href"].Value.Length - 17));

                            lReturn.Add(new PMObject(lTitel, lID, lTimeStamp));
                        }
                    }

                    updateObjects = lReturn.ToArray();
                    return updateObjects;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //kann auch null sein
                return updateObjects;
            }
        }
    }
}
