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
    public class UpdateNotification : INotification
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCount"></param>
        public UpdateNotification(int updateCount)
        {
            this.Count = updateCount;
        }

        public int Count {get; private set; }

        /// <summary>
        /// (Tut im Moment nichts)
        /// Gibt die Updates der Benachrichtigungen in einem Array zurück
        /// </summary>
        /// <returns>String-Array mit den Updates</returns>
        public string[] getUpdates()
        {
            throw new NotImplementedException();
        }
    }
}
