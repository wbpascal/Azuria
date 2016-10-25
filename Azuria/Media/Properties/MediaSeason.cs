using Azuria.Api.v1.DataModels.Info;

namespace Azuria.Media.Properties
{
    /// <summary>
    /// </summary>
    public class MediaSeason
    {
        internal MediaSeason(SeasonDataModel dataModel)
        {
            this.Season = dataModel.Season;
            this.Year = dataModel.Year;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public Season Season { get; }

        /// <summary>
        /// </summary>
        public int Year { get; }

        #endregion
    }
}