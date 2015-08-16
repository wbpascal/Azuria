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
    public class NewsObject : INotificationObject
    {
        /// <summary>
        /// (vorläufig?) Benötigt als Dummy
        /// </summary>
        internal NewsObject(Object dummy)
        {
            this.Type = NotificationObjectType.Dummy;
        }
        /// <summary>
        /// 
        /// </summary>
        internal NewsObject()
        {
            this.Type = NotificationObjectType.News;
        }

        /// <summary>
        /// 
        /// </summary>
        public NotificationObjectType Type { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return this.description; }
        }
        /// <summary>
        /// Gibt die News-ID zurück.
        /// </summary>
        public int nid { get; set; }
        /// <summary>
        /// Gibt den Zeitpunkt der News als Unix Timestamp zurück.
        /// </summary>
        public long time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Obsolete("Bitte benutzte stattdessen \"thread\"")]
        internal int mid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Obsolete]
        internal int pid { get; set; }
        /// <summary>
        /// Gibt eine Kurzbeschreibung der News zurück.
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Gibt die Bild-ID der News zurück.
        /// </summary>
        public string image_id { get; set; }
        /// <summary>
        /// Gibt Angaben zum CSS-Style des Bildes zurück.
        /// </summary>
        public string image_style { get; set; }
        /// <summary>
        /// Gibt die Überschrift der News zurück.
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// Gibt die Aufrufe der News zurück.
        /// </summary>
        public int hits { get; set; }
        /// <summary>
        /// Gibt die Thread-ID zurück.
        /// </summary>
        public int thread { get; set; }
        /// <summary>
        /// Gibt die ID des Authors zurück.
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// Gibt den Benutzernamen des Authors zurück.
        /// </summary>
        public string uname { get; set; }
        /// <summary>
        /// Gibt die Anzahl der Kommentare zurück.
        /// </summary>
        public int posts { get; set; }
        /// <summary>
        /// Gibt die ID der Kategorie, in der sich die News befindet, zurück.
        /// </summary>
        public int catid { get; set; }
        /// <summary>
        /// Gibt den Namen der Kategorie, in der sich die News befindet, zurück.
        /// </summary>
        public string catname { get; set; }
    }
}
