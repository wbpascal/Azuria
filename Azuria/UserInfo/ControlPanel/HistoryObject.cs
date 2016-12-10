using System;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// </summary>
    public class HistoryObject<T> : IHistoryObject where T : IMediaObject
    {
        internal HistoryObject(IMediaContent<T> contentObject, DateTime timeStamp,
            UserControlPanel userControlPanel)
        {
            this.ContentObject = contentObject;
            this.TimeStamp = timeStamp;
            this.UserControlPanel = userControlPanel;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public IMediaContent<T> ContentObject { get; }

        /// <inheritdoc />
        IMediaContent IHistoryObject.ContentObject => this.ContentObject;

        /// <inheritdoc />
        public DateTime TimeStamp { get; }

        /// <inheritdoc />
        public UserControlPanel UserControlPanel { get; }

        #endregion
    }
}