using System;
using Proxer.API.Utilities;

namespace Proxer.API.Notifications
{
    /// <summary>
    ///     Eine Klasse, die eine Nachricht darstellt.
    /// </summary>
    public class NewsObject : INotificationObject
    {
        internal NewsObject()
        {
            this.Type = NotificationObjectType.News;
        }

        #region Geerbt

        /// <summary>
        ///     Gibt die Nachricht der Benachrichtigung als Text zurück.
        ///     <para>(Vererbt von <see cref="INotificationObject" />)</para>
        /// </summary>
        public string Message => this.Description;

        /// <summary>
        ///     Gibt den Typ der Benachrichtigung zurück.
        ///     <para>(Vererbt von <see cref="INotificationObject" />)</para>
        /// </summary>
        public NotificationObjectType Type { get; }

        #endregion

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
        ///     Veraltet. Gibt die Thread-ID zurück.
        /// </summary>
        [Obsolete("Bitte benutzte stattdessen \"thread\"")]
        internal int Mid { get; set; }

        /// <summary>
        ///     Gibt die News-ID zurück.
        /// </summary>
        public int Nid { get; set; }

        /// <summary>
        ///     Veraltet.
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
        ///     Gibt die ID des Authors zurück.
        /// </summary>
        public int Uid { get; set; }

        /// <summary>
        ///     Gibt den Benutzernamen des Authors zurück.
        /// </summary>
        public string Uname { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.Subject + "\n" + Utility.UnixTimeStampToDateTime(this.Time) + "\n" + this.Catname;
        }

        #endregion
    }
}