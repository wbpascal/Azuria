using System;

namespace Azuria.Test.Core.Helpers
{
    public static class RandomHelper
    {
        public static string GetRandomString(int length)
        {
            //There are probably better approches to this instead of creating random GUIDs 
            // until the length is reached but it works for now
            string lReturn = Guid.NewGuid().ToString();
            while (lReturn.Length < length)
                lReturn += Guid.NewGuid().ToString();
            return lReturn.Substring(0, length);
        }
    }
}