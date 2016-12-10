using System.Threading.Tasks;
using Azuria.ErrorHandling;
using Azuria.Media;

namespace Azuria.UserInfo.ControlPanel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ToptenObject<T> : IToptenObject where T : IMediaObject
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

        /// <inheritdoc />
        IMediaObject IToptenObject.MediaObject => this.MediaObject;

        /// <inheritdoc />
        public int ToptenId { get; }

        /// <inheritdoc />
        public UserControlPanel UserControlPanel { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public Task<IProxerResult> Delete()
        {
            return this.UserControlPanel.DeleteTopten(this.ToptenId);
        }

        #endregion
    }
}