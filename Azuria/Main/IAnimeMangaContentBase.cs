using Azuria.Utilities.Properties;

namespace Azuria.Main
{
    /// <summary>
    /// </summary>
    public interface IAnimeMangaContentBase
    {
        #region Properties

        /// <summary>
        /// </summary>
        int ContentIndex { get; }

        /// <summary>
        /// </summary>
        InitialisableProperty<bool> IsAvailable { get; }

        #endregion
    }
}