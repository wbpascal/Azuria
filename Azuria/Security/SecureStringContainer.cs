using System.Linq;
using System.Security;
using Azuria.Utilities.Extensions;

namespace Azuria.Security
{
    /// <summary>
    /// </summary>
    public class SecureStringContainer : ISecureContainer<char[]>
    {
        private SecureString _secureString;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public SecureStringContainer()
        {
            this._secureString = new SecureString();
        }

        #region Methods

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this._secureString.Dispose();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public char[] ReadValue()
        {
            return this._secureString.ToCharArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(char[] value)
        {
            this._secureString = new SecureString();
            value.ToList().ForEach(c => this._secureString.AppendChar(c));
        }

        #endregion
    }
}