using Azuria.Api.v1.DataModels.Info;

namespace Azuria.AnimeManga
{
    /// <summary>
    /// </summary>
    public class AnimeMangaSeason
    {
        internal AnimeMangaSeason(SeasonDataModel dataModel)
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