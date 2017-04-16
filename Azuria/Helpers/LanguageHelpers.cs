using Azuria.Enums.Info;

namespace Azuria.Helpers
{
    internal static class LanguageHelpers
    {
        #region Methods

        internal static MediaLanguage GetMediaLanguage(string lang)
        {
            switch (lang)
            {
                case "de":
                    return MediaLanguage.German;
                case "en":
                    return MediaLanguage.English;
                case "gersub":
                    return MediaLanguage.GerSub;
                case "gerdub":
                    return MediaLanguage.GerDub;
                case "engsub":
                    return MediaLanguage.EngSub;
                case "engdub":
                    return MediaLanguage.EngDub;
                default:
                    return MediaLanguage.Unkown;
            }
        }

        #endregion
    }
}