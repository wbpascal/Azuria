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
    public class NewsNotification : INotification
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCount"></param>
        public NewsNotification(int updateCount)
        {
            this.Count = updateCount;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] getUpdates()
        {
            throw new NotImplementedException();
        }
    }
}
