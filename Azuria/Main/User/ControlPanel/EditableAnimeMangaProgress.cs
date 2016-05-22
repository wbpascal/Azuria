using System;
using System.Threading.Tasks;
using Azuria.Utilities.ErrorHandling;
using Azuria.Utilities.Properties;
using JetBrains.Annotations;

namespace Azuria.Main.User.ControlPanel
{
    /// <summary>
    ///     Represents an editable progress for an <see cref="Anime" /> or <see cref="Manga" />.
    /// </summary>
    public class EditableAnimeMangaProgress : AnimeMangaProgress
    {
        internal EditableAnimeMangaProgress(int currentProgress, int maxProgress,
            Func<int, Task<ProxerResult>> setProgressFunc) : base(currentProgress, maxProgress)
        {
            this.CurrentProgress = new AsyncProperty<int>(currentProgress, setFunc:
                i =>
                    i > this.MaxProgress
                        ? new Task<ProxerResult>(() => new ProxerResult(new[] {new IndexOutOfRangeException()}))
                        : setProgressFunc.Invoke(i));
        }

        internal EditableAnimeMangaProgress(int currentProgress, int maxProgress,
            Func<Task<ProxerResult<int>>> getProgressFunc,
            Func<int, Task<ProxerResult>> setProgressFunc) : base(currentProgress, maxProgress)
        {
            this.CurrentProgress = new AsyncProperty<int>(currentProgress, getProgressFunc, i =>
                i > this.MaxProgress
                    ? new Task<ProxerResult>(() => new ProxerResult(new[] {new IndexOutOfRangeException()}))
                    : setProgressFunc.Invoke(i));
        }

        #region Properties

        /// <summary>
        ///     Gets an object with the help of which the current progress can be set or retrieved.
        /// </summary>
        [NotNull]
        public new AsyncProperty<int> CurrentProgress { get; }

        #endregion
    }
}