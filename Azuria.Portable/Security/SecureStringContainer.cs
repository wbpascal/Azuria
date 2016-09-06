namespace Azuria.Security
{
    /// <summary>
    /// </summary>
    public class SecureStringContainer : ISecureContainer<char[]>
    {
        private char[] _secureString;

        internal SecureStringContainer()
        {
            this._secureString = new char[0];
        }

        #region Methods

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            for (int i = 0; i < this._secureString.Length; i++)
                this._secureString[i] = (char) 0;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public char[] ReadValue()
        {
            return this._secureString;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(char[] value)
        {
            this._secureString = value;
        }

        #endregion
    }
}