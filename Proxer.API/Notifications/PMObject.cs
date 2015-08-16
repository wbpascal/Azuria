using Proxer.API.Utilities;
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
    public class PMObject : INotificationObject
    {
        /// <summary>
        /// 
        /// </summary>
        public enum PMType
        {
            /// <summary>
            /// 
            /// </summary>
            Konferenz,
            /// <summary>
            /// 
            /// </summary>
            Benutzer
        }

        /// <summary>
        /// (vorläufig?) Benötigt als Dummy
        /// </summary>
        internal PMObject(Object dummy)
        {
            this.Type = NotificationObjectType.Dummy;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conID">ID der Konferenz</param>
        /// <param name="userName">Benutzername des Senders</param>
        /// <param name="timeStampDate">Datum(ohne Uhrzeit) der Nachricht</param>
        internal PMObject(int conID, string userName, DateTime timeStampDate)
        {
            this.Type = NotificationObjectType.PrivateMessage;
            this.MessageTyp = PMType.Benutzer;
            this.TimeStamp = timeStampDate;
            this.ID = conID;
            this.UserName = userName;
        }
        /// <summary>
        /// Konstruktor für PM-Konferenzen
        /// </summary>
        /// <param name="conID">ID der Konferenz</param>
        /// <param name="title">Titel der Konferenz</param>
        /// <param name="timeStampDate">Datum(ohne Uhrzeit) der Nachricht</param>
        internal PMObject(string title, int conID, DateTime timeStampDate)
        {
            this.Type = NotificationObjectType.PrivateMessage;
            this.MessageTyp = PMType.Konferenz;
            this.ConferenceTitle = title;
            this.TimeStamp = timeStampDate;
            this.ID = conID;
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
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 
        /// </summary>
        public PMType MessageTyp { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConferenceTitle { get; private set; }
        /// <summary>
        /// Gibt nur Datum zurück, keine Uhrzeit.
        /// </summary>
        public DateTime TimeStamp { get; private set; }
        /// <summary>
        /// Gibt die ID der Konferenz zurück.
        /// </summary>
        public int ID { get; private set; }
    }
}
