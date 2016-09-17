using System;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// </summary>
    public class HistoryObject<T> where T : IAnimeMangaObject
    {
        internal HistoryObject(IAnimeMangaContent<T> contentObject, DateTime timeStamp,
            UserControlPanel userControlPanel)
        {
            this.ContentObject = contentObject;
            this.TimeStamp = timeStamp;
            this.UserControlPanel = userControlPanel;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public IAnimeMangaContent<T> ContentObject { get; }

        /// <summary>
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        /// </summary>
        public UserControlPanel UserControlPanel { get; }

        #endregion
    }
}