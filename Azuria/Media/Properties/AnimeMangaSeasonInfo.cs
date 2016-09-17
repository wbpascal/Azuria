using Azuria.Api.v1.DataModels.Info;

namespace Azuria.Media.Properties
{
    /// <summary>
    /// </summary>
    public class AnimeMangaSeasonInfo
    {
        internal AnimeMangaSeasonInfo(SeasonDataModel seasonDataModel)
        {
            this.StartSeason = new AnimeMangaSeason(seasonDataModel);
            this.EndSeason = this.StartSeason;
        }

        internal AnimeMangaSeasonInfo(SeasonDataModel startSeason, SeasonDataModel endSeason)
        {
            this.StartSeason = new AnimeMangaSeason(startSeason);
            this.EndSeason = new AnimeMangaSeason(endSeason);
        }

        #region Properties

        /// <summary>
        /// </summary>
        public AnimeMangaSeason EndSeason { get; }

        /// <summary>
        /// </summary>
        public AnimeMangaSeason StartSeason { get; }

        #endregion
    }
}