using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// </summary>
    public interface IToptenObject
    {
        #region Properties

        /// <summary>
        /// </summary>
        IMediaObject MediaObject { get; }

        /// <summary>
        /// </summary>
        int ToptenId { get; }

        /// <summary>
        /// </summary>
        UserControlPanel UserControlPanel { get; }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task<IProxerResult> Delete();

        #endregion
    }
}