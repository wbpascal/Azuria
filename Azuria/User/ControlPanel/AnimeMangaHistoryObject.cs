using System;
using Azuria.AnimeManga;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    /// </summary>
    public class AnimeMangaHistoryObject<T> where T : IAnimeMangaObject
    {
        internal AnimeMangaHistoryObject(IAnimeMangaContent<T> contentObject, DateTime timeStamp,
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