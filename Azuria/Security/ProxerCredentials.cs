namespace Azuria.Security
{
    /// <summary>
    /// </summary>
    public class ProxerCredentials : IProxerCredentials
    {
        private readonly SecureStringContainer _passwordContainer = new SecureStringContainer();

        /// <summary>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public ProxerCredentials(string username = null, char[] password = null)
        {
            this.Username = username ?? string.Empty;
            this.Password = password ?? new char[0];
        }

        #region Properties

        /// <inheritdoc />
        public char[] Password
        {
            get { return this._passwordContainer.ReadValue(); }
            set { this._passwordContainer.SetValue(value); }
        }

        /// <inheritdoc />
        public string Username { get; set; }

        #endregion
    }
}