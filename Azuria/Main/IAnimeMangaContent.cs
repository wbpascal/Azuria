using System.Threading.Tasks;
using Azuria.Main.Minor;
using Azuria.Main.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Main
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAnimeMangaContent<T> : IAnimeMangaContentBase where T : IAnimeMangaObject
    {
        #region Properties

        /// <summary>
        /// </summary>
        Language GeneralLanguage { get; }

        /// <summary>
        /// </summary>
        [NotNull]
        T ParentObject { get; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        Task<ProxerResult<AnimeMangaBookmarkObject<T>>> AddToBookmarks(UserControlPanel userControlPanel = null);

        #endregion
    }
}