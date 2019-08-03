using System;

namespace Azuria.Helpers
{
    /// <summary>
    /// </summary>
    public static class DateTimeHelpers
    {
        /// <summary>
        /// Converts a unix timestamp to the equivalent localized <see cref="DateTime"/>.
        /// </summary>
        /// <param name="unixTimeStamp">The unix timestamp to convert.</param>
        /// <returns>A localized <see cref="DateTime"/> object the represents the given unix timestamp.</returns>
        public static DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(unixTimeStamp)
                .ToLocalTime();
        }
    }
}