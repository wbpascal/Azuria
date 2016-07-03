namespace Azuria.AnimeManga
{
    /// <summary>
    /// </summary>
    public class AnimeMangaRating
    {
        internal AnimeMangaRating(double rating, int voters)
        {
            this.Rating = rating;
            this.Voters = voters;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// </summary>
        public int Voters { get; set; }

        #endregion
    }
}