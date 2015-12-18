using System;
using Azuria.Utilities;
using Newtonsoft.Json;

namespace Azuria.Notifications
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
        ///     Gibt die ID des Authors zurück.
        /// </summary>
        [JsonProperty("uid")]
        public int AuthorId { get; set; }

        /// <summary>
        ///     Gibt den Benutzernamen des Authors zurück.
        /// </summary>
        [JsonProperty("uname")]
        public string AuthorName { get; set; }

        /// <summary>
        ///     Gibt die ID der Kategorie, in der sich die News befindet, zurück.
        /// </summary>
        [JsonProperty("catid")]
        public int CategoryId { get; set; }

        /// <summary>
        ///     Gibt den Namen der Kategorie, in der sich die News befindet, zurück.
        /// </summary>
        [JsonProperty("catname")]
        public string CategoryName { get; set; }

        /// <summary>
        ///     Gibt eine Kurzbeschreibung der News zurück.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///     Gibt die Aufrufe der News zurück.
        /// </summary>
        [JsonProperty("hits")]
        public int Hits { get; set; }

        /// <summary>
        ///     Gibt die Bild-ID der News zurück.
        /// </summary>
        [JsonProperty("image_id")]
        public string ImageId { get; set; }

        /// <summary>
        ///     Gibt Angaben zum CSS-Style des Bildes zurück.
        /// </summary>
        [JsonProperty("image_style")]
        public string ImageStyle { get; set; }

        /// <summary>
        ///     Gibt die News-ID zurück.
        /// </summary>
        [JsonProperty("nid")]
        public int NewsId { get; set; }

        /// <summary>
        ///     Gibt die Anzahl der Kommentare zurück.
        /// </summary>
        [JsonProperty("posts")]
        public int Posts { get; set; }

        /// <summary>
        ///     Gibt die Überschrift der News zurück.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        ///     Gibt die Thread-ID zurück.
        /// </summary>
        [JsonProperty("thread")]
        public int Thread { get; set; }

        /// <summary>
        ///     Gibt den Zeitpunkt der News als Unix Timestamp zurück.
        /// </summary>
        [JsonProperty("time")]
        public long Time { get; set; }

        /// <summary>
        ///     Veraltet. Gibt die Thread-ID zurück.
        /// </summary>
        [JsonProperty("mid"), Obsolete("Bitte benutzte stattdessen " + nameof(Thread))]
        internal int Mid { get; set; }

        /// <summary>
        ///     Veraltet.
        /// </summary>
        [JsonProperty("pid"), Obsolete]
        internal int Pid { get; set; }

        #endregion

        #region

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.Subject + "\n" + Utility.UnixTimeStampToDateTime(this.Time) + "\n" + this.CategoryName;
        }

        #endregion
    }
}