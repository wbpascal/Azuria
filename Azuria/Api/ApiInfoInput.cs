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
        public Func<Senpai, IHttpClient> CustomHttpClient { get; set; } = senpai => new HttpClient(senpai);

        /// <summary>
        /// </summary>
        public Func<ISecureContainer<char[]>> SecureContainerFactory { get; set; } = () => new SecureStringContainer();

        #endregion
    }
}