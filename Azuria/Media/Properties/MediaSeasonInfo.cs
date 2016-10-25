using Azuria.Api.v1.DataModels.Info;

namespace Azuria.Media.Properties
{
    /// <summary>
    /// </summary>
    public class MediaSeasonInfo
    {
        internal MediaSeasonInfo(SeasonDataModel seasonDataModel)
        {
            this.StartSeason = new MediaSeason(seasonDataModel);
            this.EndSeason = this.StartSeason;
        }

        internal MediaSeasonInfo(SeasonDataModel startSeason, SeasonDataModel endSeason)
        {
            this.StartSeason = new MediaSeason(startSeason);
            this.EndSeason = new MediaSeason(endSeason);
        }

        #region Properties

        /// <summary>
        /// </summary>
        public MediaSeason EndSeason { get; }

        /// <summary>
        /// </summary>
        public MediaSeason StartSeason { get; }

        #endregion
    }
}