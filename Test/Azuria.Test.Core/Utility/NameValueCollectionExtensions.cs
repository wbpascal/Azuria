using System;
using System.Collections.Specialized;

namespace Azuria.Test.Core.Utility
{
    public static class NameValueCollectionExtensions
    {
        #region Methods

        public static bool All(this NameValueCollection collection, Func<string, string, bool> predicate)
        {
            foreach (string key in collection)
                if (!predicate.Invoke(key, collection[key])) return false;
            return true;
        }

        #endregion
    }
}