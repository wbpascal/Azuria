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
        /// 
        /// </summary>
        public int nid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Obsolete("Bitte benutzte stattdessen \"thread\"")]
        public int mid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Obsolete]
        public int pid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image_style { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int hits { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int thread { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string uname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int posts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int catid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string catname { get; set; }
    }
}
