using System;
using System.Net;

namespace Azuria.Test.Utility
{
    public static class ClassExtensions
    {
        #region

        public static bool ContainsCookie(this CookieCollection collection, string name, string value)
        {
            foreach (Cookie cookie in collection)
            {
                if (cookie.Name.Equals(name) && cookie.Value.Equals(value)) return true;
            }

            return false;
        }

        public static string ToHexString(this byte[] byteArray)
        {
            string lHex = BitConverter.ToString(byteArray);
            return lHex.Replace("-", "").ToLower();
        }

        #endregion
    }
}