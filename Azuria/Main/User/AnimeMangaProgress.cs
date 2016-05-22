namespace Azuria.Main.User
{
    /// <summary>
    ///     Represents the progress a <see cref="Azuria.User" /> of an <see cref="Anime" /> or <see cref="Manga" />.
    /// </summary>
    public class AnimeMangaProgress
    {
        /// <summary>
        ///     Represents an error.
        /// </summary>
        public static AnimeMangaProgress Error = new AnimeMangaProgress(-1, -1);

        /// <summary>
        ///     Intialises a new instance of the <see cref="AnimeMangaProgress" /> class with a specified progress.
        /// </summary>
        /// <param name="currentProgress">The current progress of the user.</param>
        /// <param name="maxProgress">The maximum progress the user can achieve.</param>
        public AnimeMangaProgress(int currentProgress, int maxProgress)
        {
            this.CurrentProgress = currentProgress;
            this.MaxProgress = maxProgress;
        }

        #region Properties

        /// <summary>
        ///     Gets the current progress of the user.
        /// </summary>
        public int CurrentProgress { get; }

        /// <summary>
        ///     Gets the maximum progress the user can achieve.
        /// </summary>
        public int MaxProgress { get; }

        #endregion
    }
}