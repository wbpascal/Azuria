namespace Azuria.Security
{
    /// <summary>
    /// </summary>
    public interface IProxerCredentials
    {
        #region Properties

        /// <summary>
        /// </summary>
        char[] Password { get; }

        /// <summary>
        /// </summary>
        string Username { get; }

        #endregion
    }
}