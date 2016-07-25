using System;

namespace Azuria.User
{
    /// <summary>
    /// </summary>
    public class UserStatus
    {
        internal UserStatus(string status, DateTime lastChanged)
        {
            this.LastChanged = lastChanged;
            this.Status = status;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public DateTime LastChanged { get; set; }

        /// <summary>
        /// </summary>
        public string Status { get; set; }

        #endregion
    }
}