namespace Azuria.Api
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxerUser
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        bool IsProbablyLoggedIn { get; }

        /// <summary>
        /// 
        /// </summary>
        char[] LoginToken { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        void UsedCookies();

        #endregion
    }
}