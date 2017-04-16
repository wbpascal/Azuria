using System;
using Azuria.Enums.Info;

namespace Azuria.Helpers
{
    internal static class StreamHosterHelpers
    {
        #region Methods

        public static StreamHoster GetFromString(string hosterString)
        {
            switch (hosterString.ToLowerInvariant())
            {
                case "clipfish-extern":
                    return StreamHoster.Clipfish;
                case "crunchyroll_de":
                case "crunchyroll_en":
                    return StreamHoster.Crunchyroll;
                case "novamov":
                    return StreamHoster.Auroravid;
                case "proxer-stream":
                    return StreamHoster.ProxerStream;
                case "streamcloud2":
                    return StreamHoster.Streamcloud;
                default:
                    return (StreamHoster) Enum.Parse(typeof(StreamHoster), hosterString, true);
            }
        }

        #endregion
    }
}