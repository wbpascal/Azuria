using System;

namespace Proxer.API.Notifications
{
    /// <summary>
    /// </summary>
    public class NewsObject : INotificationObject
    {
        /// <summary>
        /// </summary>
        internal NewsObject()
        {
            this.Type = NotificationObjectType.News;
        }

        #region Properties

        /// <summary>
        ///     Gibt die ID der Kategorie, in der sich die News befindet, zurück.
        /// </summary>
        public int Catid { get; set; }

        /// <summary>
        ///     Gibt den Namen der Kategorie, in der sich die News befindet, zurück.
        /// </summary>
        public string Catname { get; set; }

        /// <summary>
        ///     Gibt eine Kurzbeschreibung der News zurück.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gibt die Aufrufe der News zurück.
        /// </summary>
        public int Hits { get; set; }

        /// <summary>
        ///     Gibt die Bild-ID der News zurück.
        /// </summary>
        public string ImageId { get; set; }

        /// <summary>
        ///     Gibt Angaben zum CSS-Style des Bildes zurück.
        /// </summary>
        public string ImageStyle { get; set; }

        /// <summary>
        /// </summary>
        public string Message
        {
            get { return this.Description; }
        }

        /// <summary>
        /// </summary>
        [Obsolete("Bitte benutzte stattdessen \"thread\"")]
        internal int Mid { get; set; }

        /// <summary>
        ///     Gibt die News-ID zurück.
        /// </summary>
        public int Nid { get; set; }

        /// <summary>
        /// </summary>
        [Obsolete]
        internal int Pid { get; set; }

        /// <summary>
        ///     Gibt die Anzahl der Kommentare zurück.
        /// </summary>
        public int Posts { get; set; }

        /// <summary>
        ///     Gibt die Überschrift der News zurück.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///     Gibt die Thread-ID zurück.
        /// </summary>
        public int Thread { get; set; }

        /// <summary>
        ///     Gibt den Zeitpunkt der News als Unix Timestamp zurück.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// </summary>
        public NotificationObjectType Type { get; private set; }

        /// <summary>
        ///     Gibt die ID des Authors zurück.
        /// </summary>
        public int Uid { get; set; }

        /// <summary>
        ///     Gibt den Benutzernamen des Authors zurück.
        /// </summary>
        public string Uname { get; set; }

        #endregion
    }
}