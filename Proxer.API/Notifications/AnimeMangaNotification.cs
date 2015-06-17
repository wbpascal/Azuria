using Proxer.API.Notifications.NotificationObjects;
using Proxer.API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Notifications
{
    /// <summary>
    /// 
    /// </summary>
    public class AnimeMangaNotification : INotification
    {
        private readonly Senpai senpai;
        private AnimeMangaUpdateObject[] updateObjects;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCount"></param>
        /// <param name="senpai"></param>
        public AnimeMangaNotification(int updateCount, Senpai senpai)
        {
            this.Typ = NotificationType.AnimeManga;
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
        /// Gibt die Updates der Benachrichtigungen in einem Array zurück
        /// </summary>
        /// <returns>UpdateObject-Array</returns>
        public async Task<INotificationObject[]> getUpdates()
        {
            List<AnimeMangaUpdateObject> lReturn = new List<AnimeMangaUpdateObject>(this.Count);
            if (this.updateObjects == null && senpai.LoggedIn)
            {
                HtmlAgilityPack.HtmlDocument lDocument = new HtmlAgilityPack.HtmlDocument();
                string lResponse = await HttpUtility.GetWebRequestResponseAsync("https://proxer.me/components/com_proxer/misc/notifications_misc.php", senpai.LoginCookies);

                lDocument.LoadHtml(lResponse);

                if (lDocument.ParseErrors.Count() == 0)
                {
                    HtmlAgilityPack.HtmlNodeCollection lNodes = lDocument.DocumentNode.SelectNodes("//a[@class='notificationList']");

                    for (int i = 0; i < this.Count; i++)
                    {
                        HtmlAgilityPack.HtmlNode curNode;

                        //falls zwischen abrufen der Infos und der Benachrichtigung sich die Zahl geändert hat (kleiner geworden)
                        try { curNode = lNodes[i]; }
                        catch (Exception) { break; }

                        string lName;
                        int lNumber;

                        int lID = Convert.ToInt32(curNode.Id.Substring(12));
                        string lMessage = curNode.ChildNodes["u"].InnerText;
                        Uri lLink = new Uri("http://proxer.me" + curNode.Attributes["href"].Value);

                        if (lMessage.IndexOf('#') != -1)
                        {
                            lName = lMessage.Split('#')[0];
                            if (!Int32.TryParse(lMessage.Split('#')[1], out lNumber)) lNumber = -1;
                        }
                        else
                        {
                            lName = "";
                            lNumber = -1;
                        }

                        AnimeMangaUpdateObject curObject = new AnimeMangaUpdateObject(lMessage, lName, lNumber, lLink, lID);
                        senpai.AnimeMangaUpdates.Add(curObject);
                        lReturn.Add(curObject);
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return updateObjects;
            }

            this.updateObjects = lReturn.ToArray();
            return this.updateObjects;
        }
    }
}
