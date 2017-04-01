using System.Threading.Tasks;
using Azuria.ErrorHandling;

namespace Azuria.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILoginManager
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        char[] LoginToken { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IProxerResult> PerformLogin(string username, string password, string secretKey = null);

        /// <summary>
        /// 
        /// </summary>
        void QueueLoginForNextRequest();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>If the login token should be send with the next request.</returns>
        bool SendTokenWithNextRequest();

        #endregion
    }
}