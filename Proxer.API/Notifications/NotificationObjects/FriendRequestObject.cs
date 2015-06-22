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
    public class FriendRequestObject : INotificationObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userID"></param>
        public FriendRequestObject(string userName, int userID)
        {
            this.Typ = NotificationObjectType.Friend;
            this.Message = userName;
            this.UserName = userName;
            this.ID = userID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userID"></param>
        /// <param name="userDescription"></param>
        /// <param name="requestDate"></param>
        /// <param name="userOnline"></param>
        public FriendRequestObject(string userName, int userID, string userDescription, DateTime requestDate, bool userOnline)
        {
            this.Typ = NotificationObjectType.Friend;
            this.Message = userName;
            this.UserName = userName;
            this.ID = userID;
            this.Description = userDescription;
            this.Date = requestDate;
            this.Online = userOnline;
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
        public string UserName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Online { get; private set; }
    }
}
