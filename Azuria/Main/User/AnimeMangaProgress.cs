namespace Azuria.Main.User
{
    /// <summary>
    /// </summary>
    public class AnimeMangaProgress
    {
        /// <summary>
        /// </summary>
        /// <param name="currentProgress"></param>
        /// <param name="maxProgress"></param>
        public AnimeMangaProgress(int currentProgress, int maxProgress)
        {
            this.CurrentProgress = currentProgress;
            this.MaxProgress = maxProgress;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public int CurrentProgress { get; }

        /// <summary>
        /// </summary>
        public int MaxProgress { get; }

        #endregion
    }
}