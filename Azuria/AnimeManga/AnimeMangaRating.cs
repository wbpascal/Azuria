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

        internal AnimeMangaRating(int totalStars, int voters)
        {
            this.Rating = voters != 0 ? totalStars/voters : 0;
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