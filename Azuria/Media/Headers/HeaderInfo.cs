using System;
using Azuria.Api.v1.DataModels.Media;

namespace Azuria.Media.Headers
{
    /// <summary>
    /// </summary>
    public class HeaderInfo
    {
        /// <summary>
        /// </summary>
        public static HeaderInfo None = new HeaderInfo();

        internal HeaderInfo(HeaderDataModel dataModel)
        {
            this.HeaderId = dataModel.HeaderId;
            this.HeaderUrl = dataModel.HeaderUrl;
        }

        private HeaderInfo()
        {
            this.HeaderId = -1;
            this.HeaderUrl = null;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public int HeaderId { get; }

        /// <summary>
        /// </summary>
        public Uri HeaderUrl { get; }

        #endregion
    }
}