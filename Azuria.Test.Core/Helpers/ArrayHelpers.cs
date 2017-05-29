using System;

namespace Azuria.Test.Core.Helpers
{
    public class ArrayHelpers
    {
        public static char[] GetRandomChars(int length)
        {
            char[] lArray = new char[length];
            for (int i = 0; i < length; i++)
                lArray[i] = (char) new Random().Next(128);
            return lArray;
        }
    }
}