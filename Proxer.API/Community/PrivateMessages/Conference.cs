using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API.Community.PrivateMessages
{
    /// <summary>
    /// 
    /// </summary>
    public class Conference
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="id"></param>
        public Conference(string title, int id)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Titel { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public List<User> Teilnehmer { get; private set; }
    }
}
