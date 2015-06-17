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
    public class PMObject : INotificationObject
    {
        /// <summary>
        /// 
        /// </summary>
        public enum PMTyp
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
        /// 
        /// </summary>
        /// <param name="conID">ID der Konferenz</param>
        /// <param name="userName">Benutzername des Senders</param>
        /// <param name="timeStampDate">Datum(ohne Uhrzeit) der Nachricht</param>
        public PMObject(int conID, string userName, DateTime timeStampDate)
        {
            this.Typ = NotificationObjectType.PrivateMessage;
            this.MessageTyp = PMTyp.Benutzer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conID"></param>
        /// <param name="title"></param>
        /// <param name="timeStampDate"></param>
        public PMObject (string title, int conID, DateTime timeStampDate)
        {
            this.Typ = NotificationObjectType.PrivateMessage;
            this.MessageTyp = PMTyp.Konferenz;
        }

        /// <summary>
        /// 
        /// </summary>
        public NotificationObjectType Typ { get; private set; }
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
        public PMTyp MessageTyp { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string User { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConferenceTitle { get; private set; }
        /// <summary>
        /// Gibt nur Datum zurück, keine Uhrzeit.
        /// </summary>
        public DateTime TimeStampDate { get; private set; }
        /// <summary>
        /// Gibt das Datum mit der Uhrzeit zurück. Nur benutzten, wenn unbedingt benötigt.
        /// </summary>
        public DateTime TimeStamp { get; private set; }
        /// <summary>
        /// Gibt die ID der Konferenz zurück.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void getAllInfo()
        {

        }
    }
}
