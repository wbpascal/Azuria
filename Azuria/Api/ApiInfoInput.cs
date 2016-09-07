using System;
using Azuria.Security;
using Azuria.Web;

namespace Azuria.Api
{
    /// <summary>
    /// </summary>
    public class ApiInfoInput
    {
        #region Properties

        /// <summary>
        /// </summary>
        public char[] ApiKeyV1 { get; set; }

        /// <summary>
        /// </summary>
        public IHttpClient CustomHttpClient { get; set; } = new HttpClient();

        /// <summary>
        /// </summary>
        public Func<ISecureContainer<char[]>> SecureContainerFactory { get; set; } = () => new SecureStringContainer();

        #endregion
    }
}