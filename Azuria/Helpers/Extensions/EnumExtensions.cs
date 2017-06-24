using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Azuria.Enums.Info;
using Azuria.Enums.Media;

#pragma warning disable 1591
namespace Azuria.Helpers.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumValue) where T : struct
        {
            Type lType = enumValue.GetType();
            if (!(enumValue is Enum))
                throw new ArgumentException("The value must be member of an enum", nameof(enumValue));

            MemberInfo lMemberInfo = lType.GetTypeInfo()
                .DeclaredMembers.FirstOrDefault(info => info.Name == enumValue.ToString());

            Attribute[] lAttributes =
                lMemberInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false).ToArray();
            if (lAttributes?.Any() ?? false)
                return ((DescriptionAttribute) lAttributes[0]).Description;

            throw new InvalidOperationException("Description Attribute not found!");
        }

        public static string ToShortString(this Country country)
        {
            switch (country)
            {
                case Country.Germany:
                    return "de";
                case Country.England:
                    return "en";
                case Country.UnitedStates:
                    return "us";
                case Country.Japan:
                    return "jp";
                case Country.Misc:
                    return "misc";
                default:
                    throw new InvalidOperationException("This Country cannot be converted to a short string!");
            }
        }

        public static string ToShortString(this Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "en";
                case Language.German:
                    return "de";
                default:
                    return string.Empty;
            }
        }

        public static string ToTypeString(this UserList list)
        {
            switch (list)
            {
                case UserList.Favourites:
                    return "favor";
                case UserList.Finished:
                    return "finish";
                default:
                    return list.ToString().ToLowerInvariant();
            }
        }

        public static string ToTypeString(this IndustryType type)
        {
            switch (type)
            {
                case IndustryType.RecordLabel:
                    return "record_label";
                case IndustryType.TalentAgent:
                    return "talent_agent";
                default:
                    return type.ToString().ToLowerInvariant();
            }
        }

        public static string ToTypeString(this HeaderStyle style)
        {
            switch (style)
            {
                case HeaderStyle.OldBlue:
                    return "old_blue";
                default:
                    return style.ToString().ToLowerInvariant();
            }
        }

        public static string ToTypeString(this MediaLanguage language)
        {
            switch (language)
            {
                case MediaLanguage.German:
                    return "de";
                case MediaLanguage.English:
                    return "en";
                case MediaLanguage.Unkown:
                    throw new InvalidOperationException();
                default:
                    return language.ToString().ToLowerInvariant();
            }
        }
    }
}