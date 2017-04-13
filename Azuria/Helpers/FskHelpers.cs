using System.Collections.Generic;
using Azuria.Enums.Info;

namespace Azuria.Helpers
{
    internal static class FskHelpers
    {
        #region Properties

        internal static Dictionary<string, Fsk> StringToFskDictionary { get; }
            = EnumHelpers.GetDescriptionDictionary<Fsk>();

        #endregion
    }
}