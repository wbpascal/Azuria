using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ToptenObject<T> where T : IMediaObject
    {
        internal ToptenObject(int toptenId, T mediaObject, UserControlPanel userControlPanel)
        {
            this.MediaObject = mediaObject;
            this.UserControlPanel = userControlPanel;
            this.ToptenId = toptenId;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public T MediaObject { get; }

        /// <summary>
        /// </summary>
        public int ToptenId { get; }

        /// <summary>
        /// </summary>
        public UserControlPanel UserControlPanel { get; }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public Task<IProxerResult> DeleteTopten()
        {
            return this.UserControlPanel.DeleteTopten(this.ToptenId);
        }

        #endregion
    }
}