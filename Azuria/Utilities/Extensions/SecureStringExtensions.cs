using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Azuria.Utilities.Extensions
{
    internal static class SecureStringExtensions
    {
        #region Methods

        internal static char[] ToCharArray(this SecureString secureString)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(valuePtr)?.ToCharArray() ?? new char[0];
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        #endregion
    }
}