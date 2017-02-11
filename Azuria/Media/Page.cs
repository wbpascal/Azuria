using System;
using Azuria.Api.v1.DataModels.Manga;

namespace Azuria.Media
{
    /// <summary>
    /// </summary>
    public class Page
    {
        internal Page(PageDataModel dataModel, int serverId, int entryId, int chapterId)
        {
            this.Height = dataModel.PageHeight;
            this.Width = dataModel.PageWidth;
            this.Image =
                new Uri($"https://manga{serverId}.proxer.me/f/{entryId}/{chapterId}/{dataModel.ServerFileName}");
        }

        #region Properties

        /// <summary>
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// </summary>
        public Uri Image { get; }

        /// <summary>
        /// </summary>
        public int Width { get; }

        #endregion
    }
}