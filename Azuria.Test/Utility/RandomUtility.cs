using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Azuria.Test.Utility
{
    internal class RandomUtility
    {
        public static string GetRandomHexString()
        {
            byte[] lRandomBytes = new byte[8];
            new Random().NextBytes(lRandomBytes);
            return new RSACryptoServiceProvider().Encrypt(lRandomBytes, true).ToHexString();
        }
    }
}
