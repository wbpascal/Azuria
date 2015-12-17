using System;
using Proxer.API.Community;

namespace Proxer.API.Notifications
{
    /// <summary>
    ///     Eine Klasse, die eine neue private Nachricht darstellt.
    /// </summary>
    public class PmObject : INotificationObject
    {
        /// <summary>
        ///     Eine Enumeration, die darstellt, ob die <see cref="PmObject">private Nachricht</see> von einem einzelnen
        ///     <see cref="User">Benutzer</see> stammt oder aus einer <see cref="Conference">Konferenz</see>.
        /// </summary>
        public enum PmType
        {
            /// <summary>
            ///     Die private Nachricht stammt aus einer <see cref="Conference">Konferenz</see>.
            /// </summary>
            Konferenz,

            /// <summary>
            ///     Die private Nachricht stammt von einem einzelnen <see cref="User">Benutzer</see>.
            /// </summary>
            Benutzer
        }


        internal PmObject(int conId, string userName, DateTime timeStampDate)
        {
            this.Type = NotificationObjectType.PrivateMessage;
            this.MessageTyp = PmType.Benutzer;
            this.TimeStamp = timeStampDate;
            this.Id = conId;
            this.UserName = userName;
        }

        internal PmObject(string title, int conId, DateTime timeStampDate)
        {
            this.Type = NotificationObjectType.PrivateMessage;
            this.MessageTyp = PmType.Konferenz;
            this.ConferenceTitle = title;
            this.TimeStamp = timeStampDate;
            this.Id = conId;
        }

        #region Geerbt

        /// <summary>
        ///     Gibt die Nachricht der Benachrichtigung als Text zurück.
        ///     <para>(Vererbt von <see cref="INotificationObject" />)</para>
        /// </summary>
        public string Message => this.ToString();

        /// <summary>
        ///     Gibt den Typ der Benachrichtigung zurück.
        ///     <para>(Vererbt von <see cref="INotificationObject" />)</para>
        /// </summary>
        public NotificationObjectType Type { get; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gibt den Titel der <see cref="Conference">Sender-Konferenz</see> zurück.
        ///     <para>(Ist nur vorhanden, wenn <see cref="MessageTyp" /> = <see cref="PmType.Konferenz" />)</para>
        /// </summary>
        public string ConferenceTitle { get; }

        /// <summary>
        ///     Gibt die ID der Konferenz zurück.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        ///     Gibt den Typ des Senders zurück.
        /// </summary>
        public PmType MessageTyp { get; }

        /// <summary>
        ///     Gibt das Empfangsdatum der Nachricht zurück.
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        ///     Gibt den Benutzernamen des Senders zurück.
        ///     <para>(Ist nur vorhanden, wenn <see cref="MessageTyp" /> = <see cref="PmType.Benutzer" />)</para>
        /// </summary>
        public string UserName { get; }

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
            return (this.MessageTyp == PmType.Konferenz ? this.ConferenceTitle : this.UserName) + "\n" + this.TimeStamp;
        }

        #endregion
    }
}