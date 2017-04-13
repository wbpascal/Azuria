using System.Collections.Generic;
using Azuria.Enums.Info;

namespace Azuria.Helpers
{
    internal static class GenreHelpers
    {
        #region Properties

        internal static Dictionary<string, Genre> StringToGenreDictionary { get; } =
            EnumHelpers.GetDescriptionDictionary<Genre>();

        #endregion
    }
}