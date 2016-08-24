using System.Threading.Tasks;
using Azuria.AnimeManga;
using Azuria.Utilities.ErrorHandling;

namespace Azuria.User.ControlPanel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ToptenObject<T> where T : IAnimeMangaObject
    {
        internal ToptenObject(int toptenId, T animeMangaObject, UserControlPanel userControlPanel)
        {
            this.AnimeMangaObject = animeMangaObject;
            this.UserControlPanel = userControlPanel;
            this.ToptenId = toptenId;
        }

        #region Properties

        /// <summary>
        /// </summary>
        public T AnimeMangaObject { get; }

        /// <summary>
        /// </summary>
        public int ToptenId { get; }

        /// <summary>
        /// </summary>
        public UserControlPanel UserControlPanel { get; }

        #endregion

        #region

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public Task<ProxerResult> DeleteEntry()
        {
            return this.UserControlPanel.DeleteTopten(this.ToptenId);
        }

        #endregion
    }
}