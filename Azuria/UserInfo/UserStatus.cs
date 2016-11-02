using System;

namespace Azuria.UserInfo
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
        public DateTime LastChanged { get; }

        /// <summary>
        /// </summary>
        public string Status { get; }

        #endregion
    }
}