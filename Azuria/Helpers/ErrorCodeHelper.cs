using System;
using Azuria.Enums;

namespace Azuria.Helpers
{
    internal static class ErrorCodeHelper
    {
        public static ErrorCode GetErrorCodeFromInt(int errorCode)
        {
            if (!Enum.IsDefined(typeof(ErrorCode), errorCode))
                return ErrorCode.Unknown;
            return (ErrorCode) errorCode;
        }
    }
}
