using System;

namespace Azuria.Security
{
    /// <summary>
    /// </summary>
    public interface ISecureContainer<T> : IDisposable
    {
        #region

        /// <summary>
        /// </summary>
        /// <returns></returns>
        T ReadValue();

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        void SetValue(T value);

        #endregion
    }
}