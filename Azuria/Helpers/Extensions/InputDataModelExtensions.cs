using System.Collections.Generic;
using Azuria.Api.v1.Input;

namespace Azuria.Helpers.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="InputDataModel"/>.
    /// </summary>
    public static class InputDataModelExtensions
    {
        /// <summary>
        /// Builds the request parameters and converts the to a dictionary. If multiple parameters have the same key, 
        /// only the value of the last one will be present in the dictionary.
        /// </summary>
        /// <param name="input">The <see cref="InputDataModel"/> instance.</param>
        /// <returns>A dictionary of the request parameters.</returns>
        public static IDictionary<string, string> BuildDictionary(this InputDataModel input)
        {
            IDictionary<string, string> lReturn = new Dictionary<string, string>();
            lReturn.AddOrUpdateRange(input.Build());
            return lReturn;
        }
    }
}