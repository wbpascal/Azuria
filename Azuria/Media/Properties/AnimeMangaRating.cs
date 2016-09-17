namespace Azuria.Media.Properties
{
    /// <summary>
    /// </summary>
    public class AnimeMangaRating
    {
        internal AnimeMangaRating(decimal rating, int voters)
        {
            this.Rating = rating;
            this.Voters = voters;
        }

        internal AnimeMangaRating(int totalStars, int voters)
        {
            this.Rating = voters != 0 ? totalStars/(decimal) voters : 0;
            this.Voters = voters;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public decimal Rating { get; set; }

        /// <summary>
        /// </summary>
        public int Voters { get; set; }

        #endregion
    }
}