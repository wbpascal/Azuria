namespace Azuria.User
{
    /// <summary>
    /// </summary>
    public class UserPoints
    {
        internal UserPoints(int anime, int manga, int info, int uploads, int forum, int misc)
        {
            this.Anime = anime;
            this.Manga = manga;
            this.Info = info;
            this.Uploads = uploads;
            this.Forum = forum;
            this.Misc = misc;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public int Anime { get; set; }

        /// <summary>
        /// </summary>
        public int Forum { get; set; }

        /// <summary>
        /// </summary>
        public int Info { get; set; }

        /// <summary>
        /// </summary>
        public int Manga { get; set; }

        /// <summary>
        /// </summary>
        public int Misc { get; set; }

        /// <summary>
        /// </summary>
        public int Uploads { get; set; }

        #endregion
    }
}