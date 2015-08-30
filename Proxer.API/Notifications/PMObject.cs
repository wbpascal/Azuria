using System;

namespace Proxer.API.Notifications
{
    /// <summary>
    /// </summary>
    public class PmObject : INotificationObject
    {
        /// <summary>
        /// </summary>
        public enum PmType
        {
            /// <summary>
            /// </summary>
            Konferenz,

            /// <summary>
            /// </summary>
            Benutzer
        }

        /// <summary>
        /// </summary>
        /// <param name="conId">ID der Konferenz</param>
        /// <param name="userName">Benutzername des Senders</param>
        /// <param name="timeStampDate">Datum(ohne Uhrzeit) der Nachricht</param>
        internal PmObject(int conId, string userName, DateTime timeStampDate)
        {
            this.Type = NotificationObjectType.PrivateMessage;
            this.MessageTyp = PmType.Benutzer;
            this.TimeStamp = timeStampDate;
            this.Id = conId;
            this.UserName = userName;
        }

        /// <summary>
        ///     Konstruktor für PM-Konferenzen
        /// </summary>
        /// <param name="conId">ID der Konferenz</param>
        /// <param name="title">Titel der Konferenz</param>
        /// <param name="timeStampDate">Datum(ohne Uhrzeit) der Nachricht</param>
        internal PmObject(string title, int conId, DateTime timeStampDate)
        {
            this.Type = NotificationObjectType.PrivateMessage;
            this.MessageTyp = PmType.Konferenz;
            this.ConferenceTitle = title;
            this.TimeStamp = timeStampDate;
            this.Id = conId;
        }

        /// <summary>
        /// </summary>
        public string ConferenceTitle { get; private set; }

        /// <summary>
        ///     Gibt die ID der Konferenz zurück.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// </summary>
        public PmType MessageTyp { get; private set; }

        /// <summary>
        ///     Gibt nur Datum zurück, keine Uhrzeit.
        /// </summary>
        public DateTime TimeStamp { get; private set; }

        /// <summary>
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// </summary>
        public NotificationObjectType Type { get; private set; }

        /// <summary>
        /// </summary>
        public string Message
        {
            get { throw new NotImplementedException(); }
        }
    }
}