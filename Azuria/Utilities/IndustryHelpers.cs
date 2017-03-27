using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azuria.Info;

namespace Azuria.Utilities
{
    internal static class IndustryHelpers
    {
        public static string ToTypeString(this IndustryType? type)
        {
            switch (type)
            {
                case IndustryType.RecordLabel:
                    return "record_label";
                case IndustryType.TalentAgent:
                    return "talent_agent";
                case null:
                    return string.Empty;
                default:
                    return type.ToString().ToLowerInvariant();
            }
        }
    }
}
