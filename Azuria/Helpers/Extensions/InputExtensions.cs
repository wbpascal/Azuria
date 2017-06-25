using System;
using System.Collections.Generic;
using System.Linq;
using Azuria.Api.v1.Input;
using Azuria.Enums;
using Azuria.Enums.Info;
using Azuria.Enums.List;

namespace Azuria.Helpers.Extensions
{
    /// <summary>
    /// TODO: Doing this with extension methods is probably not needed. Maybe change it later and merge it with the input
    /// classes?
    /// </summary>
    internal static class InputExtensions
    {
        internal static Dictionary<string, string> Build(this SearchInput input)
        {
            if (input == null) return new Dictionary<string, string>();
            Dictionary<string, string> lReturn = new Dictionary<string, string>
            {
                {"name", input.Name},
                {"type", TypeToString(input.Type)},
                {"sort", input.Sort.ToString().ToLowerInvariant()},
                {"length-limit", input.LengthLimit.ToString().ToLowerInvariant()},
                {"tagratefilter", input.FilterUnratedTags ? "rate_1" : "rate_10"},
                {"tagspoilerfilter", GetTagSpoilerFilterString(input.FilterSpoilerTags)}
            };
            lReturn.AddIfNotEmptyString("language", input.Language?.ToShortString());
            lReturn.AddIfNotEmptyString("genre", GenresToString(input.GenreInclude));
            lReturn.AddIfNotEmptyString("nogenre", GenresToString(input.GenreExclude));
            lReturn.AddIfNotEmptyString("fsk", FskToString(input.Fsk));
            lReturn.AddIf("length", input.Length.ToString(), (key, value) => input.Length != null);
            lReturn.AddIfNotEmptyString("tags", input.TagsInclude.ToString(' '));
            lReturn.AddIfNotEmptyString("notags", input.TagsExclude.ToString(' '));

            return lReturn;
        }

        internal static Dictionary<string, string> Build(this EntryListInput input)
        {
            if (input == null) return new Dictionary<string, string>();
            if (input.Medium == MediaMedium.None)
                throw new InvalidOperationException("MediaMedium.None is not allowed as a medium!");
            Dictionary<string, string> lReturn = new Dictionary<string, string>
            {
                {"kat", input.Category.ToString().ToLowerInvariant()},
                {"isH", input.ShowHContent.ToString().ToLowerInvariant()},
                {"start", input.StartWithNonAlphabeticalChar ? "nonAlpha" : input.StartWith},
                {"sort", input.SortBy.ToString().ToLowerInvariant()},
                {"sort_type", input.SortDirection.GetDescription()}
            };
            if (input.Medium != null)
                lReturn.Add("medium", input.Medium.ToString().ToLowerInvariant());

            return lReturn;
        }

        private static string FskToString(IEnumerable<Fsk> fskTypes)
        {
            return fskTypes?.Aggregate(string.Empty, (s, fsk) => string.Concat(s, fsk.GetDescription(), " ")).Trim() ??
                   string.Empty;
        }

        private static string GenresToString(IEnumerable<Genre> genres)
        {
            return genres?.Aggregate(
                       string.Empty, (s, genre) => string.Concat(s, genre.GetDescription(), " ")
                   ).Trim() ?? string.Empty;
        }

        private static string GetTagSpoilerFilterString(bool? filtered)
        {
            switch (filtered)
            {
                case false:
                    return "spoiler_0";
                case true:
                    return "spoiler_1";
                default:
                    return "spoiler_10";
            }
        }

        private static string TypeToString(SearchMediaType type)
        {
            switch (type)
            {
                case SearchMediaType.AllAnime:
                    return "all-anime";
                case SearchMediaType.AllManga:
                    return "all-manga";
                default:
                    return type.ToString().ToLowerInvariant();
            }
        }
    }
}