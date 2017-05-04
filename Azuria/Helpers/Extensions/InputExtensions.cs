using System.Collections.Generic;
using System.Linq;
using Azuria.Enums.Info;
using Azuria.Enums.List;
using Azuria.Input;

namespace Azuria.Helpers.Extensions
{
    /// <summary>
    /// TODO: Doing this with extension methods is probably not needed. Maybe change it later and merge it with the input
    /// classes?
    /// </summary>
    internal static class InputExtensions
    {
        #region Methods

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
                {
                    "tagspoilerfilter",
                    input.FilterSpoilerTags != null
                        ? input.FilterSpoilerTags.Value
                              ? "spoiler_0"
                              : "spoiler_1"
                        : "spoiler_10"
                }
            };
            lReturn.AddIf("language", input.Language?.ToShortString(), (key, value) => !string.IsNullOrEmpty(value));
            lReturn.AddIf("genre", GenresToString(input.GenreInclude), (key, value) => !string.IsNullOrEmpty(value));
            lReturn.AddIf("nogenre", GenresToString(input.GenreExclude), (key, value) => !string.IsNullOrEmpty(value));
            lReturn.AddIf("fsk", FskToString(input.Fsk), (key, value) => !string.IsNullOrEmpty(value));
            lReturn.AddIf("length", input.Length.ToString(), (key, value) => input.Length != null);
            lReturn.AddIf("tags", input.TagsInclude.ToString(' '), (key, value) => !string.IsNullOrEmpty(value));
            lReturn.AddIf("notags", input.TagsExclude.ToString(' '), (key, value) => !string.IsNullOrEmpty(value));

            return lReturn;
        }

        internal static Dictionary<string, string> Build(this EntryListInput input)
        {
            if (input == null) return new Dictionary<string, string>();
            Dictionary<string, string> lReturn = new Dictionary<string, string>
            {
                {"kat", input.Category.ToString().ToLowerInvariant()},
                {"isH", input.ShowHContent.ToString()},
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
            return fskTypes?.Aggregate(string.Empty, (s, fsk) => s + fsk.GetDescription()) ?? string.Empty;
        }

        private static string GenresToString(IEnumerable<Genre> genres)
        {
            return genres?.Aggregate(string.Empty, (s, genre) => s + genre.GetDescription()) ?? string.Empty;
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

        #endregion
    }
}