using Newtonsoft.Json;
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
    public class NewsNotification : INotification
    {
        private readonly Senpai senpai;
        //nachdem getUpdate() einmal aufgerufen wurde werden die Ergebnisse hier drin gespeichert => weniger Auslastung
        private NewsObject[] updateListe;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCount"></param>
        /// <param name="senpai"></param>
        public NewsNotification(int updateCount, Senpai senpai)
        {
            this.senpai = senpai;
            this.Typ = NotificationType.News;
            this.Count = updateCount;
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
            if (this.updateListe == null && senpai.LoggedIn)
            {
                string lResponse = await HttpUtility.GetWebRequestResponseAsync("http://proxer.me/notifications?format=json&s=news&p=1", senpai.LoginCookies);
                if (lResponse.StartsWith("{\"error\":0"))
                {
                    Dictionary<string, List<NewsObject>> lDeserialized = JsonConvert.DeserializeObject<Dictionary<string, List<NewsObject>>>("{" + lResponse.Substring("{\"error\":0,".Length));

                    if (this.Count < 15)
                    {
                        lDeserialized["notifications"].RemoveRange(this.Count, lDeserialized["notifications"].Count - this.Count);
                        senpai.News.RemoveRange(this.Count, lDeserialized["notifications"].Count - this.Count);
                    }

                    this.updateListe = lDeserialized["notifications"].ToArray();
                    return this.updateListe;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //kann auch null sein
                return this.updateListe;
            }
        }
    }
}
