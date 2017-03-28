using System;
using System.Collections.Generic;
using System.Text;

namespace Azuria.Api.Helpers
{
    internal static class DateTimeHelpers
    {
        internal static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            if (unixTimeStamp < 0) return DateTime.MinValue;
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
