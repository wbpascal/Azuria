using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxer.API
{
    /// <summary>
    /// 
    /// </summary>
    public class User
    {
        private bool checkStatus;
        private bool checkOnline;
        private bool checkRang;
        private bool checkPunkte;
        private bool checkBeschreibung;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userID"></param>
        public User(string name, int userID)
        {

        }

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
        public string Status { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Online { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Rang { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Punkte { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Beschreibung { get; private set; }
    }
}
