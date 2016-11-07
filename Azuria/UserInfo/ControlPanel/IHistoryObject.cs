using System;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// </summary>
    public interface IHistoryObject
    {
        #region Properties

        /// <summary>
        /// </summary>
        IMediaContent ContentObject { get; }

        /// <summary>
        /// </summary>
        DateTime TimeStamp { get; }

        /// <summary>
        /// </summary>
        UserControlPanel UserControlPanel { get; }

        #endregion
    }
}