using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Notifications.NotificationObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class AnimeMangaUpdateObject : INotificationObject
    {
        /// <summary>
        /// (vorläufig?) Benötigt als Dummy
        /// </summary>
        public AnimeMangaUpdateObject(Object dummy)
        {
            this.Typ = NotificationObjectType.Dummy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Die Nachricht des Updates</param>
        public AnimeMangaUpdateObject(string message)
        {
            this.Typ = NotificationObjectType.AnimeManga;
            this.Message = message;
            this.Name = "";
            this.Number = -1;
            this.Link = null;
            this.ID = -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Die Nachricht des Updates</param>
        /// <param name="name">Der Name des Anime/Manga</param>
        /// <param name="number">Die Nummer der Folge/des Kapitels</param>
        /// <param name="link">Der Link zu der Folge/dem Kapitel</param>
        /// <param name="id">Die ID der Benachrichtigung (Wichtig, um sie wieder zu löschen)</param>
        public AnimeMangaUpdateObject(string message, string name, int number, Uri link, int id)
        {
            this.Typ = NotificationObjectType.AnimeManga;
            this.Message = message;
            this.Name = name;
            this.Number = number;
            this.Link = link;
            this.ID = id;
        }

        /// <summary>
        /// 
        /// </summary>
        public NotificationObjectType Typ { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Uri Link { get; private set; }
        /// <summary>
        /// Der Name des Anime/Mangas
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Die Episode/Das Kapitel, das erschienen ist
        /// </summary>
        public int Number { get; private set; }
        /// <summary>
        /// Die ID des Anime/Manga
        /// </summary>
        public int ID { get; private set; }
    }
}
