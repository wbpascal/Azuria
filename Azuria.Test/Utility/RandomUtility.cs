using System;
using System.Security.Cryptography;

namespace Azuria.Test.Utility
{
    internal class RandomUtility
    {
        #region

        public static string GetRandomHexString()
        {
            byte[] lRandomBytes = new byte[8];
            new Random().NextBytes(lRandomBytes);
            return new RSACryptoServiceProvider().Encrypt(lRandomBytes, true).ToHexString();
        }

        #endregion
    }
}