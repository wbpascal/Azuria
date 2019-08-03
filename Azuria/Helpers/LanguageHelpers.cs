using Azuria.Enums.Info;

namespace Azuria.Helpers
{
    internal static class LanguageHelpers
    {
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

        internal static Language GetLanguageFromIdentifier(string identifier)
        {
            switch (identifier)
            {
                case "de":
                    return Language.German;
                case "en":
                    return Language.English;
                case "jp":
                    return Language.Japanese;
                case "kr":
                    return Language.Korean;
                case "zh":
                    return Language.Chinese;
                default:
                    return Language.Unkown;
            }
        }
    }
}