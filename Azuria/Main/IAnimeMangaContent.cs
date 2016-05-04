using System.Threading.Tasks;
using Azuria.Main.User.ControlPanel;
using Azuria.Utilities.ErrorHandling;
using JetBrains.Annotations;

namespace Azuria.Main
{
    /// <summary>
    /// </summary>
    public interface IAnimeMangaContent
    {
        #region

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ItemNotNull]
        Task<ProxerResult> AddToBookmarks(UserControlPanel userControlPanel = null);

        #endregion
    }
}